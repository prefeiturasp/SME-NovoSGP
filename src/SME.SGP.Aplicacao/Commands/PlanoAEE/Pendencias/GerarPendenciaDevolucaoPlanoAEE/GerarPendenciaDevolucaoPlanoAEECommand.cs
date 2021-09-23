using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class GerarPendenciaDevolucaoPlanoAEECommand : IRequest<bool>
    {
        public GerarPendenciaDevolucaoPlanoAEECommand(long planoAEEId, string motivo)
        {
            PlanoAEEId = planoAEEId;
            Motivo = motivo;
        }

        public long PlanoAEEId { get; set; }
        public string Motivo { get; set; }
    }
    public class GerarPendenciaDevolucaoPlanoAEECommandValidator : AbstractValidator<GerarPendenciaDevolucaoPlanoAEECommand>
    {
        public GerarPendenciaDevolucaoPlanoAEECommandValidator()
        {
            RuleFor(c => c.PlanoAEEId)
               .NotEmpty()
               .WithMessage("O id do plano AEE deve ser informado.");

            RuleFor(c => c.Motivo)
               .NotEmpty()
               .WithMessage("O motivo da devolução deve ser informado.");
        }
    }
}
