using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterDiariosBordoPorDevolutivaQuery : IRequest<PaginacaoResultadoDto<DiarioBordoDevolutivaDto>>
    {
        public ObterDiariosBordoPorDevolutivaQuery(long devolutivaId)
        {
            DevolutivaId = devolutivaId;
        }

        public long DevolutivaId { get; set; }
    }

    public class ObterDiariosBordoPorDevolutivaQueryValidator : AbstractValidator<ObterDiariosBordoPorDevolutivaQuery>
    {
        public ObterDiariosBordoPorDevolutivaQueryValidator()
        {
            RuleFor(a => a.DevolutivaId)
                .NotEmpty()
                .WithMessage("Necessário informar o Id da devolutiva para consulta dos diários de bordo");
        }
    }
}
