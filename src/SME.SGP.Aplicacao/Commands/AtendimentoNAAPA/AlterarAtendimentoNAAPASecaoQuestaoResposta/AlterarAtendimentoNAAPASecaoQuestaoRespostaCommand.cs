using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class AlterarAtendimentoNAAPASecaoQuestaoRespostaCommand : IRequest<bool>
    {
        public AlterarAtendimentoNAAPASecaoQuestaoRespostaCommand(RespostaEncaminhamentoNAAPA respostaAlterar, AtendimentoNAAPASecaoQuestaoDto encaminhamentoNAAPASecaoQuestaoDto)
        {
            RespostaEncaminhamento = respostaAlterar;
            RespostaQuestaoDto = encaminhamentoNAAPASecaoQuestaoDto;
        }

        public RespostaEncaminhamentoNAAPA RespostaEncaminhamento { get; set; }
        public AtendimentoNAAPASecaoQuestaoDto RespostaQuestaoDto { get; set; }
    }

    public class AlterarAtendimentoNAAPASecaoQuestaoRespostaCommandValidator : AbstractValidator<AlterarAtendimentoNAAPASecaoQuestaoRespostaCommand>
    {
        public AlterarAtendimentoNAAPASecaoQuestaoRespostaCommandValidator()
        {
            RuleFor(c => c.RespostaEncaminhamento)
            .NotEmpty()
            .WithMessage("A entidade de reposta do atendimento NAAPA deve ser informada para alteração.");

            RuleFor(c => c.RespostaQuestaoDto)
            .NotEmpty()
            .WithMessage("O dto de reposta do atendimento NAAPA deve ser informada para alteração.");
        }
    }
}
