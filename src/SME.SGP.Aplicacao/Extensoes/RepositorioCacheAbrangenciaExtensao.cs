using Newtonsoft.Json;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public static class RepositorioCacheAbrangenciaExtensao
    {
        /// <summary>
        /// Lê do cache; no miss (ou erro de leitura / JSON corrompido) busca no banco UMA vez e grava no cache.
        /// Não usa IRepositorioCache.ObterAsync(chave, buscarDados), que executa buscarDados de novo quando ele falha.
        /// </summary>
        public static async Task<T> ObterComCacheAsync<T>(this IRepositorioCache repositorioCache, string chave, Func<Task<T>> buscarNoBanco, int minutosParaExpirar)
        {
            try
            {
                var json = await repositorioCache.ObterAsync(chave);
                if (!string.IsNullOrWhiteSpace(json))
                    return JsonConvert.DeserializeObject<T>(json);
            }
            catch
            {
                // Erro de leitura ou JSON corrompido → trata como miss
            }

            var dados = await buscarNoBanco();

            try
            {
                await repositorioCache.SalvarAsync(chave, JsonConvert.SerializeObject(dados), minutosParaExpirar);
            }
            catch
            {
                // Falha ao gravar no cache não pode quebrar a resposta
            }

            return dados;
        }

        /// <summary>
        /// Lista ordenada e separada por vírgula, para compor chave de cache. A ordem de entrada não muda a chave.
        /// Nula vira "null" e vazia vira "": as funções de abrangência recebem NULL e '{}' de forma diferente.
        /// </summary>
        public static string ParaChaveCache(this IEnumerable<int> itens)
            => itens == null ? "null" : string.Join(",", itens.OrderBy(i => i));
    }
}
