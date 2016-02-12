using System;
using System.Threading;
using System.Threading.Tasks;

namespace ActiveDevelop.MvvmBaseLib.Threading
{
    /// <summary>
    /// A timer which can reside in portable class libraries and has no additional references besides System.Threading.
    /// </summary>
    public class Timer : CancellationTokenSource, IDisposable
    {
        private Task myTimerTask;
        private int myPeriod;
        private int myDueTime;
        private Action myCallBack;

        /// <summary>
        /// Creates a new instance of this class and passes the callback. Call Dispose to cancel the timer.
        /// </summary>
        /// <param name="callBack">Action delegate, which is called, when the timer has ellapsed.</param>
        public Timer(Action callBack)
        {
            if (callBack == null)
                throw new NullReferenceException("Timer Callback must not nothing (null in CSharp)");
            myCallBack = callBack;
        }

        /// <summary>
        /// Creates a new instance of this class and passes the timer control parameters. Call Dispose to cancel the timer.
        /// </summary>
        /// <param name="callBack">Action delegate, which is called, when the timer has ellapsed.</param>
        /// <param name="state">An abitrary state object for own purposes.</param>
        /// <param name="dueTime">Milliseconds, when the timer should firstly be ellapsed.</param>
        /// <param name="period">Period of timer ellapsing in milliseconds.</param>
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
