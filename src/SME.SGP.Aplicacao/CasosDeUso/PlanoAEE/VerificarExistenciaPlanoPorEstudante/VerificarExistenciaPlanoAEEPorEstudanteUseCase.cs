using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class VerificarExistenciaPlanoAEEPorEstudanteUseCase : AbstractUseCase, IVerificarExistenciaPlanoAEEPorEstudanteUseCase
    {
        public VerificarExistenciaPlanoAEEPorEstudanteUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(FiltroEstudantePlanoAEEDto filtro)
        {
            var planoAEE = await mediator.Send(new VerificarExistenciaPlanoAEEPorEstudanteQuery(filtro));

            if (planoAEE.NaoEhNulo())
                throw new NegocioException("Estudante/Criança já possui plano AEE em aberto nessa UE");

            return true;
        }
    }
}
