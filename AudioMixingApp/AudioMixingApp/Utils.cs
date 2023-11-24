namespace AudioMixingApp
{
    public class Utils
    {
        /// <summary>
        /// Gets the JSON from the Playlists.json file.
        /// </summary>
        /// <returns>a JSON string containing the playlists</returns>
        public static string GetJSON()
        {
            string path = "C:\\xampp\\htdocs\\school\\Jaar 3\\C#2 herkansing\\AudioMixingApp\\AudioMixingApp\\AudioMixingApp\\Playlists.json";
            return File.ReadAllText(path);
        }
    }
}
