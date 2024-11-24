using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HammerPopupController : BasePopup
{
    [SerializeField] private GameObject outOfMovesPopupPrefab, handObj, adsObj, loseLevelBtn;
    [SerializeField] private float duration;
    [SerializeField] private Image fill, timer, background;
    [SerializeField] private Sprite lightCircle;
    private bool pause = false;
    private void Start()
    {
        if (OnTutorial())
        {
            SetUpOnTut();
            return;
        }
        StartCoroutine(UpdateTimer(duration));
    }

    private IEnumerator UpdateTimer(float remainingDuration)
    {
        if (pause) yield return null;
        while (remainingDuration >= 0)
        {
            fill.fillAmount = Mathf.InverseLerp(0, duration, remainingDuration);
            remainingDuration -= Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        OnEnd();
        yield return null;
    }

    private void OnEnd()
    {
        Instantiate(outOfMovesPopupPrefab, GameplayCanvasController.instance.transform);
    }

    public void OnClickHammer()
    {
        pause = true;
        Destroy(gameObject);
        DataMananger.instance.gameSave.hammerBooster++;
        GameplayCanvasController.instance.OnClickHammerBooster();
    }
    public override void Close()
    {
        base.Close();
        SceneManager.LoadScene((int)SceneID.Gameplay);
    }

    private bool OnTutorial()
    {
        DataMananger.instance.LoadData();
        return (DataMananger.instance.gameSave.firstOnTut);
    }

    private void SetUpOnTut()
    {
        handObj.SetActive(true);
        adsObj.SetActive(false);
        loseLevelBtn.SetActive(false);
        timer.sprite = lightCircle;
        fill.fillAmount = 1;
        background.color = new Color(0, 0, 0, 0.7f);
        DataMananger.instance.gameSave.firstOnTut = false;
        DataMananger.instance.SaveGame();
    }
}
