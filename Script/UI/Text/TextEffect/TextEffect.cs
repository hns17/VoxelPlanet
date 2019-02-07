/**
    @file     TextEffect.cs
    @date     2019.01.09
    @author   황준성(hns17.tistory.com)
*/

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/**
    @struct     TextData
    @date       2019.01.09
    @author     황준성(hns17.tistory.com)
    @brief      문자의 애니메이션 정보를 담아둔다.
*/
public struct TextData
{
    private float   progress;
    private float   startTime;  //애니메이션 시작 시간
    private float   totalTime;  //애니메이션 종료 시간

    public TextData(float startTime, float totalTime)
    {
        progress = 0.0f;
        this.startTime = startTime;
        this.totalTime = totalTime;
    }

    public float Progress
    {
        get { return progress; }
    }
    
    //시간에 따라 Progress Update
    public void UpdateProgress(float time)
    {
        if (time < startTime)
            return;

        progress = (time - startTime) / totalTime;
    }
}

/**
    @class      TextEffect
    @date       2019.01.09
    @author     황준성(hns17.tistory.com)
    @brief      문자 효과를 표현하는 애니메이션 클래스
                BaseMeshEffect를 상속받아 UI의 Vertex를 수정한다.
*/
public class TextEffect : BaseMeshEffect {
    enum EffectMode { PLAY, REVERSE, PLAY_AND_REVERSE }
    [SerializeField] private EffectMode effectMode;


    //문자 애니메이션 정보(duration : 재생 속도, delay : 시작 시간)
    [SerializeField] private float duration = 1.0f;
    [SerializeField] private float delay = 0.01f;

    [SerializeField] private bool endOfDisable;
    
    //문장 전체 애니메이션 진행률
    [SerializeField] [Range(0.0f, 1.0f)] private float progress;


    private bool isPlaying;
    private Text textComponent;
    private TextData[] txtDataList;
    private UIVertexModifier vertexModifier;
    

    //animation time variable
    private float   currentPlayTime;
    private float   prevPlayTime;
    private float   endPlayTime;


    //Component Upadte Flag : true면 UpdateComponent 함수 호출
    private bool isDirty;

    public bool IsDirty {
        set { isDirty = value; }
    }


    /**
        @brief  수정할 Vertex 정보를 넘겨받는 CallBack Function
        @param  vh : ui Vertex Info
    */
    public override void ModifyMesh(VertexHelper vh)
    {
        int count = vh.currentVertCount;

        //문자가 없거나 비활성화 된 경우 리턴
        if (!IsActive() || count == 0)
        {
            vh.Clear();
            return;
        }

        int txtCount = 0;

        //Vertex Count : 글자 수 * 4(글자당 4개의 Vertex)
        for (int i = 0; i < count; i += 4)
        {
            //정보가 맞지 않으면 컴포넌트 정보 업데이트
            if (txtCount >= txtDataList.Length)
            {
                vh.Clear();
                UpdateComponent();
                break;
            }


            //텍스트 정보에 맞춰 Vertex 정보를 수정한다.
            var txtData = txtDataList[txtCount];
            for (int j = 0; j < 4; j++)
            {
                //UI Vertex를 가져온다.
                UIVertex uIVertex = new UIVertex();
                vh.PopulateUIVertex(ref uIVertex, i + j);

                //Vertex 정보 수정 후 교체
                vertexModifier.Apply(txtData, ref uIVertex);
                vh.SetUIVertex(uIVertex, i + j);
            }

            txtCount++;
        }
    }

    /**
        @brief      TextEffect 정보 업데이트(txtDataList, target Modifier, endPlayTime)
        @context    함수가 호출되는 경우
                    1. 처음 SetUp 
                    2. Inspector 값 변경, 
                    3. TextUI Text변경으로 인한 Length Over, 
                    4. Modifyier 변경
    */
    private void UpdateComponent()
    {
        if (isDirty) {
            if (textComponent == null)
                textComponent = GetComponent<Text>();
            
            vertexModifier = GetComponent<UIVertexModifier>();

            int charCount = textComponent.text.Length;
            txtDataList = new TextData[charCount];

            //TextEffect animation 종료 시간.
            endPlayTime = duration + (charCount * delay);
            
            //각 문자의 애니메이션 정보 생성
            for (int i = 0; i < charCount; i++)
                txtDataList[i] = new TextData(delay * i, duration);

            isDirty = false;
        }
    }

    /**
        @brief  Text 애니메이션 Update
    */
    private void UpdateEffect()
    {
        if (!isPlaying)
            currentPlayTime = progress * endPlayTime;
        else
            progress = currentPlayTime / endPlayTime;

        
        //텍스트 애니메이션 업데이트, 업데이트된 정보는 UI Vertex Modifier에서 사용
        for (int i = 0; i < txtDataList.Length; i++)
            txtDataList[i].UpdateProgress(currentPlayTime);

        //이전과 현재 재생시간이 같으면 UI Update를 하지 않는다.
        if (currentPlayTime != prevPlayTime) {
            prevPlayTime = currentPlayTime;
            graphic.SetAllDirty();
        }
        
    }
    

    /**
        @brief  animation 재생
    */
    private IEnumerator PlayEffectTime()
    {
        while (currentPlayTime + Time.deltaTime < endPlayTime)
        {
            yield return null;
            currentPlayTime += Time.deltaTime;
        }

        progress = 1;
        currentPlayTime = endPlayTime;
    }

    /**
        @brief  animation 역재생
    */
    private IEnumerator ReverceEffectTime()
    {
        while (currentPlayTime - Time.deltaTime >= 0)
        {
            yield return null;
            currentPlayTime -= Time.deltaTime;
        }

        progress = 0;
        currentPlayTime = 0;
    }


    /**
        @brief  animation 재생 후 역재생
    */
    private IEnumerator PlayAndReverseEffectTime()
    {
        yield return PlayEffectTime();
        currentPlayTime = endPlayTime;
        yield return Yields.WaitSeconds(3f);
        yield return ReverceEffectTime();
    }

    /**
        @brief  모드에 맞춰 애니메이션 진행
    */
    IEnumerator PlayTextEffect()
    {
        currentPlayTime = 0.0f;
        if (effectMode == EffectMode.PLAY) {
            yield return PlayEffectTime();
        }
        else if (effectMode == EffectMode.REVERSE) {
            currentPlayTime = endPlayTime;
            yield return  ReverceEffectTime();
        }
        else {
            yield return  PlayAndReverseEffectTime();
        }

        
        isPlaying = false;

        if (endOfDisable)
            gameObject.SetActive(false);
    }
    
    /**
        @brief  text effect animation 시작 
    */
    public void Play()
    {
        isPlaying = true;
        StartCoroutine(PlayTextEffect());
    }

    /**
        @brief  text effect animation 종료
    */
    private void Stop()
    {
        isPlaying = false;
    }


    protected override void OnEnable()
    {
        base.OnEnable();
        isDirty = true;
        UpdateComponent();

        if (Application.isPlaying)
            Play();
    }


#if UNITY_EDITOR
    protected override void OnValidate()
    {
        base.OnEnable();
        isDirty = true;
    }
#endif

    private void Update ()
    {
        UpdateComponent();
        UpdateEffect();
    }
}
