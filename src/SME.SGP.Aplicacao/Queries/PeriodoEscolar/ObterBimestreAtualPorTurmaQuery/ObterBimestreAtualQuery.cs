using MediatR;
using SME.SGP.Dominio;
using System;

namespace SME.SGP.Aplicacao
{
    public class ObterBimestreAtualQuery : IRequest<int>
    {
        public ObterBimestreAtualQuery() { }

        public ObterBimestreAtualQuery(string turmaCodigo, DateTime dataReferencia, Turma turma = null)
        {
            if (string.IsNullOrEmpty(turmaCodigo))
                throw new NegocioException("Para obter o bimestre atual é necessário informar o código da turma");
            if (dataReferencia == DateTime.MinValue)
                throw new NegocioException("Para obter o bimestre atual é necessário informar a data de referência");

            TurmaCodigo = turmaCodigo;
            DataReferencia = dataReferencia;
            Turma = turma;
        }

        public Turma Turma { get; set; }
        public string TurmaCodigo { get; set; }
        public DateTime DataReferencia { get; set; }
    }
}
