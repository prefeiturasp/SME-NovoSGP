using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciasUsuarioPorPendenciaUsuarioIdQuery : IRequest<bool>
    {
        public ObterPendenciasUsuarioPorPendenciaUsuarioIdQuery(long pendenciaId, long usuarioId)
        {
            PendenciaId = pendenciaId;
            UsuarioId = usuarioId;
        }

        public long PendenciaId { get; set; }
        public long UsuarioId { get; set; }
    }
    public class ObterPendenciasUsuarioPorPendenciaUsuarioIdQueryValidator : AbstractValidator<ObterPendenciasUsuarioPorPendenciaUsuarioIdQuery>
    {
        public ObterPendenciasUsuarioPorPendenciaUsuarioIdQueryValidator()
        {
            RuleFor(c => c.PendenciaId)
            .NotEmpty()
            .WithMessage("O id da pendencia deve ser informado.");

            RuleFor(c => c.UsuarioId)
           .NotEmpty()
           .WithMessage("O id do usuário deve ser informado.");
        }

    }

}
