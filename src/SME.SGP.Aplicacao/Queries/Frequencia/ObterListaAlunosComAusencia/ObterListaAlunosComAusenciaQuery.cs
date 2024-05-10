using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterListaAlunosComAusenciaQuery : IRequest<IEnumerable<AlunoAusenteDto>>
    {
        public ObterListaAlunosComAusenciaQuery(string turmaId, string disciplinaId, int bimestre)
        {
            TurmaId = turmaId;
            DisciplinaId = disciplinaId;
            Bimestre = bimestre;
        }

        public string TurmaId { get; set; } 
        public string DisciplinaId { get; set; }  
        public int Bimestre { get; set; }
    }

    public class ObterListaAlunosComAusenciaQueryValidator : AbstractValidator<ObterListaAlunosComAusenciaQuery>
    {
        public ObterListaAlunosComAusenciaQueryValidator()
        {
            RuleFor(a => a.TurmaId)
                .NotEmpty()
                .WithMessage("É preciso informar o id da turma para consultar os alunos com ausência.");
            RuleFor(a => a.DisciplinaId)
                .NotEmpty()
                .WithMessage("É preciso informar o id da disciplina para consultar os alunos com ausência.");
            RuleFor(a => a.Bimestre)
                .NotEmpty()
                .WithMessage("É preciso informar o bimestre para consultar os alunos com ausência.");
        }
    }
}
