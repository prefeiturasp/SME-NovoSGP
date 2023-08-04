﻿using MediatR;
using Newtonsoft.Json;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesPorTurmasCodigoQueryHandler : IRequestHandler<ObterComponentesCurricularesPorTurmasCodigoQuery, IEnumerable<DisciplinaDto>>
    {
        private readonly IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ;
        private readonly IRepositorioComponenteCurricularConsulta repositorioComponenteCurricular;
        private readonly IMediator mediator;
        private static readonly long[] IDS_COMPONENTES_REGENCIA = { 2, 7, 8, 89, 138 };

        public ObterComponentesCurricularesPorTurmasCodigoQueryHandler(IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ,
            IRepositorioComponenteCurricularConsulta repositorioComponenteCurricular, IMediator mediator)
        {
            this.repositorioAtribuicaoCJ = repositorioAtribuicaoCJ ?? throw new ArgumentNullException(nameof(repositorioAtribuicaoCJ));
            this.repositorioComponenteCurricular = repositorioComponenteCurricular ?? throw new ArgumentNullException(nameof(repositorioComponenteCurricular));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public async Task<IEnumerable<DisciplinaDto>> Handle(ObterComponentesCurricularesPorTurmasCodigoQuery request, CancellationToken cancellationToken)
        {
            var disciplinasDto = new List<DisciplinaDto>();
            IEnumerable<DisciplinaResposta> disciplinas = new List<DisciplinaResposta>();
            IEnumerable<DisciplinaResposta> disciplinasCJ = new List<DisciplinaResposta>();

            if (request.PerfilAtual == Perfis.PERFIL_CJ || request.PerfilAtual == Perfis.PERFIL_CJ_INFANTIL)
            {
                var atribuicoes = await repositorioAtribuicaoCJ.ObterPorFiltros(request.TurmaModalidade, string.Empty, string.Empty, 0, request.LoginAtual, string.Empty, true, string.Empty, request.TurmasCodigo);

                if (atribuicoes != null && atribuicoes.Any())
                {
                    var atribuicoesIds = atribuicoes.Select(a => a.DisciplinaId).Distinct().ToArray();
                    var disciplinasEol = await repositorioComponenteCurricular.ObterDisciplinasPorIds(atribuicoesIds);

                    disciplinasCJ = TransformarListaDisciplinaEolParaRetornoDto(disciplinasEol);

                    disciplinasCJ = await ObterDisciplinasCJRegencia(disciplinasCJ);
                }
            }
            disciplinas = await mediator.Send(new ObterDisciplinasTurmasEolQuery(request.TurmasCodigo, request.AdicionarComponentesPlanejamento));

            if (disciplinas != null && disciplinas.Any(a => a.Regencia) && request.AdicionarComponentesPlanejamento)
            {
                var regencias = await repositorioComponenteCurricular.ObterComponentesCurricularesRegenciaPorAnoETurno(request.TurmaAno, request.TurnoParaComponentesCurriculares);
                var novasDisciplinasSemRegencia = disciplinas.Where(a => !a.Regencia).ToList();
                var novasDisciplinasRegencia = disciplinas.Where(a => a.Regencia && regencias.Any(b => b.CodigoComponenteCurricular == a.CodigoComponenteCurricular)).ToList();
                await adicionarComponenteRegenciaPaiAsync(request.TurmasCodigo, novasDisciplinasRegencia);
                disciplinas = novasDisciplinasSemRegencia.Union(novasDisciplinasRegencia).OrderBy(a => a.Nome).ToList();
            }

            if (disciplinas != null && disciplinas.Any())
                disciplinasDto = await ChecarSeComponenteLancaFrequenciaSgp(await MapearParaDto(disciplinas, disciplinasCJ, request.TemEnsinoEspecial));

            return disciplinasDto;
        }

        private async Task adicionarComponenteRegenciaPaiAsync(string[] turmasCodigo, List<DisciplinaResposta> novasDisciplinasRegencia)
        {
            foreach (var turma in turmasCodigo)
            {
                var componenteRegenciaTurmaPai = (await mediator.Send(new ObterComponentesCurricularesPorTurmaCodigoQuery(turma))).FirstOrDefault(x => x.Regencia == true);
                if (componenteRegenciaTurmaPai != null)
                    novasDisciplinasRegencia.ForEach(d =>
                    {
                        if (d.Regencia && (d.CodigoComponenteCurricularPai == 0 || d.CodigoComponenteCurricularPai == null))
                        {
                            d.CodigoComponenteCurricularPai = componenteRegenciaTurmaPai.CodigoComponenteCurricular;
                        }
                    });
            }
        }

        private async Task<IEnumerable<DisciplinaResposta>> ObterDisciplinasCJRegencia(IEnumerable<DisciplinaResposta> disciplinas)
        {
            if (disciplinas.Any(a => a.Regencia))
            {
                var disciplinasRegenciaEolCodigos = disciplinas.Where(a => a.Regencia).Select(a => a.CodigoComponenteCurricular).Distinct().ToArray();
                var disciplinasRegenciaEol = await repositorioComponenteCurricular.ObterDisciplinasPorIds(disciplinasRegenciaEolCodigos);
                if (disciplinasRegenciaEol.Any())
                {
                    var componentesRegencia = await repositorioComponenteCurricular.ObterDisciplinasPorIds(IDS_COMPONENTES_REGENCIA);
                    var componentesRegenciaDto = TransformarListaDisciplinaEolParaRetornoDto(componentesRegencia);
                    disciplinas = disciplinas.Union(componentesRegenciaDto);
                }
            }

            return disciplinas;
        }

        private async Task<List<DisciplinaDto>> MapearParaDto(IEnumerable<DisciplinaResposta> disciplinas, IEnumerable<DisciplinaResposta> disciplinasCJ, bool turmaEspecial)
        {
            var retorno = new List<DisciplinaDto>();

            if (disciplinas != null)
            {
                var disciplinasCodigos = disciplinas.Select(a => a.CodigoComponenteCurricular);
                var disciplinasComObjetivos = await ObterComponentesPossuiObjetivos(disciplinasCodigos);

                foreach (var disciplinaCj in disciplinasCJ)
                    retorno.Add(MapearParaDto(disciplinaCj, disciplinasComObjetivos, turmaEspecial));

                foreach (var disciplina in disciplinas)
                {
                    var jaAdicionadaPorTurma = retorno.Any(d => !string.IsNullOrEmpty(d.TurmaCodigo) && d.TurmaCodigo.Equals(disciplina.TurmaCodigo) &&
                                                                d.CodigoComponenteCurricular == disciplina.CodigoComponenteCurricular);
                    if (!jaAdicionadaPorTurma)
                        retorno.Add(MapearParaDto(disciplina, disciplinasComObjetivos, turmaEspecial));
                }

            }
            return retorno;
        }

        private async Task<List<long>> ObterComponentesPossuiObjetivos(IEnumerable<long> disciplinasCodigos)
        {
            var componentes = await repositorioComponenteCurricular.ListarComponentesCurriculares();
            var componentesComObjetivos = componentes.Where(a => disciplinasCodigos.Contains(long.Parse(a.Codigo))).Select(a => long.Parse(a.Codigo)).ToList();
            return componentesComObjetivos;
        }

        private DisciplinaDto MapearParaDto(DisciplinaResposta disciplina, List<long> componentesComObjetivos, bool turmaEspecial) => new DisciplinaDto()
        {
            CdComponenteCurricularPai = disciplina.CodigoComponenteCurricularPai,
            CodigoComponenteCurricular = disciplina.CodigoComponenteCurricular,
            CodigoTerritorioSaber = disciplina.CodigoComponenteTerritorioSaber ?? 0,
            GrupoMatrizId = disciplina.GrupoMatriz.Id,
            GrupoMatrizNome = disciplina.GrupoMatriz.Nome,
            Nome = disciplina.Nome,
            Regencia = disciplina.Regencia,
            TerritorioSaber = disciplina.TerritorioSaber,
            Compartilhada = disciplina.Compartilhada,
            RegistraFrequencia = disciplina.RegistroFrequencia,
            LancaNota = disciplina.LancaNota,
            PossuiObjetivos = componentesComObjetivos.Any(a => a == disciplina.CodigoComponenteCurricular),
            ObjetivosAprendizagemOpcionais = ComponentePossuiObjetivosOpcionais(disciplina.CodigoComponenteCurricular, disciplina.Regencia, turmaEspecial),
            TurmaCodigo = disciplina.TurmaCodigo
        };

        public bool ComponentePossuiObjetivosOpcionais(long componenteCurricularCodigo, bool regencia, bool turmaEspecial)
        {
            return turmaEspecial && (regencia || new long[] { 218, 138, 1116 }.Contains(componenteCurricularCodigo));
        }

        private IEnumerable<DisciplinaResposta> TransformarListaDisciplinaEolParaRetornoDto(IEnumerable<DisciplinaDto> disciplinasEol)
        {
            foreach (var disciplinaEol in disciplinasEol)
            {
                yield return MapearDisciplinaResposta(disciplinaEol);
            }
        }
        private DisciplinaResposta MapearDisciplinaResposta(DisciplinaDto disciplinaEol) => new DisciplinaResposta()
        {
            CodigoComponenteCurricular = disciplinaEol.CodigoComponenteCurricular,
            CodigoComponenteCurricularPai = disciplinaEol.CdComponenteCurricularPai,
            Nome = disciplinaEol.Nome,
            Regencia = disciplinaEol.Regencia,
            Compartilhada = disciplinaEol.Compartilhada,
            RegistroFrequencia = disciplinaEol.RegistraFrequencia,
            LancaNota = disciplinaEol.LancaNota,
        };

        private async Task<List<DisciplinaDto>> ChecarSeComponenteLancaFrequenciaSgp(IEnumerable<DisciplinaDto> disciplinasEol)
        {
            var disciplinasAjustadas = disciplinasEol.Where(d => d.RegistraFrequencia).ToList();

            foreach (var disciplina in disciplinasEol.Where(d => !d.RegistraFrequencia).ToList())
            {
                disciplina.RegistraFrequencia = await mediator.Send(new ObterComponenteRegistraFrequenciaQuery(disciplina.CodigoComponenteCurricular));
                disciplinasAjustadas.Add(disciplina);
            }

            return disciplinasAjustadas;
        }
    }
}
