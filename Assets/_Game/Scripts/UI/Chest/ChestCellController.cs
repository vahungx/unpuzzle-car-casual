using AssetKits.ParticleImage;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChestCellController : MonoBehaviour
{
    [SerializeField] private GameObject reward;
    [SerializeField] private Image chest;
    [SerializeField] private Image rewardIcon;
    [SerializeField] private TextMeshProUGUI amount;

    [SerializeField] private ParticleImage rewardParticle;
    private Chest chestData;

    private void Start()
    {
        reward.SetActive(false);
        chest.gameObject.SetActive(true);
    }
    public void SetUp(Chest chestData)
    {
        this.chestData = chestData;

        rewardIcon.sprite = chestData.icon;
        amount.text = chestData.amount.ToString();
        chest.DOColor(new Color(1, 1, 1, 0), 0.5f).OnComplete(() =>
        {
            chest.gameObject.SetActive(false);
            Claiming();
            AudioManager.instance.Play((int)SoundID.Claiming);
        });
    }

    public void Claiming()
    {
        reward.SetActive(true);
        rewardParticle.rateOverTime = chestData.amount;
    }
}
