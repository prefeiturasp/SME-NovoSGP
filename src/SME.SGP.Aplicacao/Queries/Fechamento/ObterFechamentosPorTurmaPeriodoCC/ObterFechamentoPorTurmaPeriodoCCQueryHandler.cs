using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterFechamentoPorTurmaPeriodoCCQueryHandler : IRequestHandler<ObterFechamentosPorTurmaPeriodoCCQuery, IEnumerable<FechamentoPorTurmaPeriodoCCDto>>
    {
        private readonly IRepositorioFechamentoTurmaConsulta repositorioFechamentoTurma;
        private readonly IRepositorioCache repositorioCache;

        public ObterFechamentoPorTurmaPeriodoCCQueryHandler(IRepositorioFechamentoTurmaConsulta repositorioFechamentoTurma,
            IRepositorioCache repositorioCache)
        {
            this.repositorioFechamentoTurma = repositorioFechamentoTurma ?? throw new ArgumentNullException(nameof(repositorioFechamentoTurma));
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
        }

        private static async Task<List<FechamentoPorTurmaPeriodoCCDto>> MapearDadosDbParaCache(IEnumerable<FechamentoTurmaDisciplina> dadosBd)
        {
            if (dadosBd.EhNulo())
                return null;

            var dadosCache = new List<FechamentoPorTurmaPeriodoCCDto>();
            
            foreach (var fechamentoTurmaDisciplina in dadosBd)
            {
                var cacheFechamentoTurmaDisciplina = new FechamentoPorTurmaPeriodoCCDto
                {
                    Id = fechamentoTurmaDisciplina.Id,
                    Situacao = fechamentoTurmaDisciplina.Situacao,
                    CriadoEm = fechamentoTurmaDisciplina.CriadoEm,
                    CriadoPor = fechamentoTurmaDisciplina.CriadoPor,
                    CriadoRF = fechamentoTurmaDisciplina.CriadoRF
                };

                foreach (var fechamentoAluno in fechamentoTurmaDisciplina.FechamentoAlunos)
                {
                    var cacheFechamentoAluno = new FechamentoAlunoPorTurmaPeriodoCCDto
                    {
                        AlunoCodigo = fechamentoAluno.AlunoCodigo
                    };
                    
                    foreach (var cacheFechamentoNota in fechamentoAluno.FechamentoNotas.Select(fechamentoAlunoFechamentoNota => new FechamentoNotaPorTurmaPeriodoCCDto
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
                        }))
                    {
                        cacheFechamentoAluno.FechamentoNotas.Add(cacheFechamentoNota);
                    }
                    
                    cacheFechamentoTurmaDisciplina.FechamentoAlunos.Add(cacheFechamentoAluno);
                }
                
                dadosCache.Add(cacheFechamentoTurmaDisciplina);
            }

            return await Task.FromResult(dadosCache);
        }

        public async Task<IEnumerable<FechamentoPorTurmaPeriodoCCDto>> Handle(ObterFechamentosPorTurmaPeriodoCCQuery request, CancellationToken cancellationToken)
        {
            var nomeChave = string.Format(NomeChaveCache.FECHAMENTO_NOTA_TURMA_PERIODO_COMPONENTE,
                request.TurmaId, request.PeriodoEscolarId, request.ComponenteCurricularId);

            var retornoCache = await repositorioCache.ObterObjetoAsync<List<FechamentoPorTurmaPeriodoCCDto>>(nomeChave,
                "Obter fechamento por turma, período e conselho de classe");
            
            if (retornoCache.NaoEhNulo()) 
                return retornoCache;
            
            var dadosBd = await repositorioFechamentoTurma.ObterPorTurmaPeriodoCCAsync(request.TurmaId,
                request.PeriodoEscolarId, request.ComponenteCurricularId, request.EhRegencia);

            var retornoDb = await MapearDadosDbParaCache(dadosBd);
            await repositorioCache.SalvarAsync(nomeChave, retornoDb);

            return retornoDb;
        }
    }
}
