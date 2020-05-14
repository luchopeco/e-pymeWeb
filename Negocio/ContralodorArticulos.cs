using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Datos;
using Entidades;

namespace Negocio
{
    public class ControladorArticulos : ControladorGenerico
    {
        #region Constructores
        /// <summary>
        /// constructor por defecto de la clase base
        /// </summary>
        public ControladorArticulos()
            : base()
        {

        }
        /// <summary>
        ///para Utilizar sin using. Libera todo el garbage Collector (Espero)
        /// No maneja la coneccion
        /// </summary>
        public ControladorArticulos(bool sinConexion)
            : base(sinConexion)
        {

        }
        /// <summary>
        ///Constructor al cual le paso la coneccion para no utilizar la del Controlador actua si no otra
        /// </summary>
        public ControladorArticulos(Connection c)
            : base(c)
        {

        }

        #endregion

        #region Tipo Articulo
        private DataTable selectTipoArticulos()
        {
            string sql = @"SELECT * FROM tipo_articulo ta where fecha_baja is null";
            return conn.GetDT(sql);
        }

        private DataTable selectTipoArticulo(int idTA)
        {
            string sql = @"SELECT * FROM tipo_articulo ta WHERE ta.idtipo_articulo=:p1";
            return conn.GetDT(sql, idTA);
        }
        private int insertTipoArticulo(TipoArticulo ta)
        {
            string sql = @"INSERT INTO tipo_articulo
                            (
	                            descripcion,
	                            fecha_baja
                            )
                            VALUES
                            (
	                            :descripcion,
	                            :fecha_baja
                            )";
            conn.Execute(sql, ta.Descripcion, ta.FechaBaja);
            return conn.LastInsertedId("tipo_articulo_idtipo_articulo_seq");
        }
        private DataTable updateTipoArticulo(TipoArticulo ta)
        {
            string sql = @"UPDATE tipo_articulo
                        SET
	                        descripcion = :p1,
	                        fecha_baja = :p2
                        WHERE idtipo_articulo=:p3";
            return conn.GetDT(sql, ta.Descripcion, ta.FechaBaja, ta.IdtipoArticulo);
        }
        private void deleteTipoArticulo(int idTA)
        {
            string sql = @"DELETE FROM tipo_articulo WHERE idtipo_articulo=:p1";
            conn.Execute(sql, idTA);
        }
        private static TipoArticulo mapearTipoArticulo(DataRow row)
        {
            TipoArticulo ta = new TipoArticulo();
            ta.Descripcion = row["descripcion"].ToString();
            ta.FechaBaja = row["fecha_baja"] as DateTime?;
            ta.IdtipoArticulo = Convert.ToInt32(row["idtipo_articulo"]);
            return ta;
        }
        /// <summary>
        /// Obtiene una lista de tipos articuos sino devuelve excepcion
        /// </summary>
        /// <returns></returns>
        public List<TipoArticulo> BuscarListTipoArticulo()
        {
            BeginTransaction();
            try
            {
                DataTable dt = selectTipoArticulos();
                if (dt == null || dt.Rows.Count == 0)
                {
                    throw new ExcepcionPropia("No se han encontrado Tipos de Articulos");
                }
                List<TipoArticulo> listTA = new List<TipoArticulo>();
                foreach (DataRow row in dt.Rows)
                {
                    TipoArticulo ta = mapearTipoArticulo(row);
                    listTA.Add(ta);
                }
                CommitTransaction();
                return listTA.OrderBy(ta => ta.Descripcion).ToList();
            }
            catch (Exception myEx)
            {
                RollbackTransaction();
                ControladorExcepcion.tiraExcepcion(myEx.Message);
                return null;
            }
        }
        /// <summary>
        /// Obtiene una lista de tipos articuos sino devuelve excepcion con los dados de baja
        /// </summary>
        /// <returns></returns>
        public List<TipoArticulo> BuscarListTipoArticuloTodos()
        {
            BeginTransaction();
            try
            {
                string sql = @"SELECT * FROM tipo_articulo ta";
                DataTable dt = conn.GetDT(sql);
                if (dt == null || dt.Rows.Count == 0)
                {
                    throw new ExcepcionPropia("No se han encontrado Tipos de Articulos");
                }
                List<TipoArticulo> listTA = new List<TipoArticulo>();
                foreach (DataRow row in dt.Rows)
                {
                    TipoArticulo ta = mapearTipoArticulo(row);
                    listTA.Add(ta);
                }
                CommitTransaction();
                return listTA.OrderBy(ta => ta.Descripcion).ToList();
            }

            catch (Exception myEx)
            {         
                RollbackTransaction();       
                ControladorExcepcion.tiraExcepcion(myEx.Message);
                return null;
            }
        }

