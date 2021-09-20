using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ImportarNotaAtividadeAvaliativaGsaUseCase : AbstractUseCase, IImportarNotaAtividadeAvaliativaGsaUseCase
    {
        public ImportarNotaAtividadeAvaliativaGsaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var AtividadeNotaDto = mensagem.ObterObjetoMensagem<NotaAtividadeGsaDto>();

            await mediator.Send(new ImportarNotaAtividadeGsaCommand(AtividadeNotaDto));

            return true;
        }
    }
}
