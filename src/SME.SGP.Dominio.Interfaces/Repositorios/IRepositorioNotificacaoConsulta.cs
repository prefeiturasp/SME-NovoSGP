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
        Task<long> ObterUltimoCodigoPorAno(int ano);
        Task<int> ObterQuantidadeNotificacoesNaoLidasPorAnoLetivoEUsuarioAsync(int anoLetivo, string codigoRf);
        Task<long> ObterCodigoPorId(long notificacaoId);
        Task<IEnumerable<NotificacoesParaTratamentoCargosNiveisDto>> ObterNotificacoesParaTratamentoCargosNiveis();
        Task<IEnumerable<NotificacaoBasicaDto>> ObterNotificacoesPorAnoLetivoERfAsync(int anoLetivo, string usuarioRf, int limite = 5);
        Task<Notificacao> ObterPorCodigo(long codigo);  
    }
}