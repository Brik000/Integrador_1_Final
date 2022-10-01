using ProyectoFinal.modelo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;

namespace ProyectoFinal
{
    public partial class Ventana : Form
    {


        public Analizador analizador;
        public Ventana()
        {
            InitializeComponent();
            analizador = new Analizador();
            try
            {
                cargarArchivos();
            }
            catch (Exception e)
            {
                MessageBox.Show(this, e.Message);
                System.Environment.Exit(1);
            }

            cargarHistogramas();
            modificarGraficas();
        }


        public void modificarGraficas()
        {
            histogramaArticulos.MouseEnter += grafica1_MouseEnter;
            histogramaArticulos.MouseLeave += grafica1_MouseLeave;
            histogramaArticulos.MouseWheel += grafica1_MouseWheel;
            histogramaClientes.MouseEnter += grafica2_MouseEnter;
            histogramaClientes.MouseLeave += grafica2_MouseLeave;
            histogramaClientes.MouseWheel += grafica2_MouseWheel;
        }

        private void grafica1_MouseEnter(object sender, EventArgs e)
        {

            this.histogramaArticulos.Focus();
        }

        private void grafica1_MouseLeave(object sender, EventArgs e)
        {
            this.histogramaArticulos.Parent.Focus();
        }


        private void grafica2_MouseEnter(object sender, EventArgs e)
        {

            this.histogramaClientes.Focus();
        }

        private void grafica2_MouseLeave(object sender, EventArgs e)
        {
            this.histogramaClientes.Parent.Focus();
        }


        private void grafica1_MouseWheel(object sender, MouseEventArgs e)
        {
            var chart = (Chart)sender;
            var xAxis = chart.ChartAreas[0].AxisX;
            //var yAxis = chart.ChartAreas[0].AxisY;

            try
            {
                if (e.Delta < 0) // Scrolled down.
                {
                    xAxis.ScaleView.ZoomReset();
                    //yAxis.ScaleView.ZoomReset();
                }
                else if (e.Delta > 0) // Scrolled up.
                {
                    var xMin = xAxis.ScaleView.ViewMinimum;
                    var xMax = xAxis.ScaleView.ViewMaximum;
                    //var yMin = yAxis.ScaleView.ViewMinimum;
                    //var yMax = yAxis.ScaleView.ViewMaximum;

                    var posXStart = xAxis.PixelPositionToValue(e.Location.X) - (xMax - xMin) / 4;
                    var posXFinish = xAxis.PixelPositionToValue(e.Location.X) + (xMax - xMin) / 4;
                    //var posYStart = yAxis.PixelPositionToValue(e.Location.Y) - (yMax - yMin) / 4;
                    //var posYFinish = yAxis.PixelPositionToValue(e.Location.Y) + (yMax - yMin) / 4;

                    xAxis.ScaleView.Zoom(posXStart, posXFinish);
                    //yAxis.ScaleView.Zoom(posYStart, posYFinish);


                }
            }
            catch { }
        }


        private void grafica2_MouseWheel(object sender, MouseEventArgs e)
        {
            var chart = (Chart)sender;
            var xAxis = chart.ChartAreas[0].AxisX;
            //var yAxis = chart.ChartAreas[0].AxisY;

            try
            {
                if (e.Delta < 0) // Scrolled down.
                {
                    xAxis.ScaleView.ZoomReset();
                    //yAxis.ScaleView.ZoomReset();
                }
                else if (e.Delta > 0) // Scrolled up.
                {
                    var xMin = xAxis.ScaleView.ViewMinimum;
                    var xMax = xAxis.ScaleView.ViewMaximum;
                    //var yMin = yAxis.ScaleView.ViewMinimum;
                    //var yMax = yAxis.ScaleView.ViewMaximum;

                    var posXStart = xAxis.PixelPositionToValue(e.Location.X) - (xMax - xMin) / 4;
                    var posXFinish = xAxis.PixelPositionToValue(e.Location.X) + (xMax - xMin) / 4;
                    //var posYStart = yAxis.PixelPositionToValue(e.Location.Y) - (yMax - yMin) / 4;
                    //var posYFinish = yAxis.PixelPositionToValue(e.Location.Y) + (yMax - yMin) / 4;

                    xAxis.ScaleView.Zoom(posXStart, posXFinish);
                    //yAxis.ScaleView.Zoom(posYStart, posYFinish);


                }
            }
            catch { }
        }


