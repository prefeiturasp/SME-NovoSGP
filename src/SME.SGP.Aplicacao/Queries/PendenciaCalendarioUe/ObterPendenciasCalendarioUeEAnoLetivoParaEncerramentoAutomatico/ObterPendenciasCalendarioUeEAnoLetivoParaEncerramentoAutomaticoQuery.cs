using MediatR;
using System.Collections.Generic;
using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciasCalendarioUeEAnoLetivoParaEncerramentoAutomaticoQuery : IRequest<IEnumerable<long>>
    {
        public ObterPendenciasCalendarioUeEAnoLetivoParaEncerramentoAutomaticoQuery(long ueId,int anoLetivo)
        {
            UeId = ueId;
            AnoLetivo = anoLetivo;
        }

        public long UeId { get; set; }
        public int AnoLetivo { get; set; }
    }

    public class ObterPendenciasCalendarioUeEAnoLetivoParaEncerramentoAutomaticoQueryValidator : AbstractValidator<ObterPendenciasCalendarioUeEAnoLetivoParaEncerramentoAutomaticoQuery>
    {
        public ObterPendenciasCalendarioUeEAnoLetivoParaEncerramentoAutomaticoQueryValidator()
        {
            RuleFor(x => x.UeId).GreaterThan(0).WithMessage("Informe o Id da UE para obter as Pendêncis");
            RuleFor(x => x.AnoLetivo).GreaterThan(0).WithMessage("Informe o Ano Letivo para obter as Pendêncis");
        }
    }
}