using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciaIdPorTurmaQuery : IRequest<long>
    {
        public ObterPendenciaIdPorTurmaQuery(long turmaId, TipoPendencia tipoPendencia)
        {
            TurmaId = turmaId;
            TipoPendencia = tipoPendencia;
        }

        public long TurmaId { get; set; }
        public TipoPendencia TipoPendencia { get; set; }
    }

    public class ObterPendenciaIdPorTurmaQueryValidator : AbstractValidator<ObterPendenciaIdPorTurmaQuery>
    {
        public ObterPendenciaIdPorTurmaQueryValidator()
        {
            RuleFor(c => c.TurmaId)
               .NotEmpty()
               .WithMessage("O id da turma deve ser informado para localizar a pendência.");

        }
    }
}
