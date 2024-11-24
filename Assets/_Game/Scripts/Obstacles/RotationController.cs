//using DG.Tweening;
//using System;
//using System.Collections.Generic;
//using Unity.VisualScripting;
//using UnityEditor.Experimental.GraphView;
//using UnityEngine;
//using UnityEngine.UIElements;

//public class RotationController : BaseBox
//{
//    [SerializeField] Transform rotationTransform;
//    [SerializeField] BoxCollider2D[] boxC2Ds;
//    public GameObject up, right, down, left;
//    private bool isRotating = false;
//    private bool canRotate = true;
//    private bool isFirstClick = true;
//    private bool[] isUnparent = new bool[8];
//    private Quaternion orignalRotation = Quaternion.Euler(0, 0, 0);

//    Box lastUpBox, lastRightBox, lastDownBox, lastLeftBox;
//    private void OnEnable()
//    {
//        up.GetComponent<CarController>().OnMovingFirstPoint.AddListener(Unparent);
//        right.GetComponent<CarController>().OnMovingFirstPoint.AddListener(Unparent);
//        down.GetComponent<CarController>().OnMovingFirstPoint.AddListener(Unparent);
//        left.GetComponent<CarController>().OnMovingFirstPoint.AddListener(Unparent);

//    }

//    private void Start()
//    {
//        Debug.Log("RotationController: orignalRotation: " + orignalRotation);
//    }
//    private void OnMouseUp()
//    {
//        if (isFirstClick)
//        {
//#if EDITORMODE
//            List<List<Box>> currentBoxes = MapEditor.Instance.currentBoxes;
//#else
//            List<List<Box>> currentBoxes = LevelManager.instance.currentBoxes;
//#endif

//            Vector2 currentMapSize = new Vector2(currentBoxes.Count, currentBoxes[0].Count);
//            Box Empty = new Box()
//            {
//                type = BlockType.Random,
//                position = Vector2.zero,
//            };

//            int x = (int)box.position.x;
//            int y = -(int)box.position.y;

//            lastUpBox = (y - 1 < 0) ? Empty : new Box() { type = currentBoxes[x][y - 1].type, position = currentBoxes[x][y - 1].position };
//            lastRightBox = (x + 1 >= currentMapSize.x) ? Empty : new Box() { type = currentBoxes[x + 1][y].type, position = currentBoxes[x + 1][y].position };
//            lastDownBox = (y + 1 >= currentMapSize.y) ? Empty : new Box() { type = currentBoxes[x][y + 1].type, position = currentBoxes[x][y + 1].position };
//            lastLeftBox = (x - 1 < 0) ? Empty : new Box() { type = currentBoxes[x - 1][y].type, position = currentBoxes[x - 1][y].position };
//            Debug.Log("RotationController: - lastUpBox.position = " + lastLeftBox.position +
//                "\nlastRightBox.postion = " + lastRightBox.position + "\nlastDownBox.position = " + lastDownBox.position + "\nlastLeftBox.position = " + lastLeftBox.position);
//            isFirstClick = false;
//        }
//        if (!isRotating)
//        {
//            if (boxC2Ds[(int)RotObj.Up] != null && !isUnparent[(int)RotObj.Up]) boxC2Ds[(int)RotObj.Up].enabled = false;
//            if (boxC2Ds[(int)RotObj.Right] != null && !isUnparent[(int)RotObj.Right]) boxC2Ds[(int)RotObj.Right].enabled = false;
//            if (boxC2Ds[(int)RotObj.Down] != null && !isUnparent[(int)RotObj.Down]) boxC2Ds[(int)RotObj.Down].enabled = false;
//            if (boxC2Ds[(int)RotObj.Left] != null && !isUnparent[(int)RotObj.Left]) boxC2Ds[(int)RotObj.Left].enabled = false;
//            up.SetActive(false);
//            right.SetActive(false);
//            down.SetActive(false);
//            left.SetActive(false);
//            Rotate();
//        }
//        Debug.Log("RotationController: On Mouse Up at " + Time.time);
//    }

//    private void Update()
//    {
//        if (isRotating)
//        {
//            DontChangeRotElesRotation();
//            CarController.isProcessing = true;
//        }
//        else
//        {
//            CarController.isProcessing = false;
//        }
//        DontChangeRotElesRotation();
//    }
//    private void Rotate()
//    {
//        if (!canRotate) return;
//        isRotating = true;
//        Vector3 rotationEulerAngle = ChooseRotationAngle();
//        Vector3 targetRotation = rotationTransform.eulerAngles + rotationEulerAngle;

