using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class AlterarEncaminhamentoNAAPASecaoQuestaoCommand : IRequest<bool>
    {
        public AlterarEncaminhamentoNAAPASecaoQuestaoCommand(AtendimentoNAAPASecaoDto encaminhamentoNAAPASecaoDto, EncaminhamentoNAAPASecao encaminhamentoNAAPASecaoObj)
        {
            EncaminhamentoNAAPASecaoDto = encaminhamentoNAAPASecaoDto;
            EncaminhamentoNAAPASecaoObj = encaminhamentoNAAPASecaoObj;
        }

        public AtendimentoNAAPASecaoDto EncaminhamentoNAAPASecaoDto { get; set; }

        public EncaminhamentoNAAPASecao EncaminhamentoNAAPASecaoObj { get; set; }
    }

    public class AlterarEncaminhamentoNAAPASecaoQuestaoCommandValidator : AbstractValidator<AlterarEncaminhamentoNAAPASecaoQuestaoCommand>
    {
        public AlterarEncaminhamentoNAAPASecaoQuestaoCommandValidator()
        {
            RuleFor(a => a.EncaminhamentoNAAPASecaoDto)
               .NotNull()
               .WithMessage("A seção dto deve ser informada para a alteração do encaminhamento NAAPA itinerário!");

            RuleFor(a => a.EncaminhamentoNAAPASecaoObj)
               .NotNull()
               .WithMessage("A seção objeto deve ser informada para a alteração do encaminhamento NAAPA itinerário!");
        }
    }
}
