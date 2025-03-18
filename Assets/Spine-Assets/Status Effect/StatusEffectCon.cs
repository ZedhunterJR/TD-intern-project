using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectCon : MonoBehaviour
{
    private Animator burn, wet, rock, crystal, mud, combust;

    private void Awake()
    {
        burn = transform.Find("burn").GetComponent<Animator>();
        wet = transform.Find("wet").GetComponent <Animator>();
        rock = transform.Find("rock_spike").GetComponent<Animator>();
        crystal = transform.Find("crystal").GetComponent<Animator>();
        mud = transform.Find("mud").GetComponent<Animator>();

        SetActiveFalse();
    }
    public void SetActiveFalse()
    {
        burn.gameObject.SetActive(false);
        wet.gameObject.SetActive(false);
        rock.gameObject.SetActive(false);
        crystal.gameObject.SetActive(false);
        mud.gameObject.SetActive(false);
    }
    public void Burn()
    {
        SetActiveFalse();
        burn.gameObject.SetActive(true);
    }
    public void Wet()
    {
        SetActiveFalse();
        wet.gameObject.SetActive(true);
    }
    public void Rock()
    {
        SetActiveFalse();
        rock.gameObject.SetActive(true);
        rock.Play("rock");
    }
    public void Crystal()
    {
        SetActiveFalse();
        crystal.gameObject.SetActive(true);
        crystal.Play("crystal");
    }
    public void Mud()
    {
        SetActiveFalse();
        mud.gameObject.SetActive(true);
    }
}
