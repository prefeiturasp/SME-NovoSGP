using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Linq;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciaTurmaPAPQuery : IRequest<FrequenciaAlunoDto>
    {
        public ObterFrequenciaTurmaPAPQuery(string codigoTurma, string codigoAluno, PeriodoRelatorioPAP periodoRelatorio)
        {
            CodigoTurma = codigoTurma;
            CodigoAluno = codigoAluno;
            PeriodoRelatorio = periodoRelatorio;
        }

        public string CodigoTurma { get; set; }
        public string CodigoAluno { get; set; }
        public PeriodoRelatorioPAP PeriodoRelatorio { get; set; }
    }

    public class ObterFrequenciaTurmaPAPQueryValidator : AbstractValidator<ObterFrequenciaTurmaPAPQuery>
    {
        public ObterFrequenciaTurmaPAPQueryValidator()
        {
            RuleFor(x => x.CodigoTurma)
                .NotEmpty()
                .WithMessage("O código da turma deve ser informado para frequência turma pap.");

            RuleFor(x => x.CodigoAluno)
                .NotEmpty()
                .WithMessage("O código do aluno deve ser informado para frequência turma pap.");

            RuleFor(x => x.PeriodoRelatorio)
                .NotNull()
                .WithMessage("O período do relatório pap deve ser informado para frequência turma pap.");

            RuleFor(x => x.PeriodoRelatorio.PeriodosEscolaresRelatorio)
                .Must(x => x != null && x.Any())
                .When(x => x.PeriodoRelatorio != null )
                .WithMessage("A lista de períodos escolares do período do relatório pap deve ser informada.");
        }
    }
}
