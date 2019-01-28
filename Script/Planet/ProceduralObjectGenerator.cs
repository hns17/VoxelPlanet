/**
    @file       ProceduralObjectGenerator.cs
    @class      ProceduralObjectGenerator
    @date       2018.08.03
    @author     황준성(hns17.tistory.com)
    @brief      지형에 절차적으로 오브젝트를 배치합니다.
 */
using System.Collections.Generic;
using UnityEngine;

public class ProceduralObjectGenerator : MonoBehaviour {

    [Header("Install List")]
    [SerializeField]public List<GameObject> objs;

    //배치될 오브젝트의 Offset
    [Header("[Offset Paramiter]")]
    public Vector3 offsetPos;
    public Vector3 offsetRot;
    
    [Header("[Noise Paramiter]")]
    [Range(0.0f, 1.0f)] public float frequency = 0.0f;
    [Range(0.0f, 1.0f)] public float density = 1.0f; 

    //설치할 오브젝트의 콜라이더와 주변 오브젝트 충돌 검사 on/off
    [Header("[Check Collision]")]
    public bool colCapsule = false;
    public bool colBox = false;
    public bool colSphere = false;
    
    //설치 위치 목록 <설치 방향, 위치>
    Dictionary<string, List<Vector3>> installPosList = null;


    /**
        @brief  Object를 설치합니다.
        @todo   BuildInstallPosition 함수를 호출해 설치 목록을 만든다.
                만들어진 설치 목록에 설치할 오브젝트를 선택하여 설치한다.
     */
    public void InstallObject()
    {
        if (objs.Count <= 0)
            return;
         

        BuildInstallPosition();
        
        if (installPosList.Count <= 0)
            return;
        
        var emul = installPosList.GetEnumerator();
        int objCnt = objs.Count;

        while (emul.MoveNext())
        {
            Vector3 direction = GetDirectionVector(emul.Current.Key);
            direction = new Vector3(direction.z, direction.y, direction.x);

            //설치 방향에 따른 rotation offset 구성
            Vector3 calcOffsetRot = Vector3.zero;
            if (direction.y == -1)
                calcOffsetRot.x = 180;
            else
                calcOffsetRot = Vector3.Scale(new Vector3(90, 0, -90), direction);
            calcOffsetRot += offsetRot;

            var positions = emul.Current.Value;
            int posCnt = positions.Count;

            var parent = transform.parent.Find("ObjectList");
            if (parent == null)
                parent = new GameObject("ObjectList").transform;

            parent.parent = transform.parent;

            for (int i = 0; i < posCnt; i++)
            {
                var obj = objs[Random.Range(0, objCnt)];
                var rot = obj.transform.rotation.eulerAngles + calcOffsetRot;
                var quat = Quaternion.Euler(rot);

                //위치는 설치 위치 + (회전이 적용된 obj pos)
                var pos = positions[i] + (quat * obj.transform.position + offsetPos);

                //주변 오브젝트와 충돌검사
                if (CheckCapsuleCollision(obj, pos, rot)) continue;
                if (CheckSphereCollision(obj, pos, rot)) continue;
                if (CheckBoxCollision(obj, pos, rot)) continue;

                //오브젝트 설치
                var go = Instantiate(obj, pos, quat);
                go.transform.parent = parent;
            }
        }
    }

    /**
        @brief  설치하려는 Object의 Capsule Collider와 충돌하는 객체가 있는지 검사
     */
    bool CheckCapsuleCollision(GameObject obj, Vector3 installPos, Vector3 installRot)
    {
        if (!colCapsule)
            return false;

        var capsule = obj.GetComponents<CapsuleCollider>();
        
        if (capsule == null)
            return false;

        int cnt = capsule.Length;

        //설치 영역은 충돌에서 제외
        int layerMask = ~(1 << LayerMask.NameToLayer("InstallArea"));
        for (int i=0; i<cnt; i++)
        {

            var rot = Quaternion.Euler(installRot);
            var scale = obj.transform.localScale;
            
            //반지름 Scale은 Scale.xz 중 큰 값에 영향을 받는다. 
            Vector3 radius = new Vector3(0, capsule[i].radius * Mathf.Max(scale.x, scale.z));

            //캡슐의 높이 Scale은 Scale.y에 영향을 받는다.
            Vector3 halfHeight = Vector3.Scale(scale, new Vector3(0, capsule[i].height * 0.5f, 0)) ;
            Vector3 center = Vector3.Scale(scale, capsule[i].center);

            //회전이 적용된 CapSule Collider의 Center를 구하고,  월드좌표로 변환
            Vector3 start = installPos + rot * (center + halfHeight - radius);
            Vector3 end = installPos + rot * (center - halfHeight + radius);

            //Debug.Log(start.ToString("F4") + ", " + end.ToString("F4"));
            

            if (Physics.CheckCapsule(start, end, radius.y, layerMask))
                return true;
        }
        
        return false;
    }

