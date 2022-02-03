using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class SalvarFechamentoTurmaCommand : IRequest<long>
    {
        public SalvarFechamentoTurmaCommand(FechamentoTurma fechamentoTurma)
        {
            FechamentoTurma = fechamentoTurma;
        }

        public FechamentoTurma FechamentoTurma { get; }
    }
}
