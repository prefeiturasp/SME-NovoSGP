using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Dtos.EscolaAqui.ComunicadosFiltro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.EscolaAqui.ObterComunicadosParaFiltro
{
    public class ObterComunicadosParaFiltroDaDashboardQueryHandler : IRequestHandler<ObterComunicadosParaFiltroDaDashboardQuery, IEnumerable<ComunicadoParaFiltroDaDashboardDto>>
    {
        private readonly IRepositorioComunicado repositorioComunicado;

        public ObterComunicadosParaFiltroDaDashboardQueryHandler(IRepositorioComunicado repositorioComunicado)
        {
            this.repositorioComunicado = repositorioComunicado;
        }

        public async Task<IEnumerable<ComunicadoParaFiltroDaDashboardDto>> Handle(ObterComunicadosParaFiltroDaDashboardQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var comunicadosComTurmas = new List<ComunicadoParaFiltroDaDashboardDto>();

                var filtro = new FiltroObterComunicadosParaFiltroDaDashboardDto
                {
                    AnoEscolar = request.AnoEscolar,
                    AnoLetivo = request.AnoLetivo,
                    CodigoDre = request.CodigoDre,
                    CodigoTurma = request.CodigoTurma,
                    CodigoUe = request.CodigoUe,
                    DataEnvioFinal = request.DataEnvioFinal,
                    DataEnvioInicial = request.DataEnvioInicial,
                    Titulo = request.Descricao,
                    GruposIds = request.GruposIds,
                    Modalidade = request.Modalidade,
                    Semestre = request.Semestre
                };

                var comunicadosFiltrados = await repositorioComunicado.ObterComunicadosParaFiltroDaDashboard(filtro);

                comunicadosComTurmas = await ObterTurmasAssociadas(comunicadosFiltrados);

                return comunicadosComTurmas;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private async Task<List<ComunicadoParaFiltroDaDashboardDto>> ObterTurmasAssociadas(IEnumerable<ComunicadoParaFiltroDaDashboardDto> comunicadosFiltrados)
        {
            foreach (var item in comunicadosFiltrados)
            {
                var turmas = await repositorioComunicado.ObterComunicadosTurma(item.Id);
                item.TurmasCodigo.AddRange(turmas);
            }
            return comunicadosFiltrados.ToList();
        }
    }
}