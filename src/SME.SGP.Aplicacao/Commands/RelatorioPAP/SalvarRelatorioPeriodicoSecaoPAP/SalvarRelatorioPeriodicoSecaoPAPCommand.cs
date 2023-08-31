using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class SalvarRelatorioPeriodicoSecaoPAPCommand : IRequest<RelatorioPeriodicoPAPSecao>
    {
        public SalvarRelatorioPeriodicoSecaoPAPCommand(long secaoRelatorioPeriodicoId, long relatorioPeriodicoAlunoId)
        {
            SecaoRelatorioPeriodicoId = secaoRelatorioPeriodicoId;
            RelatorioPeriodicoAlunoId = relatorioPeriodicoAlunoId;
        }

        public long SecaoRelatorioPeriodicoId { get; set; } 
        public long RelatorioPeriodicoAlunoId { get; set; }
    }

    public class SalvarRelatorioPeriodicoSecaoPAPCommandValidator : AbstractValidator<SalvarRelatorioPeriodicoSecaoPAPCommand>
    {
        public SalvarRelatorioPeriodicoSecaoPAPCommandValidator()
        {
            RuleFor(x => x.SecaoRelatorioPeriodicoId)
                   .NotEmpty()
                   .WithMessage("O id da seção relatório periodico pap deve ser informado para registar relatório periodico seção pap!");
            RuleFor(x => x.RelatorioPeriodicoAlunoId)
                   .NotEmpty()
                   .WithMessage("O id do relatório peridodico aluno deve ser informado para registar relatório periodico seção pap!");
        }
    }
}
