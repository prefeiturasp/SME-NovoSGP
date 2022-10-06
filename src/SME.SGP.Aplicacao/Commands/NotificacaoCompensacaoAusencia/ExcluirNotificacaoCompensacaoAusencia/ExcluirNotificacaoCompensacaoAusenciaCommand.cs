using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExcluirNotificacaoCompensacaoAusenciaCommand : IRequest
    {
        public ExcluirNotificacaoCompensacaoAusenciaCommand(long compensacaoAusenciaId)
        {
            CompensacaoAusenciaId = compensacaoAusenciaId;
        }

        public long CompensacaoAusenciaId { get; }
    }

    public class ExcluirNotificacaoCompensacaoAusenciaCommandValidator : AbstractValidator<ExcluirNotificacaoCompensacaoAusenciaCommand>
    {
        public ExcluirNotificacaoCompensacaoAusenciaCommandValidator()
        {
            RuleFor(x => x.CompensacaoAusenciaId)
                .NotEmpty()
                .WithMessage("O identificador da compensação de ausência deve ser informado para excluir suas notificações");
        }
    }
}
