using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ProjectileAdvanced : MonoBehaviour
{
    //event
    public Action PreDestruct = null;
    public Action UpdateFunc = null;
    public Action<GameObject> HitEvent = null;

    //property
    public float lifeSpan = 1;
    public float speed = 10;
    public int pierce = 1;
    public string enemyTag = "enemy";

    //public modifier
    [HideInInspector] public Vector2 direction;
    [HideInInspector] public GameObject currentTarget;
    [HideInInspector] public List<GameObject> targetsHit = new();
    public float LifeSpanInInterpolation { get => 1 - lifeSpanCountDown / lifeSpan; }

    //private modifier
    private float lifeSpanCountDown;


    // Start is called before the first frame update
    void Start()
    {
        lifeSpanCountDown = lifeSpan;
    }

    // Update is called once per frame
    void Update()
    {
        lifeSpanCountDown -= Time.deltaTime;
        if (lifeSpanCountDown < 0)
        {
            DestroyObj();
        }
        //Debug.Log(InterpolateSpeed(LifeSpanInInterpolation));
        UpdateFunc?.Invoke();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(enemyTag))
        {
            targetsHit.Add(other.gameObject);
            HitEvent?.Invoke(other.gameObject.GetHighestParent());
            pierce--;
            if (pierce == 0)
            {
                DestroyObj();
            }
        }
    }

    private void DestroyObj()
    {
        PreDestruct?.Invoke();
        Destroy(gameObject);
    }

}

/*[CustomEditor(typeof(ProjectileAdvanced))]
public class ProjectileAdvancedEditor : Editor
{
    private SerializedProperty lifeSpanProp;
    private SerializedProperty pierceProp;
    private SerializedProperty enemyTagProp;
    private SerializedProperty speedProp;

    private void OnEnable()
    {
        // Link serialized properties
        lifeSpanProp = serializedObject.FindProperty("lifeSpan");
        pierceProp = serializedObject.FindProperty("pierce");
        enemyTagProp = serializedObject.FindProperty("enemyTag");
        speedProp = serializedObject.FindProperty("speed");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Expose the "property" fields
        EditorGUILayout.LabelField("Projectile Properties", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(lifeSpanProp, new GUIContent("Life Span"));
        EditorGUILayout.PropertyField(pierceProp, new GUIContent("Pierce"));
        EditorGUILayout.PropertyField(enemyTagProp, new GUIContent("Enemy Tag"));

        // Speed over lifetime list
        EditorGUILayout.LabelField("Speed Over Lifetime", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(speedProp, new GUIContent("Speed/Life List"), true);

        serializedObject.ApplyModifiedProperties();
    }
}*/