using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.Infra.Dtos.Frequencia;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioConsolidacaoFrequenciaTurmaConsulta
    {
        Task<IEnumerable<GraficoAusenciasComJustificativaDto>> ObterAusenciasComJustificativaASync(int anoLetivo, long dreId, long ueId, Modalidade? modalidade, int semestre);
        Task<IEnumerable<FrequenciaGlobalPorAnoDto>> ObterFrequenciaGlobalPorAnoAsync(int anoLetivo, long dreId, long ueId, Modalidade? modalidade, int semestre);
        Task<IEnumerable<FrequenciaGlobalPorDreDto>> ObterFrequenciaGlobalPorDreAsync(int anoLetivo, Modalidade modalidade, string ano, int? semestre);
        Task<bool> ExisteConsolidacaoFrequenciaTurmaPorAno(int ano);
        Task<IEnumerable<FrequenciaGlobalMensalSemanalDto>> ObterFrequenciasConsolidadasPorTurmaMensalSemestral(int anoLetivo, long dreId, long ueId, int modalidade, string anoTurma, DateTime dataInicioSemana, DateTime datafimSemana, int tipoConsolidadoFrequencia, int semestre, bool visaoDre = false);
        Task<IEnumerable<QuantitativoAlunosFrequenciaBaixaPorTurmaDto>> ObterQuantitativoAlunosFrequenciaBaixaPorTurma(int anoLetivo, TipoTurma tipoTurma);
    }
}
