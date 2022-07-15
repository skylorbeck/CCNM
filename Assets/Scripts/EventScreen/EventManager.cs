using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventManager : MonoBehaviour
{
    [SerializeField] private Event currentEvent;

    [field: SerializeField] public string[] eventNames { get; private set; }

    void Start()
    {
        if (currentEvent == null)
        {
            currentEvent = Instantiate(Resources
                .Load<GameObject>("Events/" + eventNames[Random.Range(0, eventNames.Length)]).GetComponent<Event>(),transform);
        }

        currentEvent.Init(this);
    }


    public void MoveOn()
    {
        GameManager.Instance.battlefield.TotalHandsPlus();
        GameManager.Instance.saveManager.SaveRun();

        GameManager.Instance.LoadSceneAdditive("MapScreen", "EventScreen");
    }
}