#define VC7
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;


/// <summary>
/// 滾動矩形助手
/// (加大小變化擴充)
/// </summary>
public class MyScrollRectHelper : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    #region 變數
    
    #region 拖移變數
        /// <summary>
        /// 內容UI物件
        /// </summary>
        public GameObject listItem;

        /// <summary>
        /// 靈敏度
        /// </summary>
        public float sensitivity = 0.15f;
        
        /// <summary>
        /// 滑動速度
        /// </summary>
        public float smooting = 5;

        /// <summary>
        /// 每頁顯示專案數量
        /// </summary>
        private int pageCount = 1;
        /// <summary>
        /// 滑動用ScrollRect
        /// </summary>
        private ScrollRect scrollRect;

        /// <summary>
        /// 總頁數
        /// </summary>
        private float pageIndex;
        /// <summary>
        /// 總頁數索引比列 
        /// </summary>
        /// <value>0-1</value>
        private List<float> listPageValue = new List<float> { 0 };

        /// <summary>
        /// 滑動目標位置(0-1)
        /// </summary>
        private float targetPos = 0;
        /// <summary>
        /// 當前位置索引
        /// </summary>
        private float nowindex = 0;
        /// <summary>
        /// 是否拖拽結束
        /// </summary>
        private bool isDrag = false;
        /// <summary>
        /// 開始拖移位置(0-1)
        /// </summary>
        private float beginDragPos;
        /// <summary>
        /// 結束拖移位置(0-1)
        /// </summary>
        private float endDragPos;
        
    #endregion

    #region 按鈕換頁
        /// <summary>
        /// 是否使用按鈕換頁
        /// </summary>
        public bool isButtonChangePage;
        /// <summary>
        /// 下一頁按鈕
        /// </summary>
        public Button nextPage;
        /// <summary>
        /// 上一頁按鈕
        /// </summary>
        public Button prePage;
    #endregion 

    #region 頁數點顯示
        /// <summary>
        /// 頁數點是否啟用
        /// </summary>
        public bool isPageNumber;
        /// <summary>
        /// 頁數點群組
        /// </summary>
        public GameObject pageNumberGroup;
        /// <summary>
        /// 頁數點物件
        /// </summary>
        public GameObject pageNumber;
        /// <summary>
        /// 目前頁數點的樣式
        /// </summary>
        public Sprite m_currentPoint;
        /// <summary>
        /// 其他頁數點的樣式
        /// </summary>
        public Sprite m_otherPages;
        /// <summary>
        /// 顯示頁數的點點
        /// </summary>
        private Image[] m_pageNumber;
    #endregion
    
    #region 放大縮小
    /// <summary>
    /// 是否大小縮放
    /// </summary>
    public bool isZoomIn;
    /// <summary>
    /// 每頁大小比例
    /// </summary>
    public RectTransform[] equipment;
    #endregion

    /// <summary>
    /// 每頁顯示物件
    /// </summary>
    public GameObject[] m_displayObject;


    
    #endregion
    

    void Awake(){
        scrollRect = this.GetComponent<ScrollRect>();

        ListPageValueInit();
        
        if(isPageNumber){
            GeneratePageCount();
        }

        if(isButtonChangePage){
            ButtonInit();
        }
    }
    /// <summary>
    /// 初始化
    /// </summary>
    public void Initialization(RectTransform[] _Equipment)
    {
        equipment = _Equipment;
        listPageValue = new List<float> { 0 };
        ListPageValueInit();
        equipment[(int)nowindex].localScale = new Vector3(1f, 1f, 1f);
        
    }

    /// <summary>
    /// 每頁比例
    /// </summary>
    void ListPageValueInit()
    {
        pageIndex = (listItem.transform.childCount / pageCount) - 1;
        if (listItem != null && listItem.transform.childCount != 0)
        {
            for (float i = 1; i <= pageIndex; i++)
            {
                listPageValue.Add((i / pageIndex));
            }
            if(isZoomIn){
                equipment = new RectTransform[listItem.transform.childCount];
                for (int i = 0; i < listItem.transform.childCount; i++)
                {
                    equipment[i] = listItem.transform.GetChild(i).GetComponent<RectTransform>();
                }
            }
        }
    }
    /// <summary>
    /// 按鈕設置
    /// </summary>
    void ButtonInit()
    {
        nextPage.onClick.AddListener(() => {PageChangeButton(1);});
        prePage.onClick.AddListener(() => {PageChangeButton(-1);});
    }
    void Update()
    {
        if (!isDrag)
            scrollRect.horizontalNormalizedPosition = Mathf.Lerp(scrollRect.horizontalNormalizedPosition, targetPos, Time.deltaTime * smooting);
        
    }

    /// <summary>
    /// 拖動開始
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        isDrag = true;
        beginDragPos = scrollRect.horizontalNormalizedPosition;
        
    }
    
    /// <summary>
    /// 拖拽結束
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {
        isDrag = false;
        endDragPos = scrollRect.horizontalNormalizedPosition; //獲取拖動的值
        endDragPos = endDragPos > beginDragPos ? endDragPos + sensitivity : endDragPos - sensitivity;
        int index = 0;
        float offset = Mathf.Abs(listPageValue[index] - endDragPos);    //拖動的絕對值
        for (int i = 1; i < listPageValue.Count; i++)
        {
            float temp = Mathf.Abs(endDragPos - listPageValue[i]);
            if (temp < offset)
            {
                index = i;
                offset = temp;
            }
        }
        targetPos = listPageValue[index];
        
        RestoreSize();
        if(isPageNumber){
            m_pageNumber[(int)nowindex].sprite = m_otherPages;
            m_pageNumber[index].sprite = m_currentPoint;
        }

        if(m_displayObject[(int)nowindex] != null)
            m_displayObject[(int)nowindex].SetActive(false);
        if(m_displayObject[index] != null)
            m_displayObject[index].SetActive(true);

        nowindex = index;

        
        Enlarge();
        
    }
    /// <summary>
    /// 換頁按鈕
    /// </summary>
    public void PageChangeButton(int num)
    {
        RestoreSize();
        //Debug.Log(nowindex);
        nowindex = Mathf.Clamp(nowindex + num, 0, pageIndex);
        targetPos = listPageValue[Convert.ToInt32(nowindex)];
        Enlarge();
    }
    /// <summary>
    /// 變回小尺寸
    /// </summary>
    private void RestoreSize(){ 
        if(isZoomIn){
            equipment[(int)nowindex].localScale = new Vector3(0.8f, 0.8f, 1f);
        }
    }
    /// <summary>
    /// 變回大尺寸
    /// </summary>
    private void Enlarge(){
        if(isZoomIn){
            if(equipment[(int)nowindex].localScale.x != 1)
                equipment[(int)nowindex].localScale = new Vector3(1f, 1f, 1f);
        }
        if(isButtonChangePage){
            if(pageIndex == nowindex){
                nextPage.gameObject.SetActive(false);
                prePage.gameObject.SetActive(true);
            }else if(0 == nowindex){
                prePage.gameObject.SetActive(false);
                nextPage.gameObject.SetActive(true);
            }else{
                nextPage.gameObject.SetActive(true);
                prePage.gameObject.SetActive(true);
            }
        }
    }
    /// <summary>
    /// 產生頁數點
    /// </summary>
    private void GeneratePageCount (){
        GameObject go;
        m_pageNumber = new Image[(int)pageIndex + 1];
        pageNumberGroup.GetComponent<GridLayoutGroup>().constraintCount = (int)pageIndex + 1;
        for (int i = 0; i <= pageIndex; i++)
        {
            go = Instantiate(pageNumber, pageNumber.transform.position, pageNumber.transform.rotation);
            m_pageNumber[i] = go.GetComponent<Image>();
            m_pageNumber[i].sprite = m_otherPages;
            go.transform.parent = pageNumberGroup.transform;
            go.transform.localScale = new Vector3(1f, 1f, 1f);
            
        }
        m_pageNumber[0].sprite = m_currentPoint;
        
    }
}