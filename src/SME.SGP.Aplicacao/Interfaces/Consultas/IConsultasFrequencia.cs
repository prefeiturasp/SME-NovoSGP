using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasFrequencia
    {
        Task<FrequenciaDto> ObterListaFrequenciaPorAula(long aulaId);
        Task<MarcadorFrequenciaDto> ObterMarcadorAluno(AlunoPorTurmaResposta aluno, PeriodoEscolarDto bimestre);
    }
}