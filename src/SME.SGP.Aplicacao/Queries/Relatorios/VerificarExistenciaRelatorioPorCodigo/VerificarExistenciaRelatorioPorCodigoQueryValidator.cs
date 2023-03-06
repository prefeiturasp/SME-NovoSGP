using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class VerificarExistenciaRelatorioPorCodigoQueryValidator : AbstractValidator<VerificarExistenciaRelatorioPorCodigoQuery>
    {
        public VerificarExistenciaRelatorioPorCodigoQueryValidator()
        {
            RuleFor(c => c.CodigoRelatorio)
                .NotEmpty()
                .WithMessage("O Código do Relatório deve ser informado.");
        }
    }
}
