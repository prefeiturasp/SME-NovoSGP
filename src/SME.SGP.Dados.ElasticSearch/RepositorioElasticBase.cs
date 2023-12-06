using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Nest;
using Newtonsoft.Json;
using SME.Pedagogico.Interface;
using SME.SGP.Infra;
using SME.SGP.Infra.ElasticSearch;

namespace SME.SGP.Dados.ElasticSearch
{
    public abstract class RepositorioElasticBase<TEntidade> : IRepositorioElasticBase<TEntidade> where TEntidade : class
    {
        private const int QUANTIDADE_RETORNO = 200;
        private readonly IElasticClient _elasticClient;
        private readonly IServicoTelemetria servicoTelemetria;
        private readonly string indicePadraoRepositorio;
        private readonly ElasticOptions elasticOptions;

        protected RepositorioElasticBase(IElasticClient elasticClient,
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
            ExistsResponse response = await servicoTelemetria.RegistrarComRetornoAsync<ExistsResponse>(async () =>
                                                                                        await _elasticClient.DocumentExistsAsync(DocumentPath<TEntidade>.Id(id).Index(indice)),
                                                                                        "Elastic",
                                                                                        nomeConsulta,
                                                                                        indice,
                                                                                        parametro?.ToString());

            if (!response.IsValid)
                throw new InvalidOperationException(response.ServerError?.ToString(), response.OriginalException);

            return response.Exists;
        }

        public async Task<TEntidade> ObterAsync(string indice, string id, string nomeConsulta, object parametro = null)
        {
            GetResponse<TEntidade> response = await servicoTelemetria.RegistrarComRetornoAsync<GetResponse<TEntidade>>(async () =>
                                                                                        await _elasticClient.GetAsync(DocumentPath<TEntidade>.Id(id).Index(indice)),
                                                                                        "Elastic",
                                                                                        nomeConsulta,
                                                                                        indice,
                                                                                        parametro?.ToString());

            if (response.IsValid)
                return response.Source;

            return null;
        }

        public async Task<IEnumerable<TEntidade>> ObterListaAsync(string indice, IEnumerable<string> ids, string nomeConsulta, object parametro = null)
        {
            IEnumerable<IMultiGetHit<TEntidade>> response = await servicoTelemetria.RegistrarComRetornoAsync<IEnumerable<IMultiGetHit<TEntidade>>>(async () =>
                                                                                        await _elasticClient.GetManyAsync<TEntidade>(ids, indice),
                                                                                        "Elastic",
                                                                                        nomeConsulta,
                                                                                        indice,
                                                                                        parametro?.ToString());

            return response.Select(item => item.Source).ToList();
        }

        public async Task<IEnumerable<TEntidade>> ObterListaAsync(string indice, Func<QueryContainerDescriptor<TEntidade>, QueryContainer> request, string nomeConsulta, object parametro = null)
        {
            var listaDeRetorno = ObtenhaInstancia();

            ISearchResponse<TEntidade> response = await servicoTelemetria.RegistrarComRetornoAsync<ISearchResponse<TEntidade>>(async () =>
                                                                                       await _elasticClient.SearchAsync<TEntidade>(s => s.Index(indice)
                                                                                                                                .Query(request)
                                                                                                                                .Scroll("10s")
                                                                                                                                .Size(QUANTIDADE_RETORNO)),
                                                                                       "Elastic",
                                                                                       nomeConsulta,
                                                                                       indice,
                                                                                       parametro?.ToString());

            if (!response.IsValid)
                throw new InvalidOperationException(response.ServerError?.ToString(), response.OriginalException);

            listaDeRetorno.AddRange(response.Documents);

            while (response.Documents.Any() && response.Documents.Count == QUANTIDADE_RETORNO)
            {
                response = await servicoTelemetria.RegistrarComRetornoAsync<ISearchResponse<TEntidade>>(async () =>
                                                                                        await _elasticClient.ScrollAsync<TEntidade>("10s", response.ScrollId),
                                                                                        "Elastic",
                                                                                        nomeConsulta + " scroll",
                                                                                        indice,
                                                                                        parametro?.ToString());
                listaDeRetorno.AddRange(response.Documents);
            }

            this._elasticClient.ClearScroll(new ClearScrollRequest(response.ScrollId));

            return listaDeRetorno;
        }

