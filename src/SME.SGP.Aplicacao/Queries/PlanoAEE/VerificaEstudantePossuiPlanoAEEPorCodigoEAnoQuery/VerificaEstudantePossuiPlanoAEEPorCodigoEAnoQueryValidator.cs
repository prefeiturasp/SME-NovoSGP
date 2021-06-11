using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class VerificaEstudantePossuiPlanoAEEPorCodigoEAnoQueryValidator : AbstractValidator<VerificaEstudantePossuiPlanoAEEPorCodigoEAnoQuery>
    {
        public VerificaEstudantePossuiPlanoAEEPorCodigoEAnoQueryValidator()
        {
            RuleFor(a => a.CodigoEstudante)
                .NotEmpty()
                .WithMessage("O código do estudante/criança deve ser informado");
            RuleFor(a => a.AnoLetivo)
               .NotEmpty()
               .WithMessage("O ano letivo deve ser informado");
        }
    }
}
