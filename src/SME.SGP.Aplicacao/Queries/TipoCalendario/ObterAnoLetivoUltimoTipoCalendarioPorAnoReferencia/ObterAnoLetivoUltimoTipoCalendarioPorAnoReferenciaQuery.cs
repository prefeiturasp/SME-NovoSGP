using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterAnoLetivoUltimoTipoCalendarioPorAnoReferenciaQuery : IRequest<int>
    {
        public ObterAnoLetivoUltimoTipoCalendarioPorAnoReferenciaQuery(int anoReferencia, ModalidadeTipoCalendario modalidadeTipoCalendario)
        {
            AnoReferencia = anoReferencia;
            ModalidadeTipoCalendario = modalidadeTipoCalendario;
        }
        public int AnoReferencia { get; set; }
        public ModalidadeTipoCalendario ModalidadeTipoCalendario { get; set; }
    }

    public class ObterAnoLetivoUltimoTipoCalendarioPorAnoReferenciaQueryValidator : AbstractValidator<ObterAnoLetivoUltimoTipoCalendarioPorAnoReferenciaQuery>
    {
        public ObterAnoLetivoUltimoTipoCalendarioPorAnoReferenciaQueryValidator()
        {
            RuleFor(x => x.AnoReferencia)
                .NotEmpty()
                .WithMessage("Ano de referência deve ser informado.");

            RuleFor(x => x.AnoReferencia)
                .GreaterThanOrEqualTo(2014)
                .WithMessage("Ano de referência deve ser maior que 2014.");

            RuleFor(x => x.ModalidadeTipoCalendario)
                .IsInEnum()
                .WithMessage("A modalidade do tipo de calendário deve ser informada.");
        }
    }
}
