namespace MediaMetaDataManager.MetaData;

class MetaDataProgram {

    public static string FileName = "mid3v2"; 

    /**
    <summary>Indicates whether the metadata program is accessible and functioning.</summary>
    */
    public static bool MetaDataProgramCheck() 
    {
        // check if mid3v is accessible
        System.Diagnostics.Process metaDataProcess = new System.Diagnostics.Process();
        //proc.EnableRaisingEvents = false; // idk whether to use this.
        metaDataProcess.StartInfo.FileName = FileName;
        metaDataProcess.StartInfo.Arguments = "--version";
        bool metaDataProgram_IsAccessible = false;
        try 
        {
            metaDataProcess.Start();
            if (!metaDataProcess.WaitForExit(5000))
            {
                metaDataProcess.Kill(true);
            }
            metaDataProgram_IsAccessible = (metaDataProcess.ExitCode == 0);
        } 
        catch 
        {
            // Oh well, we tried  -ToyStory's Rex
        }
        return metaDataProgram_IsAccessible;

        // TODO: Probs should redirect stderr and log it to help debug why mid3v2 is inaccessible.
    }

    /**
    <summary>Retrievs metadata information on a given file.</summary>
    */
    public static FileMetaData GetInfo(FileInfo fileRequested) 
    {
        // TODO:
        // read song name, output to page: --TIT2 Title,
        // read artist name, output to page: --TPE1
        // read vid-id/remark, output to page: --COMM User comment
        // read and output other info?:     --UFID Unique file identifier,  --WXXX "User-defined URL data", --TXXX User-Defined text data, , --TMOO Mood, --TLAN Audio Languages,  
        // check whether file is writable
        // have edit buttons or text fields to enable changing song or artist

        System.Diagnostics.Process metaDataProcess = new System.Diagnostics.Process();
        //proc.EnableRaisingEvents = false; // idk whether to use this.
        metaDataProcess.StartInfo.FileName = FileName;
        metaDataProcess.StartInfo.Arguments = $"--verbose --list-raw \"{fileRequested}\""; // TODO: should this be escaped / enclosed in quotes?
        metaDataProcess.StartInfo.RedirectStandardOutput = true;
        metaDataProcess.StartInfo.RedirectStandardError = true;
        metaDataProcess.StartInfo.UseShellExecute = false;

        string standardOutput = String.Empty;
        string standardError = String.Empty;
        FileMetaData fileMetaData = new();
        try 
        {
            metaDataProcess.Start();
            if (!metaDataProcess.WaitForExit(5000))
            {
                metaDataProcess.Kill(true);
            }
            fileMetaData.ExitCode = metaDataProcess.ExitCode;
            fileMetaData.SetStandardOutput(metaDataProcess.StandardOutput.ReadToEnd());
            fileMetaData.StandardError = metaDataProcess.StandardError.ReadToEnd();
        } catch {}
        
        return fileMetaData;
    }
}
