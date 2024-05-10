using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ReplicarParametrosAnoAnteriorUseCase : AbstractUseCase,IReplicarParametrosAnoAnteriorUseCase
    {
        public ReplicarParametrosAnoAnteriorUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var filtro = mensagem.ObterObjetoMensagem<FiltroInclusaoParametrosAnoAnterior>();
            
            var replicar = await mediator.Send(new ReplicarParametrosAnoAnteriorCommand(filtro.AnoLetivo,filtro.ModalidadeTipoCalendario));

            if (replicar == false)
                throw new NegocioException($"Não foi possível replicar para o ano {filtro.AnoLetivo} e a modalidade {filtro.ModalidadeTipoCalendario.Name()}");
            
            return true;
        }
    }
}