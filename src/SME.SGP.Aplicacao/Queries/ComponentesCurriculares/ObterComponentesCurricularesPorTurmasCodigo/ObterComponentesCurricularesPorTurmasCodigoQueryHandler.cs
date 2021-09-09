using MediatR;
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
        private readonly IRepositorioComponenteCurricular repositorioComponenteCurricular;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IMediator mediator;
        private static readonly long[] IDS_COMPONENTES_REGENCIA = { 2, 7, 8, 89, 138 };

        public ObterComponentesCurricularesPorTurmasCodigoQueryHandler(IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ,
            IRepositorioComponenteCurricular repositorioComponenteCurricular, IHttpClientFactory httpClientFactory, IMediator mediator)
        {
            this.repositorioAtribuicaoCJ = repositorioAtribuicaoCJ ?? throw new ArgumentNullException(nameof(repositorioAtribuicaoCJ));
            this.repositorioComponenteCurricular = repositorioComponenteCurricular ?? throw new ArgumentNullException(nameof(repositorioComponenteCurricular));
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
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
            disciplinas = await ObterDisciplinasTurmasEol(request, request.AdicionarComponentesPlanejamento);

            if (disciplinas != null && disciplinas.Any(a => a.Regencia) && request.AdicionarComponentesPlanejamento)
            {
                var regencias = await repositorioComponenteCurricular.ObterComponentesCurricularesRegenciaPorAnoETurno(request.TurmaAno, request.TurnoParaComponentesCurriculares);
                var novasDisciplinasSemRegencia = disciplinas.Where(a => !a.Regencia).ToList();
                var novasDisciplinasRegencia = disciplinas.Where(a => a.Regencia && regencias.Any(b => b.CodigoComponenteCurricular == a.CodigoComponenteCurricular)).ToList();
                disciplinas = novasDisciplinasSemRegencia.Union(novasDisciplinasRegencia).OrderBy(a => a.Nome).ToList();
            }

            if (disciplinas != null && disciplinas.Any())
            {
                disciplinasDto = await MapearParaDto(disciplinas, disciplinasCJ, request.TemEnsinoEspecial);
            }

            return disciplinasDto;

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
        private async Task<IEnumerable<DisciplinaResposta>> ObterDisciplinasTurmasEol(ObterComponentesCurricularesPorTurmasCodigoQuery request, bool adicionarComponentesPlanejamento)
        {
            var turmasCodigo = String.Join("&codigoTurmas=", request.TurmasCodigo);
            var httpClient = httpClientFactory.CreateClient("servicoEOL");
            var resposta = await httpClient.GetAsync($"v1/componentes-curriculares/turmas?codigoTurmas={turmasCodigo}&adicionarComponentesPlanejamento={adicionarComponentesPlanejamento}");
            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                var listaEol = JsonConvert.DeserializeObject<IEnumerable<ComponenteCurricularEol>>(json);
                return TransformarParaDtoDisciplina(listaEol);
            }
            else return default;

        }

        private IEnumerable<DisciplinaResposta> TransformarParaDtoDisciplina(IEnumerable<ComponenteCurricularEol> listaEol)
        {
            foreach (var disciplinaEol in listaEol)
            {
                yield return new DisciplinaResposta()
                {

                    CodigoComponenteCurricular = disciplinaEol.Codigo,
                    CodigoComponenteCurricularPai = disciplinaEol.CodigoComponenteCurricularPai,
                    Nome = disciplinaEol.Descricao,
                    Regencia = disciplinaEol.Regencia,
                    Compartilhada = disciplinaEol.Compartilhada,
                    RegistroFrequencia = disciplinaEol.RegistraFrequencia,
                    LancaNota = disciplinaEol.LancaNota,
                    TurmaCodigo = disciplinaEol.TurmaCodigo
                };
            }
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
    }
}
