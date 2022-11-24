using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirReferenciaArquivoNAAPAPorArquivoIdCommandHandler : IRequestHandler<ExcluirReferenciaArquivoNAAPAPorArquivoIdCommand, bool>
    {
        private readonly IRepositorioRespostaEncaminhamentoNAAPA repositorioResposta;

        public ExcluirReferenciaArquivoNAAPAPorArquivoIdCommandHandler(IRepositorioRespostaEncaminhamentoNAAPA repositorioResposta)
        {
            this.repositorioResposta = repositorioResposta ?? throw new ArgumentNullException(nameof(repositorioResposta));
        }


        public async Task<bool> Handle(ExcluirReferenciaArquivoNAAPAPorArquivoIdCommand request, CancellationToken cancellationToken)
            => await repositorioResposta.RemoverPorArquivoId(request.ArquivoId);
    }
}
