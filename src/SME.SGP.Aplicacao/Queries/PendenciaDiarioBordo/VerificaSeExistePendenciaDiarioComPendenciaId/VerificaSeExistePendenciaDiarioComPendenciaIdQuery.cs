using FluentValidation;
using MediatR;

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
