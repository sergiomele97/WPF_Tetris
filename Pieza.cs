﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WPFapp1
{
    public class Pieza                  // Una pieza esta formada por 4 bloques
    {
        public int tipo = 0;           //geometria
        public int orientación = 0;   //orientacion

        public int x = 0;              //posicion X
        public int y = 0;              //posicion Y

        public bool bajando = true;    // Bajando o parada

        public int[] ArrayBloques = new int[4] { 0, 0, 0, 0 };  // Posición de children.add()

        public int[] posBloquesX = new int[4] { 150, 180, 210, 240 };   // Posición en X

        public int[] posBloquesY = new int[4] { -30, -30, -30, -30 };  // Posición en Y         



    }
}
