using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// AR掃描狀態控制
/// </summary>
public class ScanQRCode : MonoBehaviour
{
    private enum ARPicture{
        PrimeMoverImage,
        ReducerImage,
        SpeedIncreaserImage,
        GeneratorImage
    }
    
    /// <summary>
    /// 掃描介面
    /// </summary>
    public GameObject scanInterface; 
    /// <summary>
    /// 掃描按鈕
    /// </summary>
    public GameObject scanning;
    /// <summary>
    /// 說明按鈕
    /// </summary>
    //public GameObject description;
    /// <summary>
    /// 退出介面
    /// </summary>
    public GameObject ExitInterface;

    /// <summary>
    /// 掃描圖片物件
    /// </summary>
    public GameObject[] aRImage;
    /// <summary>
    /// 掃描圖片資料顯示
    /// </summary>
    private DisplayDataControl displayDataControl;
    /// <summary>
    /// 掃描到的AR數值
    /// </summary>
    private ARPicture aRImageValue;

    /// <summary>
    /// 確認圖片
    /// </summary>
    public GameObject scanToConfirm;
    /// <summary>
    /// 載入動畫
    /// </summary>
    public GameObject loading;

    /// <summary>
    /// 是否處於掃描狀態
    /// </summary>
    private bool scanStatus = true;

    public GameObject qRCode;
    public Text text;
    public bool lockBlock = false;
    private void Start() {
        Debug.Log(qRCode.GetComponent<RectTransform>().rect.xMin+ " " + qRCode.GetComponent<RectTransform>().rect.xMax);
    }

    /// <summary>
    /// 掃描到QRCode
    /// </summary>
    public void ScanSuccessfully(int num){
        if(scanStatus){
            aRImageValue = (ARPicture)num;

            
            Vector3 screenPos=Camera.main.WorldToScreenPoint(aRImage[(int)aRImageValue].transform.position);
            //設定判斷區域（範圍根據實際情況設定）‎
            Rect rc = qRCode.GetComponent<RectTransform>().rect;    
            rc = new Rect(rc.x + Screen.width / 2, rc.y + Screen.height / 2, rc.width, rc.height);

            text.text = screenPos.ToString();
                //接着就是做简单的比较判断
            if(screenPos.x>rc.xMin&&screenPos.x<rc.xMax&&screenPos.y>rc.yMin&&screenPos.y<rc.yMax)
            {
                scanToConfirm.SetActive(true);
                scanStatus = false;
                Invoke("Loading", 1);
            }
            else
            {
                lockBlock = true;
                Invoke("LockBlock", 0.1f);
                //不满足条件时的操作

            }

            
        }
    }
    private void LockBlock(){
        if(lockBlock){
            ScanSuccessfully((int)aRImageValue);
        }
    }
    public void BlockUnlock(){
        lockBlock = false;
    }


    /// <summary>
    /// 進入載入
    /// </summary>
    private void Loading(){
        scanToConfirm.SetActive(false);
        scanInterface.SetActive(false);
        loading.SetActive(true);
        Invoke("DataShowsThat", 1);
    }
    /// <summary>
    /// 顯示資料
    /// </summary>
    private void DataShowsThat(){
        loading.SetActive(false);
        for (int i = 0; i < aRImage.Length; i++)
        {
            if(i == (int)aRImageValue){
                aRImage[i].SetActive(true);
                displayDataControl = aRImage[i].GetComponent<DisplayDataControl>();
                displayDataControl.DataShowsThat();
            }else{
                aRImage[i].SetActive(false);
            }
        }
        

        scanning.SetActive(true);
        //description.SetActive(true);

    }

    /// <summary>
    /// 資訊切換
    /// </summary>
    public void InformationSwitching(){
        SoundControl.audioSourcePlay();
        displayDataControl.InformationSwitching();
    }
    
    /// <summary>
    /// 返回掃描
    /// </summary>
    public void BackToScan(){
        SoundControl.audioSourcePlay();
        scanStatus = true;
        scanInterface.SetActive(true);

        scanning.SetActive(false);
        //description.SetActive(false);

        
        for (int i = 0; i < aRImage.Length; i++)
        {
            if(i == (int)aRImageValue){
                displayDataControl.BackToScan();
                aRImage[i].SetActive(false);
            }else{
                aRImage[i].SetActive(false);
            }
        }
        
    }
    /// <summary>
    /// 退出
    /// </summary>
    public void DropOut(){
        SoundControl.audioSourcePlay();
        if(!ExitInterface.activeInHierarchy)
            ExitInterface.SetActive(true);
        else
            ExitInterface.SetActive(false);
        
    }

}
