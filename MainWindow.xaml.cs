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
        //---------------------------------------------------------------------------------------------------------------------------------------------//
        /* 
         *  VARIABLES INICIALES 
         */
        int posY = 0;
        List<Pieza> ListaPiezas = new List<Pieza>();
        int piezaActiva = -1;                // Señala el indice de la lista con la pieza en movimiento
        int cont_rect = 0;  
        private System.Windows.Threading.DispatcherTimer gameTickTimer = new System.Windows.Threading.DispatcherTimer(); // Tema del tiempo

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /*
         *  CONSTRUCTOR VENTANA
         */
        public MainWindow()
        {
            InitializeComponent();  // This call combines the .cs and xaml partial clases
            gameTickTimer.Tick += GameTickTimer_Tick;

        }

        /*
         *  CONTENIDO INICIAL
         */
        private void Window_ContentRendered(object sender, EventArgs e)
        {
            DrawBackground();
            StartNewGame();
            DrawNext();

        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /*
        *  Incluir aquí el contenido de cada tick
        */
        private void GameTickTimer_Tick(object sender, EventArgs e)
        {
            NextTick();
            
        }

        private void NextTick()
        {
            ActualizarPiezas();        

        }

       

        private void StartNewGame()
        {
            gameTickTimer.Interval = TimeSpan.FromMilliseconds(40);

            // Go!          
            gameTickTimer.IsEnabled = true;
            

        }

        // Area para dibujado

        public void DrawBloque(int x,int y)
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
            cont_rect++;
            Canvas.SetLeft(rect, x);
            Canvas.SetTop(rect, y);
        }

        // Espacio que va a ejecutarse cada tick

        private void ActualizarPiezas()
        {
            if (ListaPiezas[piezaActiva].bajando == false)      // Cuando para de bajar
            {
                DrawNext();
            }
            
            for(int i = 0; i < ListaPiezas[piezaActiva].ArrayBloques.Length; i++)
            {
                
                Canvas.SetTop(GameArea.Children[ListaPiezas[piezaActiva].ArrayBloques[i]], ListaPiezas[piezaActiva].posBloquesY[i]++);
            }
            
            
        }

        //  Inicializa la siguiente pieza

        private void DrawNext()
        {
            piezaActiva++;
            Pieza pieza = new Pieza();
            ListaPiezas.Add(pieza);

            for (int i = 0; i < ListaPiezas[piezaActiva].ArrayBloques.Length; i++)
            {

                ListaPiezas[piezaActiva].ArrayBloques[i] = cont_rect;

                DrawBloque(ListaPiezas[piezaActiva].posBloquesX[i], ListaPiezas[piezaActiva].posBloquesY[i]);

            }

            
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
                    cont_rect++;
                    Canvas.SetLeft(rect, i);
                    Canvas.SetTop(rect, j);
                }               
            }
        }

        // Area para gestion de controles

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