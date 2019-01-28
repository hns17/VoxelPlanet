/**
    @file   ItemObject.cs
    @date   2018.08.20
    @author 황준성(hns17.tistory.com)
    @brief  Item Object Event
*/
using System.Collections;
using UnityEngine;

/**
    @class  ItemObject
    @date   2018.08.20
    @author 황준성(hns17.tistory.com)
    @brief  아이템 위치 조정, 빌보드 및 획득 이벤트
*/
public class ItemObject : MonoBehaviour {
    private Camera targetObject = null;
    [SerializeField] private Item itemInfo = null;

    public Item INFO
    {
        get { return itemInfo;  }
        set { itemInfo = value; }
    }

	private void Start () {
        if (targetObject == null)
            targetObject = Camera.main;

        StartCoroutine(UpdateItemPosition());
    }

    private void Update()
    {
        //빌보드
        transform.forward = -targetObject.transform.forward;
    }

    /**
        brief   Item 위치를 바닥에 맞게 조정한다.
    */
    IEnumerator UpdateItemPosition()
    {
        while (true)
        {
            yield return null;

            //바다으로 광선, 그라비티 역방향으로 바꿔야 될 것 같다.
            Ray ray = new Ray(transform.position, -transform.up);

            //레이 체크 후 충돌하면 리지드 제거
            if (Physics.Raycast(ray, 0.5f)) {
                Destroy(GetComponent<Rigidbody>());
                break;
            }
        }
    }

    
    private void OnTriggerEnter(Collider other)
    {
        //플레이어와 충돌시 인벤토리에 추가 후 제거
        //이후 동일 아이템 생성을 생각해 오브젝트 풀로 관리하도록 변경해야 할 듯...
        if (other.CompareTag("Player")) {
            InventoryEvent.Instance.AddItem(itemInfo);
            Destroy(gameObject);
        }
    }
}
