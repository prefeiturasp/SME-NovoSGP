using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso.EscolaAqui
{
    public interface ISolicitarExclusaoComunicadosEscolaAquiUseCase
    {
        Task<string> Executar(long[] ids);
    }

}
