using System;
using System.Threading;
using System.Threading.Tasks;

namespace ActiveDevelop.MvvmBaseLib.Threading
{
    public class Timer : CancellationTokenSource, IDisposable
    {
        private Task myTimerTask;
        private int myPeriod;
        private int myDueTime;
        private Action myCallBack;

        public Timer(Action callBack)
        {
            if (callBack == null)
                throw new NullReferenceException("Timer Callback must not nothing (null in CSharp)");
            myCallBack = callBack;
        }

        public Timer(Action callBack, object state, int dueTime, int period)
        {
            if (callBack == null)
                throw new NullReferenceException("Timer Callback must not nothing (null in CSharp)");

            if (dueTime == Timeout.Infinite && period == Timeout.Infinite)
                return;

            myPeriod = period;
            myDueTime = dueTime;
            myCallBack = callBack;

            myTimerTask = Task.Run(new Func<Task>(TimerLoop), this.Token);
        }

        private async Task TimerLoop()
        {
            await Task.Delay(myDueTime, this.Token).ContinueWith(
                async (state) =>
                {
                    myCallBack.Invoke();

                    do
                    {
                        await Task.Delay(myPeriod, this.Token).ConfigureAwait(false);
                        myCallBack.Invoke();
                    } while (true);
                }, this.Token);
        }

        public new void Dispose()
        {
            base.Cancel();
        }
    }
}
