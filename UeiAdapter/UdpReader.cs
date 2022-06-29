using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace UeiAdapter
{
    /// <summary>
    /// Reads datagrams from udp channel and sends them to 'consumer'.
    /// </summary>
    internal class UdpReader
    {
        private IEnqueue<byte[]> _datagramConsumer;
        log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public UdpReader(IEnqueue<byte[]> consumer)
        {
            this._datagramConsumer = consumer;
        }

        internal void Start()
        {
            //_datagramConsumer.Enqueue(new byte[] { 10 });
            Task.Run( new Action( MainTask));
        }

        private void MainTask()
        {
            double rawval = 0;
            double samplesPerCycle = 100.0;
            double delta = 2 * Math.PI / samplesPerCycle;

            byte[] byteMessage = new byte[16 + 8];
            byteMessage[5] = 0;

            while (rawval < 1000 * 2 * Math.PI)
            {
                // get message from udp
                double d = 10.0 * Math.Sin(rawval);
                byte [] eight = BitConverter.GetBytes(d);
                eight.CopyTo(byteMessage, 16);

                // send to consumer
                _datagramConsumer.Enqueue(byteMessage);

                rawval += delta;

                Thread.Sleep(1);
            }

            logger.Info("Udp end");

        }
    }

    
}