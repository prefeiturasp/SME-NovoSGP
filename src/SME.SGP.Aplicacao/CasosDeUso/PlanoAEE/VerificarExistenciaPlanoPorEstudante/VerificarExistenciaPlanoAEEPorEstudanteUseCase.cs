using MediatR;
using SME.SGP.Dominio;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class VerificarExistenciaPlanoAEEPorEstudanteUseCase : AbstractUseCase, IVerificarExistenciaPlanoAEEPorEstudanteUseCase
    {
        public VerificarExistenciaPlanoAEEPorEstudanteUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(string codigoEstudante)
        {
            var planoAEE = await mediator.Send(new VerificarExistenciaPlanoAEEPorEstudanteQuery(codigoEstudante));

            if (planoAEE != null)
                throw new NegocioException("Estudante/Criança já possui plano AEE em aberto");

            return true;
        }
    }
}
