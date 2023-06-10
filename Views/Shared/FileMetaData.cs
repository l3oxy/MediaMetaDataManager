namespace MediaMetaDataManager.MetaData;

public class FileMetaData 
{
    public int? ExitCode = null;
    public bool SuccessfullyRead 
    { 
        get 
        {
            return ((ExitCode is not null) && (ExitCode == 0));
        } 
    }
    public string? StandardOutput; // TODO: SHould probs change this to readonly or a property or something to prevent this field from being written to, instead they should forced to use the method below
    public string? StandardError;
    public Queue<FileMetaDataFrame> Frames = new();



    /**
    <summary>Sets the standard output, and parses it to also set the Tags.</summary>
    */
    public void SetStandardOutput(string standardOutput)
    {
        this.StandardOutput = standardOutput;

        var outputLines = standardOutput.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries); 
        int outputLinesCount = outputLines.Count();
        string line; // the current line.
        string[] lineFrameContents; // all the details of the frame excluding the name and containing parenthises. Includes the value (AKA lineFrameValue) and other details.
        int indexOfFirstOpenParenthises;
        string lineFrameId;    // AKA the name of the metadata-field.
        string lineFrameValue; // AKA the value of the metadata-field.
        string lineFrameValuePrecursor; // the precursor to the lineFrameValue
        int lineFrameValueStartIndex; // the precursor to the lineFrameValue
        FileMetaDataFrame tag;

        // skip the first one because it's just saying the filename.
        for (int i = 1; i < outputLinesCount; ++i)
        {
            line = outputLines[i];
            if (line == "No ID3 header found; skipping.")
            {
                /* should still show the common ones (TIT2, TPE1, COMM) 
                so that they can be edited by the user.
                    Raw IDv2 tag info for /media/fnn/WD2TB/Media/Music/Id_Forgive_You_-_Vulpes_and_Kleyna_-_{smXLKTodrgI}.flac
                    TIT2(encoding=<Encoding.UTF8: 3>, text=['Id Forgive You'])
                    TPE1(encoding=<Encoding.UTF8: 3>, text=['Vulpes and Kleyna'])
                    COMM(encoding=<Encoding.UTF8: 3>, lang='eng', desc='', text=['smXLKTodrgI'])
                */
                break;
            }
            indexOfFirstOpenParenthises = line.IndexOf('(');
            lineFrameId = line.Substring(0, indexOfFirstOpenParenthises);

            lineFrameContents = line.Substring(indexOfFirstOpenParenthises + 1, line.LastIndexOf(')') - indexOfFirstOpenParenthises - 1).Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            lineFrameValuePrecursor = lineFrameContents.Single(deet => deet.StartsWith("text=", StringComparison.CurrentCultureIgnoreCase));
            lineFrameValueStartIndex = lineFrameValuePrecursor.IndexOf("['") + 2;
            lineFrameValue = lineFrameValuePrecursor.Substring(lineFrameValueStartIndex, lineFrameValuePrecursor.LastIndexOf("']") - lineFrameValueStartIndex);
            
            tag = new(){Raw = line, Id = lineFrameId, Value = lineFrameValue};

            this.Frames.Enqueue(tag);
        }
    }
}