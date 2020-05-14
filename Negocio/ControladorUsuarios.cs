using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Datos;
using Entidades;


namespace Negocio
{
    public class ControladorUsuarios : ControladorGenerico
    {
        #region Constructores
        /// <summary>
        /// Utilizar siempre el using puesto q maneja la conexion
        /// </summary>
        public ControladorUsuarios()
            : base()
        {



            // _logger = LogManager.GetLogger(this.GetType());//aquí procedemos a inicializar el objeto log.}

        }
        /// <summary>
        /// Para Utilizar sin using. Libera todo el garbage Collector (Espero)
        /// No maneja la coneccion
        /// </summary>
        public ControladorUsuarios(bool sinConexion)
            : base(sinConexion)
        {

        }
        /// <summary>
        /// Constructor al cual le paso la coneccion para no utilizar la del Controlador actua si no otra
        /// </summary>
        public ControladorUsuarios(Connection c)
            : base(c)
        {
        }
        #endregion
        private int insertUsuario(Usuario u)
        {
            string sql = @"INSERT INTO usuario
                    (
	                    nombre_usuario,
	                    nombre_apellido,
	                    clave,
	                    fecha_baja
                    )
                    VALUES
                    (
	                    :nombre_usuario,
	                    :nombre_apellido,
	                    :clave,
	                    :fecha_baja
                    )";
            conn.Execute(sql, u.NombreUsuario, u.NombreApellido, u.Clave, u.FechaBaja);
            return conn.LastInsertedId("usuario_idusuario_seq");
        }
        private void updateUsuarioSU(int idUsu, bool esSu)
        {
            string sql = @"UPDATE usuario 
                            SET
	                    es_super_usuario = :P1
                    WHERE idusuario=:P2";
            conn.Execute(sql, esSu, idUsu);
        }
        private void updateUsuarioClave(int idUsuario, string clave)
        {
            string sql = @"UPDATE usuario
                        SET
	                        clave = :p1
                        WHERE idusuario = :p2";
            conn.Execute(sql, clave, idUsuario);
        }
        private DataTable selectUsuario()
        {
            string sql = "SELECT * FROM usuario u";
            return conn.GetDT(sql);
        }
        private DataTable selectUsuario(int idUsuario)
        {
            string sql = "SELECT * FROM usuario u WHERE u.idusuario=:p2";
            return conn.GetDT(sql, idUsuario);
        }
        private DataTable selectUsuario(string usuario, string clave)
        {
            string sql = @"SELECT * FROM usuario u WHERE u.nombre_usuario= :p2 AND u.clave=:p3";
            return conn.GetDT(sql, usuario, clave);
        }
        /// <summary>
        /// Busca una usuario.
        /// Si no lo encuentra devuele excepcion
        /// </summary>
        /// <returns></returns>
        private Usuario buscarUsuario(string usuario, string clave)
        {
            DataTable dt = selectUsuario(usuario, clave);
            if (dt == null || dt.Rows.Count == 0)
            {
                throw new ExcepcionPropia("Usuario o Clave incorrecta");
            }
            DataRow row = dt.Rows[0];
            Usuario u = mapearUsuario(row);
            return u;
        }

