using SME.SGP.Infra.Dtos;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioHistoricoEscolarObservacao : IRepositorioBase<HistoricoEscolarObservacao>
    {
        Task<HistoricoEscolarObservacaoDto> ObterPorCodigoAlunoAsync(string codigoAluno);
    }
}
