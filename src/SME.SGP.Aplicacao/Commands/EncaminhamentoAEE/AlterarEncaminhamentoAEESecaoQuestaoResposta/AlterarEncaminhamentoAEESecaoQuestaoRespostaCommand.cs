using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class AlterarEncaminhamentoAEESecaoQuestaoRespostaCommand : IRequest<bool>
    {
        public AlterarEncaminhamentoAEESecaoQuestaoRespostaCommand(RespostaEncaminhamentoAEE respostaAlterar, EncaminhamentoAEESecaoQuestaoDto encaminhamentoAEESecaoQuestaoDto)
        {
            RespostaEncaminhamento = respostaAlterar;
            RespostaQuestaoDto = encaminhamentoAEESecaoQuestaoDto;
        }

        public RespostaEncaminhamentoAEE RespostaEncaminhamento { get; set; }
        public EncaminhamentoAEESecaoQuestaoDto RespostaQuestaoDto { get; set; }
    }

    public class AlterarEncaminhamentoAEESecaoQuestaoRespostaCommandValidator : AbstractValidator<AlterarEncaminhamentoAEESecaoQuestaoRespostaCommand>
    {
        public AlterarEncaminhamentoAEESecaoQuestaoRespostaCommandValidator()
        {
            RuleFor(c => c.RespostaEncaminhamento)
            .NotEmpty()
            .WithMessage("A entidade de reposta do encaminhamento deve ser informada para alteração.");

            RuleFor(c => c.RespostaQuestaoDto)
            .NotEmpty()
            .WithMessage("O dto de reposta do encaminhamento deve ser informada para alteração.");
        }
    }
}
