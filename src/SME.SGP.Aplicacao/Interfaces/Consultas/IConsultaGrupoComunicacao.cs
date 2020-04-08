using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultaGrupoComunicacao
    {
        Task<AtividadeAvaliativaCompletaDto> Listar(FiltroGrupoComunicacaoDto filtro);

        Task<AtividadeAvaliativaCompletaDto> ObterPorIdAsync(long id);
    }
}