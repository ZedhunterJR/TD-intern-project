using UnityEngine;
using UnityEngine.UI;

public class TabManager : MonoBehaviour
{
    public GameObject[] tabContents; // Các Panel nội dung
    public Button[] tabButtons; // Các nút tab

    private int currentTab = 0;

    [SerializeField] private Sprite active, deactive; // Sprites cho trạng thái Active/Deactive

    void Start()
    {
        // Đặt tất cả các nút về trạng thái Deactive
        for (int i = 0; i < tabButtons.Length; i++)
        {
            SetButtonSprite(tabButtons[i], false);
            int index = i;
            tabButtons[i].onClick.AddListener(() => OpenTab(index));
        }

        // Mở tab đầu tiên và set nút đầu tiên là Active
        OpenTab(0);
    }

    public void OpenTab(int tabIndex)
    {
        // Ẩn tất cả nội dung tab và đặt lại trạng thái các nút
        for (int i = 0; i < tabContents.Length; i++)
        {
            tabContents[i].SetActive(i == tabIndex);
            SetButtonSprite(tabButtons[i], i == tabIndex);
        }
    }

    private void SetButtonSprite(Button button, bool isActive)
    {
        if (button != null)
        {
            button.image.sprite = isActive ? active : deactive;
        }
    }
}
