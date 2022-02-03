using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
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
            InitializeComponent();
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
        const uint WM_KEYUP = 0x101;
        const uint WM_CHAR = 0x102;

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, UInt32 Msg, int wParam, int lParam);
        private const uint WM_LBUTTONDOWN = 0x0201;
        private const uint WM_LBUTTONUP = 0x0202;

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

        public static void autoPots(Process proc)
        {
            while (true)
            {
                try
                {
                    int hpAddr = 0x010DCE10;  //New Era Addr
                                              //int hpAddr = 0x00E4CAF4;  //Vicious
                    int hpMaxAddr = hpAddr + 4;
                    int spAddr = hpAddr + 8;
                    int spMaxAddr = hpAddr + 12;
                    double spPercToPot = 10;
                    double hpPercToPot = 80;
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
                    if (hpPerc < hpPercToPot)
                    {
                        Thread.Sleep(100);
                        pressKey(proc, Keys.F7);
                    }
                } catch (Exception ex)
                {

                }

            }
        }

        Process ragnaProc = new Process();
        static List<Thread> threadList = new List<Thread>();
        private void startBtn_Click(object sender, EventArgs e)
        {
            if (ragnaProc.Container == null)
            {
                ragnaProc.StartInfo.FileName = "D:\\Games\\RequiemRO\\RequiemRO.lnk";
                ragnaProc.StartInfo.UseShellExecute = true;
                ragnaProc.Start();

                loginProcedure(ragnaProc);
            }


            Thread thd = new Thread(() => doFarming(ragnaProc, 10));
            Thread thd2 = new Thread(() => autoPots(ragnaProc));
            thd.IsBackground = true;
            threadList.Add(thd);
            threadList.Add(thd2);

            foreach (Thread th in threadList)
            {
                th.Start();
            }
        }

        public static void loginProcedure(Process proc)
        {
            Thread.Sleep(8000);
            pressKey(proc, Keys.Enter);
            Thread.Sleep(1000);
            pressKey(proc, Keys.Tab);
            Thread.Sleep(1000);
            sendWords(proc, "stfu09");
            Thread.Sleep(1000);
            pressKey(proc, Keys.Tab);
            Thread.Sleep(1000);
            sendWords(proc, "Deds1234");
            Thread.Sleep(1000);
            pressKey(proc, Keys.Enter);
            Thread.Sleep(1000);
            pressKey(proc, Keys.Enter);
            Thread.Sleep(1000);
            pressKey(proc, Keys.Enter);
            Thread.Sleep(2000);
            pressKey(proc, Keys.F8);
            Thread.Sleep(1000);
        }

        public static void doFarming(Process proc, int delay)
        {
            while (true)
            {
                if (proc.HasExited)
                {
                    proc.Start();
                    Thread.Sleep(5000);
                    loginProcedure(proc);
                }
                else
                {
                    wizFarm(proc);
                    Thread.Sleep(delay);
                }
                
            }
        }

        public static void wizFarm(Process proc)
        {
            IntPtr xAddr = (IntPtr)0x00E2EC74;
            IntPtr yAddr = (IntPtr)0x00E2EC78;
            pressKey(proc, Keys.F9);
            Thread.Sleep(50);
            pressKey(proc, Keys.F1);
            //New Era, Kratos IDS
            WriteBytes(proc.Handle, 0x00E2EC74, 509); //X
            WriteBytes(proc.Handle, 0x00E2EC78, 386); //Y
            Thread.Sleep(50);
            for (int i = 0; i < 5; i++)
            {
                mouseClick(proc, (int)Keys.LButton);
            }

        }

        private void stopBtn_Click(object sender, EventArgs e)
        {
            foreach (Thread th in threadList)
            {
                th.Interrupt();
                th.Join();
            }
            threadList.Clear();
        }

    }
}
