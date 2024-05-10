using MediatR;
using SME.SGP.Infra.Interface;
using System;

namespace SME.SGP.Aplicacao
{
    public class ObterServicoArmazenamentoUseCase : AbstractUseCase, IObterServicoArmazenamentoUseCase
    {
        private readonly IServicoArmazenamento servicoArmazenamento;

        public ObterServicoArmazenamentoUseCase(IServicoArmazenamento servicoArmazenamento,IMediator mediator) : base(mediator)
        {
            this.servicoArmazenamento = servicoArmazenamento ?? throw new ArgumentNullException(nameof(servicoArmazenamento));
        } 

        public string Executar(string nomeArquivo, bool ehPastaTemporaria)
        {
            return servicoArmazenamento.Obter(nomeArquivo,ehPastaTemporaria);
        }
    }
}
