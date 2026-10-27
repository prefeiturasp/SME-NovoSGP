using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SME.Pedagogico.Interface;
using SME.SGP.Infra;
using SME.SGP.Infra.ElasticSearch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Dados.ElasticSearch
{
    public abstract class RepositorioElasticBase<TEntidade> : IRepositorioElasticBase<TEntidade> where TEntidade : class
    {
        private const int QUANTIDADE_RETORNO = 200;
        private readonly ElasticsearchClient _elasticClient;
        private readonly IServicoTelemetria servicoTelemetria;
        private readonly string indicePadraoRepositorio;
        private readonly ElasticOptions elasticOptions;

        protected RepositorioElasticBase(ElasticsearchClient elasticClient,
                                         IServicoTelemetria servicoTelemetria,
                                         IOptions<ElasticOptions> elasticOptions,
                                         string indicePadraoRepositorio = "")
        {
            _elasticClient = elasticClient;
            this.servicoTelemetria = servicoTelemetria;
            this.indicePadraoRepositorio = indicePadraoRepositorio;
            this.elasticOptions = elasticOptions.Value ?? throw new ArgumentNullException(nameof(elasticOptions));
        }

        public async Task<bool> ExisteAsync(string indice, string id, string nomeConsulta, object parametro = null)
        {
            var response = await servicoTelemetria.RegistrarComRetornoAsync<ExistsResponse>(async () =>
                    await _elasticClient.ExistsAsync(new ExistsRequest(indice, id)),
                "Elastic",
                nomeConsulta,
                indice,
                parametro?.ToString());

            if (!response.ApiCallDetails.HasSuccessfulStatusCode)
                throw new InvalidOperationException(response.ElasticsearchServerError?.ToString(),
                    response.ApiCallDetails.OriginalException);

            return response.Exists;
        }

        public async Task<TEntidade> ObterAsync(string indice, string id, string nomeConsulta, object parametro = null)
        {
            var response = await servicoTelemetria.RegistrarComRetornoAsync<GetResponse<TEntidade>>(async () =>
                    await _elasticClient.GetAsync<TEntidade>(new GetRequest(indice, id)),
                "Elastic",
                nomeConsulta,
                indice,
                parametro?.ToString());

            if (response.ApiCallDetails.HasSuccessfulStatusCode && response.Found)
                return response.Source;

            return null;
        }

        public async Task<IEnumerable<TEntidade>> ObterListaAsync(
            string indice,
            IEnumerable<string> ids,
            string nomeConsulta,
            object parametro = null)
                {
                    var listaIds = ids.ToList();

                    var tasks = listaIds.Select(id =>
                        _elasticClient.GetAsync<TEntidade>(new GetRequest(indice, id)));

                    var responses = await Task.WhenAll(tasks);

                    return responses
                        .Where(r => r.ApiCallDetails.HasSuccessfulStatusCode && r.Found && r.Source is not null)
                        .Select(r => r.Source)
                        .ToList();
        }

        public async Task<IEnumerable<TEntidade>> ObterListaAsync(string indice, Action<QueryDescriptor<TEntidade>> request, string nomeConsulta, object parametro = null)
        {
            var listaDeRetorno = new List<TEntidade>();

            var response = await servicoTelemetria.RegistrarComRetornoAsync<SearchResponse<TEntidade>>(async () =>
                    await _elasticClient.SearchAsync<TEntidade>(s => s
                        .Index(indice)
                        .Query(request)
                        .Scroll(new Duration("10s"))
                        .Size(QUANTIDADE_RETORNO)),
                "Elastic",
                nomeConsulta,
                indice,
                parametro?.ToString());

            if (!response.ApiCallDetails.HasSuccessfulStatusCode)
                throw new InvalidOperationException(response.ElasticsearchServerError?.ToString(),
                    response.ApiCallDetails.OriginalException);

            listaDeRetorno.AddRange(response.Documents);

            while (response.Documents.Any() && response.Documents.Count == QUANTIDADE_RETORNO)
            {
                var scrollId = response.ScrollId;

                response = await servicoTelemetria.RegistrarComRetornoAsync<SearchResponse<TEntidade>>(async () =>
                        await _elasticClient.ScrollAsync<TEntidade>(s => s
                            .ScrollId(scrollId)
                            .Scroll(new Duration("10s"))),
                    "Elastic",
                    nomeConsulta + " scroll",
                    indice,
                    parametro?.ToString());

                listaDeRetorno.AddRange(response.Documents);
            }

            var lastScrollId = response.ScrollId;
            await _elasticClient.ClearScrollAsync(r => r.ScrollId(lastScrollId));

            return listaDeRetorno;
        }

        public async Task<IEnumerable<TEntidade>> ObterTodosAsync(
            string indice,
            string nomeConsulta,
            object parametro = null)
        {
            SearchResponse<TEntidade> response =
                await servicoTelemetria.RegistrarComRetornoAsync<SearchResponse<TEntidade>>(
                    async () => await _elasticClient.SearchAsync<TEntidade>(s => s
                        .Index(indice)
                        .Query(q => q.MatchAll(new MatchAllQuery()))),
                    "Elastic",
                    nomeConsulta,
                    indice,
                    parametro?.ToString());

            if (!response.ApiCallDetails.HasSuccessfulStatusCode)
                throw new InvalidOperationException(
                    response.ElasticsearchServerError?.ToString(),
                    response.ApiCallDetails.OriginalException);

            return response.Hits
                .Select(hit => hit.Source)
                .Where(source => source is not null)
                .ToList();
        }

        public async Task<long> ObterTotalDeRegistroAsync(string indice, string nomeConsulta, object parametro = null)
        {
            var response = await servicoTelemetria.RegistrarComRetornoAsync<SearchResponse<TEntidade>>(async () =>
                    await _elasticClient.SearchAsync<TEntidade>(s => s
                        .Index(indice)
                        .Query(q => q.MatchAll(new MatchAllQuery()))),
                "Elastic",
                nomeConsulta,
                indice,
                parametro?.ToString());

            if (!response.ApiCallDetails.HasSuccessfulStatusCode)
                throw new Exception(response.ElasticsearchServerError?.ToString(),
                    response.ApiCallDetails.OriginalException);

            return response.Total.GetValueOrDefault();
        }

        public virtual async Task<bool> InserirAsync(TEntidade entidade, string indice = "")
        {
            var nomeIndice = ObterNomeIndice(indice);

            if (!string.IsNullOrEmpty(nomeIndice))
            {
                var response = await servicoTelemetria.RegistrarComRetornoAsync<IndexResponse>(async () =>
                        await _elasticClient.IndexAsync(entidade, r => r.Index(nomeIndice)),
                    "Elastic",
                    $"Insert {entidade.GetType().Name}",
                    nomeIndice,
                    JsonConvert.SerializeObject(entidade));

                if (!response.ApiCallDetails.HasSuccessfulStatusCode)
                    throw new InvalidOperationException(response.ElasticsearchServerError?.ToString(),
                        response.ApiCallDetails.OriginalException);
            }

            return true;
        }

        public async Task ExcluirTodos(string indice = "")
        {
            var nomeIndice = ObterNomeIndice(indice);

            var response = await servicoTelemetria.RegistrarComRetornoAsync<DeleteByQueryResponse>(async () =>
                    await _elasticClient.DeleteByQueryAsync<TEntidade>(
                        nomeIndice,
                        r => r.Query(q => q.MatchAll(new MatchAllQuery()))),
                "Elastic",
                $"Excluir Todos [{nomeIndice}]",
                indice);

            if (!response.ApiCallDetails.HasSuccessfulStatusCode)
                throw new InvalidOperationException(response.ElasticsearchServerError?.ToString(),
                    response.ApiCallDetails.OriginalException);
        }

        public async Task ExcluirPorId(string id, string indice = "")
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromMinutes(5));
            var nomeIndice = ObterNomeIndice(indice);

            var response = await servicoTelemetria.RegistrarComRetornoAsync<DeleteByQueryResponse>(async () =>
                    await _elasticClient.DeleteByQueryAsync<TEntidade>(
                        nomeIndice,
                        r => r.Query(q => q
                            .Term(t => t
                                .Field(new Field("_id"))
                                .Value(id))),
                        cts.Token),
                "Elastic",
                $"Excluir Id [{nomeIndice}-{id}]",
                indice);

            if (!response.ApiCallDetails.HasSuccessfulStatusCode)
                throw new InvalidOperationException(response.ElasticsearchServerError?.ToString(),
                    response.ApiCallDetails.OriginalException);
        }

        private string ObterNomeIndice(string indice = "")
        {
            var nomeIndice = indice;

            if (string.IsNullOrEmpty(indice))
                nomeIndice = string.IsNullOrEmpty(indicePadraoRepositorio)
                    ? elasticOptions.IndicePadrao
                    : indicePadraoRepositorio;

            return $"{elasticOptions.Prefixo}{nomeIndice}";
        }
    }
}