using UnityEngine;

public class VoxelCameraHit : MonoBehaviour {
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                //Debug.Log(hit.point);
                VoxelHitEvent.SetBlock(hit, new BlockAir());
            }
        }
        
    }
}
