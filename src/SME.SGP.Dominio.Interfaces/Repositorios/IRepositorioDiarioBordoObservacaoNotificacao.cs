using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioDiarioBordoObservacaoNotificacao   
    {
        Task<IEnumerable<DiarioBordoObservacaoNotificacao>> ObterPorDiarioBordoObservacaoId(long diarioBordoObservacaoId);
        Task<IEnumerable<long>> ObterObservacaoPorId(long diarioBordoId);
        Task Excluir(DiarioBordoObservacaoNotificacao notificacao);
        Task Salvar(DiarioBordoObservacaoNotificacao notificacao);
    }
}