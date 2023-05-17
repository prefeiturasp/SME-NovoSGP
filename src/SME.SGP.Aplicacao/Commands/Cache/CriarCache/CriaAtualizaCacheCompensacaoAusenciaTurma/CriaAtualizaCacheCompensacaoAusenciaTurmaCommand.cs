using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class CriaAtualizaCacheCompensacaoAusenciaTurmaCommand : IRequest<bool>
    {
        public CriaAtualizaCacheCompensacaoAusenciaTurmaCommand(string codigoTurma, int bimestre)
        {
            CodigoTurma = codigoTurma;
            Bimestre = bimestre;
        }

        public string CodigoTurma { get; set; }
        public int Bimestre { get; set; }
    }

    public class CriaAtualizaCacheCompensacaoAusenciaTurmaCommandValidator : AbstractValidator<CriaAtualizaCacheCompensacaoAusenciaTurmaCommand>
    {
        public CriaAtualizaCacheCompensacaoAusenciaTurmaCommandValidator()
        {
            RuleFor(x => x.CodigoTurma)
                .NotNull()
                .NotEmpty()
                .WithMessage("O código da turma deve ser informado.");

            RuleFor(x => x.Bimestre)
                .InclusiveBetween(1, 4)
                .WithMessage("O bimestre informado é inválido");
        }
    }
}
