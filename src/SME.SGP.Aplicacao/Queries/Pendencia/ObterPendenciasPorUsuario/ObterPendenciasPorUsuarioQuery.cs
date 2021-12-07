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
        public ObterPendenciasPorUsuarioQuery(long usuarioId, string turmaId, int? tipoPendencia, string tituloPendencia)
        {
            UsuarioId = usuarioId;
            TurmaId = turmaId;
            TipoPendencia = tipoPendencia;
            TituloPendencia = tituloPendencia;
        }

        public long UsuarioId { get; set; }

        public string TurmaId { get; set; }

        public int? TipoPendencia { get; set; }

        public string TituloPendencia { get; set; }
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
