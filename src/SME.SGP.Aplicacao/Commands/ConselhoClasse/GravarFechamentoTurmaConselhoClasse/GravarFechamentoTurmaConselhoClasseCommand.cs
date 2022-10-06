using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class GravarFechamentoTurmaConselhoClasseCommand : IRequest<bool>
    {
        public FechamentoTurma FechamentoDeTurma { get; set; }
        public FechamentoTurmaDisciplina FechamentoDeTurmaDisciplina { get; set; }
        public int? Bimestre { get; set; }

        public GravarFechamentoTurmaConselhoClasseCommand(
                        FechamentoTurma fechamentoDeTurma, 
                        FechamentoTurmaDisciplina fechamentoDeTurmaDisciplina,
                        int? bimestre)
        {
            FechamentoDeTurma = fechamentoDeTurma;
            FechamentoDeTurmaDisciplina = fechamentoDeTurmaDisciplina;
            Bimestre = bimestre;
        }
    }

    public class GravarFechamentoTurmaConselhoClasseCommandValidator : AbstractValidator<GravarFechamentoTurmaConselhoClasseCommand>
    {
        public GravarFechamentoTurmaConselhoClasseCommandValidator()
        {
            RuleFor(c => c.FechamentoDeTurma)
               .NotNull()
               .NotEmpty()
               .WithMessage("O fechamento de turma deve ser informado para efetuar a gravação.");

            RuleFor(c => c.FechamentoDeTurmaDisciplina)
               .NotNull()
               .NotEmpty()
               .WithMessage("O fechamento de turma disciplina deve ser informado para efetuar a gravação.");

            RuleFor(c => c.Bimestre)
               .NotNull()
               .NotEmpty()
               .WithMessage("O bimestre deve ser informado para efetuar a gravação.");
        }
    }
}
