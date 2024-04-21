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
         *  VARIABLES GLOBALES 
         */

        int posY = 0;
        List<Pieza> ListaPiezas = new List<Pieza>();
        int piezaActiva = -1;                // Señala el indice de la lista con la pieza en movimiento
        int nGameAreaChildren = 0;

        private System.Windows.Threading.DispatcherTimer gameTickTimer = new System.Windows.Threading.DispatcherTimer(); // Tema del tiempo
        double gameSpeed = 0.1;  // ¡! Inversamente proporcional

        int tamañoPiezas = 4;
        int pixelesCuadrado = 30;
        int nCasillasX = 10;
        int nCasillasY = 16;

        bool[,] tablero = new bool[12, 21]; // Array booleano con posiciones tablero

        bool input = false;  // Determina si se permite el input de teclas

        Random rnd = new Random();  // Para generar piezas aleatorias


        //---------------------------------------------------------------------------------------------------------------------------------------------//
        /*
         *  CONSTRUCTOR VENTANA
         */


        // VENTANA

        /*  En ocasiones aparece un bug de visual studio: "InitializeComponent() no existe en el contexto actual"
         *  Permite compilar sin problemas, pero señala error.
         *  
         *  Solución:
         *      Navigate to the solution directory
         *      Delete the \obj folder
         *      Rebuild the solution
         */

        public MainWindow()
        {
            InitializeComponent();  // This call combines the .cs and xaml partial clases
            gameTickTimer.Tick += GameTickTimer_Tick;   // Le pasamos la función GameTickTimer_Tick
        }


        // CONTENIDO INICIAL VENTANA
        private void Window_ContentRendered(object sender, EventArgs e)
        {
            InitializeBackground();
            StartNewGame();
            InitializeNextPieza();
        }


        //---------------------------------------------------------------------------------------------------------------------------------------------//
        /*
        *  EJECUTAR EVERY TICK (BUCLE PRINCIPAL DEL JUEGO)
        */

        private void GameTickTimer_Tick(object sender, EventArgs e)
        {
            NextTick();
        }

        private void NextTick()
        {
            IsBottomCollision();
            ActualizarPiezas();        
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//
        // ACTUALIZAR PIEZAS
        private void ActualizarPiezas()
        {
            if (ListaPiezas[piezaActiva].bajando == false)      // Cuando para de bajar
            {
                InitializeNextPieza();
                return;                  // Impide que se ejecute lo siguiente, perimitiendo que se den las condiciones para terminar la partida
            }

            for (int i = 0; i < tamañoPiezas; i++)
            {
                Canvas.SetTop(GameArea.Children[ListaPiezas[piezaActiva].arrayBloques[i]], ListaPiezas[piezaActiva].posBloquesY[i]+=30);
                input = true;           // Colocarlo aquí asegura que X e Y no tengan valores negativos mientras haya input
            }
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//
        // COLISIONES
        private bool IsBottomCollision()  // Si para uno solo de los bloques, hay pieza en [x,y+1]: devuelve true
        {
            for (int i = 0; i < tamañoPiezas; i++)
            {
                if (ListaPiezas[piezaActiva].posBloquesY[i] <-30)    // Evita que se evalue la colisión en posiciones fuera del rango de tablero
                {
                    continue;                                        // En otras palabras, permite girar piezas en el limite superior sin que crashee
                }

                if (tablero[(ListaPiezas[piezaActiva].posBloquesX[i] + 30) / pixelesCuadrado, (ListaPiezas[piezaActiva].posBloquesY[i] + 30) / pixelesCuadrado])    // +30 compensa la primera columna pared del tablero
                {
                    // Si hay colisión: Actualizamos pieza, tablero y devolvemos true

                    input = false;
                    BottomCollision();

                    return true;                                // Devolver isColision() = True
                }
            }
            return false;
        }

        private bool IsRightCollision()
        {
            for (int i = 0; i < tamañoPiezas; i++)
            {
                if (tablero[(ListaPiezas[piezaActiva].posBloquesX[i] + 60) / pixelesCuadrado, (ListaPiezas[piezaActiva].posBloquesY[i]) / pixelesCuadrado])         // +61 compensa la primera columna + next cuadrado
                {
                    return true;                                // Colision lateral          
                }
            }
            return false;
        }

        private bool IsLeftCollision()
        {
            for (int i = 0; i < tamañoPiezas; i++)
            {
                if (tablero[(ListaPiezas[piezaActiva].posBloquesX[i]) / pixelesCuadrado, (ListaPiezas[piezaActiva].posBloquesY[i]) / pixelesCuadrado])         
                {
                    return true;                                // Colision lateral          
                }
            }
            return false;
        }

        private void BottomCollision()
        {
            ListaPiezas[piezaActiva].bajando = false;   // Actualizar Pieza

            for (int c = 0; c < tamañoPiezas; c++)      // Actualizar Tablero
            {
                tablero[(ListaPiezas[piezaActiva].posBloquesX[c]) / pixelesCuadrado + 1, (ListaPiezas[piezaActiva].posBloquesY[c] + 1) / pixelesCuadrado] = true;   // +1 compensa la primera columna pared del tablero
            }

            if (IsGameOver())
            {
                GameOver();
            }

        }

        // FIN DE PARTIDA
        private bool IsGameOver()
        {
            for (int i = 0; i < tamañoPiezas; i++)
            {
                if (ListaPiezas[piezaActiva].posBloquesY[i] < 0)
                {
                    return true;
                }
            }
            return false;
        }

        private void GameOver()
        {
            gameTickTimer.IsEnabled = false;
            MessageBox.Show("GAME OVER :(");
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//
        /*
        *  NUEVA PARTIDA
        */

        private void StartNewGame()
        {
            gameTickTimer.Interval = TimeSpan.FromSeconds(gameSpeed);

            // Go!          
            gameTickTimer.IsEnabled = true;
        }


        //---------------------------------------------------------------------------------------------------------------------------------------------//
        /*
        *  DIBUJADO
        */

        // Dibuja un cuadrado
        public void DrawBloque(int x,int y)
        {
            Color color = (Color)ColorConverter.ConvertFromString(ListaPiezas[piezaActiva].color);  // Igual hay formas mas eficientes

            Rectangle rect = new Rectangle
            {
                Width = pixelesCuadrado,
                Height = pixelesCuadrado,
                Fill = new SolidColorBrush(color),
                Stroke = Brushes.Black,
                StrokeThickness = 2

            };

            GameArea.Children.Add(rect);
            nGameAreaChildren++;
            Canvas.SetLeft(rect, x);
            Canvas.SetTop(rect, y);
        }


        //  Inicializa la siguiente pieza
        private void InitializeNextPieza()
        {
            piezaActiva++;
            Pieza pieza = new Pieza(rnd.Next(1,8));     // Le pasamos un numero random del 1 al 7
            ListaPiezas.Add(pieza);

            for (int i = 0; i < tamañoPiezas; i++)
            {
                ListaPiezas[piezaActiva].arrayBloques[i] = nGameAreaChildren;

                DrawBloque(ListaPiezas[piezaActiva].posBloquesX[i], ListaPiezas[piezaActiva].posBloquesY[i]);
            }   
        }


        // Dibuja el fondo
        private void DrawBackground()
        {
            for (int i = 0; i < nCasillasX; i++)
            {
                for (int j = 0; j < nCasillasY; j++)
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
                    nGameAreaChildren++;
                    Canvas.SetLeft(rect, i*30);
                    Canvas.SetTop(rect, j*30);
                }               
            }
        }

        // Inicializar tablero
        private void InitializeBackground()
        {
            for(int i = 0; i < nCasillasX; i++) // Crea un fondo invisibles en el tablero
            {
                tablero[i + 1, 16] = true;  // +1 para compensar la primera columna vacia
            }

            for (int i = 0; i < nCasillasY; i++) // Crea un fondo invisibles a los lados
            {
                tablero[0, i] = true;
                tablero[11, i] = true;
            }

            DrawBackground();
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//
        /*
        *  CONTROLES
        */

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                    if (input && !IsLeftCollision())    //  IMPORTANTE && y orden operandos: Si llamamos a !IsLeftC... cuando no se admite input, el juego puede crashear
                    { 
                        for (int i = 0; i < tamañoPiezas; i++)
                        {
                            Canvas.SetLeft(GameArea.Children[ListaPiezas[piezaActiva].arrayBloques[i]], ListaPiezas[piezaActiva].posBloquesX[i] -= 30);
                        }
                    }
                    break;
                
                case Key.Right:
                    if (input && !IsRightCollision())   //  IMPORTANTE && y orden operandos: Si llamamos a !IsLeftC... cuando no se admite input, el juego puede crashear
                    {
                        for (int i = 0; i < tamañoPiezas; i++)
                        {
                            Canvas.SetLeft(GameArea.Children[ListaPiezas[piezaActiva].arrayBloques[i]], ListaPiezas[piezaActiva].posBloquesX[i] += 30);
                        }
                    }
                    break;

                case Key.Down:
                    Console.WriteLine("Down");
                    break; 
                case Key.Space: // Rotar
                    if (input)              // HABRA QUE PONER MAS CONDICIONES  
                    {
                        int[,] arrayRotacion = ListaPiezas[piezaActiva].Rotar(ListaPiezas[piezaActiva].orientación);


                        for (int i = 0; i < tamañoPiezas; i++)
                        {
                            Canvas.SetLeft(GameArea.Children[ListaPiezas[piezaActiva].arrayBloques[i]], ListaPiezas[piezaActiva].posBloquesX[i] += arrayRotacion[i,0]);
                            Canvas.SetTop(GameArea.Children[ListaPiezas[piezaActiva].arrayBloques[i]], ListaPiezas[piezaActiva].posBloquesY[i] += arrayRotacion[i, 1]);
                        }

                    }
                    break;
            }
            
        }
    }
}