using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Datos;
using Entidades;

namespace Negocio
{
    public class ControladorGastos : ControladorGenerico
    {
        #region Constructores
        /// <summary>
        /// Utilizar siempre el using puesto q maneja la conexion
        /// </summary>
        public ControladorGastos()
            : base()
        {



            // _logger = LogManager.GetLogger(this.GetType());//aquí procedemos a inicializar el objeto log.}

        }
        /// <summary>
        /// Para Utilizar sin using. Libera todo el garbage Collector (Espero)
        /// No maneja la coneccion
        /// </summary>
        public ControladorGastos(bool sinConexion)
            : base(sinConexion)
        {

        }
        /// <summary>
        /// Constructor al cual le paso la coneccion para no utilizar la del Controlador actua si no otra
        /// </summary>
        public ControladorGastos(Connection c)
            : base(c)
        {
        }
        #endregion

        #region TipoGastos
        private int insertTipoGasto(string descripcion)
        {
            string sql = @"INSERT INTO tipo_gasto
                            (	                            
	                            descripcion
                            )
                            VALUES
                            (
	                            :descripcion
                            )";
            conn.Execute(sql, descripcion);
            return conn.LastInsertedId("tipo_gasto_idtipo_gasto_seq");
        }

        private void updateTipoGasto(int idTipoGasto, string desc)
        {
            string sql = @"UPDATE tipo_gasto
                            SET
	
	                            descripcion = :p3
                            WHERE idtipo_gasto=:p2";
            conn.Execute(sql, desc, idTipoGasto);
        }

        private DataTable selectTipoGasto(int idTipoGasto)
        {
            string sql = "SELECT * FROM tipo_gasto tg WHERE tg.idtipo_gasto=:p2";
            return conn.GetDT(sql, idTipoGasto);
        }

        private DataTable selectTipoGasto()
        {
            string sql = "SELECT * FROM tipo_gasto tg";
            return conn.GetDT(sql);
        }

        /// <summary>
        /// Obtiene un tipo gasto. Si no lo encuentra devuelve excepcpion propia
        /// </summary>
        /// <param name="idTipoGasto"></param>
        /// <returns></returns>
        private TipoGasto buscarTipoGasto(int idTipoGasto)
        {
            DataTable dt = selectTipoGasto(idTipoGasto);
            if (dt == null || dt.Rows.Count == 0)
            {
                throw new ExcepcionPropia("No se ha encontrado el tipo gasto");
            }
            DataRow row = dt.Rows[0];
            TipoGasto tg = new TipoGasto();
            tg.Descripcion = row["descripcion"].ToString();
            tg.IdTipoGasto = Convert.ToInt32(row["idtipo_gasto"]);
            return tg;
        }

