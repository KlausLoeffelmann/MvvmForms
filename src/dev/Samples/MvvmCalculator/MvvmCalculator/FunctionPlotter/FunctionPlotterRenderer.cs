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

    public partial class FunctionPlotterRenderer : UserControl
    {
        public event EventHandler<EventArgs> MvvmRenderSizeChanged;
        public event EventHandler<EventArgs> MvvmScalingChanged;
        public event EventHandler<EventArgs> PointsToPlotChanged;

        private MvvmSize? myScaling;
        private ObservableCollection<MvvmPoint> myPointsToPlot;

        public FunctionPlotterRenderer()
        {
            InitializeComponent();
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

        //Although we never need this really, we have to have it, so the binding engine would discover the property.
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

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            MvvmPoint? lastPoint = null;

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
                    }

                }
            }
        }
    }
}
