using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;

public class DamageAnimator : MonoBehaviour
{

    [SerializeField] private Animator redFlash;
    public static DamageAnimator Instance;
    
    private ObjectPool<AttackAnimator> attackPool;
    [SerializeField] private AttackAnimator prefab;
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        attackPool = new ObjectPool<AttackAnimator>(
            () =>
            {
                AttackAnimator animator = Instantiate(prefab, transform);
                return animator;
            },
            animator =>
            {
                animator.gameObject.SetActive(true);
            },
            animator =>
            {
                animator.gameObject.SetActive(false);
            },
            animator => {
                Destroy(animator);
            },
            true, 10, 20
        );
    }

    public void TriggerAttack(Shell target, AttackAnimator.AttackType attackType)
    {
        if (target.isPlayer)
        {
            redFlash.SetTrigger("hurt");
            if (attackType != AttackAnimator.AttackType.None)
            {
                TriggerAttack(target.transform.position, attackType, 0.5f);
            }
        }
        else
        {
            if (attackType != AttackAnimator.AttackType.None)
            {
                TriggerAttack(target.transform.position,attackType,0.25f);
            }
        }
    }

    public async void TriggerAttack(Vector3 position, AttackAnimator.AttackType attackType, float scale)
    {
        AttackAnimator animator = attackPool.Get();
        Transform aniTran = animator.transform;
        aniTran.position = position;
        aniTran.localScale = Vector3.one * scale;
        animator.Play(attackType, scale);

        do
        {
            await Task.Delay(100);
        } while (!animator.ended);
        attackPool.Release(animator);
    }

    public void TriggerRedFlash()
    {
        redFlash.SetTrigger("hurt");
    }
}
