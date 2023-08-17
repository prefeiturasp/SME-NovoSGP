using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirReferenciaArquivoPAPPorArquivoIdCommandHandler : IRequestHandler<ExcluirReferenciaArquivoPAPPorArquivoIdCommand, bool>
    {
        private readonly IRepositorioRelatorioPeriodicoPAPResposta repositorioResposta;

        public ExcluirReferenciaArquivoPAPPorArquivoIdCommandHandler(IRepositorioRelatorioPeriodicoPAPResposta repositorioResposta)
        {
            this.repositorioResposta = repositorioResposta ?? throw new ArgumentNullException(nameof(repositorioResposta));
        }

        public Task<bool> Handle(ExcluirReferenciaArquivoPAPPorArquivoIdCommand request, CancellationToken cancellationToken)
        {
            return this.repositorioResposta.RemoverPorArquivoId(request.ArquivoId);
        }
    }
}
