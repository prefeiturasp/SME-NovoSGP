using FluentValidation;
using MediatR;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.Aplicacao
{
    public class ObterDevolutivaPorTurmaQuery : IRequest<ConsolidacaoDevolutivaTurmaDTO>
    {
        public ObterDevolutivaPorTurmaQuery(string turmaCodigo)
        {
            TurmaCodigo = turmaCodigo;
        }

        public string TurmaCodigo { get; set; }
    }

    public class ObterDevolutivaPorTurmaQueryValidator : AbstractValidator<ObterDevolutivaPorTurmaQuery>
    {
        public ObterDevolutivaPorTurmaQueryValidator()
        {
            RuleFor(c => c.TurmaCodigo)
            .NotEmpty()
            .WithMessage("O Código da Turma deve ser informado para consulta de devolutivas.");
        }
    }
}
