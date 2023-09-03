using UnityEngine;
using System.Collections;

public class LSJ_DriveControl : MonoBehaviour
{

    /*
     * This script is attached to the "MainBody" of the tank.
     * This script controls the driving of the tank, such as speed, torque, acceleration and so on.
     * This script works in combination with "Drive_Wheel_Parent_CS" in the 'Create_##Wheels objects', and "Drive_Wheel_CS" in the drive wheels.
    */


    public float Torque = 2000.0f;
    public float Max_Speed = 8.0f;
    public float Turn_Brake_Drag = 150.0f;
    public float Switch_Direction_Lag = 0.5f;
    public bool Allow_Pivot_Turn;
    public float Pivot_Turn_Rate = 0.3f;

    public bool Acceleration_Flag = false;
    public float Acceleration_Time = 4.0f;
    public float Deceleration_Time = 0.1f;
    public AnimationCurve Acceleration_Curve;
    public bool Use_Downforce = false;
    public AnimationCurve Downforce_Curve;

    // Set by "inputType_Settings_CS".
    public int inputType = 0;

    // Set by "Drive_Control_Input_##_##_CS" scripts.
    public bool Stop_Flag = true; // Referred to from "Steer_Wheel_CS".
    public float L_Input_Rate;
    public float R_Input_Rate;
    public float Turn_Brake_Rate;
    public bool Pivot_Turn_Flag;
    public bool Apply_Brake;

    // Referred to from "Drive_Wheel_Parent_CS".
    public float Speed_Rate; // Referred to from also "inputType_Settings_CS".
    public float L_Brake_Drag;
    public float R_Brake_Drag;
    public float Left_Torque;
    public float Right_Torque;

    // Referred to from "Fix_Shaking_Rotation_CS".
    public bool Parking_Brake;

    // Referred to from "AI_CS", "AI_Hand_CS", "UI_Speed_Indicator_Control_CS".
    public float Current_Velocity;

    Transform thisTransform;
    Rigidbody thisRigidbody;
    float leftSpeedRate;
    float rightSpeedRate;
    float defaultTorque;
    float acceleRate;
    float deceleRate;
    int currentStep;
    bool switchDirectionTimerFlag;

    bool isSelected;

    protected LSJ_DriveControlInput00Base inputScript;


    void Start()
    {
        Initialize();
    }

    // 초기값 설정
    void Initialize()
    {
        thisTransform = transform; // Transform 변수값 선언
        thisRigidbody = GetComponent<Rigidbody>(); // 리지드바디 선언
        defaultTorque = Torque; // 기본 회전축 값 선언

        // 키 입력 감지
        if (inputType != 10)
        {
            inputType = LSJ_GeneralSettings.Input_Type;
        }

        // 가속도 값 선언
        if (Acceleration_Flag)
        {
            acceleRate = 1.0f / Acceleration_Time; // 가속
            deceleRate = 1.0f / Deceleration_Time; // 감속
        }

        // 자연스러운 감속 구간
        if (Use_Downforce && Downforce_Curve.keys.Length < 2)
        {
            Downforce_Curve = Create_Curve();
        }

        // 자연스러운 가속 구간
        if (Acceleration_Flag && Acceleration_Curve.keys.Length < 2)
        {
            Acceleration_Curve = Create_Curve();
        }


        InputControl(inputType);


        if (inputScript != null)
        {
            inputScript.Prepare(this);
        }
    }

    // 키 조작 FSM 상태 머신 설정
    void InputControl(int type)
    {
        switch (type)
        {
            case 0: // Mouse + Keyboard (Stepwise)
                    // inputScript = gameObject.AddComponent<Drive_Control_Input_01_Keyboard_Stepwise_CS>();
                break;

            case 1: // Mouse + Keyboard (Pressing)
                inputScript = gameObject.AddComponent<LSJ_DriveControlInput02KeyboardPressing>();
                break;

            case 2: // Gamepad (Single stick)
                    // inputScript = gameObject.AddComponent<Drive_Control_Input_03_Single_Stick_CS>();
                break;

            case 3: // Gamepad (Twin sticks)
                    // inputScript = gameObject.AddComponent<Drive_Control_Input_04_Twin_Sticks_CS>();
                break;

            case 4: // Gamepad (Triggers)
                    // inputScript = gameObject.AddComponent<Drive_Control_Input_05_Triggers_CS>();
                break;

            case 10: // AI
                     // inputScript = gameObject.AddComponent<Drive_Control_Input_99_AI_CS>();
                break;
        }
    }

    // 자연스러운 움직임 함수
    AnimationCurve Create_Curve()
    {
        Keyframe key1 = new Keyframe(0.0f, 0.0f, 1.0f, 1.0f);
        Keyframe key2 = new Keyframe(1.0f, 1.0f, 1.0f, 1.0f);
        return new AnimationCurve(key1, key2);
    }


    void Update()
    {
        // 플레이어가 선택되어 있다면
        if (isSelected)
        {
            // Drive_Input 함수 실행
            inputScript.Drive_Input();
        }

        // 이동과 관련된 변수 델타 함수 호출
        DrivingControl();
    }


    void FixedUpdate()
    {
        // 현재 속도는 리지드바디의 속도
        Current_Velocity = thisRigidbody.velocity.magnitude;
    }

