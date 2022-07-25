using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;


public class FileWindow : MonoBehaviour
{
    public static readonly Dictionary<FileWindowEntry.FileType, string> TypeToString = new (){
        {FileWindowEntry.FileType.Title, "Type"},
        {FileWindowEntry.FileType.UpDirectory, "UP--DIR"},
        {FileWindowEntry.FileType.Directory, "DIR"},
        {FileWindowEntry.FileType.Text, "TXT"},
        {FileWindowEntry.FileType.Executable, "EXEC"}
    };

    public static readonly Dictionary<FileWindowEntry.FileType, Color> TypeToColor = new (){
        {FileWindowEntry.FileType.Title, Utils.ColorFromHex(0xF5B973FF)},
        {FileWindowEntry.FileType.UpDirectory, Utils.ColorFromHex(0x9DD400FF)},
        {FileWindowEntry.FileType.Directory, Utils.ColorFromHex(0x9DD400FF)},
        {FileWindowEntry.FileType.Text, Utils.ColorFromHex(0x7BE7FFFF)},
        {FileWindowEntry.FileType.Executable, Utils.ColorFromHex(0x7BE7FFFF)}
    };

    public static readonly List<FileWindowEntry> Entries = new (){
        new FileWindowEntry("Name", FileWindowEntry.FileType.Title, "Permissions"),
        new FileWindowEntry("/..", FileWindowEntry.FileType.UpDirectory, ""),
        new FileWindowEntry("/Projects", FileWindowEntry.FileType.Directory, "drwxr--r--"),
        new FileWindowEntry("about.txt", FileWindowEntry.FileType.Text, "-rwxr--r--"),
        new FileWindowEntry("gorilla", FileWindowEntry.FileType.Executable, "-rwxr-xr-x")
    };

    public List<GameObject> EntryColumns = new ();
    public Transform tabsTransform;
    public GameObject entryTextPrefab;
    public GameObject selectionBarPrefab;
    public float maxSelectionBarY = -100;
    public float selectionBarSeparation = 50;

    private int _selectionIndex = 0;
    private RectTransform _selectionBarTransform;


    public void Start()
    {
        //loop through EntryColumns and delete all children of each column
        foreach (GameObject column in EntryColumns)
        {
            foreach (Transform child in column.transform)
            {
                Destroy(child.gameObject);
            }
        }

        foreach (FileWindowEntry entry in Entries)
        {
            string[] entryData = entry.GetData();
            for (int i = 0; i < entryData.Length; i++)
            {
                GameObject column = EntryColumns[i];
                GameObject textObj = Instantiate(entryTextPrefab, column.transform);
                TextMeshProUGUI text = textObj.GetComponent<TextMeshProUGUI>();
                text.text = entryData[i];
                text.color = TypeToColor[entry.type];
            }
        }

        GameObject selectionBar = Instantiate(selectionBarPrefab, tabsTransform);
        _selectionBarTransform = selectionBar.GetComponent<RectTransform>();
        _selectionBarTransform.SetAsFirstSibling();

    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            SetSelection(Mathf.Max(0, _selectionIndex - 1));
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            SetSelection(Mathf.Min(Entries.Count - 2, _selectionIndex + 1));
        }
    }

    public void SetSelection(int index)
    {
        _selectionIndex = index;
        Vector3 pos = _selectionBarTransform.anchoredPosition;
        pos.y = maxSelectionBarY - selectionBarSeparation * _selectionIndex;
        _selectionBarTransform.anchoredPosition = pos;
    }
}


[Serializable]
public class FileWindowEntry
{
    public enum FileType
    {
        Title,
        UpDirectory,
        Directory,
        Text,
        Executable
    }

    public string name;
    public FileType type;
    public string permissions;

    //create constructor
    public FileWindowEntry(string name, FileType type, string permissions)
    {
        this.name = name;
        this.type = type;
        this.permissions = permissions;
    }

    public string[] GetData()
    {
        return new [] { name, FileWindow.TypeToString[type], permissions };
    }
}
