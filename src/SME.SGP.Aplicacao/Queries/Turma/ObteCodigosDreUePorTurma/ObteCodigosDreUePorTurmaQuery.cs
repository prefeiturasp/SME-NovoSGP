using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObteCodigosDreUePorTurmaQuery : IRequest<DreUeDaTurmaDto>
    {
        public ObteCodigosDreUePorTurmaQuery(string turmaCodigo)
        {
            TurmaCodigo = turmaCodigo;
        }

        public string TurmaCodigo { get; }
    }

    public class ObteCodigosDreUePorTurmaQueryValidator : AbstractValidator<ObteCodigosDreUePorTurmaQuery>
    {
        public ObteCodigosDreUePorTurmaQueryValidator()
        {
            RuleFor(a => a.TurmaCodigo)
                .NotEmpty()
                .WithMessage("O código da Turma é necessário para consulta da DRE e UE");
        }
    }
}
