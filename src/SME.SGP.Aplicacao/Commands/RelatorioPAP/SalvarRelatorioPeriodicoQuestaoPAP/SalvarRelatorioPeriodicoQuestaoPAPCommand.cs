using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class SalvarRelatorioPeriodicoQuestaoPAPCommand : IRequest<RelatorioPeriodicoPAPQuestao>
    {
        public SalvarRelatorioPeriodicoQuestaoPAPCommand(long relatorioPeriodiocoSecaoId, long questaoId)
        {
            RelatorioPeriodiocoSecaoId = relatorioPeriodiocoSecaoId;
            QuestaoId = questaoId;
        }

        public long RelatorioPeriodiocoSecaoId { get; set; }
        public long QuestaoId { get; set; }
    }

    public class SalvarRelatorioPeriodicoQuestaoPAPQueryValidator : AbstractValidator<SalvarRelatorioPeriodicoQuestaoPAPCommand>
    {
        public SalvarRelatorioPeriodicoQuestaoPAPQueryValidator()
        {
            RuleFor(x => x.RelatorioPeriodiocoSecaoId)
                   .NotEmpty()
                   .WithMessage("O id da relatório periodico seção pap deve ser informado para registar relatório periodico questão pap!");
            RuleFor(x => x.QuestaoId)
                   .NotEmpty()
                   .WithMessage("O id do relatório peridodico aluno deve ser informado para registar relatório periodico seção pap!");
        }
    }
}
