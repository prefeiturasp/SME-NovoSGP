using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class SalvarConsolidadoAtendimentoNAAPACommand : IRequest<bool>
    {
        public SalvarConsolidadoAtendimentoNAAPACommand(ConsolidadoEncaminhamentoNAAPA consolidado)
        {
            Consolidado = consolidado;
        }

        public ConsolidadoEncaminhamentoNAAPA Consolidado { get; set; }
    }

    public class SalvarConsolidadoAtendimentoNAAPACommandValidator : AbstractValidator<SalvarConsolidadoAtendimentoNAAPACommand>
    {
        public SalvarConsolidadoAtendimentoNAAPACommandValidator()
        {
            RuleFor(x => x.Consolidado).NotEmpty().WithMessage("Você deve informar um registro de consolidado antes de salvar");
        }
    }
}