using MediatR;
using SME.SGP.Dominio;
using System;

namespace SME.SGP.Aplicacao
{
    public class ObterBimestreAtualComAberturaPorTurmaQuery : IRequest<int>
    {
        public ObterBimestreAtualComAberturaPorTurmaQuery(Turma turma, DateTime dataReferencia)
        {
            Turma = turma;
            DataReferencia = dataReferencia;
        }

        public Turma Turma { get; set; }
        public DateTime DataReferencia { get; set; }
    }
}
