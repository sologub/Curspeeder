using Microsoft.Win32;
using System;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MouseCursorSpeeder
{
    internal static class Program
    {
        static string currentId;
        static NotifyIcon trayIcon;

        [DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();

        [STAThread]
        static void Main()
        {
            if (Environment.OSVersion.Version.Major >= 6)
            {
                SetProcessDPIAware();
            }

            AddToStartup();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Set up tray icon
            trayIcon = new NotifyIcon();
            trayIcon.Icon = DrawIconWithSpeed(0); // Initial speed is set to unknown
            trayIcon.Text = "Curspeeder";
            trayIcon.Visible = true;

            ContextMenuStrip contextMenu = new ContextMenuStrip();
            for (int i = 1; i <= 20; i++)
            {
                int speed = i;
                contextMenu.Items.Add($"Speed {speed}", null, (s, e) => SetMouseSpeedFromMenu(speed));
            }
            contextMenu.Items.Add(new ToolStripSeparator());
            contextMenu.Items.Add("Exit", null, (s, e) => Application.Exit());
            trayIcon.ContextMenuStrip = contextMenu;

            ManagementEventWatcher watcher = new ManagementEventWatcher();
            WqlEventQuery query = new WqlEventQuery("SELECT * FROM Win32_DeviceChangeEvent WHERE EventType = 2");

            watcher.EventArrived += DeviceConnected;
            watcher.Query = query;
            watcher.Start();

            Application.Run(); // Keep the application running

            watcher.Stop();
            trayIcon.Visible = false;
        }

        private static void DeviceConnected(object sender, EventArrivedEventArgs e)
        {
            var id = GetMouseDeviceId();
            Console.WriteLine($"Detected mouse {id}");
            var speed = id == @"USB\VID_1532&PID_0067&MI_00\7&72BF1E6&0&0000" ? 2 : 9;
            if (currentId != id)
            {
                MouseSettings.SetMouseSpeed(speed);
                currentId = id;
                UpdateTrayIcon(speed);
                ShowSpeedChangeNotification(speed);
            }
        }

        private static string GetMouseDeviceId()
        {
            string deviceId = string.Empty;
            using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PointingDevice"))
            {
                foreach (var device in searcher.Get().Cast<ManagementObject>())
                {
                    if (device["PNPDeviceID"] != null)
                    {
                        deviceId = device["PNPDeviceID"].ToString();
                        break;
                    }
                }
            }
            return deviceId;
        }

        private static void SetMouseSpeedFromMenu(int speed)
        {
            MouseSettings.SetMouseSpeed(speed);
            UpdateTrayIcon(speed);
            ShowSpeedChangeNotification(speed);
        }

        private static void UpdateTrayIcon(int speed)
        {
            trayIcon.Text = $"Curspeeder - Current Speed: {speed}";
            trayIcon.Icon = DrawIconWithSpeed(speed);
        }

        private static void ShowSpeedChangeNotification(int speed)
        {
            trayIcon.BalloonTipTitle = "Mouse Speed Changed";
            trayIcon.BalloonTipText = $"Mouse speed has been set to {speed}";
            trayIcon.ShowBalloonTip(1000);
        }

        private static Icon DrawIconWithSpeed(int speed)
        {
            Bitmap bitmap = new Bitmap(32, 32);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.Clear(Color.Transparent);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.DrawString(speed.ToString(), new Font("Arial", 16, FontStyle.Bold), Brushes.Black, new PointF(2, 2));
            }
            IntPtr hIcon = bitmap.GetHicon();
            Icon icon = Icon.FromHandle(hIcon);
            bitmap.Dispose();
            return icon;
        }

        static void AddToStartup()
        {
            var executablePath = Application.ExecutablePath;
            var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
            key?.SetValue("MouseCursorSpeeder", executablePath);
        }
    }

    static class MouseSettings
    {
        private const uint SPI_SETMOUSESPEED = 0x0071;

        [DllImport("user32.dll")]
        static extern bool SystemParametersInfo(uint uiAction, uint uiParam, uint pvParam, uint fWinIni);

        public static void SetMouseSpeed(int speed)
        {
            if (speed < 1 || speed > 20) speed = 10; // Valid speeds are typically between 1 (slowest) and 20 (fastest)
            SystemParametersInfo(SPI_SETMOUSESPEED, 0, (uint)speed, 0);
        }
    }
}
