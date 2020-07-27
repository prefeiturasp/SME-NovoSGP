using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioGrupoComunicacao : IRepositorioBase<GrupoComunicacao>
    {
        Task<IEnumerable<GrupoComunicacaoCompletoRespostaDto>> Listar(FiltroGrupoComunicacaoDto filtro);

        Task<IEnumerable<GrupoComunicacaoCompletoRespostaDto>> ObterCompletoPorIdAsync(long id);

        Task<IEnumerable<GrupoComunicacaoCompletoRespostaDto>> ObterCompletoPorListaId(IEnumerable<long> ids);

        Task<IEnumerable<long>> ObterIdsGrupoComunicadoPorModalidade(Modalidade modalidade);
    }
}