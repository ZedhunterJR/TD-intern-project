using System.Collections;
using UnityEngine;

public class StatusEffectCon : MonoBehaviour
{
    private Animator burn, wet, rock, crystal, mud;
    private Animator activeEffect = null; // Track the currently active effect

    private void Awake()
    {
        burn = transform.Find("burn").GetComponent<Animator>();
        wet = transform.Find("wet").GetComponent<Animator>();
        rock = transform.Find("rock_spike").GetComponent<Animator>();
        crystal = transform.Find("crystal").GetComponent<Animator>();
        mud = transform.Find("mud").GetComponent<Animator>();

        ResetAll();
    }

    public void ResetAll()
    {
        burn.gameObject.SetActive(false);
        wet.gameObject.SetActive(false);
        rock.gameObject.SetActive(false);
        crystal.gameObject.SetActive(false);
        mud.gameObject.SetActive(false);
        activeEffect = null;
    }

    private void ActivateEffect(Animator effect, string animation = null, bool hasExitEffect = false)
    {
        // If null is passed, deactivate any active effect
        if (effect == null)
        {
            if (activeEffect != null)
            {
                if (activeEffect == rock || activeEffect == crystal)
                {
                    PlayExitEffect(activeEffect);
                }
                else
                {
                    activeEffect.gameObject.SetActive(false);
                }
                activeEffect = null;
            }
            return;
        }

        // Deactivate the previous effect
        if (activeEffect != null && activeEffect != effect)
        {
            if (activeEffect == rock || activeEffect == crystal)
            {
                PlayExitEffect(activeEffect);
            }
            else
            {
                activeEffect.gameObject.SetActive(false);
            }
        }

        // Activate the new effect
        activeEffect = effect;
        activeEffect.gameObject.SetActive(true);

        if (!string.IsNullOrEmpty(animation) && gameObject.activeSelf)
        {
            activeEffect.SafePlay(animation);
        }
    }

    public void Burn() => ActivateEffect(burn);
    public void Wet() => ActivateEffect(wet);
    public void Rock() => ActivateEffect(rock, "rock", true);
    public void Crystal() => ActivateEffect(crystal, "crystal", true);
    public void Mud() => ActivateEffect(mud);
    public void ClearEffect() => ActivateEffect(null); // Clears any active effect

    private void PlayExitEffect(Animator effect)
    {
        // Placeholder for exit effect logic
        EnemyManager.Instance.StartCoroutine(DisableAfterAnimation(effect));
    }

    private IEnumerator DisableAfterAnimation(Animator effect)
    {
        if (effect == rock)
            rock.SafePlay("rock_exit");
        if (effect == crystal)
            crystal.SafePlay("crystal_exit");
        if (!effect.gameObject.activeInHierarchy) yield break;

        yield return new WaitForSeconds(effect.GetCurrentAnimatorStateInfo(0).length);
        effect.gameObject.SetActive(false);
    }
}
