namespace MediaMetaDataManager.ViewModels;

public class FilesViewModel
{
    private DirectoryInfo? _directory = null;
    /// <summary>
    ///     The current directory of media that this program is operating on.
    ///     Defaults to personal music folder, but should be overwritten with user's actual choice.
    ///     dir preference: 
    ///      1. model-binding (e.g. GET param) 
    ///      2. session variable
    ///      3. whatever the OS tells us the music folder is
    /// </summary>
    public DirectoryInfo? Directory { 
        get { return _directory; } 
        set {
            // TODO: should probs also check that we have write permissions to this dir (I mean, theoretically maybe user just wants to read metadata, so maybe just show a message idk)
            if (value != null && value.Exists)
            {
                _directory = value;

                /* EnumerateFiles() is faster, but its result cannot utilize indexing via []. Thus switching to GetFiles().
                See   https://stackoverflow.com/questions/3612721/cannot-apply-indexing-with-to-an-expression-of-type-system-collections-gener */  
                this.Files = _directory?.GetFiles() ?? new FileInfo[0];
            }
        }
    }

    /// <summary>
    ///     The files within <paramref name="Directory" />.
    /// </summary>
    public FileInfo[] Files { get; set; } = new FileInfo[0];

    private int _page = 1;
    /// <summary>
    ///     Regarding pagination, which page is requested.
    ///     Must be >= 1.
    ///     1-based / The first page is 1.
    /// </summary>
    public int Page { 
        get { return this._page; } 
        set {
            if (value >= 1)
            {
                this._page = value;
            }
        } 
    }

    private int _resultsPerPage = 100;
    /// <summary>
    ///     Results per page.
    ///     Must be >= 1.
    /// </summary>
    public int ResultsPerPage { 
        get { return this._resultsPerPage; } 
        set {  
            if (value >= 1)
            {
                this._resultsPerPage = value;
            }
        }
    }



    /// <param name="directory">The directory according to the session variable.</param>
    public FilesViewModel(int page, int resultsPerPage, DirectoryInfo? directory)
    {
        Page = page;
        ResultsPerPage = resultsPerPage;
        Directory = directory;
    }
}
