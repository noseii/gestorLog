using NUnit.Framework;
using gestorLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gestorLog.Tests
{
    [TestFixture()]
    public class gestorLogMain_Tests
    {
        [Test()]
        public void grabarFichero_Test()
        {
            bool bRespuesta = false;
            KeyValuePair<string, string> strError = new KeyValuePair<string, string>();

            string path = (System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase.ToString()).Remove(0, 6)) + "\\prueba_text.log";
            string mydocpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            gestorLogMain gestorLog = new gestorLogMain("\\prueba_text.log", mydocpath);

            Console.WriteLine(path);
            Console.WriteLine(System.Reflection.Assembly.GetExecutingAssembly().Location);
            Console.WriteLine(mydocpath);

            // bool bRespuesta = gestorLog.Ficheros("Esto es una prueba", Basicos.Niveles.Ninguno, out strError);

            Console.WriteLine(strError.Value);

            Assert.True(bRespuesta);
        }

        [Test()]
        public void Sucessos_Test()
        {
            bool bRespuesta = false;
            KeyValuePair<string, string> strError = new KeyValuePair<string, string>();

            gestorLogMain gestorLog = new gestorLogMain(Basicos.Modos.Sistema);

            // bRespuesta = gestorLog.Sucessos("Esto es un mensaje de prueba", Basicos.Niveles.Error, out strError);

            Assert.True(bRespuesta);
        }

        [Test()]
        public void publicarLog_Test()
        {
            bool bRespuesta = false;
            KeyValuePair<string, string> strError = new KeyValuePair<string, string>();

            string path = (System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase.ToString()).Remove(0, 6)) + "\\prueba_text.log";
            string mydocpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            gestorLogMain gestorLog = new gestorLogMain("\\prueba_text_1.log", mydocpath);

            bRespuesta = gestorLog.publicarLog("Esto es una prueba", Basicos.Niveles.Error, out strError);

            Console.WriteLine(strError.Value);

            Assert.True(bRespuesta);
        }

        [Test()]
        public void publicarLog_Test1()
        {
            bool bRespuesta = false;
            KeyValuePair<string, string> strError = new KeyValuePair<string, string>();

            gestorLogMain gestorLog = new gestorLogMain(Basicos.Modos.Sistema);
            gestorLog.Aplicacion = "Prueba";
            gestorLog.IdEvento = 999;
            gestorLog.CategoriaEvento = 1;

            bRespuesta = gestorLog.publicarLog("Esto es un mensaje de prueba", Basicos.Niveles.Error, out strError);

            Console.WriteLine(strError.Value);

            Assert.True(bRespuesta);
        }
    }
}