    /**
        @brief  설치하려는 Object의 Sphere Collider와 충돌하는 객체가 있는지 검사
     */
    bool CheckSphereCollision(GameObject obj, Vector3 installPos, Vector3 installRot)
    {
        if (!colSphere)
            return false;

        var sphere = obj.GetComponents<SphereCollider>();

        if (sphere == null)
            return false;

        int cnt = sphere.Length;

        //설치 영역은 충돌에서 제외
        int layerMask = ~(1 << LayerMask.NameToLayer("InstallArea"));

        var rot = Quaternion.Euler(installRot);

        for (int i = 0; i < cnt; i++)
        {
            //Sphere Collider의 중심점은 대상이 되는 Obj의 크기와 회전에 영향을 받는다.
            var center = installPos + (rot * sphere[i].center);
            //var center = installPos + (rot * Vector3.Scale(sphere[i].center, scale));

            if (Physics.CheckSphere(center, sphere[i].radius, layerMask))
                return true;
        }

        return false;
    }

    /**
        @brief  설치하려는 Object의 Box Collider와 충돌하는 객체가 있는지 검사
     */
    bool CheckBoxCollision(GameObject obj, Vector3 installPos, Vector3 installRot)
    {
        if (!colBox)
            return false;

        var box = obj.GetComponents<BoxCollider>();

        if (box == null)
            return false;

        int cnt = box.Length;

        //설치 영역은 충돌에서 제외
        int layerMask = ~(1 << LayerMask.NameToLayer("InstallArea"));

        //BoxCollider의 중심점과 Size는 대상이 되는 Obj의 크기에 영향을 받는다.
        Vector3 scale = obj.transform.localScale;
        var rot = Quaternion.Euler(installRot);
        for (int i = 0; i < cnt; i++)
        {
            var center = installPos + (rot * Vector3.Scale(box[i].center, scale));
            if (Physics.CheckBox(center, Vector3.Scale(box[i].size * 0.5f, scale), rot, layerMask))
                return true;
        }

        return false;
    }

    /**
        @brief      설치영역 내 청크를 탐색해 오브젝트 설치 가능한 위치 목록을 만든다.
        @todo       1차적으로 설정된 설치영역내 청크 탐색
                    탐색된 청크를 대상으로 CreatePositionList 함수를 호출하여 설치 위치 목록 생성
     */
    void BuildInstallPosition()
    {

        if (installPosList == null)
            installPosList = new Dictionary<string, List<Vector3>>();

        installPosList.Clear();
        
        //int layerMask = (1 << LayerMask.NameToLayer("Chunk"));
        //int customLayer = LayerMask.NameToLayer("InstallArea");

        var planet = transform.parent;
        var terrain = planet.Find("Terrain");
        var cloud = planet.Find("Clouds");

        List<Chunk> objChunks = new List<Chunk>();
        SetCollisionChunk(terrain, objChunks);
        SetCollisionChunk(cloud, objChunks);



        //찾은 청크의 블럭을 탐색하여 오브젝트 설치 가능한 위치 목록을 만든다.
        int chunkCnt = objChunks.Count;
        for (int j = 0; j < chunkCnt; j++)
        {
            Debug.Log(objChunks[j]);
            var chunk = objChunks[j].GetComponent<Chunk>();
            CreatePositionList(chunk);
        }
    }

