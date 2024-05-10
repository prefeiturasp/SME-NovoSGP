using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ImportarAtividadesGsaUseCase : AbstractUseCase, IImportarAtividadesGsaUseCase
    {
        public ImportarAtividadesGsaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var avisoDto = mensagem.ObterObjetoMensagem<AtividadeGsaDto>();

            await mediator.Send(new ImportarAtividadeGsaCommand(avisoDto));

            return true;
        }
    }
}
