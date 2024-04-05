using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace WPFapp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int posY = 0;

        private System.Windows.Threading.DispatcherTimer gameTickTimer = new System.Windows.Threading.DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            gameTickTimer.Tick += GameTickTimer_Tick;
            
        }

        private void GameTickTimer_Tick(object sender, EventArgs e)
        {
            NextTick();
            
        }

        private void NextTick()
        {
            GameArea.Children.RemoveAt(160);
            DrawPieza(0, posY++);
            

        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            DrawBackground();
            StartNewGame();
            DrawPieza(0, 0);

        }

        private void StartNewGame()
        {
            gameTickTimer.Interval = TimeSpan.FromMilliseconds(40);

            // Go!          
            gameTickTimer.IsEnabled = true;

        }

        private void DrawPieza(int x,int y)
        {
            Rectangle rect = new Rectangle
            {
                Width = 30,
                Height = 30,
                Fill = Brushes.Yellow,
                Stroke = Brushes.Black,
                StrokeThickness = 2

            };

            GameArea.Children.Add(rect);
            Canvas.SetLeft(rect, x);
            Canvas.SetTop(rect, y);
        }

        private void DrawBackground()
        {
            for (int i = 0; i < GameArea.Width; i += 30)
            {
                for (int j = 0; j < GameArea.Height; j += 30)
                {
                    Rectangle rect = new Rectangle
                    {
                        Width = 30,
                        Height = 30,
                        Fill = Brushes.DarkBlue,
                        Stroke = Brushes.Black,
                        StrokeThickness = 2

                    };

                    GameArea.Children.Add(rect);
                    Canvas.SetLeft(rect, i);
                    Canvas.SetTop(rect, j);
                }               
            }      
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    Console.WriteLine("UP");
                    break;
                case Key.Down:
                    Console.WriteLine("UP");
                    break;
                case Key.Left:
                    Console.WriteLine("UP");
                    break;
                case Key.Right:
                    Console.WriteLine("UP");
                    break;
                case Key.Space:
                    Console.WriteLine("UP");
                    break;
            }
            
        }
    }
}