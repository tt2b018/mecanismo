using F5;
using System;
using System.IO;

namespace Mecanismo
{
    class Incrustador
    {
        public void Incrustar()
        {
            while (true)
            {
                if(Service1.porProcesar.Count > 0)
                {
                    String s = (String)Service1.porProcesar[0];
                    byte[] texto = Extract.Run(s);
                    if (texto == null || texto.Length == 0 || (texto.Length != 161 && texto.Length != 321)) ProcesarImagen(s, new byte[] { 0 });
                    else if (texto[0] == 0)
                    {
                        texto[0] = 1;
                        ProcesarImagen(s, texto);
                    }
                }
            }
        }

        private static void ProcesarImagen(String path, byte[] texto)
        {
            FileInfo file = new FileInfo(path);
            while (Service1.IsFileLocked(file)) ;
            Embed.Run(@path, texto);
            Service1.porProcesar.Remove(path);
            Observador.checando.Remove(path);
        }

    }
}
