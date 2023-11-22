namespace AudioMixingApp.Models
{
    internal class Playlist
    {
        public string Name { get; set; }
        public List<Song> Songs { get; set;} = new List<Song>();
    }
}
