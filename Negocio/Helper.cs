using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Negocio;
using Entidades;
using System.Data;

namespace Negocio
{
    public class Helper
    {
        public static string SHA1(string value)
        {
            var hash = System.Security.Cryptography.SHA1.Create();
            var encoder = new System.Text.ASCIIEncoding();
            var combined = encoder.GetBytes(value ?? "");
            return BitConverter.ToString(hash.ComputeHash(combined)).ToLower().Replace("-", "");
        }
        public static string GenerarContrasena(int PasswordLength)
        {
            string _allowedChars = "abcdefghijkmnpqrstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ23456789!@$?";
            Byte[] randomBytes = new Byte[PasswordLength];
            char[] chars = new char[PasswordLength];
            int allowedCharCount = _allowedChars.Length;

            for (int i = 0; i < PasswordLength; i++)
            {
                Random randomObj = new Random();
                randomObj.NextBytes(randomBytes);
                chars[i] = _allowedChars[(int)randomBytes[i] % allowedCharCount];
            }

            return new string(chars);
        }
        public static DateTime ObtenerPrimerDiaDelMes(DateTime fecha)
        {
            DateTime fechaAdevolver = new DateTime(fecha.Year, fecha.Month, 1);
            return fechaAdevolver;
        }
        public static DateTime ObtenerUltimoDiaDelMes(DateTime fecha)
        {
            DateTime fechaAdevolver = new DateTime(fecha.Year, fecha.Month, 1).AddMonths(1).AddDays(-1);
            return fechaAdevolver;
        }
        public static DateTime ObtenerPrimerDiaDelAno(DateTime fecha)
        {
            DateTime fechaAdevolver = new DateTime(fecha.Year, 1, 1);
            return fechaAdevolver;
        }
        public static DateTime ObtenerUltimoDiaDelAno(DateTime fecha)
        {
            DateTime fechaAdevolver = new DateTime(fecha.Year, 12, 31);
            return fechaAdevolver;
        }
        public static string ObtenerPeriodoAnterior(string periodoActual)
        {
            string ano = periodoActual.Substring(0, 4);
            string mes = periodoActual.Substring(4, 2);
            DateTime fechaPeriodoAnterior = new DateTime(Convert.ToInt32(ano), Convert.ToInt32(mes), 1).AddMonths(-1);
            string anoAnterior = fechaPeriodoAnterior.ToShortDateString().Substring(6, 4);
            string mesAnterior = fechaPeriodoAnterior.ToShortDateString().Substring(3, 2);
            string periodoAnterior = anoAnterior + mesAnterior;
            return periodoAnterior;
        }
        public static string ObtenerPeriodoPosterior(string periodoActual)
        {
            string ano = periodoActual.Substring(0, 4);
            string mes = periodoActual.Substring(4, 2);
            DateTime fechaPeriodoAnterior = new DateTime(Convert.ToInt32(ano), Convert.ToInt32(mes), 1).AddMonths(1);
            string anoPosterior = fechaPeriodoAnterior.ToShortDateString().Substring(6, 4);
            string mesPosterior = fechaPeriodoAnterior.ToShortDateString().Substring(3, 2);
            string periodoAnterior = anoPosterior + mesPosterior;
            return periodoAnterior;
        }
        public static string ObtenerPeriodoActual(DateTime fechaActual)
        {
            string anoActual = fechaActual.ToShortDateString().Substring(6, 4);
            string mesActual = fechaActual.ToString().Substring(3, 2);
            string periodoActual = anoActual + mesActual;
            return periodoActual;
        }
      


        /// <summary>
        /// Devuelve el numero de comprobante Formateado (PRADO)
        /// </summary>
        /// <param name="numero"></param>
        /// <param name="letra"></param>
        /// <param name="puntoVenta"></param>
        /// <returns></returns>
        public static string FormatearNumeroComprobante(int numero, string letra, int puntoVenta)
        {
           return letra + "-" + puntoVenta.ToString("0000") + "-" + numero.ToString("00000000");
        }
        /// <summary>
        /// A partir de un numero de comprobante comppleto, devuelve solo el numero como entero
        /// </summary>
        /// <param name="nroComprobante"></param>
        /// <returns></returns>
        public static int ObtenerNumeroComprobanteInteger(string nroComprobante)
        {
            int soloNumero = Convert.ToInt32(nroComprobante.Substring(7, 8));
            return soloNumero;
        }

        public static string SepararPalabraCamello(string titulo)
        {
            try
            {
                Char[] chart = titulo.ToCharArray();
                int indice = 0;
                string nuevoTitulo = "";
                foreach (Char c in chart)
                {
                    if (indice > 0)
                    {
                        if (c >= 'A' && c <= 'Z')
                        {
                            nuevoTitulo = nuevoTitulo + " " + c;
                        }
                        else
                        {
                            nuevoTitulo = nuevoTitulo + c;
                        }
                    }
                    else
                    {
                        nuevoTitulo = nuevoTitulo + c;
                    }
                    indice = indice + 1;
                }
                return nuevoTitulo;
            }
            ///Por si paso carateres especiales
            catch (ArgumentOutOfRangeException)
            {
                return titulo; 
            }
           
        }
        public static DateTime FechaHoraInicial(DateTime f)
        {
            DateTime fecha = new DateTime(f.Year, f.Month, f.Day, 0, 0, 0);
            return fecha;
        }
        /// <summary>
        /// Devuelve misma fecha con hora 23.59
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static DateTime FechaHoraFinal(DateTime f)
        {
            DateTime fecha = new DateTime(f.Year, f.Month, f.Day, 23, 59, 59);
            return fecha;
        }
    }
}
