using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterCompensacoesPorAlunoETurmaQuery : IRequest<TotalCompensacaoAlunoPorCompensacaoIdDto>
    {
        public int Bimestre { get; set; }
        public string CodigoAluno { get; set; }
        public string DisciplinaCodigo { get; set; }
        public string TurmaCodigo { get; set; }

        public ObterCompensacoesPorAlunoETurmaQuery(int bimestre, string codigoAluno, string disciplinaCodigo, string turmaCodigo)
        {
            Bimestre = bimestre;
            CodigoAluno = codigoAluno;
            DisciplinaCodigo = disciplinaCodigo;
            TurmaCodigo = turmaCodigo;
        }
    }

    public class ObterCompensacoesPorAlunoQueryValidator : AbstractValidator<ObterCompensacoesPorAlunoETurmaQuery>
    {
        public ObterCompensacoesPorAlunoQueryValidator()
        {
            RuleFor(a => a.Bimestre)
                .NotEmpty().WithMessage("É necessário informar o bimestre para consultar a quantidade de compensações do aluno.");
            RuleFor(a => a.CodigoAluno)
               .NotEmpty().WithMessage("É necessário informar o código do aluno para consultar a quantidade de compensações.");
            RuleFor(a => a.DisciplinaCodigo)
               .NotEmpty().WithMessage("É necessário informar o código da disciplina para consultar a quantidade de compensações do aluno.");
            RuleFor(a => a.TurmaCodigo)
               .NotEmpty().WithMessage("É necessário informar o código da turma para consultar a quantidade de compensações do aluno.");
        }
    }
}
