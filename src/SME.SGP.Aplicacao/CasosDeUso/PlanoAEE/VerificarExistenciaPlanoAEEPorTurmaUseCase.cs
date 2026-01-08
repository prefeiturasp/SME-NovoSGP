using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PlanoAEE;
using SME.SGP.Infra.Dtos.PlanoAEE;
using SME.SGP.Aplicacao.Queries.PlanoAEE.VerificarExistenciaPlanoAEEPorTurma;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;

namespace SME.SGP.Aplicacao;

public class VerificarExistenciaPlanoAEEPorTurmaUseCase : AbstractUseCase, IVerificarExistenciaPlanoAEEPorTurmaUseCase
{
    public VerificarExistenciaPlanoAEEPorTurmaUseCase(IMediator mediator) : base(mediator)
    {
    }

    public async Task<bool> Executar(FiltroTurmaPlanoAEEDto param)
    {
        var planoAee = await mediator.Send(new VerificarExistenciaPlanoAEEPorTurmaQuery(param));

        return planoAee.NaoEhNulo() ? throw new NegocioException(MensagemNegocioPlanoAee.CRIANCA_JA_POSSUI_PLANO_AEE_ABERTO_INTEGRACAO) : true;
    }
}