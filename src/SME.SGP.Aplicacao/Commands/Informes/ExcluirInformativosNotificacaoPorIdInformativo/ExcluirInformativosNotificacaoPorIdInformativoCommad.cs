using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExcluirInformativosNotificacaoPorIdInformativoCommad : IRequest<bool>
    {
        public ExcluirInformativosNotificacaoPorIdInformativoCommad(long informativoId)
        {
            InformativoId = informativoId;
        }
        public long InformativoId { get; }
    }

    public class ExcluirInformativosNotificacaoPorIdInformativoCommandValidator : AbstractValidator<ExcluirInformativosNotificacaoPorIdInformativoCommad>
    {
        public ExcluirInformativosNotificacaoPorIdInformativoCommandValidator()
        {
            RuleFor(c => c.InformativoId)
                .GreaterThan(0)
                .WithMessage("O Id do informativo deve ser informado para exclusão do vínculo com a notificação.");
        }
    }
}
