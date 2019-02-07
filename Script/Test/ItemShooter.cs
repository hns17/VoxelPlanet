using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemShooter : MonoBehaviour {

    private List<GameObject> itemList;
    

	void Start () {
        var items = Resources.LoadAll("Prefabs/Item");

        itemList = new List<GameObject>();
        foreach (var item in items)
            itemList.Add(item as GameObject);

        StartCoroutine(Shooter());
	}
	
	private IEnumerator Shooter()
    {
        while (true)
        {
            yield return Yields.WaitSeconds(0.5f);

            int idx = Random.Range(1, itemList.Count);
            var rigid = Instantiate(itemList[idx], transform.position, Quaternion.identity).
                   GetComponent<Rigidbody>();

            rigid.mass = 1f;

            var pos = Vector3.zero;

            pos.x += Random.Range(-3.0f, +3.0f);
            pos.z += Random.Range(-3.0f, +3.0f);
            pos.y += 4.0f;

            rigid.AddForce(pos * 80f);
        }
    }
}
