namespace AudioMixingApp.Models;

public class Song
{
    public string Title { get; set; }
    public string Artist { get; set; }
    public TimeSpan Duration { get; set; }
    public string FilePath { get; set; }

}
