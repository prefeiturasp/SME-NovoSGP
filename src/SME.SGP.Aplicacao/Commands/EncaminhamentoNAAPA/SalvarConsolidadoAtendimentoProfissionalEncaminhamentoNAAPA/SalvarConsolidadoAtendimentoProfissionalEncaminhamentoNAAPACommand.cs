using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class SalvarConsolidadoAtendimentoProfissionalEncaminhamentoNAAPACommand : IRequest<bool>
    {
        public SalvarConsolidadoAtendimentoProfissionalEncaminhamentoNAAPACommand(ConsolidadoAtendimentoNAAPA consolidado)
        {
            Consolidado = consolidado;
        }

        public ConsolidadoAtendimentoNAAPA Consolidado { get; set; }
    }

    public class SalvarConsolidadoAtendimentoProfissionalEncaminhamentoNAAPACommandValidator : AbstractValidator<SalvarConsolidadoAtendimentoProfissionalEncaminhamentoNAAPACommand>
    {
        public SalvarConsolidadoAtendimentoProfissionalEncaminhamentoNAAPACommandValidator()
        {
            RuleFor(x => x.Consolidado).NotEmpty().WithMessage("Você deve informar um registro de consolidado antes de salvar");
        }
    }
}