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
        internal string Prueba = string.Empty;

        internal string rutaFichero = string.Empty;
        internal Basicos.Modos modo = Basicos.Modos.Completo;
        internal int idEvento = 666;
        internal Int16 categoria = 0x00;

        /// =============================================================================================== ///
        /// Para que funcione la publicación en el visor de eventos, hay que crear la siguiente             ///
        /// clave en el registro:                                                                           ///
        /// HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\EventLog\Application\Prueba                ///
        /// Donde Prueba es el nombre de origen en el visor de eventos                                      ///
        /// =============================================================================================== ///
        // Mismo nombre que la entrada que hemos creado en el registro
        internal const string APLICACION = "Application";
        internal string aplicacion = string.Empty;
        #endregion Fin --> Variables globales

        #region Constructores
        /// <summary>
        /// Constructor, para el manejo de sucesos  
        /// </summary>
        /// <param name="_modo"></param>
        public gestorLogMain(Basicos.Modos _modo)
        {
            modo = _modo;
        }
        /// <summary>
        /// Constructor, para el manejo de fichero 
        /// </summary>
        /// <param name="_fichero"></param>
        /// <param name="_rutaFichero"></param>
        public gestorLogMain(string _fichero, string _rutaFichero)
        {
            nombreFichero = _fichero;
            rutaFichero = _rutaFichero;
            modo = Basicos.Modos.Fichero;
        }
        /// <summary>
        /// Constructor general
        /// </summary>
        /// <param name="_fichero"></param>
        /// <param name="_rutaFichero"></param>
        /// <param name="_modo"></param>
        public gestorLogMain(string _fichero, string _rutaFichero, Basicos.Modos _modo)
        {
            nombreFichero = _fichero;
            rutaFichero = _rutaFichero;
            modo = _modo;
        }
        #endregion Fin --> Constructores

        #region Propiedades
        /// <summary>
        /// NOmbre del fichero de Log
        /// </summary>
        public string NombreFichero
        {
            get { return nombreFichero; }
            set { nombreFichero = value; }
        }
        /// <summary>
        /// Ruta donde se encuentra el fichero de Log
        /// </summary>
        public string RutaFichero
        {
            get { return rutaFichero; }
            set { rutaFichero = value; }
        }
        /// <summary>
        /// Modos de importancia del mensaje
        /// </summary>
        public Basicos.Modos Modo
        {
            get { return modo; }
            set { modo = value; }
        }
        /// <summary>
        /// Id del evento, solo visor de eventos
        /// </summary>
        public int IdEvento
        {
            get { return idEvento; }
            set { idEvento = value; }
        }
        /// <summary>
        /// Categoria del evento, solo visor de eventos
        /// </summary>
        public Int16 CategoriaEvento
        {
            get { return categoria; }
            set { categoria = value; }
        }

        public string Aplicacion
        {
            get { return aplicacion; }
            set { aplicacion = value; }
        }
        #endregion Fin --> Propiedades

        #region Formateo del mensaje
        /// <summary>
        /// Formateo del texto del mensale
        /// </summary>
        /// <param name="_mensaje">Mensaje a publicar</param>
        /// <param name="_nivel">Nivel de advertencias</param>
        /// <returns></returns>
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
            strFechaHora = string.Format("{0:dd/MM/yyyy - HH:mm:ss}", dtFechaHora);

            // Generar mensaje
            strRespuesta = string.Format("{0} {1} --> {2}", strNivel, strFechaHora, mensaje);

            // Respuesta
            return strRespuesta;
        }
        #endregion Fin --> Formateo mensaje

        #region Gestor de sucesos de Windows
        /// <summary>
        /// Para verificar que la publicación en el visor de sucesos funciona
        /// Quitar al final
        /// </summary>
        /// <param name="_mensaje"></param>
        /// <param name="_nivel"></param>
        /// <param name="_strError"></param>
        /// <returns></returns>
        // public bool Sucessos(string _mensaje, Basicos.Niveles _nivel, out KeyValuePair<string, string> _strError)
        // {
        //     bool bRespuesta = false;
        // 
        //     bRespuesta = grabarSucesos(_mensaje, _nivel, out _strError);
        // 
        //     // Respuesta
        //     return bRespuesta;
        // }
        /// <summary>
        /// Función para publicar en el gestor de eventos
        /// </summary>
        /// <param name="_mensaje">Mensaje que vamos a publicar</param>
        /// <param name="_nivel">Nivel de importancia del mensaje</param>
        /// <param name="_strError">Si se produce un error, devuelve la descripción de el</param>
        /// <returns>True --> ejecución correcta, False --> Se ha producido un error en la ejecución</returns>
        /// <remarks>Para que funcione bien tiene que estar creada la clave en el registro.
        /// Para que funcione la publicación en el visor de eventos, hay que crear la siguiente clave en el registro:
        /// HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\EventLog\Application\Prueba
        /// Donde "Prueba" es el nombre de origen en el visor de eventos</remarks>
        internal bool publicarSucesos(string _mensaje, Basicos.Niveles _nivel, out KeyValuePair<string, string> _strError)
        {
            // Variables gestion respuesta
            bool bRespuesta = false;
            KeyValuePair<string, string> strError = new KeyValuePair<string, string>();

            // Variables internas
            string sSource;
            string mensaje = _mensaje;
            string mensajeGrabar = string.Empty;
            Basicos.Niveles nivel = _nivel;
            EventLogEntryType nivelEvento = new EventLogEntryType();
            // Establece el origen en el visor de eventos
            if (string.IsNullOrEmpty(aplicacion))
            {
                sSource = APLICACION;
            }
            else
            {
                sSource = aplicacion;
            }
            

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

            // Formatear mensaje
            mensajeGrabar = formatoMensaje(mensaje, nivel);

            try
            {
                EventLog.WriteEntry(sSource, mensajeGrabar, nivelEvento, idEvento, categoria);
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
        /// <summary>
        /// Función para la pruebas unitarias
        /// Quitar al final
        /// </summary>
        /// <param name="_mensaje"></param>
        /// <param name="_nivel"></param>
        /// <param name="_strError"></param>
        /// <returns></returns>
        // public bool Ficheros(string _mensaje, Basicos.Niveles _nivel, out KeyValuePair<string, string> _strError)
        // {
        //     // Variables gestion respuesta
        //     bool bRespuesta = false;
        // 
        //     bRespuesta = grabarFichero(_mensaje, _nivel, out _strError);
        // 
        //     // Respuesta
        //     return bRespuesta;
        // }
        /// <summary>
        /// Función que escribe en un fichero de log, los mensajes
        /// </summary>
        /// <param name="_mensaje">Mensaje que deseamos publicar</param>
        /// <param name="_nivel">Nivel de imporancia del mensaje</param>
        /// <param name="_strError">Si se produce un error, devuelve la descripción de el</param>
        /// <returns>True --> ejecución correcta, False --> Se ha producido un error en la ejecución</returns>
        internal bool publicarFichero(string _mensaje, Basicos.Niveles _nivel, out KeyValuePair<string, string> _strError)
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

        #region Publicar mensaje Log y Evento
        public bool publicarLog(string _mensaje, Basicos.Niveles _nivel, out KeyValuePair<string, string> _strError)
        {
            // Variables para procesar la respuesta
            bool bRespuesta = false;
            KeyValuePair<string, string> strError = new KeyValuePair<string, string>();

            // Variables internas
            string mensaje = _mensaje;
            Basicos.Niveles nivel = _nivel;

            if ((modo != Basicos.Modos.Completo) && (modo != Basicos.Modos.Fichero) && (modo != Basicos.Modos.Sistema))
            {
                strError = new KeyValuePair<string, string>("ERROR_MODO_NO_VALIDO", "Error en el modo de publicación.");
                bRespuesta = false;
            }
            else
            {
                if ((modo == Basicos.Modos.Completo) || (modo == Basicos.Modos.Fichero))
                {
                    // Publicamos en el fichero
                    bRespuesta = publicarFichero(mensaje, nivel, out strError);
                }
                else
                {
                    // Nada
                }

                if ((modo == Basicos.Modos.Completo) || (modo == Basicos.Modos.Sistema))
                {
                    // Publicamos en el visor de sucesos
                    bRespuesta = publicarSucesos(mensaje, nivel, out strError);
                }
                else
                {
                    // Nada
                }
            }


            // Respuesta
            _strError = strError;
            return bRespuesta;
        }
        #endregion Fin --> Publicar
    }
}
