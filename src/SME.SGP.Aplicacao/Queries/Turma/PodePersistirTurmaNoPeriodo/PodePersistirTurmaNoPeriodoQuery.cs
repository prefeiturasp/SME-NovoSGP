using System;
using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class PodePersistirTurmaNoPeriodoQuery: IRequest<bool>
    {
        public PodePersistirTurmaNoPeriodoQuery(string professorRf, string codigoTurma, long componenteCurricularId, DateTime dataInicio, DateTime dataFim)
        {
            ProfessorRf = professorRf;
            CodigoTurma = codigoTurma;
            ComponenteCurricularId = componenteCurricularId;
            DataInicio = dataInicio;
            DataFim = dataFim;
        }

        public string ProfessorRf { get; set; }
        public string CodigoTurma { get; set; }
        public long ComponenteCurricularId { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
    }

    public class PodePersistirTurmaNoPeriodoQueryValidator: AbstractValidator<PodePersistirTurmaNoPeriodoQuery>
    {
        public PodePersistirTurmaNoPeriodoQueryValidator()
        {
            RuleFor(c => c.ProfessorRf)
                .NotEmpty()
                .WithMessage("O Rf do professor deve ser informado para validar se pode persistir na turma.");
            
            RuleFor(c => c.CodigoTurma)
                .NotEmpty()
                .WithMessage("O código da turma deve ser informado para validar se pode persistir na turma.");
            
            RuleFor(c => c.ComponenteCurricularId)
                .GreaterThan(0)
                .WithMessage("O componente curricular deve ser informado para validar se pode persistir na turma.");
            
            RuleFor(c => c.DataInicio)
                .NotNull()
                .WithMessage("A data início deve ser informada para validar se pode persistir na turma.");
            
            RuleFor(c => c.DataFim)
                .NotNull()
                .WithMessage("A data final deve ser informada para validar se pode persistir na turma.");
        }
    }
}
