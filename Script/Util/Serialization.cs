/**
    @file   Serialization.cs
    @date   2018.08.02
    @author 황준성(hns17.tistory.com)
*/

using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

/**
    @class  Serialization
    @date   2018.08.02
    @author 황준성(hns17.tistory.com)
    @brief  Data Save & Load (FileType : Binary, Default Folder : Assets/MapData)
*/
public static class Serialization
{
    static IFormatter formatter = new BinaryFormatter();

    /**
        @brief  디렉토리 생성
        @return 디렉토리 path 반환
    */
    public static string SaveLocation(string directory)
    {
        string saveLocation = directory;
        
        if (saveLocation.Substring(saveLocation.Length - 2) != "/")
            saveLocation += "/";

        if (!Directory.Exists(saveLocation))
        {
            Directory.CreateDirectory(saveLocation);
        }

        return saveLocation;
    }


    /**
        @brief  지정된 Folder 삭제
    */
    public static void ResetDirectory(string directory)
    {
        var delLocation = directory;

        if (delLocation.Substring(delLocation.Length - 2) != "/")
            delLocation += "/";

        //폴더가 존재하면 지운다.
        var dirInfo = new System.IO.DirectoryInfo(delLocation);
        if (dirInfo == null)
            return;

        if (dirInfo.Exists)
            System.IO.Directory.Delete(delLocation, true);
    }

    /**
        @brief  저장될 FileName 구성, Chunk의 위치 값으로 저장한다.
        @return 구성된 FileName 반환
    */
    public static string FileName(Vector3Int chunkLocation)
    {
        string fileName = chunkLocation.x + "," + chunkLocation.y + "," + chunkLocation.z + ".bin";
        return fileName;
    }


    /**
        @brief  Chunk 정보를 저장한다.
    */
    public static void SaveChunk(Chunk chunk, string directory = "Assets/MapData")
    {
        //block 정보가 없으면 rebuild
        if (chunk.blocks == null)
            chunk.RebuildBlock();
         
        SaveChunk save = new SaveChunk(chunk);

        if (save.blocks.Count == 0)     
            return;

        string path = directory;
        if (chunk.transform.parent.name == "Terrain")
            path += "/Terrain";
        else
            path += "/Clouds";
        

        string saveFile = SaveLocation(path);
        saveFile += FileName(chunk.pos);

        Debug.Log(saveFile);

        //바이너리로 저장
        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(saveFile, FileMode.Create, FileAccess.Write, FileShare.None);
        formatter.Serialize(stream, save);
        stream.Close();
    }


    public static SavePlanet LoadPlanet(string path)
    {

        if (!File.Exists(path))
            return null;

        FileStream stream = new FileStream(path, FileMode.Open);
        SavePlanet save = (SavePlanet)formatter.Deserialize(stream);
        stream.Close();
        
        //foreach (KeyValuePair<WorldPos, List<ModifyArea>> pair in save.squareArea)
        //{
        //    Debug.Log(pair.Value[0].GetModifyFillMode());
        //}

        return save;
    }

    /**
        @brief  Chunk 정보를 불러온다.
    */
    public static void LoadChunk(Chunk chunk, string directory = "Assets/MapData")
    {
        if (chunk == null)
            return;

        var parent = chunk.transform.parent;

        //청크 블럭 생성
        parent.GetComponent<VoxelPlanet>().BuildBlocks(chunk);

        //파일 로드 후 수정
        string path = directory + "/" + chunk.transform.parent.parent.name;
        if (chunk.transform.parent.name == "Terrain")
            path += "/Terrain";
        else
            path += "/Clouds";

        path += "/" + FileName(chunk.pos);
        
        if (!File.Exists(path))
            return;

        FileStream stream = new FileStream(path, FileMode.Open);

        SaveChunk save = (SaveChunk)formatter.Deserialize(stream);
          
        foreach (var block in save.blocks)
            chunk.blocks[block.Key.x, block.Key.y, block.Key.z] = block.Value;
        
        chunk.update = true;
        stream.Close();
    }

    /**
        @brief  행성 정보를 저장한다.
     */
    public static void SaveVoxelPlanet(GameObject planet, string savePath)
    {
        if (planet == null)
            return;

        string dir = Path.GetDirectoryName(savePath) + "/" + Path.GetFileNameWithoutExtension(savePath);
        ResetDirectory(dir);
        SaveLocation(dir);

        var planetPath = dir + "/" + Path.GetFileName(savePath);
        var planetInfo = planet.GetComponent<VoxelPlanetInfo>();
        
        //행성 생성 정보 저장
        var tfTerrain = planet.transform.Find("Terrain");
        var tfClouds = planet.transform.Find("Clouds");

        var savePlanet = new SavePlanet(planetInfo, tfTerrain, tfClouds);
       
        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(planetPath, FileMode.Create, FileAccess.Write, FileShare.None);
        formatter.Serialize(stream, savePlanet);
        stream.Close();

        //행성 데이터 저장
        //dir += Path.GetFileNameWithoutExtension(savePath) + "_Data";
        
        SavePlanetData(tfTerrain, dir);
        SavePlanetData(tfClouds, dir);
    }


    /**
        @brief  행성의 블록 정보를 저장한다.
     */
    private static void SavePlanetData(Transform tf, string directory)
    {
        if (tf == null)
            return;
        
        int cnt = tf.childCount;

        for(int i=0; i < cnt; i++)
        {
            var chunk = tf.GetChild(i).GetComponent<Chunk>();

            if(chunk != null)
                SaveChunk(chunk, directory);
        }

    }

    
    /**
        @brief  Planet의 최신 정보 저장 위치를 반환
        @return 저장된 Scene인 경우 Scene폴더에, 그렇지 않은 경우 임시폴더에 저장
    */
    public static string GetCurrentPlanetInfoPath()
    {
        var scene = SceneManager.GetActiveScene();
        string savePath = scene.path;

        if (scene.path.Equals(""))
            savePath = "Assets/MapData/CurrentPlanetInfo.planet";
        else
            savePath=Path.GetDirectoryName(savePath) +"/"  + Path.GetFileNameWithoutExtension(savePath) + ".planet";
   
        return savePath;
    }
}
