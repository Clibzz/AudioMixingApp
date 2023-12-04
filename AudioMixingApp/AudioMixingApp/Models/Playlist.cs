namespace AudioMixingApp.Models
{
    internal class Playlist
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public List<Song> Songs { get; set;} = new List<Song>();
    }
}
