using System.Collections.Generic;
using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class SalvarQuestaoCommand : IRequest<bool>
    {
        public SalvarQuestaoCommand(long relatorioSecaoId, long questaoId, IEnumerable<RelatorioPAPRespostaDto> respostas)
        {
            RelatorioSecaoId = relatorioSecaoId;
            QuestaoId = questaoId;
            Respostas = respostas;
        }

        public long RelatorioSecaoId { get; set; }
        public long QuestaoId { get; set; } 
        public IEnumerable<RelatorioPAPRespostaDto> Respostas { get; set; }
    }

    public class SalvarQuestaoCommandValidator : AbstractValidator<SalvarQuestaoCommand>
    {
        public SalvarQuestaoCommandValidator()
        {
            RuleFor(x => x.QuestaoId).GreaterThan(0).WithMessage("Informe o id da questão");
            RuleFor(x => x.RelatorioSecaoId).GreaterThan(0).WithMessage(" Informe o id da seção");
            RuleFor(x => x.Respostas).NotEmpty().WithMessage(" Informe as respostas ");
        }
    }
}