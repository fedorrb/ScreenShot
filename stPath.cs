using System;
using System.Windows.Forms;
using System.IO;

namespace getScreenShoot
{
    public class stPath
    {
        public int delayMS { get; set; }//час очікування
        public string screenShotFolder { get; set; }//папка для зберігання скріншотів
        public string ext { get; set; } //формат зображення

        public stPath()
        {
            delayMS = 120000;
            screenShotFolder = "C:\\screenshots\\";
            ext = "png";
        }
        /// <summary>
        /// Чтение файла конфигурации
        /// </summary>
        public void LoadIniFile()
        {
            string path = String.Empty;
            path = System.IO.Directory.GetCurrentDirectory();
            path += @"\screenshot.ini";
            int elements = 0;
            if (System.IO.File.Exists(path))
            {
                try
                {
                    string[] allLines = System.IO.File.ReadAllLines(path);
                    elements = allLines.Length;
                    switch (elements)
                    {
                        case 3:
                            ext = allLines[2];
                            goto case 2;
                        case 2:
                            delayMS = int.Parse(allLines[1]);
                            goto case 1;
                        case 1:
                            screenShotFolder = allLines[0];
                            break;
                        case 0:
                            break;
                        default:
                            break;
                    }

                }
                catch
                {
                    delayMS = 120000;
                    screenShotFolder = "C:\\screenshots\\";
                    ext = "png";
                }
            }

            if (delayMS < 10000)
                delayMS = 10000;

            if (Directory.Exists(screenShotFolder) == false)
            {
                try
                {
                    Directory.CreateDirectory(screenShotFolder);
                }
                catch
                {
                    screenShotFolder = "C:\\screenshots\\";
                }
            }
        }

        /// <summary>
        /// Запись файла конфигурации
        /// </summary>
        public void SaveIniFile()
        {
            string[] s = { screenShotFolder,
                             delayMS.ToString(),
                             ext
                         };
            string path = String.Empty;
            path = Application.StartupPath;
            path += @"\screenshot.ini";
            System.IO.File.WriteAllLines(path, s);
        }
    }
}
