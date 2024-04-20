﻿using System.Text;
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

        bool[,] tablero = new bool[12, 17]; // Array booleano con posiciones tablero
        

        //---------------------------------------------------------------------------------------------------------------------------------------------//
        /*
         *  CONSTRUCTOR VENTANA
         */

        // VENTANA
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
            }

            for (int i = 0; i < tamañoPiezas; i++)
            {
                Canvas.SetTop(GameArea.Children[ListaPiezas[piezaActiva].ArrayBloques[i]], ListaPiezas[piezaActiva].posBloquesY[i]+=30);
            }
        }

        // COLISIONES
        private bool IsBottomCollision()  // Si para uno solo de los bloques, hay pieza en [x,y+1]: devuelve true
        {
            for (int i = 0; i < tamañoPiezas; i++)
            {
                if (tablero[(ListaPiezas[piezaActiva].posBloquesX[i] + 30) / pixelesCuadrado, (ListaPiezas[piezaActiva].posBloquesY[i] + 31) / pixelesCuadrado])    // +30 compensa la primera columna pared del tablero
                {
                    // Si hay colisión: Actualizamos pieza, tablero y devolvemos true

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
            Rectangle rect = new Rectangle
            {
                Width = pixelesCuadrado,
                Height = pixelesCuadrado,
                Fill = Brushes.Yellow,
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
            Pieza pieza = new Pieza();
            ListaPiezas.Add(pieza);

            for (int i = 0; i < tamañoPiezas; i++)
            {
                ListaPiezas[piezaActiva].ArrayBloques[i] = nGameAreaChildren;

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
                    if (!IsLeftCollision()) 
                    { 
                        for (int i = 0; i < tamañoPiezas; i++)
                        {
                            Canvas.SetLeft(GameArea.Children[ListaPiezas[piezaActiva].ArrayBloques[i]], ListaPiezas[piezaActiva].posBloquesX[i] -= 30);
                        }
                    }
                    break;
                
                case Key.Right:
                    if (!IsRightCollision())
                    {
                        for (int i = 0; i < tamañoPiezas; i++)
                        {
                            Canvas.SetLeft(GameArea.Children[ListaPiezas[piezaActiva].ArrayBloques[i]], ListaPiezas[piezaActiva].posBloquesX[i] += 30);
                        }
                    }
                    break;

                case Key.Down:
                    Console.WriteLine("Down");
                    break;
                case Key.Space:
                    Console.WriteLine("UP");
                    break;
            }
            
        }
    }
}