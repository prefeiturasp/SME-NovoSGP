using FluentValidation;
using MediatR;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.Aplicacao
{
    public class ObterDiariosDeBordoPorAnoLetivoTurmaQuery : IRequest<QuantidadeDiarioBordoRegistradoPorAnoletivoTurmaDTO>
    {
        public ObterDiariosDeBordoPorAnoLetivoTurmaQuery(string turmaCodigo, int anoLetivo)
        {
            TurmaCodigo = turmaCodigo;
            AnoLetivo = anoLetivo;
        }

        public string TurmaCodigo { get; set; }
        public int AnoLetivo { get; set; }
    }

    public class ObterDiariosDeBordoPorAnoLetivoTurmaQueryValidator : AbstractValidator<ObterDevolutivaPorTurmaQuery>
    {
        public ObterDiariosDeBordoPorAnoLetivoTurmaQueryValidator()
        {
            RuleFor(c => c.TurmaCodigo)
            .NotEmpty()
            .WithMessage("O Código da Turma deve ser informado para consulta de devolutivas.");

            RuleFor(c => c.AnoLetivo)
            .NotEmpty()
            .WithMessage("O Ano Letivo deve ser informado para consulta de devolutivas.");
        }
    }
}
