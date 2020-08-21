using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioNotificacao : IRepositorioBase<Notificacao>
    {
        Task ExcluirPorIdsAsync(long[] ids);

        Task<PaginacaoResultadoDto<Notificacao>> Obter(string dreId, string ueId, int statusId, string turmaId,
                    string usuarioId, int tipoId, int categoriaId, string titulo, long codigo, int anoLetivo, Paginacao paginaRegistros);

        IEnumerable<Notificacao> ObterNotificacoesPorAnoLetivoERf(int anoLetivo, string usuarioRf, int limite);

        int ObterQuantidadeNotificacoesNaoLidasPorAnoLetivoERf(int anoLetivo, string usuarioRf);

        long ObterUltimoCodigoPorAno(int ano);
        Task<int> ObterQuantidadeNotificacoesNaoLidasPorAnoLetivoEUsuarioAsync(int anoLetivo, string codigoRf);
        Task<IEnumerable<NotificacaoBasicaDto>> ObterNotificacoesPorAnoLetivoERfAsync(int anoLetivo, string usuarioRf, int limite = 5);
    }
}