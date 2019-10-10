using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasNotificacao
    {
        Task<PaginacaoResultadoDto<NotificacaoBasicaDto>> Listar(NotificacaoFiltroDto filtroNotificacaoDto);

        IEnumerable<NotificacaoBasicaDto> ListarPorAnoLetivoRf(int anoLetivo, string usuarioRf, int limite = 5);

        NotificacaoDetalheDto Obter(long notificacaoId);

        IEnumerable<EnumeradoRetornoDto> ObterCategorias();

        NotificacaoBasicaListaDto ObterNotificacaoBasicaLista(int anoLetivo, string usuarioRf);

        IEnumerable<EnumeradoRetornoDto> ObterStatus();

        IEnumerable<EnumeradoRetornoDto> ObterTipos();

        int QuantidadeNotificacoesNaoLidas(int anoLetivo, string usuarioRf);
    }
}