namespace AudioMixingApp.Models;

public class Song
{
    public List<Effect> Effects { get; set; }
    public string Title { get; set; }
    public string Artist { get; set; }
    public string Duration {  get; set; }   
}