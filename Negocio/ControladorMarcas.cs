using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Datos;
using Entidades;

namespace Negocio
{
    public class ControladorMarcas : ControladorGenerico
    {
        #region Constructores
        /// <summary>
        /// constructor por defecto de la clase base
        /// </summary>
        public ControladorMarcas()
            : base()
        {

        }
        /// <summary>
        ///para Utilizar sin using. Libera todo el garbage Collector (Espero)
        /// No maneja la coneccion
        /// </summary>
        public ControladorMarcas(bool sinConexion)
            : base(sinConexion)
        {

        }
        /// <summary>
        ///Constructor al cual le paso la coneccion para no utilizar la del Controlador actua si no otra
        /// </summary>
        public ControladorMarcas(Connection c)
            : base(c)
        {

        }

        #endregion
        private DataTable selectMarcas()
        {
            string sql = @"SELECT * FROM marca m ";
            return conn.GetDT(sql);
        }

        private DataTable selectMarca(int idMarca)
        {
            string sql = @"SELECT * FROM marca m WHERE m.idmarca=:p1";
            return conn.GetDT(sql, idMarca);
        }
        private int insertMarca(Marca m)
        {
            string sql = @"INSERT INTO marca
                        (
	                        descripcion,
	                        fecha_baja
                        )
                        VALUES
                        (
	                        :descripcion,
	                        :fecha_baja
                        )";
            conn.Execute(sql, m.Descripcion, m.FechaBaja);
            return conn.LastInsertedId("marca_idmarca_seq");
        }
        private DataTable updateMarca(Marca m)
        {
            string sql = @"UPDATE marca
                        SET
	                        descripcion = :p1,
	                        fecha_baja = :p2
                        WHERE idmarca=:p3";
            return conn.GetDT(sql, m.Descripcion, m.FechaBaja, m.Idmarca);
        }
        private DataTable deleteMarca(int idMarca)
        {
            string sql = @"DELETE FROM marca WHERE idmarca=:p1";
            return conn.GetDT(sql, idMarca);
        }
        private static Marca mapearMarca(DataRow row)
        {
            Marca m = new Marca();
            m.Descripcion = row["descripcion"].ToString();
            m.FechaBaja = row["fecha_baja"] as DateTime?;
            m.Idmarca = Convert.ToInt32(row["idmarca"]);
            return m;
        }
        /// <summary>
        /// Obtiene una lista de marcas sino devuelve excepcion solo las activas
        /// </summary>
        /// <returns></returns>
        public List<Marca> BuscarListMarca()
        {
            BeginTransaction();
            try
            {
                string sql = @"SELECT * FROM marca m where fecha_baja is null ";

                DataTable dt = conn.GetDT(sql);
                if (dt == null || dt.Rows.Count == 0)
                {
                    throw new ExcepcionPropia("No se han encontrado marcas");
                }
                List<Marca> listM = new List<Marca>();
                foreach (DataRow row in dt.Rows)
                {
                    Marca m = mapearMarca(row);
                    listM.Add(m);
                }
                CommitTransaction();
                return listM.OrderBy(m => m.Descripcion).ToList();
            }           
            catch ( Exception myEx)
            {
                RollbackTransaction();
                ControladorExcepcion.tiraExcepcion(myEx.Message);
                return null;
            }
        }
        /// <summary>
        /// Obtiene una lista de marcas sino devuelve excepcion las anuladas tambien
        /// </summary>
        /// <returns></returns>
        public List<Marca> BuscarListMarcaTodas()
        {
            BeginTransaction();
            try
            {
                DataTable dt = selectMarcas();
                if (dt == null || dt.Rows.Count == 0)
                {
                    throw new ExcepcionPropia("No se han encontrado marcas");
                }
                List<Marca> listM = new List<Marca>();
                foreach (DataRow row in dt.Rows)
                {
                    Marca m = mapearMarca(row);
                    listM.Add(m);
                }
                CommitTransaction();
                return listM.OrderBy(m => m.Descripcion).ToList();
            }
            catch (Exception ex)
            {
                RollbackTransaction();
                ControladorExcepcion.tiraExcepcion(ex.Message);
                return null;
            }
            
        }
        /// <summary>
        /// Obtiene una marca sino excepcion propia
        /// </summary>
        /// <param name="idMarca"></param>
        /// <returns></returns>
        public Marca BuscarMarca(int idMarca)
        {
            BeginTransaction();
            try
            {
                int id = Convert.ToInt32(idMarca);
                DataTable dt = selectMarca(id);
                if (dt == null || dt.Rows.Count == 0)
                {
                    throw new ExcepcionPropia("No se ha encontrado la marca");
                }
                DataRow row = dt.Rows[0];
                Marca m = mapearMarca(row);
                CommitTransaction();
                return m;
            }
            catch (Npgsql.NpgsqlException ex)
            {
                RollbackTransaction();
                ControladorExcepcion.tiraExcepcion(ex);
                return null;
            }
            catch (ExcepcionPropia myEx)
            {
                RollbackTransaction();
                ControladorExcepcion.tiraExcepcion(myEx.Message);
                return null;
            }
            catch (FormatException myEx)
            {
                RollbackTransaction();
                ControladorExcepcion.tiraExcepcion(myEx.Message);
                return null;
            }
        }
        /// <summary>
        /// Modifica uan marca 
        /// </summary>
        /// <param name="idMarca"></param>
        /// <param name="descripcion"></param>
        /// <param name="fechaBaja"></param>
        public void ModificarMarca(int idMarca, string descripcion, DateTime? fechaBaja)
        {
            BeginTransaction();
            try
            {
                Marca m = new Marca();
                m.Descripcion = descripcion;
                m.Idmarca = Convert.ToInt32(idMarca);
                m.FechaBaja = fechaBaja;

                updateMarca(m);
                CommitTransaction();
            }
            catch (Npgsql.NpgsqlException ex)
            {
                RollbackTransaction();
                ControladorExcepcion.tiraExcepcion(ex);
            }
            catch (ExcepcionPropia myEx)
            {
                RollbackTransaction();
                ControladorExcepcion.tiraExcepcion(myEx.Message);
            }
            catch (FormatException myEx)
            {
                RollbackTransaction();
                ControladorExcepcion.tiraExcepcion(myEx.Message);
            }
        }
        public void EliminarMarca(int idMarca)
        {
            BeginTransaction();
            try
            {
                int id = Convert.ToInt32(idMarca);
                deleteMarca(id);
                CommitTransaction();

            }
            catch (Npgsql.NpgsqlException ex)
            {
                RollbackTransaction();
                ControladorExcepcion.tiraExcepcion(ex);
            }
            catch (ExcepcionPropia myEx)
            {
                RollbackTransaction();
                ControladorExcepcion.tiraExcepcion(myEx.Message);
            }
            catch (FormatException myEx)
            {
                RollbackTransaction();
                ControladorExcepcion.tiraExcepcion(myEx.Message);
            }
        }
        public int AgregarMarca(string descripcion, DateTime? fechaBaja)
        {
            BeginTransaction();
            try
            {
                Marca m = new Marca();
                m.Descripcion = descripcion;
                m.FechaBaja = fechaBaja;

                int id = insertMarca(m);
                CommitTransaction();
                return id;
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
            catch (FormatException myEx)
            {
                RollbackTransaction();
                ControladorExcepcion.tiraExcepcion(myEx.Message);
                return 0;
            }
        }

    }
}