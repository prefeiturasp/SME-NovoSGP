using MediatR;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.Relatorios.HistoricoEscolar;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.Aula
{
    public class ObterListaAlunosFiltroHistoricoEscolarUseCase : AbstractUseCase, IObterListaAlunosFiltroHistoricoEscolarUseCase
    {
        public ObterListaAlunosFiltroHistoricoEscolarUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<PaginacaoResultadoDto<AlunoSimplesDto>> Executar(FiltroBuscaAlunosDto filtros)
        {
            var resultado = new PaginacaoResultadoDto<AlunoSimplesDto>();

            resultado.Items = await mediator.Send(new ObterAlunosPorFiltroQuery(filtros.CodigoUe,
                                                                                filtros.AnoLetivo,
                                                                                filtros.NomeAluno,
                                                                                filtros.CodigoEol));
            resultado.TotalPaginas = 1;
            resultado.TotalRegistros = resultado.Items.Count();

            return resultado;
        }
    }
}
