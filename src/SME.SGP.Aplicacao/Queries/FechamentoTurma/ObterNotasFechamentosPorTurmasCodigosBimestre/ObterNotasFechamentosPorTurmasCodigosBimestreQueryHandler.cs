using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SME.SGP.Aplicacao
{
    public class ObterNotasFechamentosPorTurmasCodigosBimestreQueryHandler :
        IRequestHandler<ObterNotasFechamentosPorTurmasCodigosBimestreQuery, IEnumerable<NotaConceitoBimestreComponenteDto>>
    {
        private readonly IRepositorioFechamentoNotaConsulta repositorioFechamentoNota;
        private readonly IMediator mediator;

        public ObterNotasFechamentosPorTurmasCodigosBimestreQueryHandler(IRepositorioFechamentoNotaConsulta repositorioFechamentoNota,
            IMediator mediator)
        {
            this.repositorioFechamentoNota = repositorioFechamentoNota ?? throw new ArgumentNullException(nameof(repositorioFechamentoNota));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        private async Task MapearDadosBdParaCacheDto(
            IEnumerable<NotaConceitoBimestreComponenteDto> dadosBd, string alunoCodigo, 
            ICollection<CacheNotaConceitoBimestreTurmaDto> destino)
        {
            var turmasCodigo = dadosBd.Select(c => c.TurmaCodigo).Distinct().ToArray();
            
            foreach (var turmaCodigo in turmasCodigo)
            {
                var resultadosDaTurma = dadosBd.Where(c => c.TurmaCodigo == turmaCodigo).ToList();
                var bimestre = resultadosDaTurma.Select(c => c.Bimestre).FirstOrDefault();
                
                var nomeChave = ObterNomeChave(bimestre, turmaCodigo);
                
                foreach (var cacheNotaConceitoBimestreTurmaSalvar in resultadosDaTurma.Select(resultadoDaTurma => new CacheNotaConceitoBimestreTurmaDto
                {
                    Bimestre = bimestre,
                    TurmaCodigo = turmaCodigo,
                    NotasConceitosComponentes =
                    {
                        new CacheNotaConceitoComponenteDto
                        {
                            Id = resultadoDaTurma.Id,
                            ComponenteCurricularCodigo = resultadoDaTurma.ComponenteCurricularCodigo,
                            ConselhoClasseNotaId = resultadoDaTurma.ConselhoClasseNotaId,
                            AlunoCodigo = alunoCodigo,
                            ConceitoId = resultadoDaTurma.ConceitoId,
                            Nota = resultadoDaTurma.Nota
                        }
                    }
                }))
                {
                    destino.Add(cacheNotaConceitoBimestreTurmaSalvar);

                    await mediator.Send(
                        new SalvarCachePorValorObjetoCommand(nomeChave, cacheNotaConceitoBimestreTurmaSalvar));
                }
            }
        }

        private static async Task<CacheNotaConceitoBimestreTurmaDto> MapearRetornoCacheParaCacheDto(string dadosBimestreTurmaCache)
        {
            return await Task.FromResult(JsonConvert.DeserializeObject<CacheNotaConceitoBimestreTurmaDto>(dadosBimestreTurmaCache));
        }

        private static async Task<IEnumerable<NotaConceitoBimestreComponenteDto>> MapearCacheDtoParaRetornoDto(
            IEnumerable<CacheNotaConceitoBimestreTurmaDto> dadosCache, string alunoCodigo)
        {
            var dadosCacheNotasConceitos = dadosCache.Where(c => c.NotasConceitosComponentes.Select(a => a.AlunoCodigo).Contains(alunoCodigo));

            var retorno = new List<NotaConceitoBimestreComponenteDto>();

            foreach (var item in dadosCacheNotasConceitos)
            {
                retorno.AddRange(item.NotasConceitosComponentes.Select(notaConceitoComponente => new NotaConceitoBimestreComponenteDto
                {
                    Id = notaConceitoComponente.Id,
                    ComponenteCurricularCodigo = notaConceitoComponente.ComponenteCurricularCodigo,
                    ConselhoClasseNotaId = notaConceitoComponente.ConselhoClasseNotaId,
                    Bimestre = item.Bimestre,
                    ConceitoId = notaConceitoComponente.ConceitoId,
                    Nota = notaConceitoComponente.Nota
                }));
            }

            return await Task.FromResult(retorno);
        }

        private static string ObterNomeChave(int? bimestre, string turmaCodigo)
        {
            bimestre ??= 0;
            return $"NotaConceito-{bimestre.ToString()}-{turmaCodigo}";
        }
        
        public async Task<IEnumerable<NotaConceitoBimestreComponenteDto>> Handle(ObterNotasFechamentosPorTurmasCodigosBimestreQuery request,
            CancellationToken cancellationToken)
        {
            var dadosCache = new List<CacheNotaConceitoBimestreTurmaDto>();
            
            foreach (var turmaCodigo in request.TurmasCodigos)
            {
                var nomeChave = ObterNomeChave(request.Bimestre, turmaCodigo);
                var retornoDadosBimestreTurmaCache = await mediator.Send(new ObterCacheAsyncQuery(nomeChave), cancellationToken);
                var dadosBimestreTurmaCache = await MapearRetornoCacheParaCacheDto(retornoDadosBimestreTurmaCache);

                if (dadosBimestreTurmaCache != null)
                    dadosCache.Add(dadosBimestreTurmaCache);
            }

            var turmasCodigosCache = dadosCache.Select(c => c.TurmaCodigo).ToArray();

            //-> Obter a lista dos códigos das turmas que ainda não estão no cache.
            var turmasCodigosParaConsultarBd =
                request.TurmasCodigos.Where(turmaCodigo => !turmasCodigosCache.Contains(turmaCodigo)).ToArray();

            //-> Caso não exista turmas a serem consultadas no banco de dados, retorna as que estão no cache.
            if (!turmasCodigosParaConsultarBd.Any())
                return await MapearCacheDtoParaRetornoDto(dadosCache, request.AlunoCodigo);
           
            //-> Obter os dados do banco apenas para as turmas que ainda não estão no cache.
            var resultados = await repositorioFechamentoNota.ObterNotasAlunoPorTurmasCodigosBimestreAsync(
                turmasCodigosParaConsultarBd, request.AlunoCodigo,
                request.Bimestre, request.DataMatricula, request.DataSituacao);

            await MapearDadosBdParaCacheDto(resultados, request.AlunoCodigo, dadosCache);
            return await MapearCacheDtoParaRetornoDto(dadosCache, request.AlunoCodigo);
        }
    }
}
