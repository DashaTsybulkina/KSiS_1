using System;
using System.Diagnostics;
using System.Net.NetworkInformation;

namespace Lab1_MacAdress
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            IPGlobalProperties computerProperties = IPGlobalProperties.GetIPGlobalProperties();
            Console.WriteLine("Interface information for {0}.{1}     ", computerProperties.HostName, computerProperties.DomainName);
            ShowMac();
            ShowShareNames();
            Console.ReadKey();
        }


        private static void ShowMac()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            if (nics == null || nics.Length < 1)
            {
                Console.WriteLine("  No network interfaces found.");
                return;
            }

            foreach (NetworkInterface adapter in nics)
            {
                if (!string.IsNullOrEmpty(adapter.GetPhysicalAddress().ToString()))
                {
                    Console.WriteLine($"{adapter.Description}\n            {String.Join("-", adapter.GetPhysicalAddress().GetAddressBytes())}");
                    /*Console.WriteLine();
                    Console.WriteLine(adapter.Description);
                    Console.Write("  Adapter physical address: ");
                    Console.WriteLine(String.Join("-", adapter.GetPhysicalAddress().GetAddressBytes()));
                    Console.WriteLine();*/
                }
            }
        }
        
        private static void ShowShareNames()
        {
            Console.WriteLine("\nNetwork computers:");
            var proc = new Process
            {
                StartInfo =
                {
                    FileName = "cmd",
                    Arguments = "/c net view",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            proc.Start();
            PrintShareNames(proc);
        }


        private static void PrintShareNames(Process proc)
        {
            string data = proc.StandardOutput.ReadToEnd();
            int start = 0;
            while (true)
            {
                start = data.IndexOf('\\', start);
                if (start == -1)
                    break;
                var stop = data.IndexOf('\n', start);
                Console.WriteLine("   {0}", data.Substring(start, stop - start));
                start = stop;
            }
        }
    }
}
