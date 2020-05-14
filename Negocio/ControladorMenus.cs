using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Datos;
using Entidades;

namespace Negocio
{
    public class ControladorMenus : ControladorGenerico
    {
        #region Constructores
        /// <summary>
        /// constructor por defecto de la clase base
        /// </summary>
        public ControladorMenus()
            : base()
        {

        }
        /// <summary>
        ///para Utilizar sin using. Libera todo el garbage Collector (Espero)
        /// No maneja la coneccion
        /// </summary>
        public ControladorMenus(bool sinConexion)
            : base(sinConexion)
        {

        }
        /// <summary>
        ///Constructor al cual le paso la coneccion para no utilizar la del Controlador actua si no otra
        /// </summary>
        public ControladorMenus(Connection c)
            : base(c)
        {

        }

        #endregion
        private DataTable selectMenuUsuarioDb(int idOperador, int idSucursal, int idMenu)
        {
            string sql = "SELECT * FROM  menu_usuario(:pcodusuario, :pcodsucursal, :pcodmenu)";
            //el tercer parametro por ahora no se tiene en cuenta es un codigo de menu por si existen varios menus
            //en este caso al archivo ObtenerMenuUsuario.sql, en propiedades cambiar em Accion de compilacion a "Recurso incrustado"
            return conn.GetDT(sql, idOperador, idSucursal, idMenu);
        }
        /// <summary>
        /// Obtiene el menu bien formateado
        /// </summary>
        /// <returns></returns>
        private DataTable selectMenuFormateado(int idMenu)
        {
            string sql = @"SELECT * from ep_menu(:v_pidmenu)";
            return conn.GetDT(sql, idMenu);
        }

        private int insertPagina(Pagina p)
        {
            string sql = @"INSERT INTO paginas
                            (	                            
	                            descripcion,
	                            nombre_pagina,
                                codigo
                            )
                            VALUES
                            (
	                            :descripcion,
	                            :nombre_pagina,
                                :p1
                            )";
            conn.Execute(sql, p.Descripcion, p.NombrePagina, p.Codigo);
            return conn.LastInsertedId("paginas_idpagina_seq");
        }
        private DataTable selectPaginas()
        {
            string sql = "SELECT * FROM paginas p";
            return conn.GetDT(sql);
        }
        private DataTable selectPagina(int idPagina)
        {
            string sql = "SELECT * FROM paginas p WHERE p.idpagina=:p1";
            return conn.GetDT(sql, idPagina);
        }
        private void updatePaginaABM(Pagina p)
        {
            string sql = @"UPDATE paginas
                            SET	                           
	                            descripcion = :p2,
	                            nombre_pagina = :p3,
                                codigo = :p4
                            WHERE idpagina=:p1";
            conn.Execute(sql, p.Descripcion, p.NombrePagina, p.Codigo, p.Idpagina);
        }
        private void deletePagina(int idPagina)
        {
            string sql = "DELETE FROM paginas WHERE idpagina=:p2";
            conn.Execute(sql, idPagina);
        }
        private DataTable selectPaginas(int idMenu, int idRol)
        {
            string sql = @"SELECT distinct p.* FROM items_menus im INNER JOIN paginas p ON p.idpagina = im.idpagina
                            WHERE im.idmenu = :p1 
                            AND im.idpagina NOT IN (SELECT rp.idpagina FROM roles_pagina rp WHERE rp.idrol=:p2)";
            return conn.GetDT(sql, idMenu, idRol);
        }
        private DataTable selectPaginasRol(int idRol)
        {
            string sql = @"SELECT * FROM roles_pagina rp WHERE rp.idrol=:p1";
            return conn.GetDT(sql, idRol);
        }
        /// <summary>
        /// Obtiene datos de las paginas en las q puede acceder el usuario.        
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        private DataTable selectPaginas(int idUsuario)
        {
            string sql = @"SELECT * FROM paginas p 
                            INNER JOIN roles_pagina rp ON rp.idpagina = p.idpagina
                            INNER JOIN roles r ON r.idrol = rp.idrol
                            INNER JOIN roles_usuarios ru ON ru.idrol = r.idrol
                            WHERE ru.codigo_operador=:p1";
            return conn.GetDT(sql, idUsuario);
        }

