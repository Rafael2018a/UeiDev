using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/// <summary>
/// All files in project might refer to this file.
/// Types in this file might NOT refer to types in any other file.
/// </summary>
namespace UeiAdapter
{

    public interface IEnqueue<Item>
    {
        void Enqueue(Item i);
    }
    //public enum DeviceType { AO308 = 0}

    /// <summary>
    /// root interface for all out-card managers
    /// </summary>
    public interface OutputDevice: IDevice
    {

        ///
        /// (Thrad safe)
        /// 
        void HandleRequest(object v);

    }

    public interface IDevice //: IEnqueue<MessageObject>
    {
        bool OpenDeviceSession(string deviceUrl);// string accessString, double vMin, double vMax);
        void CloseDeviceSession();
    }

    
}
