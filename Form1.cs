using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RothsFarmIt
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            CheckForIllegalCrossThreadCalls = false; //For the farm :'(
            InitializeComponent();
        }


        private static void KillProcessAndChildrens(int pid)
        {
            ManagementObjectSearcher processSearcher = new ManagementObjectSearcher
              ("Select * From Win32_Process Where ParentProcessID=" + pid);
            ManagementObjectCollection processCollection = processSearcher.Get();

            try
            {
                Process proc = Process.GetProcessById(pid);
                if (!proc.HasExited) proc.Kill();
            }
            catch (ArgumentException)
            {
                // Process already exited.
            }

            if (processCollection != null)
            {
                foreach (ManagementObject mo in processCollection)
                {
                    KillProcessAndChildrens(Convert.ToInt32(mo["ProcessID"])); //kill child processes(also kills childrens of childrens etc.)
                }
            }
        }

        //Important keys
        //WM_NEXTDLGCTL  == TAB

        [DllImport("kernel32.dll")]
        public static extern bool WriteProcessMemory(
        IntPtr hProcess,
        IntPtr lpBaseAddress,
        byte[] lpBuffer,
        int dwSize,
        out IntPtr lpNumberOfBytesWritten);

        [DllImport("user32.dll")]
        static extern bool PostMessage(IntPtr hWnd, UInt32 Msg, int wParam, int lParam);
        const uint WM_KEYDOWN = 0x100;
        const uint WM_SYSKEYDOWN = 0x0104;
        const uint WM_KEYUP = 0x101;
        const uint WM_SYSKEYUP = 0x0105;
        const uint WM_CHAR = 0x102;

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, UInt32 Msg, int wParam, int lParam);
        private const uint WM_LBUTTONDOWN = 0x0201;
        private const uint WM_LBUTTONUP = 0x0202;
        private const uint WM_RBUTTONDOWN = 0x0204;
        private const uint WM_RBUTTONUP = 0x0205;

        [DllImport("kernel32.dll")]
        public static extern Int32 ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress,
            [In, Out] byte[] buffer, UInt32 size, out IntPtr lpNumberOfBytesRead);

        private static byte[] ReadBytes(IntPtr handle, long address, uint bytesToRead)
        {
            IntPtr ptrBytesRead;
            byte[] buffer = new byte[bytesToRead];

            ReadProcessMemory(handle, new IntPtr(address), buffer, bytesToRead, out ptrBytesRead);

            return buffer;
        }

        private static void WriteBytes(IntPtr handle, long address, int value)
        {

            byte[] dataBuffer = BitConverter.GetBytes(value);
            IntPtr bytesWritten = IntPtr.Zero;

            WriteProcessMemory(handle, (IntPtr)address, dataBuffer, dataBuffer.Length, out bytesWritten);
        }

        private static int ReadInt32(IntPtr handle, long address)
        {
            return BitConverter.ToInt32(ReadBytes(handle, address, 4), 0);
        }

        static string parseMemoryMessage(Process proc, int addr)
        {
            byte[] buffer = ReadBytes(proc.Handle, addr, 100);
            int firstNullIndex = Array.FindIndex(buffer, b => b == 0);
            string msg = Encoding.Default.GetString(buffer, 0, firstNullIndex);
            string[] msgContent = msg.Split(".");
            return msgContent[0];
        }

        public static void pressKey(Process proc, Keys keybind)
        {
            PostMessage(proc.MainWindowHandle, WM_KEYDOWN, (int)keybind, 0);
            PostMessage(proc.MainWindowHandle, WM_KEYUP, (int)keybind, 0);
        }

        public static void sendWords(Process proc, String word)
        {
            foreach (char c in word)
            {
                int charValue = c;
                Keys key = (Keys)Enum.Parse(typeof(Keys), c.ToString().ToUpper());
                PostMessage(proc.MainWindowHandle, WM_CHAR, c, 0);
            }
        }
        public static void mouseClick(Process proc, int keybind)
        {
            SendMessage(proc.MainWindowHandle, WM_LBUTTONDOWN, keybind, 0);
            SendMessage(proc.MainWindowHandle, WM_LBUTTONUP, keybind, 0);
        }

        public static void autoPots(Process proc, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    int hpAddr = 0x010DCE10;
                    int hpMaxAddr = hpAddr + 4;
                    int spAddr = hpAddr + 8;
                    int spMaxAddr = hpAddr + 12;
                    double spPercToPot = 10;
                    double hpPercToPot = 1;
                    int spVal = 0;
                    int spMax = 0;
                    int hpVal = 0;
                    int hpMax = 0;
                    spVal = ReadInt32(proc.Handle, spAddr);
                    spMax = ReadInt32(proc.Handle, spMaxAddr);
                    hpVal = ReadInt32(proc.Handle, hpAddr);
                    hpMax = ReadInt32(proc.Handle, hpMaxAddr);
                    double hpPerc = ((double)hpVal / (double)hpMax) * 100;
                    double spPerc = ((double)spVal / (double)spMax) * 100;
                    if (spPerc < spPercToPot)
                    {
                        Thread.Sleep(100);
                        pressKey(proc, Keys.F7);
                    }
                    //if (hpPerc < hpPercToPot)
                    //{
                    //    Thread.Sleep(100);
                    //    pressKey(proc, Keys.F7);
                    //}
                    if (token.IsCancellationRequested)
                    {
                        Console.WriteLine("Cancelled");
                    }
                } catch (Exception ex)
                {

                }

            }
        }

        public static void loginProcedure(Process proc)
        {
            Thread.Sleep(9000);
            pressKey(proc, Keys.Enter);
            Thread.Sleep(500);
            pressKey(proc, Keys.Tab);
            Thread.Sleep(500);
            sendWords(proc, "user here");
            Thread.Sleep(500);
            pressKey(proc, Keys.Tab);
            Thread.Sleep(500);
            sendWords(proc, "Password here");
            Thread.Sleep(500);
            pressKey(proc, Keys.Enter);
            Thread.Sleep(500);
            pressKey(proc, Keys.Enter);
            Thread.Sleep(500);
            pressKey(proc, Keys.Enter);
            Thread.Sleep(1000);
            pressKey(proc, Keys.F8);
            Thread.Sleep(500);
            pressKey(proc, Keys.F8);
            Thread.Sleep(1500);
        }

        public static void wizFarm(Process proc, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                int newCount = WinUtil.GetWindowCount(proc.Id);
                if (newCount > 1)
                {
                    KillProcessAndChildrens(proc.Id);
                }
                if (token.IsCancellationRequested)
                {
                    Console.WriteLine("Cancelled");
                }
                if (proc.HasExited)
                {
                    proc.Start();
                    Thread.Sleep(5000);
                    loginProcedure(proc);
                } else
                {
                    int mapAddr = 0x00E43F48;
                    string mapName = parseMemoryMessage(proc, mapAddr);
                    if(mapName == "ordeal_3-2")
                    {
                        CastSkillAndTeleClip(proc);
                    } else
                    {
                        WarpToGoldRoom(proc);
                    }
                    
                }
            }
        }

        public static void WarpToGoldRoom(Process proc)
        {
            int mapAddr = 0x00E43F48;
            string mapName = parseMemoryMessage(proc, mapAddr);
            if (mapName == "prontera")
            {
                pressKey(proc, Keys.F7);
                Thread.Sleep(800);
                WriteBytes(proc.Handle, 0x00E2EC74, 515); //X
                WriteBytes(proc.Handle, 0x00E2EC78, 356); //Y
                Thread.Sleep(300);
                mouseClick(proc, (int)Keys.LButton);
                mouseClick(proc, (int)Keys.LButton);
                Thread.Sleep(300);
                pressKey(proc, Keys.Enter);
                Thread.Sleep(300);
                pressKey(proc, Keys.Enter);
                Thread.Sleep(300);
                pressKey(proc, Keys.Enter);
                Thread.Sleep(300);
                pressKey(proc, Keys.Enter);
                Thread.Sleep(300);
                pressKey(proc, Keys.Enter);
                Thread.Sleep(300);
                pressKey(proc, Keys.Enter);
                Thread.Sleep(800);
            }
            
        }

        public static void CastSkillAndTeleClip(Process proc)
        {
            IntPtr xAddr = (IntPtr)0x00E2EC74;
            IntPtr yAddr = (IntPtr)0x00E2EC78;
            pressKey(proc, Keys.F9);
            Thread.Sleep(100);
            pressKey(proc, Keys.Enter);
            Thread.Sleep(100);
            //Thread.Sleep(3);
            pressKey(proc, Keys.F1);
            //New Era, Kratos IDS
            WriteBytes(proc.Handle, 0x00E2EC74, 509); //X
            WriteBytes(proc.Handle, 0x00E2EC78, 386); //Y
                                                      //Thread.Sleep(3);
            for (int i = 0; i < 6; i++)
            {
                pressKey(proc, Keys.F1);
                Thread.Sleep(1);
                mouseClick(proc, (int)Keys.LButton);
            }
            Thread.Sleep(100);
        }

        public static void CastSkillAndWarp(Process proc)
        {
            IntPtr xAddr = (IntPtr)0x00E2EC74;
            IntPtr yAddr = (IntPtr)0x00E2EC78;
            pressKey(proc, Keys.F9);
            //Thread.Sleep(3);
            pressKey(proc, Keys.F1);
            //New Era, Kratos IDS
            WriteBytes(proc.Handle, 0x00E2EC74, 509); //X
            WriteBytes(proc.Handle, 0x00E2EC78, 386); //Y
                                                      //Thread.Sleep(3);
            for (int i = 0; i < 4; i++)
            {
                pressKey(proc, Keys.F1);
                Thread.Sleep(1);
                mouseClick(proc, (int)Keys.LButton);
            }
            for (int i = 0; i < 5; i++)
            {
                pressKey(proc, Keys.F1);
                Thread.Sleep(1);
                mouseClick(proc, (int)Keys.LButton);
            }
            Thread.Sleep(1);
        }

        public static void autoStorage(Process proc, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                int newCount = WinUtil.GetWindowCount(proc.Id);
                if (newCount > 1)
                {
                    KillProcessAndChildrens(proc.Id);
                }
                if (token.IsCancellationRequested)
                {
                    Console.WriteLine("Cancelled");
                }
                if (proc.HasExited)
                {
                    proc.Start();
                    Thread.Sleep(5000);
                    loginProcedure(proc);
                }
                else
                {
                    int maxWeight = 0x010D94AC;
                    int currWeight = 0x010D94B0;
                    int maxWVal = ReadInt32(proc.Handle, maxWeight);
                    int currWVal = ReadInt32(proc.Handle, currWeight);;
                    double weightPerc = ((double)currWVal / (double)maxWVal) * 100;
                    double WeightToStore = 60;

                    if (weightPerc > WeightToStore) { 
                        IntPtr xAddr = (IntPtr)0x00E2EC74;
                        IntPtr yAddr = (IntPtr)0x00E2EC78;
                        WriteBytes(proc.Handle, 0x00E2EC74, 54); //X
                        WriteBytes(proc.Handle, 0x00E2EC78, 175); //Y

                        SendMessage(proc.MainWindowHandle, WM_LBUTTONDOWN, (int)Keys.LButton, 0);
                        Thread.Sleep(55);
                        WriteBytes(proc.Handle, 0x00E2EC74, 344); //X
                        WriteBytes(proc.Handle, 0x00E2EC78, 191); //Y
                        Thread.Sleep(55);
                        SendMessage(proc.MainWindowHandle, WM_LBUTTONUP, (int)Keys.LButton, 0);
                        Thread.Sleep(55);
                        pressKey(proc, Keys.Enter);
                        Thread.Sleep(500);
                    }
                    //SendMessage(proc.MainWindowHandle, WM_SYSKEYDOWN, (int)Keys.Menu, 0);
                    //Thread.Sleep(111);
                    //SendMessage(proc.MainWindowHandle, WM_RBUTTONDOWN, (int)Keys.RButton, 0);
                    //Thread.Sleep(111);
                    //SendMessage(proc.MainWindowHandle, WM_RBUTTONUP, (int)Keys.RButton, 0);
                    //Thread.Sleep(111);
                    //SendMessage(proc.MainWindowHandle, WM_SYSKEYUP, (int)Keys.Menu, 0);
                }
            }
        }

        Process ragnaProc = new Process();
        static List<Thread> threadList = new List<Thread>();
        static CancellationTokenSource src = new CancellationTokenSource();
        static CancellationToken ct = src.Token;

        private async void startBtn_Click(object sender, EventArgs e)
        {
            startBtn.Enabled = false;
            if (ragnaProc.Container == null)
            {
                src = new CancellationTokenSource();
                CancellationToken ct = src.Token;

                var longOp1 = Task.Factory.StartNew(() => wizFarm(ragnaProc, ct), ct, TaskCreationOptions.LongRunning, TaskScheduler.Default);
                //Task t1 = Task.Run(() => wizFarm(ragnaProc, ct), ct);
                //Task t2 = Task.Run(() => autoPots(ragnaProc, ct), ct);

                try
                {
                    await Task.WhenAll(new[] { longOp1 });
                    //await t1;
                    //await t2;
                }
                catch (AggregateException ae)
                {
                    if (ae.InnerExceptions.Any(e => e is TaskCanceledException))
                        Console.WriteLine("Task cancelled exception detected");
                    else
                        throw;
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    src.Dispose();
                }
            }
        }

        private void stopBtn_Click(object sender, EventArgs e)
        {
            try
            {
                src.Cancel();
                startBtn.Enabled = true;
            } catch (Exception exce)
            {
               
            }
            
        }

        private void frstLoginBtn_Click(object sender, EventArgs e)
        {
            ragnaProc.StartInfo.FileName = "File location here (use shortcut .lnk if protected)";
            ragnaProc.StartInfo.UseShellExecute = true;
            ragnaProc.Start();

            loginProcedure(ragnaProc);
        }

        private void dbgAttach_Click(object sender, EventArgs e)
        {
            Process[] proc = Process.GetProcessesByName("Process Name Here for debugging");
            ragnaProc = proc[0];
        }

        private void mapBtn_Click(object sender, EventArgs e)
        {
            int mapAddr = 0x00E43F48;
            string mapName = parseMemoryMessage(ragnaProc, mapAddr);
            bool isPront = mapName == "prontera";
            MessageBox.Show(isPront.ToString());
        }
    }
}
