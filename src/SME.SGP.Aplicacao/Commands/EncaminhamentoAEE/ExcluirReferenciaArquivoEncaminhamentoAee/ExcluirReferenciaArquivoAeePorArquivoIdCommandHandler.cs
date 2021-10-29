using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirReferenciaArquivoAeePorArquivoIdCommandHandler : IRequestHandler<ExcluirReferenciaArquivoAeePorArquivoIdCommand, bool>
    {
        private readonly IRepositorioRespostaEncaminhamentoAEE repositorioResposta;

        public ExcluirReferenciaArquivoAeePorArquivoIdCommandHandler(IRepositorioRespostaEncaminhamentoAEE repositorioResposta)
        {
            this.repositorioResposta = repositorioResposta ?? throw new ArgumentNullException(nameof(repositorioResposta));
        }


        public async Task<bool> Handle(ExcluirReferenciaArquivoAeePorArquivoIdCommand request, CancellationToken cancellationToken)
            => await repositorioResposta.RemoverPorArquivoId(request.ArquivoId);
    }
}
