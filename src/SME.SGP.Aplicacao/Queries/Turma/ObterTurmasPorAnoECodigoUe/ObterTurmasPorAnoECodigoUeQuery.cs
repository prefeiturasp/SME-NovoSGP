using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasPorAnoECodigoUeQuery : IRequest<IEnumerable<TurmaNaoHistoricaDto>>
    {
        public long UsuarioId { get; set; }
        public int AnoLetivo { get; set; }

        public ObterTurmasPorAnoECodigoUeQuery(long usuarioId, int anoLetivo)
        {
            UsuarioId = usuarioId;
            AnoLetivo = anoLetivo;
        }
    }
    public class ObterTurmasPorAnoECodigoUeQueryValidator : AbstractValidator<ObterTurmasPorAnoECodigoUeQuery>
    {
        public ObterTurmasPorAnoECodigoUeQueryValidator()
        {
            RuleFor(c => c.UsuarioId)
               .NotEmpty()
               .WithMessage("O id do usuário deve ser informado para consulta das turmas.");

            RuleFor(c => c.AnoLetivo)
               .NotEmpty()
               .WithMessage("O ano letivo deve ser informado para consulta das turmas.");
        }
    }
}
