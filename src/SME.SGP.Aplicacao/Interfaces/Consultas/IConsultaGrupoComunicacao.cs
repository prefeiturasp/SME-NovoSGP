using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultaGrupoComunicacao
    {
        Task<IEnumerable<GrupoComunicacaoDto>> Listar(FiltroGrupoComunicacaoDto filtro);

        Task<GrupoComunicacaoCompletoDto> ObterPorIdAsync(long id);

        Task<IEnumerable<GrupoComunicacaoDto>> Listar(IEnumerable<long> ids);

        Task<IEnumerable<long>> ObterIdsGrupoComunicadoPorModalidade(Modalidade modalidade);
    }
}