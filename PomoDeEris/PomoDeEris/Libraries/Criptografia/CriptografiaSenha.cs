using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation; 

namespace PomoDeEris.Libraries.Criptografia
{
    public class CriptografiaSenha
    {
        // COMO FUNCIONA A CRIPTOGRAFIA
        // Misturamos a senha digitada com um salt para formar uma hash
        // Salt é um sequência de bytes completamente aleatória
        // O hash é formado pela mistura da senha com o salt, é misturado milhares de vezes
        // O salt é guardado no começo do hash para poder verificar a senha depois

        private byte[] gerarSalt()
        {
            byte[] salt = RandomNumberGenerator.GetBytes(16); // Gera uma array de 16 bytes aleatórios
            return salt;
        }

        public string hasharSenha(string senha)
        {
            byte[] salt = gerarSalt();

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2( // Hasha a senha
                password: senha!, //senha que será hashada (! para o compilador entender que não é nulo e parar de dar o erro)
                salt: salt, // salt
                prf: KeyDerivationPrf.HMACSHA256, //O algoritmo usado para hashar
                iterationCount: 10000, // Número de vezes que o PBKDF2 realiza o processo de derivação
                numBytesRequested: 32) //Tamanho do resultado
            );


            string senhaBanco = $"{Convert.ToBase64String(salt)}.{hashed}"; //Coloca o salt no começo do hash para guardar

            return senhaBanco;
        }
        public string hasharSenhaSalt(string senha, byte[] salt)
        {
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2( // Hasha a senha
                password: senha!, //senha que será hashada (! para o compilador entender que não é nulo e parar de dar o erro)
                salt: salt, // salt
                prf: KeyDerivationPrf.HMACSHA256, //O algoritmo usado para hashar
                iterationCount: 10000, // Número de vezes que o PBKDF2 realiza o processo de derivação
                numBytesRequested: 32) //Tamanho do resultado
            );


            string senhaBanco = $"{Convert.ToBase64String(salt)}.{hashed}"; //Coloca o salt no começo do hash para guardar

            return senhaBanco;
        }
    }
}
