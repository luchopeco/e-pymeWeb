using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Datos;
using System.Data;

namespace Entidades
{
    [Serializable]
    public class TipoMovimientoCaja
    {
        public TipoMovimientoCaja()
        { }
        public TipoMovimientoCaja(int idTipoMov)
        {
            string sql = @"SELECT * FROM tipo_movimiento_caja tmc WHERE tmc.idtipo_movimiento=:p1";
            DataTable dt;
            using (Connection conn = new Connection())
            {
                conn.Open();
                dt = conn.GetDT(sql, idTipoMov);
            }
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                IdtipoMovimiento = Convert.ToInt32(row["idtipo_movimiento"]);
                this.EsSuma = Convert.ToBoolean(row["es_suma"]);
                this.FechaBaja = row["fecha_baja"] as DateTime?;
                this.Nombre = row["nombre"].ToString();

            }
        }
        // Referencia: tipo_movimiento_caja.idtipo_movimiento
        public System.Int32 IdtipoMovimiento { get; set; }
        // Referencia: tipo_movimiento_caja.nombre
        public System.String Nombre { get; set; }
        // Referencia: tipo_movimiento_caja.es_suma
        public System.Boolean EsSuma { get; set; }
        // Referencia: tipo_movimiento_caja.fecha_baja
        public System.DateTime? FechaBaja { get; set; }


        public static int TipoMovimientoIngreso = 2;
        public static int TipoMovimientoRetiro = 1;
    }
}
