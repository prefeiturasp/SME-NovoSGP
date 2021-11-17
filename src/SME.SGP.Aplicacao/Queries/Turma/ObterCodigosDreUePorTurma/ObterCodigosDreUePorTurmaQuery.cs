using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterCodigosDreUePorTurmaQuery : IRequest<DreUeDto>
    {
        public ObterCodigosDreUePorTurmaQuery(string turmaCodigo)
        {
            TurmaCodigo = turmaCodigo;
        }

        public string TurmaCodigo { get; }
    }

    public class ObteCodigosDreUePorTurmaQueryValidator : AbstractValidator<ObterCodigosDreUePorTurmaQuery>
    {
        public ObteCodigosDreUePorTurmaQueryValidator()
        {
            RuleFor(a => a.TurmaCodigo)
                .NotEmpty()
                .WithMessage("O código da Turma é necessário para consulta da DRE e UE");
        }
    }
}
