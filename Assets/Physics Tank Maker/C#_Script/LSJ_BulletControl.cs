using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LSJ_BulletControl : MonoBehaviour
{
    /*
     * This script is attached to bullet prefabs.
     * This script controls the posture of the bullet, and supports the collision detecting by casting a ray while flying.
     * When the bullet hits the target, this script sends the damage value to the "Damage_Control_##_##_CS" script in the hit collider.
     * The damage value is calculated considering the hit angle.
    */

    
    GameObject target;
    float damage;
    public int Type; // 입력값 형태
    public Transform This_Transform; // 트랜스폼 값
    public Rigidbody This_Rigidbody; // 리지드바디 값
    // AP_Middle
    public GameObject Impact_Object;
    public GameObject Ricochet_Object; // 도탄
    // HE_Bullet
    public GameObject Explosion_Object; // 폭발 오브젝트
    public float Explosion_Force; // 폭발 힘
    public float Explosion_Radius; // 폭발 반경


    // Set by "Bullet_Generator_CS".
    public float Attack_Point;
    public float Initial_Velocity;
    public float Life_Time;
    public float Attack_Multiplier = 1.0f;
    public bool Debug_Flag = false;

    bool isLiving = true; // 생존 여부


    void Start()
    {
        Initialize();
    }


    void Initialize()
    {
        if (This_Transform == null)
        {
            This_Transform = transform;
        }
        if (This_Rigidbody == null)
        {
            This_Rigidbody = GetComponent<Rigidbody>();
        }

        // CollsionDetectionMode을 동적연속 모드로 기본값 세통
        // 리지드바디가 없는 콜라이더에도 충돌 감지 수행
        This_Rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        Destroy(this.gameObject, Life_Time);
    }


    void Update()
    {
        // 포탄이 없다면
        if (isLiving == false) 
        {
            // 반환
            return;
        }

        // 월드 좌표 기준 기본 값 세팅
        This_Transform.LookAt(This_Rigidbody.position + This_Rigidbody.velocity);
    }

    void OnCollisionEnter(Collision collision)
    { 
        // 포탄이 있다면
        if (isLiving)
        {
            // hit process 실행
            switch (Type)
            {
                case 0: // AP_Middle 탄환 hit process
                    AP_Hit_Process(collision.collider.gameObject, collision.relativeVelocity.magnitude, collision.contacts[0].normal);
                    break;

                case 1: // HE_Hit 탄환 hit process
                    HE_Hit_Process();
                    break;
            }
        }
        target = GameObject.Find("Cromwell_ST/MainBody");

        if (collision.gameObject.tag == "Enemy")
        {

            CKB_HPManager ckbHPManager = collision.gameObject.GetComponent<CKB_HPManager>();
            
            if (ckbHPManager)
            {
                ckbHPManager.ApplyDamage(target, damage);
                LSJ_UIAnim.Instance.ShowLog(LSJ_UIAnim.PanelType.Left);
            }
        }

        Destroy(gameObject);
    }

 

    //AP_Middle 탄환의 Hit process
    void AP_Hit_Process(GameObject hitObject, float hitVelocity, Vector3 hitNormal)
    {
        isLiving = false;

        // CollsionDetectionMode를 일반충돌 모드로 설정
        This_Rigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;

        // hitObject가 없다면
        if (hitObject == null)
        {   
            // 반환
            return;
        }

        
        var damageScript = hitObject.GetComponent<LSJ_DamageControl00Base>();
        if (damageScript != null)
        { // The hit object has "Damage_Control_##_##_CS" script. >> It should be a breakable object.

            // Calculate the hit damage.
            var hitAngle = Mathf.Abs(90.0f - Vector3.Angle(This_Transform.forward, hitNormal));
            var damageValue = Attack_Point * Mathf.Pow(hitVelocity / Initial_Velocity, 2.0f) * Mathf.Lerp(0.0f, 1.0f, Mathf.Sqrt(hitAngle / 90.0f)) * Attack_Multiplier;

            // Output for debugging.
            if (Debug_Flag)
            {
                float tempMultiplier = 1.0f;
                LSJ_DamageControl09ArmorCollider armorColliderScript = hitObject.GetComponent<LSJ_DamageControl09ArmorCollider>();
                if (armorColliderScript)
                {
                    tempMultiplier = armorColliderScript.Damage_Multiplier;
                }
                Debug.Log("AP Damage " + damageValue * tempMultiplier + " on " + hitObject.name + " (" + (90.0f - hitAngle) + " degrees)");
            }

            // Send the damage value to "Damage_Control_##_##_CS" script.
            if (damageScript.Get_Damage(damageValue, Type) == true)
            { // The hit part has been destroyed.
              // Remove the bullet from the scene.
                Destroy(this.gameObject);
            }
            else
            { // The hit part has not been destroyed.
              // Create the ricochet object.
                if (Ricochet_Object)
                {
                    Instantiate(Ricochet_Object, This_Transform.position, Quaternion.identity, hitObject.transform);
                }
            }

        }
        else
        { // The hit object does not have "Damage_Control_##_##_CS" script. >> It should not be a breakable object.
          // Create the impact object.
            if (Impact_Object)
            {
                Instantiate(Impact_Object, This_Transform.position, Quaternion.identity);
            }
        }
    }


    void HE_Hit_Process()
    {
        isLiving = false;

        // Create the explosion effect object.
        if (Explosion_Object)
        {
            Instantiate(Explosion_Object, This_Transform.position, Quaternion.identity);
        }

        // Remove the useless components.
        Destroy(GetComponent<Renderer>());
        Destroy(GetComponent<Rigidbody>());
        Destroy(GetComponent<Collider>());

        // Add the explosion force to the objects within the explosion radius.
        var colliders = Physics.OverlapSphere(This_Transform.position, Explosion_Radius, LSJ_LayerSettings.Layer_Mask);
        for (int i = 0; i < colliders.Length; i++)
        {
            Add_Explosion_Force(colliders[i]);
        }

        Destroy(this.gameObject, 0.01f * Explosion_Radius);
    }


    void Add_Explosion_Force(Collider collider)
    {
        if (collider == null)
        {
            return;
        }

        Vector3 direction = (collider.transform.position - This_Transform.position).normalized;
        var ray = new Ray();
        ray.origin = This_Rigidbody.position;
        ray.direction = direction;
        if (Physics.Raycast(ray, out RaycastHit raycastHit, Explosion_Radius, LSJ_LayerSettings.Layer_Mask))
        {
            if (raycastHit.collider != collider)
            { // The collider should be behind an obstacle.
                return;
            }

            // Calculate the distance loss rate.
            var loss = Mathf.Pow((Explosion_Radius - raycastHit.distance) / Explosion_Radius, 2);

            // Add force to the rigidbody.
            Rigidbody rigidbody = collider.GetComponent<Rigidbody>();
            if (rigidbody)
            {
                rigidbody.AddForce(direction * Explosion_Force * loss);
            }

            // Send the damage value to "Damage_Control_##_##_CS" script in the collider.
            var damageScript = collider.GetComponent<LSJ_DamageControl00Base>();
            if (damageScript != null)
            { // The collider should be a breakable object.
                var damageValue = Attack_Point * loss * Attack_Multiplier;
                damageScript.Get_Damage(damageValue, Type);
                // Output for debugging.
                if (Debug_Flag)
                {
                    Debug.Log("HE Damage " + damageValue + " on " + collider.name);
                }
            }
        }
    }

}
