using SME.SGP.Dto;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso.EscolaAqui
{
    public interface ISolicitarAlteracaoComunicadoEscolaAquiUseCase
    {
        Task<string> Executar(long id, ComunicadoAlterarDto comunicado);
    }
}
