/**
    @file   VoxelHitEvent.cs
    @date   2019.01.16
    @author 황준성(hns17.tistory.com)
*/

using UnityEngine;

/**
    @class  VoxelHitEvent
    @date   2019.01.16
    @author 황준성(hns17.tistory.com)
    @brief  Voxel Map Hit Event
*/
public class VoxelHitEvent {
    /**
        @brief  Hit된 Block의 위치 정보를 계산한다.
        @hit    hit : hit 정보, isSphere : Sphere Map 인 경우
        @return 위치 정보
    */
    public static Vector3Int GetBlockPos(RaycastHit hit, bool isSphere)
    {
        
        Vector3 point;
        //청크가 구형으로 변경되었을시 블럭의 위치좌표
        if (isSphere) {
            //위치원소의 가장 큰 절대값을 기준으로 정점좌표가 수정되었으므로, 
            //hitPoint의 길이는 위치원소의 가장 큰 값이다.
            //원래 블럭 위치를 구하기 위해 변경된 거리비를 구한다.
            //hitPoint의 길이를 hitPoint원소의 가장 큰 절대값으로 나눔

            var blockLen = Vector3.Magnitude(hit.point);
            var element = Mathf.Max(Mathf.Abs(hit.point.x), Mathf.Abs(hit.point.y), Mathf.Abs(hit.point.z));
            //거리비 구하기
            blockLen *= (blockLen / element);

            //변경되기 전의 위치
            point = hit.point.normalized * blockLen;
        }
        //큐브 형태 맵인 경우 히트된 블럭 위치 정보
        else {
            point = hit.point;
        }

        //hit 위치에서 block의 중심으로 좌표를 수정, 구형으로 변경시 블럭의 크기가 1이 아니므로 약간의 오차가 있다.
        Vector3 pos = (point + (hit.normal * -0.5f));
        
        Vector3Int blockPos = new Vector3Int(
            Mathf.FloorToInt(pos.x),
            Mathf.FloorToInt(pos.y),
            Mathf.FloorToInt(pos.z)
            );

        return blockPos;
    }
    

    /**
        @brief  hit된 블럭을 지정된 블럭으로 변경
        @param  hit : hit 정보, block : 변경할 블럭 정보
        @return 변경 상태를 리턴한다.
    */
    public static bool SetBlock(RaycastHit hit, Block block)
    {
        Chunk chunk = hit.collider.GetComponent<Chunk>();
        if (chunk == null)
            return false;
        

        //hit 위치에서 block의 위치정보를 구한다.
        Vector3Int pos = GetBlockPos(hit, chunk.isSphere) - chunk.pos;

        //블럭 정보를 변경한다.
        chunk.SetBlock(pos.x, pos.y, pos.z, block);
        return true;
    }

    /**
        @brief  hit된 블럭을 반환한다.
        @param  hit : hit 정보
        @return 블럭 정보를 리턴한다.
    */
    public static Block GetBlock(RaycastHit hit)
    {
        Chunk chunk = hit.collider.GetComponent<Chunk>();
        if (chunk == null)
            return null;

        //히트된 블럭의 위치정보를 가져온다.
        Vector3Int pos = GetBlockPos(hit, chunk.isSphere) - chunk.pos;
        
        //해당 위치의 블럭 정보를 가져온다.
        Block block = chunk.GetBlock(pos.x, pos.y, pos.z);

        return block;
    }
}
