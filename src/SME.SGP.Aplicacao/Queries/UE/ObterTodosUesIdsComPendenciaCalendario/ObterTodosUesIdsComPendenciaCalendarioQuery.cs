using System.Collections.Generic;
using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterTodosUesIdsComPendenciaCalendarioQuery : IRequest<IEnumerable<TodosUesIdsComPendenciaCalendarioDto>>
    {
        public ObterTodosUesIdsComPendenciaCalendarioQuery(int anoLetivo)
        {
            AnoLetivo = anoLetivo;
        }

        public int AnoLetivo { get; set; }
    }

    public class ObterTodosUesIdsComPendenciaCalendarioQueryValidator : AbstractValidator<ObterTodosUesIdsComPendenciaCalendarioQuery>
    {
        public ObterTodosUesIdsComPendenciaCalendarioQueryValidator()
        {
            RuleFor(x => x.AnoLetivo).GreaterThan(0).WithMessage("Informe o Ano Letico para Obter os Ids das Ues");
        }
    }
}