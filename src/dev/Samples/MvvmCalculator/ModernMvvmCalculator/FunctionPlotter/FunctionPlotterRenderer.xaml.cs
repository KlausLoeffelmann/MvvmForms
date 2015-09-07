using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using MvvmSize = MvvmCalculatorVMLib.Size;
using MvvmPoint = MvvmCalculatorVMLib.Point;
using System.Collections.ObjectModel;
using Windows.UI;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace ModernMvvmCalculator.FunctionPlotter
{
    public sealed partial class FunctionPlotterRenderer : UserControl
    {
        private ObservableCollection<MvvmPoint> myPointsToPlot;

        public FunctionPlotterRenderer()
        {
            this.InitializeComponent();
            this.SizeChanged += FunctionPlotterRenderer_SizeChanged;
        }

        private void FunctionPlotterRenderer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            MvvmRenderSize = new MvvmSize()
                                {
                                    Width = ActualWidth,
                                    Height = ActualHeight
                                };
        }

        // Using a DependencyProperty as the backing store for MvvmRenderSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MvvmRenderSizeProperty =
            DependencyProperty.Register("MvvmRenderSize", 
                typeof(MvvmSize?), 
                typeof(FunctionPlotterRenderer),
                new PropertyMetadata(null, (sender, aArgs) =>
                {
                    //Insert callbackcode here.
                }));

        public MvvmSize? MvvmRenderSize
        {
            get { return (MvvmSize?)GetValue(MvvmRenderSizeProperty); }
            set { SetValue(MvvmRenderSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PointsToPlot.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PointsToPlotProperty =
            DependencyProperty.Register("PointsToPlot", 
                typeof(ObservableCollection<MvvmPoint>), 
                typeof(FunctionPlotterRenderer), 
                new PropertyMetadata(null,(sender,eArgs)=> 
                {
                    ((FunctionPlotterRenderer)sender).myPointsToPlot = (ObservableCollection<MvvmPoint>)eArgs.NewValue;
                    ((FunctionPlotterRenderer)sender).myCanvasControl.Invalidate();
                }));

        public ObservableCollection<MvvmPoint> PointsToPlot
        {
            get
            {
                return (ObservableCollection<MvvmPoint>)GetValue(PointsToPlotProperty);
            }
            set
            {
                SetValue(PointsToPlotProperty, value);
            }
        }


        private void myCanvasControl_Draw(Microsoft.Graphics.Canvas.UI.Xaml.CanvasControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasDrawEventArgs args)
        {
            MvvmPoint? lastPoint = null;

            //await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            //{
            //    pointsToPlotTemp = PointsToPlot;
            //}).AsTask().ConfigureAwait(true);

            if (myPointsToPlot != null)
            {
                var minX = PointsToPlot.Min((item) => item.X);
                var minY = PointsToPlot.Min((item) => item.Y);

                //var drawingBrush = new Microsoft.Graphics.Canvas.Brushes.CanvasSolidColorBrush(
                //    args.DrawingSession.Device, Colors.White);

                foreach (var pointItem in PointsToPlot)
                {
                    if (!lastPoint.HasValue)
                    {
                        lastPoint = pointItem;
                    }
                    else
                    {
                        try
                        {
                            //We just ignore weird calculation results.

                            args.DrawingSession.DrawLine((float)lastPoint.Value.X,
                                                         (float)lastPoint.Value.Y,
                                                         (float)pointItem.X,
                                                         (float)pointItem.Y,
                                                         Colors.White,3);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }

                        lastPoint = pointItem;
                    }

                }
            }
        }
    }
}
