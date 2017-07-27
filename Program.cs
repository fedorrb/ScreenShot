using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace getScreenShoot
{
    static class Program
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

        [STAThread]
        static void Main()
        {
            var handle = GetConsoleWindow();
            ShowWindow(handle, SW_HIDE);
            Thread screenShotThread = new Thread(MakeScreenShot);
            screenShotThread.Priority = ThreadPriority.Normal;
            screenShotThread.IsBackground = false;
            screenShotThread.Start();
        }
        private static void MakeScreenShot()
        {
            stPath iniFile = new stPath();
            iniFile.LoadIniFile();
            string path = iniFile.screenShotFolder;
            string format = "yyyy.MM.dd HH.mm.ss";
            string ext = iniFile.ext;
            int delay = iniFile.delayMS;
            while (true)
            {
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                Size screenSz = Screen.PrimaryScreen.Bounds.Size;
                Bitmap screenshot = new Bitmap(screenSz.Width, screenSz.Height);
                Graphics gr = Graphics.FromImage(screenshot);
                gr.CopyFromScreen(Point.Empty, Point.Empty, screenSz);
                string filepath = Path.Combine(path, DateTime.Now.ToString(format)) + "." + ext;
                List<PropertyInfo> props = typeof(ImageFormat).GetProperties(BindingFlags.Static | BindingFlags.Public).ToList();
                var imgformat = (ImageFormat)props.Find(prop => prop.Name.ToLower() == ext).GetValue(null, null);
                screenshot.Save(filepath, imgformat);
                Thread.Sleep(delay);
            }
        }
    }
}