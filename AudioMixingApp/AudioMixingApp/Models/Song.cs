namespace AudioMixingApp.Models;

public class Song
{
    public List<Effect> Effects { get; set; }
    public string title { get; set; }
    public string artist { get; set; }
    public string duration {  get; set; }   
}