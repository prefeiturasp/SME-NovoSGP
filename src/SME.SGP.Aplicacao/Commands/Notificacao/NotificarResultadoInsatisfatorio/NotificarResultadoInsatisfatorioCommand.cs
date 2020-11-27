using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class NotificarResultadoInsatisfatorioCommand : IRequest<bool>
    {
        public NotificarResultadoInsatisfatorioCommand(int dias, long modalidadeTipoCalendario)
        {
            Dias = dias;
            ModalidadeTipoCalendario = modalidadeTipoCalendario;
        }

        public int Dias { get; set; }
        public long ModalidadeTipoCalendario { get; set; }

    }

    public class NotificarResultadoInsatisfatorioCommandValidator : AbstractValidator<NotificarResultadoInsatisfatorioCommand>
    {
        public NotificarResultadoInsatisfatorioCommandValidator()
        {
            RuleFor(c => c.Dias)
               .NotEmpty()
               .WithMessage("Os dias precisam ser informados para o limite.");

            RuleFor(c => c.ModalidadeTipoCalendario)
              .NotEmpty()
              .WithMessage("A ModalidadeTipoCalendario precisa ser informado.");
        }
    }
}
