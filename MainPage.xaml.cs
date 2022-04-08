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

        private int scale = 200, offset = 150;

        ObservableCollection<string> searchMethods { get; } = new ObservableCollection<string>();
        ObservableCollection<double> precisions { get; } = new ObservableCollection<double>();
        
        string selectedMethod = null;
        double selectedPrecision = 0;

        private double f(double x)
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

            var fontStyle = new CanvasTextFormat();
            fontStyle.FontSize = 12;
            
            canvas.DrawLine(
                x0: ((float)A * scale) + offset,
                y0: offset,
                x1: ((float)B * scale) + offset,
                y1: offset,
                color: Colors.Black);

            canvas.DrawLine(
                x0: offset, 
                y0: offset, 
                x1: offset, 
                y1: offset + (offset * 3),
                color: Colors.Black);

            for (double i = -Math.PI / 8; i < Math.PI / 2; i += (Math.PI / 16))
            {
                double x = i * scale + offset;

                canvas.DrawCircle((float)x, offset, 3, Colors.Blue);
                canvas.DrawText(String.Format("{0:F2}", i), (float)x, offset - 20, Colors.Black, fontStyle);
            }


            for (double x = A; x <= B; x += 0.01)
            {
                double y = f(x);

                double newX = x * scale + offset;
                double newY = y * -scale + offset;

                canvas.DrawRectangle((float)newX, (float)newY, 1, 1, Colors.Black);

                if (resultNumeric != 0)
                {
                    CanvasStrokeStyle strokeStyle = new CanvasStrokeStyle
                    {
                        DashStyle = CanvasDashStyle.DashDot
                    };

                    canvas.DrawLine(
                            (float)(resultNumeric * scale + offset),
                            (float)(resultFuncValue * -scale + offset),
                            (float)(resultNumeric * scale + offset),
                            offset, 
                            Colors.Green, 0.5F, strokeStyle);

                    canvas.DrawLine(
                            (float)(resultNumeric * scale + offset),
                            (float)(resultFuncValue * -scale + offset),
                            (float)A * scale + offset,
                            (float)(resultFuncValue * -scale + offset), 
                            Colors.Green, 0.5F, strokeStyle);

                    canvas.DrawCircle(
                            (float)(resultNumeric * scale + offset), 
                            (float)(resultFuncValue * -scale + offset), 
                            4, 
                            Colors.Red);
                }
            }
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            eps = (double)PrecisionComboBox.SelectedValue;

            switch (selectedMethod)
            {
                case "Uniform search":
                    search = new UniformSearch(A, B, eps, 10, f);
                    break;
                case "Division search":
                    search = new DivisionSearch(A, B, eps, f);
                    break;
                case "Fibonacci search":
                    search = new FibonacciSearch(A, B, eps, f);
                    break;
                case "Golden ratio search":
                    search = new GoldenRatioSearch(A, B, eps, f);
                    break;
                default:
                    return;
            }

            (double x, double y, int N) res = search.Search();

            resultNumeric = res.x;
            resultFuncValue = res.y;

            ResultTextBlock.Text = $"x: {res.x}\nf(x): {res.y}\nIterations: {search.N}\nActual iterations: {res.N}";

            canvasControl.Invalidate();
        }
    }
}
