using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MvvmSize = MvvmCalculatorVMLib.Size;
using MvvmPoint = MvvmCalculatorVMLib.Point;
using System.Collections.ObjectModel;

namespace MvvmCalculator.FunctionPlotter
{
    // Important: Since MvvmForms uses dynamic Event binding, we cannot use EventHandler<t>.
    public delegate void MvvmRenderSizeChangedEventHandler(object sender, EventArgs eArgs);
    public delegate void MvvmScalingChangedEventHandler(object sender, EventArgs eArgs);
    public delegate void PointsToPlotChangedEventHandler(object sender, EventArgs eArgs);

    public partial class FunctionPlotterRenderer : UserControl
    {
        public event MvvmRenderSizeChangedEventHandler MvvmRenderSizeChanged;
        public event MvvmScalingChangedEventHandler MvvmScalingChanged;
        public event PointsToPlotChangedEventHandler PointsToPlotChanged;

        private MvvmSize? myScaling;
        private ObservableCollection<MvvmPoint> myPointsToPlot;

        public FunctionPlotterRenderer()
        {
            InitializeComponent();
            ResizeRedraw = true;
            this.DoubleBuffered = true;
        }

        /// <summary>
        /// Delegates the actual controls size to the ViewModel.
        /// </summary>
        /// <remarks>
        /// We cannot use ClientSize or Size of the control here, because that would require to have 
        /// OneWayToSource Binding, which is not available in UWP. So, we implement a new 
        /// Resize property, and marshal the actual size only in the --> ViewModel direction. The other 
        /// direction we simply ignore, and do not assign back to the control's size.
        /// </remarks>
        public MvvmSize? MvvmRenderSize
        {
            get
            {
                return new MvvmSize()
                {
                    Width = ClientSize.Width,
                    Height = ClientSize.Height
                };
            }

            set
            {
                // One way to source binding, so, we ignore this.
            }
        }

        protected virtual void OnMvvmRenderSizeChanged(EventArgs eArgs)
        {
            if (MvvmRenderSizeChanged!=null) 
                MvvmRenderSizeChanged(this, eArgs);
        }
        
        public MvvmSize? MvvmScaling
        {
            get
            {
                return myScaling;
            }

            set
            {
                if (!object.Equals(myScaling, value))
                {
                    myScaling = value;
                    OnMvvmScalingChanged(EventArgs.Empty);
                }
            }
        }

        protected virtual void OnMvvmScalingChanged(EventArgs eArgs)
        {
            if (MvvmScalingChanged != null)
                this?.MvvmScalingChanged(this, eArgs);

            this.Invalidate();
        }

        public ObservableCollection<MvvmPoint> PointsToPlot
        {
            get
            {
                return myPointsToPlot;
            }

            set
            {
                if (!object.Equals(myPointsToPlot, value))
                {
                    myPointsToPlot = value;
                    OnPointsToPlotChanged(EventArgs.Empty);
                }
            }
        }

        protected virtual void OnPointsToPlotChanged(EventArgs eArgs)
        {
            if (PointsToPlotChanged!=null)
                PointsToPlotChanged(this, eArgs);

            this.Invalidate();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            OnMvvmRenderSizeChanged(EventArgs.Empty);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            MvvmPoint? lastPoint = null;

            e.Graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, this.ClientSize.Width - 1, this.ClientSize.Height - 1));
            e.Graphics.DrawRectangle(Pens.Black, new Rectangle(0, 0, this.ClientSize.Width-1, this.ClientSize.Height-1));

            if (MvvmScaling.HasValue && PointsToPlot!= null)
            {
                e.Graphics.ScaleTransform((float)MvvmScaling.Value.Width,
                                          (float)MvvmScaling.Value.Height);

                foreach (var pointItem in PointsToPlot) 
                {
                    if (!lastPoint.HasValue)
                    {
                        lastPoint = pointItem;
                    }
                    else
                    {
                        e.Graphics.DrawLine(Pens.Black,
                            new PointF((float)lastPoint.Value.X,
                                       (float)lastPoint.Value.Y),
                            new PointF((float)pointItem.X,
                                        (float)pointItem.Y));
                        lastPoint = pointItem;
                    }

                }
            }
        }
    }
}
