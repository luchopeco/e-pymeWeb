using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Datos;
using Entidades;
using Npgsql;


namespace Negocio
{
    public class ControladorRoles: ControladorGenerico
    {
        
        #region Altas
        private int insertRolDb(string descRol)
        {
            string sql = "INSERT INTO roles ( descripcion ) VALUES(	:descRol)";
            conn.Execute(sql, descRol);
            return conn.LastInsertedId("roles_idrol_seq");
        }
        private void updataeRol(Rol r)
        {
            string sql = @"UPDATE roles
                        SET	                        
	                        descripcion = :p2
                        WHERE idrol=:p1";
            conn.Execute(sql,r.Descripcion,r.IdRol);
        }
        /// <summary>
        /// Inserta info en la tabla roles_pagina 
        /// </summary>
        /// <param name="p">entidad pagina</param>
        /// <param name="idRol">idrol</param>
        private void insertPaginaRolDb(Pagina p, int idRol)
        {
            string sql = @"INSERT INTO roles_pagina (idrol,	idpagina,	restringido,	solo_lectura,	reingresa_clave,	pide_autorizacion)
                            VALUES(:idrol, :idpagina, :restringido, :sololectura, :reingresaclave, :pideautorizacion)";
            conn.Execute(sql, idRol, p.Idpagina, p.Restringido, p.SoloLectura, p.ReingresaClave, p.PideAutorizacion);
        }        
        /// <summary>
        /// Inserta info en la tabla roles_Funciopnalidad
        /// </summary>
        /// <param name="f">entidad funcionalidad</param>
        /// <param name="idRol">idRol</param>
        private void InsertFuncionalidadRolDb(Funcionalidad f, int idRol)
        {
            string sql = @"INSERT INTO roles_funcionalidades
                        (
	                        idrol,
	                        idfuncionalidad,
	                        restringido,
	                        solo_lectura,
	                        reingresa_clave,
	                        pide_autorizacion
                        )
                        VALUES
                        (
	                        :idrol,
	                        :idfuncionalidad,
	                        :restringido,
	                        :sololectura,
	                        :reingresaclave,
	                        :pideautorizacion 
                        )";
            conn.Execute(sql, idRol, f.IdFuncionalidad, f.Restringido, f.SoloLectura, f.ReingresaClave, f.PideAutorizacion);
        }
        private void insertRolUsuario(int idRol, int codOperador)
        {
            string sql = @"INSERT INTO roles_usuarios
                            (
	                            codigo_operador,
	                            idrol
                            )
                            VALUES
                            (
	                            :codigo_operador,
	                            :idrol
                            )";
            conn.Execute(sql, codOperador, idRol);                
        }
        #endregion

        #region Bajas
        private void deletePaginaRolDb(int idrol, int idpagina)
        {
            string sql = "DELETE FROM roles_pagina WHERE idrol =:idrol AND idpagina= :idpagina";
            conn.Execute(sql, idrol, idpagina);
        }

        private void DeleteFuncionalidadRolDb(int idRol, int idFuncionalidad)
        {
            conn.Execute(Connection.GetSqlCodeFromResource("Sql.DeleteFuncionalidadRolDb.sql"), idRol, idFuncionalidad);
        }

        private void DeleteRolDb(int idRol) 
        {
            conn.Execute("DELETE FROM roles WHERE idrol = :idrol", idRol);
        }

        private void deleteRolUsuario(int idRol, int codOperador)
        {
            string sql = "DELETE FROM roles_usuarios WHERE codigo_operador=:p1 AND idrol=:p3";
            conn.Execute(sql,codOperador, idRol);
        }

        private void deletePaginasSucursales(int idPagina, int idSucrusal)
        {
            string sql = "DELETE FROM sucursal_paginas WHERE idpagina=:p1 AND nro_sucursal=:p2";
            conn.Execute(sql,idPagina,idSucrusal);
        }
        #endregion

        #region Consultas

//        private DataTable selectRolesPaginasYFuncionalidadesDb(int idRol)
//        {
//            string sql = @"select r.descripcion, null as idpagina, rf.idfuncionalidad, rf.solo_lectura, rf.reingresa_clave, rf.pide_autorizacion, rf.restringido
//                            from roles_funcionalidades rf 
//                            inner join roles r on r.idrol = rf.idrol
//                            where r.idrol = :idrol
//                            union all
//                            select r.descripcion, rp.idpagina, null, rp.solo_lectura, rp.reingresa_clave, rp.pide_autorizacion, rp.restringido
//                            from roles_pagina rp 
//                            inner join roles r on r.idrol = rp.idrol
//                            where r.idrol = :idrol";
//            return  conn.GetDT(sql, idRol);            
//        }
        
