using MediatR;
using SME.SGP.Dominio;
using System;

namespace SME.SGP.Aplicacao
{
    public class ObterBimestreAtualQuery : IRequest<int>
    {
        public ObterBimestreAtualQuery(string codigoTurma, DateTime dataReferencia)
        {
            if (string.IsNullOrEmpty(codigoTurma))
                throw new NegocioException("Para obter o bimestre atual é necessário informar o código da turma");
            if (dataReferencia == DateTime.MinValue)
                throw new NegocioException("Para obter o bimestre atual é necessário informar a data de referência");

            CodigoTurma = codigoTurma;
            DataReferencia = dataReferencia;
        }
        public string CodigoTurma { get; set; }
        public DateTime DataReferencia { get; set; }
    }
}