        /// <summary>
        /// Obtiene una lista con todos los tipos dce gasto.
        /// Si no encuentra nada devuelve excepcion propia
        /// </summary>
        public List<TipoGasto> BuscarListTipoGasto()
        {
            BeginTransaction();
            try
            {
                DataTable dt = selectTipoGasto();
                if (dt == null || dt.Rows.Count == 0)
                {
                    throw new ExcepcionPropia("No se han encontrado tipos de gastos");
                }
                List<TipoGasto> listTg = new List<TipoGasto>();
                foreach (DataRow row in dt.Rows)
                {
                    TipoGasto tg = buscarTipoGasto(Convert.ToInt32(row["idtipo_gasto"]));
                    listTg.Add(tg);
                }
                CommitTransaction();
                return listTg;

            }
            catch (ExcepcionPropia myex)
            {
                RollbackTransaction();
                ControladorExcepcion.tiraExcepcion(myex.Message);
                return null;
            }
            catch (Npgsql.NpgsqlException myex)
            {
                RollbackTransaction();
                ControladorExcepcion.tiraExcepcion(myex.Message);
                return null;
            }
        }
        /// <summary>
        /// Obtiene un tipo gasto si no encuentra nada devuelve excepcion propia
        /// </summary>
        /// <param name="idTipoGasto"></param>
        /// <returns></returns>
        public TipoGasto BuscarTipoGasto(int idTipoGasto)
        {
            BeginTransaction();
            try
            {
                TipoGasto tg = buscarTipoGasto(idTipoGasto);
                CommitTransaction();
                return tg;
            }
            catch (ExcepcionPropia myex)
            {
                RollbackTransaction();
                ControladorExcepcion.tiraExcepcion(myex.Message);
                return null;
            }
            catch (Npgsql.NpgsqlException myex)
            {
                RollbackTransaction();
                ControladorExcepcion.tiraExcepcion(myex.Message);
                return null;
            }
        }
        /// <summary>
        /// Agrega un tipo de gasto.
        /// </summary>
        /// <param name="descripcion"></param>
        /// <returns></returns>
        public int AgregarTipoGasto(string descripcion)
        {
            BeginTransaction();
            try
            {
                int id = insertTipoGasto(descripcion);
                CommitTransaction();
                return id;
            }
            catch (ExcepcionPropia myex)
            {
                RollbackTransaction();
                ControladorExcepcion.tiraExcepcion(myex.Message);
                return 0;
            }
            catch (Npgsql.NpgsqlException myex)
            {
                RollbackTransaction();
                ControladorExcepcion.tiraExcepcion(myex.Message);
                return 0;
            }
        }
        /// <summary>
        /// Modifica un tipo de gasto
        /// </summary>
        /// <param name="idTipoGasto"></param>
        /// <param name="descripcion"></param>
        public void ModificarTipoGasto(int idTipoGasto, string descripcion)
        {
            BeginTransaction();
            try
            {
                updateTipoGasto(idTipoGasto, descripcion);
                CommitTransaction();
            }
            catch (ExcepcionPropia myex)
            {
                RollbackTransaction();
                ControladorExcepcion.tiraExcepcion(myex.Message);
            }
            catch (Npgsql.NpgsqlException myex)
            {
                RollbackTransaction();
                ControladorExcepcion.tiraExcepcion(myex.Message);
            }
        }
        /// <summary>
        /// Elimina un tipo de gasto
        /// </summary>
        /// <param name="idTipoGasto"></param>
        public void EliminarTipoGasto(int idTipoGasto)
        {
            BeginTransaction();
            try
            {
                string sql = "DELETE FROM tipo_gasto WHERE idtipo_gasto=:p2";
                conn.Execute(sql, idTipoGasto);
                CommitTransaction();
            }
            catch (ExcepcionPropia myex)
            {
                RollbackTransaction();
                ControladorExcepcion.tiraExcepcion(myex.Message);
            }
            catch (Npgsql.NpgsqlException myex)
            {
                RollbackTransaction();
                ControladorExcepcion.tiraExcepcion(myex.Message);
            }
        }
        #endregion

        private DataTable selectGasto(int idGasto)
        {
            string sql = "SELECT * FROM gasto g WHERE g.idgasto=:p1";
            return conn.GetDT(sql, idGasto);
        }
        private DataTable selectGastos(DateTime fechaDesde, DateTime fechaHasta)
        {
            string sql = "SELECT * FROM gasto g WHERE g.fecha BETWEEN :p1 AND :p3 AND g.fecha_anulado IS NULL";
            return conn.GetDT(sql, fechaDesde, fechaHasta);
        }
        private DataTable selectGastos(DateTime fechaDesde, DateTime fechaHasta, int idSucursal)
        {
            string sql = "SELECT * FROM gasto g WHERE g.fecha BETWEEN :p1 AND :p3 AND g.idsucursal=:p5 AND g.fecha_anulado IS NULL";
            return conn.GetDT(sql, fechaDesde, fechaHasta, idSucursal);
        }
        private DataTable selectFormaPago(int idGasto)
        {
            string sql = "SELECT * FROM forma_pago_gasto fpg WHERE fpg.idgasto=:p1";
            return conn.GetDT(sql, idGasto);
        }

        private void insertFormaPagoGasto(int idGasto, int idTipoFormaPago, decimal monto)
        {
            string sql = @"INSERT INTO forma_pago_gasto
                            (
	                            idtipo_forma_pago,
	                            idgasto,
	                            monto
                            )
                            VALUES
                            (
	                            :idtipo_forma_pago,
	                            :idgasto,
	                            :monto
                            )";
            conn.Execute(sql, idTipoFormaPago, idGasto, monto);
        }
        private void deleteFormaPagoGasto(int idGasto, int idTipoFormaPago)
        {
            string sql = "DELETE FROM forma_pago_gasto WHERE idtipo_forma_pago=:p1 AND idgasto=:p3";
            conn.Execute(sql, idTipoFormaPago, idGasto);
        }