        public async Task<IEnumerable<TEntidade>> ObterTodosAsync(string indice, string nomeConsulta, object parametro = null)
        {
            var search = new SearchDescriptor<TEntidade>(indice).MatchAll();
            ISearchResponse<TEntidade> response = await servicoTelemetria.RegistrarComRetornoAsync<ISearchResponse<TEntidade>>(async () =>
                                                                                await _elasticClient.SearchAsync<TEntidade>(search),
                                                                                "Elastic",
                                                                                nomeConsulta,
                                                                                indice,
                                                                                parametro?.ToString());

            if (!response.IsValid)
                throw new InvalidOperationException(response.ServerError?.ToString(), response.OriginalException);

            return response.Hits.Select(hit => hit.Source).ToList();
        }

        public async Task<long> ObterTotalDeRegistroAsync(string indice, string nomeConsulta, object parametro = null)
        {
            var search = new SearchDescriptor<TEntidade>(indice).MatchAll();
            ISearchResponse<TEntidade> response = await servicoTelemetria.RegistrarComRetornoAsync<ISearchResponse<TEntidade>>(async () =>
                                                                                await _elasticClient.SearchAsync<TEntidade>(search),
                                                                                "Elastic",
                                                                                nomeConsulta,
                                                                                indice,
                                                                                parametro?.ToString());

            if (!response.IsValid)
                throw new Exception(response.ServerError?.ToString(), response.OriginalException);

            return response.Total;
        }

        public virtual async Task<bool> InserirAsync(TEntidade entidade, string indice = "")
        {
            var nomeIndice = ObterNomeIndice(indice);

            if (!string.IsNullOrEmpty(nomeIndice))
            {
                var response = await servicoTelemetria.RegistrarComRetornoAsync<ISearchResponse<TEntidade>>(async () =>
                         await _elasticClient.IndexAsync(entidade, descriptor => descriptor.Index(nomeIndice)),
                    "Elastic",
                    $"Insert {entidade.GetType().Name}",
                    nomeIndice,
                    JsonConvert.SerializeObject(entidade));

                if (!response.IsValid)
                    throw new InvalidOperationException(response.ServerError?.ToString(), response.OriginalException);
            }

            return true;
        }

        private string ObterNomeIndice(string indice = "")
        {
            var nomeIndice = string.IsNullOrEmpty(indice) ?
                string.IsNullOrEmpty(indicePadraoRepositorio) ?
                    elasticOptions.IndicePadrao :
                    indicePadraoRepositorio :
                indice;

            return $"{elasticOptions.Prefixo}{nomeIndice}";
        }

        private List<TEntidade> ObtenhaInstancia()
        {
            Type tipoGenerico = typeof(List<>);
            Type construtor = tipoGenerico.MakeGenericType(typeof(TEntidade));

            return (List<TEntidade>)Activator.CreateInstance(construtor);
        }

        public async Task ExcluirTodos(string indice = "")
        {
            var nomeIndice = ObterNomeIndice(indice);
            DeleteByQueryResponse response = await servicoTelemetria.RegistrarComRetornoAsync<DeleteByQueryResponse>(async () =>
                await _elasticClient.DeleteByQueryAsync<TEntidade>(q => q
                      .Index(nomeIndice)
                      .Query(rq => rq.MatchAll())),
                "Elastic",
                $"Excluir Todos [{nomeIndice}]",
                indice);

            if (!response.IsValid)
                throw new InvalidOperationException(response.ServerError?.ToString(), response.OriginalException);
        }
    }
}
