using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Datos;
using Entidades;

namespace Negocio
{
    public class ControladorPersonas:ControladorGenerico
    {
        #region Constructores
        /// <summary>
        /// Utilizar siempre el using puesto q maneja la conexion
        /// </summary>
        public ControladorPersonas()
            : base()
        {

        }
        /// <summary>
        /// Para Utilizar sin using. Libera todo el garbage Collector (Espero)
        /// No maneja la coneccion
        /// </summary>
        public ControladorPersonas(bool sinConexion)
            : base(sinConexion)
        {

        }
        /// <summary>
        /// Constructor al cual le paso la coneccion para no utilizar la del Controlador actua si no otra
        /// </summary>
        public ControladorPersonas(Connection c)
            : base(c)
        {
        } 
        #endregion

        private int insertPersona(Persona p)
        {
            string sql = @"INSERT INTO persona
            (
	            nombre_persona,
	            direccion_persona,
	            telefono_persona,
	            celular_persona,
	            email_persona,
	            ciudad,
	            provincia,
	            observaciones,
	            tipo_documento,
	            numero_documento,
	            esproveedor
            )
            VALUES
            (
	            :nombre_persona,
	            :direccion_persona,
	            :telefono_persona,
	            :celular_persona,
	            :email_persona,
	            :ciudad,
	            :provincia,
	            :observaciones,
	            :tipo_documento,
	            :numero_documento,
	            :esproveedor
            )";
            conn.Execute(sql, p.NombrePersona, p.DireccionPersona, p.TelefonoPersona,p.CelularPersona,p.EmailPersona,
                p.Ciudad,p.Provincia,p.Observaciones,p.TipoDocumento,p.NumeroDocumento, p.EsProveedor);
            return conn.LastInsertedId("persona_idpersona_seq");

        }

        private void updatePersona(Persona p)
        {
            string sql = @"UPDATE persona
                        SET
	                        nombre_persona = :p2,
	                        direccion_persona = :p3,
	                        telefono_persona = :p4,
	                        celular_persona = :p5,
	                        email_persona = :p6,
	                        ciudad = :p7,
	                        provincia = :p8,
	                        observaciones = :p9,
	                        tipo_documento = :pp,
	                        numero_documento = :pp1,
	                        esproveedor = :p22
                        WHERE idpersona = :p1";
            conn.Execute(sql, p.NombrePersona, p.DireccionPersona, p.TelefonoPersona, p.CelularPersona, p.EmailPersona,
                p.Ciudad, p.Provincia, p.Observaciones, p.TipoDocumento, p.NumeroDocumento, p.EsProveedor,p.Idpersona);
        }

        private DataTable selectPersona(int idPersona)
        {
            string sql = "SELECT * FROM persona p WHERE p.idpersona=:p1";
            return conn.GetDT(sql, idPersona);
        }

        private DataTable selectPersona(string nombreCompleto)
        {
            string sql = "SELECT * FROM persona p WHERE p.nombre_persona = :p1";
            return conn.GetDT(sql, nombreCompleto);
        }

        private DataTable selectPersonas(string descParcial, bool esProveedor)
        {
            string sql = @"SELECT * FROM persona p WHERE UPPER(p.nombre_persona) LIKE UPPER('%'||:p1||'%') AND p.esproveedor =:p2";
            return conn.GetDT(sql, descParcial, esProveedor);
        }

        /// <summary>
        /// Obtiene una persona.
        /// Si no la encuentra excepcion propia
        /// </summary>
        /// <param name="idPersona"></param>
        /// <returns></returns>
        public Persona BuscarPersona(int idPersona)
        {
            BeginTransaction();
            try
            {
                Persona p = buscarPersona(idPersona);         
                CommitTransaction();
                return p;
            }
            catch (Npgsql.NpgsqlException ex)
            {
                RollbackTransaction();
                ControladorExcepcion.tiraExcepcion(ex);
                return null;
            }
            catch(ExcepcionPropia myEx)
            {
                RollbackTransaction();
                ControladorExcepcion.tiraExcepcion(myEx.Message);
                return null;
            }
        }

