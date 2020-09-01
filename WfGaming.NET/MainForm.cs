using System;
using System.Drawing;
using System.Drawing.Imaging;
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
        private KeyHook keyHook;
        private Thread keyHookThread;

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
            keyHook = new KeyHook();
            keyHook.InstallHook();

            keyHookThread = new Thread(new ThreadStart(keyHook.DigestQueue));
            keyHookThread.IsBackground = true;
            keyHookThread.Start();
        }

        ~MainForm()
        {
            Console.WriteLine("MainForm.Destructor");

            keyHook.UninstallHook();
        }

        private Thread backgroundThread;
        private Thread jobThread;

        private void WorkerButton_Click(object sender, EventArgs e)
        {
            backgroundThread = new Thread(dataSource.Run);
            backgroundThread.IsBackground = true;
            backgroundThread.Start();

            jobThread = new Thread(Job);
            jobThread.IsBackground = true;
            jobThread.Start();

            WorkerButton.Enabled = false;
        }

        private void Job()
        {
            // System.Threading.ThreadInterruptedException
            while (true)
            {
                while (!game.IsBattleStarted)
                {
                    try
                    {
                        Thread.Sleep(1000);
                    }
                    catch (System.Threading.ThreadInterruptedException)
                    {
                        goto Endpoint;
                    }
                }

                Console.WriteLine("-----------------------------------------");
                Console.WriteLine($"[{DateTime.Now}] -*- Battle started! -*-");
                Console.WriteLine("-----------------------------------------");

                dataSource.Reset();
                keyHook.Reset();

                while (!dataSource.Enemy.IsVisible)
                {
                    try
                    {
                        Thread.Sleep(1000);
                    }
                    catch (System.Threading.ThreadInterruptedException)
                    {
                        goto Endpoint;
                    }
                }

                Console.WriteLine("-----------------------------------------");
                Console.WriteLine($"[{DateTime.Now}] -*- Enemy detected! -*-");
                Console.WriteLine("-----------------------------------------");

                long battleId = Time.GetTimestamp();
                string path = $@"C:\Users\{Environment.UserName}\Desktop\WfGaming";
                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }

                string dirName = $@"{path}\{battleId}";
                if (!System.IO.Directory.Exists(dirName))
                {
                    System.IO.Directory.CreateDirectory(dirName);
                }

                path = $@"{dirName}\meta.csv";

                System.IO.File.WriteAllText(path, string.Join(",", DataSource.Headers));

                while (!game.IsBattleEnded)
                {
                    long timestamp = Time.GetTimestamp();
                    Bitmap frame = GetScreenCapture();
                    frame.Save($@"{dirName}\{timestamp}.jpg", ImageFormat.Jpeg);

                    string playerCSV = dataSource.GetPlayerCSV();
                    string enemyCSV = dataSource.GetEnemyCSV();

                    int forwarding = 0;
                    int turning = 0;
                    int firing = 0;
                    int zooming = 0;
                    int repairing = 0;
                    char forwardingKey = keyHook.ForwardKey;
                    char turningKey = keyHook.TurnKey;
                    char firingKey = keyHook.FireKey;
                    bool zoomKey = keyHook.ZoomKey;
                    char repairKey = keyHook.RepairKey;

                    if (forwardingKey == 'S')
                    {
                        forwarding = -1;
                    }
                    else if (forwardingKey == 'W')
                    {
                        forwarding = 1;
                    }

                    if (turningKey == 'Q')
                    {
                        turning = -1;
                    }
                    else if (turningKey == 'E')
                    {
                        turning = 1;
                    }

                    if (firingKey == ' ')
                    {
                        firing = 1;
                    }

                    if (zoomKey)     // LShift
                    {
                        zooming = 1;
                    }

                    if (repairKey == 'R')
                    {
                        repairing = 1;
                    }

                    Mouse mouse = dataSource.Mouse;

                    string actions = $"{forwarding},{turning},{firing},{zooming},{repairing},{mouse.X},{mouse.Y}";

                    string data = $"{timestamp}.jpg,{playerCSV},{enemyCSV},{actions}";

                    using (var file = System.IO.File.AppendText(path))
                    {
                        file.WriteLine(data);
                    }

                    try
                    {
                        Thread.Sleep(200);
                    }
                    catch (System.Threading.ThreadInterruptedException e)
                    {
                        goto Endpoint;
                    }
                }

                Console.WriteLine("-----------------------------------------");
                Console.WriteLine($"[{DateTime.Now}] -*- Battle's ended! -*-");
                Console.WriteLine("-----------------------------------------");
            }
            
        Endpoint:
            Console.WriteLine("Background thread exterminated!");
        }

        private void ShowMessageBox(string message, string caption = "WfGaming.NET")
        {
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            DialogResult _ = MessageBox.Show(message, caption, buttons);
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            backgroundThread?.Interrupt();
            jobThread?.Interrupt();

            backgroundThread?.Join();
            jobThread?.Join();

            backgroundThread = null;
            jobThread = null;

            WorkerButton.Enabled = true;
        }

        private void ModSelectButton_Click(object sender, EventArgs e)
        {
            DialogResult result = ModFileOpenDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                string fileName = ModFileOpenDialog.FileName;

                if (!fileName.EndsWith(".py"))
                {
                    ShowMessageBox("Python 파일만 지원됩니다. (*.py)");
                    return;
                }

                ModFilePathTextBox.Text = fileName;
            }
        }

        private void ModInstallButton_Click(object sender, EventArgs e)
        {
            // TODO: Install Mod
            string scriptPath = ModFilePathTextBox.Text;
            Console.WriteLine($"Mod Script: {scriptPath}");

            if (scriptPath.Equals(string.Empty))
            {
                ShowMessageBox("경로를 설정해 주세요.");
                return;
            }

            game.InstallMod(scriptPath);

            ShowMessageBox("모드 설치 완료.");
        }
    }
}
