using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ExcluirAulaFuturaTerritorioDisponibilizadoCommand : IRequest<RetornoBaseDto>
    {
        public ExcluirAulaFuturaTerritorioDisponibilizadoCommand(long aulaId)
        {
            AulaId = aulaId;
        }

        public long AulaId { get; set; }
    }

    public class ExcluirAulaFuturaTerritorioDisponibilizadoCommandValidator : AbstractValidator<ExcluirAulaFuturaTerritorioDisponibilizadoCommand>
    {
        public ExcluirAulaFuturaTerritorioDisponibilizadoCommandValidator()
        {
            RuleFor(a => a.AulaId)
                .NotEmpty()
                .WithMessage("AulaId deve ser informado para a exclusão de aula.");
        }
    }
}
