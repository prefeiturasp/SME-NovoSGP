using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class AlterarAtendimentoNAAPASecaoQuestaoCommand : IRequest<bool>
    {
        public AlterarAtendimentoNAAPASecaoQuestaoCommand(AtendimentoNAAPASecaoDto encaminhamentoNAAPASecaoDto, EncaminhamentoNAAPASecao encaminhamentoNAAPASecaoObj)
        {
            EncaminhamentoNAAPASecaoDto = encaminhamentoNAAPASecaoDto;
            EncaminhamentoNAAPASecaoObj = encaminhamentoNAAPASecaoObj;
        }

        public AtendimentoNAAPASecaoDto EncaminhamentoNAAPASecaoDto { get; set; }

        public EncaminhamentoNAAPASecao EncaminhamentoNAAPASecaoObj { get; set; }
    }

    public class AlterarAtendimentoNAAPASecaoQuestaoCommandValidator : AbstractValidator<AlterarAtendimentoNAAPASecaoQuestaoCommand>
    {
        public AlterarAtendimentoNAAPASecaoQuestaoCommandValidator()
        {
            RuleFor(a => a.EncaminhamentoNAAPASecaoDto)
               .NotNull()
               .WithMessage("A seção dto deve ser informada para a alteração do atendimento NAAPA itinerário!");

            RuleFor(a => a.EncaminhamentoNAAPASecaoObj)
               .NotNull()
               .WithMessage("A seção objeto deve ser informada para a alteração do atendimento NAAPA itinerário!");
        }
    }
}
