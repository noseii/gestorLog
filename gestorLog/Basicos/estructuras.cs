using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gestorLog.Basicos
{
    public enum Modos
    {
        Completo = 0,
        Fichero = 1,
        Sistema = 2
    }

    public enum Niveles
    {
        Depuracion = 0,
        Informacion = 1,
        Advertencia = 2,
        Error = 3,
        Fatal = 4,
        Completo = 5,
        Ninguno = 6
    }
}
