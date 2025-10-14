using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Linq;
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
            var resultado = await repositorio.ObterAbandonoUe(request.AnoLetivo, request.CodigoDre, request.CodigoUe, request.Modalidade, request.NumeroPagina, request.NumeroRegistros);
            return new PainelEducacionalAbandonoUeDto
            {
                Modalidades = resultado.Items.Select(r => new PainelEducacionalAbandonoTurmaDto
                {
                    Turma = r.NomeTurma,
                    QuantidadeDesistentes = r.QuantidadeDesistencias
                }).ToList(),

                TotalPaginas = resultado.TotalPaginas,
                TotalRegistros = resultado.TotalRegistros
            };
        }
    }
}