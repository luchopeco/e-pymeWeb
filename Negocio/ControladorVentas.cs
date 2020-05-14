using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Datos;
using Entidades;
using Negocio;


namespace Negocio
{
    public class ControladorVentas : ControladorGenerico
    {
        #region Constructores
        /// <summary>
        /// constructor por defecto de la clase base
        /// </summary>
        public ControladorVentas()
            : base()
        {

        }
        /// <summary>
        ///para Utilizar sin using. Libera todo el garbage Collector (Espero)
        /// No maneja la coneccion
        /// </summary>
        public ControladorVentas(bool sinConexion)
            : base(sinConexion)
        {

        }
        /// <summary>
        ///Constructor al cual le paso la coneccion para no utilizar la del Controlador actua si no otra
        /// </summary>
        public ControladorVentas(Connection c)
            : base(c)
        {

        }

        #endregion

        private DataTable selectVenta(DateTime fechaDesde, DateTime fechaHasta)
        {
            string sql = "SELECT * FROM venta v WHERE v.fecha BETWEEN :p1 AND :p2";
            return conn.GetDT(sql, fechaDesde, fechaHasta);
        }
        private DataTable selectVenta(DateTime fechaDesde, DateTime fechaHasta, int idSucursal)
        {
            string sql = "SELECT * FROM venta v WHERE v.fecha BETWEEN :p1 AND :p2 AND v.idsucursal=:p5";
            return conn.GetDT(sql, fechaDesde, fechaHasta, idSucursal);
        }
        private DataTable selectVenta(int idVenta)
        {
            string sql = "SELECT * FROM venta v WHERE v.idventa=:p2";
            return conn.GetDT(sql, idVenta);
        }
        private DataTable selectLineasVentas(int idVenta)
        {
            string sql = "SELECT * FROM venta_detalle vd WHERE vd.idventa=:p1";
            return conn.GetDT(sql, idVenta);
        }
        private DataTable selectFormasPagoVentas(int idventa)
        {
            string sql = "SELECT * FROM forma_pago_venta fpv WHERE fpv.idventa=:p2";
            return conn.GetDT(sql, idventa);
        }

        private DataTable selectNotaCreditoNumero(int numero)
        {
            string sql = "SELECT * FROM nota_credito nc WHERE nc.numero=:p1";
            return conn.GetDT(sql, numero);
        }
        private DataTable selectNotaCredito(int idNotaCredito)
        {
            string sql = "SELECT * FROM nota_credito nc WHERE nc.idnota_credito=:p";
            return conn.GetDT(sql, idNotaCredito);
        }



