/**
    @file    Yields.cs
    @class   Yields   
    @date    2019.02.07
    @author  황준성(hns17.tistory.com)
    @brief   Couroutine 사용시 return Yield return의 Object가 동적으로 생성되는 것 같다.
             반복적인 생성이 GC를 생성하기 때문에 캐싱하여 사용한다.
*/
using UnityEngine;
using System.Collections.Generic;

public static class Yields {
    //WaitForSeconds 캐싱을 위해 정보를 담아두기 위한 자료구조
    private static Dictionary<float, WaitForSeconds> _timeInterval = new Dictionary<float, WaitForSeconds>(100);

    //WaitForEndOfFrame을 만들어 두고 반환
    private static WaitForEndOfFrame _endOfFrame = new WaitForEndOfFrame();
    public static WaitForEndOfFrame EndOfFrame
    {
        get { return _endOfFrame; }
    }

    //WaitForFixedUpdate를 만들어 두고 반환
    private static WaitForFixedUpdate _fixedUpdate = new WaitForFixedUpdate();
    public static WaitForFixedUpdate FixedUpdate
    {
        get { return _fixedUpdate; }
    }


    //WaitForSeconds 정보를 만들어 두고 반환
    public static WaitForSeconds WaitSeconds(float seconds)
    {
        if (!_timeInterval.ContainsKey(seconds))
            _timeInterval.Add(seconds, new WaitForSeconds(seconds));
        return _timeInterval[seconds];
    }
}
