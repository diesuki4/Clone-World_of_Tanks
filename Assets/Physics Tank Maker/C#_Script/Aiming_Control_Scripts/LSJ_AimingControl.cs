using System.Collections;
using UnityEngine;
using System.Collections.Generic;

// 탱크 사격 및 조준
// [DefaultExecutionOrder(+1)] 
public class LSJ_AimingControl : MonoBehaviour
{
    /*
     * This script is attached to the "MainBody" of the tank.
     * This script controls the aiming of the tank.
     * "Turret_Horizontal_CS" and "Cannon_Vertical_CS" scripts rotate the turret and cannon referring to this variables.
    */


    public float OpenFire_Angle = 180.0f; // 발사각

    int inputType; // 입력값
    LSJ_TurretHorizontal[] turretHorizontalScripts;
    LSJ_CannonVertical[] cannonVerticalScripts;
    public bool Use_Auto_Turn; // Referred to from "Turret_Horizontal_CS" and "Cannon_Vertical_CS".
    public bool Use_Auto_Lead; // Referred to from "Turret_Horizontal_CS".
    public float Aiming_Blur_Multiplier = 1.0f; // Referred to from "Turret_Horizontal_CS".
    public bool reticleAimingFlag; // Controlled from "Aiming_Control_Input_##_###", and referred to from "UI_Aim_Marker_Control_CS".

    // For auto-turn.
    public int Mode; // Referred to from "UI_Aim_Marker_Control_CS". // 0 => Keep the initial positon, 1 => Free aiming,  2 => Locking on.
    Transform rootTransform;
    Rigidbody thisRigidbody;
    public Vector3 Target_Position; // Referred to from "Turret_Horizontal_CS", "Cannon_Vertical_CS", "UI_Aim_Marker_Control_CS", "ReticleWheel_Control_CS".
    public Transform Target_Transform; // Referred to from "UI_Aim_Marker_Control_CS", "UI_HP_Bars_Target_CS".
    Vector3 targetOffset;
    public Rigidbody Target_Rigidbody; // Referred to from "Turret_Horizontal_CS".
    public Vector3 Adjust_Angle; // Referred to from "Turret_Horizontal_CS" and "Cannon_Vertical_CS".
    const float spherecastRadius = 3.0f;
    LSJ_CameraRotation cameraRotationScript;
    public float Turret_Speed_Multiplier; // Referred to from "Turret_Horizontal_CS" and "Cannon_Vertical_CS".

    // For manual-turn.
    public float Turret_Turn_Rate; // Referred to from "Turret_Horizontal_CS".
    public float Cannon_Turn_Rate; // Referred to from "Cannon_Vertical_CS".


    protected LSJ_AimingControlInput00Base inputScript;

    public bool Is_Selected; // Referred to from "UI_HP_Bars_Target_CS".


    void Start()
    {
        Initialize();
    }

    // 초기 값 설정
    void Initialize()
    {
        rootTransform = transform.root; // 중심 Transform 값 (객체 전체)
        thisRigidbody = GetComponent<Rigidbody>(); // 리지드바디 선언
        Turret_Speed_Multiplier = 1.0f; //  터렛 회전 속도 배수

        // AI 탱크가 아니라면
        if (inputType != 10)
        {
            inputType = LSJ_GeneralSettings.Input_Type;
            Use_Auto_Lead = LSJ_GeneralSettings.Use_Auto_Lead;
        }

        // 터렛과 캐논과 관련한 컴포넌트 호출
        turretHorizontalScripts = GetComponentsInChildren<LSJ_TurretHorizontal>();
        cannonVerticalScripts = GetComponentsInChildren<LSJ_CannonVertical>();

        // 카메라 회전과 관련
        cameraRotationScript = transform.parent.GetComponentInChildren<LSJ_CameraRotation>();

        // 키 입력
        InputControl(inputType);

        // 입력 대기.
        if (inputScript != null)
        {
            inputScript.Prepare(this);
        }
    }


