namespace MediaMetaDataManager.Models;

public class DirectoryChangeModel
{
    private string _currentDirectory = Environment.CurrentDirectory;
    public string CurrentDirectory { 
        get { return _currentDirectory; } 
        set { 
            if (!String.IsNullOrWhiteSpace(value))
            { 
                _currentDirectory = value;
            }
        } 
    }
    public string? RequestedNewDirectory { get; set; }
    public bool RequestedNewDirectoryValidity { get; set; }
}