        /// <summary>
        /// Ver si despues lo usas
        /// </summary>
        /// <param name="idRol"></param>
        /// <returns></returns>
        private Rol selectRolXIdDb(int idRol)
        {
            Rol rolRet = new Rol();
            DataTable dt = selectRol(idRol);
            rolRet.IdRol = idRol;
            if (dt!=null&& dt.Rows.Count > 0)
            {
                rolRet.Descripcion = dt.Rows[0]["descripcion"].ToString();
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["idpagina"].Equals(DBNull.Value))
                    {
                        Funcionalidad itemF = new Funcionalidad();
                        itemF.IdFuncionalidad = Convert.ToInt32(dr["idfuncionalidad"]);
                        itemF.PideAutorizacion = Convert.ToBoolean(dr["pide_autorizacion"]);
                        itemF.ReingresaClave = Convert.ToBoolean(dr["reingresa_clave"]);
                        itemF.Restringido = Convert.ToBoolean(dr["restringido"]);
                        itemF.SoloLectura = Convert.ToBoolean(dr["solo_lectura"]);

                        rolRet.ListFuncionalidades.Add(itemF);
                    }
                    else
                    {
                        Pagina itemP = new Pagina();
                        itemP.Idpagina = Convert.ToInt32(dr["idpagina"]);
                        itemP.PideAutorizacion = Convert.ToBoolean(dr["pide_autorizacion"]);
                        itemP.ReingresaClave = Convert.ToBoolean(dr["reingresa_clave"]);
                        itemP.Restringido = Convert.ToBoolean(dr["restringido"]);
                        itemP.SoloLectura = Convert.ToBoolean(dr["solo_lectura"]);
                        rolRet.ListPaginas.Add(itemP);
                    }
                }
            }
            return rolRet;
        }       
        private DataTable selectRoles()
        {
            string sql = "SELECT * FROM roles r ";
            return conn.GetDT(sql);
        }
        private DataTable selectRol(int idRol)
        {
            string sql = "SELECT * FROM roles r WHERE r.idrol=:p1";
            return conn.GetDT(sql, idRol);
        }
        private DataTable selectRolesUsuarios(int codOperador)
        {
            string sql = "SELECT * FROM roles_usuarios ru WHERE ru.codigo_operador =:p1";
            return conn.GetDT(sql,codOperador);
        }
        #endregion

        #region Modificaciones        

        private void updatePaginaRolDb(int idrol, int idpagina, bool solo_lectura, bool reingresa_clave, bool pide_autorizacion, bool restringido)
        {
            string sql = @"UPDATE roles_pagina
                            SET
	                            solo_lectura = :sololectura,
	                            reingresa_clave = :reingresaclave,
	                            pide_autorizacion = :pideautorizacion,
	                            restringido = :restringido
                                where idrol=:p1 and idpagina=:p2";
            conn.Execute(sql, solo_lectura, reingresa_clave, pide_autorizacion, restringido,idrol, idpagina);
        }
        private void UpdateFuncionalidadRolDb(int idrol, int idfuncionalidad, bool solo_lectura, bool reingresa_clave, bool pide_autorizacion, bool restringido)
        {
            conn.Execute(Connection.GetSqlCodeFromResource("Sql.UpdateFuncionalidadRolDb.sql"), idrol, idfuncionalidad, solo_lectura, reingresa_clave, pide_autorizacion, restringido);
        }

        #endregion


        /// <summary>
        /// Obtiene un rol con sus propiedades basicas.Si no lo encuentra devuelve
        /// </summary>
        /// <param name="idRol"></param>
        /// <returns></returns>
        private Rol buscarRol(int idRol)
        {
            DataTable dt = selectRol(idRol);
            if (dt==null || dt.Rows.Count==0)
            {
                throw new ExcepcionPropia("No se ha encontrado el rol");
            }
            DataRow row = dt.Rows[0];
            Rol r = new Rol();
            r.IdRol = idRol;
            r.Descripcion = row["descripcion"].ToString();
            return r;
        }

        #region MetodosNegocio     

        //public void ActualizarRolCompleto(DataTable dtRolActual)
        //{
        //    DataTable dtPaginasYfuncActuales = selectRolesPaginasYFuncionalidadesDb(Convert.ToInt32(dtRolActual.Rows[0]["idrol"].ToString()));
        //    foreach (DataRow drActual in dtRolActual.Rows)
        //    {
        //        foreach (DataRow drDb in dtPaginasYfuncActuales.Rows)
        //        {
        //            if (drActual["idpagina"].ToString() == drDb["idpagina"].ToString())
        //            {
        //                int idrol = Convert.ToInt32(drActual["idrol"].ToString());
        //                int idpagina = Convert.ToInt32(drActual["idpagina"].ToString());
        //                bool restringido = Convert.ToBoolean(drActual[""].ToString());
        //                bool solo_lectura = Convert.ToBoolean(drActual["solo_lectura"].ToString()); 
        //                bool reingresa_clave = Convert.ToBoolean(drActual["reingresa_clave"].ToString()); 
        //                bool pide_autorizacion = Convert.ToBoolean(drActual["pide_autorizacion"].ToString()); 
        //                UpdatePaginaRolDb(idrol, idpagina, solo_lectura, reingresa_clave, pide_autorizacion, restringido);
        //            }
        //            if (drActual["idfuncionalidad"].ToString() == drDb["idfuncionalidad"].ToString())
        //            {
        //                int idrol = Convert.ToInt32(drActual["idrol"].ToString());
        //                int idfuncionalidad = Convert.ToInt32(drActual["idfuncionalidad"].ToString());
        //                bool restringido = Convert.ToBoolean(drActual[""].ToString());
        //                bool solo_lectura = Convert.ToBoolean(drActual["solo_lectura"].ToString()); 
        //                bool reingresa_clave = Convert.ToBoolean(drActual["reingresa_clave"].ToString()); 
        //                bool pide_autorizacion = Convert.ToBoolean(drActual["pide_autorizacion"].ToString()); 
        //                UpdateFuncionalidadRolDb(idrol, idfuncionalidad, solo_lectura, reingresa_clave, pide_autorizacion, restringido);
        //            }
        //        }

        //    }
        //}

        /// <summary>
        /// Agrega Un rol.     
        /// </summary>
        /// <param name="r"></param>
        public void AgregarRol(Rol r)
        {
            conn.BeginTransaction();
            try
            {
                r.IdRol= insertRolDb(r.Descripcion);
                //foreach (Funcionalidad f in r.ListFuncionalidades)
                //{
                //    InsertFuncionalidadRolDb(f, r.IdRol);
                //}
                //foreach (Pagina p in r.ListPaginas)
                //{
                //    insertPaginaRolDb(p, r.IdRol);
                //}
                conn.CommitTransaction();
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
        /// Agrega la relacion pagina rol
        /// </summary>
        /// <param name="p"></param>
        /// <param name="idRol"></param>
        public void AgregarPaginaRol(Pagina p, int idRol)
        {
            BeginTransaction();
            try
            {
                insertPaginaRolDb(p,idRol);
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
        /// Elimina una relacion pagina rol
        /// </summary>
        public void EliminarPaginaRol(int idRol, int idPagina)
        {
            BeginTransaction();
            try
            {
                deletePaginaRolDb(idRol, idPagina);
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
        public void ModificarPaginaRol(Pagina p, int idRol)
        {
            BeginTransaction();
            try
            {
                updatePaginaRolDb(idRol, p.Idpagina, p.SoloLectura, p.ReingresaClave, p.PideAutorizacion, p.Restringido);
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
        /// Agrega un rol usuario
        /// </summary>
        /// <param name="idRol"></param>
        /// <param name="codOperador"></param>
        public void AgregarRolUsuario(int idRol, int codOperador)
        {
            BeginTransaction();
            try
            {
                insertRolUsuario(idRol, codOperador);
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
        /// Elimina un rol Usuario
        /// </summary>
        /// <param name="idRol"></param>
        /// <param name="codOperador"></param>
        public void EliminarRolUsuario(int idRol, int codOperador)        
        {
            BeginTransaction();
            try
            {
                deleteRolUsuario(idRol,codOperador);
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
        /// Modifica un rol. Solo los tiois basicos
        /// Se debe pasar un rol completo con todos las propiedades a modificar, 
        /// y tambien con las que no se modifican
        /// </summary>
        /// <param name="r"></param>
        public void ModificarRolABM(Rol r)
        {

            BeginTransaction();
            try
            {
                updataeRol(r);
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
        public void EliminarRol(int idRol)
        {
            BeginTransaction();
            try
            {
                DeleteRolDb(idRol);
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
        /// Obtiene un rol con su lista de paginas y Todavia Sin sus funcionalidades
        /// </summary>
        /// <param name="idRol"></param>
        /// <returns></returns>
        public Rol BuscarRolCompleto(int idRol)
        {
            BeginTransaction();
            try
            { 
                Rol rolRet = buscarRol(idRol);
                ControladorMenus c_menus = new ControladorMenus(conn);
                rolRet.ListPaginas = c_menus.BuscarListPaginas(idRol);
                CommitTransaction();
                return rolRet;
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
        /// Obtiene todos los roles. Solo con sus propiedades basicas.
        /// Si no encuentra nada devuelve excepcion propia
        /// </summary>
        /// <returns></returns>
        public List<Rol> BuscarListRoles()
        {
            BeginTransaction();
            try
            {
                DataTable dt = selectRoles();
                if (dt == null || dt.Rows.Count == 0)
                {
                    throw new ExcepcionPropia("No se han encontrado roles");
                }
                List<Rol> listR = new List<Rol>();
                foreach (DataRow row in dt.Rows)
                {
                    Rol r = buscarRol(Convert.ToInt32(row["idrol"]));
                    listR.Add(r);
                }
                CommitTransaction();
                return listR;
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
        /// Obtiene los roles asignados al usuario . Solo con sus propiedades basicas.
        /// Si no encuentra nada devuelve excepcion propia
        /// </summary>
        /// <param name="codOperador"></param>
        /// <returns></returns>
        public List<Rol> BuscarListRoles(int codOperador)
        {
            BeginTransaction();
            try
            {
                DataTable dt = selectRolesUsuarios(codOperador);
                if (dt == null || dt.Rows.Count == 0)
                {
                    throw new ExcepcionPropia("No se han encontrado roles para el usuario");
                }
                List<Rol> listR = new List<Rol>();
                foreach (DataRow row in dt.Rows)
                {
                    Rol r = buscarRol(Convert.ToInt32(row["idrol"]));
                    listR.Add(r);
                }
                CommitTransaction();
                return listR;
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
        private void insertSucursalPagina(int idSucursal, int idPagina)        
        {
            string sql = @"INSERT INTO sucursal_paginas
                            (
	                            idpagina,
	                            nro_sucursal
                            )
                            VALUES
                            (
	                            :idpagina,
	                            :nro_sucursal
                            )";
            conn.Execute(sql, idPagina,idSucursal);
        }
        private DataTable selectPaginasSinBloquear(int idSucursal)
        {
            string sql=@"SELECT DISTINCT * FROM paginas p 
                        WHERE p.idpagina NOT IN (SELECT sp.idpagina FROM sucursal_paginas sp WHERE sp.nro_sucursal=:p1)";
            return conn.GetDT(sql,idSucursal);
        }
        private DataTable selectSucursalesPaginas(int idSucursal)
        {
            string sql = "SELECT * FROM sucursal_paginas sp WHERE sp.nro_sucursal = :p1";
            return conn.GetDT(sql,idSucursal);
        }
        /// <summary>
        /// Obtiene la lista de paginas bloqueadas a la sucursal
        /// Si no encuentra nada devuelve excepcion propia
        /// </summary>
        /// <param name="idSucursal"></param>
        /// <returns></returns>
        public List<Pagina> BuscarListPaginasBloqueadas(int idSucursal)
        {
            BeginTransaction();
            try
            {
                DataTable dt = selectSucursalesPaginas(idSucursal);
                if (dt == null || dt.Rows.Count == 0)
                {
                    throw new ExcepcionPropia("La surcursal no posee paginas Restringidas");
                }
                List<Pagina> listP = new List<Pagina>();
                foreach (DataRow row in dt.Rows)
                {
                    ControladorMenus c_menu = new ControladorMenus(conn);
                    Pagina p = c_menu.BuscarPagina(Convert.ToInt32(row["idpagina"]));
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
        /// Obtiene la lista de paginas sin  bloqueadas a la sucursal
        /// Si no encuentra nada devuelve excepcion propia
        /// </summary>
        /// <param name="idSucursal"></param>
        /// <returns></returns>
        public List<Pagina> BuscarListPaginasSinBloquear(int idSucursal)
        {
            BeginTransaction();
            try
            {
                DataTable dt = selectPaginasSinBloquear(idSucursal);
                if (dt == null || dt.Rows.Count == 0)
                {
                    throw new ExcepcionPropia("No se han Encontrado Paginas");
                }
                List<Pagina> listP = new List<Pagina>();
                foreach (DataRow row in dt.Rows)
                {
                    ControladorMenus c_menu = new ControladorMenus(conn);
                    Pagina p = c_menu.BuscarPagina(Convert.ToInt32(row["idpagina"]));
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
            catch ( ExcepcionPropia myEx)
            {
                RollbackTransaction();
                ControladorExcepcion.tiraExcepcion(myEx.Message);
                return null;
            }
        }
        
        /// <summary>
        /// Agrega una sucursalPagina
        /// </summary>
        /// <param name="idPagina"></param>
        /// <param name="idSucursal"></param>
        public void AgregarSucursalPagina(int idPagina, int idSucursal)
        {
            BeginTransaction();
            try
            {
                insertSucursalPagina(idSucursal, idPagina);
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
        /// Elimina una pagina surcusal
        /// </summary>
        /// <param name="idPagina"></param>
        /// <param name="idSucursal"></param>
        public void EliminarSucursalPagina(int idPagina, int idSucursal)
        {
            BeginTransaction();
            try
            {
                deletePaginasSucursales(idPagina, idSucursal);
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
