using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;
using System.IO;

public class FileWindow : MonoBehaviour
{
    public const string Name = "FileWindow";
    
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

    public static readonly Dictionary<FileWindowEntry.FileType, string> TypeToPermission = new (){
        {FileWindowEntry.FileType.Title, ""},
        {FileWindowEntry.FileType.UpDirectory, ""},
        {FileWindowEntry.FileType.Directory, "drw-r--r--"},
        {FileWindowEntry.FileType.Text, "-rw-r--r--"},
        {FileWindowEntry.FileType.Executable, "-rwxr-xr-x"}
    };

    // these are the entries that will always be at the top of the list, regardless of the contents of the directory.
    private static readonly List<FileWindowEntry> EntriesHeader = new (){
        new FileWindowEntry("Name", FileWindowEntry.FileType.Title, "Permissions"),
        new FileWindowEntry("/..", FileWindowEntry.FileType.UpDirectory, "")
    };

    private List<FileWindowEntry> Entries;

    public TextAsset fileStructure;
    public List<GameObject> EntryColumns = new ();
    public Transform tabsTransform;
    public GameObject entryTextPrefab;
    public GameObject selectionBarPrefab;
    public float maxSelectionBarY = -100;
    public float selectionBarSeparation = 50;
    public string topDirectoryName = "/home/griffinteller/";

    private int _selectionIndex = 0;
    private RectTransform _selectionBarTransform;
    private FileStructure.Directory _currentDirectory;


    public void Start()
    {
        _currentDirectory = LoadFileStructure();

        GameObject selectionBar = Instantiate(selectionBarPrefab, tabsTransform);
        _selectionBarTransform = selectionBar.GetComponent<RectTransform>();
        _selectionBarTransform.SetAsFirstSibling();

        RefreshDirectory();
    }

    private void RefreshDirectory()
    {
        LoadEntries();
        DrawEntries();
    }

    // load entries from structure file into FileStructure tree
    private void LoadEntries()
    {
        Entries = new List<FileWindowEntry>();
        Entries.AddRange(EntriesHeader);
        
        for (int i = 0; i < _currentDirectory.files.Count; i++)
        {
            FileStructure.File file = _currentDirectory.files[i];
            FileWindowEntry.FileType type;
            switch (file)
            {
                case FileStructure.Directory _:
                    type = FileWindowEntry.FileType.Directory;
                    break;
                
                case FileStructure.Text _:
                    type = FileWindowEntry.FileType.Text;
                    break;

                case FileStructure.Executable _:
                    type = FileWindowEntry.FileType.Executable;
                    break;

                default:
                    throw new ArgumentException("Unknown file type");
            }

            Entries.Add(new FileWindowEntry(file.name, type, TypeToPermission[type]));
        }
    }

    private void DrawEntries()
    {
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

        SetSelection(0);
    }

    private FileStructure.Directory LoadFileStructure()
    {
        FileStructure.Directory topDir = new(topDirectoryName, null);
        FileStructure.Directory currentDir = topDir;

        string[] lines = fileStructure.text.Split('\n');
        int lastIndent = 0;
        foreach (string rawLine in lines)
        {
            (int indent, string line) = GetIndent(rawLine);

            if (indent > lastIndent)
                currentDir = (FileStructure.Directory) currentDir.files[currentDir.files.Count - 1];

            else if (indent < lastIndent)
                for (int i = 0; i < lastIndent - indent; i++)
                    currentDir = currentDir.parent;

            lastIndent = indent;

            FileStructure.File newFile;
            if (line.Length > 1 && line[line.Length - 1] == '/')
                newFile = new FileStructure.Directory(line, currentDir);

            else if (line.Length > 4 && line.Substring(line.Length - 4) == ".txt")
                newFile = new FileStructure.Text(line);

            else
                newFile = new FileStructure.Executable(line);

            currentDir.AddFile(newFile);
        }

        return topDir;
    }

    private (int, string) GetIndent(string rawLine)
    {
        int spaces = 0;
        for (int i = 0; i < rawLine.Length; i++)
        {
            if (rawLine[i] == ' ')
                spaces++;
            else
                break;
        }

        return (spaces / 4, rawLine.Substring(spaces, rawLine.Length - spaces - 1)); // -1 is for windows line endings 😩
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

        if (Input.GetKeyDown(KeyCode.Return))
        {
            // +1 is because selection index of 0 is second entry (because of title entry)
            if (Entries[_selectionIndex + 1].type == FileWindowEntry.FileType.Directory)
            {
                _currentDirectory = (FileStructure.Directory) _currentDirectory.files[_selectionIndex - 1]; // -1 because up--dir is not in files
                RefreshDirectory();
            }

            else if (Entries[_selectionIndex + 1].type == FileWindowEntry.FileType.UpDirectory && _currentDirectory.parent != null)
            {
                _currentDirectory = _currentDirectory.parent;
                RefreshDirectory();
            }

            else
            {
                FileStructure.File file = _currentDirectory.files[_selectionIndex - 1];
                WindowManager.Instance.SelectWindow(_currentDirectory.path + file.name);
            }
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


// An entry in the current list of files and directories. FileWindow maintains a list of these,
// which are updated whenever the directory changes

[Serializable]
public class FileWindowEntry
{
    // this is an enum of types that show up in the "type" column 
    // they are not all actual file types
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


// these types are maintained in a tree that facilitates navigation
namespace FileStructure
{
    public abstract class File
    {
        public string name;

        public File(string name)
        {
            this.name = name;
        }
    }

    public class Directory : File
    {
        public string path;
        public Directory parent;
        public List<File> files = new ();

        public Directory(string name, Directory parent) : base(name)
        {
            this.parent = parent;
            path = parent?.path ?? "" + name;
        }

        public void AddFile(File file)
        {
            files.Add(file);
        }
    }

    public class Text : File
    {
        public Text(string name) : base(name)
        {
        }
    }

    public class Executable : File
    {
        public Executable(string name) : base(name)
        {
        }
    }
}
