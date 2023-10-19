using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso
{
    public interface IObterGruposDeUsuariosUseCase
    {
        Task<IEnumerable<GruposDeUsuariosDto>> Executar();
    }
}
