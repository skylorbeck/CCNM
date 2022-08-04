using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class GenericMenuV1 : MonoBehaviour
{
    [SerializeField] private MenuEntry menuPrefab;
    [SerializeField] private Mode mode = Mode.FullWheel;

    [SerializeField] private GenericMenuEntry[] entries;
    [SerializeField] private List<MenuEntry> menuEntries = new List<MenuEntry>();
    [SerializeField] private float yDistance = 3f;
    [SerializeField] private float xDistance = 1.5f;
    [SerializeField] private float offset = 0f;
    [SerializeField] private float stickiness = 1f;
    [field:SerializeField] public int selected{ get; private set; }
    async void Start()
    {
        await Task.Delay(10);
        Init();
    }

    private void Init()
    {
        GameManager.Instance.inputReader.Drag += OnDrag;

        foreach (GenericMenuEntry generic in entries)
        {
            MenuEntry entry = Instantiate(menuPrefab, transform);
            entry.InsertData(generic.description, generic.icon);
            menuEntries.Add(entry);
        }
    }

    void Update()
    {
        if (menuEntries.Count == 0)
            return;
        float target;
        int newSelected;
        switch (mode)
        {
            case Mode.FullWheel:
                target = (float)(Math.Round(offset / menuEntries.Count, MidpointRounding.AwayFromZero)) *
                         (menuEntries.Count + 0.25f);

                offset = Mathf.Lerp(offset, target, Time.deltaTime * menuEntries.Count);

                newSelected = (int)Math.Round(offset / menuEntries.Count, MidpointRounding.AwayFromZero);

                break;
            default:
            case Mode.LeftWheel:
            case Mode.RightWheel:
                target = (float)(Math.Round(offset / yDistance, MidpointRounding.AwayFromZero) * yDistance);
                offset = Mathf.Clamp(Mathf.Lerp(offset, target, Time.deltaTime * yDistance * stickiness),
                    yDistance - menuEntries.Count * yDistance, 0);

                newSelected = Mathf.Abs((int)Math.Round(offset / yDistance, MidpointRounding.AwayFromZero));
                break;
            case Mode.TopWheel:
            case Mode.BottomWheel:
                target = (float)(Math.Round(offset / xDistance, MidpointRounding.AwayFromZero) * xDistance);
                offset = Mathf.Clamp(Mathf.Lerp(offset, target, Time.deltaTime * xDistance * stickiness),
                    xDistance - menuEntries.Count * xDistance, 0);

                newSelected = Mathf.Abs((int)Math.Round(offset / xDistance, MidpointRounding.AwayFromZero));
                break;
        }


        if (newSelected != selected)
        {
            selected = newSelected;
        }

        for (var i = 0; i < menuEntries.Count; i++)
        {
            MenuEntry menuEntry = menuEntries[i];
            Transform entryTransform = menuEntry.transform;
            Vector3 entryPosition = entryTransform.position;

            switch (mode)
            {
                case Mode.FullWheel:
                    float angle = i * Mathf.PI * 2 / menuEntries.Count;
                    angle += offset / menuEntries.Count;
                    entryPosition.y = Mathf.Sin(angle) * yDistance;
                    entryPosition.x = Mathf.Cos(angle) * xDistance;
                    break;
                case Mode.RightWheel:
                    entryPosition.y = (i * yDistance) + offset;
                    entryPosition.x = Mathf.Cos(Mathf.Abs(entryPosition.y * 0.5f)) * xDistance;
                    break;
                case Mode.LeftWheel:
                    entryPosition.y = (i * yDistance) + offset;
                    entryPosition.x = -Mathf.Cos(Mathf.Abs(entryPosition.y * 0.5f)) * xDistance;
                    break;
                case Mode.TopWheel:
                    entryPosition.x = (i * xDistance) + offset;
                    entryPosition.y = Mathf.Cos(Mathf.Abs(entryPosition.x * 0.5f)) * yDistance;
                    break;
                case Mode.BottomWheel:
                    entryPosition.x = (i * xDistance) + offset;
                    entryPosition.y = -Mathf.Cos(Mathf.Abs(entryPosition.x * 0.5f)) * yDistance;
                    break;
            }

            entryTransform.localPosition = entryPosition;
        }
    }

    public void OnDrag(Vector2 delta)
    {
        switch (mode)
        {
            case Mode.FullWheel:
            case Mode.TopWheel:
            case Mode.BottomWheel:
                offset += delta.x * Time.deltaTime * PlayerPrefs.GetFloat("TouchSensitivity", 1f);
                break;
            case Mode.LeftWheel:
            case Mode.RightWheel:
                offset += delta.y * Time.deltaTime * PlayerPrefs.GetFloat("TouchSensitivity", 1f);
                break;
        }
    }

    private void OnDestroy()
    {
        GameManager.Instance.inputReader.Drag -= OnDrag;
    }

    private enum Mode
    {
        RightWheel,
        LeftWheel,
        TopWheel,
        BottomWheel,
        FullWheel
    }
    
    public void InvokeSelected()
    {
        entries[selected].method.Invoke();
    }
}
[Serializable]
public class GenericMenuEntry
{
    public string name;
    public string description;
    public Sprite icon;
    public UnityEvent method;
}
