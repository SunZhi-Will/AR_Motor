using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 文字顯示系統
/// </summary>
public class WritingSystem : MonoBehaviour
{
    /// <summary>
    /// 每頁文字
    /// </summary>
    [MultilineAttribute(5)]
    public string[] textContent;

    public Text textContentObject;
    /// <summary>
    /// 下一頁物件
    /// </summary>
    public GameObject nextPage;
    /// <summary>
    /// 上一頁物件
    /// </summary>
    public GameObject previousPage;

    /// <summary>
    /// 現在頁數
    /// </summary>
    private int index = 0;

    /// <summary>
    /// 初始化
    /// </summary>
    public void Initial()
    {
        index = 0;
        PageChange(0);
    }
    public void PageChange(int i){
        SoundControl.audioSourcePlay();
        index += i ;
        if(index == 0){
            previousPage.SetActive(false);
        }else{
            previousPage.SetActive(true);
        }
        if(index == textContent.Length - 1){
            nextPage.SetActive(false);
        }else{
            nextPage.SetActive(true);
        }
        textContentObject.text = textContent[index];
    }
}
