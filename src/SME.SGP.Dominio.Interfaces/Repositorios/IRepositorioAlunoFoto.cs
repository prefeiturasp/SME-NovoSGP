using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioAlunoFoto : IRepositorioBase<AlunoFoto>
    {
        Task<MiniaturaFotoDto> ObterFotosPorAlunoCodigo(string alunoCodigo);
        Task<AlunoFoto> ObterFotoPorAlunoCodigo(string codigoAluno);
    }
}
