using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class GerarPendenciaValidacaoPlanoAEECommand : IRequest<bool>
    {
        public long PlanoAEEId { get; set; }
        public PerfilUsuario PerfilUsuario { get; set; }

        public GerarPendenciaValidacaoPlanoAEECommand(long planoAEEId, PerfilUsuario pendenciaPerfil)
        {
            PlanoAEEId = planoAEEId;
            PerfilUsuario = pendenciaPerfil;
        }
    }

    public class GerarPendenciaValidacaoPlanoAEECommandValidator : AbstractValidator<GerarPendenciaValidacaoPlanoAEECommand>
    {
        public GerarPendenciaValidacaoPlanoAEECommandValidator()
        {

            RuleFor(c => c.PlanoAEEId)
               .NotEmpty()
               .WithMessage("O id do plano AEE precisa ser informado.");
        }
    }
}
