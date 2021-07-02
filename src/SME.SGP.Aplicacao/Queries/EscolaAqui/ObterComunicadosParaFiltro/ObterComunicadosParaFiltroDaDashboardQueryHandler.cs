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
            this.repositorioComunicado = repositorioComunicado ?? throw new ArgumentNullException(nameof(repositorioComunicado));
        }

        public async Task<IEnumerable<ComunicadoParaFiltroDaDashboardDto>> Handle(ObterComunicadosParaFiltroDaDashboardQuery request, CancellationToken cancellationToken)
        {
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
                Modalidades = request.Modalidades,
                Semestre = request.Semestre
            };

            var comunicadosFiltrados = await repositorioComunicado.ObterComunicadosParaFiltroDaDashboard(filtro);

            return await ObterTurmasAssociadas(comunicadosFiltrados);            
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