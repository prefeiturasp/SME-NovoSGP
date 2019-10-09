using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasNotificacao
    {
        IEnumerable<NotificacaoBasicaDto> Listar(NotificacaoFiltroDto filtroNotificacaoDto);

        IEnumerable<NotificacaoBasicaDto> ListarPorAnoLetivoRf(int anoLetivo, string usuarioRf, int limite = 5);

        NotificacaoBasicaListaDto ObterNotificacaoBasicaLista(int anoLetivo, string usuarioRf);

        int QuantidadeNotificacoesNaoLidas(int anoLetivo, string usuarioRf);

        NotificacaoDetalheDto Obter(long notificacaoId);

        IEnumerable<EnumeradoRetornoDto> ObterCategorias();

        IEnumerable<EnumeradoRetornoDto> ObterStatus();

        IEnumerable<EnumeradoRetornoDto> ObterTipos();
    }
}