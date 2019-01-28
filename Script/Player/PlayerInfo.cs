/**
    @file   PlayerInfo.cs
    @class  PlayerInfo
    @date   2018.08.04
    @author 황준성(hns17.tistory.com)
    @brief  플레이어의 기본 정보를 관리
*/
using UnityEngine;
using System.Collections.Generic;


public class PlayerInfo : MonoSingleton<PlayerInfo> {
    //플레이어의 상태
    public enum PlayerState {
        IDLE, JUMP, FALL, LANDING, WALK, RUN, WAKEUP, DEATH, ROLL, ATTACK
    };

    //렌더될 플레이어 모델 타입
    public enum ModelType
    {
        POLYGON, VOXEL
    }

    //폴리곤 모델
    [SerializeField] private GameObject polyModel;
    [SerializeField] private Avatar polyAvatar;

    //복셀 모델
    [SerializeField] private GameObject voxelModel;
    [SerializeField] private Avatar voxelAvatar;

    //콜라이더
    [SerializeField] private CapsuleCollider coll;

    //시작 모델 타입
    [SerializeField] private ModelType type;
    //현재 플레이어 상태
    [SerializeField] private PlayerState state;

    //조작 금지
    [SerializeField] private bool isLock = false;
    //이동속도
    [SerializeField] private float baseSpeed = 2f;
    //점프 높이
    [SerializeField] private float jumpHeight = 1f;

    private Rigidbody rigid;
    private Animator anim;

    //모델의 Transform
    private Transform tfModelWraper;
    //현재 모델 오브젝트
    private GameObject currentModel;

    //리스폰 위치
    private Vector3 posRespawn = Vector3.zero;


    //============== Get & Set Function ==============//
    public GameObject ModelMesh
    {
        get { return currentModel; }
    }

    public float JumpHeight
    {
        get { return jumpHeight; }
        set { jumpHeight = value; }
    }

    public Vector3 RespawnPosition
    {
        get { return posRespawn; }
        set { posRespawn = value; }
    }

    public ModelType Type
    {
        get { return type; }
    }

    public PlayerState State
    {
        get { return state; }
        set { state = value; }
    }

    public float BaseSpeed
    {
        get { return baseSpeed; }
        set { baseSpeed = value; }
    }
    
    public bool IsLock
    {
        get { return isLock; }
        set {
            isLock = value;

            //조작 금지 상태에서 속도 제거 및 콜라이더 조정
            if (isLock) {
                coll.center = new Vector3(0, anim.GetFloat("ColliderPosition"), 0);
                coll.height = anim.GetFloat("ColliderHeight");
                rigid.velocity = Vector3.zero;
            }
        }
    }

    public Animator Anim
    {
        get {
            if (anim == null)
                anim = GetComponent<Animator>();
            return anim;
        }
    }

    public Transform Model{
        get {
            return tfModelWraper;
        }
    }

    public Rigidbody Rigid
    {
        get {   return rigid;   }
    }

    public CapsuleCollider Collider
    {
        get { return coll; }
    }
    //====================================================//


    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        tfModelWraper = transform.Find("ModelWrapper");

        if (coll == null)
            coll = tfModelWraper.GetComponent<CapsuleCollider>();
        
        SetModel(type);

        MyUtil.DisableCursor();
        PlayerAnimation.Instance.WakeUp();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        //리스폰 레이어에 닿으면 리스폰 업데이트
        if (other.gameObject.layer == LayerMask.NameToLayer("RespawnArea"))
            posRespawn = other.transform.position;
        //크리티컬 레이어에 닿으면 죽음
        else if (other.gameObject.layer == LayerMask.NameToLayer("CriticalArea"))
                PlayerAnimation.Instance.Death();
    }

    private void OnCollisionEnter(Collision collision)
    {
        //바인드 오브젝트와 충돌하면 충돌된 오브젝트 자식으로
        if (collision.gameObject.CompareTag("BindObject"))
            transform.parent = collision.transform;
        
    }
    private void OnCollisionExit(Collision collision)
    {
        //바인드 오브젝트와 충돌하지 않으면 연결 해제
        if (collision.gameObject.CompareTag("BindObject"))
            transform.parent = null;
    }

   
    /**
        @brief  바닥과 충돌하는지 체크
        @param  distance : 충돌 거리, radius : sphere check radius
    */
    public bool IsGround(float distance, float radius = 0.5f)
    {
        RaycastHit hit;
        //start : 캐릭터 충돌체 중심(현재위치 + (upVec * (coll_HalfHeight+ coll_y Center)))
        //coll_HalfHeight : 캡슐 충돌체 HalfHeight
        //coll_y_Center : 충돌체는 y Axis가 높이로 맞춰져있다.
        var start = transform.position + transform.up * (coll.height * 0.5f + coll.center.y);
        bool isHit = Physics.SphereCast(start, radius, -transform.up, out hit, distance);

        return isHit;
    }

    //캐릭터 구형 충돌 테스트 용 이벤트 함수
//#if UNITY_EDITOR
//    private void OnDrawGizmos()
//    {
//        if (!Application.isPlaying)
//            return;

//        float maxDistance = 1.0f;
//        RaycastHit hit;

//        var start = transform.position + transform.up * (coll.height * 0.5f + coll.center.y);
//        var direction = -transform.up;
//        var radius = 0.05f;

//        // Physics.SphereCast (레이저를 발사할 위치, 구의 반경, 발사 방향, 충돌 결과, 최대 거리)
//        bool isHit = Physics.SphereCast(start, radius, direction, out hit, maxDistance);

//        Gizmos.color = Color.red;
//        if (isHit)
//        {
//            Gizmos.DrawRay(start, direction * hit.distance);
//            Gizmos.DrawWireSphere(start + direction * hit.distance, radius);
//        }
//        else
//        {
//            Gizmos.DrawRay(start, direction * maxDistance);
//        }

//    }
//#endif


    /**
        @brief  Player Model을 Poly or Voxel로 지정 
    */
    public void SetModel(ModelType setType, bool isActive = true)
    {
        //Poly Model로 지정할 경우
        if (setType == ModelType.POLYGON) {
            voxelModel.SetActive(false);

            type = ModelType.POLYGON;
            currentModel = polyModel;
            Instance.Anim.avatar = polyAvatar;
        }
        //Voxel 모델로 지정할 경우
        else {
            polyModel.SetActive(false);

            type = ModelType.VOXEL;
            currentModel = voxelModel;
            Instance.Anim.avatar = voxelAvatar;
        }
        //플레이어 활성화 상태
        currentModel.SetActive(isActive);
    }

}
