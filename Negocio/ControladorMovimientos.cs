using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entidades;
using Datos;

namespace Negocio
{

    public class ControladorMovimientos : ControladorGenerico
    {
        #region Constructores
        /// <summary>
        /// constructor por defecto de la clase base
        /// </summary>
        public ControladorMovimientos()
            : base()
        {

        }
        /// <summary>
        ///para Utilizar sin using. Libera todo el garbage Collector (Espero)
        /// No maneja la coneccion
        /// </summary>
        public ControladorMovimientos(bool sinConexion)
            : base(sinConexion)
        {

        }
        /// <summary>
        ///Constructor al cual le paso la coneccion para no utilizar la del Controlador actua si no otra
        /// </summary>
        public ControladorMovimientos(Connection c)
            : base(c)
        {

        }

        #endregion

        /// <summary>
        /// Si no encuentra nada devuelve null
        /// </summary>
        /// <returns></returns>
        public List<TipoMovimientoArticulo> BuscarListTiposMovimientos()
        {
            try
            {
                string sql = "SELECT * FROM tipo_movimiento_articulo tma WHERE tma.fecha_baja IS NULL";
                DataTable dt = conn.GetDT(sql);
                if (dt == null || dt.Rows.Count == 0)
                {
                    return null;
                }
                else
                {
                    List<TipoMovimientoArticulo> listT = new List<TipoMovimientoArticulo>();
                    foreach (DataRow row in dt.Rows)
                    {
                        TipoMovimientoArticulo tm = mapearTipoMovimiento(row);
                        listT.Add(tm);
                    }
                    return listT;
                }
            }
            catch (Npgsql.NpgsqlException ex)
            {
                ControladorExcepcion.tiraExcepcion(ex);
                return null;
            }
        }

        private static TipoMovimientoArticulo mapearTipoMovimiento(DataRow row)
        {
            TipoMovimientoArticulo tm = new TipoMovimientoArticulo();
            tm.Descripcion = row["descripcion"].ToString();
            tm.EsSuma = Convert.ToBoolean(row["es_suma"]);
            tm.IdTipoMovimiento = Convert.ToInt32(row["idtipo_movimiento"]);
            tm.FechaBaja = row["fecha_baja"] as DateTime?;
            return tm;
        }
        /// <summary>
        /// Si no encuentra nada tira excepcion propia
        /// </summary>
        /// <param name="idTipoMov"></param>
        /// <returns></returns>
        public TipoMovimientoArticulo BuscarTipoMovimiento(int idTipoMov)
        {
            try
            {
                string sql = "SELECT * FROM tipo_movimiento_articulo tma WHERE tma.idtipo_movimiento=:p2";
                DataTable dt = conn.GetDT(sql, idTipoMov);
                if (dt == null || dt.Rows.Count == 0)
                {
                    throw new ExcepcionPropia("No se ha encontrado el tipo de movimiento");
                }
                DataRow row = dt.Rows[0];
                return mapearTipoMovimiento(row);
            }
            catch (Npgsql.NpgsqlException ex)
            {
                ControladorExcepcion.tiraExcepcion(ex);
                return null;
            }
        }


        public void AgregarAjusteStock(MovimientosArticulos m)
        {
            BeginTransaction();
            try
            {
                ///inserto el movimiento
                string sql = @"INSERT INTO movimiento_articulos
                                (                               
                                idsucursal_desde,
                                idartciulo,
                                idtipo_movimiento,
                                cantidad,
                                fecha,
                                idusuario,
                                observacion
                                )
                                VALUES
                                (
                                :idsucursal_desde,
                                :idartciulo,
                                :idtipo_movimiento,
                                :cantidad,
                                :fecha,
                                :idusuario,
                                :observacion
                                )";
                conn.Execute(sql, m.IdSucursalDesde, m.IdArticulo_, m.IdTipoMovimiento, m.Cantidad, DateTime.Today, m.IdUsuario, m.Observacion);

                ///Ahora actualizo el stock del articulo
                TipoMovimientoArticulo tm = BuscarTipoMovimiento((int)m.IdTipoMovimiento);

                ControladorArticulos c_Art = new ControladorArticulos(conn);
                if (tm.EsSuma)
                {
                    c_Art.ActualizarStockArticulo(m.IdArticulo_, m.Cantidad, m.IdSucursalDesde);
                }
                else
                {

                    c_Art.ActualizarStockArticulo(m.IdArticulo_, -m.Cantidad, m.IdSucursalDesde);
                }

                CommitTransaction();
            }
            catch (Npgsql.NpgsqlException ex)
            {
                RollbackTransaction();
                ControladorExcepcion.tiraExcepcion(ex);
            }
            catch (FormatException ex)
            {
                RollbackTransaction();
                ControladorExcepcion.tiraExcepcion(ex.Message);
            }
        }

        public void AgregarMovimientoEntreSucursales(MovimientosArticulos m)
        {
            BeginTransaction();
            try
            {
                string sql = @"INSERT INTO movimiento_articulos_sucursales
                                (	
	                                idsucursal_desde,
	                                idsucursal_hasta,
	                                idarticulo,
	                                cantidad,
	                                fecha,
	                                idusuario,
	                                observacion
                                )
                                VALUES
                                (
	                                :idsucursal_desde,
	                                :idsucursal_hasta,
	                                :idarticulo,
	                                :cantidad,
	                                :fecha,
	                                :idusuario,
	                                :observacion
                                )";
                conn.Execute(sql, m.IdSucursalDesde, m.IdSucursalHasta, m.IdArticulo_, m.Cantidad, m.Fecha, m.IdUsuario, m.Observacion);
                ControladorArticulos c_Art = new ControladorArticulos(conn);
                ///Resto articulo en sucursal Desde
                c_Art.ActualizarStockArticulo(m.IdArticulo_, -m.Cantidad, m.IdSucursalDesde);
                ///Sumo Articulkos en sucursal hasta
                c_Art.ActualizarStockArticulo(m.IdArticulo_, m.Cantidad, (int)m.IdSucursalHasta);
                CommitTransaction();
            }
            catch (Npgsql.NpgsqlException ex)
            {
                RollbackTransaction();
                ControladorExcepcion.tiraExcepcion(ex);

            }
        }

