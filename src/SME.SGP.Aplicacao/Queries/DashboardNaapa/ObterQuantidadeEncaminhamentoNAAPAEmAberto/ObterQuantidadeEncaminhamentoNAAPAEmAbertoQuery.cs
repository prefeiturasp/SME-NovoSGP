﻿using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterQuantidadeEncaminhamentoNAAPAEmAbertoQuery : IRequest<GraficoEncaminhamentoNAAPADto>
    {
        public ObterQuantidadeEncaminhamentoNAAPAEmAbertoQuery(int anoLetivo, long? dreId)
        {
            AnoLetivo = anoLetivo;
            DreId = dreId;
        }

        public int AnoLetivo { get; set; }
        public long? DreId { get; set; }
    }

    public class ObterQuantidadeEncaminhamentoNAAPAEmAbertoQueryValidator : AbstractValidator<ObterQuantidadeEncaminhamentoNAAPAEmAbertoQuery>
    {
        public ObterQuantidadeEncaminhamentoNAAPAEmAbertoQueryValidator()
        {
            RuleFor(c => c.AnoLetivo)
                .GreaterThan(0)
                .WithMessage("O ano letivo deve ser informado.");
        }
    }
}
