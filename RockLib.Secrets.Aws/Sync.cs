using System;
using System.Threading;
using System.Threading.Tasks;

namespace RockLib.Secrets.Aws
{
    internal static class Sync
    {
        public static TResult OverAsync<TResult>(Func<Task<TResult>> getTaskOfTResult)
        {
            SynchronizationContext old = SynchronizationContext.Current;
            try
            {
                SynchronizationContext.SetSynchronizationContext(null);
                return getTaskOfTResult().GetAwaiter().GetResult();
            }
            finally
            {
                SynchronizationContext.SetSynchronizationContext(old);
            }
        }
    }
}