//        Tweener tweener = rotationTransform.DORotate(targetRotation, 0.5f).OnComplete(() =>
//        {
//            if (boxC2Ds[(int)RotObj.Up] != null && !isUnparent[(int)RotObj.Up]) boxC2Ds[(int)RotObj.Up].enabled = true;
//            if (boxC2Ds[(int)RotObj.Right] != null && !isUnparent[(int)RotObj.Right]) boxC2Ds[(int)RotObj.Right].enabled = true;
//            if (boxC2Ds[(int)RotObj.Down] != null && !isUnparent[(int)RotObj.Down]) boxC2Ds[(int)RotObj.Down].enabled = true;
//            if (boxC2Ds[(int)RotObj.Left] != null && !isUnparent[(int)RotObj.Left]) boxC2Ds[(int)RotObj.Left].enabled = true;
//            up.SetActive(true);
//            right.SetActive(true);
//            down.SetActive(true);
//            left.SetActive(true);

//            isRotating = false;
//        });
//        UpdateAfterRotating(rotationEulerAngle);
//    }
//    private Vector3 ChooseRotationAngle()
//    {
//#if EDITORMODE
//        List<List<Box>> listBoxs = MapEditor.Instance.currentBoxes; //?
//        Debug.Log("RotationController: listBoxs size (x,y): " + listBoxs.Count + ", " + listBoxs[0].Count);
//#else
//        List<List<Box>> listBoxs = LevelManager.instance.currentBoxes;
//#endif
//        RotElement[] rotEs = new RotElement[8];

//        Vector2 currentMapSize = new Vector2(listBoxs.Count, listBoxs[0].Count);

//        int x = (int)box.position.x;
//        int y = -(int)box.position.y;

//        rotEs[(int)RotObj.Right] = (x + 1 >= currentMapSize.x) ? RotElement.Blocked : CheckRotBox(listBoxs[x + 1][y].type);
//        rotEs[(int)RotObj.Down] = (y + 1 >= currentMapSize.y) ? RotElement.Blocked : CheckRotBox(listBoxs[x][y + 1].type);
//        rotEs[(int)RotObj.Up] = (y - 1 < 0) ? RotElement.Blocked : CheckRotBox(listBoxs[x][y - 1].type);
//        rotEs[(int)RotObj.Left] = (x - 1 < 0) ? RotElement.Blocked : CheckRotBox(listBoxs[x - 1][y].type);
//        rotEs[(int)RotObj.S_Right] = (x + 2 >= currentMapSize.x) ? RotElement.Blocked : CheckRotBox(listBoxs[x + 2][y].type);
//        rotEs[(int)RotObj.S_Down] = (y + 2 >= currentMapSize.y) ? RotElement.Blocked : CheckRotBox(listBoxs[x][y + 2].type);
//        rotEs[(int)RotObj.S_Up] = (y - 2 <= 0) ? RotElement.Blocked : CheckRotBox(listBoxs[x][y - 2].type);
//        rotEs[(int)RotObj.S_Left] = (x - 2 <= 0) ? RotElement.Blocked : CheckRotBox(listBoxs[x - 2][y].type);

//        bool canRotate = false;

//        for (int i = 0; i < rotEs.Length; i++)
//        {
//            Debug.Log("RotationController: rotEs-" + i + " = " + rotEs[i]);
//        }

//        // Check Rotate 90
//        for (int i = 0; i < rotEs.Length - 4; i++)
//        {
//            RotElement curEle = rotEs[i];
//            RotElement nextEle = (i < 3) ? rotEs[i + 1] : rotEs[0];

//            if (CanSwapRotElement(curEle, nextEle)) canRotate = true;
//            else { canRotate = false; break; }
//        }
//        if (canRotate)
//        {
//            Debug.Log("RotationController: Euler Angles = 90");
//            return new Vector3(0, 0, -90f);
//        }
//        //Check Rotate 180
//        if (!canRotate)
//        {
//            if (CanSwapRotElement(rotEs[(int)RotObj.Up], rotEs[(int)RotObj.Down]) && CanSwapRotElement(rotEs[(int)RotObj.Down], rotEs[(int)RotObj.Up]) &&
//                CanSwapRotElement(rotEs[(int)RotObj.Left], rotEs[(int)RotObj.Right]) && CanSwapRotElement(rotEs[(int)RotObj.Right], rotEs[(int)RotObj.Left]))
//            {
//                canRotate = true;
//                Debug.Log("RotationController: Euler Angles = 180");
//                return new Vector3(0, 0, -180);
//            }
//        }
//        //Check Rotate 270
//        if (!canRotate)
//        {
//            for (int i = 0; i < rotEs.Length - 4; i++)
//            {
//                RotElement curEle = rotEs[i];
//                RotElement nextEle = (i > 0) ? rotEs[i - 1] : rotEs[i + 3];

