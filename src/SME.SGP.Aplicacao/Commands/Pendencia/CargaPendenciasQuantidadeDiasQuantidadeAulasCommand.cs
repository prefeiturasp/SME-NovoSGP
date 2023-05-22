using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class CargaPendenciasQuantidadeDiasQuantidadeAulasCommand : IRequest<bool>
    {
        public CargaPendenciasQuantidadeDiasQuantidadeAulasCommand(AulasDiasPendenciaDto carga)
        {
            Carga = carga;
        }

        public AulasDiasPendenciaDto Carga { get; set; }
    }

    public class CargaPendenciasQuantidadeDiasQuantidadeAulasCommandValidator : AbstractValidator<CargaPendenciasQuantidadeDiasQuantidadeAulasCommand>
    {
        public CargaPendenciasQuantidadeDiasQuantidadeAulasCommandValidator()
        {
            RuleFor(x => x.Carga.QuantidadeAulas).GreaterThan(0).WithMessage("Informe o ID da Pendência para Realizar a Carga");
        }
    }
}