using System;
using System.Drawing;
using System.Threading;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using WfGaming.Utils;
using WfGaming.Models;
using System.ComponentModel;

namespace WfGaming
{
    public partial class MainForm : Form
    {
        private Game game;
        private DataSource dataSource;

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

        private void MainForm_Load(object sender, EventArgs e)
        {
#if !DEBUG
            if (ProcessManager.IsGameProcessRunning())
            {
                string message = "World of Warships를 종료 후 다시 실행해 주세요.";
                string caption = "WfGaming.NET";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult _ = MessageBox.Show(message, caption, buttons);
                this.Close();
            }
#endif
            game = new Game();
            Console.WriteLine($"Build: {game.Build}, Version: {game.Version}");

            BuildLabel.Text = game.Build.ToString();
            VersionLabel.Text = game.Version;

            dataSource = new DataSource(game);

            //KeyHook keyHook = new KeyHook();
            //KeyHook.InstallHook();
            //new Thread(new ThreadStart(keyHook.DigestQueue)).Start();
        }

        ~MainForm()
        {
            Console.WriteLine("MainForm.Destructor");

            //KeyHook.UninstallHook();
        }

        private Thread backgroundThread;

        private void WorkerButton_Click(object sender, EventArgs e)
        {
            backgroundThread = new Thread(dataSource.Run);
            backgroundThread.IsBackground = true;
            backgroundThread.Start();
            //dataSource.Run();

            //backgroundThread = new Thread(Job);
            //backgroundThread.IsBackground = true;
            //backgroundThread.Start();

            WorkerButton.Enabled = false;
        }

        private void Job()
        {
            while (true)
            {
                while (!game.IsBattleStarted)
                {
                    Thread.Sleep(1000);
                }

                Console.WriteLine("-----------------------------------------");
                Console.WriteLine($"[{DateTime.Now}] -*- Battle started! -*-");
                Console.WriteLine("-----------------------------------------");

                Console.WriteLine($"[{Thread.CurrentThread}] [{DateTime.Now}] Background thread is running..");
                try
                {
                    Thread.Sleep(1000);
                }
                catch (System.Threading.ThreadInterruptedException e)
                {
                    break;
                }
            }

            Console.WriteLine("Background thread exterminated!");
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            backgroundThread.Interrupt();
            backgroundThread.Join();

            WorkerButton.Enabled = true;
        }
    }
}
