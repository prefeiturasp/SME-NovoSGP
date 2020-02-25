using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoRecuperacaoParalela
    {
        Task<IEnumerable<KeyValuePair<string, int>>> ObterFrequencias(string[] CodigoAlunos, string CodigoDisciplina, int Ano, PeriodoRecuperacaoParalela Periodo);

        RecuperacaoParalelaStatus ObterStatusRecuperacaoParalela(int RespostasRecuperacaoParalela, int Respostas);

        long ValidarParecerConclusivo(char Parecer);
    }
}