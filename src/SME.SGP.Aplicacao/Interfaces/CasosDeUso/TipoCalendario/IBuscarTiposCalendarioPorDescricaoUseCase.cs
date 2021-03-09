using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IBuscarTiposCalendarioPorDescricaoUseCase
    {
        Task<IEnumerable<TipoCalendarioBuscaDto>> Executar(string descricao);
    }
}
