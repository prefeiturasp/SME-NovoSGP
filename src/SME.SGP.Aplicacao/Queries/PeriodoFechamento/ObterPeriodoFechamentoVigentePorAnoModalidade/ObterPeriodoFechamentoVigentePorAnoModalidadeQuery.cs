using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodoFechamentoVigentePorAnoModalidadeQuery : IRequest<PeriodoFechamentoVigenteDto>
    {
        public ObterPeriodoFechamentoVigentePorAnoModalidadeQuery(int anoLetivo, ModalidadeTipoCalendario? modalidadeTipoCalendario = null)
        {
            AnoLetivo = anoLetivo;
            ModalidadeTipoCalendario = modalidadeTipoCalendario;
        }

        public int AnoLetivo { get; }
        public ModalidadeTipoCalendario? ModalidadeTipoCalendario { get; }
    }

    public class ObterPeriodoFechamentoVigentePorAnoModalidadeQueryValidator : AbstractValidator<ObterPeriodoFechamentoVigentePorAnoModalidadeQuery>
    {
        public ObterPeriodoFechamentoVigentePorAnoModalidadeQueryValidator()
        {
            RuleFor(a => a.AnoLetivo)
                .NotEmpty()
                .WithMessage("O ano letivo deve ser informado para consulta de período de fechamento vigente.");
        }
    }
}
