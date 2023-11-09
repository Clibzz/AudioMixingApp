using AudioMixingApp.Models;

namespace AudioMixingApp.ViewModels;

public class MixingPageViewModel
{
    public List<Song> Songs { get; set; }
    
    public MixingPageViewModel()
    {
        Songs = new List<Song>
        {
            new Song {Title = "a"},
            new Song {Title = "b"},
            new Song {Title = "c"},
        };
    }
}