namespace MediaMetaDataManager.Models;

public class FileMetaData
{
    //public string? fileName { get; set; } // the file name.
    public string? filePath { get; set; } // full file path including dir.
    public string? TIT2 { get; set; } // name
    public string? TPE1 { get; set; } // artist
    public string? COMM { get; set; } // comment / video-ID
}
