using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(MyScrollRectHelper))]
public class MyScrollRectHelperEditor : Editor
{
    MyScrollRectHelper myScrollRectHelper;
    private bool isPageNumber;
    private bool isButtonChangePage;
    private bool isZoomIn;


    private bool isPageNumberFolding;
    private bool isButtonChangePageFolding;
    private bool isZoomInFolding;

    SerializedProperty listItem;

    SerializedProperty nextPage;
    SerializedProperty prePage;

    SerializedProperty pageNumberGroup;
    SerializedProperty pageNumber;
    SerializedProperty m_currentPoint;
    SerializedProperty m_otherPages;

    SerializedProperty equipment;
 
     private void OnEnable()
     {
         listItem = serializedObject.FindProperty("listItem");

         nextPage = serializedObject.FindProperty("nextPage");
         prePage = serializedObject.FindProperty("prePage");

         pageNumberGroup = serializedObject.FindProperty("pageNumberGroup");
         pageNumber = serializedObject.FindProperty("pageNumber");
         m_currentPoint = serializedObject.FindProperty("m_currentPoint");
         m_otherPages = serializedObject.FindProperty("m_otherPages");

         equipment = serializedObject.FindProperty("equipment");


     }


    public override void OnInspectorGUI()
    {
        myScrollRectHelper =  (MyScrollRectHelper)target;
        

        listItem.objectReferenceValue = EditorGUILayout.ObjectField("內容UI物件", listItem.objectReferenceValue, typeof(Object), true);
        myScrollRectHelper.sensitivity = EditorGUILayout.FloatField("靈敏度", myScrollRectHelper.sensitivity);
        myScrollRectHelper.smooting = EditorGUILayout.FloatField("滑動速度", myScrollRectHelper.smooting);

        #region 按鈕換頁
        isButtonChangePageFolding = EditorGUILayout.BeginFoldoutHeaderGroup(isButtonChangePageFolding, "Button Change Page");
        if(isButtonChangePageFolding){
            if(Selection.activeTransform){
                isButtonChangePage = EditorGUILayout.Toggle("按鈕換頁啟用", myScrollRectHelper.isButtonChangePage);
                myScrollRectHelper.isButtonChangePage = isButtonChangePage;

                EditorGUI.BeginDisabledGroup(isButtonChangePage == false);
                nextPage.objectReferenceValue = EditorGUILayout.ObjectField("下一頁按鈕", nextPage.objectReferenceValue, typeof(Button), true);
                prePage.objectReferenceValue = EditorGUILayout.ObjectField("上一頁按鈕", prePage.objectReferenceValue, typeof(Button), true);
                EditorGUI.EndDisabledGroup();
            }

        }
        if(!Selection.activeTransform){
            isButtonChangePageFolding = false;
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        #endregion

        #region 頁數點顯示
        isPageNumberFolding = EditorGUILayout.BeginFoldoutHeaderGroup(isPageNumberFolding , "Page Number");
        if(isPageNumberFolding){
            if (Selection.activeTransform)
            {
                isPageNumber = EditorGUILayout.Toggle("頁數點啟用", myScrollRectHelper.isPageNumber);
                myScrollRectHelper.isPageNumber = isPageNumber;

                EditorGUI.BeginDisabledGroup(isPageNumber == false);
                pageNumberGroup.objectReferenceValue = EditorGUILayout.ObjectField("群組物件", pageNumberGroup.objectReferenceValue, typeof(GameObject), true);
                pageNumber.objectReferenceValue = EditorGUILayout.ObjectField("頁數點樣式物件", pageNumber.objectReferenceValue, typeof(GameObject), false);

                m_currentPoint.objectReferenceValue = EditorGUILayout.ObjectField("目前頁數點的樣式", m_currentPoint.objectReferenceValue, typeof(Sprite));
                m_otherPages.objectReferenceValue = EditorGUILayout.ObjectField("其他頁數點的樣式", m_otherPages.objectReferenceValue, typeof(Sprite));
                EditorGUI.EndDisabledGroup();
            }
        }
        if (!Selection.activeTransform)
        {
            isPageNumberFolding = false;
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        
        #endregion
        
        #region 放大縮小
        
        isZoomInFolding = EditorGUILayout.BeginFoldoutHeaderGroup(isZoomInFolding , "ZoomIn");
        EditorGUILayout.EndFoldoutHeaderGroup();
        if(isZoomInFolding){
            if (Selection.activeTransform)
            {
                isZoomIn = EditorGUILayout.Toggle("大小縮放啟動", myScrollRectHelper.isZoomIn);
                myScrollRectHelper.isZoomIn = isZoomIn;
                
                EditorGUI.BeginDisabledGroup(isZoomIn == false);
                EditorGUILayout.PropertyField(equipment, new GUIContent("每頁大小比例"), true);
                EditorGUI.EndDisabledGroup();
            }
        }
        if (!Selection.activeTransform)
        {
            isZoomInFolding = false;
        }
        
        
        
        #endregion

    }
}
