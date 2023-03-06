using FluentValidation;
using MediatR;
using System;

namespace SME.SGP.Aplicacao
{
    public class ProfessorPodePersistirTurmaQuery : IRequest<bool>
    {
        public string ProfessorRf { get; set; }
        public string CodigoTurma { get; set; }
        public DateTime DataAula { get; set; }

        public ProfessorPodePersistirTurmaQuery(string professorRf, string codigoTurma, DateTime dataAula)
        {
            ProfessorRf = professorRf;
            CodigoTurma = codigoTurma;
            DataAula = dataAula;
        }
    }

    public class ProfessorPodePersistirTurmaQueryValidator : AbstractValidator<ProfessorPodePersistirTurmaQuery>
    {
        public ProfessorPodePersistirTurmaQueryValidator()
        {
            RuleFor(x => x.ProfessorRf)
                .NotEmpty()
                .WithMessage("Informe o rf do profesor para verificação se o professor pode persistir na turma.");
            
            RuleFor(x => x.CodigoTurma)
                .NotEmpty()
                .WithMessage("Informe o código da turma para verificação se o professor pode persistir na turma.");
            
            RuleFor(x => x.DataAula)
                .NotEmpty()
                .WithMessage("Informe a data da aula para verificação se o professor pode persistir na turma.");
        }
    }
}
