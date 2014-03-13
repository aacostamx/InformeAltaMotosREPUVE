using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace ConexiónAntonio
{
    class Conexion
    {
        private string BD;          //DATA BASE
        private string IP;          //IP
        private string USER;        //USER
        private string PASS;        //PASS
        private SqlConnection con = new SqlConnection();
        private string cConexion;   //CADENA DE CONEXION
        private SqlCommand comm = new SqlCommand();
        private SqlDataAdapter da = new SqlDataAdapter();

        public SqlDataAdapter DataAdapter
        {
            get { return da; }
            set { da = value; }
        }

        public SqlCommand Command
        {
            get { return comm; }
            set { comm = value; }
        }

        public string cadenaConexion
        {
            get { return cConexion; }
            set { cConexion = value; }
        }

        public SqlConnection sqlConexion
        {
            get { return con; }
            set { con = value; }
        }

        public string cadenaPassword
        {
            get { return PASS; }
            set { PASS = value; }
        }

        public string cadenaUser
        {
            get { return USER; }
            set { USER = value; }
        }

        public string cadenaIP
        {
            get { return IP; }
            set { IP = value; }
        }

        public string cadenaBaseDatos
        {

            get { return BD; }
            set { BD = value; }
        }

        public Conexion()
        {
            //Conexion default a la base de datos AntonioAcosta servidor de pruebas
            cConexion = new ConexionSQL().LeeArchivo("C:/sys/exe/Conexion/Conexion123.txt");
            Conectar();
        }

        public Conexion(string InitialCatalog, string DataSource, string User, string Password)
        {
            cadenaBaseDatos = InitialCatalog;
            cadenaIP = DataSource;
            cadenaUser = User;
            cadenaPassword = Password;
            cadenaConexion = "Initial Catalog=" + cadenaBaseDatos + ";Data Source=" + cadenaIP + ";UID=" + cadenaUser + ";Pwd=" + cadenaPassword;
            Conectar();
        }

        public Conexion(string connection)
        {
            cadenaConexion = connection;
            Conectar();
        }

        private void Conectar()
        {
            con.ConnectionString = cadenaConexion;
        }

        private void ConexionAbrir()
        {
            if (con.State == ConnectionState.Closed)
                con.Open();
        }

        private void ConexionCerrar()
        {
            if (con.State == ConnectionState.Open)
                con.Close();
        }

        public DataTable LlenarDataTable(ref DataTable dt, string Sentencia)
        {
            try
            {
                ConexionAbrir();
                comm.CommandType = CommandType.Text;
                comm.CommandText = Sentencia;
                comm.CommandTimeout = 10000;
                da.SelectCommand = comm;
                da.SelectCommand.Connection = con;
                da.SelectCommand.CommandTimeout = 10000;
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex) { throw ex; }
            finally { ConexionCerrar(); }
        }

        public void EjecutarSentencia(string Sentencia)
        {
            try
            {
                ConexionAbrir();
                comm.CommandType = CommandType.Text;
                comm.CommandText = Sentencia;
                comm.Connection = con;
                comm.CommandTimeout = 10000;
                comm.ExecuteNonQuery();
            }
            catch (Exception ex) { throw ex; }
            finally { ConexionCerrar(); }
        }
    }
}
