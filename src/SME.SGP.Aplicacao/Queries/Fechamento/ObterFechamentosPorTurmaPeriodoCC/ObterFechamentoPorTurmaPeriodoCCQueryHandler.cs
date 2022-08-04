using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterFechamentoPorTurmaPeriodoCCQueryHandler : IRequestHandler<ObterFechamentosPorTurmaPeriodoCCQuery, IEnumerable<CacheFechamentoTurmaDisciplinaDto>>
    {
        private readonly IRepositorioFechamentoTurmaConsulta repositorioFechamentoTurma;
        private readonly IMediator mediator;

        public ObterFechamentoPorTurmaPeriodoCCQueryHandler(IRepositorioFechamentoTurmaConsulta repositorioFechamentoTurma,
            IMediator mediator)
        {
            this.repositorioFechamentoTurma = repositorioFechamentoTurma ?? throw new ArgumentNullException(nameof(repositorioFechamentoTurma));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        private static async Task<List<CacheFechamentoTurmaDisciplinaDto>> MapearDadosDbParaCache(IEnumerable<FechamentoTurmaDisciplina> dadosBd)
        {
            if (dadosBd == null)
                return null;

            var dadosCache = new List<CacheFechamentoTurmaDisciplinaDto>();
            
            foreach (var fechamentoTurmaDisciplina in dadosBd)
            {
                var cacheFechamentoTurmaDisciplina = new CacheFechamentoTurmaDisciplinaDto
                {
                    Id = fechamentoTurmaDisciplina.Id,
                    Situacao = fechamentoTurmaDisciplina.Situacao,
                    CriadoEm = fechamentoTurmaDisciplina.CriadoEm,
                    CriadoPor = fechamentoTurmaDisciplina.CriadoPor,
                    CriadoRF = fechamentoTurmaDisciplina.CriadoRF
                };

                foreach (var fechamentoAluno in fechamentoTurmaDisciplina.FechamentoAlunos)
                {
                    var cacheFechamentoAluno = new CacheFechamentoAlunoDto
                    {
                        AlunoCodigo = fechamentoAluno.AlunoCodigo
                    };
                    
                    foreach (var fechamentoAlunoFechamentoNota in fechamentoAluno.FechamentoNotas)
                    {
                        var cacheFechamentoNota = new CacheFechamentoNotaDto
                        {
                            Id = fechamentoAlunoFechamentoNota.Id,
                            Nota = fechamentoAlunoFechamentoNota.Nota,
                            AlteradoEm = fechamentoAlunoFechamentoNota.AlteradoEm,
                            AlteradoPor = fechamentoAlunoFechamentoNota.AlteradoPor,
                            ConceitoId = fechamentoAlunoFechamentoNota.ConceitoId,
                            CriadoEm = fechamentoAlunoFechamentoNota.CriadoEm,
                            CriadoPor = fechamentoAlunoFechamentoNota.CriadoPor,
                            DisciplinaId = fechamentoAlunoFechamentoNota.DisciplinaId,
                            AlteradoRF = fechamentoAlunoFechamentoNota.AlteradoRF,
                            CriadoRF = fechamentoAlunoFechamentoNota.CriadoRF
                        };
                        
                        cacheFechamentoAluno.FechamentoNotas.Add(cacheFechamentoNota);
                    }
                    
                    cacheFechamentoTurmaDisciplina.FechamentoAlunos.Add(cacheFechamentoAluno);
                }
                
                dadosCache.Add(cacheFechamentoTurmaDisciplina);
            }

            return await Task.FromResult(dadosCache);
        }
        
        private static async Task<List<CacheFechamentoTurmaDisciplinaDto>> MapearDadosCacheParaRetorno(string dadosCache)
        {
            if (string.IsNullOrEmpty(dadosCache))
                return null;

            return await Task.FromResult(JsonConvert.DeserializeObject<List<CacheFechamentoTurmaDisciplinaDto>>(dadosCache));
        }        

        public async Task<IEnumerable<CacheFechamentoTurmaDisciplinaDto>> Handle(ObterFechamentosPorTurmaPeriodoCCQuery request, CancellationToken cancellationToken)
        {
            var nomeChave =
                $"FechamentoNotas-{request.TurmaId.ToString()}-{request.PeriodoEscolarId.ToString()}-{request.ComponenteCurricularId.ToString()}";

            var dadosCache = await mediator.Send(new ObterCacheAsyncQuery(nomeChave), cancellationToken);
            var retornoCache = await MapearDadosCacheParaRetorno(dadosCache);

            if (retornoCache != null) 
                return retornoCache;
            
            var dadosBd = await repositorioFechamentoTurma.ObterPorTurmaPeriodoCCAsync(request.TurmaId,
                request.PeriodoEscolarId, request.ComponenteCurricularId);

            var retornoDb = await MapearDadosDbParaCache(dadosBd);
            await mediator.Send(new SalvarCachePorValorObjectCommand(nomeChave, retornoDb), cancellationToken);

            return retornoDb;
        }
    }
}