    /**
        @brief      블럭을 탐색해 오브젝트 설치 가능한 위치를 찾아 목록을 만든다.
        @condition  1. AirBlock에는 설치 불가능
                    2. 설정한 CustomArea내에 속하는 블럭 일 것
                    3. 설치방향의 이웃 블럭은 AirBlock 일 것
     */
    void CreatePositionList(Chunk chunk)
    {
        //layerMask : chunkArea만 체크
        //blockHalfExtent : 블럭의 HalfSize, 블럭의 크기는 Vector(1,1,1)
        int cnt = chunk.chunkSize;
        int layerMask = (1 << LayerMask.NameToLayer("InstallArea"));
        Vector3 blockHalfExtent = Vector3.one * 0.5f;

        int maxNoiseValue = 100;

        for (int x = 0; x < cnt; x++) {
            for (int y = 0; y < cnt; y++) {
                for (int z = 0; z < cnt; z++) {
                    var block = chunk.GetBlock(x, y, z);

                    //AirBlock은 제외
                    if (block.IsSolid() == -1)
                        continue;

                    //블럭의 월드기준 중심점
                    Vector3 blockCenter = transform.root.position + (Vector3Int)chunk.pos + new Vector3(x, y, z) + blockHalfExtent;

                    //CustomArea에 속한 Block인지 검사 
                    var colls = Physics.OverlapBox(blockCenter, blockHalfExtent, Quaternion.identity, layerMask);

                    if (colls.Length <= 0)
                        continue;

                    string direction = colls[0].gameObject.tag;
                    var vecDirection = GetDirectionVector(direction);

                    var worldPos = (Vector3Int)chunk.pos + new Vector3Int(x, y, z);
                    var neighborWorldPos = worldPos + vecDirection;
                    
                    //이웃 블럭 검사
                    if (chunk.GetBlock(neighborWorldPos).IsSolid() != -1)
                        continue;


                    if (MyUtil.GetSNoise(worldPos.x, worldPos.y, worldPos.z, frequency, maxNoiseValue) > maxNoiseValue * density)
                        continue;

                    //설치 위치 추가, block의 크기가 1이므로 중심에서 half vecDirection만큼 더해
                    //설치할 면의 위치를 구한다.
                    if (!installPosList.ContainsKey(direction))
                        installPosList.Add(direction, new List<Vector3>());
                    
                    installPosList[direction].Add(blockCenter + ((Vector3)vecDirection * 0.5f));
                }
            }
        }
    }


    /**
        @brief  설치 방향에 맞는 방향 벡터를 반환한다.
     */
    Vector3Int GetDirectionVector(string direction)
    {
        Vector3Int vecDirection = Vector3Int.zero;

        if (direction == "InstallDirectionDown")
            vecDirection.y = -1;
        else if (direction == "InstallDirectionLeft")
            vecDirection.x = -1;
        else if (direction == "InstallDirectionRight")
            vecDirection.x = 1;
        else if (direction == "InstallDirectionFront")
            vecDirection.z = -1;
        else if (direction == "InstallDirectionBack")
            vecDirection.z = 1;
        else
            vecDirection.y = 1;

        return vecDirection;
    }

    void SavePlanetData()
    {

        
    }

    void LoadPlanetData()
    {

    }

    void SetCollisionChunk(Transform tf, List<Chunk> list)
    {
        if (tf == null)
            return;
        if (tf.childCount <= 0)
            return;

        int layerMask = (1 << LayerMask.NameToLayer("InstallArea"));

        for (int i = 0; i < tf.childCount; i++)
        {
            //Custom Area에 포함된 청크 체크를 위해 청크 범위 정보를 구성
            Chunk chunk = tf.GetChild(i).GetComponent<Chunk>();
            //int chunkSize = chunk.chunkSize;
            float radius = chunk.chunkSize * 0.5f;

            Vector3 halfExtent = new Vector3(radius, radius, radius);

            //(tf.position+ chunk.pos) : world 좌표
            //world좌표 + halfExtent : chunk의 pos 정보는 -x,-y,-z 위치이기 때문에 center로 맞춰준다.
            Vector3 center = tf.position + chunk.pos + halfExtent;

            Quaternion checkQuat = Quaternion.identity;

            //Sphere화 된 경우
            if (chunk.isSphere)
            {
                var prevCenter = center;

                //center 정보를 Sphere 좌표로 변환
                float len = Mathf.Max(Mathf.Abs(center.x), Mathf.Abs(center.y), Mathf.Abs(center.z));
                center = center.normalized * len;

                //chunk의 회전 정보
                checkQuat = Quaternion.FromToRotation(prevCenter, center);
            }


            //Area에 포함된 Chunk 검사
            if (Physics.CheckBox(center, halfExtent, checkQuat, layerMask))
                list.Add(chunk);
        }
    }
}


