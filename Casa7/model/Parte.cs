using Casa7.interfaz;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Casa7.model
{
    class Parte : IDibujo
    {
        public Punto cm { get; set; }
        public Color color { get; set; }
        public string name { get; set; }

        public List<Punto> l;

        private Matrix4 m;

        public Parte()
        {
            l = new List<Punto>();
            color = Color.White;
            m = Matrix4.Identity;
        }

        public Parte(String name) : this()
        {
            this.name = name;
        }

        public Parte(Parte p)
        {
            this.name = p.name;
            this.color = p.color;
            this.cm = p.cm.copiar();
            this.l = new List<Punto>();

            Punto punto;
            for (int i = 0; i < p.nroPuntos(); i++)
            {
                punto = p.getPunto(i);
                addPunto(punto.copiar());
            }
        }

        public void addPunto(Punto punto)
        {
            l.Add(punto);
        }

        public Punto getPunto(int i)
        {
            return (Punto)l[i];
        }

        public void borrarPunto(int pos)
        {
            Punto p = getPunto(pos);
            l.Remove(p);
        }

        public void borrarPunto(Punto punto)
        {
            l.Remove(punto);
        }

        public int nroPuntos()
        {
            return l.Count;
        }

        public Parte copiar()
        {
            Parte p1 = new Parte(this);
            return p1;
        }

        public void dibujar(Matrix4 m)
        {
            Matrix4 u = m * this.m;

            GL.LoadMatrix(ref u);

            GL.Begin(PrimitiveType.Polygon);
            GL.Color3(color);
            foreach (Punto punto in l)
            {
                GL.Vertex3(punto.ToVector3());
            }
            GL.End();
        }

        public void dispose()
        {
            l.Clear();
            l = null;
        }

        public void rotar(float x, float y, float z)
        {
            m = Matrix4.CreateTranslation(-cm.x, -cm.y, -cm.z) * m;
            m = Matrix4.CreateRotationX(x) * Matrix4.CreateRotationY(y) * Matrix4.CreateRotationZ(z) * m;
            m = Matrix4.CreateTranslation(cm.x, cm.y, cm.z) * m;
        }

        public void ampliar(float x, float y, float z)
        {
            m = Matrix4.CreateTranslation(-cm.x, -cm.y, -cm.z) * m;
            m = Matrix4.CreateScale(x, y, z) * m;
            m = Matrix4.CreateTranslation(cm.x, cm.y, cm.z) * m;
        }

        public void trasladar(float x, float y, float z)
        {
            m = Matrix4.CreateTranslation(x, y, z) * m;
            /*
            Vector4 vec = new Vector4(cm.x, cm.y, cm.z, 1);
            vec = vec * m;
            cm.x = vec.X;
            cm.y = vec.Y;
            cm.z = vec.Z;
            */
        }
    }
}
