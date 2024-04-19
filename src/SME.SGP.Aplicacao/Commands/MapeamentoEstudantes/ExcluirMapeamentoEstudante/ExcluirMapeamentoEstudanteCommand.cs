using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExcluirMapeamentoEstudanteCommand : IRequest<bool>
    {
        public ExcluirMapeamentoEstudanteCommand(long mapeamentoEstudanteId)
        {
            MapeamentoEstudanteId = mapeamentoEstudanteId;
        }

        public long MapeamentoEstudanteId { get; }
    }

    public class ExcluirMapeamentoEstudanteCommandValidator : AbstractValidator<ExcluirMapeamentoEstudanteCommand>
    {
        public ExcluirMapeamentoEstudanteCommandValidator()
        {

            RuleFor(c => c.MapeamentoEstudanteId)
            .NotEmpty()
            .WithMessage("O id do mapeamento de estudante deve ser informado para exclusão.");

        }
    }
}
