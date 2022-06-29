using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UeiDaq;

namespace UeiAdapter
{
    /// <summary>
    ///  This singlton class can create device objects of type IDevice
    ///  It holds a list of available devices and their slot index.
    /// </summary>
    public class DeviceFactroy
    {
        log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static DeviceFactroy _instace = new DeviceFactroy();
        public static DeviceFactroy Instance1
        {
            get { return _instace; }
        }
        //List<Type> _deviceList = new List<Type>();
        Dictionary<string, int> _config;
        
        public DeviceFactroy()
        {
            //_deviceList.Add(typeof(AO308Adapter));
            string configString = System.IO.File.ReadAllText(@"Config.json");
            _config = JsonConvert.DeserializeObject<Dictionary<string, int>>(configString);

        }

        public Dictionary<int, DeviceInfo> DeviceMap { get => _deviceMap; }

        public class DeviceInfo
        {
            public DeviceInfo(string deviceName, int slotIndex, string resourceName, Type deviceObjectType) //IDevice deviceObject) 
            {
                DeviceName = deviceName;
                SlotIndex = slotIndex;
                ResourceName = resourceName;
                //DeviceObject = deviceObject;
                DeviceObjectType = deviceObjectType;
            }
            public string DeviceName { get; }
            public int SlotIndex { get; } // zero based
            public string ResourceName { get; }
            public IDevice DeviceObject { get; }
            public Type DeviceObjectType { get; }
        }

        Dictionary<int, DeviceInfo> _deviceMap = new Dictionary<int, DeviceInfo>();

        /// <summary>
        /// Create CardManager object
        /// </summary>
        /// <param name="analogOut"></param>
        /// <returns></returns>
        //public OutDevice NewDeviceObject(int ctype)
        //{
        //    //if (ctype == int.AO308)
        //    //    return new AO308Adapter("url");

        //    return null;
        //}

        internal bool ReadDeviceInfo()
        {
            DeviceCollection devColl = new DeviceCollection(@"pdna://192.168.100.2/");
            int unknownDeviceIndex = 100;

            foreach (Device dev in devColl)
            {
                if (dev != null)
                {
                    string name = dev.GetDeviceName();
                    int index = dev.GetIndex();
                    var res = dev.GetResourceName();
                    //var y = dev.GetSerialNumber();
                    //var slot = dev.GetSlot();
                    //var m = dev.GetStatus();

                    int icd_idx = Find_Icd_DeviceIndex(name);
                    if (icd_idx < 0)
                    {
                        icd_idx = unknownDeviceIndex++;
                    }


                    {
                        //IDevice idev = FindCreateDeviceObject(name);
                        Type dt = GetDeviceType(name);
                        //if (dt != null)
                        {
                            DeviceMap.Add(icd_idx, new DeviceInfo(name, index, res, dt));
                        }
                    }
                    
                }
            }
            return true;
        }

        private Type GetDeviceType(string name)
        {
            switch(name)
            {
                case "AO-308":
                    return typeof(AO308Adapter);
            }

            return null;
        }

        private IDevice FindCreateDeviceObject(string name)
        {
            throw new NotImplementedException();
        }

        private int Find_Icd_DeviceIndex(string name)
        {
            foreach (var entry in _config)
            {
                if (entry.Key == name)
                    return entry.Value;
            }
            return -1;
        }

        //    private int FindIcdIndex(string name)
        //    {
        //        foreach (var x in Enum.GetNames(typeof(int)))
        //        {
        //            if (name.Replace('-', '-').Equals( x))
        //            {
        //                return Enum.
        //            }

        //        }

        //    }
    }
}
