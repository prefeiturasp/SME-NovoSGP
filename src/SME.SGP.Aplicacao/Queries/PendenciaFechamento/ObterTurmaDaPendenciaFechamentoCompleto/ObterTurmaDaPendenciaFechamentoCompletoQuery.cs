using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmaDaPendenciaFechamentoCompletoQuery : IRequest<PendenciaFechamentoCompletoDto>
    {
        public ObterTurmaDaPendenciaFechamentoCompletoQuery(long pendenciaId)
        {
            PendenciaId = pendenciaId;
        }

        public long PendenciaId { get; set; }
    }

    public class ObterTurmaDaPendenciaFechamentoCompletoQueryValidator : AbstractValidator<ObterTurmaDaPendenciaFechamentoCompletoQuery>
    {
        public ObterTurmaDaPendenciaFechamentoCompletoQueryValidator()
        {
            RuleFor(c => c.PendenciaId)
            .NotEmpty()
            .WithMessage("O id da pendencia deve ser informado para a busca da pendência completa.");
        }
    }
}
