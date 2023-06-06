using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;

namespace SME.SGP.TesteIntegracao.PendenciaGeral.ServicosFake
{
    public class PublicarFilaSgpCommandExecutarConsolidadoAtendimentoProfissionalEncaminhamentoNAAPAUseCase : IRequestHandler<PublicarFilaSgpCommand, bool>
    {
        private readonly IExecutarInserirConsolidadoAtendimentoProfissionalEncaminhamentoNAAPAUseCase executarInserirConsolidadoAtendimentoProfissionalEncaminhamentoNAAPAUseCase;
        private readonly IExecutarExcluirConsolidadoAtendimentoProfissionalEncaminhamentoNAAPAUseCase executarExcluirConsolidadoAtendimentoProfissionalEncaminhamentoNAAPAUseCase;

        public PublicarFilaSgpCommandExecutarConsolidadoAtendimentoProfissionalEncaminhamentoNAAPAUseCase(IExecutarInserirConsolidadoAtendimentoProfissionalEncaminhamentoNAAPAUseCase executarInserirConsolidadoAtendimentoProfissionalEncaminhamentoNAAPAUseCase, IExecutarExcluirConsolidadoAtendimentoProfissionalEncaminhamentoNAAPAUseCase executarExcluirConsolidadoAtendimentoProfissionalEncaminhamentoNAAPAUseCase)
        {
            this.executarInserirConsolidadoAtendimentoProfissionalEncaminhamentoNAAPAUseCase = executarInserirConsolidadoAtendimentoProfissionalEncaminhamentoNAAPAUseCase ?? throw new ArgumentNullException(nameof(executarInserirConsolidadoAtendimentoProfissionalEncaminhamentoNAAPAUseCase));
            this.executarExcluirConsolidadoAtendimentoProfissionalEncaminhamentoNAAPAUseCase = executarExcluirConsolidadoAtendimentoProfissionalEncaminhamentoNAAPAUseCase ?? throw new ArgumentNullException(nameof(executarExcluirConsolidadoAtendimentoProfissionalEncaminhamentoNAAPAUseCase));
        }

        public Task<bool> Handle(PublicarFilaSgpCommand request, CancellationToken cancellationToken)
        {
            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(request.Filtros));

            return request.Rota switch
            {
                RotasRabbitSgpNAAPA.ExecutarInserirConsolidadoAtendimentoProfissionalEncaminhamentoNAAPA => executarInserirConsolidadoAtendimentoProfissionalEncaminhamentoNAAPAUseCase.Executar(mensagem),
                RotasRabbitSgpNAAPA.ExecutarExcluirConsolidadoAtendimentoProfissionalEncaminhamentoNAAPA => executarExcluirConsolidadoAtendimentoProfissionalEncaminhamentoNAAPAUseCase.Executar(mensagem),
                _ => throw new NotImplementedException($"Rota: {request.Rota} não implementada para o teste"),
            };
        }
    }
}
