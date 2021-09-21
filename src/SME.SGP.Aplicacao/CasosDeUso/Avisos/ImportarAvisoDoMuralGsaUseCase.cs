using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ImportarAvisoDoMuralGsaUseCase : AbstractUseCase, IImportarAvisoDoMuralGsaUseCase
    {
        public ImportarAvisoDoMuralGsaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var avisoDto = mensagem.ObterObjetoMensagem<AvisoMuralGsaDto>();

            await mediator.Send(new ImportarAvisoDoMuralGsaCommand(avisoDto));

            return true;
        }
    }
}
