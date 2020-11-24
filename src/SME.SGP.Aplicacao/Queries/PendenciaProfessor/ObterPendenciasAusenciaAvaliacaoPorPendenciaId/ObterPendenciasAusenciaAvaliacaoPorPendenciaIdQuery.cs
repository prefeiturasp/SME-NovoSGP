using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciasAusenciaAvaliacaoPorPendenciaIdQuery : IRequest<IEnumerable<PendenciaProfessorDto>>
    {
        public ObterPendenciasAusenciaAvaliacaoPorPendenciaIdQuery(long pendenciaId)
        {
            PendenciaId = pendenciaId;
        }

        public long PendenciaId { get; set; }
    }

    public class ObterPendenciasAusenciaAvaliacaoPorPendenciaIdQueryValidator : AbstractValidator<ObterPendenciasAusenciaAvaliacaoPorPendenciaIdQuery>
    {
        public ObterPendenciasAusenciaAvaliacaoPorPendenciaIdQueryValidator()
        {
            RuleFor(c => c.PendenciaId)
               .NotEmpty()
               .WithMessage("O id da pendência deve ser informado para consulta das pendencias do professor.");
        }
    }
}
