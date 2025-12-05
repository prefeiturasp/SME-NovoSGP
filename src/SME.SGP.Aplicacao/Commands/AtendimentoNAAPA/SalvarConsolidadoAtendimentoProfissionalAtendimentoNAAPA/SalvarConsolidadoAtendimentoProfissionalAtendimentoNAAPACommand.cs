using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class SalvarConsolidadoAtendimentoProfissionalAtendimentoNAAPACommand : IRequest<bool>
    {
        public SalvarConsolidadoAtendimentoProfissionalAtendimentoNAAPACommand(ConsolidadoAtendimentoNAAPA consolidado)
        {
            Consolidado = consolidado;
        }

        public ConsolidadoAtendimentoNAAPA Consolidado { get; set; }
    }

    public class SalvarConsolidadoAtendimentoProfissionalAtendimentoNAAPACommandValidator : AbstractValidator<SalvarConsolidadoAtendimentoProfissionalAtendimentoNAAPACommand>
    {
        public SalvarConsolidadoAtendimentoProfissionalAtendimentoNAAPACommandValidator()
        {
            RuleFor(x => x.Consolidado).NotEmpty().WithMessage("Você deve informar um registro de consolidado antes de salvar");
        }
    }
}