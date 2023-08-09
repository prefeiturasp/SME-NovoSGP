using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class AlterarRelatorioPeriodicoQuestaoPAPCommand : IRequest<long>
    {
        public AlterarRelatorioPeriodicoQuestaoPAPCommand(RelatorioPeriodicoPAPQuestao relatorioPeriodicoQuestao)
        {
            RelatorioPeriodicoQuestao = relatorioPeriodicoQuestao;
        }

        public RelatorioPeriodicoPAPQuestao RelatorioPeriodicoQuestao { get; set; }
    }

    public class AlterarRelatorioPeriodicoQuestaoPAPCommandValidator : AbstractValidator<AlterarRelatorioPeriodicoQuestaoPAPCommand>
    {
        public AlterarRelatorioPeriodicoQuestaoPAPCommandValidator()
        {
            RuleFor(x => x.RelatorioPeriodicoQuestao)
                   .NotEmpty()
                   .WithMessage("O objeto relatório periodico questão pap deve ser informado para registar sua alteração!");
        }
    }
}
