using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using RocketGM;


public class SceneLoader : MonoBehaviour
{
    public Slider loadingSlider;
    public int lastLevel;
    [SerializeField] private GameObject updateReqPanel;
    private bool isUpdateRequired;

    private void Awake()
    {
        RocGmControllerBase.UpdateRequired += OnUpdateRequired;
    }
    public void OnUpdateRequired()
    {
        isUpdateRequired = true;
        updateReqPanel.SetActive(true);

    }
    public void OnOkButtonClicked()
    {
//#if ROC_GM_REPORTING
//        RocGm.OpenStorePage();
//#endif
    }
    void Start()
    {
        if (!isUpdateRequired)
        {
            lastLevel = PlayerPrefs.GetInt("lastLevel", 1);

            if (SceneController.instance.firstLogin)
            {
                StartCoroutine(AsyncSceneLoader((lastLevel % 4) == 0 ? 4 : (lastLevel % 4)));
                SceneController.instance.firstLogin = false;
            }
            else
                StartCoroutine(AsyncSceneLoader(SceneController.instance.levelIndex));
        }
    }

    IEnumerator AsyncSceneLoader(int BuildIndex)
    {
        AsyncOperation asyncLoadScene = SceneManager.LoadSceneAsync(BuildIndex, LoadSceneMode.Single);

        while (!asyncLoadScene.isDone)
        {
            loadingSlider.value = asyncLoadScene.progress;
            yield return null;
        }
    }
}