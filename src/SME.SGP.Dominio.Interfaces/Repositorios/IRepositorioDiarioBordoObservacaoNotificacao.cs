using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioDiarioBordoObservacaoNotificacao   
    {
        Task<IEnumerable<DiarioBordoObservacaoNotificacao>> ObterPorDiarioBordoObservacaoId(long DiarioBordoObservacaoId);
        Task Excluir(DiarioBordoObservacaoNotificacao notificacao);
        Task Salvar(DiarioBordoObservacaoNotificacao notificacao);
    }
}