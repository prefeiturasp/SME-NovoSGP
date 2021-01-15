using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciasParametroEventoPorPendenciaQuery : IRequest<IEnumerable<PendenciaParametroEventoDto>>
    {
        public ObterPendenciasParametroEventoPorPendenciaQuery(long pendenciaId)
        {
            PendenciaId = pendenciaId;
        }

        public long PendenciaId { get; set; }
    }

    public class ObterPendenciasParametroEventoPorPendenciaQueryValidator : AbstractValidator<ObterPendenciasParametroEventoPorPendenciaQuery>
    {
        public ObterPendenciasParametroEventoPorPendenciaQueryValidator()
        {
            RuleFor(c => c.PendenciaId)
            .NotEmpty()
            .WithMessage("O id da pendência deve ser informado para pesquisa dos eventos não cadastrados.");

        }
    }
}
