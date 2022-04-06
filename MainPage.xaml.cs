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
        private double a = -Math.PI / 7;
        private double b = Math.PI / 2;
        private double eps = 0.001;

        private string precision = "0.001";

        private double resultNumeric   = 0;
        private double resultFuncValue = 0;

        private int scale = 200, offset = 150;

        ObservableCollection<string> searchMethods { get; } = new ObservableCollection<string>();
        string selectedMethod = null;

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
        }

        void canvasControl_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            var canvas = args.DrawingSession;

            for (double x = a; x <= b; x += 0.01)
            {
                double y = f(x);

                double newX = x * scale + offset;
                double newY = y * -scale + offset;

                canvas.DrawRectangle((float)newX, (float)newY, 1, 1, Colors.Black);

                if (resultNumeric != 0)
                {
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
            eps = double.Parse(precision, CultureInfo.InvariantCulture);

            switch (selectedMethod)
            {
                case "Uniform search":
                    search = new UniformSearch(a, b, eps, 10, f);
                    break;
                case "Division search":
                    search = new DivisionSearch(a, b, eps, f);
                    break;
                case "Fibonacci search":
                    search = new FibonacciSearch(a, b, eps, f);
                    break;
                case "Golden ratio search":
                    search = new GoldenRatioSearch(a, b, eps, f);
                    break;
                default:
                    return;
            }

            //Debug.WriteLine(search.N);
            (double x, double y, int N) res = search.Search();

            resultNumeric = res.x;
            resultFuncValue = res.y;

            ResultTextBlock.Text = $"x: {res.x}\nf(x): {res.y}\nIterations: {search.N}\nActual iterations: {res.N}";

            canvasControl.Invalidate();
        }
    }
}
