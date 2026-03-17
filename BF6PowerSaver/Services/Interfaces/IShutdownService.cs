using System.Threading.Tasks;

namespace BF6PowerSaver.Services
{
    public interface IShutdownService
    {
        /// Request a system shutdown after the specified delay (in seconds).
        /// Implementations should not throw for normal failures; return a Task for async tests.
        Task RequestShutdownAsync(int delaySeconds = 30);
    }
}