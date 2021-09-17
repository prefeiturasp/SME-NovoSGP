using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterNotasPorBimestresUeAlunoTurmaQuery : IRequest<IEnumerable<NotaConceitoBimestreComponenteDto>>
    {
        public ObterNotasPorBimestresUeAlunoTurmaQuery(int[] bimestres, string turmaCodigo, string ueCodigo, string alunoCodigo)
        {
            Bimestres = bimestres;
            TurmaCodigo = turmaCodigo;
            UeCodigo = ueCodigo;
            AlunoCodigo = alunoCodigo;
        }

        public int[] Bimestres { get; set; }
        public string TurmaCodigo { get; set; }
        public string UeCodigo { get; set; }
        public string AlunoCodigo { get; set; }
    }

    public class ObterNotasPorBimestresUeAlunoTurmaQueryValidator : AbstractValidator<ObterNotasPorBimestresUeAlunoTurmaQuery>
    {
        public ObterNotasPorBimestresUeAlunoTurmaQueryValidator()
        {
            RuleFor(a => a.Bimestres)
                .NotNull()
                .NotEmpty()
                .WithMessage("Necessário informar os bimestres");
            RuleFor(a => a.TurmaCodigo)
                .NotNull()
                .NotEmpty()
                .WithMessage("Necessário informar o código da turma");
            RuleFor(a => a.UeCodigo)
               .NotNull()
               .NotEmpty()
               .WithMessage("Necessário informar o código da Ue");
            RuleFor(a => a.AlunoCodigo)
               .NotNull()
               .NotEmpty()
               .WithMessage("Necessário informar o código do aluno");
        }
    }
}
