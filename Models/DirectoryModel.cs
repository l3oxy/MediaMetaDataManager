namespace MediaMetaDataManager.Models;

public class DirectoryModel
{
    public DirectoryModel() 
    {

    }

    public DirectoryModel(DirectoryInfo? CurrentDirectory = null, DirectoryInfo? RequestedNewDirectory = null) 
    {
        this.CurrentDirectory = CurrentDirectory ?? new("");
        this.RequestedNewDirectory = RequestedNewDirectory;
    }

    public DirectoryModel(string? CurrentDirectoryFullPath = null, string? RequestedNewDirectoryFullPath = null) 
    {
        if (!String.IsNullOrWhiteSpace(CurrentDirectoryFullPath))
        {
            this.CurrentDirectory = new(CurrentDirectoryFullPath);
        }
        
        if (!String.IsNullOrWhiteSpace(RequestedNewDirectoryFullPath))
        {
            this.RequestedNewDirectory = new(RequestedNewDirectoryFullPath);
        }
        
    }

    /// <summary>
    /// The current directory of media that this program is operating on.
    /// Defaults to personal music folder, but should be overwritten with user's actual choice.
    /// </summary>
    private DirectoryInfo _currentDirectory = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic));
    public DirectoryInfo CurrentDirectory { 
        get { return _currentDirectory; } 
        set {
            // TODO: should probs also check that we have write permissions to this dir (I mean, theoretically maybe user just wants to read metadata, so maybe just show a message idk)
            if (value is not null && value.Exists)
            {
                _currentDirectory = value;
            }
        } 
    }

    /// <summary>The directory (of media) that the user is requesting to operate on.</summary>
    public DirectoryInfo? RequestedNewDirectory { get; set; }
    
}
