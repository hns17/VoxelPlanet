using UnityEngine;

/**
    @file   ModifyArea.cs
    @class  ModifyArea
    @date   2018.07.26
    @author 황준성(hns17.tistory.com)
    @brief  Planet Chunk의 Custom Block Area 정보
*/

public enum ModifyFillMode
{
    NOISE, FILL
}

[System.Serializable]
public abstract class ModifyArea
{
    public float[] pos = new float[3];
    public float[] rot = new float[4];
    public float[] scale = new float[3];

    public string assetPath;
    public bool isSphere = false;
    public bool isUpdate = true;
    
    public ModifyArea() { }
    public ModifyArea(string path, Transform tf)
    {
        assetPath = path;
        SetTransformInfo(tf);
    }


    protected ModifyArea(ModifyArea copyThis) : this()
    {
        assetPath = copyThis.assetPath;
        isSphere = copyThis.isSphere;

        pos = (float[])copyThis.pos.Clone();
        rot = (float[])copyThis.rot.Clone();
        scale = (float[])copyThis.scale.Clone();
    }

    public void SetTransformInfo(Transform tf)
    {
        pos[0] = tf.position.x;
        pos[1] = tf.position.y;
        pos[2] = tf.position.z;

        scale[0] = tf.localScale.x;
        scale[1] = tf.localScale.y;
        scale[2] = tf.localScale.z;


        rot[0] = tf.rotation.w;
        rot[1] = tf.rotation.x;
        rot[2] = tf.rotation.y;
        rot[3] = tf.rotation.z;


    }

    public Vector3 GetPosition()
    {
        return new Vector3(pos[0], pos[1], pos[2]);
    }

    public Vector3 GetScale()
    {
        return new Vector3(scale[0], scale[1], scale[2]);
    }

    public Quaternion GetRotation()
    {
        return new Quaternion(rot[1], rot[2], rot[3], rot[0]);
    }

    public abstract ModifyArea DeepCopy();
    public abstract Block BuildBlock(Vector3Int pos);
    public abstract ModifyFillMode GetModifyFillMode();
}


/**
    @class  FillArea
    @date   2018.07.26
    @author 황준성(hns17.tistory.com)
    @brief  Area내 블럭을 단순 채우기 방식으로 변경
*/
[System.Serializable]
public class FillArea : ModifyArea
{
    public enum BlockType { BLOCK, DESERT, GRASS, CLOUD, AIR, WATER }
    public BlockType blockType = BlockType.BLOCK;

    public FillArea() : base() { }
    public FillArea(string path, Transform tf, BlockType type)
                : base(path, tf)
    {
        blockType = type;
    }

    public FillArea(FillArea copyThis) : base(copyThis)
    {
        this.blockType = copyThis.blockType;
    }


    public override ModifyArea DeepCopy()
    {
        return new FillArea(this);
    }

    public override Block BuildBlock(Vector3Int pos)
    {

        Block newBlock = null;

        if (blockType == FillArea.BlockType.BLOCK)
            newBlock = new Block();
        else if (blockType == FillArea.BlockType.GRASS)
            newBlock = new BlockGrass();
        else if (blockType == FillArea.BlockType.DESERT)
            newBlock = new BlockDesert();
        else if (blockType == FillArea.BlockType.CLOUD)
            newBlock = new BlockCloud();
        else if (blockType == FillArea.BlockType.WATER)
            newBlock = new BlockWater();
        else
            newBlock = new BlockAir();

        return newBlock;
    }

    public override ModifyFillMode GetModifyFillMode()
    {
        return ModifyFillMode.FILL;
    }
}


/**
    @class  FillArea
    @date   2018.07.26
    @author 황준성(hns17.tistory.com)
    @brief  Area내 블럭을 Noise 정보와 Rate 정보에 맞게 변경
*/
[System.Serializable]
public class NoiseArea : ModifyArea
{
    [Header("[Block Rate Info]")]
    [ReadOnly] public float blockRate = 0.25f;
    [ReadOnly] public float desertRate = 0.35f;
    [ReadOnly] public float grassRate = 0.5f;
    [ReadOnly] public float cloudRate = 0.1f;

    [Header("[Noise Info]")]
    [ReadOnly] public float frequency = 0.01f;
    [ReadOnly] public float persistence = 0.1f;
    [ReadOnly] public int octave = 1;

    public NoiseArea() : base() { }

    public NoiseArea(NoiseArea copyThis) : base(copyThis)
    {
        this.blockRate = copyThis.blockRate;
        this.desertRate = copyThis.desertRate;
        this.grassRate = copyThis.grassRate;
        this.cloudRate = copyThis.cloudRate;

        this.frequency = copyThis.frequency;
        this.persistence = copyThis.persistence;
        this.octave = copyThis.octave;
    }


    public override ModifyArea DeepCopy()
    {
        return new NoiseArea(this);
    }


    public override Block BuildBlock(Vector3Int pos)
    {
        Block newBlock = null;


        float noise = (MyUtil.Get3DNoise(pos.x, pos.y, pos.z, frequency,
                                            octave, persistence) + 1.0f) * 0.5f;

        noise = Mathf.Clamp01(noise);

        if (noise <= blockRate)
            newBlock = new Block();
        else if (noise <= desertRate)
            newBlock = new BlockDesert();
        else if (noise <= grassRate)
            newBlock = new BlockGrass();
        else if (noise <= cloudRate)
            newBlock = new BlockCloud();
        else
            newBlock = new BlockAir();

        return newBlock;
    }

    public override ModifyFillMode GetModifyFillMode()
    {
        return ModifyFillMode.NOISE;
    }
}

