using SME.SGP.Dto;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasNotificacao
    {
        IEnumerable<NotificacaoBasicaDto> Listar(NotificacaoFiltroDto filtroNotificacaoDto);

        IEnumerable<NotificacaoBasicaDto> Listar(int anoLetivo, string usuarioRf, int limite = 5);

        NotificacaoBasicaListaDto ListarNotificacaoBasica(int anoLetivo, string usuarioRf);

        int QuantidadeNotificacoes(int anoLetivo, string usuarioRf);

        NotificacaoDetalheDto Obter(long notificacaoId);

        IEnumerable<EnumeradoRetornoDto> ObterCategorias();

        IEnumerable<EnumeradoRetornoDto> ObterStatus();

        IEnumerable<EnumeradoRetornoDto> ObterTipos();
    }
}