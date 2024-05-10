using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciasParametroEventoPorPendenciaEParametroQuery : IRequest<PendenciaParametroEvento>
    {
        public ObterPendenciasParametroEventoPorPendenciaEParametroQuery(long pendenciaId, long parametroId)
        {
            PendenciaId = pendenciaId;
            ParametroId = parametroId;
        }

        public long PendenciaId { get; set; }
        public long ParametroId { get; set; }
    }

    public class ObterPendenciasParametroEventoPorPendenciaEParametroQueryValidator : AbstractValidator<ObterPendenciasParametroEventoPorPendenciaEParametroQuery>
    {
        public ObterPendenciasParametroEventoPorPendenciaEParametroQueryValidator()
        {
            RuleFor(c => c.PendenciaId)
            .NotEmpty()
            .WithMessage("O id da pendência deve ser informado para pesquisa de pendência por parâmetro.");

            RuleFor(c => c.ParametroId)
            .NotEmpty()
            .WithMessage("O id do parâmetro deve ser informado para pesquisa de pendência por parâmetro.");

        }
    }
}
