using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Windows.Forms;

namespace MelFitnessAssinaturas.Util
{
    public static class ValidacaoUtil
    {

        public static IEnumerable<ValidationResult> GetValidationErros(object obj)
        {
            var resultadoValidacao = new List<ValidationResult>();
            var contexto = new ValidationContext(obj, null, null);
            Validator.TryValidateObject(obj, contexto, resultadoValidacao, true);
            return resultadoValidacao;
        }

        public static bool ValidarModelo(object obj)
        {
            var erros = GetValidationErros(obj);
            string strErros = string.Empty;

            foreach (var error in erros)
            {
                strErros += error.ErrorMessage + Environment.NewLine;
            }

            if (strErros.Length > 0)
            {

                // Se existirem erros, apresenta Mensagem com a listagem de erros.
                // Você pode modificar essa classe para retornar a listagem dos erros e apresentar em um label no formulário

                strErros = "Favor verificar os seguintes campos: " + Environment.NewLine + Environment.NewLine + strErros;

                strErros = strErros.Replace(@"\n", Environment.NewLine);

                MessageBox.Show(strErros, "Atenção !!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;

        }
        
    }
}
