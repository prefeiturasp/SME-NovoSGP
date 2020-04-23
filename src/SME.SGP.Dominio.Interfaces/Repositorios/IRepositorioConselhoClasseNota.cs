using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioConselhoClasseNota : IRepositorioBase<ConselhoClasseNota>
    {
        Task<IEnumerable<NotaConceitoBimestreComponenteDto>> ObterNotasAluno(long conselhoClasseId, string alunoCodigo);
    }
}