        private void cargarHistogramas()
        {
            var articulos = analizador.generarHistogramaArticulos();
            this.desdeArticulos.Minimum = articulos.Min(a => a.Key);
            this.desdeArticulos.Maximum = articulos.Max(a => a.Key);
            this.hastaArticulos.Minimum = articulos.Min(a => a.Key);
            this.hastaArticulos.Maximum = articulos.Max(a => a.Key);


           


            var clientes = analizador.generarHistogramaClientes();
                    this.desdeClientes.Minimum = clientes.Min(a => a.Key);
            this.desdeClientes.Maximum = clientes.Max(a => a.Key);
            this.hastaClientes.Minimum = clientes.Min(a => a.Key);
            this.hastaClientes.Maximum = clientes.Max(a => a.Key);
            this.histogramaArticulos.Series[0].Points.Clear();
          
            this.histogramaArticulos.ChartAreas[0].AxisY.Maximum = articulos.Max(a => a.Value)+20;
            this.histogramaArticulos.ChartAreas[0].AxisX.Title = "Numero de articulos por transaccion";
            this.histogramaArticulos.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            this.histogramaArticulos.ChartAreas[0].AxisY.ScaleView.Zoomable = false;
            this.histogramaClientes.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            this.histogramaClientes.ChartAreas[0].AxisY.ScaleView.Zoomable = false;
            this.histogramaArticulos.ChartAreas[0].AxisY.Title = "Frecuencia absoluta";
            this.histogramaArticulos.Series[0].XValueType = ChartValueType.Int32;
            this.histogramaClientes.Series[0].XValueType = ChartValueType.Int32;
            histogramaClientes.Series[0]["PointWidth"] = "1";
                histogramaArticulos.Series[0]["PointWidth"] = "1";
            foreach (var cosa in articulos)
            {
                histogramaArticulos.Series[0].Points.AddXY(cosa.Key, cosa.Value);
            }

            this.histogramaClientes.Series[0].Points.Clear();
            this.histogramaArticulos.Titles.Add("Articulos comprados");
            this.histogramaClientes.Titles.Add("Frecuencia de compras");
            this.histogramaClientes.ChartAreas[0].AxisY.Maximum = clientes.Max(a => a.Value)+20;
            this.histogramaClientes.ChartAreas[0].AxisX.Title = "Frecuencia de compra de cliente";
            this.histogramaClientes.ChartAreas[0].AxisY.Title = "Frecuencia absoluta";
            this.histogramaClientes.Series[0].ToolTip = "(#VALX,#VALY)";
            this.histogramaArticulos.Series[0].ToolTip = "(#VALX,#VALY)";
            foreach (var cosa in clientes)
            {
                histogramaClientes.Series[0].Points.AddXY(cosa.Key, cosa.Value);
            }
        }

        private void cargarArchivos()
        {
            String desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            String transacciones = Path.Combine(desktop, "DatosMineria/transacciones.txt");
            String items = Path.Combine(desktop, "DatosMineria/items.txt");
            String clientes = Path.Combine(desktop, "DatosMineria/clientes.txt");

            if (!File.Exists(transacciones) || !File.Exists(items) || !File.Exists(clientes))
            {
                throw new Exception("Los archivos de datos no existen, por favor proporcionelos.");
            }
            else
            {
               


                try
                {
                    analizador.cargarClientes(clientes);
                    analizador.cargarItems(items);
                    analizador.cargarTransacciones(transacciones);
                    MessageBox.Show(this, "Archivos cargados satisfactoriamente.");

                }
                catch (Exception e)
                {
                    throw new Exception("Formato de datos fallido, codigo de error:\n" + e.Message+"\n"+e.StackTrace);
                }
            }
        }

        private void btnLimitar_Click(object sender, EventArgs e)
        {
            var trans = analizador.aplicarFiltros(Int32.Parse(desdeArticulos.Value.ToString()), Int32.Parse(hastaArticulos.Value.ToString()), Int32.Parse(desdeClientes.Value.ToString()), Int32.Parse(hastaClientes.Value.ToString()));
            var timer = System.Diagnostics.Stopwatch.StartNew();
            bruteForce.DataSet data = new bruteForce.DataSet(analizador, 0.8,0.2);


            Console.WriteLine("Asociaciones Validas:" + data.asociaciones.Count);
            Console.WriteLine("Running time (ms):" + timer.Elapsed.TotalMilliseconds);
            foreach (bruteForce.Asociacion a in data.asociaciones)
            {
                Console.WriteLine("{" + String.Join(",", a.de) + "} ----> {" + String.Join(",", a.a) + "}");
            }
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void histogramaClientes_Click(object sender, EventArgs e)
        {

        }
    }
}
