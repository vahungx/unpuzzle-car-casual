using System.Collections.Generic;
[System.Serializable]
public class GameSave
{
    //------------ State -----------
    public bool isNew = true;
    public bool music = true;
    public bool sounds = true;
    public bool vibrate = true;
    public bool isNoAds = false;
    public bool isFirstTime = true;
    public bool isSetAge = false;
    public bool isRated = false;
    public int gameMode = 0;
    //------------ Value -----------
    public int currentDiamonds = 0;
    public int currentLevel = 0;
    //------------ Firebase --------
    //------------ Other -----------
    public bool isCheating = true;
    //------------ Skin ------------
    public int currentSkinId = 0;// skin đang select
    public List<Skin> currentSkins = new List<Skin>(); // list skinid đã onwed hoặc canget
    //------------ Booster ---------
    public int bombBooster = 3;
    public int hammerBooster = 3;
    //------------ Other -----------
    public int chestKey = 0;
    public string luckySpinStartCountDown;
    public bool firstSpin = true;
    public bool firstOnTut = true;
    public bool[] claimedDailys = new bool[7];
    public bool isResetedDailys = false;
    //------------ Timer -----------
    public string installTime;
    public int dayPlayed = 0;
    public GameSave()
    {
        isNew = true;
        music = true;
        sounds = true;
        vibrate = true;
        isNoAds = false;
        isCheating = true;
        isFirstTime = true;
        isSetAge = false;
        isRated = false;
        gameMode = 0;

        bombBooster = 3;
        hammerBooster = 3;
        chestKey = 0;

        dayPlayed = 0;

        firstSpin = true;
        firstOnTut = true;
        currentDiamonds = 0;
        currentLevel = 0;

        currentSkinId = 0;
        currentSkins = new List<Skin>
        {
            new Skin()
            {
                id = currentSkinId,
                state = SkinState.Onwed,
            }
        };
    }
}
