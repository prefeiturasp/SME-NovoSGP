using System.Threading.Tasks;
using SME.SGP.Infra;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioCadastroAcessoABAEConsulta : IRepositorioBase<CadastroAcessoABAE>
    {
        Task<bool> ExisteCadastroAcessoABAEPorCpf(string cpf);
        Task<PaginacaoResultadoDto<DreUeNomeSituacaoTipoEscolaDataABAEDto>> ObterPaginado(FiltroDreIdUeIdNomeSituacaoABAEDto filtro,Paginacao paginacao);
        Task<CadastroAcessoABAE> ObterCompletoPorId(long id);
    }
}
