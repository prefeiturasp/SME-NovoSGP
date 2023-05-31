using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;

namespace SME.SGP.TesteIntegracao.PendenciaGeral.ServicosFake
{
    public class PublicarFilaSgpCommandExecutarInserirConsolidadoAtendimentoProfissionalEncaminhamentoNAAPAUseCase : IRequestHandler<PublicarFilaSgpCommand, bool>
    {
        private readonly IExecutarInserirConsolidadoAtendimentoProfissionalEncaminhamentoNAAPAUseCase useCase;

        public PublicarFilaSgpCommandExecutarInserirConsolidadoAtendimentoProfissionalEncaminhamentoNAAPAUseCase(IExecutarInserirConsolidadoAtendimentoProfissionalEncaminhamentoNAAPAUseCase useCase)
        {
            this.useCase = useCase ?? throw new ArgumentNullException(nameof(useCase));
        }

        public async Task<bool> Handle(PublicarFilaSgpCommand request, CancellationToken cancellationToken)
        {
            return await useCase.Executar(new MensagemRabbit(JsonConvert.SerializeObject(request.Filtros)));
        }
    }
}
