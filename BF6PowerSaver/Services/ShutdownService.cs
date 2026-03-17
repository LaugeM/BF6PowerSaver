using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace BF6PowerSaver.Services
{
    public class ShutdownService : IShutdownService
    {
        public Task RequestShutdownAsync(int delaySeconds = 30)
        {
            try
            {
                // Use the Windows shutdown command. This will throw if exec fails.
                var psi = new ProcessStartInfo("shutdown", $"/s /t {delaySeconds}")
                {
                    CreateNoWindow = true,
                    UseShellExecute = false
                };
                Process.Start(psi);
            }
            catch
            {
                // swallow or log; do not rethrow to avoid crashing callers.
            }

            return Task.CompletedTask;
        }
    }
}