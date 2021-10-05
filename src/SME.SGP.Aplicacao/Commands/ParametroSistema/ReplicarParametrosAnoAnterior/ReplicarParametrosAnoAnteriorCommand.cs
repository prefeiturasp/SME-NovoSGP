using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ReplicarParametrosAnoAnteriorCommand : IRequest<bool>
    {
        public ReplicarParametrosAnoAnteriorCommand(int anoLetivo, ModalidadeTipoCalendario modalidadeTipoCalendario)
        {
            AnoLetivo = anoLetivo;
            ModalidadeTipoCalendario = modalidadeTipoCalendario;
        }

        public int AnoLetivo { get; set; }
        public ModalidadeTipoCalendario ModalidadeTipoCalendario { get; set; }        
    }

    public class ReplicarParametrosAnoAnteriorCommandValidator : AbstractValidator<ReplicarParametrosAnoAnteriorCommand>
    {
        public ReplicarParametrosAnoAnteriorCommandValidator()
        {
            RuleFor(a => a.AnoLetivo)
                .NotEmpty()
                .WithMessage("O ano deve ser informado!");

            RuleFor(a => a.ModalidadeTipoCalendario)
                .IsInEnum()
                .WithMessage("A modalidade do tipo de calendário deve ser informada.");
        }
    }
}
