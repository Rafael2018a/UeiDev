using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Newtonsoft.Json;
using System.IO;

namespace UeiAdapter
{
    //public class Account
    //{
    //    public string Email { get; set; }
    //    public bool Active { get; set; }
    //    public DateTime CreatedDate { get; set; }
    //    public IList<string> Roles { get; set; }
    //}

    class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();
            p.Run();
        }

        log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        

        private void Run()
        {
            // get config


            DeviceFactroy df = new DeviceFactroy();
            if (false == df.ReadDeviceInfo())
            {
                goto exit;
            }

            foreach (var v in df.DeviceMap)
            {
                var objectType = (v.Value.DeviceObjectType == null) ? "<>" : v.Value.DeviceObjectType.ToString();
                var icdIndex = (v.Key >= 100) ? "Nan" : v.Key.ToString();
                //v.Value.DeviceObjectType.ToString(),
                var s = string.Format("DeviceType {0} Dev{1} ICDIndex:{2} Class:{3}", v.Value.DeviceName, v.Value.SlotIndex, icdIndex, objectType);
                logger.Info(s);
            }

            //OutDevice aoDevice = df.NewDeviceObject(0);
            //bool okFlag = aoDevice.StartDeviceSession();

            Screener mainScreaner = new Screener();
            //mainScreaner.Register(aoDevice, 0);
            mainScreaner.Start();

            UdpReader ureader = new UdpReader(mainScreaner);
            ureader.Start();

            while(true)
            {
                System.Threading.Thread.Sleep(1000);
            }

            exit: return;
        }
    }
}
