using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;

public class NotificationPopController : MonoBehaviour
{
    public static NotificationPopController Instance;
    
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
    
    public async void PopNotification(string value,Vector3 worldPos)
    {
        await CreatePop(value,TextPop.PopTypes.Notification,worldPos,false);
    } 
    public async void PopCredits(string value,Vector3 worldPos)
    {
        await CreatePop(value,TextPop.PopTypes.GotCredits,worldPos,false);
    } 
    public async void PopEgo(string value,Vector3 worldPos)
    {
        await CreatePop(value,TextPop.PopTypes.GotEgo,worldPos,false);
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
