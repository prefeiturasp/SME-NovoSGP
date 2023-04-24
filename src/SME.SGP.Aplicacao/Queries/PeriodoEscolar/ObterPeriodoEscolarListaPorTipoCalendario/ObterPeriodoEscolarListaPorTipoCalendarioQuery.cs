using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodoEscolarListaPorTipoCalendarioQuery : IRequest<PeriodoEscolarListaDto>
    {
        public ObterPeriodoEscolarListaPorTipoCalendarioQuery(long tipoCalendarioId)
        {
            TipoCalendarioId = tipoCalendarioId;
        }

        public long TipoCalendarioId { get; set; }
    }

    class ObterPeriodoEscolarListaPorTipoCalendarioQueryValidator : AbstractValidator<ObterPeriodoEscolarListaPorTipoCalendarioQuery>
    {
        public ObterPeriodoEscolarListaPorTipoCalendarioQueryValidator()
        {
            RuleFor(x => x.TipoCalendarioId).GreaterThan(0).WithMessage("Informe o id do Tipo Calendario para realizar a consulta");
        }
    }
}