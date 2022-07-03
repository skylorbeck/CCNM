using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventManager : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Image image;
    async void Start()
    {
        image.sprite = GameManager.Instance.battlefield.eventObject.eventImage;
        text.text = GameManager.Instance.battlefield.eventObject.eventDescription;
        await GameManager.Instance.battlefield.eventObject.StartEvent();
    }

    void Update()
    {
        GameManager.Instance.battlefield.eventObject.UpdateEvent();
    }

    void FixedUpdate()
    {
        GameManager.Instance.battlefield.eventObject.FixedUpdateEvent();
    }
}
