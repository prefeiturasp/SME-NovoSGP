using SME.SGP.Dto;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso.EscolaAqui
{
    public interface ISolicitarInclusaoComunicadoEscolaAquiUseCase
    {
        Task<string> Executar(ComunicadoInserirDto comunicado);
    }
}
