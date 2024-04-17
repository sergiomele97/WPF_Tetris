using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WPFapp1
{
    public class Pieza
    {
        public int tipo = 0;           //geometria
        public int orientación = 0;   //orientacion

        public int x = 0;              //posicion X
        public int y = 0;              //posicion Y

        public bool bajando = true;    // Bajando o parada
        public int[] ArrayBloques = new int[4] { 0, 0, 0, 0 };

        public int[] posBloquesX = new int[4] { 150, 180, 210, 240 };

        public int[] posBloquesY = new int[4] { 0, 0, 0, 0 };



    }
}
