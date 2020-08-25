using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using WfGaming.Utils;
using System.Threading;

namespace WfGaming
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private Bitmap GetScreenCapture()
        {
            int width = 1920;   // Screen.PrimaryScreen.WorkingArea.Width;
            int height = 1080;  // Screen.PrimaryScreen.WorkingArea.Height;

            Size size = new Size(width, height);

            Bitmap bitmap = new Bitmap(width, height);

            Graphics g = Graphics.FromImage(bitmap);

            g.CopyFromScreen(0, 0, 0, 0, size);

            // bitmap.Save("Image.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);

            return bitmap;
        }

        private delegate IntPtr LowLevelKeyboardProc(int nCode, int wParam, ref IntPtr lParam);

        [DllImport("user32.dll")]
        static extern IntPtr SetWindowsHookExA(int idHook, LowLevelKeyboardProc lpfn, IntPtr hmod, uint dwThreadId);

        const int WH_KEYBOARD_LL = 13;

        private void MainForm_Load(object sender, EventArgs e)
        {
            /*
            if (ProcessManager.IsGameProcessRunning())
            {
                string message = "World of Warships를 종료 후 다시 실행해 주세요.";
                string caption = "WfGaming.NET";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult _ = MessageBox.Show(message, caption, buttons);
                this.Close();
            }
            */

            KeyHook keyHook = new KeyHook();
            KeyHook.InstallHook();
            new Thread(new ThreadStart(keyHook.DigestQueue)).Start();
        }

        ~MainForm()
        {
            Console.WriteLine("MainForm.Destructor");

            KeyHook.UninstallHook();
        }
    }
}