        private int insertGasto(Gasto g)
        {
            string sql = @"INSERT INTO gasto
                            (	                            
	                            descripcion,
	                            idtipo_gasto,
	                            monto,
	                            fecha,
                                idsucursal,
                                idcaja
                            )
                            VALUES
                            (
	                            :descripcion ,
	                            :idtipo_gasto,
	                            :monto ,
	                            :fecha,
                                :g,
                                :p2
                            )	";
            conn.Execute(sql, g.Descripcion, g.TipoGasto.IdTipoGasto, g.Monto, g.Fecha, g.IdSucursal, g.IdCaja);
            return conn.LastInsertedId("gasto_idgasto_seq");
        }
        private void updateGasto(Gasto g)
        {
            string sql = @"UPDATE gasto
                        SET	                        
	                        descripcion = :p6,
	                        idtipo_gasto = :p5,
	                        monto = :p4,
	                        fecha = :p3,
	                        fecha_anulado = :p2
                        WHERE idgasto=:p1";
            conn.Execute(sql, g.Descripcion, g.TipoGasto.IdTipoGasto, g.Monto, g.Fecha, g.FechaAnulado, g.IdGasto);
        }

        private void deleteGasto(int idGasto)
        {

        }

        /// <summary>
        /// Obtiene una lista de formas de pago para un gasto.
        /// Si no encuentra nada devuelve excepcion propia
        /// </summary>
        /// <returns></returns>
        private List<FormaPago> buscarListFormaPago(int idGasto)
        {
            DataTable dt = selectFormaPago(idGasto);
            if (dt == null || dt.Rows.Count == 0)
            {
                throw new ExcepcionPropia("No se han encontrado Formas de pago para el gasto");
            }
            List<FormaPago> listFp = new List<FormaPago>();
            foreach (DataRow row in dt.Rows)
            {
                ControladorFormaPago c_formaPago = new ControladorFormaPago(conn);
                FormaPago fp = c_formaPago.BuscarFormaPago(Convert.ToInt32(row["idtipo_forma_pago"]));
                fp.Monto = Convert.ToDecimal(row["monto"]);
                listFp.Add(fp);
            }
            return listFp;
        }

        /// <summary>
        /// Obtiene un gasto Completo.
        /// Si no encuentra nada devuelve excepcion propia
        /// </summary>
        /// <param name="idGasto"></param>
        /// <returns></returns>
        private Gasto buscarGasto(int idGasto)
        {
            DataTable dt = selectGasto(idGasto);
            if (dt == null || dt.Rows.Count == 0)
            {
                throw new ExcepcionPropia("No se ha encontrado el gasto");
            }
            DataRow row = dt.Rows[0];
            Gasto g = new Gasto();
            g.Descripcion = row["descripcion"].ToString();
            g.Fecha = Convert.ToDateTime(row["fecha"]);
            g.IdGasto = Convert.ToInt32(row["idgasto"]);
            g.Monto = Convert.ToInt32(row["monto"]);
            g.TipoGasto = buscarTipoGasto(Convert.ToInt32(row["idtipo_gasto"]));
            g.FechaAnulado = row["fecha_anulado"] as DateTime?;
            g.ListFormaPago = buscarListFormaPago(g.IdGasto);
            g.IdCaja = Convert.ToInt32(row["idcaja"]);
            ControladorSucursal c_s = new ControladorSucursal(conn);
            g.Sucursal_ = c_s.BuscarSucursal(Convert.ToInt32(row["idsucursal"]));
            return g;

        }
        /// <summary>
        /// Buscar una lista de gastos entre las fechas anulada o no.
        /// Si no encuentra nada devuelve excepcion propia
        /// </summary>
        /// <param name="fechaDesde"></param>
        /// <param name="FechaHasta"></param>
        /// <returns></returns>
        public List<Gasto> BuscarListGastos(DateTime fechaDesde, DateTime FechaHasta, int? idSucursal)
        {

            try
            {
                if (idSucursal == null)
                {
                    DataTable dt = selectGastos(fechaDesde, FechaHasta);
                    if (dt == null || dt.Rows.Count == 0)
                    {
                        throw new ExcepcionPropia("No se han Encontrado Gastos");
                    }
                    List<Gasto> listGastos = new List<Gasto>();
                    foreach (DataRow row in dt.Rows)
                    {
                        Gasto g = buscarGasto(Convert.ToInt32(row["idgasto"]));
                        listGastos.Add(g);
                    }

                    return listGastos;
                }
                else
                {
                    DataTable dt = selectGastos(fechaDesde, FechaHasta, (int)idSucursal);
                    if (dt == null || dt.Rows.Count == 0)
                    {
                        throw new ExcepcionPropia("No se han Encontrado Gastos");
                    }
                    List<Gasto> listGastos = new List<Gasto>();
                    foreach (DataRow row in dt.Rows)
                    {
                        Gasto g = buscarGasto(Convert.ToInt32(row["idgasto"]));
                        listGastos.Add(g);
                    }

                    return listGastos;
                }

            }
            catch (ExcepcionPropia myex)
            {

                ControladorExcepcion.tiraExcepcion(myex.Message);
                return null;
            }
            catch (Npgsql.NpgsqlException myex)
            {

                ControladorExcepcion.tiraExcepcion(myex.Message);
                return null;
            }
        }

