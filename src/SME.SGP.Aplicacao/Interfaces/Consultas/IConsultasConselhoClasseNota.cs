using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasConselhoClasseNota
    {
        Task<IEnumerable<NotaConceitoBimestreComponenteDto>> ObterNotasAlunoAsync(long conselhoClasseId, string alunoCodigo);
    }
}
