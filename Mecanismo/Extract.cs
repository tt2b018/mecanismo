using Mecanismo;
using System;
using System.IO;
using System.Threading;

namespace F5
{
    public static class Extract
    {
        public static byte[] Run(String path)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                JpegExtract extractor = new JpegExtract("h$-aZ+crufe@L0$9*wi3");
                FileInfo file = new FileInfo(path);
                while (Service1.IsFileLocked(file)) ;
                Thread.Sleep(1000);
                using (Stream s = extractor.Extract(File.OpenRead(path)))
                {
                    if (s != null)
                    {
                        s.Position = 0;
                        s.CopyTo(ms);
                        byte[] texto = ms.ToArray();
                        s.Close();
                        s.Dispose();
                        ms.Close();
                        ms.Dispose();
                        return texto;
                    }
                    ms.Close();
                    ms.Dispose();
                    return null;
                }
            }
        }
    }
}
