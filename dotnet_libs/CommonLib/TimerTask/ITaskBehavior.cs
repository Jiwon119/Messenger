using System.Threading.Tasks;

namespace CommonLib.TimerTask
{

    public interface ITaskBehaviour
    {
        bool IsTimerTaskComplete { get; set; }
        int TimerTaskHash { get; set; }

        void OnInitTimerTask();
        Task<bool> DoTimerTask();

        void OnDisposed();
    }
}
