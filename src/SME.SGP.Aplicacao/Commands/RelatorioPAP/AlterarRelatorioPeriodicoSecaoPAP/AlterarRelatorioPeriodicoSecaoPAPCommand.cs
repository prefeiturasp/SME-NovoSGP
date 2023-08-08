using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class AlterarRelatorioPeriodicoSecaoPAPCommand : IRequest<long>
    {
        public AlterarRelatorioPeriodicoSecaoPAPCommand(RelatorioPeriodicoPAPSecao relatorioPeriodicoSecao)
        {
            RelatorioPeriodicoSecao = relatorioPeriodicoSecao;
        }

        public RelatorioPeriodicoPAPSecao RelatorioPeriodicoSecao { get; set; }
    }

    public class AlterarRelatorioPeriodicoSecaoPAPCommandValidator : AbstractValidator<AlterarRelatorioPeriodicoSecaoPAPCommand>
    {
        public AlterarRelatorioPeriodicoSecaoPAPCommandValidator()
        {
            RuleFor(x => x.RelatorioPeriodicoSecao)
                   .NotEmpty()
                   .WithMessage("O relatório periodico seção pap deve ser informado para registar sua alteração!");
        }
    }
}
