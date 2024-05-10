using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterParametrosArredondamentoNotaPorDataAvaliacaoQueryHandler : IRequestHandler<ObterParametrosArredondamentoNotaPorDataAvaliacaoQuery, NotaParametroDto>
    {
        private readonly IRepositorioNotaParametro repositorio;

        public ObterParametrosArredondamentoNotaPorDataAvaliacaoQueryHandler(IRepositorioNotaParametro repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }
        
        public Task<NotaParametroDto> Handle(ObterParametrosArredondamentoNotaPorDataAvaliacaoQuery request, CancellationToken cancellationToken)
        {
            return repositorio.ObterParametrosArredondamentoNotaPorDataAvaliacao(request.DataAvaliacao);
        }
    }
}
