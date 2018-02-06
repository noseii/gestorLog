using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
// Sucesos
using System.Diagnostics;


namespace pruebas
{
    class Program
    {
        static void Main(string[] args)
        {
            /* Publicar log en fichero
            StreamWriter fichero;
            string nombreFichero = string.Empty;
            string mensaje = "Esto es una prueba ...";
            string mensajeGrabar = string.Empty;
            string strNivel = string.Empty;
            string strFechaHora = string.Empty;

            nombreFichero = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\prueba_text.log";

            // Saber si el fichero existe
            if (File.Exists(nombreFichero))
            {
                // Agregamos las lineas al fichero existente
                fichero = File.AppendText(nombreFichero);
            }
            else
            {
                // El fichero no existe, lo creamos
                fichero = File.CreateText(nombreFichero);
            }


            // Crear el mensaje
            strNivel = "[+]";

            // Formatear fecha y hora
            DateTime dtFechaHora = DateTime.Now;
            strFechaHora = string.Format("{0:MM/dd/yyyy - HH:mm:ss}", dtFechaHora);

            // Generar mensaje
            mensajeGrabar = string.Format("{0} {1} --> {2}", strNivel, strFechaHora, mensaje);
            Console.WriteLine(mensajeGrabar);

            fichero.WriteLine(mensajeGrabar);

            fichero.Close();
            Fin --> Publicar log en fichero */

            // Publicar log en visor de sucesos / eventos
            // Variables internas


            Console.WriteLine("Pulsa una tecla para Salir ...");
            Console.ReadKey();
        }
    }
}