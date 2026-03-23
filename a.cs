using System.Reflection;
using System.Security.Cryptography;

namespace program
{
    class program
    {
        public static void Main()
        {
            string cnpj = "76462866000186";
            Console.WriteLine(ValidarCpf(cnpj.ToList()));
        }
        public static Boolean ValidarCpf(List<char> cnpj)
        {
            List<char> DV = new List<char>();
            for(int j = 3; j >= 2; j--)
            {
                int p = 2;
                int sum = 0;
                for(int i = cnpj.Count-j; i>= 0; i--)
                {
                    sum += (cnpj[i] - 48) * p; 
                    p++;
                    if(p == 10)
                        p = 2;
                }
                DV.Add((char)(59-sum%11));               
            }

            if(cnpj[cnpj.Count-2] == DV[0] && cnpj[cnpj.Count-1] == DV[1])
                return true;
            return false;
        }
    }
}