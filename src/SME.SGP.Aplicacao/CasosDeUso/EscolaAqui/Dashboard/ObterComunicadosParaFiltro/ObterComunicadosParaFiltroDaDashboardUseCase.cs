using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Queries.EscolaAqui.ObterComunicadosParaFiltro;
using SME.SGP.Infra.Dtos.EscolaAqui.Dashboard.ComunicadosFiltro;
using SME.SGP.Infra.Dtos.EscolaAqui.Dashboard.ComunicadosPesquisa;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComunicadosParaFiltroDaDashboardUseCase : IObterComunicadosParaFiltroDaDashboardUseCase
    {
        private readonly IMediator mediator;

        public ObterComunicadosParaFiltroDaDashboardUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<IEnumerable<ComunicadoParaFiltroDaDashboardDto>> Executar(ObterComunicadosParaFiltroDaDashboardDto obterComunicadosFiltroDto)
        {
            var query = new ObterComunicadosParaFiltroDaDashboardQuery(
                obterComunicadosFiltroDto.AnoLetivo,
                obterComunicadosFiltroDto.CodigoDre,
                obterComunicadosFiltroDto.CodigoUe,
                obterComunicadosFiltroDto.GruposIds,
                obterComunicadosFiltroDto.Modalidade,
                obterComunicadosFiltroDto.Semestre,
                obterComunicadosFiltroDto.AnoEscolar,
                obterComunicadosFiltroDto.CodigoTurma,
                obterComunicadosFiltroDto.DataEnvioInicial,
                obterComunicadosFiltroDto.DataEnvioFinal,
                obterComunicadosFiltroDto.Descricao);
            return await mediator.Send(query);
        }
    }
}
