using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciasPorUsuarioQuery : IRequest<PaginacaoResultadoDto<PendenciaDto>>
    {
        public ObterPendenciasPorUsuarioQuery(long usuarioId)
        {
            UsuarioId = usuarioId;
        }

        public long UsuarioId { get; set; }
    }

    public class ObterPendenciasPorUsuarioQueryValidator : AbstractValidator<ObterPendenciasPorUsuarioQuery>
    {
        public ObterPendenciasPorUsuarioQueryValidator()
        {
            RuleFor(c => c.UsuarioId)
            .NotEmpty()
            .WithMessage("O id do usuário deve ser informado para consulta de suas pendências.");

        }
    }
}
