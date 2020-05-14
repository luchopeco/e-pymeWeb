using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Datos;
using Entidades;

namespace Negocio
{
    public class ControladorInformes : ControladorGenerico
    {
        #region Constructores
        /// <summary>
        /// constructor por defecto de la clase base
        /// </summary>
        public ControladorInformes()
            : base()
        {

        }
        /// <summary>
        ///para Utilizar sin using. Libera todo el garbage Collector (Espero)
        /// No maneja la coneccion
        /// </summary>
        public ControladorInformes(bool sinConexion)
            : base(sinConexion)
        {

        }
        /// <summary>
        ///Constructor al cual le paso la coneccion para no utilizar la del Controlador actua si no otra
        /// </summary>
        public ControladorInformes(Connection c)
            : base(c)
        {

        }

        #endregion
        public DataTable BuscarVentasCantidadesYTotalesPorArticulos(DateTime fechaDesde, DateTime fechaHasta, int idSucursal)
        {
            try
            {
                string sql = @"SELECT ta.descripcion ||' '||a.descripcion||' '||m.descripcion  articulo, 
                                SUM(vd.cantidad) cantidad , SUM(vd.precio_unitario * vd.cantidad) importe
                                FROM venta_detalle vd
                                INNER JOIN venta v ON v.idventa = vd.idventa
                                INNER JOIN articulo a ON a.idarticulo = vd.idarticulo
                                INNER JOIN marca m ON m.idmarca = a.idmarca
                                INNER JOIN tipo_articulo ta ON ta.idtipo_articulo = a.idtipoarticulo
                                WHERE v.fecha_baja IS NULL 
                                AND v.idsucursal=:p3
                                AND vd.fecha_baja IS NULL
                                AND v.fecha BETWEEN :p1 AND :p2
                                GROUP BY articulo
                                ";
                DataTable dt = conn.GetDT(sql, idSucursal, fechaDesde, fechaHasta);
                if (dt == null || dt.Rows.Count == 0)
                {
                    throw new ExcepcionPropia("No se han encontrado Datos");
                }
                else
                {
                    return dt;
                }
            }
            catch (Npgsql.NpgsqlException ex)
            {
                ControladorExcepcion.tiraExcepcion(ex.Message);
                return null;
            }
        }
        public DataTable BuscarVentasCantidadesYTotalesPorMarcas(DateTime fechaDesde, DateTime fechaHasta, int idSucursal)
        {
            try
            {
                string sql = @"SELECT m.descripcion  marca, 
                                SUM(vd.cantidad) cantidad , SUM(vd.precio_unitario * vd.cantidad) importe
                                FROM venta_detalle vd
                                INNER JOIN venta v ON v.idventa = vd.idventa
                                INNER JOIN articulo a ON a.idarticulo = vd.idarticulo
                                INNER JOIN marca m ON m.idmarca = a.idmarca
                                INNER JOIN tipo_articulo ta ON ta.idtipo_articulo = a.idtipoarticulo
                                WHERE v.fecha_baja IS NULL 
                                AND vd.fecha_baja IS NULL
                                AND v.idsucursal = :p3
                                AND v.fecha BETWEEN :p1 AND :p2
                                GROUP BY marca";
                DataTable dt = conn.GetDT(sql, idSucursal, fechaDesde, fechaHasta);
                if (dt == null || dt.Rows.Count == 0)
                {
                    throw new ExcepcionPropia("No se han encontrado Datos");
                }
                else
                {
                    return dt;
                }
            }
            catch (Npgsql.NpgsqlException ex)
            {
                ControladorExcepcion.tiraExcepcion(ex.Message);
                return null;
            }
        }
        public DataTable BuscarVentasCantidadesYTotalesPorTipoArticulo(DateTime fechaDesde, DateTime fechaHasta, int idSucursal)
        {
            try
            {
                string sql = @"SELECT ta.descripcion  tipo_articulo, 
                                SUM(vd.cantidad) cantidad , SUM(vd.precio_unitario * vd.cantidad) importe
                                FROM venta_detalle vd
                                INNER JOIN venta v ON v.idventa = vd.idventa
                                INNER JOIN articulo a ON a.idarticulo = vd.idarticulo
                                INNER JOIN marca m ON m.idmarca = a.idmarca
                                INNER JOIN tipo_articulo ta ON ta.idtipo_articulo = a.idtipoarticulo
                                WHERE v.fecha_baja IS NULL 
                                AND vd.fecha_baja IS NULL
                                AND v.idsucursal =:p4
                                AND v.fecha BETWEEN :p1 AND :p2
                                GROUP BY tipo_articulo";
                DataTable dt = conn.GetDT(sql, idSucursal, fechaDesde, fechaHasta);
                if (dt == null || dt.Rows.Count == 0)
                {
                    throw new ExcepcionPropia("No se han encontrado Datos");
                }
                else
                {
                    return dt;
                }
            }
            catch (Npgsql.NpgsqlException ex)
            {
                ControladorExcepcion.tiraExcepcion(ex.Message);
                return null;
            }
        }
        public DataTable BuscarVentasTotalesPorFormaPago(DateTime fechaDesde, DateTime fechaHasta, int idSucursal)
        {
            try
            {
                string sql = @"SELECT SUM(fpv.monto) importe, tfp.descripcion forma_pago FROM venta v 
                                INNER JOIN forma_pago_venta fpv ON fpv.idventa = v.idventa
                                INNER JOIN tipo_forma_pago tfp ON tfp.idtipo_forma_pago = fpv.idforma_pago
                                WHERE v.fecha BETWEEN :p1 AND :p2
                                AND v.idsucursal=:p3
                                GROUP BY forma_pago";
                DataTable dt = conn.GetDT(sql, fechaDesde, fechaHasta, idSucursal);
                if (dt == null || dt.Rows.Count == 0)
                {
                    throw new ExcepcionPropia("No se han encontrado Datos");
                }
                else
                {
                    return dt;
                }
            }
            catch (Npgsql.NpgsqlException ex)
            {
                ControladorExcepcion.tiraExcepcion(ex.Message);
                return null;
            }
        }

