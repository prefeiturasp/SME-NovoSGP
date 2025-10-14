using SME.SGP.Infra.Dtos.PainelEducacional.SondagemEscrita;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioSondagemEscritaUe
    {
        Task<IEnumerable<SondagemEscritaUeDto>> ObterSondagemEscritaAsync();
    }
}
