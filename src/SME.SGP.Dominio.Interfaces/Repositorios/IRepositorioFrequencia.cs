using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioFrequencia : IRepositorioBase<RegistroFrequencia>
    {
        Task ExcluirFrequenciaAula(long aulaId);

        RegistroFrequenciaAulaDto ObterAulaDaFrequencia(long registroFrequenciaId);

        IEnumerable<AulasPorTurmaDisciplinaDto> ObterAulasSemRegistroFrequencia(string turmaId, string disciplinaId);

        Task<IEnumerable<AusenciaAlunoDto>> ObterAusencias(string turmaCodigo, string disciplinaCodigo, DateTime[] datas, string[] alunoCodigos);

        IEnumerable<RegistroAusenciaAluno> ObterListaFrequenciaPorAula(long aulaId);

        RegistroFrequencia ObterRegistroFrequenciaPorAulaId(long aulaId);
    }
}