using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class SalvarCompensacaoAusenciaAlunoCommand : IRequest<long>
    {
        public SalvarCompensacaoAusenciaAlunoCommand(CompensacaoAusenciaAluno compensacaoAusenciaAluno)
        {
            CompensacaoAusenciaAluno = compensacaoAusenciaAluno;
        }

        public CompensacaoAusenciaAluno CompensacaoAusenciaAluno { get; }
    }
}
