using DG.Tweening;
using EnhancedScrollerDemos.GridSimulation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class CarController : MonoBehaviour
{
    [SerializeField] private SkinCollection skinCollection;
    [SerializeField] private SkinCollection staticSkinCollection;
    [SerializeField] private SpriteRenderer skinSpriteRenderer;

    private bool canMove = false;
    private Vector3 backPoint;
    private Box box;
    private BaseBox baseBox;

    [HideInInspector] public Gradient tireGradient;
    [HideInInspector] public float speed;
    [HideInInspector] public bool isMoving = false;
    [HideInInspector] public float timeClick;
    public Direction direction = Direction.Left;

    public static event Action OnWin;
    public static event Action OnLose;
    public static event Action OnOneCarOut;
    public static event Action OnClickCar;
    public static event Action OnDisableHidden;
    public static event Action OnOutOfMoves;
    public static event Action OnUnder6Moves;
    public static event Action OnPlay;

    public static bool isUnlockedByKey = false;

    private List<GameObject> objTween;
    private void OnEnable()
    {
        SkinRowCellView.ChooseSkin += ChooseSkin;
    }
    private void OnDisable()
    {
        SkinRowCellView.ChooseSkin -= ChooseSkin;
    }
    private void Start()
    {
        isUnlockedByKey = false;
        box = GetComponent<BaseBox>().box;
        baseBox = gameObject.GetComponent<BaseBox>();
        ChooseSkin();
    }
    private void Update()
    {
        Movement();
    }
    private void OnMouseDown()
    {
        if (GameplayCanvasController.instance.onBombBooster)
        {
            GameplayCanvasController.instance.onBombBooster = false;
            skinSpriteRenderer.sprite = staticSkinCollection.skins[2].elements[0].up;
            skinSpriteRenderer.gameObject.transform.DOShakeScale(0.5f, 0.5f).OnComplete(() =>
            {
                GetComponent<BoosterController>().DestroyNearCar();
            });
        }
        if (GameplayCanvasController.instance.onHammerBooster)
        {
            GameplayCanvasController.instance.onHammerBooster = false;
            GetComponent<BoosterController>().DestroyItSelf();
        }
    }

    private void OnMouseDrag()
    {
        skinSpriteRenderer.transform.DOScale(0.9f, 0.2f).SetEase(Ease.Linear).OnComplete(() =>
        {
            skinSpriteRenderer.transform.localScale = Vector3.one * 0.9f;
        });
    }

    public void OnMouseUp()
    {
        skinSpriteRenderer.gameObject.transform.DOKill();
        skinSpriteRenderer.gameObject.transform.DOScale(1f, 0.2f).SetEase(Ease.Linear).OnComplete(() =>
        {
            skinSpriteRenderer.gameObject.transform.localScale = Vector3.one;
        });
        Debug.Log("CarController: - isUnlockedByKey = " + isUnlockedByKey);
        box = GetComponent<BaseBox>().box;
#if EDITORMODE
#else
        if (GameManager.instance.GameState != State.Play) return;
        OnPlay();
        PlayClickingSound(box);
        baseBox.HiddenBoxShaking();
        if (!CanMove()) return;
        if (isMoving) return;
        GameplayCanvasController.instance.currentNumOfMoves--;
        OnUnder6Moves();
#endif
        if (Constant.Lose())
        {
            GameManager.instance.GameState = State.Lose;
            OnLose();
            return;
        }
        backPoint = new Vector3(box.position.x, box.position.y, 0);
        if (IsOutCurPoint())
        {
            SetMovingBox(box);
            OnOneCarOut();
            PlayDrivingAwaySound();
        }
        isMoving = true;
        canMove = true;
        speed = Constant.speed;
        OnClickCar();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
#if EDITORMODE
        MapEditor.Instance.currentBoxes[(int)box.position.x][-(int)box.position.y].type = BlockType.Empty;
#else
        LevelManager.instance.currentBoxes[(int)box.position.x][-(int)box.position.y].type = BlockType.Empty;
        LevelManager.instance.currentBoxes[(int)box.position.x][-(int)box.position.y].isMoving = false;

        DropBomb();
        DropKey();
        DropChestKey();

        if (Constant.Win())
        {
            GameManager.instance.GameState = State.Win;
            OnWin();
            Debug.Log("CarController: OnWin()");
        }
        else if (Constant.CantWin())
        {
            GameManager.instance.GameState = State.Pause;
            StartCoroutine(RedBlinkListCar());
        }

#endif
        StartCoroutine(WaitToDestroy());
    }
    private IEnumerator RedBlinkListCar()
    {
        float delayTime = Constant.RedBlinkListCar(0);
        yield return new WaitForSeconds(delayTime);
        OnOutOfMoves();
    }

    private IEnumerator WaitToDestroy()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(this.gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        box = GetComponent<BaseBox>().box;
        Obstacle obstacle = collision.gameObject.GetComponent<BaseBox>().obstacle;

        Debug.Log("CarControll OnCollisionEnter2D");
        switch (obstacle)
        {
            case Obstacle.Default:
                if (isMoving)
                {
                    if (!collision.gameObject.GetComponent<CarController>().isMoving)
                    {
                        canMove = false;
                        UpdateCurrentPoint(collision.gameObject.GetComponent<BaseBox>().box);
                        BackCurrentPoint();
                        Shaking(direction, box);
                    }
                    else
                    {
                        if (IsBlockInFront(collision.gameObject.transform))
                        {
                            UpdateCurrentPoint();
                            BackCurrentPoint();
                            canMove = false;
                        }
                    }
                    PlayCollisionSound();
                    if (Constant.CantWin())
                    {
                        GameManager.instance.GameState = State.Pause;
                        StartCoroutine(RedBlinkListCar());
                    }
                }
                break;
        }
    }

    private void PlayClickingSound(Box box)
    {
        AudioManager.instance.Stop();
        if (box.isLockedByKey) AudioManager.instance.Play((int)SoundID.ClickingHiddenIronBox);
        else if (box.isLockedItSelf) AudioManager.instance.Play((int)SoundID.ClickingHiddenBox);
    }
    private void PlayDrivingAwaySound()
    {
        AudioManager.instance.Stop();
        if (DataMananger.instance.gameSave.currentSkinId == Constant.SKIN_AMBULANCE)
            AudioManager.instance.Play((int)SoundID.DrivingAwayAmbulance);
        else if (DataMananger.instance.gameSave.currentSkinId == Constant.SKIN_ROCKET)
            AudioManager.instance.Play((int)SoundID.DrivingAwayRocket);
        else if (DataMananger.instance.gameSave.currentSkinId == Constant.SKIN_UFO)
            AudioManager.instance.Play((int)SoundID.DrivingAwayUFO);
        else
            AudioManager.instance.Play((int)SoundID.DrivingAwayDefault);
    }
    private void PlayCollisionSound()
    {
        AudioManager.instance.Stop();
        AudioManager.instance.Play((int)SoundID.Collision);
    }
    public void Shaking(Direction direction, Box boxCollosion)
    {
        objTween = new List<GameObject>();
        List<Box> boxes = GetListByDirection(boxCollosion);

        float a = 0;

        float b = 0.05f;

        foreach (Box box in boxes)
        {
            if (box.isMoving) continue;
            if (box.type == BlockType.Empty || box.isLockedByKey || box.isLockedItSelf) break;
            switch (direction)
            {
                case Direction.Left:
                case Direction.Right:

                    if (objTween.Contains(box.GetTransform().GetChild(0).gameObject))
                        continue;

                    objTween.Add(box.GetTransform().GetChild(0).gameObject);
                    Vibrator.Vibrate((long)a);
                    box.GetTransform().GetChild(0).DOPunchPosition(Vector3.right * 0.1f, 0.5f).SetDelay(a).OnComplete(() =>
                    {
                        objTween.Remove(box.GetTransform().GetChild(0).gameObject);
                        box.GetTransform().GetChild(0).DOLocalMove(Vector3.zero, 0.5f);
                    });
                    a += b;
                    break;
                case Direction.Down:
                case Direction.Up:

                    if (objTween.Contains(box.GetTransform().GetChild(0).gameObject))
                        continue;

                    objTween.Add(box.GetTransform().GetChild(0).gameObject);
                    Vibrator.Vibrate((long)a);
                    box.GetTransform().GetChild(0).DOPunchPosition(Vector3.up * 0.1f, 0.5f).SetDelay(a).OnComplete(() =>
                    {
                        objTween.Remove(box.GetTransform().GetChild(0).gameObject);
                        box.GetTransform().GetChild(0).DOLocalMove(Vector3.zero, 0.5f);
                    });
                    a += b;
                    break;
            }
        }
        if ((box.isLockedByKey || box.isLockedItSelf))
        {
            //Some Anim
            return;
        }
    }

    private List<Box> GetListByDirection(Box boxColision)
    {
        List<Box> boxes = new List<Box>();
        List<List<Box>> list = LevelManager.instance.currentBoxes;

        for (int x = 0; x < list.Count; x++)
        {
            for (int y = 0; y < list[x].Count; y++)
            {
                if (boxColision.position == list[x][y].position) continue;
                switch (boxColision.type)
                {
                    case BlockType.Up:
                        if (boxColision.position.x == list[x][y].position.x && !list[x][y].isMoving
                            && boxColision.position.y < list[x][y].position.y)
                            boxes.Add(list[x][y]);
                        break;
                    case BlockType.Down:
                        if (boxColision.position.x == list[x][y].position.x && !list[x][y].isMoving
                            && boxColision.position.y > list[x][y].position.y)
                            boxes.Add(list[x][y]);
                        break;
                    case BlockType.Left:
                        if (boxColision.position.y == list[x][y].position.y && !list[x][y].isMoving
                            && boxColision.position.x > list[x][y].position.x)
                            boxes.Add(list[x][y]);
                        break;
                    case BlockType.Right:
                        if (boxColision.position.y == list[x][y].position.y && !list[x][y].isMoving
                            && boxColision.position.x < list[x][y].position.x)
                            boxes.Add(list[x][y]);
                        break;
                }
            }
        }
        if (boxColision.type == BlockType.Left || boxColision.type == BlockType.Up) boxes.Reverse();

        return boxes;
    }
    public void Movement()
    {
        if (canMove)
        {
            switch (direction)
            {
                case Direction.Up:
                    transform.Translate(Vector3.up * SpeedUp() * Time.deltaTime);
                    break;
                case Direction.Down:
                    transform.Translate(Vector3.down * SpeedUp() * Time.deltaTime);
                    break;
                case Direction.Left:
                    transform.Translate(Vector3.left * SpeedUp() * Time.deltaTime);
                    break;
                case Direction.Right:
                    transform.Translate(Vector3.right * SpeedUp() * Time.deltaTime);
                    break;
            }
        }
        else
        {
            isMoving = false;
            transform.Translate(Vector3.zero);
        }
    }
    private void SetMovingBox(Box box)
    {
#if EDITORMODE
        List<List<Box>> currentBoxes = MapEditor.Instance.currentBoxes;
#else
        List<List<Box>> currentBoxes = LevelManager.instance.currentBoxes;

#endif
        int x = (int)box.position.x;
        int y = -(int)box.position.y;

        currentBoxes[x][y].isMoving = true;
        box.isMoving = true;
        Debug.Log("CarController: SetMovingBox");
    }
    public bool IsOutCurPoint()
    {
#if EDITORMODE
        List<List<Box>> currentBoxes = MapEditor.Instance.currentBoxes;
#else
        List<List<Box>> currentBoxes = LevelManager.instance.currentBoxes;

#endif
        Vector2 currentMapSize = new Vector2(currentBoxes.Count, currentBoxes[0].Count);
        int x = (int)box.position.x;
        int y = -(int)box.position.y;

        switch (direction)
        {
            case Direction.Up:
                if (y - 1 < 0 || currentBoxes[x][y - 1].type == BlockType.Empty || currentBoxes[x][y - 1].isMoving)
                {
                    Debug.Log("CarController: - isOneCarOut = true");
                    return true;
                }
                break;
            case Direction.Down:
                if (y + 1 >= currentMapSize.y || currentBoxes[x][y + 1].type == BlockType.Empty || currentBoxes[x][y + 1].isMoving)
                {
                    Debug.Log("CarController: - isOneCarOut = true");
                    return true;
                }
                break;
            case Direction.Left:
                if (x - 1 < 0 || currentBoxes[x - 1][y].type == BlockType.Empty || currentBoxes[x - 1][y].isMoving)
                {
                    Debug.Log("CarController: - isOneCarOut = true");
                    return true;
                }
                break;
            case Direction.Right:
                if (x + 1 >= currentMapSize.x || currentBoxes[x + 1][y].type == BlockType.Empty || currentBoxes[x + 1][y].isMoving)
                {
                    Debug.Log("CarController: - isOneCarOut = true");
                    return true;
                }
                break;
        }
        return false;
    }
    public void BackCurrentPoint()
    {
        transform.position = Vector3.Lerp(transform.position, backPoint, speed * Time.deltaTime);
        transform.position = backPoint;
#if EDITORMODE
        List<List<Box>> currentBoxes = MapEditor.Instance.currentBoxes;
#else
        List<List<Box>> currentBoxes = LevelManager.instance.currentBoxes;
#endif
        Debug.Log("CarController: current box = " + box.position + " - " + box.type + "\n back point = " + backPoint);

        if (box.position.x != backPoint.x || box.position.y != backPoint.y)
        {
            currentBoxes[(int)backPoint.x][-(int)backPoint.y].type = box.type;
            currentBoxes[(int)box.position.x][-(int)box.position.y].type = BlockType.Empty;
        }
        currentBoxes[(int)box.position.x][-(int)box.position.y].isMoving = false;
        box.position = backPoint;
        box.isMoving = false;
        box.SetTransform(transform);
        currentBoxes[(int)box.position.x][-(int)box.position.y].hasBomb = box.hasBomb;
        currentBoxes[(int)box.position.x][-(int)box.position.y].hasKey = box.hasKey;
        currentBoxes[(int)box.position.x][-(int)box.position.y].bombCountDown = box.bombCountDown;
        currentBoxes[(int)box.position.x][-(int)box.position.y].SetTransform(transform);
    }

    private bool IsBlockInFront(Transform block)
    {
        float x = transform.position.x;
        float y = transform.position.y;
        float blockX = block.position.x;
        float blockY = block.position.y;

        switch (box.type)
        {
            case BlockType.Up:
                if (y < blockY)
                    return true;
                break;
            case BlockType.Down:
                if (y > blockY)
                    return true;
                break;
            case BlockType.Left:
                if (x > blockX)
                    return true;
                break;
            case BlockType.Right:
                if (x < blockX)
                    return true;
                break;
        }
        return false;
    }
    public void UpdateCurrentPoint()
    {
        Debug.Log("CarController: UpdateCurPoint");
        switch (direction)
        {
            case Direction.Left:
                backPoint.x = (float)Math.Floor(transform.position.x + 1f);
                backPoint.y = transform.position.y;
                break;
            case Direction.Right:
                backPoint.x = (float)Math.Floor(transform.position.x);
                backPoint.y = transform.position.y;
                break;
            case Direction.Up:
                backPoint.x = transform.position.x;
                backPoint.y = (float)Math.Floor(transform.position.y);
                break;
            case Direction.Down:
                backPoint.x = transform.position.x;
                backPoint.y = (float)Math.Floor(transform.position.y + 1f);
                break;
        }
    }
    public void UpdateCurrentPoint(Box colBox)
    {
        Debug.Log("CarController: - currentDirection = " + direction);
        Debug.Log("CarController: - currentPos = " + colBox.position);
        switch (direction)
        {
            case Direction.Left:
                backPoint.x = colBox.position.x + 1f;
                backPoint.y = colBox.position.y;
                break;
            case Direction.Right:
                backPoint.x = colBox.position.x - 1;
                backPoint.y = colBox.position.y;
                break;
            case Direction.Up:
                backPoint.x = colBox.position.x;
                backPoint.y = colBox.position.y - 1;
                break;
            case Direction.Down:
                backPoint.x = colBox.position.x;
                backPoint.y = colBox.position.y + 1;
                break;
        }
    }

    private float SpeedUp()
    {
        float firstSpeed = 0.25f;
        float accleration = 0.25f;
        return speed += (speed < 40f * firstSpeed) ? accleration : 25 * accleration;
    }

    public void ChooseSkin()
    {
        box = GetComponent<BaseBox>().box;
#if EDITORMODE

        switch (direction)
        {
            case Direction.Up:
                skinSpriteRenderer.sprite = staticSkinCollection.skins[0].up;
                break;
            case Direction.Down:
                skinSpriteRenderer.sprite = staticSkinCollection.skins[0].down;
                break;
            case Direction.Left:
                skinSpriteRenderer.sprite = staticSkinCollection.skins[0].left;
                break;
            case Direction.Right:
                skinSpriteRenderer.sprite = staticSkinCollection.skins[0].right;
                break;
        }
#else
        DataMananger.instance.LoadData();
        int elementRandomIndex = UnityEngine.Random.Range(0, skinCollection.skins[DataMananger.instance.gameSave.currentSkinId].elements.Count);
        foreach (Skin skin in skinCollection.skins)
        {
            if (DataMananger.instance.gameSave.currentSkinId == skin.id)
            {
                switch (direction)
                {
                    case Direction.Up:
                        skinSpriteRenderer.sprite = (box.hasBomb) ? staticSkinCollection.skins[1].elements[Constant.SKIN_DEFAULT].up
                                                                : skin.elements[elementRandomIndex].up;
                        break;
                    case Direction.Down:
                        skinSpriteRenderer.sprite = (box.hasBomb) ? staticSkinCollection.skins[1].elements[Constant.SKIN_DEFAULT].down
                                                                : skin.elements[elementRandomIndex].down;
                        break;
                    case Direction.Left:
                        skinSpriteRenderer.sprite = (box.hasBomb) ? staticSkinCollection.skins[1].elements[Constant.SKIN_DEFAULT].left
                                                                : skin.elements[elementRandomIndex].left;
                        break;
                    case Direction.Right:
                        skinSpriteRenderer.sprite = (box.hasBomb) ? staticSkinCollection.skins[1].elements[Constant.SKIN_DEFAULT].right
                                                                : skin.elements[elementRandomIndex].right;
                        break;
                }
            }
        }
        tireGradient = (box.hasBomb) ? staticSkinCollection.skins[1].elements[Constant.SKIN_DEFAULT].gradient : skinCollection.skins[DataMananger.instance.gameSave.currentSkinId].elements[elementRandomIndex].gradient;
#endif
    }

    private bool CanMove()
    {
        if (box.isLockedItSelf)
        {
            Debug.Log("CarController: - isUnLockedCountDown = " + (bool)(box.lockedCountDown == 0));
            return box.lockedCountDown == 0;
        }
        else if (box.isLockedByKey)
        {
            Debug.Log("CarController: -isLockedByKey = " + isUnlockedByKey);
            return isUnlockedByKey;
        }
        else return true;
    }

    public void DropKey()
    {
#if EDITORMODE
        List<List<Box>> currentBoxes = MapEditor.Instance.currentBoxes;
#else

        List<List<Box>> currentBoxes = LevelManager.instance.currentBoxes;
#endif
        if (box.hasKey)
        {
            baseBox.GetKeyController().Drop();
            isUnlockedByKey = true;
            OnDisableHidden();
            for (int x = 0; x < currentBoxes.Count; x++)
            {
                for (int y = 0; y < currentBoxes[x].Count; y++)
                {
                    if (currentBoxes[x][y].isLockedByKey)
                        currentBoxes[x][y].isLockedByKey = false;
                }
            }
        }
    }
    private void DropChestKey()
    {
        baseBox.GetChestKeyController().Drop();
    }
    public void DropBomb()
    {
        if (box.hasBomb)
        {
            if (box.bombCountDown > 0) { baseBox.GetBombController().Drop(); }
        }
    }

}
