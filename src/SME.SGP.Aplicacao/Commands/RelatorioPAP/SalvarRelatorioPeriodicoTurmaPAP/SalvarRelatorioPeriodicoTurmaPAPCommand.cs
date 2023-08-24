using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class SalvarRelatorioPeriodicoTurmaPAPCommand : IRequest<RelatorioPeriodicoPAPTurma>
    {
        public SalvarRelatorioPeriodicoTurmaPAPCommand(long turmaId, long periodoRelatorioPAPId)
        {
            TurmaId = turmaId;
            PeriodoRelatorioPAPId = periodoRelatorioPAPId;
        }

        public long TurmaId { get; set; }
        public long PeriodoRelatorioPAPId { get; set; }
    }

    public class SalvarRelatorioPeriodicoTurmaPAPCommandValidator : AbstractValidator<SalvarRelatorioPeriodicoTurmaPAPCommand>
    {
        public SalvarRelatorioPeriodicoTurmaPAPCommandValidator()
        {
            RuleFor(x => x.TurmaId)
                   .GreaterThan(0)
                   .WithMessage("O Id da turma deve ser informado para registar relatório periodico turma pap!");
            RuleFor(x => x.PeriodoRelatorioPAPId)
                   .GreaterThan(0)
                   .WithMessage("O Id do período relatório pap deve ser informada para registar relatório periodico turma pap!");
        }
    }
}
