using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneAnim : MonoBehaviour
{
    private void OnApplicationQuit()
    {
        EnemyLibrary.Instance.SaveDictionary();
    }
}
