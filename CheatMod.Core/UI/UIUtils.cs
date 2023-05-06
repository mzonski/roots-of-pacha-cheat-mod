using System;
using System.Threading;
using System.Threading.Tasks;

namespace CheatMod.Core.UI;

public static class UIUtils
{
    public static Action<T> Debounce<T>(this Action<T> func, int milliseconds = 300)
    {
        CancellationTokenSource cancelTokenSource = null;

        return arg =>
        {
            cancelTokenSource?.Cancel();
            cancelTokenSource = new CancellationTokenSource();
            Task.Delay(milliseconds, cancelTokenSource.Token)
                .ContinueWith(t =>
                {
                    if (t.IsCompleted)
                    {
                        func(arg);
                    }
                }, TaskScheduler.Default);
        };
    }
}