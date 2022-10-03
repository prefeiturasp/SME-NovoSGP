using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterConselhoClasseIdsPorTurmaEBimestreQuery : IRequest<long[]>
    {
       public ObterConselhoClasseIdsPorTurmaEBimestreQuery(string[] turmasCodigos, long? bimestre)
        {
        TurmasCodigos = turmasCodigos;
        Bimestre = bimestre;
        }

        public string[] TurmasCodigos { get; set; }
        public long? Bimestre { get; set; }

        public class ObterConselhoClasseIdsPorTurmaEBimestreQueryValidator : AbstractValidator<ObterConselhoClasseIdsPorTurmaEBimestreQuery>
        {
            public ObterConselhoClasseIdsPorTurmaEBimestreQueryValidator()
            {
                RuleFor(c => c.TurmasCodigos)
               .NotEmpty()
                    .WithMessage("Os códigos de turma devem ser informados para obtenção dos ids de conselhos de classe.");
            }
        }
    }
}