//                if (CanSwapRotElement(curEle, nextEle)) canRotate = true;
//                else { canRotate = false; break; }
//            }
//        }
//        if (canRotate)
//        {
//            Debug.Log("RotationController: Euler Angles = 270");
//            return new Vector3(0, 0, -270f);
//        }

//        Debug.Log("RotationController: Euler Angles = 0");
//        return Vector3.zero;
//    }

//    private RotElement CheckRotBox(BlockType type)
//    {
//        switch (type)
//        {
//            case BlockType.Rotation:
//            case BlockType.Up:
//            case BlockType.Right:
//            case BlockType.Down:
//            case BlockType.Left:
//            case BlockType.Random:
//                return RotElement.Blocked;
//            case BlockType.RotUp:
//            case BlockType.RotDown:
//            case BlockType.RotLeft:
//            case BlockType.RotRight:
//                return RotElement.UnBlocked;
//            case BlockType.Empty:
//                return RotElement.Empty;
//        }
//        return RotElement.Blocked;
//    }

//    private bool CanSwapRotElement(RotElement rotA, RotElement rotB)
//    {
//        switch (rotB)
//        {
//            case RotElement.Empty:
//            case RotElement.UnBlocked:
//                return true;
//            case RotElement.Blocked:
//                switch (rotA)
//                {
//                    case RotElement.Empty:
//                    case RotElement.Blocked:
//                        return true;
//                }
//                break;
//        }
//        return false;
//    }

//    private void Unparent(CarController carCtrl)
//    {
//#if EDITORMODE
//        if (carCtrl == null) return;
//        carCtrl.gameObject.transform.SetParent(MapEditor.Instance.levelContainer.transform);
//#else
//#endif
//        if (carCtrl.gameObject == up)
//        {
//            isUnparent[(int)RotObj.Up] = true;
//            Debug.Log("RotationController: Unparent " + carCtrl.gameObject.name);
//        }
//        if (carCtrl.gameObject == down)
//        {
//            isUnparent[(int)RotObj.Down] = true;
//            Debug.Log("RotationController: Unparent " + carCtrl.gameObject.name);
//        }
//        if (carCtrl.gameObject == left)
//        {
//            isUnparent[(int)RotObj.Left] = true;
//            Debug.Log("RotationController: Unparent " + carCtrl.gameObject.name);
//        }
//        if (carCtrl.gameObject == right)
//        {
//            isUnparent[(int)RotObj.Right] = true;
//            Debug.Log("RotationController: Unparent " + carCtrl.gameObject.name);
//        }
//    }

//    private void UpdateAfterRotating(Vector3 eulerAngle)
//    {
//#if EDITORMODE
//        List<List<Box>> listBoxs = MapEditor.Instance.currentBoxes; //?
//        Debug.Log("RotationController: listBoxs size (x,y): " + listBoxs.Count + ", " + listBoxs[0].Count);
//#else
//        List<List<Box>> listBoxs = LevelManager.instance.currentBoxes; //?
//#endif
//        int x = (int)box.position.x;
//        int y = -(int)box.position.y;
//        Box rotUpBox = (!isUnparent[(int)RotObj.Up] && up != null && up.activeSelf) ? up.gameObject.GetComponent<BaseBox>().box : null;
//        Box rotRightBox = (!isUnparent[(int)RotObj.Right] && right != null && right.activeSelf) ? right.gameObject.GetComponent<BaseBox>().box : null;
//        Box rotDownBox = (!isUnparent[(int)RotObj.Down] && down != null && down.activeSelf & down.activeSelf) ? down.gameObject.GetComponent<BaseBox>().box : null;
//        Box rotLeftBox = (!isUnparent[(int)RotObj.Left] && left != null && left.activeSelf) ? left.gameObject.GetComponent<BaseBox>().box : null;

//        Debug.Log("RotationController: List Box Element: \n"
//                                                + "up: type = " + listBoxs[x][y - 1].type + " - pos = " + listBoxs[x][y - 1].position + "\n"
//                                                + "right: type = " + listBoxs[x + 1][y].type + " - pos = " + listBoxs[x + 1][y].position + "\n"
//                                                + "down: type = " + listBoxs[x][y + 1].type + " - pos = " + listBoxs[x][y + 1].position + "\n"
//                                                + "left: type = " + listBoxs[x - 1][y].type + " - pos = " + listBoxs[x - 1][y].position + "\n");

