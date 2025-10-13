using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterAbandono
{
    public class ObterAbandonoUeQueryHandler : IRequestHandler<ObterAbandonoUeQuery, PainelEducacionalAbandonoUeDto>
    {
        private readonly IRepositorioPainelEducacionalAbandonoUe repositorio;
        
        public ObterAbandonoUeQueryHandler(IRepositorioPainelEducacionalAbandonoUe repositorio)
        {
            this.repositorio = repositorio;
        }
        
        public async Task<PainelEducacionalAbandonoUeDto> Handle(ObterAbandonoUeQuery request, CancellationToken cancellationToken)
        {
            var (modalidades, totalPaginas, totalRegistros) = await repositorio.ObterAbandonoUe(
                request.AnoLetivo,
                request.CodigoDre,
                request.CodigoUe,
                request.Modalidade,
                request.NumeroPagina,
                request.NumeroRegistros);
            
            return new PainelEducacionalAbandonoUeDto
            {
                Modalidades = new System.Collections.Generic.List<PainelEducacionalAbandonoTurmaDto>(modalidades),
                TotalPaginas = totalPaginas,
                TotalRegistros = totalRegistros
            };
        }
    }
}