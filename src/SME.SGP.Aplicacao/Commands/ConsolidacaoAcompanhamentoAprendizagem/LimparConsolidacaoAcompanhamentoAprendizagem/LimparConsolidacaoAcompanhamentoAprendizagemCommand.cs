using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class LimparConsolidacaoAcompanhamentoAprendizagemCommand : IRequest<bool>
    {
        public LimparConsolidacaoAcompanhamentoAprendizagemCommand(int anoLetivo)
        {
            AnoLetivo = anoLetivo;
        }

        public int AnoLetivo { get; }
    }

    public class LimparConsolidacaoAcompanhamentoAprendizagemCommandValidator : AbstractValidator<LimparConsolidacaoAcompanhamentoAprendizagemCommand>
    {
        public LimparConsolidacaoAcompanhamentoAprendizagemCommandValidator()
        {
            RuleFor(a => a.AnoLetivo)
                .NotEmpty()
                .WithMessage("O ano letivo deve ser informado para limpar a consolidação de Acompanhamento de Aprendizagem do Aluno");
        }
    }
}
