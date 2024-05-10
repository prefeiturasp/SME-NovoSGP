using MediatR;
using SME.SGP.Dominio;
using System;

namespace SME.SGP.Aplicacao
{
    public class ObterBimestreAtualQuery : IRequest<int>
    {
        public ObterBimestreAtualQuery(DateTime dataReferencia, Turma turma = null)
        {
            DataReferencia = dataReferencia;
            Turma = turma;
        }

        public Turma Turma { get; set; }
        public DateTime DataReferencia { get; set; }
    }
}
