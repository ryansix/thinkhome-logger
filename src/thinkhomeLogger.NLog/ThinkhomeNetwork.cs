﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ThinkhomeLogger
{
    /// <summary>
    /// thinkhome mac address
    /// </summary>
    public class ThinkhomeNetwork
    { 
        private static string macAddress = "00-00-00-00-00-00";
        private static string computerName = string.Empty;
        private static string ipAddress = "00.00.00.00";
        /// <summary>
        /// 获取电脑名称
        /// </summary>
        /// <returns></returns>
        public static string GetComputerName()
        {
            if (string.IsNullOrWhiteSpace(computerName))
                computerName = System.Net.Dns.GetHostName();
            return computerName;
        }

        /// <summary>
        /// 获取电脑 MAC（物理） 地址
        /// </summary>
        /// <returns></returns>
        public static string GetIPAddress()
        {
            if (!string.IsNullOrEmpty(ipAddress) && !ipAddress.StartsWith("00.00"))
                return ipAddress; 
            var mic = GetNetworkInterface();
            var c = mic.GetIPProperties();
            ipAddress = (c.UnicastAddresses.FirstOrDefault(u => u.Address.AddressFamily == AddressFamily.InterNetwork).Address)?.ToString(); 
            return ipAddress;
        }
        /// <summary>
        /// 获取电脑 MAC（物理） 地址
        /// </summary>
        /// <returns></returns>
        public static string GetMACAddress()
        {
            if (!string.IsNullOrEmpty(macAddress) && !macAddress.StartsWith("00-00-00"))
                return macAddress;

            var bytes = GetMacAddress().GetAddressBytes();
            macAddress = BitConverter.ToString(bytes); ;
            return macAddress;
        }
        /// <summary>
        /// Gets the MAC address of the current PC.
        /// </summary>
        /// <returns></returns>
        private static PhysicalAddress GetMacAddress()
        {
            var nic = GetNetworkInterface();
            if (nic != null)
            {
                return nic.GetPhysicalAddress();
            }
            return new PhysicalAddress(new byte[] { 0, 0, 0, 0, 0, 0 });
        }

       

        private static NetworkInterface GetNetworkInterface()
        {
            // order interfaces by speed and filter out down and loopback
            // take first of the remaining
            var interfaces = NetworkInterface.GetAllNetworkInterfaces().ToList();  
            if (interfaces == null || !interfaces.Any())
                return null;
          
            Expression<Func<NetworkInterface, bool>>  predicate = c => c.NetworkInterfaceType != NetworkInterfaceType.Loopback
             && c.OperationalStatus == OperationalStatus.Up
             && (!c.Description.ToLower().Contains("virtu"))
             && (!c.Description.Contains("Pseudo")); 

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                predicate = predicate.And(c => c.Name.ToLower().Contains("wlan"));
            else
                predicate = predicate.And(c => c.Name.ToLower().Contains("eth")); 
            var firstUpInterface = interfaces .FirstOrDefault(predicate.Compile());

            return firstUpInterface;
        }

 
    }
}
