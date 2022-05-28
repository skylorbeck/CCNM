using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;

public class TextPopController : MonoBehaviour
{
    public static TextPopController Instance;
    
    private ObjectPool<TextPop> popPool;
    [SerializeField] private TextPop prefab;

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

        popPool = new ObjectPool<TextPop>(
            () =>
            {
                TextPop textPop = Instantiate(prefab, transform);
                return textPop;
            },
            textPop =>
            {
                textPop.gameObject.SetActive(true);
            },
            textPop =>
            {
                textPop.gameObject.SetActive(false);
            },
            textPop => {
                Destroy(textPop);
            },
            true, 10, 20
        );
    }
    
    public async void PopDamage(int damage,Vector3 worldPos)
    {
        await CreatePop("-" + damage,TextPop.PopTypes.Damage,worldPos,true);
    }
    
    public async void PopHeal(int heal,Vector3 worldPos)
    {
        await CreatePop("+" + heal,TextPop.PopTypes.Heal,worldPos,true);
    }
    
    public async void PopPositive(string displayText,Vector3 worldPos, bool large)
    {
        await CreatePop(displayText,TextPop.PopTypes.Positive,worldPos,large);
    }
    
    public async void PopNegative(string displayText,Vector3 worldPos,bool large)
    {
        await CreatePop(displayText,TextPop.PopTypes.Negative,worldPos,large);
    }
    
    public async void PopCritical(int crit,Vector3 worldPos)
    {
        await CreatePop("-"+worldPos+"!",TextPop.PopTypes.Critical,worldPos,true);
    }
    
    public async void PopShield(int shield,Vector3 worldPos)
    {
        await CreatePop("{"+shield+"}",TextPop.PopTypes.Shield,worldPos,true);
    }

    private async Task CreatePop(string displayText,TextPop.PopTypes popType,Vector3 worldPos,bool large)
    {
        TextPop pop = popPool.Get();
        pop.Pop(displayText, popType, worldPos,large);
        do
        {
            await Task.Delay(100);
        } while (!pop.finished);
        popPool.Release(pop);
    }
}
