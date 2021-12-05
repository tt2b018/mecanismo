using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Mecanismo
{

    class Cifrador
    {
        private byte[] llaveAES;
        public Cifrador()
        {
            byte[] llavePrivada;
            byte[] llavePublicaAdmin;
            llavePrivada = File.ReadAllBytes(Service1.rutaLlavePrivada);
            ECDiffieHellmanCng ECD = new ECDiffieHellmanCng(CngKey.Import(llavePrivada, CngKeyBlobFormat.EccPrivateBlob));
            ECD.KeyDerivationFunction = ECDiffieHellmanKeyDerivationFunction.Hash;
            ECD.HashAlgorithm = CngAlgorithm.Sha256;
            llavePublicaAdmin = File.ReadAllBytes(Service1.rutaLlavePublicaAdmin);
            CngKey llaveAdmin = CngKey.Import(llavePublicaAdmin, CngKeyBlobFormat.EccPublicBlob);
            llaveAES = ECD.DeriveKeyMaterial(llaveAdmin);
        }

        public byte[] Cifrar(String texto)
        {
            Aes aes = new AesCryptoServiceProvider();
            aes.Key = llaveAES;
            MemoryStream ciphertext = new MemoryStream();
            CryptoStream cs = new CryptoStream(ciphertext, aes.CreateEncryptor(), CryptoStreamMode.Write);
            byte[] textoPlano = Encoding.UTF8.GetBytes(texto);
            cs.Write(textoPlano, 0, textoPlano.Length);
            cs.Dispose();
            cs.Close();
            byte[] textoCifrado = ciphertext.ToArray();
            ciphertext.Dispose();
            ciphertext.Close();
            byte[] resultado = new byte[160];
            Buffer.BlockCopy(textoCifrado, 0, resultado, 0, textoCifrado.Length);
            Buffer.BlockCopy(aes.IV, 0, resultado, textoCifrado.Length, aes.IV.Length);
            return resultado;
        }

    }
}
