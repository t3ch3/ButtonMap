using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UIKit;

namespace JustButtons
{
    /// <summary>
    /// Contains data file info e.g. path
    /// Used to save and load the data file
    /// </summary>
    public class FileManager
    {
        //data file info
        public string FileName = "JustButtons.txt";
        public string FileDirectory;
        public string FilePath; //when delete app it will delete in this path because personal is apart of this app

        public FileManager()
        {
            //gets the personal directory for the app and combines it with the name to get the absolute path
            FileDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            FilePath = Path.Combine(FileDirectory, FileName);
        }

        /// <summary>
        /// Saves the app data to the data file
        /// Overwrites
        /// </summary>
        /// <param name="appData">App data.</param>
        public void CreateFileAppData(AppData appData)
        {
            //*If crashes, simply rightclcik update package and rebuild*
            //converts the passed app data into a json string
            //ignores any null values - not including them in the json string
            string jsonString = JsonConvert.SerializeObject(appData, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }); //doesn't save members that are null
            File.WriteAllText(FilePath, jsonString); //writes the json string to the data file
            Console.WriteLine("wrote:" + jsonString);
            Console.WriteLine(jsonString);
            Console.WriteLine("saved file");
        }

        /// <summary>
        /// Loads the file app data into an app data obj and returns it.
        /// </summary>
        /// <returns>The file app data.</returns>
        public AppData LoadFileAppData()
        {
            //create new app data obj to hold the deserialised json string
            AppData appData = new AppData();
            string jsonString = File.ReadAllText(FilePath); //get the json text from the file
            Console.WriteLine(jsonString);

            // what if json is incorrect????
            //deserialize the json string into an app data obj
            appData = JsonConvert.DeserializeObject<AppData>(jsonString);
            return appData;
        }
    }
}
