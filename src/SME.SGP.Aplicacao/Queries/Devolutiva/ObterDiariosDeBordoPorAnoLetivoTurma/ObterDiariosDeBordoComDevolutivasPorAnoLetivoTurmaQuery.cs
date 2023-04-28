using FluentValidation;
using MediatR;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.Aplicacao
{
    public class ObterDiariosDeBordoComDevolutivasPorAnoLetivoTurmaQuery : IRequest<QuantidadeDiarioBordoRegistradoPorAnoletivoTurmaDTO>
    {
        public ObterDiariosDeBordoComDevolutivasPorAnoLetivoTurmaQuery(long turmaId, int anoLetivo)
        {
            TurmaId = turmaId;
            AnoLetivo = anoLetivo;
        }

        public long TurmaId { get; set; }
        public int AnoLetivo { get; set; }
    }

    public class ObterDiariosDeBordoPorAnoLetivoTurmaQueryValidator : AbstractValidator<ObterDevolutivaPorTurmaQuery>
    {
        public ObterDiariosDeBordoPorAnoLetivoTurmaQueryValidator()
        {
            RuleFor(c => c.TurmaCodigo)
            .NotEmpty()
            .WithMessage("O identificador da Turma deve ser informado para consulta de devolutivas.");

            RuleFor(c => c.AnoLetivo)
            .NotEmpty()
            .WithMessage("O Ano Letivo deve ser informado para consulta de devolutivas.");
        }
    }
}