        private int insertMenu(Menuu m)
        {
            string sql = @"INSERT INTO menus
                        (
	                       nombre
                        )
                        VALUES
                        (
	                        :nombre
                        )";
            conn.Execute(sql, m.Nombre);
            return conn.LastInsertedId("ep_menus_cod_menu_seq");
        }
        private DataTable selectMenus()
        {
            string sql = "SELECT * FROM menus m";
            return conn.GetDT(sql);
        }
        private void updateMenu(Menuu m)
        {
            string sql = @"UPDATE menus
                        SET	                       
	                        nombre = :p1
                        WHERE idmenu = :p2";
            conn.Execute(sql, m.Nombre, m.IdMenu);
        }
        private DataTable selectMenu(int idMenu)
        {
            string sql = "SELECT * FROM menus m WHERE m.idmenu=:p2";
            return conn.GetDT(sql, idMenu);
        }
        private void deleteMenu(int idMenu)
        {
            string sql = "DELETE FROM menus WHERE idmenu=:p1";
            conn.Execute(sql, idMenu);

        }
        private void funcionMenuAgregarMover(int idHijo, int? idPadre, bool alInicio, int? despuesDe, bool moverRama)
        {
            string sql = @"SELECT * from nsmenu_agregar_mover(:v_pcod_hijo, :v_pcod_padre, :v_pal_inicio, 
                          :v_pdespues_de, :v_pmover_rama)";
            conn.GetDT(sql, idHijo, idPadre, alInicio, despuesDe, moverRama);
        }

        private int insertIntemMenu(ItemMenu i)
        {
            string sql = @"INSERT INTO items_menus
                            (	                            
	                            idmenu,
	                            idpagina,
	                            etiqueta,
	                            es_division
                            )
                            VALUES
                            (
	                            :idmenu,
	                            :idpagina,
	                            :etiqueta,
	                            :es_division
                            )";
            conn.Execute(sql, i.Idmenu, i.Idpagina, i.Etiqueta, i.EsDivision);
            return conn.LastInsertedId("ep_items_menus_cod_item_menu_seq");
        }
        private void deleteItemMenu(int idItem)
        {
            string sql = @"DELETE FROM items_menus WHERE iditem_menu=:p1";
            conn.Execute(sql, idItem);
        }
        private DataTable selectItemMenu(int idItemMenu)
        {
            string sql = @"SELECT * FROM items_menus im WHERE im.iditem_menu=:p1";
            return conn.GetDT(sql, idItemMenu);
        }
        private DataTable selectItemsMenu(int idMenu)
        {
            string sql = @"SELECT * FROM items_menus im WHERE im.idmenu=:p1";
            return conn.GetDT(sql, idMenu);
        }
        private void updateItemMenu(ItemMenu i)
        {
            string sql = @"UPDATE items_menus
                            SET	                      
	                            idmenu = :p2,
	                            idpagina = :p3,
	                            etiqueta = :p4,
	                            es_division = :p5
                            WHERE iditem_menu=:p1";
            conn.Execute(sql, i.Idmenu, i.Idpagina, i.Etiqueta, i.EsDivision, i.IditemMenu);
        }
        private void funcionNsmenu_agregar(int idHijo, int? idPadre, bool alInicio, int? despuesDe)
        {
            string sql = "SELECT * FROM nsmenu_agregar(:v_pcod_hijo, :v_pcod_padre, :v_pal_inicio, :v_pdespues_de)";
            conn.GetDT(sql, idHijo, idPadre, alInicio, despuesDe);
        }
        private void deleteArbolMenu(int idItemMenu)
        {
            string sql = "DELETE FROM arbol_menus WHERE iditem_menu=:p1";
            conn.Execute(sql, idItemMenu);
        }

