using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;


namespace UeiAdapter
{
    class DeviceRef
    {
        //private DeviceRef() { }
        public DeviceRef(int cardTypeIcd, Type deviceAdapterType, Func< MessageObject, object> messageConvertDelegate) //: this()
        {
            CardTypeIcd = cardTypeIcd;
            DeviceAdapterType = deviceAdapterType;
            MessageConvertDelegate = messageConvertDelegate;
            DeviceAdapterRef = null;
        }

        public int CardTypeIcd { get; }
        public Type DeviceAdapterType { get;  }
        public OutputDevice DeviceAdapterRef { get; set;}
        public Func<MessageObject, object> MessageConvertDelegate { get; }
    }

    /// <summary>
    /// The screener gets byte messages from ethernt, 
    /// converts them to message object,
    /// finds appropriate conveter and device adapter
    /// and creates handling task.
    /// </summary>
    internal class Screener : IEnqueue<byte[]>
    {
        log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Dictionary<int, DeviceRef> _deviceRefMap = new Dictionary<int, DeviceRef>();

        BlockingCollection<byte[]> dataItemsQueue = new BlockingCollection<byte[]>(100); // max 100 items
        void f()
        {
            
        }
        public Screener()
        {
            // add devices to map
            _deviceRefMap.Add(0, new DeviceRef(0, typeof(AO308Adapter), Converters.AO308Convert));
        }

        public void Enqueue(byte[] byteMessage)
        {
            dataItemsQueue.Add(byteMessage);
        }

        internal void Start()
        {
            Task.Factory.StartNew(() => MainTask());
        }

        private void MainTask()
        {
            while (false == dataItemsQueue.IsCompleted)
            {
                // get message from q
                byte[] byteMessage = dataItemsQueue.Take();
                MessageObject mo = new MessageObject(byteMessage);

                if (_deviceRefMap.ContainsKey(mo.CardType))
                {
                    DeviceRef dr = _deviceRefMap[mo.CardType];
                    if (null == dr.DeviceAdapterRef)
                    {
                        dr.DeviceAdapterRef = (OutputDevice)Activator.CreateInstance(dr.DeviceAdapterType);
                        dr.DeviceAdapterRef.OpenDeviceSession(@"pdna://192.168.100.2/Dev0/Ao0");
                        logger.Debug(string.Format($"New instance of {dr.DeviceAdapterType.ToString()} created "));
                    }

                    Task.Factory.StartNew(
                        () =>
                        {
                            object req = dr.MessageConvertDelegate(mo);
                            dr.DeviceAdapterRef.HandleRequest(req);
                        }
                    );
                }
                else
                {
                    logger.Warn(string.Format($"Could not find entry for device type {mo.CardType}"));
                }
            }
        }
    }
}