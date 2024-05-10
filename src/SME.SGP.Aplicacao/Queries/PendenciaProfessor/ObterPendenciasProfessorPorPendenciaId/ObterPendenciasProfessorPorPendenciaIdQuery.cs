using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciasProfessorPorPendenciaIdQuery : IRequest<IEnumerable<PendenciaProfessorDto>>
    {
        public ObterPendenciasProfessorPorPendenciaIdQuery(long pendenciaId)
        {
            PendenciaId = pendenciaId;
        }

        public long PendenciaId { get; set; }
    }

    public class ObterPendenciasAusenciaAvaliacaoPorPendenciaIdQueryValidator : AbstractValidator<ObterPendenciasProfessorPorPendenciaIdQuery>
    {
        public ObterPendenciasAusenciaAvaliacaoPorPendenciaIdQueryValidator()
        {
            RuleFor(c => c.PendenciaId)
               .NotEmpty()
               .WithMessage("O id da pendência deve ser informado para consulta das pendencias do professor.");
        }
    }
}
