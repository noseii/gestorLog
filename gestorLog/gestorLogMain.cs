using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Ficheros
using System.IO;
// Sucesos
using System.Diagnostics;


namespace gestorLog
{
    public class gestorLogMain
    {
        #region Variables / Constantes globales
        internal string nombreFichero = string.Empty;
        internal string rutaFichero = string.Empty;
        internal Basicos.Modos modo = Basicos.Modos.Completo;

        /// =============================================================================================== ///
        /// Para que funcione la publicación en el visor de eventos, hay que crear la siguiente             ///
        /// clave en el registro:                                                                           ///
        /// HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\EventLog\Application\Prueba                ///
        /// Donde Prueba es el nombre de origen en el visor de eventos                                      ///
        /// =============================================================================================== ///
        // Mismo nombre que la entrada que hemos creado en el registro
        internal const string APLICACION = "Prueba";
        #endregion Fin --> Variables globales

        #region Constructores
        public gestorLogMain(Basicos.Modos _modo)
        {
            modo = _modo;
        }

        public gestorLogMain(string _fichero, string _rutaFichero)
        {
            nombreFichero = _fichero;
            rutaFichero = _rutaFichero;
            modo = Basicos.Modos.Fichero;
        }

        public gestorLogMain(string _fichero, string _rutaFichero, Basicos.Modos _modo)
        {
            nombreFichero = _fichero;
            rutaFichero = _rutaFichero;
            modo = _modo;
        }
        #endregion Fin --> Constructores

        #region Propiedades
        public string NombreFichero
        {
            get { return nombreFichero; }
            set { nombreFichero = value; }
        }

        public string RutaFichero
        {
            get { return rutaFichero; }
            set { rutaFichero = value; }
        }

        public Basicos.Modos Modo
        {
            get { return modo; }
            set { modo = value; }
        }
        #endregion Fin --> Propiedades


        internal string formatoMensaje(string _mensaje, Basicos.Niveles _nivel)
        {
            // Variables gestion respuesta
            string strRespuesta = string.Empty;

            // Variables internas
            string mensaje = _mensaje;
            Basicos.Niveles nivel = _nivel;
            string strNivel = string.Empty;
            string strFechaHora = string.Empty;

            // Crear el mensaje
            if (nivel == Basicos.Niveles.Depuracion)
            {
                strNivel = "[D]";
            }
            else if (nivel == Basicos.Niveles.Informacion)
            {
                strNivel = "[+]";
            }
            else if (nivel == Basicos.Niveles.Advertencia)
            {
                strNivel = "[!]";
            }
            else if (nivel == Basicos.Niveles.Error)
            {
                strNivel = "[-]";
            }
            else if (nivel == Basicos.Niveles.Fatal)
            {
                strNivel = "[*]";
            }
            // No comple...
            else
            {
                strNivel = "[?]";
            }

            // Formatear fecha y hora
            DateTime dtFechaHora = DateTime.Now;
            strFechaHora = string.Format("{0:MM/dd/yyyy - HH:mm:ss}", dtFechaHora);

            // Generar mensaje
            strRespuesta = string.Format("{0} {1} --> {2}", strNivel, strFechaHora, mensaje);

            // Respuesta
            return strRespuesta;
        }

        #region Gestor de sucesos de Windows
        public bool Sucessos(string _mensaje, Basicos.Niveles _nivel, out KeyValuePair<string, string> _strError)
        {
            bool bRespuesta = false;

            bRespuesta = grabarSucesos(_mensaje, _nivel, out _strError);

            // Respuesta
            return bRespuesta;
        }

        internal bool grabarSucesos(string _mensaje, Basicos.Niveles _nivel, out KeyValuePair<string, string> _strError)
        {
            // Variables gestion respuesta
            bool bRespuesta = false;
            KeyValuePair<string, string> strError = new KeyValuePair<string, string>();


            // Variables internas
            string sSource;
            string mensaje = _mensaje;
            Basicos.Niveles nivel = _nivel;
            EventLogEntryType nivelEvento = new EventLogEntryType();
            // Establece el origen en el visor de eventos
            sSource = APLICACION;

            // Extraer nivel
            switch(nivel)
            {
                case Basicos.Niveles.Advertencia:
                    nivelEvento = EventLogEntryType.Warning;
                    break;
                case Basicos.Niveles.Completo:
                    nivelEvento = EventLogEntryType.Information;
                    break;
                case Basicos.Niveles.Depuracion:
                    nivelEvento = EventLogEntryType.Information;
                    break;
                case Basicos.Niveles.Error:
                    nivelEvento = EventLogEntryType.Error;
                    break;
                case Basicos.Niveles.Fatal:
                    nivelEvento = EventLogEntryType.Information;
                    break;
                case Basicos.Niveles.Informacion:
                    nivelEvento = EventLogEntryType.Information;
                    break;
                case Basicos.Niveles.Ninguno:
                    nivelEvento = EventLogEntryType.Information;
                    break;
                default:
                    nivelEvento = EventLogEntryType.Error;
                    break;
            }



            try
            {
                EventLog.WriteEntry(sSource, mensaje, nivelEvento, 01, 01);

                bRespuesta = true;
            }
            catch (Exception ex)
            {
                strError = new KeyValuePair<string, string>("ERROR_EVENTO", ex.Message);
                bRespuesta = false;
            }

            // Respuesta
            _strError = strError;
            return bRespuesta;
        }
        #endregion Fin - Gestor sucesos


        #region Grabar en fichero
        public bool Ficheros(string _mensaje, Basicos.Niveles _nivel, out KeyValuePair<string, string> _strError)
        {
            // Variables gestion respuesta
            bool bRespuesta = false;

            bRespuesta = grabarFichero(_mensaje, _nivel, out _strError);

            // Respuesta
            return bRespuesta;
        }
        
        internal bool grabarFichero(string _mensaje, Basicos.Niveles _nivel, out KeyValuePair<string, string> _strError)
        {
            // Variables para gestionar la respuesta
            bool bRespuesta = false;
            KeyValuePair<string, string> strError = new KeyValuePair<string, string>();

            // Variables internas
            string mensaje = _mensaje;
            Basicos.Niveles nivel = _nivel;
            string mensajeGrabar = string.Empty;
            string completoFichero = rutaFichero + nombreFichero;
            StreamWriter fichero;

            // Saber si el fichero existe
            if (File.Exists(completoFichero))
            {
                // Agregamos las lineas al fichero existente
                fichero = File.AppendText(completoFichero);
            }
            else
            {
                // El fichero no existe, lo creamos
                fichero = File.CreateText(completoFichero);
            }

            // formateo del mensaje
            mensajeGrabar = formatoMensaje(mensaje, nivel);

            // Grabar el mensaje en el fichero
            try
            {
                fichero.WriteLine(mensajeGrabar);
                bRespuesta = true;
            }
            catch (Exception ex)
            {
                strError = new KeyValuePair<string, string>("ERROR_NO_DEFINIDO", ex.Message);
                bRespuesta = false;
            }

            // Cerrar fichero
            fichero.Close();

            // Respuesta
            _strError = strError;
            return bRespuesta;
        }
        #endregion Fin --> Grabar en fichero
    }
}
