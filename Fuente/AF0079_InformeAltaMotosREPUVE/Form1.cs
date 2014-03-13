using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ConexiónAntonio;
using System.IO;

namespace AF0079_InformeAltaMotosREPUVE
{
    public partial class Form1 : Form
    {
        ConexionSQL con = new ConexionSQL();
        CrystalDecisions.CrystalReports.Engine.ReportDocument RptREPUVE = new CrystalDecisions.CrystalReports.Engine.ReportDocument();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }

            if (e.KeyCode == Keys.F1)
            {
                Ayuda obj = new Ayuda();
                this.Hide();
                obj.ShowDialog();
                this.Show();
            }
        }

        private void bInciar_Click(object sender, EventArgs e)
        {
            lbEstatus.Text = "";

            if ((dtpFechaInicio.Value.Date > dtpFechaFinal.Value.Date))
            {
                MessageBox.Show("La fecha inicial no puede ser mayor a la fecha final", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return;
            }

            InformeAltaMotosREPUVE();
        }

        private void InformeAltaMotosREPUVE()
        {
            this.lbSistemaHoraInicio.Text = "--";
            this.lbSistemaHoraFinal.Text = "--";
            string conexionCT11;
            string sentencia = "EXEC AF0079_InformeAltaMotosREPUVE ";
            string dropTable = "if exists (select * from sysobjects where name = 'AltaMotosREPUVE') drop table AltaMotosREPUVE";

            Refresh();

            dtpFechaInicio.Enabled = false;
            dtpFechaFinal.Enabled = false;

            lbSistemaHoraInicio.Text = DateTime.Now.ToShortTimeString();
            Refresh();

            this.Cursor = Cursors.WaitCursor;
            bInciar.Enabled = false;
            lbEstatus.Visible = true;
            lbEstatus.Text = "Generando Informe de Motos REPUVE";

            DateTime FechaInicio = dtpFechaInicio.Value.Date;
            DateTime FechaFinal = dtpFechaFinal.Value.Date;
            FechaInicio = FechaInicio.AddDays(-1);
            FechaFinal = FechaFinal.AddDays(-1);

            string fInicio = FechaInicio.ToString("yyyyMMdd");
            string fFinal = FechaFinal.ToString("yyyyMMdd");

            sentencia = sentencia + "'" + fInicio + "'," + "'" + fFinal + "'";

            if (!File.Exists("C:/Sys/Exe/Conexion/ConexionCT.txt"))
            {
                MessageBox.Show("El archivo txt de conexión no existe","Advertencia");
                bInciar.Enabled = true;
                Cursor = Cursors.Default;
                dtpFechaInicio.Enabled = true;
                dtpFechaFinal.Enabled = true;
                lbEstatus.Text = "Error: el archivo de conexión no existe";
                return;
            }
            conexionCT11 = con.LeeArchivo("C:/Sys/Exe/Conexion/ConexionCT.txt");
            Conexion conexion = new Conexion(conexionCT11);
            conexion.EjecutarSentencia(sentencia);
            Refresh();

            //Genera el reporte
            RptREPUVE.FileName = @"rassdk://C:\sys\Crystal\AF0079_InformeAltaMotosREPUVE.rpt";
            RptREPUVE.DataSourceConnections[0].SetConnection(con.IP, con.DB, con.USER, con.PASS);
            RptREPUVE.Refresh();
            RptREPUVE.SetDatabaseLogon(con.USER, con.PASS);
            RptREPUVE.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA3;
            RptREPUVE.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
            //RptREPUVE.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.Excel, "AF0079_InformeAltaMotosREPUVE" + fInicio + "_" + fFinal + ".xls"); //Muestra la información sin pintar las celdas
            RptREPUVE.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.ExcelRecord, "AF0079_InformeAltaMotosREPUVE_" + fInicio + "_" + fFinal + ".xls");  //Muestra la información con las celdas pintadas

            conexion.EjecutarSentencia(dropTable);

            RptREPUVE.Refresh();

            lbSistemaHoraFinal.Text = DateTime.Now.ToShortTimeString();
            MessageBox.Show("Informe de Motos REPUVE Generado", "Notificación");
            Refresh();

            lbEstatus.Text = "Informe de Motocicletas REPUVE Generado";
            bInciar.Enabled = true;
            Cursor = Cursors.Default;
            lbSistemaHoraFinal.Text = DateTime.Now.ToShortTimeString();
            dtpFechaInicio.Enabled = true;
            dtpFechaFinal.Enabled = true;
        }
    }
}