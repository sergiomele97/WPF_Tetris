using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;


namespace WPFapp1
{
    public class Pieza                  // Una pieza esta formada por 4 bloques
    {
        public int tipo = 1;           //geometria
        public int orientacion = 0;   //orientacion
        public string color = "Yellow";

        public int altura = 0;              //posicion Y

        public bool bajando = true;    // Bajando o parada

        public int[] arrayBloques = new int[4] { 0, 0, 0, 0 };  // Posición de children.add()

        public int[] posBloquesX = new int[4] { 90, 120, 150, 180 };   // Posición en X

        public int[] posBloquesY = new int[4] { -30, -30, -30, -30 };  // Posición en Y

        public int[][][] sprite = new int[4][][];       // Jagged array  


        public Pieza(int nRandom)
        {
            this.tipo = nRandom;
            DefinirPieza(tipo);
        }

        public int[,] GetRotación()        // pendiente
        {

            //Este es el formato del array que vamos a devolver con dX y dY para cada bloque

            int[,] arrayRotacion = new int[4, 2] { { 0, 0}, { 0, 0 }, { 0, 0 }, { 0, 0 }, };

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {

                    if (sprite[orientacion][j][i] != 0)   // Le vamos a restar su posición anterior y sumar la nueva, en arrayRotacion quedara la diferencia
                    {
                        // Vieja X
                        arrayRotacion[sprite[orientacion][j][i] - 1, 0] -= i * 30;  
                        // Vieja Y  
                        arrayRotacion[sprite[orientacion][j][i] - 1, 1] -= j * 30;
                    }
                    if (sprite[NextOrientacion()][j][i] != 0)   // Se escribe al reves de como seria intuitivo
                    {
                        // Nueva X
                        arrayRotacion[sprite[NextOrientacion()][j][i] - 1, 0] += i * 30;
                        // Nueva Y
                        arrayRotacion[sprite[NextOrientacion()][j][i] - 1, 1] += j * 30;
                    } 
                }
            }

            return arrayRotacion;
        }

        // Nos permite consultar la siguiente orientación
        public int NextOrientacion()
        {
            if (orientacion < 3)
            {
                return orientacion + 1;
            }
            else { return 0; }
        }

