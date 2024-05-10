using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesPorProfessorETurmaParaPlanejamentoQuery : IRequest<IEnumerable<DisciplinaDto>>
    {
        public long CodigoDisciplina { get; set; } 
        public string CodigoTurma { get; set; }
        public bool TurmaPrograma { get; set; }
        public bool Regencia { get; set; }

        public ObterComponentesCurricularesPorProfessorETurmaParaPlanejamentoQuery(long codigoDisciplina, string codigoTurma, bool turmaPrograma, bool regencia)
        {
            CodigoDisciplina = codigoDisciplina;
            CodigoTurma = codigoTurma;
            TurmaPrograma = turmaPrograma;
            Regencia = regencia;
        }
    }

    public class ObterComponentesCurricularesPorProfessorETurmaParaPlanejamentoQueryValidator : AbstractValidator<ObterComponentesCurricularesPorProfessorETurmaParaPlanejamentoQuery>
    {
        public ObterComponentesCurricularesPorProfessorETurmaParaPlanejamentoQueryValidator()
        {
            RuleFor(c => c.CodigoDisciplina)
                .GreaterThanOrEqualTo(0)
                .WithMessage("O código da disciplina deve ser informado para obter componentes curriculares por professor e turma.");

            RuleFor(c => c.CodigoTurma)
                .NotEmpty()
                .WithMessage("O código da turma deve ser informado para obter componentes curriculares por professor e turma.");
        }
    }
}