        /// <summary>
        /// Agrega un gasto
        /// </summary>
        /// <param name="g"></param>
        /// <returns></returns>
        public int AgregarGasto(Gasto g)
        {
            BeginTransaction();
            try
            {
                if (g.ListFormaPago == null || g.ListFormaPago.Count == 0)
                {
                    throw new ExcepcionPropia("Debe indicar al menos una forma de pago");
                }
                if (g.Monto != g.ListFormaPago.Sum(gg => gg.Monto))
                {
                    throw new ExcepcionPropia("El total del gasto debe ser igual a la suma de las formas de pago");
                }
                int id = insertGasto(g);
                g.IdGasto = id;
                foreach (FormaPago fp in g.ListFormaPago)
                {
                    insertFormaPagoGasto(g.IdGasto, fp.IdtipoFormaPago, fp.Monto);
                }
                CommitTransaction();
                return id;
            }
            catch (ExcepcionPropia myex)
            {
                RollbackTransaction();
                ControladorExcepcion.tiraExcepcion(myex.Message);
                return 0;
            }
            catch (Npgsql.NpgsqlException myex)
            {
                RollbackTransaction();
                ControladorExcepcion.tiraExcepcion(myex.Message);
                return 0;
            }
        }
        /// <summary>
        /// Modificar Gasto
        /// </summary>
        /// <param name="g"></param>
        public void ModificarGasto(Gasto g)
        {
            BeginTransaction();
            try
            {
                if (g.ListFormaPago == null || g.ListFormaPago.Count == 0)
                {
                    throw new ExcepcionPropia("Debe indicar al menos una forma de pago");
                }
                if (g.Monto != g.ListFormaPago.Sum(gg => gg.Monto))
                {
                    throw new ExcepcionPropia("El total del gasto debe ser igual a la suma de las formas de pago");
                }
                foreach (FormaPago fp in g.ListFormaPago)
                {
                    deleteFormaPagoGasto(g.IdGasto, fp.IdtipoFormaPago);
                    insertFormaPagoGasto(g.IdGasto, fp.IdtipoFormaPago, fp.Monto);
                }
                updateGasto(g);
                CommitTransaction();
            }
            catch (ExcepcionPropia myex)
            {
                RollbackTransaction();
                ControladorExcepcion.tiraExcepcion(myex.Message);
            }
            catch (Npgsql.NpgsqlException myex)
            {
                RollbackTransaction();
                ControladorExcepcion.tiraExcepcion(myex.Message);
            }
        }
        /// <summary>
        /// Busca un gasto. Si no lo encuentra devuelve excepcion propia
        /// </summary>
        /// <param name="idGasto"></param>
        public Gasto BuscarGasto(int idGasto)
        {
            BeginTransaction();
            try
            {
                Gasto g = buscarGasto(idGasto);
                CommitTransaction();
                return g;
            }
            catch (ExcepcionPropia myex)
            {
                RollbackTransaction();
                ControladorExcepcion.tiraExcepcion(myex.Message);
                return null;
            }
            catch (Npgsql.NpgsqlException myex)
            {
                RollbackTransaction();
                ControladorExcepcion.tiraExcepcion(myex.Message);
                return null;
            }
        }
        /// <summary>
        /// Setea fecha de gasto
        /// </summary>
        /// <param name="idGasto"></param>
        public void AnularGasto(int idGasto)
        {
            try
            {
                string sql = @"UPDATE gasto
                            SET	
	                            fecha_anulado = :p2
                            WHERE idgasto=:p1";
                conn.Execute(sql, DateTime.Today, idGasto);
            }
            catch (Npgsql.NpgsqlException ex)
            {
                ControladorExcepcion.tiraExcepcion(ex.Message);
            }
        }

    }
}
