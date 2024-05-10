using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class VerificarExistenciaPlanoAEEPorEstudanteQuery : IRequest<PlanoAEEResumoDto>
    {
        public VerificarExistenciaPlanoAEEPorEstudanteQuery(string codigoEstudante)
        {
            CodigoEstudante = codigoEstudante;
        }

        public string CodigoEstudante { get; }
    }

    public class VerificarExistenciaPlanoAEEPorEstudanteQueryValidator : AbstractValidator<VerificarExistenciaPlanoAEEPorEstudanteQuery>
    {
        public VerificarExistenciaPlanoAEEPorEstudanteQueryValidator()
        {
            RuleFor(a => a.CodigoEstudante)
                .NotEmpty()
                .WithMessage("O código do estudante/criança deve ser informado para consulta de seu Plano AEE");
        }
    }
}
