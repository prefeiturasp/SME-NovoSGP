using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioNotificacaoConsulta : IRepositorioBase<Notificacao>
    {
        Task<PaginacaoResultadoDto<Notificacao>> Obter(string dreId, string ueId, int statusId, string turmaId, string usuarioRf, int tipoId, int categoriaId, string titulo, long codigo, int anoLetivo, Paginacao paginacao);
        Task<IEnumerable<Notificacao>> ObterNotificacoesPorAnoLetivoERf(int anoLetivo, string usuarioRf, int limite);
        int ObterQuantidadeNotificacoesNaoLidasPorAnoLetivoERf(int anoLetivo, string usuarioRf);
        Task<long> ObterUltimoCodigoPorAnoAsync(int ano);
        long ObterUltimoCodigoPorAno(int ano);
        Task<int> ObterQuantidadeNotificacoesNaoLidasPorAnoLetivoEUsuarioAsync(int anoLetivo, string codigoRf);
        Task<long> ObterCodigoPorId(long notificacaoId);
        Task<IEnumerable<NotificacaoUsuarioDto>> ObterUsuariosNotificacoesPorIds(long[] notificacoesIds);
        Task<IEnumerable<NotificacoesParaTratamentoCargosNiveisDto>> ObterNotificacoesParaTratamentoCargosNiveis();
        Task<string> ObterUsuarioNotificacaoPorId(long id);
        Task<Notificacao> ObterPorCodigo(long codigo);
        Task<IEnumerable<Notificacao>> ObterPorWorkFlowAprovacaoId(long requestWorkFlowAprovacaoId);
    }
}