using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class LogoAnimationTriggerer : MonoBehaviour
{
    float _timer = 0;
    bool _hasTriggeredLetters = false;
    [SerializeField] float timeToWait = 5;
    Animator[] _animators;
    [SerializeField] private Animator buttons;

    void Start()
    {
        List<Animator> animators = new List<Animator>(GetComponentsInChildren<Animator>());
        animators.Remove(GetComponent<Animator>());
        _animators = animators.ToArray();
    }

    void Update()
    {
        if (_hasTriggeredLetters)
        {
            _timer += Time.deltaTime;
            if (_timer > timeToWait)
            {
                _timer = 0;
                TriggerBounce();
            }
        }
    }

    void FixedUpdate()
    {

    }

    public async void TriggerLogoEnter()
    {
        _hasTriggeredLetters = true;
        foreach (Animator animator in _animators)
        {
            await Task.Delay(100);
            if (animator != null)  animator.SetTrigger("Enter");
        }
    }

    public async void TriggerBounce()
    {
        foreach (Animator animator in _animators)
        {
            await Task.Delay(100);
            if (animator != null) animator.SetTrigger("Bounce");
        }
    }

    public void FadeInButtons()
    {
        buttons!.SetTrigger("FadeIn");
    }
}