    // 움직임 제어
    void DrivingControl()
    {
        // 가속 시
        if (Acceleration_Flag)
        {
            // 좌/우측 속도 계산
            leftSpeedRate = CalculateSpeedRate(leftSpeedRate, -L_Input_Rate);
            rightSpeedRate = CalculateSpeedRate(rightSpeedRate, R_Input_Rate);
        }
        else
        {
            // 기본 값 할당
            leftSpeedRate = -L_Input_Rate;
            rightSpeedRate = R_Input_Rate;
        }

        // 스피드 값
        // 좌우 스피드 절대값 중 더 큰 변수 사용
        Speed_Rate = Mathf.Max(Mathf.Abs(leftSpeedRate), Mathf.Abs(rightSpeedRate));
        // Speed_Rate 커브값을 받아온다
        Speed_Rate = Acceleration_Curve.Evaluate(Speed_Rate);

        // 피봇을 중심으로 회전
        if (Pivot_Turn_Flag)
        {
            // 스피드값 제한
            Speed_Rate = Mathf.Clamp(Speed_Rate, 0.0f, Pivot_Turn_Rate);
        }

        // 좌우 회전에 대한 제한 값.
        L_Brake_Drag = Mathf.Clamp(Turn_Brake_Drag * -Turn_Brake_Rate, 0.0f, Turn_Brake_Drag);
        R_Brake_Drag = Mathf.Clamp(Turn_Brake_Drag * Turn_Brake_Rate, 0.0f, Turn_Brake_Drag);

        // 회전 축
        Left_Torque = Torque * -Mathf.Sign(leftSpeedRate) * Mathf.Ceil(Mathf.Abs(leftSpeedRate));
        Right_Torque = Torque * Mathf.Sign(rightSpeedRate) * Mathf.Ceil(Mathf.Abs(rightSpeedRate));
    }

    // 스피드 값 계산
    float CalculateSpeedRate(float currentRate, float targetRate)
    {
        // 방향이 바뀌면
        if (switchDirectionTimerFlag)
        {
            // 스피드값 0 리턴
            return 0.0f;
        }
        // 현재값과 목표값이 같으면
        if (currentRate == targetRate)
        {
            // 현재 값을 반환
            return currentRate;
        }

        float moveRate; // 이동값
                        // 브레이크가 들어오면
        if (Apply_Brake)
        {
            // 감속 실행
            moveRate = deceleRate * 10.0f;
        }
        // 목표값이 0이 되면
        else if (targetRate == 0.0f)
        {
            // 이동 값을 감속값으로 치환
            moveRate = deceleRate;
        }
        else if (Mathf.Sign(targetRate) == Mathf.Sign(currentRate))
        {
            if (Mathf.Abs(targetRate) > Mathf.Abs(currentRate))
            {
                // 가속 전환
                moveRate = acceleRate;
            }
            else
            {
                // 감속 전환
                moveRate = deceleRate;
            }
        }
        else
        {

            // 브레이크 걸린 것처럼 빠르게 감속
            moveRate = deceleRate * 10.0f;

            // 방향 전환 시 탱크 정지
            // 만약 탱크가 AI가 아니라면
            if (inputType != 10)
            {
                var tempRate = Mathf.MoveTowards(currentRate, targetRate, moveRate * Time.deltaTime);
                if ((currentRate > 0.0f && tempRate <= 0.0f) || (currentRate <= 0.0f && tempRate > 0.0f))
                {
                    // 방향 전환
                    StartCoroutine("Switch_Direction_Timer");
                    return tempRate;
                }
            }
        }

        return Mathf.MoveTowards(currentRate, targetRate, moveRate * Time.deltaTime);
    }


    IEnumerator Switch_Direction_Timer()
    {
        switchDirectionTimerFlag = true;
        var count = 0.0f;
        while (count < Switch_Direction_Lag)
        {
            count += Time.deltaTime;
            yield return null;
        }
        switchDirectionTimerFlag = false;
    }
    void Call_Indicator()
    {
        // Call "UI_Speed_Indicator_Control_CS" in the scene.
        if (LSJ_UISpeedIndicatorControl.Instance)
        {
            bool isManual = (LSJ_GeneralSettings.Input_Type == 1);
            LSJ_UISpeedIndicatorControl.Instance.Get_Drive_Script(this, isManual, currentStep);
        }
    }


    public void Shift_Gear(int currentStep)
    { // Called from "Drive_Control_Input_01_Keyboard_Stepwise_CS".
        this.currentStep = currentStep;

        // Call "UI_Speed_Indicator_Control_CS" in the scene.
        if (LSJ_UISpeedIndicatorControl.Instance)
        {
            LSJ_UISpeedIndicatorControl.Instance.Get_Current_Step(currentStep);
        }
    }


    /*void Get_AI_CS()
    { // Called from "AI_CS".
        inputType = 10;
    }*/

    // 플레이어 선택 여부
    void Selected(bool isSelected)
    { // Called from "ID_Settings_CS".
        this.isSelected = isSelected;

        if (isSelected)
        {
            Call_Indicator();
        }
    }

    // 플레이어 파괴
    void MainBody_Destroyed_Linkage()
    { // Called from "Damage_Control_Center_CS".
        Destroy(inputScript as Object);
        Destroy(this);
    }

    // 멈춤
    void Pause(bool isPaused)
    { // Called from "Game_Controller_CS".
        this.enabled = !isPaused;
    }

}