        /// <summary>
        /// Valida si una nota de credito esta utilizada en una venta.
        /// False si esta utilizada en venta, true si no esta utlizada
        /// </summary>
        /// <param name="nroNotaCredito"></param>
        private bool validarNotaCreditoSinUtilizar(int nroNotaCredito)
        {
            string sql = @"SELECT 1 FROM nota_credito nc INNER JOIN forma_pago_venta fpv ON fpv.idnota_credito = nc.idnota_credito
                           WHERE nc.numero=:p2";
            DataTable dt = conn.GetDT(sql, nroNotaCredito);
            if (dt != null && dt.Rows.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        private void insertVentaLinea(VentaLinea lv)
        {
            string sql = @"INSERT INTO venta_detalle
                        (
	                        idventa,
	                        idarticulo,
	                        precio_unitario,
	                        cantidad
                        )
                        VALUES
                        (
	                        :idventa,
	                        :idarticulo,
	                        :precio_unitario,
	                        :cantidad

                        )";
            conn.Execute(sql, lv.Idventa, lv.Idarticulo, lv.PrecioUnitario, lv.Cantidad);
        }
        private int insertVenta(Venta v)
        {
            string sql = @"INSERT INTO venta
                        (	                        
	                        descripcion,
	                        fecha,
	                        total,
	                        idcliente,
	                        idusuario,
                            idsucursal,
                            idcaja
                        )
                        VALUES
                        (
	                        :descrpcion,
	                        :fecha,
	                        :total,
	                        :idcliente,
	                        :idusuario,
                            :p1,
                            :p2
                        )";
            conn.Execute(sql, v.Descripcion, v.Fecha, v.Total, v.Idcliente, v.Idusuario, v.IdSucursal, v.IdCaja);
            return conn.LastInsertedId("venta_idventa_seq");
        }
        private void insertFormaPagoVenta(int idVenta, int idFormaPago, decimal monto, int? idNotaCredito)
        {
            string sql = @"INSERT INTO forma_pago_venta
                            (
	                            idventa,
	                            idforma_pago,
	                            monto,
                                idnota_credito
                            )
                            VALUES
                            (
	                            :idventa,
	                            :idforma_pago,
	                            :monto,
                                :p2
                            )";
            conn.Execute(sql, idVenta, idFormaPago, monto, idNotaCredito);
        }

        private int insertNotaCredito(NotaCredito nc)
        {
            string sql = @"INSERT INTO nota_credito
                        (	                     
	                        fecha,
	                        monto,
	                        descripcion,
	                        fecha_vto,
	                        idusuario
                        )
                        VALUES
                        (
	                        :fecha,
	                        :monto,
	                        :descripcion,
	                        :fecha_vto,
	                        :idusuario 
                        )";
            conn.Execute(sql, nc.Fecha, nc.Monto, nc.Descripcion, nc.FechaVto, nc.Idusuario);
            return conn.LastInsertedId("nota_credito_idnota_credito_seq");
        }

        private int insertComprobante(Comprobante c)
        {

            string sql = @"INSERT INTO comprobante
                            (	                            
	                            numero,
	                            tipo_comprobante,
	                            fecha,
	                            monto
                            )
                            VALUES
                            (
	                            :numero,
	                            :tipo_comprobante,
	                            :fecha,
	                            :monto
                            )";
            conn.Execute(sql, c.Numero, c.TipoComprobante, c.Fecha, c.Monto);
            return conn.LastInsertedId("comprobante_idcomprobante_seq");
        }

        private void updateVentaLinea(int idVenta, int idArticulo, DateTime fechaBaja, int idUsuario)
        {
            string sql = @"UPDATE venta_detalle
                            SET
	                            fecha_baja = :p4,
	                            idusuario_baja = :p3
                            WHERE idventa=:p1 AND idarticulo=:p2";
            conn.Execute(sql, fechaBaja, idUsuario, idVenta, idArticulo);
        }
        private void updateVentaLinea(int idVenta, int idArticulo, int idNotaCredito)
        {
            string sql = @"UPDATE venta_detalle
                        SET
                            idnota_credito = :p1
                        WHERE idventa =:p2 AND idarticulo=:p3";
            conn.Execute(sql, idNotaCredito, idVenta, idArticulo);
        }
        private void updateVentaTotal(int idVenta, decimal montoASumar)
        {
            string sql = @"UPDATE venta
                        SET	
	                        total = total +:p2	
                        WHERE idventa=:p1";
            conn.Execute(sql, montoASumar, idVenta);
        }

        private void updateNotaCredito(NotaCredito nc)
        {
            string sql = @"UPDATE nota_credito
                        SET	                        
	                        fecha = :p8,
	                        monto = :p7,
	                        descripcion = :p6,
	                        fecha_vto = :p5,
	                        idusuario = :p4,
	                        numero = :p3
                        WHERE idnota_credito=:p2";
            conn.Execute(sql, nc.Fecha, nc.Monto, nc.Descripcion, nc.FechaVto, nc.Idusuario, nc.Numero, nc.IdnotaCredito);
        }

        private void agregarListFormaPagoVenta(List<FormaPago> listFp)
        {
            foreach (FormaPago fp in listFp)
            {
                if (fp.NotaCredito != null)
                {

                }
            }
        }

        /// <summary>
        /// Obtiene una lista de fp para una venta, si no encuentra nada devuelve excepcion propia
        /// </summary>
        /// <param name="idVenta"></param>
        /// <returns></returns>
        private List<FormaPago> buscarListFormaPagoVenta(int idVenta)
        {
            DataTable dt = selectFormasPagoVentas(idVenta);
            if (dt == null || dt.Rows.Count == 0)
            {
                throw new ExcepcionPropia("No se han encontrado formas de pago");
            }
            ControladorFormaPago c_formaPago = new ControladorFormaPago(conn);
            List<FormaPago> listFp = new List<FormaPago>();
            foreach (DataRow row in dt.Rows)
            {
                FormaPago fp = c_formaPago.BuscarFormaPago(Convert.ToInt32(row["idforma_pago"]));
                fp.Monto = Convert.ToDecimal(row["monto"]);
                listFp.Add(fp);

            }

            return listFp;
        }
        /// <summary>
        /// Obtiene uan lista de linea de venta de una venta.
        /// Si no encuentra nada devuelve exceocion propia
        /// </summary>
        /// <param name="idVenta"></param>
        /// <param name="idArt"></param>
        /// <returns></returns>
        private List<VentaLinea> buscarListLineaVenta(int idVenta)
        {
            DataTable dt = selectLineasVentas(idVenta);
            if (dt == null || dt.Rows.Count == 0)
            {
                throw new ExcepcionPropia("No se han encontrado lineas de venta");
            }
            List<VentaLinea> listLv = new List<VentaLinea>();
            ControladorArticulos c_articulos = new ControladorArticulos(conn);
            foreach (DataRow row in dt.Rows)
            {
                VentaLinea lv = new VentaLinea();
                lv.Articulo = c_articulos.BuscarArticulo(Convert.ToInt32(row["idarticulo"]));
                lv.Cantidad = Convert.ToInt32(row["cantidad"]);
                lv.FechaBaja = row["fecha_baja"] as DateTime?;
                lv.IdusuarioBaja = row["idusuario_baja"] as int?;
                lv.Idventa = Convert.ToInt32(row["idventa"]);
                lv.PrecioUnitario = Convert.ToDecimal(row["precio_unitario"]);
                if (row["idnota_credito"] != DBNull.Value)
                {
                    lv.NotaCredito = BuscarNotaCredito(Convert.ToInt32(row["idnota_credito"]));
                }

                listLv.Add(lv);
            }
            return listLv;
        }
        /// <summary>
        /// Obtiene una venta. Si no la encuentra devuelve excepcion propia
        /// </summary>
        /// <param name="idVenta"></param>
        /// <returns></returns>
        private Venta buscarVenta(int idVenta)
        {
            DataTable dt = selectVenta(idVenta);
            if (dt == null || dt.Rows.Count == 0)
            {
                throw new ExcepcionPropia("No se ha encontrado la venta");
            }
            DataRow row = dt.Rows[0];
            Venta v = new Venta();
            v.Descripcion = row["descripcion"].ToString();
            v.Fecha = Convert.ToDateTime(row["fecha"]);
            v.Idusuario = Convert.ToInt32(row["idusuario"]);
            ControladorUsuarios c_usu = new ControladorUsuarios(conn);
            v.Usuario = c_usu.BuscarUsuario(v.Idusuario);
            v.Idventa = Convert.ToInt32(row["idventa"]);
            v.Total = Convert.ToDecimal(row["total"]);
            //v.Cliente =idcliente
            v.ListFormaPago = buscarListFormaPagoVenta(v.Idventa);
            v.ListLineaVenta = buscarListLineaVenta(idVenta);
            v.IdCaja = Convert.ToInt32(row["idcaja"]);
            ControladorSucursal c_S = new ControladorSucursal(conn);
            v.Sucursal_ = c_S.BuscarSucursal(Convert.ToInt32(row["idsucursal"]));

            return v;

        }
        /// <summary>
        /// Obtiene una venta. Si no la encuentra devuelve excepcion propia
        /// </summary>
        /// <param name="idVenta"></param>
        /// <returns></returns>
        private Venta buscarVenta(DataRow rowVenta)
        {
            int idventa = Convert.ToInt32(rowVenta["idventa"]);
            
            Venta v = new Venta();
            v.Descripcion = rowVenta["descripcion"].ToString();
            v.Fecha = Convert.ToDateTime(rowVenta["fecha"]);
            v.Idusuario = Convert.ToInt32(rowVenta["idusuario"]);
            ControladorUsuarios c_usu = new ControladorUsuarios(conn);
            v.Usuario = c_usu.BuscarUsuario(v.Idusuario);
            v.Idventa = Convert.ToInt32(rowVenta["idventa"]);
            v.Total = Convert.ToDecimal(rowVenta["total"]);
            //v.Cliente =idcliente
            v.ListFormaPago = buscarListFormaPagoVenta(v.Idventa);
            v.ListLineaVenta = buscarListLineaVenta(idventa);
            v.IdCaja = Convert.ToInt32(rowVenta["idcaja"]);
            ControladorSucursal c_S = new ControladorSucursal(conn);
            v.Sucursal_ = c_S.BuscarSucursal(Convert.ToInt32(rowVenta["idsucursal"]));

            return v;

        }

        /// <summary>
        /// Devuelve un comprobanet si no oe encuentra devuelve excepcion propia
        /// </summary>
        /// <param name="idComprobante"></param>
        /// <returns></returns>
        private Comprobante buscarComprobante(int idComprobante)
        {
            string sql = "SELECT * FROM comprobante c WHERE c.idcomprobante=:p1";
            DataTable dt = conn.GetDT(sql, idComprobante);
            if (dt == null || dt.Rows.Count == 0)
            {
                throw new ExcepcionPropia("No se ha encontrado el comprobante");
            }
            DataRow row = dt.Rows[0];
            Comprobante c = new Comprobante();
            c.Fecha = Convert.ToDateTime(row["fecha"]);
            c.Idcomprobante = Convert.ToInt32(row["idcomprobante"]);
            c.Monto = Convert.ToDecimal(row["monto"]);
            c.Numero = row["numero"].ToString();
            c.TipoComprobante = row["tipo_comprobante"].ToString();
            return c;

        }
        /// <summary>
        /// Agrega el comprobante, le paso la venta completa, para q calcule el monto
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        private void agregarComprobante(Venta v)
        {
            if (v.ComprobanteVenta != null)
            {
                decimal monto = v.ListFormaPago.Where(fp => fp.NotaCredito == null).Sum(fp => fp.Monto);
                v.ComprobanteVenta.Monto = monto;
                insertComprobante(v.ComprobanteVenta);
            }


        }

        /// <summary>
        /// Obtiene uan lista de ventas.
        /// Si no encuentra nada devuelve excepcion propia
        ///se puede o no indicar sucursal
        /// </summary>
        /// <param name="fechaDesde"></param>
        /// <param name="fechaHasta"></param>
        /// <returns></returns>
        public List<Venta> BuscarListVentas(DateTime fechaDesde, DateTime fechaHasta, int? idSucursal)
        {

            try
            {
                if (idSucursal == null)
                {
                    DataTable dt = selectVenta(fechaDesde, fechaHasta);
                    if (dt == null || dt.Rows.Count == 0)
                    {
                        throw new ExcepcionPropia("No se han encontrado ventas");
                    }
                    List<Venta> listV = new List<Venta>();
                    foreach (DataRow row in dt.Rows)
                    {
                        Venta v = buscarVenta(row);
                        listV.Add(v);
                    }
                    return listV;
                }
                else
                {
                    DataTable dt = selectVenta(fechaDesde, fechaHasta, (int)idSucursal);
                    if (dt == null || dt.Rows.Count == 0)
                    {
                        throw new ExcepcionPropia("No se han encontrado ventas");
                    }
                    List<Venta> listV = new List<Venta>();
                    foreach (DataRow row in dt.Rows)
                    {
                        Venta v = buscarVenta(row);
                        listV.Add(v);
                    }
                    return listV;
                }

            }
            catch (Npgsql.NpgsqlException ex)
            {

                ControladorExcepcion.tiraExcepcion(ex);
                return null;
            }
            catch (ExcepcionPropia myEx)
            {

                ControladorExcepcion.tiraExcepcion(myEx.Message);
                return null;
            }
        }
        /// <summary>
        /// Obtiene una venta si no la encuentra devuelve excepcion propia
        /// </summary>
        /// <param name="idVenta"></param>
        /// <returns></returns>
        public Venta BuscarVenta(int idVenta)
        {
            BeginTransaction();
            try
            {
                Venta v = buscarVenta(idVenta);
                CommitTransaction();
                return v;
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
        }

        /// <summary>
        /// Agrega una venta
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public int AgregarVenta(Venta v)
        {
            BeginTransaction();
            try
            {
                v.Idventa = insertVenta(v);
                foreach (VentaLinea lv in v.ListLineaVenta)
                {
                    lv.Idventa = v.Idventa;
                    insertVentaLinea(lv);
                    if (lv.Articulo.ControlarStock)
                    {
                        ControladorArticulos c_articulos = new ControladorArticulos(conn);
                        c_articulos.ActualizarStockArticulo(lv.Idarticulo, -lv.Cantidad, v.IdSucursal);
                    }
                }
                foreach (FormaPago fp in v.ListFormaPago)
                {
                    insertFormaPagoVenta(v.Idventa, fp.IdtipoFormaPago, fp.Monto, fp.IdNotaCredito);
                }

                ///Si tengo q generar una nota de credito porque sobra un monto               
                if (v.Total < v.ListFormaPago.Sum(fp => fp.Monto) && v.ListFormaPago.Exists(fp => fp.AceptaNotaCredito))
                {
                    NotaCredito nc;
                    nc = new NotaCredito();
                    nc.Descripcion = "Diferencia en venta";
                    nc.Fecha = DateTime.Today;
                    nc.FechaVto = DateTime.Today.AddMonths(3);
                    nc.Idusuario = v.Idusuario;
                    nc.Monto = v.ListFormaPago.Sum(fp => fp.Monto) - v.Total;
                    insertNotaCredito(nc);
                }
                agregarComprobante(v);
                CommitTransaction();
                return v.Idventa;
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

        /// <summary>
        /// Da de baja una linea. Cambia el Stock y modifica el total de la venta
        /// </summary>
        public void BajaLinea(int idVenta, int idArticulo, int idUsuario)
        {
            BeginTransaction();
            try
            {
                Venta v = buscarVenta(idVenta);
                decimal totalLinea = v.ListLineaVenta.FirstOrDefault(lv => lv.Idarticulo == idArticulo).Subtotal;
                int cant = 0;
                if (v.ListLineaVenta.FirstOrDefault(lv => lv.Idarticulo == idArticulo).Articulo.ControlarStock)
                {
                    cant = v.ListLineaVenta.FirstOrDefault(lv => lv.Idarticulo == idArticulo).Cantidad;
                }
                ///modifico la linea
                updateVentaLinea(idVenta, idArticulo, DateTime.Today, idUsuario);
                ///modifico el total de la venta
                updateVentaTotal(idVenta, -totalLinea);
                ///modifico el stock
                ControladorArticulos c_art = new ControladorArticulos(conn);
                c_art.ActualizarStockArticulo(idArticulo, cant, v.IdSucursal);
                CommitTransaction();
            }
            catch (ExcepcionPropia myex)
            {
                RollbackTransaction();
                ControladorExcepcion.tiraExcepcion(myex.Message);
            }
        }

        /// <summary>
        /// Agrega la nota de credito y actualiza las lineas de ventas correspondientes
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <param name="fechaVto"></param>
        /// <param name="descripcion"></param>
        /// <param name="monto"></param>
        /// <param name="listlv"></param>
        /// <returns></returns>
        public int AgregarNotaCredito(int idUsuario, DateTime? fechaVto, string descripcion, decimal monto, List<VentaLinea> listlv)
        {
            BeginTransaction();
            try
            {
                Venta v = buscarVenta(listlv[0].Idventa);

                NotaCredito nc = new NotaCredito();
                nc.Descripcion = descripcion;
                nc.Fecha = DateTime.Today;
                nc.FechaVto = fechaVto;
                nc.Idusuario = idUsuario;
                nc.Monto = monto;
                nc.IdnotaCredito = insertNotaCredito(nc);
                ControladorArticulos c_art = new ControladorArticulos(conn);
                foreach (VentaLinea lv in listlv)
                {
                    updateVentaLinea(lv.Idventa, lv.Idarticulo, nc.IdnotaCredito);
                    c_art.ActualizarStockArticulo(lv.Idarticulo, lv.Cantidad, v.IdSucursal);
                }
                CommitTransaction();
                return nc.IdnotaCredito;
            }
            catch (Npgsql.NpgsqlException ex)
            {
                RollbackTransaction();
                ControladorExcepcion.tiraExcepcion(ex);
                return 0;
            }
        }

        /// <summary>
        /// Busca una nota de credito si no la encuentra devulve eccepcion propia.
        /// Te indica si esta utilizada en venta o no
        /// No trae las lineas de ventas q contiene
        /// </summary>
        /// <param name="idNotaCredito"></param>
        /// <returns></returns>
        public NotaCredito BuscarNotaCredito(int idNotaCredito)
        {
            try
            {
                DataTable dt = selectNotaCredito(idNotaCredito);
                if (dt == null || dt.Rows.Count == 0)
                {
                    throw new ExcepcionPropia("La nota de credito no existe");
                }
                DataRow row = dt.Rows[0];
                NotaCredito nc = new NotaCredito();
                nc.Descripcion = row["descripcion"].ToString();
                nc.Fecha = Convert.ToDateTime(row["fecha"]);
                nc.FechaVto = row["fecha_vto"] as DateTime?;
                nc.IdnotaCredito = Convert.ToInt32(row["idnota_credito"]);
                nc.Idusuario = Convert.ToInt32(row["idusuario"]);
                nc.Monto = Convert.ToDecimal(row["monto"]);
                nc.Numero = Convert.ToInt32(row["numero"]);
                nc.UtilizadaEnVenta = !validarNotaCreditoSinUtilizar(nc.Numero);
                return nc;
            }
            catch (Npgsql.NpgsqlException ex)
            {
                ControladorExcepcion.tiraExcepcion(ex);
                return null;
            }
        }
        /// <summary>
        /// Busca una nota de credito si no la encuentra devulve eccepcion propia.
        /// Te indica si esta utilizada en venta o no
        /// No trae las lineas de ventas q contiene
        /// </summary>
        /// <param name="nroNotaCredito"></param>
        /// <returns></returns>
        public NotaCredito BuscarNotaCreditoXNumero(int nroNotaCredito)
        {
            try
            {
                string sql = @"SELECT nc.idnota_credito FROM nota_credito nc WHERE nc.numero=:p1";
                DataTable dt = conn.GetDT(sql, nroNotaCredito);
                if (dt == null || dt.Rows.Count == 0)
                {
                    throw new ExcepcionPropia("No se ha encontrado la nota de credito");
                }
                return BuscarNotaCredito(Convert.ToInt32(dt.Rows[0]["idnota_credito"]));
            }
            catch (Npgsql.NpgsqlException myex)
            {
                ControladorExcepcion.tiraExcepcion(myex);
                return null;
            }
        }
        /// <summary>
        /// Devuelve una lista de notas de creditos, Si no encuentra nada devuelve excepcion propia
        /// </summary>
        /// <param name="fechaDesde"></param>
        /// <param name="fechaHasta"></param>
        /// <returns></returns>
        public List<NotaCredito> BuscarListNotaCredito(DateTime fechaDesde, DateTime fechaHasta)
        {
            try
            {
                string sql = @"SELECT nc.idnota_credito  FROM nota_credito nc WHERE nc.fecha BETWEEN :p1 AND :p2 ORDER BY nc.numero DESC";
                DataTable dt = conn.GetDT(sql, fechaDesde, fechaHasta);
                if (dt == null || dt.Rows.Count == 0)
                {
                    throw new ExcepcionPropia("No se han encontrado notas de credito para las fechas indicadas");
                }
                List<NotaCredito> listNc = new List<NotaCredito>();
                foreach (DataRow row in dt.Rows)
                {
                    NotaCredito nc = BuscarNotaCredito(Convert.ToInt32(row["idnota_credito"]));
                    listNc.Add(nc);
                }
                return listNc;
            }
            catch (Npgsql.NpgsqlException myex)
            {
                ControladorExcepcion.tiraExcepcion(myex.Message);
                return null;
            }
        }

        /// <summary>
        /// Devuelve un comprobante de venta si no encuentra nada devuelve excepcion propia
        /// </summary>
        /// <param name="idComprobante"></param>
        /// <returns></returns>
        public Comprobante BuscarComprobante(int idComprobante)
        {
            try
            {
                string sql = "SELECT * FROM comprobante c";
                DataTable dt = conn.GetDT(sql, idComprobante);
                if (dt == null || dt.Rows.Count == 0)
                {
                    throw new ExcepcionPropia("No se ha encontrado el comprobante");
                }
                DataRow row = dt.Rows[0];
                Comprobante c = new Comprobante();
                c.Fecha = Convert.ToDateTime(row["fecha"]);
                c.Idcomprobante = Convert.ToInt32(row["idcomprobante"]);
                c.Monto = Convert.ToDecimal(row["monto"]);
                c.Numero = row["numero"].ToString();
                c.TipoComprobante = row["tipo_comprobante"].ToString();
                return c;

            }
            catch (Npgsql.NpgsqlException ex)
            {
                ControladorExcepcion.tiraExcepcion(ex);
                return null;
            }
        }

        /// <summary>
        /// Realiza el cambio.
        /// 
        /// </summary>
        /// <param name="lvNueva"></param>
        /// <param name="lvCambiada"></param>
        public void RealizarCambio(VentaLinea lvNueva, VentaLineaCambio lvCambiada)
        {
            BeginTransaction();
            try
            {
                string sql = "DELETE FROM venta_detalle WHERE idventa=:p1 AND idarticulo=:p2";
                conn.Execute(sql, lvCambiada.IdVenta, lvCambiada.IdArticuloAnterior);

                string sql1 = @"INSERT INTO venta_detalle_cambio
                                (
	                                idventa,
	                                idarticulo,
	                                cantidad,
	                                idusuario,
	                                fecha_cambio,
	                                idarticulo_anterior
                                )
                                VALUES
                                (
	                                :idventa,
	                                :idarticulo,
	                                :cantidad,
	                                :idusuario,
	                                :fecha_cambio,
	                                :idarticulo_anterior
                                )";
                conn.Execute(sql1, lvCambiada.IdVenta, lvCambiada.IdArticulo, lvCambiada.Cantidad, lvCambiada.IdUsuario, lvCambiada.FechaCambio, lvCambiada.IdArticuloAnterior);

                insertVentaLinea(lvNueva);

                ///Actualizo strock articulos
                Venta v = buscarVenta(lvCambiada.IdVenta);
                ControladorArticulos c_art = new ControladorArticulos(conn);
                c_art.ActualizarStockArticulo(lvNueva.Idarticulo, -lvNueva.Cantidad, v.IdSucursal);
                c_art.ActualizarStockArticulo(lvCambiada.ArticuloAnterior.Idarticulo, lvCambiada.Cantidad, v.IdSucursal);

                CommitTransaction();
            }
            catch (Npgsql.NpgsqlException ex)
            {
                RollbackTransaction();
                ControladorExcepcion.tiraExcepcion(ex);

            }
        }

        /// <summary>
        /// Devuelve una lista de cambios si no encuentra nada devuielve excepcion propia
        /// </summary>
        /// <param name="idVenta"></param>
        /// <returns></returns>
        public List<VentaLineaCambio> BuscarListCambios(int idVenta)
        {
            try
            {
                string sql = "SELECT * FROM venta_detalle_cambio vdc WHERE vdc.idventa=:p1";
                DataTable dt = conn.GetDT(sql, idVenta);
                if (dt == null || dt.Rows.Count == 0)
                {
                    throw new ExcepcionPropia("No se han encontrado Cambios");
                }
                else
                {
                    ControladorArticulos c_art = new ControladorArticulos(conn);
                    List<VentaLineaCambio> listVlc = new List<VentaLineaCambio>();
                    foreach (DataRow row in dt.Rows)
                    {
                        VentaLineaCambio vlc = new VentaLineaCambio();
                        vlc.Articulo = c_art.BuscarArticulo(Convert.ToInt32(row["idarticulo"]));
                        vlc.ArticuloAnterior = c_art.BuscarArticulo(Convert.ToInt32(row["idarticulo_anterior"]));
                        vlc.Cantidad = Convert.ToInt32(row["cantidad"]);
                        vlc.FechaCambio = Convert.ToDateTime(row["fecha_cambio"]);
                        vlc.IdVenta = Convert.ToInt32(row["idventa"]);
                        vlc.IdUsuario = Convert.ToInt32(row["idusuario"]);
                        listVlc.Add(vlc);
                    }
                    return listVlc;
                }

            }
            catch (Npgsql.NpgsqlException ex)
            {
                ControladorExcepcion.tiraExcepcion(ex);
                return null;
            }
        }

        public void ModificarVenta(int idVenta, string descripcionVenta)
        {
            BeginTransaction();
            try
            {
                string sql = @"UPDATE venta
                                SET
                                    descripcion = :p1 
                                WHERE idventa=:p2";
                conn.Execute(sql, descripcionVenta, idVenta);
                CommitTransaction();
            }
            catch (Npgsql.NpgsqlException ex)
            {
                RollbackTransaction();
                ControladorExcepcion.tiraExcepcion(ex);
            }
        }

        public void EliminarVenta(int idVenta)
        {
            BeginTransaction();
            try
            {
                ControladorArticulos c_art = new ControladorArticulos(conn);
                Venta v = buscarVenta(idVenta);
                foreach (VentaLinea lv in v.ListLineaVenta)
                {
                    if (lv.NotaCredito != null)
                    {
                        string sql1 = "DELETE FROM nota_credito WHERE idnota_credito=:p1";
                        conn.Execute(sql1, lv.NotaCredito.IdnotaCredito);
                    }
                    else
                    {
                        c_art.ActualizarStockArticulo(lv.Idarticulo, lv.Cantidad, v.IdSucursal);
                    }

                }

                string sql = "DELETE FROM venta WHERE idventa=:p1";
                conn.Execute(sql, idVenta);



                CommitTransaction();
            }
            catch (ExcepcionPropia ex)
            {
                RollbackTransaction();
                ControladorExcepcion.tiraExcepcion(ex.Message);
            }
            catch (Npgsql.NpgsqlException ex)
            {
                RollbackTransaction();
                ControladorExcepcion.tiraExcepcion(ex.Message);
            }
        }

        public void ModificarFormasPago(List<FormaPago> listFp, int idVenta)
        {
            try
            {
                BeginTransaction();
                string sql = "DELETE FROM forma_pago_venta WHERE idventa=:p1";
                conn.Execute(sql, idVenta);
                foreach (FormaPago fp in listFp)
                {
                    insertFormaPagoVenta(idVenta, fp.IdtipoFormaPago, fp.Monto, fp.IdNotaCredito);
                }
                CommitTransaction();
            }
            catch (Exception ex)
            {
                RollbackTransaction();
                throw new Exception(ex.Message);
            }


        }
    }
}
