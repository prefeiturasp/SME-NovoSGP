using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class RemoverConsolidacaoAlunoFrequenciaMensalCommand : IRequest<bool>
    {
        public long ConsolidacaoId { get; set; }
    }

    public class RemoverConsolidacaoAlunoFrequenciaMensalCommandValidator : AbstractValidator<RemoverConsolidacaoAlunoFrequenciaMensalCommand>
    {
        public RemoverConsolidacaoAlunoFrequenciaMensalCommandValidator()
        {
            RuleFor(a => a.ConsolidacaoId)
                .NotEmpty()
                .WithMessage("É necessário informar o id da consolidação para executar a exclusão do mesmo.");
        }
    }
}
