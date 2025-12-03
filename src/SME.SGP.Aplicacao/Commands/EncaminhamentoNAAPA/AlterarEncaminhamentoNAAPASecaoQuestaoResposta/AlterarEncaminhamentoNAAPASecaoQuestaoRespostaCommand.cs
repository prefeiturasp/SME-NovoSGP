using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class AlterarEncaminhamentoNAAPASecaoQuestaoRespostaCommand : IRequest<bool>
    {
        public AlterarEncaminhamentoNAAPASecaoQuestaoRespostaCommand(RespostaEncaminhamentoNAAPA respostaAlterar, AtendimentoNAAPASecaoQuestaoDto encaminhamentoNAAPASecaoQuestaoDto)
        {
            RespostaEncaminhamento = respostaAlterar;
            RespostaQuestaoDto = encaminhamentoNAAPASecaoQuestaoDto;
        }

        public RespostaEncaminhamentoNAAPA RespostaEncaminhamento { get; set; }
        public AtendimentoNAAPASecaoQuestaoDto RespostaQuestaoDto { get; set; }
    }

    public class AlterarEncaminhamentoNAAPASecaoQuestaoRespostaCommandValidator : AbstractValidator<AlterarEncaminhamentoNAAPASecaoQuestaoRespostaCommand>
    {
        public AlterarEncaminhamentoNAAPASecaoQuestaoRespostaCommandValidator()
        {
            RuleFor(c => c.RespostaEncaminhamento)
            .NotEmpty()
            .WithMessage("A entidade de reposta do encaminhamento NAAPA deve ser informada para alteração.");

            RuleFor(c => c.RespostaQuestaoDto)
            .NotEmpty()
            .WithMessage("O dto de reposta do encaminhamento NAAPA deve ser informada para alteração.");
        }
    }
}
