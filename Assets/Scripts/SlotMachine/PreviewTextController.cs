using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PreviewTextController : MonoBehaviour
{
    [SerializeField] private TextMeshPro title;
    [SerializeField] private TextMeshPro line1;
    [SerializeField] private TextMeshPro line2;

    public void SetText(string title, string line1, string line2)
    {
        this.title.text = title;
        this.line1.text = line1;
        this.line2.text = line2;
    }
}