//        UpdateCurrentBoxesByEulerAngleZ(listBoxs[x][y - 1], listBoxs[x + 1][y], listBoxs[x][y + 1], listBoxs[x - 1][y], eulerAngle.z);

//        Debug.Log("RotationController: After Rotating, List Box Element: \n"
//                                                + "up: type = " + listBoxs[x][y - 1].type + " - pos = " + listBoxs[x][y - 1].position + "\n"
//                                                + "right: type = " + listBoxs[x + 1][y].type + " - pos = " + listBoxs[x + 1][y].position + "\n"
//                                                + "down: type = " + listBoxs[x][y + 1].type + " - pos = " + listBoxs[x][y + 1].position + "\n"
//                                                + "left: type = " + listBoxs[x - 1][y].type + " - pos = " + listBoxs[x - 1][y].position + "\n");
//        UpdateRotBoxByAngleZ(lastUpBox, lastRightBox, lastDownBox, lastLeftBox, eulerAngle.z);
//        if (rotUpBox != null) rotUpBox.position = lastUpBox.position;
//        if (rotRightBox != null) rotRightBox.position = lastRightBox.position;
//        if (rotDownBox != null) rotDownBox.position = lastDownBox.position;
//        if (rotLeftBox != null) rotLeftBox.position = lastLeftBox.position;
//    }

//    private void UpdateCurrentBoxesByEulerAngleZ(Box upBox, Box rightBox, Box downBox, Box leftBox, float posZ)
//    {
//        Debug.Log("RotationController: - UpdateCurBoxes - List Boxes: \n" +
//            "up: " + upBox.type + "\nright: " + rightBox.type + "\ndown: " + downBox.type + "\nleft: " + leftBox.type);
//        BlockType upTemp_type = upBox.type, rightTemp_type = rightBox.type, leftTemp_type = leftBox.type, downTemp_type = downBox.type;
//        switch (posZ)
//        {
//            case 0f:
//                // Nothing to updates
//                break;
//            case -90f:
//                // Update theo tung con
//                if (leftBox != null) upBox.type = UpdateType(leftTemp_type, upTemp_type, upBox.type);
//                if (upBox != null) rightBox.type = UpdateType(upTemp_type, rightTemp_type, rightBox.type);
//                if (rightBox != null) downBox.type = UpdateType(rightTemp_type, downTemp_type, downBox.type);
//                if (downBox != null) leftBox.type = UpdateType(downTemp_type, leftTemp_type, leftBox.type);
//                break;
//            case -180f:
//                //Update theo tung cap doi xung
//                if (upBox != null) downBox.type = UpdateType(upTemp_type, downTemp_type, downBox.type);
//                if (rightBox != null) leftBox.type = UpdateType(rightTemp_type, leftTemp_type, leftBox.type);
//                if (downBox != null) upBox.type = UpdateType(downTemp_type, upTemp_type, upBox.type);
//                if (leftBox != null) rightBox.type = UpdateType(leftTemp_type, rightTemp_type, rightBox.type);
//                break;
//            case -270f:
//                //Update theo tung con
//                if (upBox != null) leftBox.type = UpdateType(upTemp_type, leftTemp_type, leftBox.type);
//                if (rightBox != null) upBox.type = UpdateType(rightTemp_type, upTemp_type, upBox.type);
//                if (downBox != null) rightBox.type = UpdateType(downTemp_type, rightTemp_type, rightBox.type);
//                if (leftBox != null) downBox.type = UpdateType(leftTemp_type, downTemp_type, downBox.type);
//                break;
//        }
//        Debug.Log("RotationController: - UpdateCurBoxes - After Updating, List Boxes: \n" +
//            "up: " + upBox.type + "\nright: " + rightBox.type + "\ndown: " + downBox.type + "\nleft: " + leftBox.type);
//    }
//    private BlockType UpdateType(BlockType rotA, BlockType rotB, BlockType rotTarget)
//    {
//        rotTarget = BlockType.Empty;
//        switch (rotB)
//        {
//            case BlockType.Up:
//            case BlockType.Down:
//            case BlockType.Left:
//            case BlockType.Right:
//            case BlockType.Random:
//            case BlockType.Rotation:
//                rotTarget = rotB; break;
//            case BlockType.Empty:
//                switch (rotA)
//                {
//                    case BlockType.RotUp:
//                    case BlockType.RotDown:
//                    case BlockType.RotRight:
//                    case BlockType.RotLeft:
//                    case BlockType.Empty:
//                        rotTarget = rotA;
//                        break;
//                    case BlockType.Up:
//                    case BlockType.Down:
//                    case BlockType.Left:
//                    case BlockType.Right:
//                    case BlockType.Random:
//                    case BlockType.Rotation:
//                        rotTarget = rotB;
//                        break;
//                }
//                break;
//            case BlockType.RotUp:
//            case BlockType.RotDown:
//            case BlockType.RotLeft:
//            case BlockType.RotRight:
//                switch (rotA)
//                {
//                    case BlockType.RotUp:
//                    case BlockType.RotDown:
//                    case BlockType.RotRight:
//                    case BlockType.RotLeft:
//                    case BlockType.Empty:
//                        rotTarget = rotA;
//                        break;
//                    case BlockType.Up:
//                    case BlockType.Down:
//                    case BlockType.Left:
//                    case BlockType.Right:
//                    case BlockType.Random:
//                    case BlockType.Rotation:
//                        rotTarget = BlockType.Empty;
//                        break;
//                }
//                break;
//        }
//        return rotTarget;
//    }
//    private void UpdateRotBoxByAngleZ(Box upBox, Box rightBox, Box downBox, Box leftBox, float posZ)
//    {
//        Vector2 upTemp_pos = upBox.position,
//            rightTemp_pos = rightBox.position,
//            leftTemp_pos = leftBox.position,
//            downTemp_pos = downBox.position;
//        switch (posZ)
//        {
//            case 0f:
//                // Nothing to update
//                break;
//            case -90f:
//                // Update theo tung con
//                if (upBox != null)
//                {
//                    upBox.position = rightTemp_pos;
//                }
//                if (rightBox != null)
//                {
//                    rightBox.position = downTemp_pos;
//                }
//                if (downBox != null)
//                {
//                    downBox.position = leftTemp_pos;
//                }
//                if (leftBox != null)
//                {
//                    leftBox.position = upTemp_pos;
//                }

