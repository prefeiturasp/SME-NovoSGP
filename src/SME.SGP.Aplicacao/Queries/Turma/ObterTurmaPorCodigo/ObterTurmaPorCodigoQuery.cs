using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmaPorCodigoQuery : IRequest<Turma>
    {
        public ObterTurmaPorCodigoQuery() { }
        public ObterTurmaPorCodigoQuery(string turmaCodigo)
        {
            TurmaCodigo = turmaCodigo;
        }

        public string TurmaCodigo { get; set; }
    }

    public class ObterTurmaPorCodigoValidator : AbstractValidator<ObterTurmaPorCodigoQuery>
    {

        public ObterTurmaPorCodigoValidator()
        {
            RuleFor(c => c.TurmaCodigo)
                .NotEmpty()
                .WithMessage("O código da turma deve ser informado.");
        }
    }
}
