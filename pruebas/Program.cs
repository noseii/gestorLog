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

            /*
            string sSource;
            string sLog;
            string sEvent;

            sSource = "dotNET Sample App";
            sLog = "Application";
            sEvent = "Sample Event";

            if (!EventLog.SourceExists(sSource))
                EventLog.CreateEventSource(sSource, sLog);

            EventLog.WriteEntry(sSource, sEvent);
            EventLog.WriteEntry(sSource, sEvent,
            EventLogEntryType.Warning, 234);
            */

            // Obtiene los posibles logs en el equipo
            // EventLog[] remoteEventLogs;
            // // Gets logs on the local computer, gives remote computer name to get the logs on the remote computer.
            // remoteEventLogs = EventLog.GetEventLogs(System.Environment.MachineName);
            // Console.WriteLine("Number of logs on computer: " + remoteEventLogs.Length);
            // 
            // for (int i = 0; i < remoteEventLogs.Length; i++)
            //     Console.WriteLine("Log: " + remoteEventLogs[i].Log);
            // // Fin --> logs equipo

            // // Leer logs
            // //logType can be Application, Security, System or any other Custom Log.
            // string logType = "Application";
            // 
            // EventLog ev = new EventLog(logType, System.Environment.MachineName);
            // int LastLogToShow = ev.Entries.Count;
            // if (LastLogToShow <= 0)
            //     Console.WriteLine("No Event Logs in the Log :" + logType);
            // 
            // // Read the last 2 records in the specified log. 
            // int i;
            // for (i = ev.Entries.Count - 1; i >= LastLogToShow - 2; i--)
            // {
            //     EventLogEntry CurrentEntry = ev.Entries[i];
            //     Console.WriteLine("Event ID : " + CurrentEntry.EventID);
            //     Console.WriteLine("Entry Type : " + CurrentEntry.EntryType.ToString());
            //     Console.WriteLine("Message :  " + CurrentEntry.Message + "\n");
            // }
            // ev.Close();

            //See if the source exists. 
            // if (!(EventLog.SourceExists("MySystemSource", System.Environment.MachineName)))
            //     EventLog.CreateEventSource("MySystemSource", "System", System.Environment.MachineName);
            // 
            // EventLog ev = new EventLog("System", System.Environment.MachineName, "MySystemSource");
            // /* Writing to system log, in the similar way you can write to other 
            //  * logs that you have appropriate permissions to write to
            //  */
            // ev.WriteEntry("Warning is written to system Log", EventLogEntryType.Warning, 10001);
            // Console.WriteLine("Warning is written to System Log");
            // ev.Close();

            // Create the source, if it does not already exist.
            // if (!(EventLog.SourceExists("Application", System.Environment.MachineName)))
            //     EventLog.CreateEventSource("Application", "MyNewLog", System.Environment.MachineName);
            // Console.WriteLine("CreatingEventSource");

            /// =============================================================================================== ///
            /// Para que funcione hay que crear la siguiente clave en el registro                               ///
            /// HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\EventLog\Application\Prueba                ///
            /// Donde Prueba es el nombre de origen en el visor de eventos                                      ///
            /// =============================================================================================== ///




            // Create the source, if it does not already exist. 
            if (!EventLog.SourceExists("Prueba"))
            {
                //An event log source should not be created and immediately used. 
                //There is a latency time to enable the source, it should be created 
                //prior to executing the application that uses the source. 
                //Execute this sample a second time to use the new source. 
                EventLog.CreateEventSource("Prueba", "MyNewLog");
                Console.WriteLine("CreatedEventSource");
                Console.WriteLine("Exiting, execute the application a second time to use the source.");
                // The source is created.  Exit the application to allow it to be registered. 
                return;
            }

            // Create an EventLog instance and assign its source. 
            EventLog myLog = new EventLog();
            
            myLog.Source = "Prueba";

            // Write an informational entry to the event log.     
            myLog.WriteEntry("Este es mi texto para el visor de eventos.");
            myLog.WriteEntry("Texto de prueba, error", EventLogEntryType.Error);
            myLog.WriteEntry("Texto de prueba, information, id: 666", EventLogEntryType.Information, 666);
            myLog.WriteEntry("Texto de prueba, SuccessAudit, id: 666, categoria: 10", EventLogEntryType.SuccessAudit, 666, 10);

            //Create a byte array for binary data to associate with the entry.
            byte[] myByte = { 69, 83, 84, 79, 32, 69, 83, 20, 85, 78, 65, 32, 80, 82, 85, 69, 66, 65 }; // ESTO ES UNA PRUEBA
            //Populate the byte array with simulated data.
            // for (int i = 0; i < 10; i++)
            // {
            //     myByte[i] = (byte)(i % 2);
            // }
            System.Text.ASCIIEncoding codificador = new System.Text.ASCIIEncoding();
            myByte = codificador.GetBytes("Esto es una prueba !!!");
            myLog.WriteEntry("Texto de prueba, information, id: 666, categoria: 10, con Byte[]", EventLogEntryType.Warning, 666, 10, myByte);


            EventLogPermission permisos = new EventLogPermission();
            permisos.Assert();
            myLog.BeginInit();



            Console.WriteLine("Pulsa una tecla para Salir ...");
            Console.ReadKey();
        }
    }
}