//                break;
//            case -180f:
//                if (downBox != null)
//                {
//                    downBox.position = upTemp_pos;
//                }
//                if (leftBox != null)
//                {
//                    leftBox.position = rightTemp_pos;
//                }
//                if (upBox != null)
//                {
//                    upBox.position = downTemp_pos;
//                }
//                if (rightBox != null)
//                {
//                    rightBox.position = leftTemp_pos;
//                }
//                break;
//            case -270f:
//                if (leftBox != null)
//                {
//                    leftBox.position = downTemp_pos;
//                }
//                if (upBox != null)
//                {
//                    upBox.position = leftTemp_pos;
//                }
//                if (rightBox != null)
//                {
//                    rightBox.position = upTemp_pos;
//                }
//                if (downBox != null)
//                {
//                    downBox.position = rightTemp_pos;
//                }
//                break;
//        }
//    }

//    private void DontChangeRotElesRotation()
//    {
//        if (!isUnparent[(int)RotObj.Up] && up != null && up.activeSelf) up.transform.rotation = orignalRotation;
//        if (!isUnparent[(int)RotObj.Down] && down != null && down.activeSelf & down.activeSelf) down.transform.rotation = orignalRotation;
//        if (!isUnparent[(int)RotObj.Right] && right != null && right.activeSelf) right.transform.rotation = orignalRotation;
//        if (!isUnparent[(int)RotObj.Left] && left != null && left.activeSelf) left.transform.rotation = orignalRotation;

//        Debug.Log("RotationController: - DontChangeRotElesRotation");
//    }
//    private bool CanChangeType(BlockType rotA, BlockType rotB)
//    {
//        switch (rotA)
//        {
//            case BlockType.Up:
//            case BlockType.Down:
//            case BlockType.Left:
//            case BlockType.Right:
//            case BlockType.Random:
//            case BlockType.Rotation:
//                return false;
//            case BlockType.Empty:
//            case BlockType.RotUp:
//            case BlockType.RotDown:
//            case BlockType.RotLeft:
//            case BlockType.RotRight:
//                switch (rotB)
//                {
//                    case BlockType.Up:
//                    case BlockType.Down:
//                    case BlockType.Left:
//                    case BlockType.Right:
//                    case BlockType.Random:
//                    case BlockType.Rotation:
//                        return false;
//                }
//                break;
//        }
//        return true;
//    }
//}

//enum RotObj
//{
//    Up = 0, Right, Down, Left, S_Up, S_Right, S_Down, S_Left
//}
//enum RotElement
//{
//    Blocked, UnBlocked, Empty
//}