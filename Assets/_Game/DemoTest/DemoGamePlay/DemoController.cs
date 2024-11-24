//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Array2DEditor;
//using UnityEngine.SceneManagement;
//using UnityEngine.UIElements;
//using TMPro;
//using UnityEditor;

//public class DemoController : MonoBehaviour
//{

//    //[SerializeField] private Array2DBool shape = null;
//    [SerializeField] private GameObject boxPrefab, sawPrefab, boxs, gameOverPanel, reloadBtn, nextLevelBtn;
//    [SerializeField] private DemoLevel demoLevel;
//    [SerializeField] private TextMeshProUGUI gameOverText;

//    private bool gameOver = false;

//    public static DemoController instance;

//    #region Current Level
//    private Box[] currentBoxs;
//    private Saw[] currentSaws;
//    private int currentLevelId = 0;
//    private LevelData currentLevelData;
//    #endregion

//    //private void Awake()
//    //{
//    //    if(instance == null)
//    //    {
//    //        instance = this;
//    //    }
//    //    else
//    //    {
//    //        Destroy(gameObject);
//    //    }
//    //    DontDestroyOnLoad(gameObject);
//    //}
//    private void Start()
//    {

//        if (PlayerPrefs.HasKey(Constant.CURRENT_LEVEL_ID))
//        {
//            currentLevelId = PlayerPrefs.GetInt(Constant.CURRENT_LEVEL_ID);
//        }
//        else { currentLevelId = 0; }

//        if (currentLevelId > 4)
//        {
//            gameOver = true;
//            reloadBtn.SetActive(false);
//            gameOverPanel.SetActive(true);
//            nextLevelBtn.SetActive(false);
//            gameOverText.text = "Next Level Comming Soon!!!";
//        }
//        else
//            SetUpMap();
//    }

//    public void SetUpMap()
//    {
//        #region Use Array2D
//        //if (shape == null || boxPrefab == null)
//        //{
//        //    Debug.LogError("Fill in all the fields in order to start this example");
//        //    return;
//        //}
//        //var cells = shape.GetCells();

//        //for (var y = 0; y < shape.GridSize.y; y++)
//        //{
//        //    for (var x = 0; x < shape.GridSize.x; x++)
//        //    {
//        //        if (!cells[y, x])
//        //        {
//        //            var prefabGO = Instantiate(boxPrefab, new Vector3(x, -y, 0), Quaternion.identity, boxs.transform);
//        //            prefabGO.name = $"({y}, {x})";
//        //        }
//        //    }
//        //}
//        #endregion

//        #region DemoLevel - Scriptable Object

//        currentLevelData = demoLevel.levels[currentLevelId];
//        currentBoxs = currentLevelData.boxs;

//        float sizex = currentLevelData.mapSize.x;
//        float sizey = currentLevelData.mapSize.y;

//        Camera.main.transform.position = new Vector3((float)(sizex - 1) / 2, ((float)(-(sizey - 1) / 2)), -10f);
//        Camera.main.orthographicSize = (sizex > 5f || sizey > 6f) ? Mathf.Max(sizex, sizey) : 5f;

//        //gen boxs
//        foreach (Box box in currentBoxs)
//        {
//            float x = box.position.x;
//            float y = box.position.y;
//            boxPrefab.GetComponent<BoxController>().direction = box.direction;
//            GameObject boxObj = Instantiate(boxPrefab, new Vector3(x, y, 0), Quaternion.identity, boxs.transform);
//            boxObj.name = $"({x}, {y})";
//        }
//        // gen saw
//        if (currentSaws == null) return;
//        foreach (Saw saw in currentSaws)
//        {
//            float x = saw.position.x;
//            float y = saw.position.y;
//            GameObject sawObj = Instantiate(sawPrefab, new Vector3(x, y, 0), Quaternion.identity, boxs.transform);
//            sawObj.name = $"saw - ({x}, {y})";
//        }

//        #endregion
//    }

//    [System.Obsolete]
//    private void LateUpdate()
//    {
//        if (boxs.transform.GetChildCount() < 1)
//        {
//            gameOver = true;
//            reloadBtn.SetActive(false);
//            gameOverPanel.SetActive(true);

//        }
//    }
//    public void OnClick2Restart()
//    {
//        currentLevelId += 1;
//        PlayerPrefs.SetInt(Constant.CURRENT_LEVEL_ID, currentLevelId);
//        SceneManager.LoadScene(0);
//    }
//    public void OnClick2Reload()
//    {
//        SceneManager.LoadScene(0);
//    }

//    public void OnClick2Quit()
//    {
//#if UNITY_EDITOR
//        EditorApplication.isPlaying = false;
//#else
//    Application.Quit();

//#endif
//    }

//    public void OnClick2ResetLevel()
//    {
//        currentLevelId = 0;
//        PlayerPrefs.SetInt(Constant.CURRENT_LEVEL_ID, currentLevelId);
//        SceneManager.LoadScene(0);
//    }
//    // Follow là:
//    // Start lần đầu thì 
//}//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Array2DEditor;
//using UnityEngine.SceneManagement;
//using UnityEngine.UIElements;
//using TMPro;
//using UnityEditor;

