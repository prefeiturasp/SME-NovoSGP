using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmaDaPendenciaFechamentoQuery : IRequest<Turma>
    {
        public ObterTurmaDaPendenciaFechamentoQuery(long pendenciaId)
        {
            PendenciaId = pendenciaId;
        }

        public long PendenciaId { get; set; }
    }

    public class ObterTurmaDaPendenciaFechamentoQueryValidator : AbstractValidator<ObterTurmaDaPendenciaFechamentoQuery>
    {
        public ObterTurmaDaPendenciaFechamentoQueryValidator()
        {
            RuleFor(c => c.PendenciaId)
            .NotEmpty()
            .WithMessage("O id da pendencia deve ser informado.");
        }
    }
}
