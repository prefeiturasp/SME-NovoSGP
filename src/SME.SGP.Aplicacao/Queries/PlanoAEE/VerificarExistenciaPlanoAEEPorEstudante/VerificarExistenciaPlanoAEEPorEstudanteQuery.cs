using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class VerificarExistenciaPlanoAEEPorEstudanteQuery : IRequest<PlanoAEEResumoDto>
    {
        public VerificarExistenciaPlanoAEEPorEstudanteQuery(FiltroEstudantePlanoAEEDto filtro)
        {
            Filtro = filtro;
        }

        public FiltroEstudantePlanoAEEDto Filtro { get; }
    }

    public class VerificarExistenciaPlanoAEEPorEstudanteQueryValidator : AbstractValidator<VerificarExistenciaPlanoAEEPorEstudanteQuery>
    {
        public VerificarExistenciaPlanoAEEPorEstudanteQueryValidator()
        {
            RuleFor(a => a.Filtro.CodigoEstudante)
                .NotEmpty()
                .WithMessage("O código do estudante/criança deve ser informado para consulta de seu Plano AEE");
            RuleFor(a => a.Filtro.CodigoUe)
                .NotEmpty()
                .WithMessage("O código da UE deve ser informado para consulta de seu Plano AEE");
        }
    }
}
