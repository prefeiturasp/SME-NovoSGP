using MediatR;
using SME.SGP.Dominio;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarCompensacaoAusenciaAlunoAulaCommand : IRequest<long>
    {
        public SalvarCompensacaoAusenciaAlunoAulaCommand(CompensacaoAusenciaAlunoAula compensacaoAusenciaAlunoAula)
        {
            CompensacaoAusenciaAlunoAula = compensacaoAusenciaAlunoAula;
        }

        public CompensacaoAusenciaAlunoAula CompensacaoAusenciaAlunoAula { get; }
    }
}
