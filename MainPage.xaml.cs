using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.Graphics.Canvas.UI.Xaml;
using minimum.Methods;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x419

namespace minimum
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        private AbstractSearch search;
        private readonly double A = -Math.PI / 7;
        private readonly double B = Math.PI / 2;

        private double eps = 0.001;

        private double resultNumeric   = 0;
        private double resultFuncValue = 0;
        
        private readonly int scale = 200;
        private readonly int offset = 125;

        ObservableCollection<string> searchMethods { get; } = new ObservableCollection<string>();
        ObservableCollection<double> precisions { get; } = new ObservableCollection<double>();
        
        string selectedMethod = "Uniform search";
        double selectedPrecision = 0.001;

        private double F(double x)
        {
            return -(Math.Sin(x) + Math.Cos(x));
        }

        public MainPage()
        {
            InitializeComponent();

            searchMethods.Add("Uniform search");
            searchMethods.Add("Division search");
            searchMethods.Add("Fibonacci search");
            searchMethods.Add("Golden ratio search");

            precisions.Add(0.001);
            precisions.Add(0.000001);
            precisions.Add(0.000000000001);
        }

        void canvasControl_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            var canvas = args.DrawingSession;

            var fontStyle = new CanvasTextFormat
            {
                FontSize = 12
            };

            float widthDependantOffset  = (float)canvasControl.ActualWidth  / 500 * offset;
            float heightDependantOffset = (float)canvasControl.ActualHeight / 500 * offset;

            float widthDependantScale  = (float)canvasControl.ActualWidth  / 500 * scale;
            float heightDependantScale = (float)canvasControl.ActualHeight / 500 * scale;

            canvas.DrawLine(x0: ((float)A * widthDependantScale) + widthDependantOffset,
                            y0: heightDependantOffset,
                            x1: ((float)B * widthDependantScale) + widthDependantOffset,
                            y1: heightDependantOffset,
                            color: Colors.Black);

            canvas.DrawLine(x0: widthDependantOffset,
                            y0: heightDependantOffset,
                            x1: widthDependantOffset,
                            y1: heightDependantOffset + (heightDependantOffset * 2.5F),
                            color: Colors.Black);

            for (double i = -Math.PI / 8; i < Math.PI / 2; i += (Math.PI / 16))
            {
                double x = i * widthDependantScale + widthDependantOffset;

                canvas.FillCircle((float)x,
                                  heightDependantOffset,
                                  3,
                                  Colors.Blue);
                
                canvas.DrawText(string.Format("{0:F2}", i),
                                (float)x,
                                heightDependantOffset - 20,
                                Colors.Black,
                                fontStyle);
            }

            canvas.FillCircle(widthDependantOffset,
                              ((float)F(0) * -heightDependantScale) + heightDependantOffset,
                              4,
                              Colors.Blue);

            canvas.DrawText(string.Format("{0:F2}", -1),
                                widthDependantOffset - 35,
                                ((float)F(0) * -heightDependantScale) + heightDependantOffset - 10,
                                Colors.Black,
                                fontStyle);


            for (double x = A; x <= B; x += 0.01)
            {
                double y = F(x);

                double newX = x *  widthDependantScale  + widthDependantOffset;
                double newY = y * -heightDependantScale + heightDependantOffset;

                canvas.DrawRectangle((float)newX, (float)newY, 1, 1, Colors.Black);

            }

            if (resultNumeric != 0)
            {
                CanvasStrokeStyle strokeStyle = new CanvasStrokeStyle
                {
                    DashStyle = CanvasDashStyle.DashDot
                };

                canvas.DrawLine((float)(resultNumeric   *  widthDependantScale + widthDependantOffset),
                                (float)(resultFuncValue * -heightDependantScale + heightDependantOffset),
                                (float)(resultNumeric   *  widthDependantScale + widthDependantOffset),
                                heightDependantOffset,
                                Colors.Green,
                                0.5F,
                                strokeStyle);

                canvas.DrawLine((float)((resultNumeric   * widthDependantScale) + widthDependantOffset),
                                (float)((resultFuncValue * -heightDependantScale) + heightDependantOffset),
                                widthDependantOffset,
                                (float)(resultFuncValue * -heightDependantScale + heightDependantOffset),
                                Colors.Green,
                                0.5F,
                                strokeStyle);

                canvas.FillCircle(widthDependantOffset,
                                  (float)(resultFuncValue * -heightDependantScale + heightDependantOffset),
                                  4,
                                  Colors.Green);

                canvas.DrawText(string.Format("{0:F2}", resultFuncValue),
                                widthDependantOffset - 35,
                                (float)(resultFuncValue * -heightDependantScale + heightDependantOffset) - 10,
                                Colors.Black,
                                fontStyle);

                canvas.FillCircle((float)(resultNumeric   *  widthDependantScale + widthDependantOffset),
                                  (float)(resultFuncValue * -heightDependantScale + heightDependantOffset),
                                  4,
                                  Colors.Red);
            }
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            eps = selectedPrecision;

            switch (selectedMethod)
            {
                case "Uniform search":
                    search = new UniformSearch(A, B, eps, 10, F);
                    break;
                case "Division search":
                    search = new DivisionSearch(A, B, eps, F);
                    break;
                case "Fibonacci search":
                    search = new FibonacciSearch(A, B, eps, F);
                    break;
                case "Golden ratio search":
                    search = new GoldenRatioSearch(A, B, eps, F);
                    break;
                default:
                    return;
            }

            (double x, double y, int N) res = search.Search();

            resultNumeric   = res.x;
            resultFuncValue = res.y;

            ResultTextBlock.Text = $"x: {res.x}\nf(x): {res.y}\nIterations: {search.N}\nActual iterations: {res.N}";

            canvasControl.Invalidate();
        }
    }
}
