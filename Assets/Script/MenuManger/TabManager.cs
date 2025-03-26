using UnityEngine;
using UnityEngine.UI;

public class TabManager : MonoBehaviour
{
    public GameObject[] tabContents; // Các Panel nội dung
    public Button[] tabButtons; // Các nút tab

    private int currentTab = 0;

    void Start()
    {
        // Đăng ký sự kiện click cho các nút tab
        for (int i = 0; i < tabButtons.Length; i++)
        {
            int index = i;
            tabButtons[i].onClick.AddListener(() => OpenTab(index));
        }

        OpenTab(0); // Mở tab đầu tiên mặc định
    }

    public void OpenTab(int tabIndex)
    {
        // Ẩn tất cả các tab
        for (int i = 0; i < tabContents.Length; i++)
        {
            tabContents[i].SetActive(i == tabIndex);
        }
    }
}
