using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciasFechamentoIdDisciplinaQuery : IRequest<IEnumerable<PendenciaFechamento>>
    {
        public ObterPendenciasFechamentoIdDisciplinaQuery(long fechamentoId, long disciplinaId)
        {
            FechamentoId = fechamentoId;
            DisciplinaId = disciplinaId;
        }

        public long FechamentoId { get; set; }
        public long DisciplinaId { get; set; }
    }

    public class ObterPendenciasFechamentoIdDisciplinaQueryValidator : AbstractValidator<ObterPendenciasFechamentoIdDisciplinaQuery>
    {
        public ObterPendenciasFechamentoIdDisciplinaQueryValidator()
        {
            RuleFor(c => c.FechamentoId)
            .NotEmpty()
            .WithMessage("O id do fechamento deve ser informado.");

            RuleFor(c => c.DisciplinaId)
            .NotEmpty()
            .WithMessage("O id do componente curricular deve ser informada.");
        }
    }
}
