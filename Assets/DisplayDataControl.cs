using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 各別資料顯示
/// </summary>
public class DisplayDataControl : MonoBehaviour
{
    /// <summary>
    /// 資訊顯示狀態
    /// </summary>
    private enum informationStatus{
        NumericalValue,
        textDescription
    }

    /// <summary>
    /// 數值介面群組
    /// </summary>
    public GameObject numericalGroup;
    /// <summary>
    /// 文字資訊群組
    /// </summary>
    public GameObject textInformationGroup; 
    /// <summary>
    /// 文字資訊裡的文字系統
    /// </summary>
    private WritingSystem writingSystem;

    private informationStatus interfaceUIStatus = informationStatus.NumericalValue;

    private void Start() {
        writingSystem = textInformationGroup.GetComponent<WritingSystem>();
    }

    /// <summary>
    /// 顯示資料
    /// </summary>
    public void DataShowsThat(){
        interfaceUIStatus = informationStatus.NumericalValue;

        numericalGroup.SetActive(true);
        textInformationGroup.SetActive(false);
    }
    /// <summary>
    /// 資訊切換
    /// </summary>
    public void InformationSwitching(){
        switch (interfaceUIStatus)
        {
            case informationStatus.NumericalValue:
                interfaceUIStatus = informationStatus.textDescription;
                numericalGroup.SetActive(false);
                textInformationGroup.SetActive(true);
                writingSystem.Initial();
                break;
            case informationStatus.textDescription:
                interfaceUIStatus = informationStatus.NumericalValue;
                numericalGroup.SetActive(true);
                textInformationGroup.SetActive(false);
                break;

        }
    } 

    /// <summary>
    /// 返回掃描
    /// </summary>
    public void BackToScan(){
        numericalGroup.SetActive(false);
        textInformationGroup.SetActive(false);
    }
}