        private static Usuario mapearUsuario(DataRow row)
        {
            Usuario u = new Usuario();
            u.Clave = row["clave"].ToString();
            u.Idusuario = Convert.ToInt32(row["idusuario"]);
            u.NombreUsuario = row["nombre_usuario"].ToString();
            u.NombreApellido = row["nombre_apellido"].ToString();
            u.EsSuperUsuario = Convert.ToBoolean(row["es_super_usuario"]);
            u.FechaBaja = row["fecha_baja"] as DateTime?;
            u.Imagen = row["imagen"].ToString();
            return u;
        }
        /// <summary>
        /// Busca una usuario.
        /// Si no lo encuentra devuele excepcion
        /// </summary>
        /// <returns></returns>
        public Usuario BuscarUsuario(string usuario, string clave)
        {
            BeginTransaction();
            try
            {
                Usuario u = buscarUsuario(usuario, clave);
                CommitTransaction();
                return u;
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
        /// Obtiene un usuario si no tira excepcion
        /// </summary>
        /// <param name="idUsaurio"></param>
        /// <returns></returns>
        public Usuario BuscarUsuario(int idUsaurio)
        {
            
            try
            {
                DataTable dt = selectUsuario(idUsaurio);
                if (dt == null || dt.Rows.Count == 0)
                {
                    throw new ExcepcionPropia("No se ha encontrado un usuario");
                }
                DataRow row = dt.Rows[0];
                Usuario u = mapearUsuario(row);
            
                return u;
            }
            catch (Npgsql.NpgsqlException ex)
            {
             
                ControladorExcepcion.tiraExcepcion(ex);
                return null;
            }
            catch (ExcepcionPropia my)
            {
              
                ControladorExcepcion.tiraExcepcion(my.Message);
                return null;
            }
        }
        /// <summary>
        /// Modifica el bool es su del usuario
        /// </summary>
        /// <param name="idUsaurio"></param>
        /// <returns></returns>
        public void ModificarUsuarioEsSU(int idUsaurio, bool esSU)
        {
            BeginTransaction();
            try
            {
                updateUsuarioSU(idUsaurio, esSU);
                CommitTransaction();

            }
            catch (Npgsql.NpgsqlException ex)
            {
                RollbackTransaction();
                ControladorExcepcion.tiraExcepcion(ex);

            }
            catch (ExcepcionPropia my)
            {
                RollbackTransaction();
                ControladorExcepcion.tiraExcepcion(my.Message);

            }
        }

        public void ModificarUsuarioImagen(int idUsuario, string imagen)
        {
            BeginTransaction();
            try
            {
                string sql = @"UPDATE usuario
                                SET	
	                                imagen = :p2
                                WHERE idusuario=:p1";
                conn.Execute(sql, imagen, idUsuario);
                CommitTransaction();
            }
            catch (Npgsql.NpgsqlException ex)
            {
                RollbackTransaction();
                ControladorExcepcion.tiraExcepcion(ex);
            }
        }

        public void ModificarUsuario(Usuario u)
        {
            BeginTransaction();
            try
            {
                if (u.Clave != string.Empty)
                {
                    updateUsuarioClave(u.Idusuario, u.Clave);
                }
                string sql = @"UPDATE usuario
                                SET
	                                nombre_usuario = :p4,
	                                nombre_apellido = :p3,
	                                fecha_baja = :p2
                                WHERE idusuario=:p1";
                conn.Execute(sql, u.NombreUsuario, u.NombreApellido, u.FechaBaja, u.Idusuario);
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
        public List<Usuario> BuscarListUsuario()
        {
            BeginTransaction();
            try
            {
                DataTable dt = selectUsuario();
                if (dt == null || dt.Rows.Count == 0)
                {
                    throw new ExcepcionPropia("No se han encontrado Usuarios");
                }
                List<Usuario> listU = new List<Usuario>();
                foreach (DataRow row in dt.Rows)
                {
                    Usuario u = mapearUsuario(row);
                    listU.Add(u);
                }
                CommitTransaction();
                return listU.OrderBy(u => u.NombreUsuario).ToList();
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

        public int AgregarUsuario(Usuario u)
        {
            BeginTransaction();
            try
            {
                if (u.NombreApellido == string.Empty)
                {
                    throw new ExcepcionPropia("Debe ingresar un Nombre y Apellido");
                }
                if (u.NombreUsuario == string.Empty)
                {
                    throw new ExcepcionPropia("Debe ingresar un Nombre de usuario");
                }
                if (u.Clave == string.Empty)
                {
                    throw new ExcepcionPropia("Debe ingresar una Clave");
                }
                int id = insertUsuario(u);
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
