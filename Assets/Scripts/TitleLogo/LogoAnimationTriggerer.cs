using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class LogoAnimationTriggerer : MonoBehaviour
{
    float timer = 0;
    bool hasTriggeredLetters = false;
    [SerializeField] float timeToWait = 5;
    Animator[] animators;
    Animator logo;
    [SerializeField] private Animator buttons;

    async void Start()
    {
        List<Animator> animators = new List<Animator>(GetComponentsInChildren<Animator>());
        animators.Remove(GetComponent<Animator>());
        this.animators = animators.ToArray();
        await Task.Delay(200);
        logo = GetComponent<Animator>();
        logo.SetTrigger("Enter");
        GameManager.Instance.inputReader.PushAnyButton += Skip;
    }

    private void Skip()
    {
        logo.SetTrigger("Skip");
        TriggerLogoEnter();
        FadeInButtons();
    }

    void Update()
    {
        if (hasTriggeredLetters)
        {
            timer += Time.deltaTime;
            if (timer > timeToWait)
            {
                timer = 0;
                TriggerBounce();
            }
        }
    }

    void FixedUpdate()
    {

    }

    public async void TriggerLogoEnter()
    {
        hasTriggeredLetters = true;
        foreach (Animator animator in animators)
        {
            await Task.Delay(100);
            if (animator != null)  animator.SetTrigger("Enter");
        }
    }

    public async void TriggerBounce()
    {
        foreach (Animator animator in animators)
        {
            await Task.Delay(100);
            if (animator != null) animator.SetTrigger("Bounce");
        }
    }

    public void FadeInButtons()
    {
        GameManager.Instance.inputReader.PushAnyButton -= Skip;
        buttons!.SetTrigger("FadeIn");
    }
}
