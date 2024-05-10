using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterRelatorioPeriodicoAlunoPAPQuery : IRequest<RelatorioPeriodicoPAPAluno>
    {
        public ObterRelatorioPeriodicoAlunoPAPQuery(long relatorioAlunoId)
        {
            RelatorioAlunoId = relatorioAlunoId;
        }

        public long RelatorioAlunoId { get; set; }
    }

    public class ObterRelatorioPeriodicoAlunoPAPQueryValidator : AbstractValidator<ObterRelatorioPeriodicoAlunoPAPQuery>
    {
        public ObterRelatorioPeriodicoAlunoPAPQueryValidator()
        {
            RuleFor(x => x.RelatorioAlunoId)
                .NotEmpty()
                .WithMessage("O id relatório periodico aluno pap deve ser informado para busca.");
        }
    }
}
