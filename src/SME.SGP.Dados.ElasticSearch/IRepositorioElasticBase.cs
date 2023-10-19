using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nest;

namespace SME.Pedagogico.Interface
{
    public interface IRepositorioElasticBase<T> where T : class
    {
        Task<T> ObterAsync(string indice, string id, string nomeConsulta, object parametro = null);
        Task<IEnumerable<T>> ObterTodosAsync(string indice, string nomeConsulta, object parametro = null);
        Task<IEnumerable<T>> ObterListaAsync(string indice, IEnumerable<string> ids, string nomeConsulta, object parametro = null);
        Task<IEnumerable<T>> ObterListaAsync(string indice, Func<QueryContainerDescriptor<T>, QueryContainer> request, string nomeConsulta, object parametro = null);
        Task<long> ObterTotalDeRegistroAsync(string indice, string nomeConsulta, object parametro = null);
        Task<bool> ExisteAsync(string indice, string id, string nomeConsulta, object parametro = null);
        Task<bool> InserirAsync(T entidade, string indice = "");
    }
}