    protected virtual void InputControl(int type)
    {
        switch (type)
        {
            case 0: // Mouse + Keyboard (Stepwise)
            case 1: // Mouse + Keyboard (Pressing)
                inputScript = gameObject.AddComponent<LSJ_AimingControlInput01MouseKeyboard>();
                break;

            case 2: // GamePad (Single stick)
                    // inputScript = gameObject.AddComponent<Aiming_Control_Input_02_For_Single_Stick_Drive_CS>();
                break;

            case 3: // GamePad (Twin sticks)
                    // inputScript = gameObject.AddComponent<Aiming_Control_Input_03_For_Twin_Sticks_Drive_CS>();
                break;

            case 4: // GamePad (Triggers)
                    // inputScript = gameObject.AddComponent<Aiming_Control_Input_04_For_Triggers_Drive_CS>();
                break;

            case 10: // AI
                     // The order is sent from "AI_CS".
                break;
        }
    }


    void Update()
    {
        // 플레이어가 선택되지 않으면
        if (Is_Selected == false)
        {
            // 리턴한다
            return;
        }

        // 키입력이 되고 있다면
        if (inputScript != null)
        {
            // 호출한다
            inputScript.Get_Input();
        }
    }


    void FixedUpdate()
    {
        // 타겟 포지션이 감지된다면
        if (Target_Transform)
        {
            // 타겟 포지션을 최신화한다
            Update_Target_Position();
        }
        else
        {
            // 타겟 포지션을 속도와 시간 변화량에 따라 더해준다
            Target_Position += thisRigidbody.velocity * Time.fixedDeltaTime;
        }
    }

    // 타겟 포지션 업데이트
    void Update_Target_Position()
    {
        // 타겟의 생존 여부를 파악한다
        // 타겟 root의 태그가 "Finish"라면
        if (Target_Transform.root.tag == "Finish")
        {
            // 타겟은 파괴된 상태이다
            Target_Transform = null;
            Target_Rigidbody = null;
            return;
        }

        // 이동하는 타겟 포지션을 최신화한다
        Target_Position = Target_Transform.position + (Target_Transform.forward * targetOffset.z) + (Target_Transform.right * targetOffset.x) + (Target_Transform.up * targetOffset.y);
    }

    // 에이밍 모드 FSM
    public void Switch_Mode()
    {
        switch (Mode)
        {
            case 0:
                // 초기값 설정
                Target_Transform = null; // 타겟 Transform 값을 받지 않음
                Target_Rigidbody = null; // 타겟 Rigidbody 값을 받지 않음
                for (int i = 0; i < turretHorizontalScripts.Length; i++)
                {
                    // 터렛 트래킹 중지
                    turretHorizontalScripts[i].Stop_Tracking();
                }
                for (int i = 0; i < cannonVerticalScripts.Length; i++)
                {
                    // 캐논 트래킹 중지
                    cannonVerticalScripts[i].Stop_Tracking();
                }
                break;

            case 1:
                // 기본 에이밍
                Target_Transform = null; // 타겟 Transform 값을 받지 않음
                Target_Rigidbody = null; // 타겟 Rigidbody 값을 받지 않음
                for (int i = 0; i < turretHorizontalScripts.Length; i++)
                {
                    // 터렛 트래킹 시작
                    turretHorizontalScripts[i].Start_Tracking();
                }
                for (int i = 0; i < cannonVerticalScripts.Length; i++)
                {
                    // 캐논 트래킹 시작
                    cannonVerticalScripts[i].Start_Tracking();
                }
                break;

            case 2:
                // 락온 에이밍
                for (int i = 0; i < turretHorizontalScripts.Length; i++)
                {
                    // 터렛 트래킹 시작
                    turretHorizontalScripts[i].Start_Tracking();
                }
                for (int i = 0; i < cannonVerticalScripts.Length; i++)
                {
                    // 캐논 트래킹 시작
                    cannonVerticalScripts[i].Start_Tracking();
                }
                break;
        }
    }

