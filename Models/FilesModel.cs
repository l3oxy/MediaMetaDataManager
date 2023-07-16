namespace MediaMetaDataManager.Models;

public class FilesModel
{
    /// <summary>
    ///     Regarding pagination, which page is requested.
    /// </summary>
    public int Page { get; set; }
    /// <summary>
    ///     Results per page.
    ///     Unsure what the correct word would be; Step, Stride, Size?
    /// </summary>
    public int Step { get; set; }

    public FilesModel()
    {
        Page = 1;
        Step = 100;
    }
}