        /// <summary>
        /// Devuelve movimientos de stock y de sucursales, en los q este presente la sucursal indicada
        /// si no encuentra nada devuelve null
        /// </summary>
        /// <param name="fechaDesde"></param>
        /// <param name="fechaHasta"></param>
        /// <returns></returns>
        public List<MovimientosArticulos> BuscarListMovimientosArtculos(DateTime fechaDesde, DateTime fechaHasta, int idSucursal)
        {
            try
            {
                List<MovimientosArticulos> listm = new List<MovimientosArticulos>();
                List<MovimientosArticulos> listMSuc = new List<MovimientosArticulos>();
                listm = buscarListMovStock(fechaDesde, fechaHasta, idSucursal);
                if (listm == null)
                {
                    listm = new List<MovimientosArticulos>();
                }
                listMSuc = buscarListMovSucursales(fechaDesde, fechaHasta, idSucursal);
                if (listMSuc != null)
                {
                    listm.AddRange(listMSuc);
                }
                return listm;

            }
            catch (Npgsql.NpgsqlException ex)
            {
                ControladorExcepcion.tiraExcepcion(ex);
                return null;
            }
        }
        /// <summary>
        /// Devuelve movimientos de stock, en los q este presente la sucursal indicada
        /// si no encuentra nada devuelve null
        /// </summary>
        /// <param name="fechaDesde"></param>
        /// <param name="fechaHasta"></param>
        /// <param name="idSucursal"></param>
        /// <returns></returns>
        private List<MovimientosArticulos> buscarListMovStock(DateTime fechaDesde, DateTime fechaHasta, int idSucursal)
        {
            string sql = "SELECT * FROM movimiento_articulos ma WHERE ma.idsucursal_desde = :p3 and  ma.fecha BETWEEN :p1 AND :p2";
            DataTable dt = conn.GetDT(sql, idSucursal, fechaDesde, fechaHasta);
            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }
            ControladorArticulos c_art = new ControladorArticulos(conn);
            ControladorSucursal c_suc = new ControladorSucursal(conn);
            List<MovimientosArticulos> listMa = new List<MovimientosArticulos>();
            foreach (DataRow row in dt.Rows)
            {
                MovimientosArticulos m = new MovimientosArticulos();
                m.IdMovimiento = Convert.ToInt32(row["idmovimiento"]);
                m.Articulo_ = c_art.BuscarArticulo(Convert.ToInt32(row["idartciulo"]));
                m.Cantidad = Convert.ToInt32(row["cantidad"]);
                m.Fecha = Convert.ToDateTime(row["fecha"]);
                m.IdUsuario = Convert.ToInt32(row["idusuario"]);
                m.Observacion = row["observacion"].ToString();
                m.SucursalDesde = c_suc.BuscarSucursal(Convert.ToInt32(row["idsucursal_desde"]));
                m.TipoMovimiento = BuscarTipoMovimiento(Convert.ToInt32(row["idtipo_movimiento"]));
                listMa.Add(m);
            }
            return listMa;
        }
        /// <summary>
        /// Devuelve movimientos de sucursales, en los q este presente la sucursal indicada
        /// Si no encuentra nada devuelve null
        /// </summary>
        /// <param name="fechaDesde"></param>
        /// <param name="fechaHasta"></param>
        /// <param name="idSucursal"></param>
        /// <returns></returns>
        private List<MovimientosArticulos> buscarListMovSucursales(DateTime fechaDesde, DateTime fechaHasta, int idSucursal)
        {
            string sql = "SELECT * FROM movimiento_articulos_sucursales mas WHERE  (mas.idsucursal_desde =:p3 OR mas.idsucursal_hasta=:p4) and  mas.fecha BETWEEN :p1 AND :p2";
            DataTable dt = conn.GetDT(sql, idSucursal, idSucursal, fechaDesde, fechaHasta);
            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }
            ControladorArticulos c_art = new ControladorArticulos(conn);
            ControladorSucursal c_suc = new ControladorSucursal(conn);
            List<MovimientosArticulos> listMa = new List<MovimientosArticulos>();
            foreach (DataRow row in dt.Rows)
            {
                MovimientosArticulos m = new MovimientosArticulos();
                m.IdMovimiento = Convert.ToInt32(row["idmovimiento"]);
                m.Articulo_ = c_art.BuscarArticulo(Convert.ToInt32(row["idarticulo"]));
                m.Cantidad = Convert.ToInt32(row["cantidad"]);
                m.Fecha = Convert.ToDateTime(row["fecha"]);
                m.IdUsuario = Convert.ToInt32(row["idusuario"]);
                m.Observacion = row["observacion"].ToString();
                m.SucursalDesde = c_suc.BuscarSucursal(Convert.ToInt32(row["idsucursal_desde"]));
                m.SucursalHasta = c_suc.BuscarSucursal(Convert.ToInt32(row["idsucursal_hasta"]));
                listMa.Add(m);

            }
            return listMa;
        }

    }
}