//public class DemoController : MonoBehaviour
//{

//    //[SerializeField] private Array2DBool shape = null;
//    [SerializeField] private GameObject boxPrefab, sawPrefab, boxs, gameOverPanel, reloadBtn, nextLevelBtn;
//    [SerializeField] private DemoLevel demoLevel;
//    [SerializeField] private TextMeshProUGUI gameOverText;

//    private bool gameOver = false;

//    public static DemoController instance;

//    #region Current Level
//    private Box[] currentBoxs;
//    private Saw[] currentSaws;
//    private int currentLevelId = 0;
//    private LevelData currentLevelData;
//    #endregion

//    //private void Awake()
//    //{
//    //    if(instance == null)
//    //    {
//    //        instance = this;
//    //    }
//    //    else
//    //    {
//    //        Destroy(gameObject);
//    //    }
//    //    DontDestroyOnLoad(gameObject);
//    //}
//    private void Start()
//    {

//        if (PlayerPrefs.HasKey(Constant.CURRENT_LEVEL_ID))
//        {
//            currentLevelId = PlayerPrefs.GetInt(Constant.CURRENT_LEVEL_ID);
//        }
//        else { currentLevelId = 0; }

//        if (currentLevelId > 4)
//        {
//            gameOver = true;
//            reloadBtn.SetActive(false);
//            gameOverPanel.SetActive(true);
//            nextLevelBtn.SetActive(false);
//            gameOverText.text = "Next Level Comming Soon!!!";
//        }
//        else
//            SetUpMap();
//    }

//    public void SetUpMap()
//    {
//        #region Use Array2D
//        //if (shape == null || boxPrefab == null)
//        //{
//        //    Debug.LogError("Fill in all the fields in order to start this example");
//        //    return;
//        //}
//        //var cells = shape.GetCells();

//        //for (var y = 0; y < shape.GridSize.y; y++)
//        //{
//        //    for (var x = 0; x < shape.GridSize.x; x++)
//        //    {
//        //        if (!cells[y, x])
//        //        {
//        //            var prefabGO = Instantiate(boxPrefab, new Vector3(x, -y, 0), Quaternion.identity, boxs.transform);
//        //            prefabGO.name = $"({y}, {x})";
//        //        }
//        //    }
//        //}
//        #endregion

//        #region DemoLevel - Scriptable Object

//        currentLevelData = demoLevel.levels[currentLevelId];
//        currentBoxs = currentLevelData.boxs;

//        float sizex = currentLevelData.mapSize.x;
//        float sizey = currentLevelData.mapSize.y;

//        Camera.main.transform.position = new Vector3((float)(sizex - 1) / 2, ((float)(-(sizey - 1) / 2)), -10f);
//        Camera.main.orthographicSize = (sizex > 5f || sizey > 6f) ? Mathf.Max(sizex, sizey) : 5f;

//        //gen boxs
//        foreach (Box box in currentBoxs)
//        {
//            float x = box.position.x;
//            float y = box.position.y;
//            boxPrefab.GetComponent<BoxController>().direction = box.direction;
//            GameObject boxObj = Instantiate(boxPrefab, new Vector3(x, y, 0), Quaternion.identity, boxs.transform);
//            boxObj.name = $"({x}, {y})";
//        }
//        // gen saw
//        if (currentSaws == null) return;
//        foreach (Saw saw in currentSaws)
//        {
//            float x = saw.position.x;
//            float y = saw.position.y;
//            GameObject sawObj = Instantiate(sawPrefab, new Vector3(x, y, 0), Quaternion.identity, boxs.transform);
//            sawObj.name = $"saw - ({x}, {y})";
//        }

//        #endregion
//    }

//    [System.Obsolete]
//    private void LateUpdate()
//    {
//        if (boxs.transform.GetChildCount() < 1)
//        {
//            gameOver = true;
//            reloadBtn.SetActive(false);
//            gameOverPanel.SetActive(true);

//        }
//    }
//    public void OnClick2Restart()
//    {
//        currentLevelId += 1;
//        PlayerPrefs.SetInt(Constant.CURRENT_LEVEL_ID, currentLevelId);
//        SceneManager.LoadScene(0);
//    }
//    public void OnClick2Reload()
//    {
//        SceneManager.LoadScene(0);
//    }

//    public void OnClick2Quit()
//    {
//#if UNITY_EDITOR
//        EditorApplication.isPlaying = false;
//#else
//    Application.Quit();

//#endif
//    }

//    public void OnClick2ResetLevel()
//    {
//        currentLevelId = 0;
//        PlayerPrefs.SetInt(Constant.CURRENT_LEVEL_ID, currentLevelId);
//        SceneManager.LoadScene(0);
//    }
//    // Follow là:
//    // Start lần đầu thì 
//}
