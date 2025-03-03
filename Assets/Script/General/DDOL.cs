using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DDOL : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(this);
        SceneManager.sceneLoaded += (s, m) =>
        {
            TryGetComponent<Canvas>(out Canvas c);
            if (c != null)
            {
                c.worldCamera = Camera.main;
                c.sortingLayerName = "UI";
            }
        };
    }

}
