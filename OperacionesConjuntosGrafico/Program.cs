using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace OperacionesConjuntosGrafico
{
    public class Program : Form
    {
        private Button btnUnión;
        private Button btnIntersección;
        private Button btnDiferencia;
        private Button btnDiferenciaSimétrica;
        private Panel pnlGrafico;
        private TextBox txtConjuntoA;
        private TextBox txtConjuntoB;
        private Label lblTiempoEjecucion;
        private Label lblComplejidad;

        public Program()
        {
            // Configuración de botones del menú
            btnUnión = new Button { Text = "Unión", Location = new Point(10, 10), Size = new Size(100, 30) };
            btnIntersección = new Button { Text = "Intersección", Location = new Point(10, 50), Size = new Size(100, 30) };
            btnDiferencia = new Button { Text = "Diferencia", Location = new Point(10, 90), Size = new Size(100, 30) };
            btnDiferenciaSimétrica = new Button { Text = "Dif. Simétrica", Location = new Point(10, 130), Size = new Size(100, 30) };

            txtConjuntoA = new TextBox { Location = new Point(330, 320), Size = new Size(200, 30) };
            txtConjuntoB = new TextBox { Location = new Point(120, 320), Size = new Size(200, 30) };

            pnlGrafico = new Panel { Location = new Point(120, 10), Size = new Size(400, 300), BackColor = Color.White };

            lblTiempoEjecucion = new Label { Location = new Point(10, 360), Size = new Size(500, 30), Text = "Tiempo de ejecución: " };
            lblComplejidad = new Label { Location = new Point(10, 390), Size = new Size(500, 30), Text = "Complejidad Algorítmica: " };

            // Evento para cada botón
            btnUnión.Click += (sender, e) => DibujarOperacion("Unión");
            btnIntersección.Click += (sender, e) => DibujarOperacion("Intersección");
            btnDiferencia.Click += (sender, e) => DibujarOperacion("Diferencia");
            btnDiferenciaSimétrica.Click += (sender, e) => DibujarOperacion("Dif. Simétrica");

            Controls.Add(btnUnión);
            Controls.Add(btnIntersección);
            Controls.Add(btnDiferencia);
            Controls.Add(btnDiferenciaSimétrica);
            Controls.Add(txtConjuntoA);
            Controls.Add(txtConjuntoB);
            Controls.Add(pnlGrafico);
            Controls.Add(lblTiempoEjecucion);
            Controls.Add(lblComplejidad);

            Text = "Operaciones con Conjuntos";
            Size = new Size(550, 500);
        }

        private void DibujarOperacion(string operacion)
        {
            var conjuntoA = txtConjuntoA.Text.Split(',').Select(s => s.Trim()).ToHashSet();
            var conjuntoB = txtConjuntoB.Text.Split(',').Select(s => s.Trim()).ToHashSet();
            HashSet<string> resultado = new HashSet<string>();

            Color colorOperacion = Color.Black;
            string complejidad = string.Empty;
            string resultadoComplejidad = string.Empty;

            // Tamaños de los conjuntos
            int n = conjuntoA.Count;
            int m = conjuntoB.Count;

            // Medición de tiempo
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            DateTime inicio = DateTime.Now;

            switch (operacion)
            {
                case "Unión":
                    resultado = new HashSet<string>(conjuntoA);
                    resultado.UnionWith(conjuntoB);
                    colorOperacion = Color.Purple;
                    complejidad = $"O(n + m)";
                    resultadoComplejidad = $"O({n} + {m}) = {n + m}";
                    break;
                case "Intersección":
                    resultado = new HashSet<string>(conjuntoA);
                    resultado.IntersectWith(conjuntoB);
                    colorOperacion = Color.Green;
                    complejidad = $"O(n)";
                    resultadoComplejidad = $"O({Math.Min(n, m)}) = {Math.Min(n, m)}";
                    break;
                case "Diferencia":
                    resultado = new HashSet<string>(conjuntoA);
                    resultado.ExceptWith(conjuntoB);
                    colorOperacion = Color.Orange;
                    complejidad = $"O(n)";
                    resultadoComplejidad = $"O({n}) = {n}";
                    break;
                case "Dif. Simétrica":
                    resultado = new HashSet<string>(conjuntoA);
                    resultado.SymmetricExceptWith(conjuntoB);
                    colorOperacion = Color.Brown;
                    complejidad = $"O(n + m)";
                    resultadoComplejidad = $"O({n} + {m}) = {n + m}";
                    break;
            }

            stopwatch.Stop();
            DateTime fin = DateTime.Now;
            TimeSpan duracion = stopwatch.Elapsed;

            // Actualizar las etiquetas con los tiempos y la complejidad
            lblTiempoEjecucion.Text = $"Inicio: {inicio:HH:mm:ss.fff}, Fin: {fin:HH:mm:ss.fff}, Duración: {duracion.TotalMilliseconds} ms";
            lblComplejidad.Text = $"Complejidad Algorítmica: {complejidad}, Resultado: {resultadoComplejidad}";

            using (Graphics g = pnlGrafico.CreateGraphics())
            {
                g.Clear(Color.White);

                // Dibujamos los círculos para representar los conjuntos
                Pen pen = new Pen(Color.Black, 2);
                Brush brushA = new SolidBrush(Color.FromArgb(128, Color.Blue));
                Brush brushB = new SolidBrush(Color.FromArgb(128, Color.Red));
                HatchBrush brushOperacion = new HatchBrush(HatchStyle.ForwardDiagonal, colorOperacion, Color.Transparent);

                // Coordenadas y tamaño de los conjuntos
                Rectangle conjuntoA_Rect = new Rectangle(50, 50, 200, 200);
                Rectangle conjuntoB_Rect = new Rectangle(150, 50, 200, 200);

                g.FillEllipse(brushA, conjuntoA_Rect);
                g.FillEllipse(brushB, conjuntoB_Rect);

                // Dibujar la operación
                if (operacion == "Unión" || operacion == "Dif. Simétrica")
                {
                    g.FillEllipse(brushOperacion, conjuntoA_Rect);
                    g.FillEllipse(brushOperacion, conjuntoB_Rect);
                }
                else if (operacion == "Intersección")
                {
                    Region regionA = new Region(conjuntoA_Rect);
                    Region regionB = new Region(conjuntoB_Rect);
                    regionA.Intersect(regionB);
                    g.FillRegion(brushOperacion, regionA);
                }
                else if (operacion == "Diferencia")
                {
                    Region regionA = new Region(conjuntoA_Rect);
                    Region regionB = new Region(conjuntoB_Rect);
                    regionA.Exclude(regionB);
                    g.FillRegion(brushOperacion, regionA);
                }

                // Etiqueta de operación
                Font font = new Font("Arial", 14);
                g.DrawString($"Operación: {operacion}", font, Brushes.Black, new PointF(10, 10));

                // Etiquetas de los conjuntos
                g.DrawString("A", font, Brushes.Black, new PointF(140, 20));
                g.DrawString("B", font, Brushes.Black, new PointF(240, 20));

                // Mostrar el resultado de la operación
                g.DrawString($"Resultado: {string.Join(", ", resultado)}", font, Brushes.Black, new PointF(10, 270));
            }
        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new Program());
        }
    }
}