        /// <summary>
        /// Busca una pagina con las propiedades basicas
        /// Si no encuentra nada devuelve excepcion propia
        /// </summary>
        /// <param name="idPagina"></param>
        /// <returns></returns>
        private Pagina buscarPagina(int idPagina)
        {
            DataTable dt = selectPagina(idPagina);
            if (dt == null || dt.Rows.Count == 0)
            {
                throw new ExcepcionPropia("No se ha encontrado la pagina");
            }
            DataRow row = dt.Rows[0];
            Pagina p = mapearPagina(row);
            return p;
        }
        /// <summary>
        /// Otiene el item menu solo con las propiedades basicas
        /// Si no encuentra nada devuelve excepcionPropia
        /// </summary>
        /// <returns></returns>
        private ItemMenu buscarItemMenu(int idItemMenu)
        {
            DataTable dt = selectItemMenu(idItemMenu);
            if (dt == null || dt.Rows.Count == 0)
            {
                throw new ExcepcionPropia("No se ha econtrado el itemMenu");
            }
            DataRow row = dt.Rows[0];
            ItemMenu im = new ItemMenu();
            im.EsDivision = Convert.ToBoolean(row["es_division"]);
            im.Etiqueta = row["etiqueta"].ToString();
            im.IditemMenu = Convert.ToInt32(row["iditem_menu"]);
            im.Idmenu = Convert.ToInt32(row["idmenu"]);
            if (row["idpagina"] != DBNull.Value)
            {
                im.Pagina = buscarPagina(Convert.ToInt32(row["idpagina"]));
            }
            return im;
        }
        /// <summary>
        /// Busca un menu con sus propiedades basicas
        /// Si no encuentra nada devuelve excepcion propia
        /// </summary>
        /// <param name="idMenu"></param>
        /// <returns></returns>
        private Menuu buscarMenu(int idMenu)
        {
            DataTable dt = selectMenu(idMenu);
            if (dt == null || dt.Rows.Count == 0)
            {
                throw new ExcepcionPropia("No se ha econtrado el Menu");
            }
            DataRow row = dt.Rows[0];
            Menuu m = new Menuu();
            m.IdMenu = Convert.ToInt32(row["idmenu"]);
            m.Nombre = row["nombre"].ToString();
            return m;
        }
        private List<ItemMenu> generarMenu(DataTable dt, DataRow[] rowsMenu)
        {
            List<ItemMenu> listI = new List<ItemMenu>();
            foreach (DataRow row in rowsMenu)
            {
                string idpagina = row["pagina"].ToString();
                string etiqueta = row["etiqueta"].ToString();
                bool esDivision = Convert.ToBoolean(row["es_division"]);
                int idItemMenu = Convert.ToInt32(row["iditem_menu"].ToString());
                int idItemPadre = 0;
                if (row["iditem_padre"] != DBNull.Value)
                {
                    idItemPadre = Convert.ToInt32(row["iditem_padre"].ToString());
                }
                ItemMenu im = buscarItemMenu(idItemMenu);
                im.IdPadre = idItemPadre;
                DataRow[] subMenu = dt.Select(String.Format("iditem_padre = {0}", idItemMenu));
                if (subMenu.Length > 0 && !idItemMenu.Equals(idItemPadre))
                {
                    im.ListItemHijos = generarMenu(dt, subMenu);
                }
                listI.Add(im);
            }
            return listI;
        }
        private static Pagina mapearPagina(DataRow row)
        {
            Pagina p = new Pagina();
            p.Idpagina = Convert.ToInt32(row["idpagina"]);
            p.NombrePagina = row["nombre_pagina"].ToString();
            p.Descripcion = row["descripcion"].ToString();
            p.Codigo = row["codigo"].ToString();
            return p;
        }

