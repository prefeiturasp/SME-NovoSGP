using MediatR;
using Newtonsoft.Json;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.Frequencia.ServicosFakes
{
    public class PublicarFilaSgpCommandHandlerFakePresencasMaiorTotalAulas : IRequestHandler<PublicarFilaSgpCommand, bool>
    {
        private readonly ICalculoFrequenciaTurmaDisciplinaUseCase calculoFrequenciaTurmaDisciplinaUseCase;

        public PublicarFilaSgpCommandHandlerFakePresencasMaiorTotalAulas(ICalculoFrequenciaTurmaDisciplinaUseCase calculoFrequenciaTurmaDisciplinaUseCase)
        {
            this.calculoFrequenciaTurmaDisciplinaUseCase = calculoFrequenciaTurmaDisciplinaUseCase ?? throw new ArgumentNullException(nameof(calculoFrequenciaTurmaDisciplinaUseCase));
        }

        public async Task<bool> Handle(PublicarFilaSgpCommand request, CancellationToken cancellationToken)
        {
            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(request.Filtros));
            return await calculoFrequenciaTurmaDisciplinaUseCase.Executar(mensagem);
        }
    }
}
