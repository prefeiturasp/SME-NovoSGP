using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExisteConselhoClasseParaTurmaQuery : IRequest<bool>
    {
        public ExisteConselhoClasseParaTurmaQuery(string[] codigosTurmas, int bimestre)
        {
            CodigosTurmas = codigosTurmas;
            Bimestre = bimestre;
        }

        public string[] CodigosTurmas { get; set; }

        public int Bimestre { get; set; }
    }

    public class ExisteConselhoClasseParaTurmaQueryValidator : AbstractValidator<ExisteConselhoClasseParaTurmaQuery>
    {

        public ExisteConselhoClasseParaTurmaQueryValidator()
        {
            RuleFor(c => c.CodigosTurmas)
                .NotEmpty() 
                .NotNull()
                .WithMessage("A turma deve ser informado para a busca do conselho de classe.");
        }
    }
}
