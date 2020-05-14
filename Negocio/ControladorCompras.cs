using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Datos;
using Entidades;

namespace Negocio
{
    public class ControladorCompras : ControladorGenerico
    {
        #region Constructores
        /// <summary>
        /// constructor por defecto de la clase base
        /// </summary>
        public ControladorCompras()
            : base()
        {

        }
        /// <summary>
        ///para Utilizar sin using. Libera todo el garbage Collector (Espero)
        /// No maneja la coneccion
        /// </summary>
        public ControladorCompras(bool sinConexion)
            : base(sinConexion)
        {

        }
        /// <summary>
        ///Constructor al cual le paso la coneccion para no utilizar la del Controlador actua si no otra
        /// </summary>
        public ControladorCompras(Connection c)
            : base(c)
        {

        }

        #endregion

        #region Tipos de compras
        private DataTable selectTiposCompras()
        {
            string sql = @"SELECT * FROM tipo_compra tc";
            return conn.GetDT(sql);
        }
        private DataTable selectTipoCompra(int idTC)
        {
            string sql = @"SELECT * FROM tipo_compra tc WHERE tc.idtipo_compra=:p2";
            return conn.GetDT(sql, idTC);
        }
        private int insertTipoCompra(TipoCompra tc)
        {
            string sql = @"INSERT INTO tipo_compra
                        (	                     
	                        descripcion,
	                        genera_cargo,
	                        fecha_baja
                        )
                        VALUES
                        (
	                       :descripcion,
	                        :genera_cargo,
	                        :fecha_baja
                        )  ";
            conn.Execute(sql, tc.Descripcion, tc.GeneraCargo, tc.FechaBaja);
            return conn.LastInsertedId("tipo_compra_idtipo_compra_seq");
        }
        private void updateTipoCompra(TipoCompra tc)
        {
            string sql = @"UPDATE tipo_compra
                            SET	
	                            descripcion = :p5,
	                            genera_cargo = :p4,
	                            fecha_baja = :p3
                            WHERE idtipo_compra=:p2";
            conn.Execute(sql, tc.Descripcion, tc.GeneraCargo, tc.FechaBaja, tc.IdtipoCompra);
        }
        private void deleteTipoCompra(int idTc)
        {
            string sql = @"DELETE FROM tipo_compra WHERE idtipo_compra=:p1";
            conn.GetDT(sql, idTc);
        }
        private static TipoCompra mapearTipoCompra(DataRow row)
        {
            TipoCompra tc = new TipoCompra();
            tc.Descripcion = row["descripcion"].ToString();
            tc.FechaBaja = row["fecha_baja"] as DateTime?;
            tc.GeneraCargo = Convert.ToBoolean(row["genera_cargo"]);
            tc.IdtipoCompra = Convert.ToInt32(row["idtipo_compra"]);
            return tc;
        }
        /// <summary>
        /// Obtiene una lista de tipos de compra sino devuelve excepcion
        /// </summary>
        /// <returns></returns>
        public List<TipoCompra> BuscarListTipoCompras()
        {
            BeginTransaction();
            try
            {
                DataTable dt = selectTiposCompras();
                if (dt == null || dt.Rows.Count == 0)
                {
                    throw new ExcepcionPropia("No se han encontrado Tipos de Compras");
                }
                List<TipoCompra> listTc = new List<TipoCompra>();
                foreach (DataRow row in dt.Rows)
                {
                    TipoCompra tc = mapearTipoCompra(row);
                    listTc.Add(tc);
                }
                CommitTransaction();
                return listTc.OrderBy(ta => ta.Descripcion).ToList();
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
        /// Obtiene un tipo compra sino excepcion propia
        /// </summary>
        /// <param name="idMarca"></param>
        /// <returns></returns>
        public TipoCompra BuscarTipoCompra(int idTipoCompra)
        {
            BeginTransaction();
            try
            {
                TipoCompra tc = buscarTipoCompra(idTipoCompra);
                CommitTransaction();
                return tc;
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

        private TipoCompra buscarTipoCompra(int idTipoCompra)
        {
            DataTable dt = selectTipoCompra(idTipoCompra);
            if (dt == null || dt.Rows.Count == 0)
            {
                throw new ExcepcionPropia("No se ha encontrado el tipo de compra");
            }
            DataRow row = dt.Rows[0];
            TipoCompra tc = mapearTipoCompra(row);
            return tc;
        }
        /// <summary>
        /// Modifica un tipo de compra
        /// </summary>
        /// <param name="idMarca"></param>
        /// <param name="descripcion"></param>
        /// <param name="fechaBaja"></param>
        public void ModificarTipoCompra(int idTipoCompra, string descripcion, bool generaCargo, DateTime? fechaBaja)
        {
            BeginTransaction();
            try
            {
                TipoCompra tc = new TipoCompra();
                tc.Descripcion = descripcion;
                tc.FechaBaja = fechaBaja;
                tc.GeneraCargo = generaCargo;
                tc.IdtipoCompra = idTipoCompra;
                updateTipoCompra(tc);
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
        public void EliminarTipoCompra(int idTipoCompra)
        {
            BeginTransaction();
            try
            {
                deleteTipoCompra(idTipoCompra);
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
        public int AgregarTipoCompra(string descripcion, bool generaCargo, DateTime? fechaBaja)
        {
            BeginTransaction();
            try
            {
                TipoCompra tc = new TipoCompra();
                tc.Descripcion = descripcion;
                tc.FechaBaja = fechaBaja;
                tc.GeneraCargo = generaCargo;

                int id = insertTipoCompra(tc);
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
        #endregion

        #region Proveedor
        private DataTable selectProveedores(string descParcial)
        {
            string sql = "SELECT * FROM proveedor p WHERE UPPER(p.nombre) LIKE UPPER('%'||:p1||'%')";
            return conn.GetDT(sql, descParcial);
        }
        private DataTable selectProveedores()
        {
            string sql = @"SELECT * FROM proveedor p";
            return conn.GetDT(sql);
        }
        private DataTable selectProveedor(int idProv)
        {
            string sql = @"SELECT * FROM proveedor p WHERE p.idproveedor=:p1";
            return conn.GetDT(sql, idProv);
        }
        private int insertProveedor(Proveedor p)
        {
            string sql = @"INSERT INTO proveedor
                        (
	                        nombre,
	                        direccion,
	                        telefonos,
	                        observaciones,
	                        fecha_baja,
                            mail
                        )
                        VALUES
                        (
	                        :nombre,
	                        :direccion,
	                        :telefonos,
	                        :observaciones,
	                        :fecha_baja,
                            :mail
                        )";
            conn.Execute(sql, p.Nombre, p.Direccion, p.Telefonos, p.Observaciones, p.FechaBaja, p.Mail);
            return conn.LastInsertedId("proveedor_idproveedor_seq");
        }
        private void updateProveedor(Proveedor p)
        {
            string sql = @"UPDATE proveedor
                            SET	
	                            nombre = :p7,
	                            direccion = :p6,
	                            telefonos = :p5,
	                            observaciones = :p4,
	                            fecha_baja = :p3,
                                mail=:p9
                            WHERE idproveedor=:p2";
            conn.Execute(sql, p.Nombre, p.Direccion, p.Telefonos, p.Observaciones, p.FechaBaja, p.Mail, p.Idproveedor);
        }
        private void deleteProveedor(int idProv)
        {
            string sql = @"DELETE FROM proveedor WHERE idproveedor=:p1";
            conn.GetDT(sql, idProv);
        }
        private static Proveedor mapearProveedor(DataRow row)
        {
            Proveedor p = new Proveedor();
            p.Direccion = row["direccion"].ToString();
            p.FechaBaja = row["fecha_baja"] as DateTime?;
            p.Idproveedor = Convert.ToInt32(row["idproveedor"]);
            p.Nombre = row["nombre"].ToString();
            p.Observaciones = row["observaciones"].ToString();
            p.Telefonos = row["telefonos"].ToString();
            p.Mail = row["mail"].ToString();
            return p;
        }
        /// <summary>
        /// Obtiene una lista de proveedores sino devuelve excepcion
        /// </summary>
        /// <returns></returns>
        public List<Proveedor> BuscarListProveedores()
        {
            BeginTransaction();
            try
            {
                DataTable dt = selectProveedores();
                if (dt == null || dt.Rows.Count == 0)
                {
                    throw new ExcepcionPropia("No se han encontrado Proveedores");
                }
                List<Proveedor> listP = new List<Proveedor>();
                foreach (DataRow row in dt.Rows)
                {
                    Proveedor p = mapearProveedor(row);
                    listP.Add(p);
                }
                CommitTransaction();
                return listP.OrderBy(p => p.Nombre).ToList();
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
        /// Obtiene un Proveeodr sino excepcion propia
        /// </summary>
        /// <param name="idMarca"></param>
        /// <returns></returns>
        public Proveedor BuscarProveedor(int idProv)
        {
            BeginTransaction();
            try
            {
                Proveedor p = buscarProveedor(idProv);
                CommitTransaction();
                return p;
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

        private Proveedor buscarProveedor(int idProv)
        {
            DataTable dt = selectProveedor(idProv);
            if (dt == null || dt.Rows.Count == 0)
            {
                throw new ExcepcionPropia("No se ha encontrado el Proveedor");
            }
            DataRow row = dt.Rows[0];
            Proveedor p = mapearProveedor(row);
            return p;
        }
        /// <summary>
        /// Modifica un Proveedor
        /// </summary>
        /// <param name="idMarca"></param>
        /// <param name="descripcion"></param>
        /// <param name="fechaBaja"></param>
        public void ModificarProveedor(int idProv, string nombre, string direccion, string telefonos, string mail,
            string observaciones, DateTime? fechaBaja)
        {
            BeginTransaction();
            try
            {
                Proveedor p = new Proveedor();
                p.Direccion = direccion;
                p.FechaBaja = fechaBaja;
                p.Mail = mail;
                p.Idproveedor = idProv;
                p.Nombre = nombre;
                p.Observaciones = observaciones;
                p.Telefonos = telefonos;
                updateProveedor(p);
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
        public void EliminarProveedor(int idProv)
        {
            BeginTransaction();
            try
            {
                deleteProveedor(idProv);
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
        public int AgregarProveedor(string nombre, string direccion, string telefonos, string mail,
            string observaciones, DateTime? fechaBaja)
        {
            BeginTransaction();
            try
            {
                Proveedor p = new Proveedor();
                p.Direccion = direccion;
                p.Mail = mail;
                p.FechaBaja = fechaBaja;
                p.Nombre = nombre;
                p.Observaciones = observaciones;
                p.Telefonos = telefonos;
                int id = insertProveedor(p);
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
        /// <summary>
        /// Obtiene una lista de prov para la desc parcial, sino devuelve null
        /// </summary>
        /// <param name="descParcial"></param>
        /// <returns></returns>
        public List<Proveedor> BuscarListProveedores(string descParcial)
        {
            BeginTransaction();
            try
            {
                DataTable dt = selectProveedores(descParcial);
                if (dt != null && dt.Rows.Count > 0)
                {
                    List<Proveedor> listP = new List<Proveedor>();
                    foreach (DataRow row in dt.Rows)
                    {
                        Proveedor p = mapearProveedor(row);
                        listP.Add(p);
                    }
                    CommitTransaction();
                    return listP;
                }
                else
                {
                    CommitTransaction();
                    return null;
                }
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
        #endregion

        #region Compras
        private int insertCompra(Compra c)
        {
            string sql = @"INSERT INTO compra
                            (
	                            fecha,
	                            total,
	                            descripcion,
	                            idproveedor,
	                            idusuario,
	                            idtipo_compra,
                                idcomprobante,
                                idsucursal,
                                idcaja
	
                            )
                            VALUES
                            (
                                :p1,
                                :p2,
                                :p3,
                                :p4,
                                :p5,
                                :p6,
                                :p7,
                                :p8,
                                :p9                        
                            )";
            conn.Execute(sql, c.Fecha, c.Total, c.Descripcion, c.Idproveedor, c.Idusuario, c.IdtipoCompra, c.IdComprobante, c.IdSucursal, c.IdCaja);
            return conn.LastInsertedId("compra_idcompra_seq");
        }
        private void insertCompraLinea(CompraLinea lc)
        {
            string sql = @"INSERT INTO compra_detalle
                            (
	                            idcompra,
	                            idarticulo,
	                            costo_unitario,
	                            cantidad
	
                            )
                            VALUES
                            (
                            :p1,
                            :p2,
                            :p3,
                            :p4
                            )";
            conn.Execute(sql, lc.Idcompra, lc.Idarticulo, lc.CostoUnitario, lc.Cantidad);
        }
        private void insertFormaPagoCompra(int idCompra, int idFormaPago, decimal monto)
        {
            string sql = @"INSERT INTO forma_pago_compra
                        (
	                        idcompra,
	                        idtipo_forma_pago,
	                        monto
                        )
                        VALUES
                        (
	                        :p1idcompra,
	                        :idtipo_forma_pago,
	                        :monto
                        )
                        ";
            conn.Execute(sql, idCompra, idFormaPago, monto);
        }
        /// <summary>
        /// Si el comprobate es distinto de nulo lo agrega, devuelve el id y le asigna el id al comprobante
        /// Si es null devuelve null y no hjace nada
        /// </summary>
        /// <param name="cc"></param>
        /// <returns></returns>
        private int? insertComprobanteCompra(ComprobanteCompra cc)
        {
            if (cc != null)
            {
                string sql = @"INSERT INTO comprobante_compra
                            (	                            
	                            numero,
	                            idproveedor,
	                            fecha,
	                            monto,
	                            tipo_comprobante
                            )
                            VALUES
                            (
	                             :numero,
	                            :idproveedor,
	                            :fecha,
	                            :monto,
	                            :tipo_comprobante
                            )";
                conn.Execute(sql, cc.Numero, cc.Idproveedor, cc.Fecha, cc.Monto, cc.TipoComprobante);
                int id = conn.LastInsertedId("comprobante_compra_idcomprobante_seq");
                cc.Idcomprobante = id;
                return id;

            }
            else
            {
                return null;
            }

        }

        private DataTable selectCompra(int idCompra)
        {
            string sql = "SELECT * FROM compra c WHERE c.idcompra=:p1";
            return conn.GetDT(sql, idCompra);
        }
        private DataTable selectCompras(DateTime fechaDesde, DateTime fechaHasta)
        {
            string sql = "SELECT * FROM compra c WHERE c.fecha BETWEEN :p1 AND :p2";
            return conn.GetDT(sql, fechaDesde, fechaHasta);
        }
        private DataTable selectCompras(DateTime fechaDesde, DateTime fechaHasta, int idSucursal)
        {
            string sql = "SELECT * FROM compra c WHERE c.fecha BETWEEN :p1 AND :p2 AND c.idsucursal=:p3";
            return conn.GetDT(sql, fechaDesde, fechaHasta, idSucursal);
        }
        private DataTable selectCompraDetalles(int idCompra)
        {
            string sql = "SELECT * FROM compra_detalle cd WHERE cd.idcompra=:p1";
            return conn.GetDT(sql, idCompra);
        }
        private DataTable selectFormaPagoCompra(int idCompra)
        {
            string sql = "SELECT * FROM forma_pago_compra fpc WHERE fpc.idcompra=:p1";
            return conn.GetDT(sql, idCompra);
        }

        /// <summary>
        /// Indica la baja de una linea de compra
        /// </summary>
        /// <param name="idCompra"></param>
        /// <param name="idArt"></param>
        /// <param name="idUsuario"></param>
        /// <param name="fechaBaja"></param>
        private void updateCompraDetalle(int idCompra, int idArt, int idUsuario, DateTime fechaBaja)
        {
            string sql = @"UPDATE compra_detalle
                        SET
                            idusuario_baja = :p1,
                            fecha_baja = :p2,
                            motivo_baja = :p6
                        WHERE idcompra = :p3 AND idarticulo=:p4";
            conn.Execute(sql, idUsuario, fechaBaja, "Error", idCompra, idArt);

        }
        private void updateCompraTotal(int idCompra, decimal importeARestar)
        {
            string sql = @"UPDATE compra
                            SET
                                total = total-:p1
                            WHERE idcompra=:p2";
            conn.Execute(sql, importeARestar, idCompra);
        }
        private void updateCompra(Compra c)
        {
            string sql = @"UPDATE compra
                            SET
	
	                            fecha = :p1,
	                            total = :p2,
	                            descripcion = :p3,
	                            idproveedor = :p4,
	                            idusuario = :p5,
	                            idtipo_compra = :p6,
	                            idcomprobante = :p7                                
                            WHERE idcompra = :p8";
            conn.Execute(sql, c.Fecha, c.Total, c.Descripcion, c.Idproveedor, c.Idusuario, c.IdtipoCompra, c.IdComprobante, c.Idcompra);
        }

        /// <summary>
        /// Elimina todo el detalle de compra
        /// </summary>
        /// <param name="idCompra"></param>
        private void deleteCompraDetalles(int idCompra)
        {
            string sql = "DELETE FROM compra_detalle WHERE  idcompra=:p1";
            conn.Execute(sql, idCompra);
        }
        /// <summary>
        /// Elimina toda la forma de pogo de una compra
        /// </summary>
        /// <param name="idCompra"></param>
        private void deleteFormaPagoCompra(int idCompra)
        {
            string sql = "DELETE FROM forma_pago_compra WHERE idcompra=:p2";
            conn.Execute(sql, idCompra);
        }
        private void deleteComprobante(int? idComprobante)
        {
            if (idComprobante != null)
            {
                string sql = "DELETE FROM comprobante_compra WHERE idcomprobante=:p2";
                conn.Execute(sql, idComprobante);
            }

        }

        /// <summary>
        /// Agrega una nueva compra
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public int AgregarCompra(Compra c)
        {
            BeginTransaction();
            try
            {
                agregarCompra(c);

                CommitTransaction();
                return c.Idcompra;
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

        private void agregarCompra(Compra c)
        {
            validarCompra(c);

            insertComprobanteCompra(c.Comprobante);
            c.Idcompra = insertCompra(c);

            agregarLineasCompra(c);

            if (c.ListFormaPago != null)
            {
                foreach (FormaPago fp in c.ListFormaPago)
                {
                    insertFormaPagoCompra(c.Idcompra, fp.IdtipoFormaPago, fp.Monto);
                }
            }
        }

        private void agregarLineasCompra(Compra c)
        {
            ControladorArticulos c_art = new ControladorArticulos(conn);
            foreach (CompraLinea lc in c.ListLineaCompra)
            {
                if (lc.Idarticulo == 0)
                {
                    lc.Articulo.Idarticulo = c_art.AgregarArticulo(lc.Articulo, c.IdSucursal);
                }
                else
                {
                    c_art.ModificarArticuloSucursal(lc.Articulo.CostoUltimo, lc.Articulo.Precio, lc.Articulo.Idarticulo, c.IdSucursal);
                }
                if (lc.Articulo.ControlarStock)
                {
                    c_art.ActualizarStockArticulo(lc.Articulo.Idarticulo, lc.Cantidad, c.IdSucursal);
                }
                lc.Idcompra = c.Idcompra;
                insertCompraLinea(lc);

            }
        }

        private static void validarCompra(Compra c)
        {

            if (c.ListFormaPago != null && c.ListFormaPago.Count > 0 && !c.TipoCompra.GeneraCargo)
            {
                throw new ExcepcionPropia("El tipo de compra actual no genera cargo, no debe tener formas de pago");
            }
            else if ((c.ListFormaPago == null || c.ListFormaPago.Count == 0 )&& c.TipoCompra.GeneraCargo)
            {
                throw new ExcepcionPropia("El tipo de compra actual  genera cargo, debe indicar al menos una forma de pago");
            }
            if (c.ListFormaPago != null && c.ListFormaPago.Sum(l => l.Monto) != c.Total)
            {
                throw new ExcepcionPropia("El total de la compra debe ser igual a la suma de las formas de pago");
            }
        }
        /// <summary>
        /// Busca una lista de compra pra las fechas indicadas. Si no encuentra nada devuelve Excepcion propia
        /// Si se indica la sucrusal trae las de la sucursal si no trae todas
        /// </summary>
        /// <param name="fechaDesde"></param>
        /// <param name="fechaHasta"></param>
        /// <returns></returns>
        public List<Compra> BuscarListCompras(DateTime fechaDesde, DateTime fechaHasta, int? idSucursal)
        {
            try
            {
                if (idSucursal == null)
                {
                    DataTable dt = selectCompras(fechaDesde, fechaHasta);
                    if (dt == null || dt.Rows.Count == 0)
                    {
                        throw new ExcepcionPropia("No se han encontrado compras");
                    }
                    List<Compra> listC = new List<Compra>();
                    foreach (DataRow row in dt.Rows)
                    {
                        Compra c = mapearCompra(row);
                        listC.Add(c);
                    }
                    return listC;
                }
                else
                {
                    DataTable dt = selectCompras(fechaDesde, fechaHasta, (int)idSucursal);
                    if (dt == null || dt.Rows.Count == 0)
                    {
                        throw new ExcepcionPropia("No se han encontrado compras");
                    }
                    List<Compra> listC = new List<Compra>();
                    foreach (DataRow row in dt.Rows)
                    {
                        Compra c = mapearCompra(row);
                        listC.Add(c);
                    }
                    return listC;
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
        /// Obtiene una compra completa con todas sus lineas.
        /// Si no encuentra tira excepcion
        /// </summary>
        /// <param name="idCompra"></param>
        /// <returns></returns>
        public Compra BuscarCompra(int idCompra)
        {

            try
            {
                Compra c = buscarCompra(idCompra);

                return c;
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
        /// Busca una compra si no la encuentra devuelve exceocion propia
        /// </summary>
        /// <param name="idCompra"></param>
        /// <returns></returns>
        private Compra buscarCompra(int idCompra)
        {
            DataTable dt = selectCompra(idCompra);
            if (dt == null || dt.Rows.Count == 0)
            {
                throw new ExcepcionPropia("No se ha encontrado la compra");
            }
            DataRow row = dt.Rows[0];
            Compra c = mapearCompra(row);
            return c;
        }
        /// <summary>
        /// Indica la baja de una linea de compra.
        /// Se fija si el articulo lleva control de stock, resta lo necesario,
        /// si la compra es de tipo q genera cargo resta el total
        /// </summary>
        /// <param name="idCompra"></param>
        /// <param name="idArt"></param>
        /// <param name="idUsuario"></param>
        public void BajaLineaCompra(int idCompra, int idArt, int idUsuario)
        {
            BeginTransaction();
            try
            {
                Compra c = buscarCompra(idCompra);
                if (c.TipoCompra.GeneraCargo)
                {
                    throw new ExcepcionPropia("La compra genera cargo. Debe modificar tambien las formas de pago. La suma de forma de pago debe ser igual al total de la compra");
                    //decimal subtotal = c.ListLineaCompra.FirstOrDefault(lc=>lc.Idarticulo==idArt).Subtotal;
                    //updateCompraTotal(idCompra,subtotal);
                }
                if (c.ListLineaCompra.FirstOrDefault(lc => lc.Idarticulo == idArt).Articulo.ControlarStock)
                {
                    int cant = c.ListLineaCompra.FirstOrDefault(lc => lc.Idarticulo == idArt).Cantidad;
                    ControladorArticulos c_art = new ControladorArticulos(conn);
                    c_art.ActualizarStockArticulo(idArt, -cant, c.IdSucursal);
                }
                updateCompraDetalle(idCompra, idArt, idUsuario, DateTime.Now);
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
        }

        /// <summary>
        /// Indica la baja de una linea de compra.
        /// Se fija si el articulo lleva control de stock, resta lo necesario,
        /// si la compra es de tipo q genera cargo resta el total
        /// </summary>
        /// <param name="idCompra"></param>
        /// <param name="idArt"></param>
        /// <param name="idUsuario"></param>
        public void BajaLineaCompra(int idCompra, int idArt, int idUsuario, List<FormaPago> listFormaPago)
        {

            BeginTransaction();
            try
            {
                Compra c = buscarCompra(idCompra);
                if (c.TipoCompra.GeneraCargo)
                {
                    decimal subtotal = c.ListLineaCompra.FirstOrDefault(lc => lc.Idarticulo == idArt).Subtotal;
                    if (c.ListFormaPago == null || c.ListFormaPago.Count == 0)
                    {
                        throw new ExcepcionPropia("Debe indicar al menos una forma de pago");
                    }
                    if (c.ListFormaPago.Sum(l => l.Monto) != (c.Total - subtotal))
                    {
                        throw new ExcepcionPropia("El total de la compra debe ser igual a la suma de las formas de pago");
                    }
                    foreach (FormaPago fp in c.ListFormaPago)
                    {

                    }
                    updateCompraTotal(idCompra, subtotal);
                }
                if (c.ListLineaCompra.FirstOrDefault(lc => lc.Idarticulo == idArt).Articulo.ControlarStock)
                {
                    int cant = c.ListLineaCompra.FirstOrDefault(lc => lc.Idarticulo == idArt).Cantidad;
                    ControladorArticulos c_art = new ControladorArticulos(conn);
                    c_art.ActualizarStockArticulo(idArt, -cant, c.IdSucursal);
                }
                updateCompraDetalle(idCompra, idArt, idUsuario, DateTime.Now);
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
        }
        /// <summary>
        /// Mapea una compra pero le completa toooodas sus propiedades
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private Compra mapearCompra(DataRow row)
        {
            Compra c = new Compra();
            c.Descripcion = row["descripcion"].ToString();
            c.Fecha = Convert.ToDateTime(row["fecha"]);
            c.Idcompra = Convert.ToInt32(row["idcompra"]);
            c.Idusuario = Convert.ToInt32(row["idusuario"]);
            ControladorUsuarios c_usu = new ControladorUsuarios(conn);
            c.Usuario = c_usu.BuscarUsuario(c.Idusuario);
            c.Total = Convert.ToDecimal(row["total"]);

            ControladorSucursal c_suc = new ControladorSucursal(conn);
            c.Sucursal_ = c_suc.BuscarSucursal(Convert.ToInt32(row["idsucursal"]));

            c.Proveedor = buscarProveedor(Convert.ToInt32(row["idproveedor"]));
            c.TipoCompra = buscarTipoCompra(Convert.ToInt32(row["idtipo_compra"]));
            c.ListFormaPago = buscarListFormaPagoCompra(c.Idcompra);
            c.ListLineaCompra = buscarListLineasCompras(c.Idcompra, c.IdSucursal);
            c.Comprobante = buscarComprobanteCompra(row["idcomprobante"] as int?);
            c.IdCaja = Convert.ToInt32(row["idcaja"]);
            return c;
        }
        /// <summary>
        /// Obtiene las forma de pago de la compra. Si no encuentra nada devuelve null
        /// </summary>
        /// <param name="idCompra"></param>
        /// <returns></returns>
        private List<FormaPago> buscarListFormaPagoCompra(int idCompra)
        {
            DataTable dt = selectFormaPagoCompra(idCompra);
            ControladorFormaPago c_formaPago = new ControladorFormaPago(conn);
            if (dt != null && dt.Rows.Count > 0)
            {
                List<FormaPago> listFp = new List<FormaPago>();
                foreach (DataRow row in dt.Rows)
                {
                    FormaPago fp = c_formaPago.BuscarFormaPago(Convert.ToInt32(row["idtipo_forma_pago"]));
                    fp.Monto = Convert.ToDecimal(row["monto"]);
                    listFp.Add(fp);
                }
                return listFp;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Obtiene una lista de lineas de compra, si no encuentra nada devuelve null
        /// </summary>
        /// <param name="idCompra"></param>
        /// <returns></returns>
        private List<CompraLinea> buscarListLineasCompras(int idCompra, int idSucursal)
        {
            DataTable dt = selectCompraDetalles(idCompra);
            if (dt != null && dt.Rows.Count > 0)
            {
                ControladorArticulos c_articulos = new ControladorArticulos(conn);
                List<CompraLinea> listLc = new List<CompraLinea>();
                foreach (DataRow row in dt.Rows)
                {
                    CompraLinea lc = new CompraLinea();
                    lc.Articulo = c_articulos.BuscarArticulo(Convert.ToInt32(row["idarticulo"]), idSucursal);
                    lc.Cantidad = Convert.ToInt32(row["cantidad"]);
                    lc.CostoUnitario = Convert.ToDecimal(row["costo_unitario"]);
                    lc.FechaBaja = row["fecha_baja"] as DateTime?;
                    lc.IdusuarioBaja = row["idusuario_baja"] as int?;
                    lc.Idcompra = Convert.ToInt32(row["idcompra"]);
                    listLc.Add(lc);
                }
                return listLc;
            }
            else
            {
                return null;
            }

        }

        /// <summary>
        /// Modifica una compra completa.
        /// </summary>
        /// <param name="c">Compra Completa</param>
        public void ModificarCompra(Compra c)
        {
            BeginTransaction();
            try
            {
                validarCompra(c);
                ///Instancio un controlador de articulos por que lo voy a utilizar
                ControladorArticulos c_art = new ControladorArticulos(conn);
                ///Antes de modificar la compra actualizo el stock de los articulos
                ///busco la compra
                Compra compraEnDb = buscarCompra(c.Idcompra);
                //List<CompraLinea> listLineaCompra = buscarListLineasCompras(c.Idcompra);
                foreach (CompraLinea cl in compraEnDb.ListLineaCompra)
                {
                    if (cl.Articulo.ControlarStock)
                    {
                        int cant = cl.Cantidad;
                        c_art.ActualizarStockArticulo(cl.Idarticulo, -cant, c.IdSucursal);
                    }
                }

                ///Ahora elimino todo lo relacionado a lacompra para despues agregarlo nuevamente
                deleteCompraDetalles(c.Idcompra);
                deleteFormaPagoCompra(c.Idcompra);
                deleteComprobante(compraEnDb.IdComprobante);
                insertComprobanteCompra(c.Comprobante);
                updateCompra(c);

                agregarLineasCompra(c);
                if (c.ListFormaPago != null)
                {
                    foreach (FormaPago fp in c.ListFormaPago)
                    {
                        insertFormaPagoCompra(c.Idcompra, fp.IdtipoFormaPago, fp.Monto);
                    }
                }

                CommitTransaction();
            }
            catch (Npgsql.NpgsqlException myex)
            {
                RollbackTransaction();
                ControladorExcepcion.tiraExcepcion(myex);
            }
        }

        /// <summary>
        /// Otbiene un comprobante de compra si no encuentra nada devuelve excepcion propia.
        /// Si le paso un int null devuelve null
        /// </summary>
        /// <param name="idComprobante"></param>
        /// <returns></returns>
        private ComprobanteCompra buscarComprobanteCompra(int? idComprobante)
        {
            if (idComprobante == null)
            {
                return null;
            }
            else
            {
                string sql = "SELECT * FROM comprobante_compra cc WHERE cc.idcomprobante=:p2";
                DataTable dt = conn.GetDT(sql, idComprobante);
                if (dt == null || dt.Rows.Count == 0)
                {
                    throw new ExcepcionPropia("No se ha encontrado el comprobante");
                }
                DataRow row = dt.Rows[0];
                ComprobanteCompra cc = new ComprobanteCompra();
                cc.Fecha = Convert.ToDateTime(row["fecha"]);
                cc.Idcomprobante = Convert.ToInt32(row["idcomprobante"]);
                cc.Monto = Convert.ToDecimal(row["monto"]);
                cc.Numero = row["numero"].ToString();
                cc.Proveedor = buscarProveedor(Convert.ToInt32(row["idproveedor"]));
                cc.TipoComprobante = row["tipo_comprobante"].ToString();
                return cc;
            }
        }

        public void EliminarCompra(int idCompra)
        {
            BeginTransaction();
            try
            {
                Compra compraAEliminar = buscarCompra(idCompra);
                ControladorArticulos c_art = new ControladorArticulos(conn);
                //List<CompraLinea> listLineaCompra = buscarListLineasCompras(c.Idcompra);
                foreach (CompraLinea cl in compraAEliminar.ListLineaCompra)
                {
                    if (cl.Articulo.ControlarStock)
                    {
                        int cant = cl.Cantidad;
                        c_art.ActualizarStockArticulo(cl.Idarticulo, -cant, compraAEliminar.IdSucursal);
                    }
                }
                deleteComprobante(compraAEliminar.IdComprobante);

                string sql = "DELETE FROM compra WHERE idcompra=:p1";
                conn.Execute(sql, idCompra);

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

        #endregion

    }

}
