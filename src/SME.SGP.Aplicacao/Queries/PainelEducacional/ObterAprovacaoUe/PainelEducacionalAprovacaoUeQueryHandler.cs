using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterAprovacaoUe
{
    public class ObterAprovacaoUeQueryHandler : IRequestHandler<PainelEducacionalAprovacaoUeQuery, PainelEducacionalAprovacaoUeResultadoDto>
    {
        private readonly IRepositorioPainelEducacionalAprovacaoUe repositorio;

        public ObterAprovacaoUeQueryHandler(IRepositorioPainelEducacionalAprovacaoUe repositorio)
        {
            this.repositorio = repositorio;
        }

        public async Task<PainelEducacionalAprovacaoUeResultadoDto> Handle(PainelEducacionalAprovacaoUeQuery request, CancellationToken cancellationToken)
        {
            var resultado = await repositorio.ObterAprovacao(
                request.AnoLetivo,
                request.CodigoUe,
                request.NumeroPagina,
                request.NumeroRegistros);

            return new PainelEducacionalAprovacaoUeResultadoDto
            {
                Turmas = resultado.Items.Select(r => new PainelEducacionalAprovacaoUeDto
                {
                    CodigoDre = r.CodigoDre,
                    CodigoUe = r.CodigoUe,
                    Turma = r.Turma,
                    Modalidade = r.Modalidade,
                    TotalPromocoes = r.TotalPromocoes,
                    TotalRetencoesAusencias = r.TotalRetencoesAusencias,
                    TotalRetencoesNotas = r.TotalRetencoesNotas,
                    AnoLetivo = r.AnoLetivo
                }).ToList(),

                TotalPaginas = resultado.TotalPaginas,
                TotalRegistros = resultado.TotalRegistros
            };
        }
    }
}
