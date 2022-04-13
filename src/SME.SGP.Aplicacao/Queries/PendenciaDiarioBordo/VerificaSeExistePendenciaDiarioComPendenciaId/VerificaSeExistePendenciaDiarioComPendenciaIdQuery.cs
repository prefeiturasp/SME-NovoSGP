using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class VerificaSeExistePendenciaDiarioComPendenciaIdQuery : IRequest<bool>
    {
        public VerificaSeExistePendenciaDiarioComPendenciaIdQuery(long pendenciaId)
        {
            PendenciaId = pendenciaId;
        }
        public long PendenciaId { get; set; }
    }


    public class VerificaSeExistePendenciaDiarioComPendenciaIdQueryValidator : AbstractValidator<VerificaSeExistePendenciaDiarioComPendenciaIdQuery>
    {
        public VerificaSeExistePendenciaDiarioComPendenciaIdQueryValidator()
        {
            RuleFor(c => c.PendenciaId)
               .NotEmpty()
               .WithMessage("O Id da pendencia ser informado para obter a pendência de diário de bordo.");

        }
    }
}
