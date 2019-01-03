using System;
using System.IO;
using System.Management;
using RestSharp;
using RestSharp.Authenticators;
using System.Linq;
using Microsoft.VisualBasic.Devices;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Diagnostics;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace MonitorV2
{
    class MyDevice
    {
        public static string getPcName()
        {
            string pcName = Environment.MachineName.ToString();
            return pcName;

        }

        //Get Local IP Address
        public static string getLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");

        }

        //Gets Mac Address()
        public static string getMACAddress()
        {



            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            String sMacAddress = string.Empty;
            foreach (NetworkInterface adapter in nics)
            {
                if (sMacAddress == String.Empty)// only return MAC Address from first card  
                {
                    IPInterfaceProperties properties = adapter.GetIPProperties();
                    sMacAddress = adapter.GetPhysicalAddress().ToString();
                }
            }

            return sMacAddress;
            //Library.WriteErrorLog("Registering");


        }

        //Gets IP external Address()
        public static string getExternalIp()
        {
            try
            {
                string externalIP;
                externalIP = (new WebClient()).DownloadString("http://checkip.dyndns.org/");
                externalIP = (new Regex(@"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}"))
                             .Matches(externalIP)[0].ToString();


                return externalIP;
            }
            catch { return null; }
        }

        public static string getProcessor()
        {

            ManagementObjectSearcher myProcessorObject = new ManagementObjectSearcher("select * from Win32_Processor");
            List<string> list = new List<string>();

            foreach (ManagementObject obj in myProcessorObject.Get())
            {
               
                list.Add((""+ obj["Name"]));

            }

            return list[0].ToString();
            
        }

        public static string getNetworkcards()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();

            if (nics == null || nics.Length < 1)
            {
                Console.WriteLine("  No network interfaces found.");
            }
            else
            {
                foreach (NetworkInterface adapter in nics)
                {
                    IPInterfaceProperties properties = adapter.GetIPProperties();
                    Console.WriteLine();
                    Console.WriteLine(adapter.Description);
                    Console.WriteLine(String.Empty.PadLeft(adapter.Description.Length, '='));
                    Console.WriteLine("  Interface type .......................... : {0}", adapter.NetworkInterfaceType);
                    Console.WriteLine("  Physical Address ........................ : {0}", adapter.GetPhysicalAddress().ToString());
                    Console.WriteLine("  Operational status ...................... : {0}", adapter.OperationalStatus);
                }
            }

            return "";
        }

        public static void showNetworkInterfaces()
        {
            IPGlobalProperties computerProperties = IPGlobalProperties.GetIPGlobalProperties();
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            Console.WriteLine("Interface information for {0}.{1}     ",
                    computerProperties.HostName, computerProperties.DomainName);
            if (nics == null || nics.Length < 1)
            {
                Console.WriteLine("  No network interfaces found.");
                return;
            }

            Console.WriteLine("  Number of interfaces .................... : {0}", nics.Length);

            foreach (NetworkInterface adapter in nics)
            {
                IPInterfaceProperties properties = adapter.GetIPProperties();
                Console.WriteLine();
                Console.WriteLine(adapter.Description);
                Console.WriteLine(String.Empty.PadLeft(adapter.Description.Length, '='));
                Console.WriteLine("  Interface type .......................... : {0}", adapter.NetworkInterfaceType);
                Console.WriteLine("  Physical Address ........................ : {0}",
                            adapter.GetPhysicalAddress().ToString());
                Console.WriteLine("  Operational status ...................... : {0}",
                    adapter.OperationalStatus);
                string versions = "";

                // Create a display string for the supported IP versions.
                if (adapter.Supports(NetworkInterfaceComponent.IPv4))
                {
                    versions = "IPv4";
                }
                if (adapter.Supports(NetworkInterfaceComponent.IPv6))
                {
                    if (versions.Length > 0)
                    {
                        versions += " ";
                    }
                    versions += "IPv6";
                }
                Console.WriteLine("  IP version .............................. : {0}", versions);
                //ShowIPAddresses(properties);

                // The following information is not useful for loopback adapters.
                if (adapter.NetworkInterfaceType == NetworkInterfaceType.Loopback)
                {
                    continue;
                }
                Console.WriteLine("  DNS suffix .............................. : {0}",
                    properties.DnsSuffix);

                string label;
                if (adapter.Supports(NetworkInterfaceComponent.IPv4))
                {
                    IPv4InterfaceProperties ipv4 = properties.GetIPv4Properties();
                    Console.WriteLine("  MTU...................................... : {0}", ipv4.Mtu);
                    if (ipv4.UsesWins)
                    {

                        IPAddressCollection winsServers = properties.WinsServersAddresses;
                        if (winsServers.Count > 0)
                        {
                            label = "  WINS Servers ............................ :";
                            //ShowIPAddresses(label, winsServers);
                        }
                    }
                }

                Console.WriteLine("  DNS enabled ............................. : {0}",
                    properties.IsDnsEnabled);
                Console.WriteLine("  Dynamically configured DNS .............. : {0}",
                    properties.IsDynamicDnsEnabled);
                Console.WriteLine("  Receive Only ............................ : {0}",
                    adapter.IsReceiveOnly);
                Console.WriteLine("  Multicast ............................... : {0}",
                    adapter.SupportsMulticast);
            }
        }

        public static string WindowsVer()
        {
            var name = (from x in new ManagementObjectSearcher("SELECT Caption FROM Win32_OperatingSystem").Get().Cast<ManagementObject>()
                        select x.GetPropertyValue("Caption")).FirstOrDefault();

     

            return name.ToString();

        }

        public static string AntiVirus()
        {
           
            using (var antiVirusSearch = new ManagementObjectSearcher(@"\\" + Environment.MachineName + @"\root\SecurityCenter2", "Select * from AntivirusProduct"))
            {
                var getSearchResult = antiVirusSearch.Get();
                foreach (var searchResult in getSearchResult)
                {
                    
                    string WindowsAntivirus1 = searchResult["displayName"].ToString();
                    return WindowsAntivirus1;
               
                }

                return null;
            }
            
        }
    
        public static string FreeHDDSpace()
        {
            try
            {
                DriveInfo driveInfo = new DriveInfo(@"C:");
                long FreeSpace = driveInfo.AvailableFreeSpace;
                long GB = 1024;
                long answer1 = FreeSpace / GB / GB / GB;

                string WindowsHHDSpace1 = answer1.ToString() + " " + "GB";

                return WindowsHHDSpace1;
            }
            catch
            {
                return null;

            }

        }

        public static string PhysicalMemory()
        {
            try
            {
                ComputerInfo ci = new ComputerInfo();
                double mem = ci.TotalPhysicalMemory;
                long GB = 1024;

                double answer1 = mem / GB / GB;
                double after3 = Math.Round(answer1);
                string WindowsAvMemory1 = after3.ToString() + "MB";

                return WindowsAvMemory1;

            }
            catch
            {
                return null;
            }

        }

        public static void InstalledProgrames()
        {

            ManagementObjectSearcher mos = new ManagementObjectSearcher("SELECT * FROM Win32_Product");
            foreach (ManagementObject mo in mos.Get())
            {
                Console.WriteLine(mo["Name"]);
            }
        }

        public static TimeSpan GetUptime()
        {
            var mo = new ManagementObject(@"\\.\root\cimv2:Win32_OperatingSystem=@");
            var lastBootUp = ManagementDateTimeConverter.ToDateTime(mo["LastBootUpTime"].ToString());
            return DateTime.Now.ToUniversalTime() - lastBootUp.ToUniversalTime();

        }

        public static void hddSpaceCha()
        {
            try
            {
                DriveInfo driveInfo = new DriveInfo(@"C:");
                long totalSize = driveInfo.TotalSize;
                long FreeSpace = driveInfo.AvailableFreeSpace;
                long GB = 1024;
                long answer1 = FreeSpace / GB / GB / GB;
                long answer2 = totalSize / GB / GB / GB;
                long answer3 = answer2 - answer1;

                string totalSpace = answer2.ToString();
                string freeSpace = answer1.ToString();

                

            }
            catch
            {


            }
        }

        public static string TotalSpace()
        {
            try
            {
                DriveInfo driveInfo = new DriveInfo(@"C:");
                long totalSize = driveInfo.TotalSize;
                long FreeSpace = driveInfo.AvailableFreeSpace;
                long GB = 1024;
                long answer2 = totalSize / GB / GB / GB;

                string totalspace = answer2.ToString();

                return totalspace;




            }
            catch
            {
                return null;

            }
        }

        public static string freeSpace()
        {
            try
            {
                DriveInfo driveInfo = new DriveInfo(@"C:");
                long FreeSpace = driveInfo.AvailableFreeSpace;
                long GB = 1024;
                long answer1 = FreeSpace / GB / GB / GB;

                string freeSpace = answer1.ToString();

                return freeSpace;



            }
            catch
            {

                return null;
            }
        }

        public static string usedSpace()
        {
            try
            {
                DriveInfo driveInfo = new DriveInfo(@"C:");
                long totalSize = driveInfo.TotalSize;
                long FreeSpace = driveInfo.AvailableFreeSpace;
                long GB = 1024;
                long answer1 = FreeSpace / GB / GB / GB;
                long answer2 = totalSize / GB / GB / GB;
                long answer3 = answer2 - answer1;

                string totalSpace = answer2.ToString();
                string freeSpace = answer1.ToString();

                return answer3.ToString();


            }
            catch
            {

                return null;

            }
        }

        public static void getCPUStats()
        {

            var processName = Process.GetCurrentProcess().ProcessName;
            PerformanceCounter ramCounter = new PerformanceCounter("Process", "Working Set - Private", processName);
            PerformanceCounter cpuCounter = new PerformanceCounter("Process", "% Processor Time", processName);

            while (true)
            {
                double ram = ramCounter.NextValue();
                double cpu = cpuCounter.NextValue() / Environment.ProcessorCount;

                Console.Clear();
                Console.WriteLine("RAM: "
                                  + (ram / 1024).ToString("N0") + " KB; CPU: "
                                  + cpu.ToString("N1") + " %;");

                Thread.Sleep(500);
            }

        }

    }
}
