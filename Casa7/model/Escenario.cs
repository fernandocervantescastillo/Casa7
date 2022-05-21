using Casa7.interfaz;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Text;

namespace Casa7.model
{
    class Escenario : IDibujo
    {
        public string name { get; set; }
        public Punto cm { get; set; }
        public Dictionary<string, Objeto> d;
        private Matrix4 m;

        public Escenario()
        {
            d = new Dictionary<string, Objeto>();
            m = Matrix4.Identity;
            cm = new Punto(0, 0, 0);
        }

        public Escenario(string name)
        {
            this.name = name;
            cm = new Punto(0, 0, 0);
        }

        public Escenario(string name, Dictionary<string, Objeto> d)
        {
            this.d = new Dictionary<string, Objeto>();
            foreach (KeyValuePair<string, Objeto> value in d)
            {
                addObjeto(value.Key, value.Value);
            }
        }

        public Escenario(Escenario e)
        {
            this.name = e.name;
            this.cm = e.cm.copiar();
            this.d = new Dictionary<string, Objeto>();

            foreach (KeyValuePair<string, Objeto> value in e.d)
            {
                addObjeto(value.Key, value.Value);
            }
        }

        public void addObjeto(string key, Objeto value)
        {
            d.Add(key, value);
        }

        public Objeto getObjeto(string key)
        {
            return d[key];
        }

        public Objeto getObjeto(int index)
        {
            int i = 0;
            foreach (KeyValuePair<string, Objeto> value in d)
            {
                if (i == index)
                    return value.Value;
                i = i + 1;
            }
            return null;
        }

        public void deleteObjeto(string key)
        {
            d.Remove(key);
        }

        public Dictionary<string,Objeto> getDictionary()
        {
            return d;
        }

        public int cantObjetos()
        {
            return d.Count;
        }

        public void vaciarEscenario()
        {
            d.Clear();
        }

        public void dibujar(Matrix4 m)
        {
            foreach (KeyValuePair<string, Objeto> value in d)
            {
                value.Value.dibujar(this.m);
            }
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

        public void dispose()
        {
            foreach (KeyValuePair<string, Objeto> value in d)
            {
                value.Value.dispose();
            }
            d.Clear();
            d = null;
        }
    }
}
