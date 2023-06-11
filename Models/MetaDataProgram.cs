using MediaMetaDataManager.Models;
using System.Diagnostics;

namespace MediaMetaDataManager.MetaData;

// TODO: THIS FILE SHOULD PROBS BE MOVED FROM VIEWS TO IDK MODELS OR /.

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
    public static MetaDataProgramOutput GetInfo(FileInfo fileRequested) 
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
        MetaDataProgramOutput fileMetaData = new();
        try 
        {
            metaDataProcess.Start();
            if (!metaDataProcess.WaitForExit(5000))
            {
                metaDataProcess.Kill(true);
            }
            fileMetaData.ExitCode = metaDataProcess.ExitCode;
            fileMetaData.StandardOutput = metaDataProcess.StandardOutput.ReadToEnd();
            fileMetaData.StandardError = metaDataProcess.StandardError.ReadToEnd();
        } catch {}
        
        return fileMetaData;
    }

    public static int ChangeMetaData(FileMetaData fileMetaData)
    {
        ProcessStartInfo startInfo = new(FileName);
        List<string> framesToDelete = new();

        if (String.IsNullOrWhiteSpace(fileMetaData.TIT2))
        {
            framesToDelete.Add(nameof(fileMetaData.TIT2));
        } 
        else 
        {
            startInfo.ArgumentList.Add($"--{nameof(fileMetaData.TIT2)}={fileMetaData.TIT2}");
        }
        
        if (String.IsNullOrWhiteSpace(fileMetaData.TPE1))
        {
            framesToDelete.Add(nameof(fileMetaData.TPE1));
        }
        else
        {
            startInfo.ArgumentList.Add($"--{nameof(fileMetaData.TPE1)}={fileMetaData.TPE1}");
        }

        if (String.IsNullOrWhiteSpace(fileMetaData.COMM))
        {
            framesToDelete.Add(nameof(fileMetaData.COMM));
        }
        else
        {
            startInfo.ArgumentList.Add($"--{nameof(fileMetaData.COMM)}={fileMetaData.COMM}");
        }

        if (framesToDelete.Count > 0)
        {
            string framesToDeleteAsCsv = String.Join(',', framesToDelete.ToArray());
            startInfo.ArgumentList.Add($"--delete-frames={framesToDeleteAsCsv}");
        }

        startInfo.ArgumentList.Add($"{fileMetaData.filePath}");
        startInfo.RedirectStandardError = true;
        startInfo.RedirectStandardOutput = true;
        startInfo.UseShellExecute = false;
        Process metaDataProcess = new Process();
        metaDataProcess.StartInfo = startInfo;


        string standardOutput = String.Empty;
        string standardError = String.Empty;
        try 
        {
            metaDataProcess.Start();
            if (!metaDataProcess.WaitForExit(5000))
            {
                metaDataProcess.Kill(true);
            }
            Console.WriteLine(metaDataProcess.ExitCode);
            Console.WriteLine(metaDataProcess.StandardOutput.ReadToEnd());
            Console.WriteLine(metaDataProcess.StandardError.ReadToEnd());
        } catch {}

        return metaDataProcess.ExitCode;
    }
}
