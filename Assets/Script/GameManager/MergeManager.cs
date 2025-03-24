using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MergeManager : Singleton<MergeManager>
{
    private Queue<ButtonUI> listButtons = new();

    [SerializeField] private GameObject mergeButtonPrefabs;
    [SerializeField] private Transform worldCanvas;
    [SerializeField] private LineRenderer arrowRenderer;

    //all of this serve dragging function
    ButtonUI currentDragingButton = null;
    private float buttonDragUpdateTimer = 0f;
    private bool canFireButtonDrag = false;
    private TowerStat hoveingTower;

    private void Start()
    {
        CreatePool(10);
    }
    private void Update()
    {
        if (!canFireButtonDrag)
        {
            buttonDragUpdateTimer += Time.deltaTime;
            if (buttonDragUpdateTimer > 1/30f )
            {
                buttonDragUpdateTimer = 0;
                canFireButtonDrag = true;
            }
        }
    }
    private void CreatePool(int num)
    {
        for (int i = 0; i < num; i++)
        {
            GameObject instane = Instantiate(mergeButtonPrefabs, worldCanvas);
            instane.SetActive(false);
            listButtons.Enqueue(instane.GetComponent<ButtonUI>());
        }
    }

    public ButtonUI GetFromPool()
    {
        if(listButtons.Count <= 0)
        {
            CreatePool(5);
            return GetFromPool();
        }

        ButtonUI buttonUI = listButtons.Dequeue();
        return buttonUI;
    }

    public void ReturnButtonUI(ButtonUI buttonUI)
    {
        buttonUI.gameObject.SetActive(false);
        listButtons.Enqueue(buttonUI);
    }

    public void SetButtonPos(Vector2 pos)
    {
        ButtonUI buttonUI = GetFromPool();
        buttonUI.ResetButtonUI();
        buttonUI.gameObject.SetActive(true);

        buttonUI.transform.position = pos;

        buttonUI.ClickFunc = () => 
        { 
           
           
        };
        buttonUI.MouseDragBegin = () =>
        {
            //Debug.Log($"Begin = {buttonUI.transform.position}");
            currentDragingButton = buttonUI;
            hoveingTower = null;

            var tower = TileManager.Instance.GetTowerStatInTile(buttonUI.transform.position);
            tower.ShowRange(Color.white);
            //Debug.Log($"currentDragingButton {currentDragingButton == null}");
        };

        buttonUI.MouseDragEnd = () =>
        {
            currentDragingButton = null;
            arrowRenderer.positionCount = 0;
            var tower = TileManager.Instance.GetTowerStatInTile(buttonUI.transform.position);
            tower.HideRange();
        };

        buttonUI.MouseDrop = () =>
        {
            if (currentDragingButton == null) return;
            //Debug.Log("Have Button");

            if (currentDragingButton == buttonUI) return;
            //Debug.Log("Different");

            TowerStat towerBegin = TileManager.Instance.GetTowerStatInTile(currentDragingButton.transform.position);
            //Debug.Log($"Tower Begin = {towerBegin == null}");

            TowerStat towerEnd = TileManager.Instance.GetTowerStatInTile(buttonUI.transform.position);
            //Debug.Log($"Tower End = {towerEnd == null}");

            if(towerBegin.CanMerge(towerEnd))
            {
                TileManager.Instance.RemoveTileInDic(currentDragingButton.transform.position);
                ReturnButtonUI(currentDragingButton);

                towerEnd.LevelUp();
                towerEnd.HideRange();
            }

            towerEnd.HideRange();
            currentDragingButton = null;
            arrowRenderer.positionCount = 0;
        };
        buttonUI.MouseDrag = () =>
        {
            if (!canFireButtonDrag) return;

            //also this need to get touch position as well
            if (Input.touchCount > 0 || Input.GetMouseButton(0))
            {
                Vector3 touchPosition = Input.touchCount > 0 ?
                    (Vector3)Input.GetTouch(0).position :
                    Input.mousePosition;

                //draw
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(touchPosition);
                DrawArrow(buttonUI.transform.position, worldPosition);

                // Check if we are over a button
                PointerEventData eventData = new PointerEventData(EventSystem.current)
                {
                    position = touchPosition
                };
                //raycasting, hopefully if only 60 times a sec, wont be any issue
                List<RaycastResult> results = new List<RaycastResult>();
                EventSystem.current.RaycastAll(eventData, results);

                bool found = false;
                foreach (var hit in results)
                {
                    ButtonUI button = hit.gameObject.GetComponent<ButtonUI>();
                    if (button != null && buttonUI != button)
                    {
                        TowerStat towerEnd = TileManager.Instance.GetTowerStatInTile(button.transform.position);
                        TowerStat towerBegin = TileManager.Instance.GetTowerStatInTile(buttonUI.transform.position);
                        if (towerEnd != null)
                        {
                            if (towerEnd != hoveingTower)
                            {
                                if (hoveingTower != null)
                                    hoveingTower.HideRange();
                                bool canMerge = towerEnd.CanMerge(towerBegin);
                                towerEnd.ShowRange(canMerge? Color.green : Color.red);
                                hoveingTower = towerEnd;
                            }
                            found = true;
                        }
                    }
                    break;
                }
                if (!found && hoveingTower != null)
                {
                    hoveingTower.HideRange();
                    hoveingTower = null;
                }
            }

            canFireButtonDrag = false;
        };
    }

    private List<Vector3> GetQuadraticBezierPoints(Vector2 start, Vector2 control, Vector2 end, int resolution = 20)
    {
        List<Vector3> points = new List<Vector3>();

        for (int i = 0; i <= resolution; i++)
        {
            float t = i / (float)resolution;
            Vector2 point = Mathf.Pow(1 - t, 2) * start +
                            2 * (1 - t) * t * control +
                            Mathf.Pow(t, 2) * end;
            points.Add(point);
        }

        return points;
    }

    private void DrawArrow(Vector2 start, Vector2 end)
    {
        float distance = Vector2.Distance(start, end);
        var cHeight = Mathf.Clamp(distance, 4f, 8f);
        var control = (start + end) / 2 + new Vector2(0, cHeight); // Scale curve height with distance

        var points = GetQuadraticBezierPoints(start, control, end);
        arrowRenderer.positionCount = points.Count;
        for (int i = 0; i < points.Count; i++)
        {
            arrowRenderer.SetPosition(i, points[i]);
        }
    }


}
