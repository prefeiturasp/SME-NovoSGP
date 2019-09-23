using System.Collections.Generic;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioNotificacao : IRepositorioBase<Notificacao>
    {
        IEnumerable<Notificacao> Obter(string dreId, string escolaId, int statusId, string turmaId,
            string usuarioId, int tipoId, int categoriaId, string titulo, long codigo, int anoLetivo);

        //(Notificacao, string) ObterDetalhePorId(long idNotificacao);

        long ObterUltimoCodigoPorAno(int ano);
    }
}