        // Define las propiedades de la siguiente pieza
        public void DefinirPieza(int tipo)
        {
            switch (tipo)      
            {
                case 1:     // Hero (Pieza larga)
                    posBloquesX = [90, 120, 150, 180];
                    posBloquesY = [-30, -30, -30, -30];
                    color = "Aqua";
                    sprite = [[       [0, 0, 0, 0],
                                      [0, 0, 0, 0],
                                      [0, 0, 0, 0],
                                      [1, 2, 3, 4]],

                                     [[0, 0, 1, 0],
                                      [0, 0, 2, 0],
                                      [0, 0, 3, 0],
                                      [0, 0, 4, 0]],

                                     [[0, 0, 0, 0],
                                      [0, 0, 0, 0],
                                      [0, 0, 0, 0],
                                      [1, 2, 3, 4]],

                                     [[0, 0, 1, 0],
                                      [0, 0, 2, 0],
                                      [0, 0, 3, 0],
                                      [0, 0, 4, 0]]
                                      ];
                    break;

                case 2:     // Orange Ricky (L normal)
                    posBloquesX = [150, 150, 150, 180];
                    posBloquesY = [-60, -30, 0, 0];
                    color = "Orange";
                    sprite = [[       [0, 0, 0, 0],
                                      [0, 0, 1, 0],
                                      [0, 0, 2, 0],
                                      [0, 0, 3, 4]],

                                     [[0, 0, 0, 0],
                                      [0, 0, 0, 0],
                                      [0, 3, 2, 1],
                                      [0, 4, 0, 0]],

                                     [[0, 0, 0, 0],
                                      [0, 4, 3, 0],
                                      [0, 0, 2, 0],
                                      [0, 0, 1, 0]],

                                     [[0, 0, 0, 0],
                                      [0, 0, 0, 0],
                                      [0, 0, 0, 4],
                                      [0, 1, 2, 3]]
                                      ];
                    break;

                case 3:     // Blue Ricky (L inversa)
                    posBloquesX = [150, 150, 150, 120];
                    posBloquesY = [-60, -30, 0, 0];
                    color = "Blue";
                    sprite = [[       [0, 0, 0, 0],
                                      [0, 0, 1, 0],
                                      [0, 0, 2, 0],
                                      [0, 4, 3, 0]],

                                     [[0, 0, 0, 0],
                                      [0, 0, 0, 0],
                                      [0, 4, 0, 0],
                                      [0, 3, 2, 1]],

                                     [[0, 0, 0, 0],
                                      [0, 0, 3, 4],
                                      [0, 0, 2, 0],
                                      [0, 0, 1, 0]],

                                     [[0, 0, 0, 0],
                                      [0, 0, 0, 0],
                                      [0, 1, 2, 3],
                                      [0, 0, 0, 4]]
                                      ];
                    break;
                case 4:     // Cleveland Z  (-_)
                    posBloquesX = [120, 150, 150, 180];
                    posBloquesY = [-30, -30, 0, 0];
                    color = "Red";
                    sprite = [[       [0, 0, 0, 0],
                                      [0, 0, 0, 0],
                                      [0, 1, 2, 0],
                                      [0, 0, 3, 4]],

                                     [[0, 0, 0, 0],
                                      [0, 0, 1, 0],
                                      [0, 3, 2, 0],
                                      [0, 4, 0, 0]],

                                     [[0, 0, 0, 0],
                                      [0, 0, 0, 0],
                                      [0, 1, 2, 0],
                                      [0, 0, 3, 4]],

                                     [[0, 0, 0, 0],
                                      [0, 0, 1, 0],
                                      [0, 3, 2, 0],
                                      [0, 4, 0, 0]]
                                      ];
                    break;
                case 5:     // Rhode Island Z (_-)
                    posBloquesX = [180, 150, 150, 120];
                    posBloquesY = [-30, -30, 0, 0];
                    color = "GreenYellow";
                    sprite = [[       [0, 0, 0, 0],
                                      [0, 0, 0, 0],
                                      [0, 0, 2, 1],
                                      [0, 4, 3, 0]],

                                     [[0, 0, 0, 0],
                                      [0, 0, 1, 0],
                                      [0, 0, 2, 3],
                                      [0, 0, 0, 4]],

                                      [[0, 0, 0, 0],
                                      [0, 0, 0, 0],
                                      [0, 0, 2, 1],
                                      [0, 4, 3, 0]],

                                     [[0, 0, 0, 0],
                                      [0, 0, 1, 0],
                                      [0, 0, 2, 3],
                                      [0, 0, 0, 4]]
                                      ];
                    break;
                case 6:     // SmashBoy (cuadrado)
                    posBloquesX = [150, 180, 150, 180];
                    posBloquesY = [-30, -30, 0, 0];
                    color = "Yellow";
                    sprite = [[       [0, 0, 0, 0],
                                      [0, 0, 0, 0],
                                      [0, 0, 1, 2],
                                      [0, 0, 3, 4]],

                                     [[0, 0, 0, 0],
                                      [0, 0, 0, 0],
                                      [0, 0, 1, 2],
                                      [0, 0, 3, 4]],

                                     [[0, 0, 0, 0],
                                      [0, 0, 0, 0],
                                      [0, 0, 1, 2],
                                      [0, 0, 3, 4]],

                                     [[0, 0, 0, 0],
                                      [0, 0, 0, 0],
                                      [0, 0, 1, 2],
                                      [0, 0, 3, 4]],];
                    break;
                case 7:     // Teewee (_|_)
                    posBloquesX = [150, 120, 150, 180];
                    posBloquesY = [-30, 0, 0, 0];
                    color = "Violet";
                    sprite = [[       [0, 0, 0, 0],
                                      [0, 0, 0, 0],
                                      [0, 0, 1, 0],
                                      [0, 2, 3, 4]],

                                     [[0, 0, 0, 0],
                                      [0, 2, 0, 0],
                                      [0, 3, 1, 0],
                                      [0, 4, 0, 0]],

                                     [[0, 0, 0, 0],
                                      [0, 0, 0, 0],
                                      [0, 4, 3, 2],
                                      [0, 0, 1, 0]],

                                     [[0, 0, 0, 0],
                                      [0, 0, 4, 0],
                                      [0, 1, 3, 0],
                                      [0, 0, 2, 0]]];
                    break;

            }
        }
    }
}
