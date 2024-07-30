using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Notifications.Classes;
using Serilog.Events;

namespace Notifications.Interfaces
{

    public interface INotifyer
    {
        string Name { get; }
        IEnumerable<IChannel> Channels { get; }
        void Send(Notification notification);
    }

}
