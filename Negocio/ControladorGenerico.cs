using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Datos;

namespace Negocio
{
    public  class ControladorGenerico : IDisposable
    {
        //protected static ILog _logger = LogManager.GetLogger(typeof(C_Generico));

        protected bool connEsExterna = false;
        protected Connection conn;
        /// <summary>
        /// Utilizar siempre el using puesto q maneja la conexion
        /// </summary>
        public ControladorGenerico()
        {
            conn = new Connection();
            conn.Open();

            
          // _logger = LogManager.GetLogger(this.GetType());//aquí procedemos a inicializar el objeto log.}

        }
        /// <summary>
        /// Para Utilizar sin using. Libera todo el garbage Collector (Espero)
        /// No maneja la coneccion
        /// </summary>
        public ControladorGenerico(bool sinConexion)
        {

        }
        /// <summary>
        /// Constructor al cual le paso la coneccion para no utilizar la del Controlador actua si no otra
        /// </summary>
        public ControladorGenerico(Connection c)
        {
            conn = c;
            connEsExterna = true;
        }
        //utilizamos este metodo para no tener q implementar el constructor en cada clase derivada
        //public void AsignarConexion(Connection c)
        //{
        //    conn.Close();
        //    conn.Dispose();
        //    conn = c;
        //    connEsExterna = true;
        //}


        protected void BeginTransaction()
        {
            if (!connEsExterna)
            {
                conn.BeginTransaction();
            }

        }
        protected void CommitTransaction()
        {
            if (!connEsExterna)
            {
                conn.CommitTransaction();
            }

        }
        protected void RollbackTransaction()
        {
            if (!connEsExterna)
            {
                conn.RollbackTransaction();
            }     
        }

        public void Dispose()
        {
            if (conn != null && connEsExterna == false)
            {
                if (conn.State == State.Open)
                    conn.Close();
                conn.Dispose(); 
            }
        }
    }
}