        public DataTable BuscarArticulosConPocoStock(int stockMenorA, int cantidadArtciulosAMostrar, int idSucursal)
        {
            try
            {
                string sql = @"SELECT ta.descripcion ||' '||m.descripcion||' ' ||a.descripcion articulo,a.codigo, as1.stock
                                FROM articulo a
                                INNER JOIN tipo_articulo ta ON ta.idtipo_articulo = a.idtipoarticulo 
                                INNER JOIN marca m ON m.idmarca = a.idmarca
                                INNER JOIN articulo_sucursal as1 ON as1.idarticulo = a.idarticulo
                                WHERE a.fecha_baja IS NULL 
                                AND a.controlar_stock = TRUE
                                AND as1.idsucursal = :p3
                                AND as1.stock <=:p1
                                ORDER BY as1.stock
                                LIMIT :p2";
                DataTable dt = conn.GetDT(sql, idSucursal, stockMenorA, cantidadArtciulosAMostrar);
                if (dt == null || dt.Rows.Count == 0)
                {
                    throw new ExcepcionPropia("No se han encontrado Datos");
                }
                else
                {
                    return dt;
                }
            }
            catch (Npgsql.NpgsqlException ex)
            {
                ControladorExcepcion.tiraExcepcion(ex.Message);
                return null;
            }
        }

        public DataTable BuscarTiposArticulosConPocoStock(int stockMenorA, int cantidadArtciulosAMostrar, int idSucursal)
        {
            try
            {
                string sql = @"SELECT 
	                            ta.descripcion TipoArticulo,	
	                            Sum(as1.stock) stock
                                FROM articulo a
                                INNER JOIN tipo_articulo ta ON ta.idtipo_articulo = a.idtipoarticulo 
                                INNER JOIN marca m ON m.idmarca = a.idmarca
                                INNER JOIN articulo_sucursal as1 ON as1.idarticulo = a.idarticulo
                                WHERE a.fecha_baja IS NULL 
                                AND a.controlar_stock = TRUE
                                AND as1.idsucursal = :p3                                
                                GROUP BY ta.descripcion
                                HAVING sum(as1.stock)<= :p1
                                ORDER BY
	                                stock
                                LIMIT :p2";
                DataTable dt = conn.GetDT(sql, idSucursal, stockMenorA, cantidadArtciulosAMostrar);
                if (dt == null || dt.Rows.Count == 0)
                {
                    throw new ExcepcionPropia("No se han encontrado Datos");
                }
                else
                {
                    return dt;
                }
            }
            catch (Npgsql.NpgsqlException ex)
            {
                ControladorExcepcion.tiraExcepcion(ex.Message);
                return null;
            }
        }

        public DataTable BuscarArticulosConMuchoStock(int stockMayorA, int cantidadArtciulosAMostrar, int idSucursal)
        {
            try
            {
                string sql = @"SELECT ta.descripcion ||' '||m.descripcion||' ' ||a.descripcion articulo,a.codigo, as1.stock
                                FROM articulo a
                                INNER JOIN tipo_articulo ta ON ta.idtipo_articulo = a.idtipoarticulo 
                                INNER JOIN marca m ON m.idmarca = a.idmarca
                                INNER JOIN articulo_sucursal as1 ON as1.idarticulo = a.idarticulo
                                WHERE a.fecha_baja IS NULL 
                                AND as1.idsucursal =:p3
                                AND a.controlar_stock = TRUE
                                AND as1.stock >:p1
                                ORDER BY as1.stock
                                LIMIT :p2";
                DataTable dt = conn.GetDT(sql, idSucursal, stockMayorA, cantidadArtciulosAMostrar);
                if (dt == null || dt.Rows.Count == 0)
                {
                    throw new ExcepcionPropia("No se han encontrado Datos");
                }
                else
                {
                    return dt;
                }
            }
            catch (Npgsql.NpgsqlException ex)
            {
                ControladorExcepcion.tiraExcepcion(ex.Message);
                return null;
            }
        }


        public DataTable BuscarTiposArticulosConMuchoStock(int stockMayorA, int cantidadArtciulosAMostrar, int idSucursal)
        {
            try
            {
                string sql = @"SELECT
	                                ta.descripcion                   TipoArticulo,
	                                SUM (as1.stock)                  stock
                                FROM
	                                articulo a
	                                INNER JOIN tipo_articulo ta
		                                ON   ta.idtipo_articulo = a.idtipoarticulo
	                                INNER JOIN marca m
		                                ON   m.idmarca = a.idmarca
	                                INNER JOIN articulo_sucursal  as1
		                                ON   as1.idarticulo = a.idarticulo
                                WHERE
	                                a.fecha_baja IS NULL
	                                AND as1.idsucursal = :p3
	                                AND a.controlar_stock = TRUE
                                GROUP BY
	                                ta.descripcion
                                HAVING
	                                SUM (as1.stock) >:p1
                                ORDER BY
	                                stock	                            
	                                LIMIT :p2";
                DataTable dt = conn.GetDT(sql, idSucursal, stockMayorA, cantidadArtciulosAMostrar);
                if (dt == null || dt.Rows.Count == 0)
                {
                    throw new ExcepcionPropia("No se han encontrado Datos");
                }
                else
                {
                    return dt;
                }
            }
            catch (Npgsql.NpgsqlException ex)
            {
                ControladorExcepcion.tiraExcepcion(ex.Message);
                return null;
            }
        }
    }
}
