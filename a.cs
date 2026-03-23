using System.Reflection;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace program
{
    class program
    {
        public static void Main()
        {
            string cnpj = "26.853.151/0001-86";
            //string cnpjformatado = Regex.Replace(cnpj,@"[.\-/]+",string.Empty);
            Console.Write(ValidarCpf(cnpj));
        }
        public static Boolean ValidarCpf(string cnpj)
        {
            string cnpjFormatado = new string(cnpj.Where(char.IsDigit).ToArray());
            Console.Write($"{cnpj} ");
            List<char> DV = new();
            for(int j = 3; j >= 2; j--)
            {
                int p = 2;
                int sum = 0;
                for(int i = cnpj.Length - j; i>= 0; i--)
                {
                    sum += (cnpj[i] - 48) * p; 
                    p++;
                    if(p == 10)
                        p = 2;
                }
                DV.Add((char)(59-sum%11));               
            }

            if(cnpj[cnpj.Length-2] == DV[0] && cnpj[cnpj.Length-1] == DV[1])
                return true;
            return false;
        }
    }
}