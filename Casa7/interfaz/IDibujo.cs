using OpenTK;
using System;
using System.Collections.Generic;
using System.Text;

namespace Casa7.interfaz
{
    public interface IDibujo
    {
        void dibujar(Matrix4 m);
        void rotar(float x, float y, float z);
        void ampliar(float x, float y, float z);
        void trasladar(float x, float y, float z);
        void dispose();
    }
}
