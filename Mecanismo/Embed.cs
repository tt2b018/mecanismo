using System;
using System.Drawing;
using System.IO;

namespace F5
{
    using F5.James;
    using Mecanismo;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading;

    public static class Embed
    {
        public static void Run(String path, byte[] texto)
        { 
            Image imaget = Image.FromFile(path);
            using (Stream s = new MemoryStream())
            {
                imaget.Save(s, System.Drawing.Imaging.ImageFormat.Jpeg);
                imaget.Dispose();
                File.Delete(path);
                Image image = Image.FromStream(s);
                JpegEncoder jpg = new JpegEncoder(image, File.OpenWrite(path), String.Empty, 80);
                String informacion = DateTime.Now.ToString("ddMMyyyyHHmmss") + Service1.informacionIdentificadora;
                String hash = Service1.HashString(informacion);
                Cifrador c = new Cifrador();
                byte[] infoUsuario = c.Cifrar(informacion + hash);
                byte[] informacionIncrustar = new byte[texto.Length + infoUsuario.Length];
                Buffer.BlockCopy(texto, 0, informacionIncrustar, 0, texto.Length);
                Buffer.BlockCopy(infoUsuario, 0, informacionIncrustar, texto.Length, infoUsuario.Length);
                using (MemoryStream ms = new MemoryStream(informacionIncrustar))
                {
                    jpg.Compress(ms, "h$-aZ+crufe@L0$9*wi3");
                    var File2 = new FileInfo(path);
                    while (Service1.IsFileLocked(File2)) ;
                    Service1.logHash[Service1.logPath.IndexOf(path)] = Service1.Hash(path);
                    ms.Close();
                    ms.Dispose();
                    s.Close();
                    s.Dispose();
                }
            }
        }

    }
}
