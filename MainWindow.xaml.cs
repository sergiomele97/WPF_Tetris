using Microsoft.Win32;
using System;
using System.Numerics;
using System.Reflection;
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

        bool IsInputAllowed = false;  // Determina si se permite el input de teclas

        Random rnd = new Random();  // Para generar piezas aleatorias

        private MediaPlayer mediaPlayer = new MediaPlayer();    // Audio


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
                IsInputAllowed = true;           // Colocarlo aquí asegura que X e Y no tengan valores negativos mientras haya input
            }
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//
        // COLISIONES: Es posible que se puedan refactorizar en una sola funcion de colision
        private bool IsBottomCollision()  // Si para uno solo de los bloques, hay pieza en [x,y+1]: devuelve true
        {
            for (int i = 0; i < tamañoPiezas; i++)
            {
                if (ListaPiezas[piezaActiva].posBloquesY[i] <-30)    // Evita que se evalue la colisión en posiciones fuera del rango de tablero
                {
                    continue;                                        // En otras palabras, permite mover la pieza en la parte superior del canvas
                }

                if (tablero[(ListaPiezas[piezaActiva].posBloquesX[i] + 30) / pixelesCuadrado, (ListaPiezas[piezaActiva].posBloquesY[i] + 30) / pixelesCuadrado])    // +30 compensa la primera columna pared del tablero
                {
                    // Si hay colisión: Actualizamos pieza, tablero y devolvemos true

                    IsInputAllowed = false;
                    BottomCollision();
                    

                    return true;                                // Devolver isColision() = True
                }
            }
            return false;
        }

        private bool IsSpinColission(int[,] arrayRotacion)
        {
            for (int i = 0; i < tamañoPiezas; i++)
            {
                if (ListaPiezas[piezaActiva].posBloquesY[i] + arrayRotacion[i, 1] <= 0)    // Evita que se evalue la colisión en posiciones fuera del rango de tablero
                {
                    continue;                                        
                }

                if (ListaPiezas[piezaActiva].posBloquesX[i] + arrayRotacion[i, 0] < 0 || ListaPiezas[piezaActiva].posBloquesX[i] + arrayRotacion[i, 0] > nCasillasX*30)    // Colisión lateral extremo tablero
                {
                    return true;
                }

                // Esta expresion es la ubicación de destino en tablero
                if (tablero[(ListaPiezas[piezaActiva].posBloquesX[i] + arrayRotacion[i, 0] + 30) / pixelesCuadrado, (ListaPiezas[piezaActiva].posBloquesY[i] + arrayRotacion[i, 1]) / pixelesCuadrado])         // +30 compensa la primera columna
                {
                    return true;                                        
                }
            }
            return false;
        }

        private bool IsRightCollision()
        {
            for (int i = 0; i < tamañoPiezas; i++)
            {
                if (ListaPiezas[piezaActiva].posBloquesY[i] < 0)    // Evita que se evalue la colisión en posiciones fuera del rango de tablero
                {
                    continue;                                        // En otras palabras, permite mover la pieza en la parte superior del canvas
                }

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
                if (ListaPiezas[piezaActiva].posBloquesY[i] < 0)    // Evita que se evalue la colisión en posiciones fuera del rango de tablero
                {
                    continue;                                        // En otras palabras, permite mover la pieza en la parte superior del canvas
                }

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
            int[] arrayValoresY = new int[5] { 0, 0, 0, 0, 0};           // Creamos un array para guardar las posiciones en y involucradas en la colision

            for (int c = 0; c < tamañoPiezas; c++)      // Actualizar Tablero
            {
                tablero[(ListaPiezas[piezaActiva].posBloquesX[c]) / pixelesCuadrado + 1, (ListaPiezas[piezaActiva].posBloquesY[c] + 1) / pixelesCuadrado] = true;   // +1 compensa la primera columna pared del tablero

                arrayValoresY[c] = (ListaPiezas[piezaActiva].posBloquesY[c] + 1) / pixelesCuadrado; 
            }

            if (IsGameOver())
            {
                GameOver();
            }

            for (int i = 0; i < arrayValoresY.Length - 1; i++)     // Le pasamos las lineas (y) involucradas en esta colision
            {
                if (arrayValoresY[i] != arrayValoresY[i + 1])   // Llamamos a IsLineCompleted para cada linea involucrada
                {
                    if (IsLineCompleted(arrayValoresY[i]))
                    {
                        LineCompleted(arrayValoresY[i] * pixelesCuadrado);
                    }
                }
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

            // Audio
            mediaPlayer.Open(new Uri(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\..\..\..\Audio\OriginalTetrisTheme.mp3"));
            mediaPlayer.Play();
            mediaPlayer.MediaEnded += new EventHandler(Media_Ended);    // Canción en bucle
        }


        //---------------------------------------------------------------------------------------------------------------------------------------------//
        /*
        *  MUSICA
        */
        private void Media_Ended(object sender, EventArgs e)
        {
            mediaPlayer.Position = TimeSpan.Zero;
            mediaPlayer.Play();
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
        *  ROTACION PIEZA
        */

        private void RotarPieza()
        {
            int[,] arrayRotacion = ListaPiezas[piezaActiva].GetRotación();

            if (!IsSpinColission(arrayRotacion))
            {

                for (int i = 0; i < tamañoPiezas; i++)
                {
                    Canvas.SetLeft(GameArea.Children[ListaPiezas[piezaActiva].arrayBloques[i]], ListaPiezas[piezaActiva].posBloquesX[i] += arrayRotacion[i, 0]);
                    Canvas.SetTop(GameArea.Children[ListaPiezas[piezaActiva].arrayBloques[i]], ListaPiezas[piezaActiva].posBloquesY[i] += arrayRotacion[i, 1]);
                }

                // Actualizar orientacion
                if (ListaPiezas[piezaActiva].orientacion < 3)
                {
                    ListaPiezas[piezaActiva].orientacion++;
                }
                else { ListaPiezas[piezaActiva].orientacion = 0; }
            }
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//
        /*
        *  LINEA COMPLETADA
        */

        private bool IsLineCompleted(int linea)
        {
            
            for(int i = 1; i <= nCasillasX; i++)
            {
                if (tablero[i,linea] == false)
                {
                    return false;
                }
            } 

            return true;
        }

        private void LineCompleted(int y)
        {

            // Borra tablero por encima y en la linea

            for (int i = 1; i <= nCasillasX; i++)   
            {
                for (int j = 0; j <= y/pixelesCuadrado; j++) 
                {
                    tablero[i, j] = false;
                }
            }

            // Actualizar Canvas + tablero

            int c = 0;
            foreach(UIElement child in GameArea.Children)   // Unica forma de iterarsobre los children
            {
                if (c < 160 || child.IsVisible == false)    // Evita mover los bloques del background y los invisibles
                {
                    c++;
                    continue;
                }

                int childY = Convert.ToInt32(Canvas.GetTop(child));     // Y en pixeles 
                int childX = Convert.ToInt32(Canvas.GetLeft(child));    // X en pixeles


                if (y > childY)     // Caso bajando
                {
                    Canvas.SetTop(child, (childY + pixelesCuadrado));      // Actualiza canvas
                    tablero[childX / pixelesCuadrado + 1,(childY + pixelesCuadrado) / pixelesCuadrado] = true;   // Actualiza tablero (+1 columna vacia)
                }
                else if (y == childY)   // Caso en linea eliminada
                {
                    child.Visibility = Visibility.Collapsed;    // Hace que el elemento no sea visible
                }
            }
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
                    if (IsInputAllowed && !IsLeftCollision())    //  IMPORTANTE && y orden operandos: Si llamamos a !IsLeftC... cuando no se admite input, el juego puede crashear
                    { 
                        for (int i = 0; i < tamañoPiezas; i++)
                        {
                            Canvas.SetLeft(GameArea.Children[ListaPiezas[piezaActiva].arrayBloques[i]], ListaPiezas[piezaActiva].posBloquesX[i] -= 30);
                        }
                    }
                    break;
                
                case Key.Right:
                    if (IsInputAllowed && !IsRightCollision())   //  IMPORTANTE && y orden operandos: Si llamamos a !IsLeftC... cuando no se admite input, el juego puede crashear
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

                case Key.Space: 
                    if (IsInputAllowed)                
                    {
                        RotarPieza();

                    }
                    break;
            }

        }

    }

}