    // 
    public void Cast_Ray_Lock(Vector3 screenPos)
    { // Called from "Aiming_Control_Input_##_###".

        // 카메라로부터 Ray를 쏴서 타겟을 찾는다
        var mainCamera = Camera.main;
        var ray = mainCamera.ScreenPointToRay(screenPos);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 2048.0f, LSJ_LayerSettings.Aiming_Layer_Mask))
        {
            // ray가 부딪힌 콜라이더 transform 값을 colliderTransform 변수에 저장한다 
            var colliderTransform = raycastHit.collider.transform;

            // ray에 부딪힌 객체가 내가 아니라면
            if (colliderTransform.root != rootTransform)
            {
                // raycastHit에 저장된 리지드바디 값이 있다면
                // ray에 맞은 Collider에 리지드바디가 없다면 부모 오브젝트의 리지드바디를 호출한다
                if (raycastHit.rigidbody)
                {
                    // ray가 부딪힌 값이 저장된 colliderTransform이 타겟이라면
                    Target_Transform = colliderTransform;

                    // offset 값 조정
                    targetOffset = Target_Transform.InverseTransformPoint(raycastHit.point);
                    if (Target_Transform.localScale != Vector3.one)
                    { // The hit collider should be an "Armor_Collider".
                        targetOffset.x *= Target_Transform.localScale.x;
                        targetOffset.y *= Target_Transform.localScale.y;
                        targetOffset.z *= Target_Transform.localScale.z;
                    }

                    // Store the rigidbody of the target.
                    Target_Rigidbody = raycastHit.rigidbody;
                }
                else
                { // The hit object does not have a rigidbody.

                    // Clear the target.
                    Target_Transform = null;
                    Target_Rigidbody = null;

                    // Store the hit point.
                    Target_Position = raycastHit.point;
                }

                // Switch the aiming mode.
                Mode = 2; // Lock on.
                Switch_Mode();
                return;
            }
            else
            { // The hit collider is a part of itself.

                // Clear the target.
                Target_Transform = null;
                Target_Rigidbody = null;

                // Switch the aiming mode.
                Mode = 0; // Keep the initial position.
                Switch_Mode();
                return;
            }
        }
        else
        { // The ray does not hit anythig.

            // Clear the target.
            Target_Transform = null;
            Target_Rigidbody = null;

            // Set the position through the camera.
            screenPos.z = 1024.0f;
            Target_Position = mainCamera.ScreenToWorldPoint(screenPos);

            // Switch the aiming mode.
            Mode = 2; // Lock on.
            Switch_Mode();
            return;
        }
    }


    public void Cast_Ray_Free(Vector3 screenPos)
    { // Called from "Aiming_Control_Input_##_###".

        // Find a target by casting a ray from the camera.
        var mainCamera = Camera.main;
        var ray = mainCamera.ScreenPointToRay(screenPos);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 2048.0f, LSJ_LayerSettings.Aiming_Layer_Mask))
        {
            var colliderTransform = raycastHit.collider.transform;

            // Check the hit collider is not a part of itself.
            if (colliderTransform.root != rootTransform)
            {
                // Check the hit object has a rigidbody.
                // (Note.) When the hit collider has no rigidbody, and its parent has a rigidbody, then the parent's rigidbody is set as 'RaycastHit.rigidbody'.
                if (raycastHit.rigidbody)
                {
                    // Check the hit object is not a destroyed tank.
                    if (colliderTransform.root.tag != "Finish")
                    {
                        // Set the hit collider as the target.
                        Target_Transform = colliderTransform;

                        // Set the offset from the pivot.
                        targetOffset = Target_Transform.InverseTransformPoint(raycastHit.point);
                        if (Target_Transform.localScale != Vector3.one)
                        { // The hit collider should be an "Armor_Collider".
                            targetOffset.x *= Target_Transform.localScale.x;
                            targetOffset.y *= Target_Transform.localScale.y;
                            targetOffset.z *= Target_Transform.localScale.z;
                        }

                        // Store the rigidbody of the target.
                        Target_Rigidbody = raycastHit.rigidbody;

                        // Set the hit point.
                        Target_Position = raycastHit.point;

                        return;
                    }
                }
                // The hit object does not have a rigidbody, or it is a destroyed tank.

                // Check the distance to the hit point is enough.
                if (Vector3.Distance(transform.position, raycastHit.point) > 16.0f)
                {
                    // Clear the target.
                    Target_Transform = null;
                    Target_Rigidbody = null;

                    // Set the hit point.
                    Target_Position = raycastHit.point;

                    return;
                }
                // The hit point is too close.

            } // The hit collider is a part of itself.

        } // The ray does not hit anythig.

        // Clear the target.
        Target_Transform = null;
        Target_Rigidbody = null;

        // Set the position through this tank.
        screenPos.z = 64.0f;
        Target_Position = mainCamera.ScreenToWorldPoint(screenPos);
    }


    public void Reticle_Aiming(Vector3 screenPos, int thisRelationship)
    { // Called from "Aiming_Control_Input_##_###".

        // Find a target by casting a sphere from the camera.
        var ray = Camera.main.ScreenPointToRay(screenPos);
        var raycastHits = Physics.SphereCastAll(ray, spherecastRadius, 2048.0f, LSJ_LayerSettings.Aiming_Layer_Mask);
        for (int i = 0; i < raycastHits.Length; i++)
        {
            Transform colliderTransform = raycastHits[i].collider.transform;

            // Check the hit collider is not a part of itself.
            if (colliderTransform.root == rootTransform)
            {
                continue;
            }

            // Check the hit object has a rigidbody.
            // (Note.) When the hit collider has no rigidbody, and its parent has a rigidbody, then the parent's rigidbody is set as 'RaycastHit.rigidbody'.
            var targetRigidbody = raycastHits[i].rigidbody;
            if (targetRigidbody == null)
            {
                continue;
            }

            // Check the target is a MainBody. 
            if (targetRigidbody.gameObject.layer != LSJ_LayerSettings.Body_Layer)
            {
                continue;
            }

            // Check the hit object is not a destroyed tank.
            if (colliderTransform.root.tag == "Finish")
            {
                continue;
            }

            // Check the relationship.
            var idScript = raycastHits[i].transform.GetComponentInParent<LSJ_IDSettings>();
            if (idScript && idScript.Relationship == thisRelationship)
            {
                continue;
            }

            // Check the obstacle.
            if (Physics.Linecast(ray.origin, raycastHits[i].point, out RaycastHit raycastHit, LSJ_LayerSettings.Aiming_Layer_Mask))
            {
                // Check the obstacle is not a part of itself.
                if (raycastHit.transform.root != rootTransform)
                {
                    continue;
                }
            }

            // Set the hit object as a new target.
            Target_Transform = raycastHits[i].transform;
            targetOffset = Vector3.zero;
            targetOffset.y = 0.5f;
            Target_Rigidbody = targetRigidbody;
            Target_Position = raycastHits[i].point;
            Adjust_Angle = Vector3.zero;
            return;

        } // New target cannot be found.
    }


    public void Auto_Lock(int direction, int thisRelationship)
    { // Called from "Aiming_Control_Input_##_###". (0 = Left, 1 = Right, 2 = Front)

        // Check the "AI_Headquaters_CS" is set in the scene.
        if (LSJ_AIHeadquaters.Instance == null)
        {
            return;
        }

        // Get the base angle to detect the new target.
        float baseAng;
        var mainCamera = Camera.main;
        if (direction != 2 && Target_Transform)
        {
            Vector3 currentLocalPos = mainCamera.transform.InverseTransformPoint(Target_Position);
            baseAng = Vector2.Angle(Vector2.up, new Vector2(currentLocalPos.x, currentLocalPos.z)) * Mathf.Sign(currentLocalPos.x);
        }
        else
        {
            baseAng = 0.0f;
        }

        // Get the tank list from the "AI_Headquaters_Helper_CS".
        List<LSJ_AIHeadquatersHelper> enemyTankList;
        if (thisRelationship == 0)
        {
            enemyTankList = LSJ_AIHeadquaters.Instance.Hostile_Tanks_List;
        }
        else
        {
            enemyTankList = LSJ_AIHeadquaters.Instance.Friendly_Tanks_List;
        }

        // Find a new target.
        int targetIndex = 0;
        int oppositeTargetIndex = 0;
        float minAng = 180.0f;
        float maxAng = 0.0f;
        for (int i = 0; i < enemyTankList.Count; i++)
        {
            if (enemyTankList[i].Body_Transform.root.tag == "Finish" || (Target_Transform && Target_Transform == enemyTankList[i].Body_Transform))
            { // The tank is dead, or the tank is the same as the current target.
                continue;
            }
            Vector3 localPos = mainCamera.transform.InverseTransformPoint(enemyTankList[i].Body_Transform.position);
            float tempAng = Vector2.Angle(Vector2.up, new Vector2(localPos.x, localPos.z)) * Mathf.Sign(localPos.x);
            float deltaAng = Mathf.Abs(Mathf.DeltaAngle(tempAng, baseAng));
            if ((direction == 0 && tempAng > baseAng) || (direction == 1 && tempAng < baseAng))
            { // On the opposite side.
                if (deltaAng > maxAng)
                {
                    oppositeTargetIndex = i;
                    maxAng = deltaAng;
                }
                continue;
            }
            if (deltaAng < minAng)
            {
                targetIndex = i;
                minAng = deltaAng;
            }
        }

        if (minAng != 180.0f)
        { // Target is found on the indicated side.
            Target_Transform = enemyTankList[targetIndex].Body_Transform;
            Target_Rigidbody = Target_Transform.GetComponent<Rigidbody>();
            targetOffset = Vector3.zero;
            targetOffset.y = 0.5f;
            Mode = 2; // Lock on.
            Switch_Mode();
        }
        else if (maxAng != 0.0f)
        { // Target is not found on the indicated side, but it could be found on the opposite side.
            Target_Transform = enemyTankList[oppositeTargetIndex].Body_Transform;
            Target_Rigidbody = Target_Transform.GetComponent<Rigidbody>();
            targetOffset = Vector3.zero;
            targetOffset.y = 0.5f;
            Mode = 2; // Lock on.
            Switch_Mode();
        }

        if (Target_Transform)
        {
            StartCoroutine("Send_Target_Position");
        }
    }


    IEnumerator Send_Target_Position()
    {
        // Send the target position to the "Camera_Rotation_CS" in the 'Camera_Pivot' object.
        if (cameraRotationScript)
        {
            yield return new WaitForFixedUpdate();
            cameraRotationScript.Look_At_Target(Target_Transform.position);
        }
    }


    public void AI_Lock_On(Transform tempTransform)
    { // Called from "AI_CS".
        Target_Transform = tempTransform;
        Target_Rigidbody = Target_Transform.GetComponent<Rigidbody>();
        Mode = 2;
        Switch_Mode();
    }


    public void AI_Lock_Off()
    { // Called from "AI_CS".
        Target_Transform = null;
        Target_Rigidbody = null;
        Mode = 0;
        Switch_Mode();
    }


    public void AI_Random_Offset()
    { // Called from "Cannon_Fire".

        // Set the new offset.
        Vector3 newOffset;
        newOffset.x = Random.Range(-0.5f, 0.5f);
        newOffset.y = Random.Range(0.0f, 1.0f);
        newOffset.z = Random.Range(-1.0f, 1.0f);
        targetOffset = newOffset;

        // Set the new aiming blur.
        Aiming_Blur_Multiplier = Random.Range(0.5f, 1.5f);
    }


    void Get_AI_CS(LSJ_AI aiScript)
    { // Called from "AI_CS".
        inputType = 10;
        Use_Auto_Turn = true;
        Use_Auto_Lead = true;
        Aiming_Blur_Multiplier = 1.0f;
        OpenFire_Angle = aiScript.Fire_Angle;
    }


    void Selected(bool isSelected)
    { // Called from "ID_Settings_CS".
        this.Is_Selected = isSelected;

        if (isSelected == false)
        {
            return;
        } // This tank is selected.

        // Send this reference to the "UI_HP_Bars_Target_CS" in the scene.
        if (LSJ_UIHPBarsTarget.Instance)
        {
            LSJ_UIHPBarsTarget.Instance.Get_Aiming_Script(this);
        }
    }


    void MainBody_Destroyed_Linkage()
    { // Called from "Damage_Control_Center_CS".
        Destroy(inputScript as Object);
        Destroy(this);
    }


    void Pause(bool isPaused)
    { // Called from "Game_Controller_CS".
        this.enabled = !isPaused;
    }

}
