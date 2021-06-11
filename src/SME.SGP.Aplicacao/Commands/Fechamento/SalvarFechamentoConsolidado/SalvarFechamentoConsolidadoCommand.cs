using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class SalvarFechamentoConsolidadoCommand : IRequest<long>
    {
        public SalvarFechamentoConsolidadoCommand(FechamentoConsolidadoComponenteTurma fechamentoConsolidadoComponenteTurma)
        {
            FechamentoConsolidadoComponenteTurma = fechamentoConsolidadoComponenteTurma;
        }

        public FechamentoConsolidadoComponenteTurma FechamentoConsolidadoComponenteTurma { get; }
    }
}

