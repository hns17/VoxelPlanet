/**
    @file   MyUtil.cs
    @date   2018.07.26
    @author 황준성(hns17.tistory.com)
    @brief  프로젝트에 사용될 함수를 모아둠.
*/

using UnityEngine;
using SimplexNoise;
using System.IO;

/**
    @class  MyUtil
    @date   2018.07.26
    @author 황준성(hns17.tistory.com)
    @brief  프로젝트에 사용되는 개인 함수 모음
*/
public class MyUtil
{
    /**
        @brief  Component를 복사하여 대상에 추가한다.
        @return 복사 생성한 Component를 리턴한다.
    */
    public static T CopyComponent<T>(T original, GameObject destination) where T : Component
    {
        System.Type type = original.GetType();
        Component copy = destination.AddComponent(type);
        System.Reflection.FieldInfo[] fields = type.GetFields();
        foreach (System.Reflection.FieldInfo field in fields)
        {
            field.SetValue(copy, field.GetValue(original));
        }
        return copy as T;
    }

    /**
        @brief  SimplexNoise
        @param  frequency : 변동 폭, max : 최대 노이즈 값 0 ~ max-1 까지 생성
        @return Snoise된 정수 값을 반환한다.
    */
    public static int GetSNoise(int x, int y, int z, float frequency, int max)
    {
        return Mathf.FloorToInt((Noise.Generate(x * frequency, y * frequency, z * frequency) + 1f) * (max / 2f));
    }

    /**
        @brief  SimplexNoise Sampling
        @return Sampling 된 3DNoise 값을 반환한다.
    */
    public static float Get3DNoise(int x, int y, int z, float startFrequency, int octave, float persistence)
    {

        float noise = 0;
        float nomalizeFactor = 0;

        float amplitude = 1.0f;
        float frequency = startFrequency;

        for (int i = 0; i < octave; i++)
        {
            nomalizeFactor += amplitude;

            noise += amplitude * Noise.Generate(frequency * x, frequency * y, frequency * z);
            frequency *= 2;

            amplitude *= persistence;
        }

        return noise / nomalizeFactor;
    }

    /**
        @brief  큐브의 정점이 Range 내에 있는지 검사
        @return Range 내에 위치한 정점의 수(0 : 범위 밖, 8 : 범위 내, 1~7 : 큐브가 범위에 걸쳐있다.)
    */
    public static int IsCubeInRange(Vector3Int cubePos, int cubeSize, float range)
    {
        int cnt = 0;

        if (IsVertexInRange(new Vector3Int(cubePos.x, cubePos.y, cubePos.z), -range, range)) cnt++;
        if (IsVertexInRange(new Vector3Int(cubePos.x, cubePos.y, cubePos.z + cubeSize), -range, range)) cnt++;
        if (IsVertexInRange(new Vector3Int(cubePos.x, cubePos.y + cubeSize, cubePos.z), -range, range)) cnt++;
        if (IsVertexInRange(new Vector3Int(cubePos.x, cubePos.y + cubeSize, cubePos.z + cubeSize), -range, range)) cnt++;
        if (IsVertexInRange(new Vector3Int(cubePos.x + cubeSize, cubePos.y, cubePos.z), -range, range)) cnt++;
        if (IsVertexInRange(new Vector3Int(cubePos.x + cubeSize, cubePos.y, cubePos.z + cubeSize), -range, range)) cnt++;
        if (IsVertexInRange(new Vector3Int(cubePos.x + cubeSize, cubePos.y + cubeSize, cubePos.z), -range, range)) cnt++;
        if (IsVertexInRange(new Vector3Int(cubePos.x + cubeSize, cubePos.y + cubeSize, cubePos.z + cubeSize), -range, range)) cnt++;

        return cnt;
    }

    /**
        @brief  정점이 Range 내에 위치하는지 검사
        @return true : 범위 내, false : 범위 밖
    */
    public static bool IsVertexInRange(Vector3Int point, float min, float max)
    {
        if (min >= point.x || max <= point.x)
            return false;
        if (min >= point.y || max <= point.y)
            return false;
        if (min >= point.z || max <= point.z)
            return false;

        return true;
    }


    /**
        @brief  object의 world 좌표를 계산한다
        @return 계산된 좌표값 반환
    */
    public static Vector3 GetObjectWorldPos(GameObject obj)
    {
        if(obj == null)
           return Vector3.zero;

        
        Transform tf = obj.transform;
        Vector3 worldPos = tf.position;

        while (tf.parent)
        {
            tf = tf.parent;
            worldPos += tf.position;
        }

        return worldPos;
    }
    
    
    /**
        @brief  Cusor의 잠금상태를 해제한다.
    */
    public static void EnableCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    /**
        @brief  Cusor의 상태를 잠금으로 설정한다.
    */
    public static void DisableCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }



    /**
        @brief  Vector3 소수점 내림 후 Vector3Into 반환
        @param  vec : Vector3 data
    */
    public static Vector3Int FloorVector3(Vector3 vec)
    {
        Vector3Int newVec = new Vector3Int();
        newVec.x = Mathf.FloorToInt(vec.x);
        newVec.y = Mathf.FloorToInt(vec.y);
        newVec.z = Mathf.FloorToInt(vec.z);

        return newVec;
    }

    /**
        @brief  Vector3 소수점 반올림 후 Vector3Into 반환
        @param  vec : Vector3 data
    */
    public static Vector3Int RoundVector3(Vector3 vec)
    {
        Vector3Int newVec = new Vector3Int();
        newVec.x = Mathf.RoundToInt(vec.x);
        newVec.y = Mathf.RoundToInt(vec.y);
        newVec.z = Mathf.RoundToInt(vec.z);

        return newVec;
    }


    //public static GameObject LoadPrefab(string path)
    //{
    //    var assetPath = Path.Combine(Application.dataPath, path);
    //    Debug.Log(Path.Combine(Application.dataPath, assetPath));
    //    var asset = AssetBundle.LoadFromFile(assetPath);

    //    if (asset == null)
    //    {
    //        Debug.Log("Load Failed : " + assetPath);
    //        return null;
    //    }

    //    return asset.LoadAsset<GameObject>("");
    //}

}