using System.IO;
using UeiDaq;

namespace UeiAdapter
{
    internal class AO308Adapter : OutputDevice//, IEnqueue<MessageObject>
    {
        Session _deviceSession;
        //private string _cardUrl;
        public static string devId = "AO-308";

        log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public bool OpenDeviceSession(string deviceUrl)
        {
            _deviceSession = new Session();
            _deviceSession.CreateAOChannel(deviceUrl, -10.0, 10.0);
            int n = _deviceSession.GetNumberOfChannels();
            //int i = _deviceSession.GetChannel(n-1).GetIndex();
            _deviceSession.ConfigureTimingForSimpleIO();

            return true;
        }
        public void CloseDeviceSession()
        {
            throw new System.NotImplementedException();
        }

        public void HandleRequest(object v)
        {
            double[] scan = (double[])v;

            //logger.Info(string.Format("Out: {0}", scan[0]));

            var writer = new AnalogScaledWriter( _deviceSession.GetDataStream());

            writer.WriteSingleScan(scan);
        }


    }
}