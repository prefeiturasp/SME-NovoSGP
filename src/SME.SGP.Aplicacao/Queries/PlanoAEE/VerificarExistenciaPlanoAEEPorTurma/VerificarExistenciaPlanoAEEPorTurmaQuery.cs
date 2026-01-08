using System.Collections.Generic;
using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PlanoAEE;

namespace SME.SGP.Aplicacao.Queries.PlanoAEE.VerificarExistenciaPlanoAEEPorTurma
{
    public class VerificarExistenciaPlanoAEEPorTurmaQuery : IRequest<IEnumerable<PlanoAEEResumoIntegracaoDto>>
    {
        public VerificarExistenciaPlanoAEEPorTurmaQuery(FiltroTurmaPlanoAEEDto filtro)
        {
            Filtro = filtro;
        }
        public FiltroTurmaPlanoAEEDto Filtro { get; set; }   
    }
    public class VerificarExistenciaPlanoAEEPorTurmaQueryValidator : AbstractValidator<VerificarExistenciaPlanoAEEPorTurmaQuery>
    {
        public VerificarExistenciaPlanoAEEPorTurmaQueryValidator()
        {
            RuleFor(a => a.Filtro.CodigoTurma)
                .NotEmpty()
                .WithMessage("O código da Turma deve ser informado para consulta de seu Plano AEE");
        }
    }
}