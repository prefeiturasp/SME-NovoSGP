using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioEncaminhamentoNAAPASecao : IRepositorioBase<EncaminhamentoNAAPASecao>
    {
        Task<IEnumerable<long>> ObterIdsSecoesPorEncaminhamentoNAAPAId(long encaminhamentoNAAPAId);
        Task<AuditoriaDto> ObterAuditoriaEncaminhamentoNaapaSecao(long id);
        Task<bool> ExisteSecoesDeItineracia(long encaminhamentoNAAPAId);
    }
}
