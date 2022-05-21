using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Casa7.model;
using Casa7.objetos;
using OpenTK;
using OpenTK.Graphics.OpenGL;


namespace Casa7
{
    public partial class Form1 : Form
    {
        private Timer _timer = null!;
        private float _angle = 0.0f;
        Escenario escenario;

        public Form1()
        {
            InitializeComponent();
        }

        private void glControl1_Load(object sender, EventArgs e)
        {
            glControl1.Resize += glControl1_Resize;
            glControl1.Paint += glControl1_Paint;

            _timer = new Timer();
            _timer.Tick += (sender, e) =>
            {
                _angle += 0.5f;
                Render();
            };
            _timer.Interval = 50;   // 1000 ms por sec / 50 ms por frame = 20 FPS
            _timer.Start();

            glControl1_Resize(glControl1, EventArgs.Empty);
            onLoadOpentk();
        }

        private void onLoadOpentk()
        {
            GL.ClearColor(Color.Black);

            if (escenario != null)
            {
                escenario.dispose();
            }


            escenario = new Escenario();
            escenario.name = "Escenario 1";

            Casa casa = new Casa(5, 5, 5, 0, 0, 0);
            Casa casa1 = new Casa(5, 5, 5, 0, 0, 0);

            casa.serializar("Casa.txt");
            casa1.serializar("Casa1.txt");


            Casa c1 = Objeto.desserializar<Casa>("Casa.txt");
            c1.init();
            Casa c2 = Objeto.desserializar<Casa>("Casa1.txt");
            c2.init();

            escenario.addObjeto("casa", c1);
            escenario.addObjeto("casa1", c2);

            escenario.getObjeto("casa1").trasladar(10, 10, 0);

            //Rellenamos los comboBox
            rellenarObjetosComboBox();
            comboBox2.Enabled = false;
        }

        private void onUnloadOpentk()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            //GL.DeleteBuffer(VertexBufferObject);
        }

        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            Render();
        }

        private void glControl1_Resize(object sender, EventArgs e)
        {
            float d = 30;
            glControl1.MakeCurrent();

            if (glControl1.ClientSize.Height == 0)
                glControl1.ClientSize = new System.Drawing.Size(glControl1.ClientSize.Width, 1);

            GL.Viewport(0, 0, glControl1.ClientSize.Width, glControl1.ClientSize.Height);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(-d, d, -d, d, -d, d);
            //GL.Frustum(-80, 80, -80, 80, 4, 100);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
        }

        private void Render()
        {
            GL.DepthMask(true);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);
            GL.LoadIdentity();

            float gg = 0.01f;
            escenario.rotar(0, gg, 0);
            //escenario.getObjeto("casa").trasladar(0.2f, 0, 0);
            //escenario.getObjeto("casa").rotar(0, 0, gg);

            //escenario.getObjeto("casa1").ampliar(1.001f, 1.001f, 1);



            escenario.dibujar(Matrix4.Identity);



            glControl1.SwapBuffers();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                comboBox2.Items.Clear();
                comboBox2.Enabled = false;
            }
            else 
            {
                comboBox2.Enabled = true;
                string key = comboBox1.SelectedItem.ToString();
                rellenarPartesComboBox(key);
            }
            


        }

        private void rellenarObjetosComboBox()
        {
            comboBox1.Items.Clear();
            comboBox1.Items.Add(escenario.name);
            Dictionary<string, Objeto> d = escenario.getDictionary();
            foreach (KeyValuePair<string, Objeto> value in d)
            {
                comboBox1.Items.Add(value.Key);
            }
            comboBox1.SelectedItem = comboBox1.Items[0];
        }

        private void rellenarPartesComboBox(string keyObjeto)
        {
            comboBox2.Items.Clear();
            Objeto ob = escenario.getObjeto(keyObjeto);
            Dictionary<string, Parte> d = ob.getDictionary();
            comboBox2.Items.Add(keyObjeto);
            foreach (KeyValuePair<string, Parte> value in d)
            {
                comboBox2.Items.Add(value.Key);
            }

            comboBox2.SelectedItem = comboBox2.Items[0];
        }

        private void button1_Click(object sender, EventArgs e)
        {

            float x, y, z;
            try
            {
                x = float.Parse(textBox1.Text);
                y = float.Parse(textBox2.Text);
                z = float.Parse(textBox3.Text);
            }
            catch (Exception exception)
            {
                return;
            }

            if (comboBox1.SelectedIndex == 0)
            {
                escenario.trasladar(x, y, z);
            }
            else
            {
                string keyObjeto = comboBox1.SelectedItem.ToString();
                Objeto ob = escenario.getObjeto(keyObjeto);
                if (comboBox2.SelectedIndex == 0)
                {
                    ob.trasladar(x, y, z);
                }
                else
                {
                    string keyParte = comboBox2.SelectedItem.ToString();
                    Parte parte = ob.getParte(keyParte);
                    parte.trasladar(x, y, z);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            float x, y, z;
            try
            {
                x = float.Parse(textBox1.Text);
                y = float.Parse(textBox2.Text);
                z = float.Parse(textBox3.Text);
            }
            catch (Exception exception)
            {
                return;
            }

            if (comboBox1.SelectedIndex == 0)
            {
                escenario.rotar(x, y, z);
            }
            else
            {
                string keyObjeto = comboBox1.SelectedItem.ToString();
                Objeto ob = escenario.getObjeto(keyObjeto);
                if (comboBox2.SelectedIndex == 0)
                {
                    ob.rotar(x, y, z);
                }
                else
                {
                    string keyParte = comboBox2.SelectedItem.ToString();
                    Parte parte = ob.getParte(keyParte);
                    parte.rotar(x, y, z);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            float x, y, z;
            try
            {
                x = float.Parse(textBox1.Text);
                y = float.Parse(textBox2.Text);
                z = float.Parse(textBox3.Text);
            }catch(Exception exception)
            {
                return;
            }

            if (comboBox1.SelectedIndex == 0)
            {
                escenario.ampliar(x, y, z);
            }
            else
            {
                string keyObjeto = comboBox1.SelectedItem.ToString();
                Objeto ob = escenario.getObjeto(keyObjeto);
                if (comboBox2.SelectedIndex == 0)
                {
                    ob.ampliar(x, y, z);
                }
                else
                {
                    string keyParte = comboBox2.SelectedItem.ToString();
                    Parte parte = ob.getParte(keyParte);
                    parte.ampliar(x, y, z);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            onLoadOpentk();
        }
    }
}
