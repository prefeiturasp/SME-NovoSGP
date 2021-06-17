using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class ObterMiniaturaFotoEstudantePorAlunoCodigoQueryValidator : AbstractValidator<ObterMiniaturaFotoEstudantePorAlunoCodigoQuery>
    {
        public ObterMiniaturaFotoEstudantePorAlunoCodigoQueryValidator()
        {
            RuleFor(x => x.AlunoCodigo)
                .NotEmpty()
                .WithMessage("O código do aluno deve ser informado.");
        }
    }
}