using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioNovoEncaminhamentoNAAPASecao : IRepositorioBase<EncaminhamentoNAAPASecao>
    {
        Task<IEnumerable<long>> ObterIdsSecoesPorNovoEncaminhamentoNAAPAId(long encaminhamentoNAAPAId);
        Task<AuditoriaDto> ObterAuditoriaNovoEncaminhamentoNaapaSecao(long id);
        Task<bool> ExisteSecoesDeItineracia(long encaminhamentoNAAPAId);
    }
}