using Casa7.extras;
using Casa7.interfaz;
using Newtonsoft.Json;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Text;

namespace Casa7.model
{
    class Objeto : IDibujo
    {
        public Punto cm { get; set; }
        public string name { get; set; }
        public Dictionary<string, Parte> d;
        private Matrix4 m;

        public Objeto()
        {
            d = new Dictionary<string, Parte>();
            m = Matrix4.Identity;
        }

        public Objeto(Punto cm)
        {
            this.cm = cm;
        }

        public Objeto(Objeto o)
        {
            this.cm = o.cm.copiar();
            this.d = new Dictionary<string, Parte>();
            foreach (KeyValuePair<string, Parte> value in o.d)
            {
                addParte(value.Key, value.Value);
            }
        }

        public void addParte(string key, Parte value)
        {
            d.Add(key, value);
        }

        public Parte getParte(string key)
        {
            return d[key];
        }

        public string getParteKey(int index)
        {
            return null;//d.ElementAt(index).Key;
        }

        public void vaciar()
        {
            d.Clear();
        }

        public Parte getParteValue(int index)
        {
            int i = 0;
            foreach (KeyValuePair<string, Parte> value in d)
            {
                if (index == i)
                    return value.Value;
                i = i + 1;
            }
            return null;
        }

        public Dictionary<string, Parte> getDictionary()
        {
            return d;
        }

        public void dibujar(Matrix4 m)
        {
            Matrix4 u = m * this.m;
            foreach (KeyValuePair<string, Parte> value in d)
            {
                value.Value.dibujar(u);
            }
        }

        public string toJson()
        {
            return JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
        }

        public static Objeto toObjeto(string json)
        {
            return JsonConvert.DeserializeObject<Objeto>(json);
        }

        public void serializar(string name)
        {
            string t = toJson();
            Archivos.agregarArhivo(name, t);
        }

        public static T desserializar<T>(string name)
        {
            string t = Archivos.leerArchivo(name);
            return JsonConvert.DeserializeObject<T>(t);
        }

        public Objeto copy()
        {
            return new Objeto(this);
        }

        public void rotar(float x, float y, float z)
        {
            m = Matrix4.CreateTranslation(-cm.x, -cm.y, -cm.z) * m;
            m = (Matrix4.CreateRotationX(x) * Matrix4.CreateRotationY(y) * Matrix4.CreateRotationZ(z)) * m;

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
            foreach (KeyValuePair<string, Parte> value in d)
            {
                value.Value.dispose();
            }
            d.Clear();
            d = null;
        }
    }
}
