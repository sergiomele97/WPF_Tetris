using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;


namespace WPFapp1
{
    public class Pieza                  // Una pieza esta formada por 4 bloques
    {
        public int tipo = 1;           //geometria
        public int orientación = 0;   //orientacion
        public string color = "Yellow";

        public int x = 0;              //posicion X
        public int y = 0;              //posicion Y

        public bool bajando = true;    // Bajando o parada

        public int[] arrayBloques = new int[4] { 0, 0, 0, 0 };  // Posición de children.add()

        public int[] posBloquesX = new int[4] { 90, 120, 150, 180 };   // Posición en X

        public int[] posBloquesY = new int[4] { -30, -30, -30, -30 };  // Posición en Y

        public int[,] rotaciones = new int[4, 2];       


        public Pieza(int nRandom)
        {
            this.tipo = nRandom;
            DefinirPieza(tipo);
        }

        public int[,] Rotar(int orientacion)        // pendiente
        {
            int[,] arrayRotacion = new int[4, 2] { { 30, -90}, { 0, -60}, { -30, -30 }, { -60, 0 } };
            return arrayRotacion;
        }

        public void DefinirPieza(int tipo)
        {
            switch (tipo)      
            {
                case 1:     // Hero (Pieza larga)
                    posBloquesX = [90, 120, 150, 180];
                    posBloquesY = [-30, -30, -30, -30];
                    color = "Aqua";
                    break;

                case 2:     // Orange Ricky (L normal)
                    posBloquesX = [90, 120, 150, 150];
                    posBloquesY = [0, 0, 0, -30];
                    color = "Orange";
                    break;

                case 3:     // Blue Ricky (L inversa)
                    posBloquesX = [90, 120, 150, 150];
                    posBloquesY = [-30, -30, -30, 0];
                    color = "Blue";
                    break;
                case 4:     // Cleveland Z  (-_)
                    posBloquesX = [90, 120, 120, 150];
                    posBloquesY = [-30, -30, 0, 0];
                    color = "Red";
                    break;
                case 5:     // Rhode Island Z (_-)
                    posBloquesX = [90, 120, 120, 150];
                    posBloquesY = [0, 0, -30, -30];
                    color = "GreenYellow";
                    break;
                case 6:     // SmashBoy (cuadrado)
                    posBloquesX = [120, 120, 150, 150];
                    posBloquesY = [0, -30, -30, 0];
                    color = "Yellow";
                    break;
                case 7:     // Teewee (_|_)
                    posBloquesX = [90, 120, 120, 150];
                    posBloquesY = [0, 0, -30, 0];
                    color = "Violet";
                    break;

            }
        }
    }
}
