using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DDOLChangeScene : MonoBehaviour
{
    private void Start()
    {
        SceneManager.LoadScene("Title");
    }
}
