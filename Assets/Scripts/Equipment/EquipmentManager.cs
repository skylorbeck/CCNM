using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentManager : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.inputReader.Back+=Back;
        GameManager.Instance.eventSystem.SetSelectedGameObject(GetComponentInChildren<Slider>().gameObject);

    }

    private void OnDestroy()
    {
        GameManager.Instance.inputReader.Back -= Back;
    }

    public void Back()
    {
        GameManager.Instance.LoadSceneAdditive("Hotel",false,"Equipment");
    }
}
