using FluentValidation;
using MediatR;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciasCalendarioPorUeQuery : IRequest<IEnumerable<long>>
    {
        public ObterPendenciasCalendarioPorUeQuery(int anoLetivo, long ueId)
        {
            AnoLetivo = anoLetivo;
            UeId = ueId;
        }

        public int AnoLetivo { get; set; }
        public long UeId { get; set; }
    }

    public class ObterTodosUesIdsComPendenciaCalendarioQueryValidator : AbstractValidator<ObterPendenciasCalendarioPorUeQuery>
    {
        public ObterTodosUesIdsComPendenciaCalendarioQueryValidator()
        {
            RuleFor(x => x.AnoLetivo).GreaterThan(0).WithMessage("Informe o Ano Letivo para obter as pendências correspondentes");
            RuleFor(x => x.UeId).GreaterThan(0).WithMessage("Informe o id da UE para obter as pendências correspondentes");
        }
    }
}