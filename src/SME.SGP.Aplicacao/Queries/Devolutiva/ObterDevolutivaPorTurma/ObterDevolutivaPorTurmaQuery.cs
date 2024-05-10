using FluentValidation;
using MediatR;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.Aplicacao
{
    public class ObterDevolutivaPorTurmaQuery : IRequest<ConsolidacaoDevolutivaTurmaDTO>
    {
        public ObterDevolutivaPorTurmaQuery(string turmaCodigo, int anoLetivo)
        {
            TurmaCodigo = turmaCodigo;
            AnoLetivo = anoLetivo;
        }

        public string TurmaCodigo { get; set; }
        public int AnoLetivo { get; set; }
    }

    public class ObterDevolutivaPorTurmaQueryValidator : AbstractValidator<ObterDevolutivaPorTurmaQuery>
    {
        public ObterDevolutivaPorTurmaQueryValidator()
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
