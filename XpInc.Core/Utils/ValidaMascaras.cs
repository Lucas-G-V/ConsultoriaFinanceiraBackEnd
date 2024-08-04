using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace XpInc.Core.Utils
{
    public static class ValidaMascaras
    {
        public static bool ValidaCep(string cep)
        {
            cep = FormatarString.ExtractNumbers(cep);
            if (cep.Length != 8) return false;
            return true;
        }

        public static bool ValidaCPF(string valor)
        {
            valor = FormatarString.ExtractNumbers(valor);
            if (valor.Length != 11)
                return false;
            bool todosIguais = true;
            for (int i = 1; i < valor.Length; i++)
            {
                if (valor[i] != valor[0])
                {
                    todosIguais = false;
                    break;
                }
            }
            if (todosIguais)
                return false;
            int soma = 0;
            for (int i = 0; i < 9; i++)
            {
                soma += (10 - i) * (int)char.GetNumericValue(valor[i]);
            }
            int resto = soma % 11;
            int digitoVerificador1 = (resto < 2) ? 0 : 11 - resto;
            soma = 0;
            for (int i = 0; i < 10; i++)
            {
                soma += (11 - i) * (int)char.GetNumericValue(valor[i]);
            }
            resto = soma % 11;
            int digitoVerificador2 = (resto < 2) ? 0 : 11 - resto;
            return (int)char.GetNumericValue(valor[9]) == digitoVerificador1 && (int)char.GetNumericValue(valor[10]) == digitoVerificador2;
        }

        public static bool ValidaTelefoneFixo(string valor)
        {
            valor = FormatarString.ExtractNumbers(valor);
            if (valor.Length >= 10 && valor.Length <= 12) return true;
            return false;
        }

        public static bool ValidaCNPJ(string valor)
        {
            valor = FormatarString.ExtractNumbers(valor);
            if (valor.Length != 14)
                return false;
            int soma = 0;
            int multiplicador = 5;
            for (int i = 0; i < 12; i++)
            {
                soma += multiplicador * (int)char.GetNumericValue(valor[i]);
                multiplicador = (multiplicador == 2) ? 9 : multiplicador - 1;
            }
            int resto = soma % 11;
            int digitoVerificador1 = (resto < 2) ? 0 : 11 - resto;
            soma = 0;
            multiplicador = 6;
            for (int i = 0; i < 13; i++)
            {
                soma += multiplicador * (int)char.GetNumericValue(valor[i]);
                multiplicador = (multiplicador == 2) ? 9 : multiplicador - 1;
            }
            resto = soma % 11;
            int digitoVerificador2 = (resto < 2) ? 0 : 11 - resto;
            return (int)char.GetNumericValue(valor[12]) == digitoVerificador1
                && (int)char.GetNumericValue(valor[13]) == digitoVerificador2;
        }

        public static bool ValidaEmail(string valor)
        {
            return Regex.IsMatch(valor, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        public static bool ValidaCelular(string valor)
        {
            valor = FormatarString.ExtractNumbers(valor);
            if (valor.Length >= 10 && valor.Length <= 13) return true;
            return false;
        }

        public static bool ValidaWebSite(string valor)
        {
            return Uri.TryCreate(valor, UriKind.Absolute, out _);
        }
    }
}
