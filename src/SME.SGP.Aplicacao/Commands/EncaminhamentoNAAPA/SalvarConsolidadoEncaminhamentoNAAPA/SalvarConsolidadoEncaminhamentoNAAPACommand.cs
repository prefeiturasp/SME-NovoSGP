using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class SalvarConsolidadoEncaminhamentoNAAPACommand : IRequest<bool>
    {
        public SalvarConsolidadoEncaminhamentoNAAPACommand(ConsolidadoEncaminhamentoNAAPA consolidado)
        {
            Consolidado = consolidado;
        }

        public ConsolidadoEncaminhamentoNAAPA Consolidado { get; set; }
    }

    public class SalvarConsolidadoEncaminhamentoNAAPACommandValidator : AbstractValidator<SalvarConsolidadoEncaminhamentoNAAPACommand>
    {
        public SalvarConsolidadoEncaminhamentoNAAPACommandValidator()
        {
            RuleFor(x => x.Consolidado).NotEmpty().WithMessage("Você deve informar um registro de consolidado antes de salvar");
        }
    }
}