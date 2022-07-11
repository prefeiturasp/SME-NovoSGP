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
                .WithMessage("O id da turma deve ser informado.");
            RuleFor(a => a.DisciplinaId)
                .NotEmpty()
                .WithMessage("O id da disciplina deve ser informado.");
            RuleFor(a => a.Bimestre)
                .NotEmpty()
                .WithMessage("O bimestre deve ser informado.");
        }
    }
}
