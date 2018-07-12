using System;
using System.Collections.Generic;

namespace JustButtons
{
    /// <summary>
    /// App data class holds the app data that is saved.
    /// </summary>
    public class AppData
    {
        //sets default values
        public int ButtonsPerPage = 6;
        public int NumberOfPages = 3;
        public List<ButtonData> Buttons = new List<ButtonData>();

        public AppData()
        {
        }
    }
}
