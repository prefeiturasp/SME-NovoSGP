using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExcluirSecoesAtendimentoNAAPAPorAtendimentoIdCommand : IRequest<bool>
    {
        public ExcluirSecoesAtendimentoNAAPAPorAtendimentoIdCommand(long encaminhamentoNAAPAId)
        {
            EncaminhamentoNAAPAId = encaminhamentoNAAPAId;
        }

        public long EncaminhamentoNAAPAId { get; }
    }

    public class ExcluirSecoesAtendimentoNAAPAPorAtendimentoIdCommandValidator : AbstractValidator<ExcluirSecoesAtendimentoNAAPAPorAtendimentoIdCommand>
    {
        public ExcluirSecoesAtendimentoNAAPAPorAtendimentoIdCommandValidator()
        {
            RuleFor(c => c.EncaminhamentoNAAPAId)
            .NotEmpty()
            .WithMessage("O id do encaminhamento NAAPA deve ser informado para exclusão de suas seções.");

        }
    }
}
