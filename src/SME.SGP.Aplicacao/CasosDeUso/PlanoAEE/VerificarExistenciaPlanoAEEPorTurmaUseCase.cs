using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PlanoAEE;
using SME.SGP.Infra.Dtos.PlanoAEE;
using SME.SGP.Aplicacao.Queries.PlanoAEE.VerificarExistenciaPlanoAEEPorTurma;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;

namespace SME.SGP.Aplicacao
{
    public class VerificarExistenciaPlanoAEEPorTurmaUseCase : AbstractUseCase, IVerificarExistenciaPlanoAEEPorTurmaUseCase
    {
        public VerificarExistenciaPlanoAEEPorTurmaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<PlanoAEEResumoIntegracaoDto>> Executar(FiltroTurmaPlanoAEEDto param)
        {
            var planoAee = await mediator.Send(new VerificarExistenciaPlanoAEEPorTurmaQuery(param));

            return planoAee.EhNulo() ? throw new NegocioException(MensagemNegocioPlanoAee.NENHUM_PLANO_AEE_ENCONTRADO_PARA_TURMA) : planoAee;
        }
    }
}