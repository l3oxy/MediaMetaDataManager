namespace MediaMetaDataManager.MetaData;

/**
<summary>
    A key-value-pair of metadata of a file/song. 
    A song will have 0 to MANY of these. 
    For example, a song-file may have 1 of these indicating the song title, and may have another indicating song artist.
</summary>
*/
public class FileMetaDataFrame 
{
    /** 
    <summary>
        The raw data the contains the key, and the value, and additional formatting / information.
        Example: TIT2(encoding=&lt;Encoding.UTF8: 3&gt;, text=['Happy Bounce (Nightcore)'])
    </summary>
    */
    public string Raw;

    /** <summary>The KEY/name in the key-value-pair. Example: TPE1</summary> */
    public string Id;

    /** <summary>The value in the key-value-pair. Example: Michael Jackson</summary> */
    public string Value;
}