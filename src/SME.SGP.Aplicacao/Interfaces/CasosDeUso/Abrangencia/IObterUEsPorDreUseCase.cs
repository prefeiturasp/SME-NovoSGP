using SME.SGP.Dominio;
using SME.SGP.Dto;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IObterUEsPorDreUseCase
    {
        Task<IEnumerable<AbrangenciaUeRetorno>> Executar(UEsPorDreDto dto);
    }
}
