using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasFrequencia
    {
        Task<FrequenciaDto> ObterListaFrequenciaPorAula(long aulaId);
        Task<bool> FrequenciaAulaRegistrada(long aulaId);

        FrequenciaAluno ObterPorAlunoDisciplinaData(string codigoAluno, string disciplinaId, DateTime dataAtual);
    }
}