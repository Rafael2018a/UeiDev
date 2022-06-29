using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UeiAdapter
{

    public class MessageObject
    {
        private static int _serial = 0;
        public bool IsValidMessage { get; }
        public int UnitId { get; }
        public int CardType { get; }
        public int SlotNumber { get; }
        public int SlotChannelNumber { get; }
        public byte[] OrigByteMessage { get; }
        public int _debugSerial { get => _serial;  } // serial number of message
        public MessageObject(byte[] byteMessage)
        {
            OrigByteMessage = byteMessage;
            _serial++;

            //if (BitConverter.IsLittleEndian)
            //    Array.Reverse(bytes);

            int uniqId = BitConverter.ToInt32(byteMessage, 0);
            CardType = byteMessage[4];

            // ============================================
            byte[] bytes1 = BitConverter.GetBytes(201805978);
            //Console.WriteLine("byte array: " + BitConverter.ToString(bytes1));
        }

    }


}
