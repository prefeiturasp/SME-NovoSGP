using Microsoft.AspNetCore.Mvc;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    // [Authorize("Bearer")]
    [Route("api/v1/dashboard/compensacoes/ausencia")]
    public class DashboardCompensacaoAusenciaController : Controller
    {
        [HttpGet("anos/{anoLetivo}/dres/{dreId}/ues/{ueId}/modalidades/{modalidade}/consolidado/anos-turmas")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(GraficoCompensacaoAusenciaDto), 200)]
        public async Task<IActionResult> ObterTotalAusenciasCompensadas(int anoLetivo, long dreId, long ueId, int modalidade, int bimestre, [FromQuery] int semestre)
        {
            var dadosGraficoAno = new GraficoCompensacaoAusenciaDto
            {
                QuantidadeAusenciasRegistrada = 20000,
                PorcentagemAulas = 25,
                DadosCompensacaoAusenciaDashboard = new List<DadosRetornoAusenciasCompensadasDashboardDto>()
                {
                    new DadosRetornoAusenciasCompensadasDashboardDto(){Descricao = "EF-1", Quantidade = 1250},
                    new DadosRetornoAusenciasCompensadasDashboardDto(){Descricao = "EF-2", Quantidade = 1500},
                    new DadosRetornoAusenciasCompensadasDashboardDto(){Descricao = "EF-3", Quantidade = 1600},
                    new DadosRetornoAusenciasCompensadasDashboardDto(){Descricao = "EF-4", Quantidade = 1900},
                    new DadosRetornoAusenciasCompensadasDashboardDto(){Descricao = "EF-5", Quantidade = 1300},
                    new DadosRetornoAusenciasCompensadasDashboardDto(){Descricao = "EF-6", Quantidade = 1700},
                    new DadosRetornoAusenciasCompensadasDashboardDto(){Descricao = "EF-7", Quantidade = 1000},
                    new DadosRetornoAusenciasCompensadasDashboardDto(){Descricao = "EF-8", Quantidade = 1200},
                    new DadosRetornoAusenciasCompensadasDashboardDto(){Descricao = "EF-9", Quantidade = 2000},
                }
            };

            var dadosGraficoTurma = new GraficoCompensacaoAusenciaDto
            {
                QuantidadeAusenciasRegistrada = 20000,
                PorcentagemAulas = 25,
                DadosCompensacaoAusenciaDashboard = new List<DadosRetornoAusenciasCompensadasDashboardDto>()
                {
                    new DadosRetornoAusenciasCompensadasDashboardDto(){Descricao = "EF-1A", Quantidade = 1250},
                    new DadosRetornoAusenciasCompensadasDashboardDto(){Descricao = "EF-1B", Quantidade = 1500},
                    new DadosRetornoAusenciasCompensadasDashboardDto(){Descricao = "EF-1C", Quantidade = 1600},
                    new DadosRetornoAusenciasCompensadasDashboardDto(){Descricao = "EF-2A", Quantidade = 1900},
                    new DadosRetornoAusenciasCompensadasDashboardDto(){Descricao = "EF-2B", Quantidade = 900},
                    new DadosRetornoAusenciasCompensadasDashboardDto(){Descricao = "EF-3A", Quantidade = 1700},
                    new DadosRetornoAusenciasCompensadasDashboardDto(){Descricao = "EF-3B", Quantidade = 1000},
                    new DadosRetornoAusenciasCompensadasDashboardDto(){Descricao = "EF-3C", Quantidade = 1100},
                    new DadosRetornoAusenciasCompensadasDashboardDto(){Descricao = "EF-4A", Quantidade = 1900},
                    new DadosRetornoAusenciasCompensadasDashboardDto(){Descricao = "EF-4B", Quantidade = 1100},
                    new DadosRetornoAusenciasCompensadasDashboardDto(){Descricao = "EF-4C", Quantidade = 1500},
                    new DadosRetornoAusenciasCompensadasDashboardDto(){Descricao = "EF-5A", Quantidade = 1600},
                    new DadosRetornoAusenciasCompensadasDashboardDto(){Descricao = "EF-5B", Quantidade = 1900},
                    new DadosRetornoAusenciasCompensadasDashboardDto(){Descricao = "EF-6A", Quantidade = 1500},
                    new DadosRetornoAusenciasCompensadasDashboardDto(){Descricao = "EF-6B", Quantidade = 1200},
                    new DadosRetornoAusenciasCompensadasDashboardDto(){Descricao = "EF-6C", Quantidade = 1457},
                    new DadosRetornoAusenciasCompensadasDashboardDto(){Descricao = "EF-7A", Quantidade = 1000},
                    new DadosRetornoAusenciasCompensadasDashboardDto(){Descricao = "EF-7B", Quantidade = 1900},
                    new DadosRetornoAusenciasCompensadasDashboardDto(){Descricao = "EF-7C", Quantidade = 1600},
                    new DadosRetornoAusenciasCompensadasDashboardDto(){Descricao = "EF-8A", Quantidade = 1500},
                    new DadosRetornoAusenciasCompensadasDashboardDto(){Descricao = "EF-8B", Quantidade = 1800},
                    new DadosRetornoAusenciasCompensadasDashboardDto(){Descricao = "EF-8C", Quantidade = 1500},
                    new DadosRetornoAusenciasCompensadasDashboardDto(){Descricao = "EF-9A", Quantidade = 2000},
                    new DadosRetornoAusenciasCompensadasDashboardDto(){Descricao = "EF-9B", Quantidade = 1200},
                    new DadosRetornoAusenciasCompensadasDashboardDto(){Descricao = "EF-9C", Quantidade = 1600},
                }
            };

            if (ueId == -99)
                return Ok(dadosGraficoAno);

            return Ok(dadosGraficoTurma);
        }

        [HttpGet("anos/{anoLetivo}/dres/{dreId}/ues/{ueId}/modalidades/{modalidade}/consolidado/compensacoes-consideradas")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [ProducesResponseType(typeof(GraficoCompensacaoAusenciaDto), 200)]
        public async Task<IActionResult> ObterTotalCompensacoesConsideradas(int anoLetivo, long dreId, long ueId, int modalidade, int bimestre, [FromQuery] int semestre)
        {
            var dadosGraficoAno = new GraficoCompensacaoAusenciaDto
            {
                QuantidadeAusenciasRegistrada = 20000,
                PorcentagemAulas = 25,
                DadosCompensacaoAusenciaDashboard = new List<DadosRetornoAusenciasCompensadasDashboardDto>()
                {
                    new DadosRetornoAusenciasCompensadasDashboardDto(){Descricao = "EF-1", Quantidade = 1250},
                    new DadosRetornoAusenciasCompensadasDashboardDto(){Descricao = "EF-2", Quantidade = 1500},
                    new DadosRetornoAusenciasCompensadasDashboardDto(){Descricao = "EF-3", Quantidade = 1600},
                    new DadosRetornoAusenciasCompensadasDashboardDto(){Descricao = "EF-4", Quantidade = 1900},
                    new DadosRetornoAusenciasCompensadasDashboardDto(){Descricao = "EF-5", Quantidade = 1300},
                    new DadosRetornoAusenciasCompensadasDashboardDto(){Descricao = "EF-6", Quantidade = 1700},
                    new DadosRetornoAusenciasCompensadasDashboardDto(){Descricao = "EF-7", Quantidade = 1000},
                    new DadosRetornoAusenciasCompensadasDashboardDto(){Descricao = "EF-8", Quantidade = 1200},
                    new DadosRetornoAusenciasCompensadasDashboardDto(){Descricao = "EF-9", Quantidade = 2000},
                }
            };

            var dadosGraficoTurma = new GraficoCompensacaoAusenciaDto
            {
                QuantidadeAusenciasRegistrada = 20000,
                PorcentagemAulas = 25,
                DadosCompensacaoAusenciaDashboard = new List<DadosRetornoAusenciasCompensadasDashboardDto>()
                {
                    new DadosRetornoAusenciasCompensadasDashboardDto(){Descricao = "EF-1A", Quantidade = 1250},
                    new DadosRetornoAusenciasCompensadasDashboardDto(){Descricao = "EF-1B", Quantidade = 1500},
                    new DadosRetornoAusenciasCompensadasDashboardDto(){Descricao = "EF-1C", Quantidade = 1600},
                    new DadosRetornoAusenciasCompensadasDashboardDto(){Descricao = "EF-2A", Quantidade = 1900},
                    new DadosRetornoAusenciasCompensadasDashboardDto(){Descricao = "EF-2B", Quantidade = 900},
                    new DadosRetornoAusenciasCompensadasDashboardDto(){Descricao = "EF-3A", Quantidade = 1700},
                    new DadosRetornoAusenciasCompensadasDashboardDto(){Descricao = "EF-3B", Quantidade = 1000},
                    new DadosRetornoAusenciasCompensadasDashboardDto(){Descricao = "EF-3C", Quantidade = 1100},
                    new DadosRetornoAusenciasCompensadasDashboardDto(){Descricao = "EF-4A", Quantidade = 1900},
                    new DadosRetornoAusenciasCompensadasDashboardDto(){Descricao = "EF-4B", Quantidade = 1100},
                    new DadosRetornoAusenciasCompensadasDashboardDto(){Descricao = "EF-4C", Quantidade = 1500},
                    new DadosRetornoAusenciasCompensadasDashboardDto(){Descricao = "EF-5A", Quantidade = 1600},
                    new DadosRetornoAusenciasCompensadasDashboardDto(){Descricao = "EF-5B", Quantidade = 1900},
                    new DadosRetornoAusenciasCompensadasDashboardDto(){Descricao = "EF-6A", Quantidade = 1500},
                    new DadosRetornoAusenciasCompensadasDashboardDto(){Descricao = "EF-6B", Quantidade = 1200},
                    new DadosRetornoAusenciasCompensadasDashboardDto(){Descricao = "EF-6C", Quantidade = 1457},
                    new DadosRetornoAusenciasCompensadasDashboardDto(){Descricao = "EF-7A", Quantidade = 1000},
                    new DadosRetornoAusenciasCompensadasDashboardDto(){Descricao = "EF-7B", Quantidade = 1900},
                    new DadosRetornoAusenciasCompensadasDashboardDto(){Descricao = "EF-7C", Quantidade = 1600},
                    new DadosRetornoAusenciasCompensadasDashboardDto(){Descricao = "EF-8A", Quantidade = 1500},
                    new DadosRetornoAusenciasCompensadasDashboardDto(){Descricao = "EF-8B", Quantidade = 1800},
                    new DadosRetornoAusenciasCompensadasDashboardDto(){Descricao = "EF-8C", Quantidade = 1500},
                    new DadosRetornoAusenciasCompensadasDashboardDto(){Descricao = "EF-9A", Quantidade = 2000},
                    new DadosRetornoAusenciasCompensadasDashboardDto(){Descricao = "EF-9B", Quantidade = 1200},
                    new DadosRetornoAusenciasCompensadasDashboardDto(){Descricao = "EF-9C", Quantidade = 1600},
                }
            };

            if (ueId == -99)
                return Ok(dadosGraficoAno);

            return Ok(dadosGraficoTurma);
        }
    }
}
