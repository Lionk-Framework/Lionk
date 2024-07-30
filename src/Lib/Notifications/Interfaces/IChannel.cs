using Notifications.Classes;

namespace Notifications.Interfaces
{
    public interface IChannel
    {
        string Name { get; }
        bool IsInitialized { get; }

        void Initialize();
        void TestChannel();
        void Send(Notification notification);
    }
}