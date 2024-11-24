using EnhancedScrollerDemos.GridSimulation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MotionEffect : MonoBehaviour
{
    [SerializeField] CarController carController;
    [SerializeField] GameObject smoke, tire;

    [SerializeField] TrailRenderer leftTrailRenderer, rightTrailRenderer;
    [SerializeField] SkinCollection skinCollection;

    private void OnEnable()
    {
        SkinRowCellView.ChooseSkin += SetTrailColor;
    }
    private void OnDisable()
    {
        SkinRowCellView.ChooseSkin -= SetTrailColor;
    }
    void Start()
    {
        Direction direction = carController.direction;
        RotateByDirection(direction);
        SetTrailColor();
    }

    void Update()
    {
        SetActiveMotionEffect();
    }
    private void RotateByDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.Left:
                transform.Rotate(0, 0, 90f);
                break;
            case Direction.Right:
                transform.Rotate(0, 0, -90f);
                break;
            case Direction.Up:
                transform.Rotate(0, 0, 0);
                break;
            case Direction.Down:
                transform.Rotate(0, 0, 180f);
                break;

        }
    }

    public void SetTrailColor()
    {
        DataMananger.instance.LoadData();
        rightTrailRenderer.material = new Material(Shader.Find("Sprites/Default"));
        leftTrailRenderer.material = new Material(Shader.Find("Sprites/Default"));
        rightTrailRenderer.colorGradient = carController.tireGradient;
        leftTrailRenderer.colorGradient = carController.tireGradient;
    }

    private void SetActiveMotionEffect()
    {
        if (carController.isMoving)
        {
            smoke.SetActive(true);
            tire.SetActive(true);
        }
        else
        {
            smoke.SetActive(false);
            tire.SetActive(false);
        }
    }
}
