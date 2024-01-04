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
            string path = $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\AudioMixingApp\playlists.json";
            return File.ReadAllText(path);
        }
    }
}
