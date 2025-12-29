using System.Collections.Generic;
using System.Threading.Tasks;
using SME.SGP.Infra;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioCadastroAcessoABAEConsulta : IRepositorioBase<CadastroAcessoABAE>
    {
        Task<bool> ExisteCadastroAcessoABAEPorCpf(string cpf, long ueId);
        Task<PaginacaoResultadoDto<DreUeNomeSituacaoTipoEscolaDataABAEDto>> ObterPaginado(FiltroDreIdUeIdNomeSituacaoABAEDto filtro,Paginacao paginacao);
        Task<IEnumerable<NomeCpfABAEDto>> ObterCadastrosABAEPorDre(string cpf, string codigoDre, string codigoUe, string nome);
        Task<CadastroAcessoABAE> ObterCadastroABAEPorCpf(string cpf);
    }
}
