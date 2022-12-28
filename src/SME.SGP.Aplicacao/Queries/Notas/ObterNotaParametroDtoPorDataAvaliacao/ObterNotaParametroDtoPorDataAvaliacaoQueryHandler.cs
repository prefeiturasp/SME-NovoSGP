using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Dtos.Notas;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries
{
    public class ObterNotaParametroDtoPorDataAvaliacaoQueryHandler : IRequestHandler<ObterNotaParametroDtoPorDataAvaliacaoQuery, NotaParametroDto>
    {
        private readonly IRepositorioNotaParametro repositorio;

        public ObterNotaParametroDtoPorDataAvaliacaoQueryHandler(IRepositorioNotaParametro repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }
        
        public async Task<NotaParametroDto> Handle(ObterNotaParametroDtoPorDataAvaliacaoQuery request, CancellationToken cancellationToken)
        {
            return await repositorio.ObterDtoPorDataAvaliacao(request.DataAvaliacao);
        }
    }
}
