using FluentValidation.Results;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Infra.Dtos
{
    public class RespostaApi
    {
        internal RespostaApi(bool ok, IEnumerable<string> erros, object data)
        {
            Ok = ok;
            Erros = erros.ToArray();
            Data = data;
        }

        internal RespostaApi(bool ok, IList<ValidationFailure> erros)
        {
            Ok = ok;
            Erros = erros.Select(x => x.ErrorMessage).ToArray();
            Data = null;
        }

        public RespostaApi()
        {
        }

        public bool Ok { get; set; }

        public string[] Erros { get; set; }

        public IDictionary<string, string[]> ValidacaoErros { get; set; }

        public object Data { get; set; }

        public static RespostaApi Sucesso(object data = null)
        {
            return new RespostaApi(true, new string[] { }, data);
        }

        public static RespostaApi Falha(IEnumerable<string> errors)
        {
            return new RespostaApi(false, errors, null);
        }

        public static RespostaApi Falha(string error)
        {
            return new RespostaApi(false, new string[] { error }, null);
        }

        public static RespostaApi Falha(IDictionary<string, string[]> erros)
        {
            return new RespostaApi
            {
                ValidacaoErros = erros,
                Data = null,
                Erros = null,
                Ok = false
            };
        }

        public static RespostaApi Falha(IList<ValidationFailure> erros)
        {
            return new RespostaApi(false, erros);
        }
    }

}