        /// <summary>
        /// Obtiene una persona.
        /// Si no la encuentra excepcion propia
        /// </summary>
        /// <param name="idPersona"></param>
        /// <returns></returns>
        internal Persona buscarPersona(int idPersona)
        {
            DataTable dt = selectPersona(idPersona);
            if (dt == null || dt.Rows.Count == 0)
            {
                throw new ExcepcionPropia("No se ha encontrado la persona");
            }
            DataRow row = dt.Rows[0];
            Persona p = mapearPersona(row);
            return p;
        }
        /// <summary>
        /// Mapea una row de la tabla persona en una Persona
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private static Persona mapearPersona(DataRow row)
        {
            Persona p = new Persona();
            p.Idpersona = Convert.ToInt16(row["idpersona"]);
            p.CelularPersona = row["celular_persona"].ToString();
            p.Ciudad = row["ciudad"].ToString();
            p.DireccionPersona = row["direccion_persona"].ToString();
            p.EmailPersona = row["email_persona"].ToString();
            p.EsProveedor = Convert.ToBoolean(row["esproveedor"]);
            p.NombrePersona = row["nombre_persona"].ToString();
            p.NumeroDocumento = row["numero_documento"].ToString();
            p.Observaciones = row["observaciones"].ToString();
            p.Provincia = row["provincia"].ToString();
            p.TelefonoPersona = row["telefono_persona"].ToString();
            p.TipoDocumento = row["tipo_documento"].ToString();
            return p;
        }

        /// <summary>
        /// Busca una lista de personas segun su nombre y si es o no proveedor
        /// si no encuentra nada devuelve uan excepcion
        /// </summary>
        /// <param name="descParcial"></param>
        /// <param name="esProveedor"></param>
        /// <returns></returns>
        internal List<Persona> buscarListPersonas(string descParcial, bool esProveedor)
        {
            DataTable dt = selectPersonas(descParcial, esProveedor);
            if (dt==null&& dt.Rows.Count==0)
            {
                throw new ExcepcionPropia("No se ha encontrado ninguna Persona");
            }
            List<Persona> listP = new List<Persona>();
            foreach (DataRow row in dt.Rows)
            {
                Persona p = mapearPersona(row);
                listP.Add(p);
            }
            return listP;
        }
        /// <summary>        
        /// Busca una lista de personas segun su nombre y si es o no proveedor
        /// si no encuentra nada devuelve uan excepcion
        /// </summary>
        /// <param name="descParcial"></param>
        /// <param name="esProveedor"></param>
        /// <returns></returns>
        public List<Persona> BuscarListPersona(string descParcial, bool esProveedor)
        {
            BeginTransaction();
            try
            {
                List<Persona> listp = buscarListPersonas(descParcial, esProveedor);
                CommitTransaction();
                return listp;
            }
            catch (Npgsql.NpgsqlException ex)
            {
                RollbackTransaction();
                ControladorExcepcion.tiraExcepcion(ex);
                return null;               
            }
            catch(ExcepcionPropia myEx)
            {
                RollbackTransaction();
                ControladorExcepcion.tiraExcepcion(myEx.Message);
                return null;
            }
        }

        public Persona BuscarPersona(string nombreCompleto)
        {
            BeginTransaction();
            try
            {
                DataTable dt = selectPersona(nombreCompleto);
                if (dt==null||dt.Rows.Count==0)
                {
                    throw new ExcepcionPropia("No se ha encontrado la persona");
                }
                DataRow row = dt.Rows[0];
                 Persona p= mapearPersona(row);
                CommitTransaction();
                return p;
            }
            catch (Npgsql.NpgsqlException ex)
            {
                RollbackTransaction();
                ControladorExcepcion.tiraExcepcion(ex);
                return null;
            }
            catch(ExcepcionPropia myEx)
            {
                RollbackTransaction();
                ControladorExcepcion.tiraExcepcion(myEx.Message);
                return null;
            }
        }

        public int AgregarPersona(Persona p)
        {
            BeginTransaction();
            try
            {
               int idpersona = insertPersona(p);
               CommitTransaction();
               return idpersona;

            }
            catch (Npgsql.NpgsqlException ex)
            {
                RollbackTransaction();
                ControladorExcepcion.tiraExcepcion(ex);
                return 0;
            }
            catch (ExcepcionPropia myEx)
            {
                RollbackTransaction();
                ControladorExcepcion.tiraExcepcion(myEx.Message);
                return 0;
            }
        }

        public void ModificarPersona(Persona p)
        {
            BeginTransaction();
            try
            {
                updatePersona(p);
                CommitTransaction();
            }
            catch (Npgsql.NpgsqlException ex)
            {
                RollbackTransaction();
                ControladorExcepcion.tiraExcepcion(ex);
            }
        }
    }
}