        /// <summary>
        /// Devuelve el menu usuario si no enceuntra nada devuelve excpetion propia
        /// Devuelve: nivel/iditem_menu/iditem_padre/es_division/
        /// etiqueta/pagina/restringido/solo_lectura/pide_autorizacion/reingresa_clave
        /// </summary
        /// <param name="idOperador"></param>
        /// <param name="idSucursal"></param>
        /// <param name="idMenu"></param>
        /// <returns></returns>
        public DataTable ObtenerMenusUsuario(int idOperador, int idSucursal, int idMenu)
        {
            BeginTransaction();
            try
            {
                DataTable dt = selectMenuUsuarioDb(idOperador, idSucursal, idMenu);
                if (dt == null || dt.Rows.Count == 0)
                {
                    throw new ExcepcionPropia("No se ha podido generar el menu usuario");
                }
                CommitTransaction();
                return dt;
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
        public void EliminarMenu(int idMenu)
        {
            BeginTransaction();
            try
            {
                deleteMenu(idMenu);
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
        /// Agrega un Menu. Solo el menu sin sus items
        /// </summary>
        /// <param name="m"></param>
        public void AgregarMenu(Menuu m)
        {
            BeginTransaction();
            try
            {
                insertMenu(m);
                //if (m.ListItems != null && m.ListItems.Count > 0)
                //{
                //    foreach (ItemMenu item in m.ListItems)
                //    {
                //        insertIntemMenu(item);
                //        funcionNsmenu_agregar(item.IditemMenu, item.IdPadre, item.AlInicio, item.DespuesDe);
                //    }
                //}
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
        /// Si no encuentra nada devuelve excepcion Propia
        /// </summary>
        /// <param name="idMenu"></param>
        /// <returns></returns>
        public Menuu BuscarMenuFormateadoCompleto(int idMenu)
        {
            BeginTransaction();
            try
            {
                DataTable dt = selectMenuFormateado(idMenu);
                if (dt == null && dt.Rows.Count == 0)
                {
                    throw new ExcepcionPropia("No se ha encontrado el menu Formateado");
                }
                DataRow[] rowsMenu = dt.Select("nivel = 1");
                Menuu menuActual = buscarMenu(idMenu);
                List<ItemMenu> listI = generarMenu(dt, rowsMenu);
                menuActual.ListItems = listI;
                CommitTransaction();
                return menuActual;
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
        public void ModificarMenu(Menuu m)
        {
            BeginTransaction();
            try
            {
                updateMenu(m);
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
        /// Obtiene una lista de menus solo con las propiedades de tipo basico
        /// Si no encuentra nada devuelve null
        /// </summary>
        public List<Menuu> BuscarListMenus()
        {

            BeginTransaction();
            try
            {
                DataTable dt = selectMenus();
                if (dt == null || dt.Rows.Count == 0)
                {
                    throw new ExcepcionPropia("No se han encontrado Menues");
                }
                List<Menuu> listM = new List<Menuu>();
                foreach (DataRow row in dt.Rows)
                {
                    Menuu m = new Menuu();
                    m.IdMenu = Convert.ToInt32(row["idmenu"]);
                    m.Nombre = row["nombre"].ToString();
                    listM.Add(m);
                } CommitTransaction();
                return listM;
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
        /// Obtiene una lista de todas las paginas.
        /// Si no encuentra nada devuelve excepcion
        /// </summary>
        /// <returns></returns>
        public List<Pagina> BuscarListPaginas()
        {
            BeginTransaction();
            try
            {
                DataTable dt = selectPaginas();
                if (dt == null && dt.Rows.Count == 0)
                {
                    throw new ExcepcionPropia("No se han encontrado Paginas");
                }
                List<Pagina> listP = new List<Pagina>();
                foreach (DataRow row in dt.Rows)
                {
                    Pagina p = mapearPagina(row);
                    listP.Add(p);
                }
                CommitTransaction();
                return listP;
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
        /// Obtiene una lista de paginas que ESTAN en el MENU pero NO en el ROL.
        /// Solo trae una aparicion de la pagina, si es q aparece varias veces
        ///Si no encuentra nada devuelve excepcion propia
        /// </summary>
        /// <param name="idMenu"></param>
        /// <returns></returns>
        public List<Pagina> BuscarListPaginas(int idMenu, int idRol)
        {
            BeginTransaction();
            try
            {
                DataTable dt = selectPaginas(idMenu, idRol);
                if (dt == null && dt.Rows.Count == 0)
                {
                    throw new ExcepcionPropia("No se han encontrado Paginas");
                }
                List<Pagina> listP = new List<Pagina>();
                foreach (DataRow row in dt.Rows)
                {
                    Pagina p = mapearPagina(row);
                    listP.Add(p);
                }
                CommitTransaction();
                return listP;
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
        /// Obtiene una lista de paginas de un ROL
        /// Si no encuentra nada devuelve Excepcion propia
        /// </summary>
        /// <param name="idMenu"></param>
        /// <returns></returns>
        public List<Pagina> BuscarListPaginas(int idRol)
        {
            BeginTransaction();
            try
            {
                DataTable dt = selectPaginasRol(idRol);
                if (dt == null && dt.Rows.Count == 0)
                {
                    throw new ExcepcionPropia("No se han encontrado Paginas");
                }
                List<Pagina> listP = new List<Pagina>();
                foreach (DataRow row in dt.Rows)
                {
                    int idPagina = Convert.ToInt32(row["idpagina"]);
                    Pagina p = buscarPagina(idPagina);
                    p.SoloLectura = Convert.ToBoolean(row["solo_lectura"]);
                    p.ReingresaClave = Convert.ToBoolean(row["reingresa_clave"]);
                    p.PideAutorizacion = Convert.ToBoolean(row["pide_autorizacion"]);
                    p.Restringido = Convert.ToBoolean(row["restringido"]);
                    listP.Add(p);
                }
                CommitTransaction();
                return listP;
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
        /// Obtiene una lista de paginas para un usuario.
        /// Si no encuentra nada devuelve Excepcion propia
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        public List<Pagina> BuscarListPaginasXUsuario(int idUsuario)
        {
            // BeginTransaction();
            try
            {
                ControladorUsuarios c_usu = new ControladorUsuarios(conn);
                Usuario u = c_usu.BuscarUsuario(idUsuario);
                if (u.EsSuperUsuario)
                {
                    List<Pagina> listP = BuscarListPaginas();
                    // CommitTransaction();
                    return listP;
                }
                else
                {
                    DataTable dt = selectPaginas(idUsuario);
                    if (dt == null || dt.Rows.Count == 0)
                    {
                        throw new ExcepcionPropia("No se han encontrado paginas para el Usuario");
                    }
                    List<Pagina> listP = new List<Pagina>();
                    foreach (DataRow row in dt.Rows)
                    {
                        Pagina p = buscarPagina(Convert.ToInt32(row["idpagina"]));
                        listP.Add(p);
                    }
                    // CommitTransaction();
                    return listP;
                }

            }
            catch (Npgsql.NpgsqlException ex)
            {
                // RollbackTransaction();
                ControladorExcepcion.tiraExcepcion(ex);
                return null;
            }

        }


        /// <summary>
        /// Obtiene una pagina. Si no la encuentra devuelve excepcionPropia
        /// </summary>
        /// <param name="idPagina"></param>
        /// <returns></returns>
        public Pagina BuscarPagina(int idPagina)
        {
            BeginTransaction();
            try
            {
                Pagina p = buscarPagina(idPagina);
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

        }
        public int AgregarPagina(Pagina p)
        {
            BeginTransaction();
            try
            {
                int id = insertPagina(p);
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
        /// <summary>
        ///Se debe pasar una pagina completo con todos las propiedades a modificar, 
        /// y tambien con las que no se modifican
        /// </summary>
        /// <param name="p"></param>
        public void ModificarPagina(Pagina p)
        {
            BeginTransaction();
            try
            {
                updatePaginaABM(p);
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
        public void EliminarPagina(int idPagina)
        {
            BeginTransaction();
            try
            {
                deletePagina(idPagina);
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
        /// Agrega un nuevo intem Menu. y Genera el arbol
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public void AgregarListItemMenu(List<ItemMenu> ListItem)
        {
            BeginTransaction();
            try
            {
                foreach (ItemMenu item in ListItem)
                {
                    int id = insertIntemMenu(item);
                    funcionNsmenu_agregar(id, item.IdPadre, item.AlInicio, item.DespuesDe);
                }
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
        /// Modifica el item Menu y el arbol
        /// </summary>
        /// <param name="i"></param>
        public void ModificarListItemMenuCompleto(List<ItemMenu> ListItem)
        {
            BeginTransaction();
            try
            {
                foreach (ItemMenu i in ListItem)
                {
                    updateItemMenu(i);
                    deleteArbolMenu(i.Idmenu);
                    funcionNsmenu_agregar(i.IditemMenu, i.IdPadre, i.AlInicio, i.DespuesDe);
                }
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
        /// Modifica el item Menu Solamente
        /// </summary>
        /// <param name="i"></param>
        public void ModificarListItemMenu(List<ItemMenu> ListItem)
        {
            BeginTransaction();
            try
            {
                foreach (ItemMenu i in ListItem)
                {
                    updateItemMenu(i);
                }
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


        public void EliminarListItemMenu(List<ItemMenu> ListItem)
        {
            BeginTransaction();
            try
            {
                foreach (ItemMenu i in ListItem)
                {
                    deleteItemMenu(i.IditemMenu);
                    // deleteArbolMenu(i.IditemMenu);
                }
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
        /// Obtiene un item menu si no lo encuentra devuelve excepcion
        /// </summary>
        /// <param name="idItemMenu"></param>
        /// <returns></returns>
        public ItemMenu BuscarItemMenu(int idItemMenu)
        {
            BeginTransaction();
            try
            {
                ItemMenu ie = buscarItemMenu(idItemMenu);
                CommitTransaction();
                return ie;
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
        /// Mueve un item menu segun sus propiedades indicadas
        /// </summary>
        public void MoverItemMenu(ItemMenu im)
        {
            BeginTransaction();
            try
            {
                funcionMenuAgregarMover(im.IditemMenu, im.IdPadre, im.AlInicio, im.DespuesDe, true);
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

    }
}