        /// <summary>
        /// Obtiene un tipo articulo sino excepcion propia
        /// </summary>
        /// <param name="idMarca"></param>
        /// <returns></returns>
        public TipoArticulo BuscarTipoArticulo(int idTipoArticulo)
        {
            BeginTransaction();
            try
            {
                TipoArticulo ta = buscarTipoArticulo(idTipoArticulo);
                CommitTransaction();
                return ta;
            }
            catch (ExcepcionPropia myEx)
            {
                RollbackTransaction();
                ControladorExcepcion.tiraExcepcion(myEx.Message);
                return null;
            }
        }

        private TipoArticulo buscarTipoArticulo(int idTipoArticulo)
        {
            int id = Convert.ToInt32(idTipoArticulo);
            DataTable dt = selectTipoArticulo(id);
            if (dt == null || dt.Rows.Count == 0)
            {
                throw new ExcepcionPropia("No se ha encontrado el tipo de articulo");
            }
            DataRow row = dt.Rows[0];
            TipoArticulo ta = mapearTipoArticulo(row);
            return ta;
        }
        /// <summary>
        /// Modifica un tipo de articulo
        /// </summary>
        /// <param name="idMarca"></param>
        /// <param name="descripcion"></param>
        /// <param name="fechaBaja"></param>
        public void ModificarTipoArticulo(int idTipoArticulo, string descripcion, DateTime? fechaBaja)
        {
            BeginTransaction();
            try
            {
                TipoArticulo ta = new TipoArticulo();
                ta.Descripcion = descripcion;
                ta.IdtipoArticulo = Convert.ToInt32(idTipoArticulo);
                ta.FechaBaja = fechaBaja;

                updateTipoArticulo(ta);
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
        public void EliminarTipoArticulo(int idTipoArticulo)
        {
            BeginTransaction();
            try
            {
                int id = Convert.ToInt32(idTipoArticulo);
                deleteTipoArticulo(id);
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
        public int AgregarTipoArticulo(string descripcion, DateTime? fechaBaja)
        {
            BeginTransaction();
            try
            {
                TipoArticulo ta = new TipoArticulo();
                ta.Descripcion = descripcion;
                ta.FechaBaja = fechaBaja;

                int id = insertTipoArticulo(ta);
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


        #region Articulo
        /// <summary>
        /// Valida q no exista un articulo con la misa descripcion marca y tipo de articulo.
        /// Si el articulo ya exsite tira excepcion Propia
        /// </summary>
        /// <param name="descripcion"></param>
        /// <param name="idMarca"></param>
        /// <param name="idTipoArticulo"></param>
        public void ValidarArticulo(string descripcion, int idMarca, int idTipoArticulo)
        {
            BeginTransaction();
            try
            {
                string sql = "SELECT * FROM articulo a WHERE a.descripcion = :p1 AND a.idmarca=:p2 AND a.idtipoarticulo=:p3";
                DataTable dt = conn.GetDT(sql, descripcion, idMarca, idTipoArticulo);
                if (dt != null && dt.Rows.Count > 0)
                {
                    throw new ExcepcionPropia("Ya existe un articulo con la misma marca, descripcion y tipo de articulo");
                }
                CommitTransaction();
            }
            catch (ExcepcionPropia myEx)
            {
                RollbackTransaction();
                ControladorExcepcion.tiraExcepcion(myEx.Message);
            }
            catch (Npgsql.NpgsqlException ex)
            {
                RollbackTransaction();
                ControladorExcepcion.tiraExcepcion(ex.Message);
            }
        }

        private int insertArticulo(Articulo a)
        {
            string sql = @"INSERT INTO articulo
                        (	                     
	                        descripcion,
	                        idmarca,
	                        idtipoarticulo,
	                        fecha_alta,
	                        controlar_stock,
	                        codigo,
                            fecha_baja,
                            idarticulo_agrupacion
                        )
                        VALUES
                        (
	                        :descripcion,
	                        :idmarca,
	                        :idtipoarticulo,
	                        :precio,
	                        :p1,
	                        :p2,
	                        :p3,
                             :p4
                        )";
            conn.Execute(sql, a.Descripcion, a.Idmarca, a.Idtipoarticulo, a.FechaAlta, a.ControlarStock,
                a.Codigo, a.FechaBaja, a.IdagrupacionArticulo);
            return conn.LastInsertedId("articulo_idarticulo_seq");
        }

        private void updateArticuloStock(int idArt, int cantASumar, int idSucursal)
        {
            string sql = @"UPDATE articulo_sucursal
                            SET
	                            stock = stock + :pq
                            WHERE idarticulo=:p1 AND idsucursal=:p2";
            conn.Execute(sql, cantASumar, idArt, idSucursal);
        }

        private void updateArtciulo(Articulo a)
        {
            string sql = @"UPDATE articulo
                        SET	
	                        descripcion = :p11,
	                        idmarca = :p10,
	                        idtipoarticulo = :p9,
	                        controlar_stock = :p5,
	                        codigo = :p4,
	                        fecha_baja = :p3,
                            idarticulo_agrupacion= :p12
                        WHERE idarticulo=:p1";
            conn.Execute(sql, a.Descripcion, a.Idmarca, a.Idtipoarticulo, a.ControlarStock, a.Codigo, a.FechaBaja, a.IdagrupacionArticulo, a.Idarticulo);
        }

        private DataTable selectArticulos()
        {
            string sql = "SELECT * FROM articulo a";
            return conn.GetDT(sql);
        }
        private DataTable selectArticuloLikeVenta(string descripcionParcial, int idSucursal)
        {
            string sql = @"SELECT
	                        a.idarticulo
                        FROM
	                        articulo a
	                        INNER JOIN tipo_articulo ta
		                        ON   ta.idtipo_articulo = a.idtipoarticulo
	                        INNER JOIN articulo_sucursal  as1
		                        ON   as1.idarticulo = a.idarticulo
	                        INNER JOIN marca m
		                        ON   m.idmarca = a.idmarca
                        WHERE
	                        (
		                        UPPER (a.descripcion) LIKE UPPER ('%' ||:p3 || '%')
		                        OR UPPER (ta.descripcion) LIKE UPPER ('%' ||:p4 || '%')
		                        OR UPPER (m.descripcion) LIKE UPPER ('%' ||:p5 || '%')
	                        )
	                        AND a.fecha_baja IS NULL
	                        AND                           as1.stock > 0
	                        AND as1.idsucursal = :p1";
            return conn.GetDT(sql, descripcionParcial, descripcionParcial, descripcionParcial, idSucursal);
        }
        private DataTable selectArticuloLikecompra(string descripcionParcial)
        {
            string sql = @"SELECT
	                        a.idarticulo
                        FROM
	                        articulo a
	                        INNER JOIN tipo_articulo ta
		                        ON   ta.idtipo_articulo = a.idtipoarticulo
	                        INNER JOIN marca m
		                        ON   m.idmarca = a.idmarca
                        WHERE
	                        (
		                        UPPER (a.descripcion) LIKE UPPER ('%' ||:p3 || '%')
		                        OR UPPER (ta.descripcion) LIKE UPPER ('%' ||:p4 || '%')
		                        OR UPPER (m.descripcion) LIKE UPPER ('%' ||:p5 || '%')
	                        )
	                        AND a.fecha_baja IS NULL";
            return conn.GetDT(sql, descripcionParcial, descripcionParcial, descripcionParcial);
        }
        private DataTable selectArticulo(string codigo, int idSucursal)
        {
            string sql = @"SELECT * FROM articulo a
                        INNER JOIN articulo_sucursal as1 ON as1.idarticulo = a.idarticulo
                        WHERE a.codigo=:p2 AND fecha_baja IS NULL
                        AND as1.idsucursal=:p1";
            return conn.GetDT(sql, codigo, idSucursal);
        }
        private DataTable selectArticulo(int idArt)
        {
            string sql = "SELECT * FROM articulo a WHERE a.idarticulo=:p1";
            return conn.GetDT(sql, idArt);
        }
        /// <summary>
        /// obtiene un articulo sino devuelve excepcion propia
        /// </summary>
        /// <returns></returns>
        private Articulo buscarArticulo(int idArt)
        {
            DataTable dt = selectArticulo(idArt);
            if (dt == null || dt.Rows.Count == 0)
            {
                throw new ExcepcionPropia("No se ha encontrado el articulo");
            }
            DataRow row = dt.Rows[0];
            Articulo a = mapearArticuloSinStock(row);
            return a;
        }


        /// <summary>
        /// Mapea el articulo sin stock
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private Articulo mapearArticuloSinStock(DataRow row)
        {
            Articulo a = new Articulo();
            a.Codigo = row["codigo"].ToString();
            a.ControlarStock = Convert.ToBoolean(row["controlar_stock"]);
            //a.CostoUltimo = Convert.ToDecimal(row["costo_ultimo"]);
            a.Descripcion = row["descripcion"].ToString();
            a.FechaAlta = Convert.ToDateTime(row["fecha_alta"]);
            a.FechaBaja = row["fecha_baja"] as DateTime?;
            a.Idarticulo = Convert.ToInt32(row["idarticulo"]);
            //a.Precio = Convert.ToDecimal(row["precio"]);
            //a.Stock = Convert.ToInt32(row["stock"]);
            ControladorMarcas c_marcas = new ControladorMarcas(conn);

            a.Marca = c_marcas.BuscarMarca(Convert.ToInt32(row["idmarca"]));

            a.TipoArticulo = buscarTipoArticulo(Convert.ToInt32(row["idtipoarticulo"]));

            if (row["idarticulo_agrupacion"] != DBNull.Value)
            {
                a.AgrupacionArticulo = buscarArticuloAgrupacion(Convert.ToInt32(row["idarticulo_agrupacion"]));
            }

            return a;
        }

        /// <summary>
        /// Mapea el articulo con stock y todo
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private Articulo mapearArticulo(DataRow row)
        {
            Articulo a = mapearArticuloSinStock(row);
            a.CostoUltimo = Convert.ToDecimal(row["costo_ultimo"]);
            a.Precio = Convert.ToDecimal(row["precio"]);
            a.Stock = Convert.ToInt32(row["stock"]);
            a.IdSucursal = Convert.ToInt32(row["idsucursal"]);
            return a;
        }


        public Articulo BuscarArticulo(string codigo, int idSucursal)
        {
            BeginTransaction();
            try
            {
                if (codigo == string.Empty)
                {
                    throw new ExcepcionPropia("El codigo no puede ser nulo");
                }
                DataTable dt = selectArticulo(codigo, idSucursal);
                if (dt == null || dt.Rows.Count == 0)
                {
                    throw new ExcepcionPropia("No se ha encontrado ningun articulo con el codigo " + codigo);
                }
                DataRow row = dt.Rows[0];
                Articulo a = mapearArticulo(row);
                CommitTransaction();
                return a;

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
        public Articulo BuscarArticulo(int idArticulo, int idSucursal)
        {

            try
            {
                string sql = @"SELECT * FROM articulo a 
                                INNER JOIN articulo_sucursal as1 ON as1.idarticulo = a.idarticulo
                                WHERE as1.idarticulo=:p1 AND as1.idsucursal=:p2";
                DataTable dt = conn.GetDT(sql, idArticulo, idSucursal);
                if (dt == null || dt.Rows.Count == 0)
                {
                    throw new ExcepcionPropia("No se ha encontrado ningun articulo ");
                }
                DataRow row = dt.Rows[0];
                Articulo a = mapearArticulo(row);
                return a;

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

        public Articulo BuscarArticulo(int idArticulo)
        {
            BeginTransaction();
            try
            {
                DataTable dt = selectArticulo(idArticulo);
                if (dt == null || dt.Rows.Count == 0)
                {
                    throw new ExcepcionPropia("No se ha encontrado ningun articulo ");
                }
                DataRow row = dt.Rows[0];
                Articulo a = mapearArticuloSinStock(row);
                CommitTransaction();
                return a;

            }
            catch (Exception myEx)
            {
                RollbackTransaction();
                ControladorExcepcion.tiraExcepcion(myEx.Message);
                return null;
            }
        }
        /// <summary>
        /// Buscar un Articulo para la venta con un like a descripcion articulo,       
        /// si no encuentra nada devuelve null
        /// </summary>
        /// <param name="descripcionLarga"></param>
        /// <returns></returns>
        public List<Articulo> BuscarListArticuloVenta(string descripcionParcial, int idSucursal)
        {
            BeginTransaction();
            try
            {
                DataTable dt = selectArticuloLikeVenta(descripcionParcial, idSucursal);
                if (dt != null && dt.Rows.Count > 0)
                {
                    List<Articulo> listA = new List<Articulo>();
                    foreach (DataRow row in dt.Rows)
                    {
                        Articulo a = buscarArticulo(Convert.ToInt32(row["idarticulo"]));
                        listA.Add(a);
                    }
                    CommitTransaction();
                    return listA;
                }
                else
                {
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
        /// <summary>
        /// Buscar un Articulo con un like a descripcion articulo para la compra,       
        /// si no encuentra nada devuelve null
        /// </summary>
        /// <param name="descripcionLarga"></param>
        /// <returns></returns>
        public List<Articulo> BuscarListArticuloCompra(string descripcionParcial)
        {
            BeginTransaction();
            try
            {
                DataTable dt = selectArticuloLikecompra(descripcionParcial);
                if (dt != null && dt.Rows.Count > 0)
                {
                    List<Articulo> listA = new List<Articulo>();
                    foreach (DataRow row in dt.Rows)
                    {
                        Articulo a = buscarArticulo(Convert.ToInt32(row["idarticulo"]));
                        listA.Add(a);
                    }
                    CommitTransaction();
                    return listA;
                }
                else
                {
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
        public List<Articulo> BuscarListArticulo()
        {
            BeginTransaction();
            try
            {
                DataTable dt = selectArticulos();
                if (dt != null && dt.Rows.Count > 0)
                {
                    List<Articulo> listA = new List<Articulo>();
                    foreach (DataRow row in dt.Rows)
                    {
                        Articulo a = buscarArticulo(Convert.ToInt32(row["idarticulo"]));
                        listA.Add(a);
                    }
                    CommitTransaction();
                    return listA.OrderBy(a => a.Descripcion).OrderBy(a => a.FechaBaja).ToList();
                }
                else
                {
                    CommitTransaction();
                    return null;
                }
            }
            catch (Exception ex)
            {
                RollbackTransaction();
                ControladorExcepcion.tiraExcepcion(ex.Message);
                return null;
            }
        }

        public int AgregarArticulo(Articulo a)
        {
            BeginTransaction();
            try
            {
                int id = insertArticulo(a);
                CommitTransaction();
                return id;
            }
            catch (Exception myEx)
            {
                RollbackTransaction();
                ControladorExcepcion.tiraExcepcion(myEx.Message);
                return 0;
            }
        }

        public int AgregarArticulo(Articulo a, int idSucursal)
        {
            BeginTransaction();
            try
            {
                int id = insertArticulo(a);
                a.Idarticulo = id;
                insertArticuloSucursal(a, idSucursal);
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
        }

        private void insertArticuloSucursal(Articulo a, int idSucursal)
        {
            string sql = @"INSERT INTO articulo_sucursal
                                (
	                                idarticulo,
	                                idsucursal,
	                                stock,
	                                costo_ultimo,
	                                precio
                                )
                                VALUES
                                (
	                                :idarticulo,
	                                :idsucursal,
	                                :stock,
	                                :costo_ultimo,
	                                :precio
                                )";
            conn.Execute(sql, a.Idarticulo, idSucursal, a.Stock, a.CostoUltimo, a.Precio);
        }
        public int AgregarArticulo(string desc, int idMarca, int idTipoArt,
            bool controlaStock, string codigo, DateTime fechaBaja, int? idArticuloAgrupacion)
        {
            BeginTransaction();
            try
            {
                Articulo a = new Articulo();
                a.Codigo = codigo;
                a.ControlarStock = controlaStock;
                //a.CostoUltimo = costoUltimo;
                a.Descripcion = desc;
                a.FechaAlta = DateTime.Today;
                a.Marca = new Marca();
                a.Marca.Idmarca = idMarca;
                a.TipoArticulo = new TipoArticulo();
                a.TipoArticulo.IdtipoArticulo = idTipoArt;
                // a.Precio = precio;
                // a.Stock = stock;
                if (idArticuloAgrupacion != null && idArticuloAgrupacion != 0)
                {
                    a.AgrupacionArticulo = new ArticuloAgrupacion();
                    a.AgrupacionArticulo.IdagrupacionArticulo = (int)idArticuloAgrupacion;
                }

                int id = insertArticulo(a);
                CommitTransaction();
                return id;
            }
            catch (Exception myEx)
            {
                RollbackTransaction();
                ControladorExcepcion.tiraExcepcion(myEx.Message);
                return 0;
            }
        }

        /// <summary>
        /// Actualiza el stock del art, la cant a sumar puede ser negativa,
        /// en caso de la venta
        /// </summary>
        /// <param name="idArt"></param>
        /// <param name="cantASumar"></param>
        public void ActualizarStockArticulo(int idArt, int cantASumar, int idSucursal)
        {
            BeginTransaction();
            try
            {
                validarExitenciaArticuloSucursal(idArt, idSucursal);
                updateArticuloStock(idArt, cantASumar, idSucursal);
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
        /// Valida q el art sucursal exista, si no existe lo crea
        /// </summary>
        /// <param name="idArt"></param>
        /// <param name="idSucursal"></param>
        private void validarExitenciaArticuloSucursal(int idArt, int idSucursal)
        {
            string sql = @"SELECT 1 FROM articulo_sucursal as1 WHERE as1.idarticulo=:p1 AND as1.idsucursal=:p2";
            DataTable dt = conn.GetDT(sql, idArt, idSucursal);
            ///si no existe lo agrego
            if (dt == null || dt.Rows.Count == 0)
            {
                string sql1 = @"INSERT INTO articulo_sucursal
                                    (
	                                    idarticulo,
	                                    idsucursal	                                   
                                    )
                                    VALUES
                                    (
	                                    :idarticulo,
	                                    :idsucursal
                                    )";
                conn.Execute(sql1, idArt, idSucursal);
            }
        }
        /// <summary>
        /// Actualiza el articulo sucursal menos el stock
        /// </summary>
        /// <param name="costo"></param>
        /// <param name="precio"></param>
        /// <param name="idArt"></param>
        /// <param name="idSucu"></param>
        public void ModificarArticuloSucursal(decimal costo, decimal precio, int idArt, int idSucu)
        {
            BeginTransaction();
            try
            {
                validarExitenciaArticuloSucursal(idArt, idSucu);
                string sql = @"UPDATE articulo_sucursal
                            SET
	                            costo_ultimo = :p4,
	                            precio = :p3
                            WHERE idarticulo=:p1 AND idsucursal=:p2";
                conn.Execute(sql, costo, precio, idArt, idSucu);
                CommitTransaction();

            }
            catch (Npgsql.NpgsqlException ex)
            {
                RollbackTransaction();
                ControladorExcepcion.tiraExcepcion(ex);
            }
        }

        public void ModificarArticulo(int idArt, string desc, int idMarca, int idTipoArt,
            bool controlSotock, string codigo, DateTime? fechaBaja, int? idArticuloAgrupacion)
        {
            BeginTransaction();
            try
            {
                Articulo a = new Articulo();
                a.Idarticulo = idArt;
                a.Codigo = codigo;
                a.ControlarStock = controlSotock;
                //a.CostoUltimo = costo;
                a.Descripcion = desc;
                a.FechaBaja = fechaBaja;
                a.Marca = new Marca();
                a.Marca.Idmarca = idMarca;
                a.TipoArticulo = new TipoArticulo();
                a.TipoArticulo.IdtipoArticulo = idTipoArt;
                ///a.Precio = precio;
                // a.Stock = stock;
                if (idArticuloAgrupacion != null && idArticuloAgrupacion != 0)
                {
                    a.AgrupacionArticulo = new ArticuloAgrupacion();
                    a.AgrupacionArticulo.IdagrupacionArticulo = (int)idArticuloAgrupacion;
                }
                updateArtciulo(a);
                CommitTransaction();
            }
            catch (Exception myEx)
            {
                RollbackTransaction();
                ControladorExcepcion.tiraExcepcion(myEx.Message);
            }

        }
        #endregion

        #region Articulo Agrupacion
        /// <summary>
        /// Obtiene una lista de articulos agrupacion si no encuentra nada devuelve null solo los activos
        /// </summary>
        /// <returns></returns>
        public List<ArticuloAgrupacion> BuscarListArticuloAgrupacion()
        {
            BeginTransaction();
            try
            {
                string sql = "SELECT * FROM articulo_agrupacion aa where fecha_baja is null";
                DataTable dt = conn.GetDT(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    List<ArticuloAgrupacion> listAA = new List<ArticuloAgrupacion>();
                    foreach (DataRow row in dt.Rows)
                    {
                        ArticuloAgrupacion aa = mapearArticuloAgrupacion(row);
                        listAA.Add(aa);
                    }
                    CommitTransaction();
                    return listAA;
                }
                else
                {
                    CommitTransaction();
                    return null;
                }
            }
            catch (Exception ex)
            {
                RollbackTransaction();
                ControladorExcepcion.tiraExcepcion(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Obtiene una lista de articulos agrupacion si no encuentra nada devuelve null inclusive los dados de baja
        /// </summary>
        /// <returns></returns>
        public List<ArticuloAgrupacion> BuscarListArticuloAgrupacionTodos()
        {
            BeginTransaction();
            try
            {
                string sql = "SELECT * FROM articulo_agrupacion aa";
                DataTable dt = conn.GetDT(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    List<ArticuloAgrupacion> listAA = new List<ArticuloAgrupacion>();
                    foreach (DataRow row in dt.Rows)
                    {
                        ArticuloAgrupacion aa = mapearArticuloAgrupacion(row);
                        listAA.Add(aa);
                    }
                    CommitTransaction();
                    return listAA;
                }
                else
                {
                    CommitTransaction();
                    return null;
                }
            }
            catch (Exception ex)
            {
                RollbackTransaction();
                ControladorExcepcion.tiraExcepcion(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Modifica un articulo agrupacion
        /// </summary>
        /// <param name="idArticuloAgrupacion"></param>
        /// <param name="descripcion"></param>
        /// <param name="idMarca"></param>
        /// <param name="idTipoArticulo"></param>
        /// <param name="fechaAlta"></param>
        /// <param name="fechaBaja"></param>
        /// <param name="?"></param>
        public void ModificarArticuloAgrupacion(int idArticuloAgrupacion, string descripcion, int idMarca, int idTipoArticulo, DateTime fechaAlta, DateTime? fechaBaja)
        {

            try
            {
                BeginTransaction();
                string sql = @"UPDATE articulo_agrupacion
                        SET
	
                        descripcion = :p6,
                        idmarca = :p5,
                        idtipo_articulo = :p4,
                        fecha_alta = :p3,
                        fecha_baja = :p2
                        WHERE idagrupacion_articulo=:p1";
                conn.Execute(sql, descripcion, idMarca, idTipoArticulo, fechaAlta, fechaBaja, idArticuloAgrupacion);
                CommitTransaction();
            }
            catch (Npgsql.NpgsqlException ex)
            {
                RollbackTransaction();
                ControladorExcepcion.tiraExcepcion(ex);
            }

        }

        /// <summary>
        /// Agrega naagrupacion articulos
        /// </summary>
        /// <param name="descripcion"></param>
        /// <param name="idMarca"></param>
        /// <param name="idTipoArticulo"></param>
        /// <param name="fechaAlta"></param>
        /// <param name="fechaBaja"></param>
        /// <param name="?"></param>
        /// <returns></returns>
        public int AgregarArticuloAgrupacion(string descripcion, int idMarca, int idTipoArticulo, DateTime fechaAlta, DateTime? fechaBaja)
        {
            try
            {
                BeginTransaction();
                string sql = @"INSERT INTO articulo_agrupacion
                                (	                                
	                                descripcion,
	                                idmarca,
	                                idtipo_articulo,
	                                fecha_alta,
	                                fecha_baja
                                )
                                VALUES
                                (
	                                :descripcion,
	                                :idmarca,
	                                :idtipo_articulo, 
	                                :fecha_alta,
	                                :fecha_baja 
                                )";
                int id = conn.Execute(sql, descripcion, idMarca, idTipoArticulo, fechaAlta, fechaBaja);
                CommitTransaction();
                return id;
            }
            catch (Npgsql.NpgsqlException ex)
            {
                RollbackTransaction();
                ControladorExcepcion.tiraExcepcion(ex);
                return 0;
            }

        }

        /// <summary>
        /// Obtiene una lista de articulos agrupacion si no encuentra nada devuelve null
        /// </summary>
        /// <returns></returns>
        public ArticuloAgrupacion BuscarArticuloAgrupacion(int idArticuloAgrupacion)
        {
            BeginTransaction();
            try
            {
                ArticuloAgrupacion art = buscarArticuloAgrupacion(idArticuloAgrupacion);
                CommitTransaction();
                return art;
            }
            catch (Exception ex)
            {
                RollbackTransaction();
                ControladorExcepcion.tiraExcepcion(ex.Message);
                return null;
            }
        }

        private ArticuloAgrupacion buscarArticuloAgrupacion(int idArticuloAgrupacion)
        {
            string sql = "SELECT * FROM articulo_agrupacion aa WHERE aa.idagrupacion_articulo=:p1";
            DataTable dt = conn.GetDT(sql, idArticuloAgrupacion);
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                ArticuloAgrupacion aa = mapearArticuloAgrupacion(row);

                return aa;
            }
            else
            {

                return null;
            }
        }
        private ArticuloAgrupacion mapearArticuloAgrupacion(DataRow row)
        {
            ArticuloAgrupacion aa = new ArticuloAgrupacion();
            aa.Descripcion = row["descripcion"].ToString();
            aa.FechaAlta = Convert.ToDateTime(row["fecha_alta"]);
            aa.FechaBaja = row["fecha_baja"] as DateTime?;
            aa.IdagrupacionArticulo = Convert.ToInt32(row["idagrupacion_articulo"]);
            ControladorMarcas c_marca = new ControladorMarcas(conn);
            aa.Marca = c_marca.BuscarMarca(Convert.ToInt32(row["idmarca"]));
            aa.TipoArticulo = buscarTipoArticulo(Convert.ToInt32(row["idtipo_articulo"]));
            return aa;
        }
        /// <summary>
        ///Devuelve los articulos q se pueden cambiar por el indicado,
        ///Es decir los articulos q estan agrupados en el articulo en cuestion
        /// </summary>
        /// <param name="idArticuloAgrupacion"></param>
        /// <returns></returns>
        public List<Articulo> BuscarListArticulosACambiar(int idarticulo, int idSucursal)
        {
            try
            {
                Articulo aa = buscarArticulo(idarticulo);
                string sql = @"SELECT * FROM articulo a 
                                INNER JOIN articulo_sucursal as1 on a.idarticulo = as1.idarticulo
                                WHERE a.idarticulo_agrupacion=:p1 AND a.idarticulo<>:p2
                                AND as1.idsucursal=:p3";
                DataTable dt = conn.GetDT(sql, aa.IdagrupacionArticulo, idarticulo, idSucursal);
                if (dt == null || dt.Rows.Count == 0)
                {
                    return null;
                }
                else
                {
                    List<Articulo> listA = new List<Articulo>();
                    foreach (DataRow row in dt.Rows)
                    {
                        Articulo a = mapearArticulo(row);
                        listA.Add(a);
                    }
                    return listA;
                }
            }
            catch (Npgsql.NpgsqlException ex)
            {
                ControladorExcepcion.tiraExcepcion(ex);
                return null;
            }

        }
        #endregion

    }
}
