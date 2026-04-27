using MediatR;
using Newtonsoft.Json;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasDisciplina : AbstractUseCase, IConsultasDisciplina
    {
        private static readonly long[] IDS_COMPONENTES_REGENCIA = { 2, 7, 8, 89, 138 };
        private readonly IConsultasObjetivoAprendizagem consultasObjetivoAprendizagem;
        private readonly IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ;
        private readonly IRepositorioCache repositorioCache;
        private readonly IRepositorioComponenteCurricularJurema repositorioComponenteCurricularJurema;

        private const int ID_COMPONENTE_INFORMATICA_OIE = 1060;
        private const int ID_COMPONENTE_LEITURA_OSL = 1061;
        private static readonly long[] IDS_COMPONENTES_COMPARTILHADOS_EJA_EF = { ID_COMPONENTE_INFORMATICA_OIE, ID_COMPONENTE_LEITURA_OSL };

        public ConsultasDisciplina(IRepositorioCache repositorioCache,
            IConsultasObjetivoAprendizagem consultasObjetivoAprendizagem,
            IRepositorioComponenteCurricularJurema repositorioComponenteCurricularJurema,
            IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ,
        IMediator mediator) : base(mediator)
        {
            this.repositorioCache = repositorioCache ??
                throw new ArgumentNullException(nameof(repositorioCache));
            this.consultasObjetivoAprendizagem = consultasObjetivoAprendizagem ??
                throw new ArgumentNullException(nameof(consultasObjetivoAprendizagem));
            this.repositorioAtribuicaoCJ = repositorioAtribuicaoCJ ??
                throw new ArgumentNullException(nameof(repositorioAtribuicaoCJ));
            this.repositorioComponenteCurricularJurema = repositorioComponenteCurricularJurema ??
                throw new ArgumentNullException(nameof(repositorioComponenteCurricularJurema));
        }

        public async Task<IEnumerable<DisciplinaResposta>> ObterComponentesCJ(Modalidade? modalidade, string codigoTurma, string ueId, long codigoDisciplina, string rf, bool ignorarDeParaRegencia = false)
        {
            IEnumerable<DisciplinaResposta> componentes = null;
            var atribuicoes = await repositorioAtribuicaoCJ.ObterPorFiltros(modalidade,
                codigoTurma,
                ueId,
                codigoDisciplina,
                rf,
                string.Empty,
                true);

            if (atribuicoes.EhNulo() || !atribuicoes.Any())
                return null;

            var disciplinasEol = await mediator.Send(new ObterComponentesCurricularesPorIdsQuery(atribuicoes.Select(a => a.DisciplinaId).Distinct().ToArray()));
            
            var componenteRegencia = disciplinasEol?.FirstOrDefault(c => c.Regencia);
            if (componenteRegencia.EhNulo() || ignorarDeParaRegencia)
                return MapearDisciplinaResposta(disciplinasEol);

            var componentesRegencia = await mediator.Send(new ObterComponentesCurricularesPorIdsQuery(IDS_COMPONENTES_REGENCIA));
            if (componentesRegencia.NaoEhNulo())
                return MapearDisciplinaResposta(componentesRegencia);

            return componentes;
        }

        public async Task<List<DisciplinaDto>> ObterComponentesCurricularesPorProfessorETurma(string codigoTurma, bool turmaPrograma, bool realizarAgrupamentoComponente = false, bool consideraTurmaInfantil = true)
        {
            List<DisciplinaDto> disciplinasDto;
            var disciplinasEol = new List<DisciplinaResposta>();

            var usuarioLogado = await mediator.Send(ObterUsuarioLogadoQuery.Instance);

            var dataInicioNovoSGP = await mediator.Send(new ObterParametroSistemaPorTipoQuery(TipoParametroSistema.DataInicioSGP));

            var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(codigoTurma));
            if (turma.EhNulo())
                throw new NegocioException("Não foi possível encontrar a turma");

            bool ehEnsinoMedio = turma.ModalidadeCodigo == Modalidade.Medio;

            IEnumerable<DisciplinaResposta> disciplinasAtribuicaoCj;

            if (turma.EhNulo())
                throw new NegocioException("Não foi possível encontrar a turma");

            if (usuarioLogado.EhProfessorCj())
            {
                disciplinasAtribuicaoCj = (await ObterDisciplinasPerfilCJ(codigoTurma, usuarioLogado.Login)).ToList();

                var disciplinasEolTratadas = disciplinasAtribuicaoCj.OrderBy(s => s.Nome);

                if (realizarAgrupamentoComponente)
                    disciplinasEolTratadas = disciplinasAtribuicaoCj?.DistinctBy(s => (s.Nome.ToUpper(), s.TerritorioSaber ? s.Professor : null)).OrderBy(s => s.Nome);

                disciplinasDto = (MapearParaDto(disciplinasEolTratadas, ehEnsinoMedio, turmaPrograma, turma.EnsinoEspecial))?.OrderBy(c => c.Nome)?.ToList();
            }
            else
            {
                var componentesCurriculares = new List<ComponenteCurricularEol>();

                if (!usuarioLogado.TemPerfilGestaoUes())
                {
                    componentesCurriculares = (await mediator
                        .Send(new ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQuery(codigoTurma, usuarioLogado.Login, usuarioLogado.PerfilAtual, consideraTurmaInfantil ? turma.EhTurmaInfantil || realizarAgrupamentoComponente : false))).ToList();

                    componentesCurriculares ??= (await mediator
                        .Send(new ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQuery(codigoTurma, usuarioLogado.Login, usuarioLogado.PerfilAtual, consideraTurmaInfantil ? turma.EhTurmaInfantil || realizarAgrupamentoComponente : false, false))).ToList();
                }
                else
                {
                    componentesCurriculares = (await mediator
                        .Send(new ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQuery(codigoTurma, usuarioLogado.Login, usuarioLogado.PerfilAtual, realizarAgrupamentoComponente))).ToList();

                    if (!componentesCurriculares.Any())
                    {
                        var componentesCurricularesDaTurma = await mediator.Send(new ObterDisciplinasPorCodigoTurmaQuery(codigoTurma));

                        if (componentesCurricularesDaTurma.Any() && componentesCurricularesDaTurma.NaoEhNulo())
                        {
                            componentesCurriculares = componentesCurricularesDaTurma.Select(c => new ComponenteCurricularEol()
                            {
                                Codigo = c.CodigoComponenteCurricular,
                                TerritorioSaber = c.TerritorioSaber,
                                CodigoComponenteTerritorioSaber = c.CodigoComponenteTerritorioSaber ?? 0,
                                Descricao = c.Nome,
                                GrupoMatriz = new Dominio.GrupoMatriz() { Id = c.GrupoMatriz.Id, Nome = c.GrupoMatriz.Nome },
                                TurmaCodigo = c.TurmaCodigo,
                                Regencia = c.Regencia
                            }).ToList();
                        }
                    }
                }

                var idsDisciplinas = componentesCurriculares?.Select(a => a.Codigo).ToArray();
                
                if (usuarioLogado.TemPerfilAdmUE() || usuarioLogado.TemPerfilGestaoUes() && !usuarioLogado.EhCP())
                    idsDisciplinas = await ObterDisciplinasAtribuicaoCJParaTurma(codigoTurma, componentesCurriculares, idsDisciplinas);

                disciplinasDto = (await mediator.Send(new ObterComponentesCurricularesPorIdsQuery(idsDisciplinas)))?.OrderBy(c => c.Nome)?.ToList();

                var componentesCurricularesJurema = await repositorioCache.ObterAsync(NomeChaveCache.COMPONENTES_JUREMA, () => Task.FromResult(repositorioComponenteCurricularJurema.Listar()));

                if (componentesCurricularesJurema.EhNulo())
                    throw new NegocioException("Não foi possível recuperar a lista de componentes curriculares.");

                foreach(var d in disciplinasDto)
                {
                    var componenteEOL = componentesCurriculares.FirstOrDefault(a => a.Codigo == d.CodigoComponenteCurricular);
                    d.PossuiObjetivos = PossuiObjetivos(turma, Convert.ToInt32(dataInicioNovoSGP), componenteEOL, componentesCurricularesJurema);
                    d.ObjetivosAprendizagemOpcionais = componenteEOL.PossuiObjetivosDeAprendizagemOpcionais(componentesCurricularesJurema, turma.EnsinoEspecial);
                    d.Nome = componenteEOL.Descricao;
                    d.NomeComponenteInfantil = componenteEOL.DescricaoComponenteInfantil;
                    d.Professor = componenteEOL.Professor;
                };

                if (usuarioLogado.TemPerfilGestaoUes())
                {
                    disciplinasAtribuicaoCj = await ObterDisciplinasPerfilCJ(codigoTurma, usuarioLogado.Login, usuarioLogado.TemPerfilGestaoUes(), turma.Ue.Dre.CodigoDre, turma.Ue.CodigoUe);

                    // desconsidera atribuição cj de território atribuido no EOL
                    var codigosTerritorioAgrupamentos = componentesCurriculares
                        .Where(c => c.TerritorioSaber && c.CodigosTerritoriosAgrupamento.NaoEhNulo() && c.CodigosTerritoriosAgrupamento.Any())
                        .SelectMany(c => c.CodigosTerritoriosAgrupamento);

                    var codigosComponentesPai = disciplinasDto
                        .Where(d => d.CdComponenteCurricularPai.HasValue && d.CdComponenteCurricularPai.Value > 0)
                        .Select(d => d.CdComponenteCurricularPai.Value);

                    var disciplinasAdicionar = disciplinasAtribuicaoCj?
                        .Where(d => !codigosTerritorioAgrupamentos.Contains(d.CodigoComponenteCurricular) && (d.CodigoComponenteCurricularPai.HasValue && d.CodigoComponenteCurricularPai.Value > 0 && !codigosComponentesPai.Contains(d.CodigoComponenteCurricularPai.Value)));

                    if (disciplinasAdicionar.NaoEhNulo() && disciplinasAdicionar.Any())
                        disciplinasDto.AddRange(MapearParaDto(disciplinasAdicionar, turmaPrograma, turma.EnsinoEspecial));

                    disciplinasDto = disciplinasDto.NaoEhNulo() && disciplinasDto.Any() ? disciplinasDto.Where(d => d.TerritorioSaber).DistinctBy(d => (d.CodigoComponenteCurricular, d.Professor))
                        .Concat(disciplinasDto.Where(d => !d.TerritorioSaber).DistinctBy(d => d.CodigoComponenteCurricular))
                        .OrderBy(c => c.Nome).ToList() : null;
                }
            }

            //Exceção para disciplinas 1060 e 1061 que são compartilhadas entre EF e EJA
            if (turma.ModalidadeCodigo == Modalidade.EJA)
                disciplinasDto?.ForEach(disciplina =>
                {
                    if (IDS_COMPONENTES_COMPARTILHADOS_EJA_EF.Contains(disciplina.CodigoComponenteCurricular))
                        disciplina.RegistraFrequencia = false;
                });

            if (turma.ModalidadeCodigo == Modalidade.Medio && turma.TipoTurno == (int)TipoTurnoEOL.Noite && disciplinasDto.Any())
            {
                var idComponenteSalaLeituraEM = 1347;
                var idsComponenteTecAprendizagem = new long[] { 1359, 1347, 1312 };
                foreach (var disciplina in disciplinasDto)
                {
                    if (disciplina.CodigoComponenteCurricular == idComponenteSalaLeituraEM || idsComponenteTecAprendizagem.Contains(disciplina.CodigoComponenteCurricular))
                        disciplina.RegistraFrequencia = false;
                }
            }

            if (turma.ModalidadeCodigo == Modalidade.EducacaoInfantil)
                disciplinasDto = disciplinasDto.DistinctBy(x => x.CodigoComponenteCurricular).ToList();

            return disciplinasDto;
        }
        private bool PossuiObjetivos(
                        Turma turma,
                        int anoInicioSgp,
                        ComponenteCurricularEol componenteEOL,
                        IEnumerable<ComponenteCurricularJurema> componentesCurricularesJurema)
        {
            
            var componentesPAPs = ComponentesCurricularesConstants.IDS_COMPONENTES_CURRICULARES_PAP_NOVO.Select(i => (long)i).ToArray();

            return turma.AnoLetivo >= anoInicioSgp
                    && (turma.TipoTurma != TipoTurma.Programa || componentesPAPs.Contains(componenteEOL.Codigo))
                    && componenteEOL.PossuiObjetivosDeAprendizagem(componentesCurricularesJurema, turma.ModalidadeCodigo);
        }

        private async Task<long[]> ObterDisciplinasAtribuicaoCJParaTurma(string codigoTurma, List<ComponenteCurricularEol> componentesCurriculares, long[] idsDisciplinas)
        {
            var atribuicoesCJTurma = await ObterDisciplinasPerfilCJ(codigoTurma, null);
            var codigosDisciplinasAtribuicao = new long[] { };

            if (atribuicoesCJTurma.NaoEhNulo() && atribuicoesCJTurma.Any())
                codigosDisciplinasAtribuicao = (from a in atribuicoesCJTurma
                                                where !idsDisciplinas.Any(id => a.CodigoComponenteCurricular == id || a.CodigoComponenteTerritorioSaber == id)
                                                select a.TerritorioSaber ? a.CodigoComponenteCurricular : a.CodigoComponenteTerritorioSaber)
                                                .Where(a => a.HasValue)
                                                .Select(a => a.Value)
                                                .Distinct()
                                                .ToArray();

            if (codigosDisciplinasAtribuicao.Any())
            {
                idsDisciplinas = idsDisciplinas.Union(codigosDisciplinasAtribuicao).ToArray();
                var componentesInclusao = await mediator.Send(new ObterComponentesCurricularesPorIdsQuery(codigosDisciplinasAtribuicao));

                componentesInclusao.ToList().ForEach(ci =>
                {
                    componentesCurriculares.Add(new ComponenteCurricularEol()
                    {
                        Codigo = ci.CodigoComponenteCurricular,
                        CodigoComponenteCurricularPai = ci.CdComponenteCurricularPai,
                        CodigoComponenteTerritorioSaber = ci.CodigoComponenteCurricularTerritorioSaber,
                        Compartilhada = ci.Compartilhada,
                        Descricao = ci.NomeComponenteInfantil ?? ci.Nome,
                        LancaNota = ci.LancaNota,
                        PossuiObjetivos = ci.PossuiObjetivos,
                        Regencia = ci.Regencia,
                        RegistraFrequencia = ci.RegistraFrequencia,
                        TerritorioSaber = ci.TerritorioSaber,
                        TurmaCodigo = ci.TurmaCodigo,
                    });
                });
            }

            return idsDisciplinas;
        }

        public async Task<IEnumerable<DisciplinaDto>> ObterComponentesCurricularesPorProfessorETurmaParaPlanejamento(long codigoDisciplina, string codigoTurma, bool turmaPrograma, bool regencia)
        {
            List<DisciplinaDto> disciplinasDto;
            var usuario = await mediator.Send(ObterUsuarioLogadoQuery.Instance);
            var dataInicioNovoSGP = await mediator.Send(new ObterParametroSistemaPorTipoQuery(TipoParametroSistema.DataInicioSGP));

            var chaveCache = string.Format(NomeChaveCache.COMPONENTES_PLANEJAMENTO_TURMA_COMPONENTE_PERFIL, codigoTurma, codigoDisciplina, usuario.PerfilAtual);
            if (!usuario.EhProfessor() && !usuario.EhProfessorCj() && !usuario.EhProfessorPoa())
            {
                var disciplinasCacheString = await repositorioCache.ObterAsync(chaveCache);

                if (!string.IsNullOrWhiteSpace(disciplinasCacheString))
                {
                    disciplinasDto = JsonConvert.DeserializeObject<List<DisciplinaDto>>(disciplinasCacheString);
                    var disciplinas = await TratarRetornoDisciplinasPlanejamento(disciplinasDto, codigoDisciplina, regencia, codigoTurma);
                    return disciplinas?.OrderBy(c => c.Nome)?.ToList();
                }
            }

            var componentesCurricularesJurema = await repositorioCache.ObterAsync(NomeChaveCache.COMPONENTES_JUREMA, () => Task.FromResult(repositorioComponenteCurricularJurema.Listar()));
            if (componentesCurricularesJurema.EhNulo())
            {
                throw new NegocioException("Não foi possível recuperar a lista de componentes curriculares.");
            }

            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(codigoTurma));
            if (turma.EhNulo())
                throw new NegocioException("Não foi possível encontrar a turma");

            bool ehEnsinoMedio = turma.ModalidadeCodigo == Modalidade.Medio;

            if (usuario.EhProfessorCj())
            {
                var componentesCJ = await ObterComponentesCJ(null, codigoTurma,
                    string.Empty,
                    codigoDisciplina,
                    usuario.Login);
                disciplinasDto = (MapearParaDto(componentesCJ, ehEnsinoMedio, turmaPrograma))?.OrderBy(c => c.Nome)?.ToList();
            }
            else
            {
                var componentesCurriculares = await mediator.Send(new ObterComponentesCurricularesPorCodigoTurmaLoginEPerfilParaPlanejamentoQuery(codigoTurma, usuario.Login, usuario.PerfilAtual));

                if (turma.ModalidadeCodigo == Modalidade.EJA)
                    componentesCurriculares = RemoverEdFisicaEJA(componentesCurriculares);

                disciplinasDto = (await mediator.Send(new ObterComponentesCurricularesPorIdsQuery(componentesCurriculares.Select(a => a.Codigo).ToArray())))?.OrderBy(c => c.Nome)?.ToList();
                
                disciplinasDto?.ForEach(d =>
                {
                    var componenteEOL = componentesCurriculares.FirstOrDefault(a => a.Codigo == d.CodigoComponenteCurricular);
                    d.PossuiObjetivos = turma.AnoLetivo < Convert.ToInt32(dataInicioNovoSGP) ? false : componenteEOL.PossuiObjetivosDeAprendizagem(componentesCurricularesJurema, turma.ModalidadeCodigo);
                    d.Regencia = componenteEOL.Regencia;
                    d.ObjetivosAprendizagemOpcionais = componenteEOL.PossuiObjetivosDeAprendizagemOpcionais(componentesCurricularesJurema, turma.EnsinoEspecial);
                });
            }

            if (!usuario.EhProfessor() && !usuario.EhProfessorCj() && !usuario.EhProfessorPoa())
                await repositorioCache.SalvarAsync(chaveCache, JsonConvert.SerializeObject(disciplinasDto));

            return await TratarRetornoDisciplinasPlanejamento(disciplinasDto, codigoDisciplina, regencia, codigoTurma, usuario.EhProfessorCj());
        }

        private IEnumerable<ComponenteCurricularEol> RemoverEdFisicaEJA(IEnumerable<ComponenteCurricularEol> componentesCurriculares)
            => componentesCurriculares.Where(c => c.Codigo != MensagemNegocioComponentesCurriculares.COMPONENTE_CURRICULAR_CODIGO_ED_FISICA);

        public async Task<DisciplinaDto> ObterDisciplina(long disciplinaId)
        {
            var disciplina = await mediator.Send(new ObterComponenteCurricularPorIdQuery(disciplinaId));
            if (disciplina.EhNulo()) 
                throw new NegocioException($"Componente curricular não localizado no SGP [{disciplinaId}]");

            return disciplina;
        }

        private IEnumerable<DisciplinaResposta> MapearDto(IEnumerable<ComponenteCurricularEol> componentesCurriculares)
        {
            if (componentesCurriculares.EhNulo() || !componentesCurriculares.Any())
                return Enumerable.Empty<DisciplinaResposta>();
            return componentesCurriculares.Select(cc => new DisciplinaResposta()
            {
                CodigoComponenteCurricular = cc.Codigo,
                Id = cc.Codigo,
                Compartilhada = cc.Compartilhada,
                CodigoComponenteCurricularPai = cc.CodigoComponenteCurricularPai,
                CodigoComponenteTerritorioSaber = cc.CodigoComponenteTerritorioSaber,
                Nome = cc.Descricao,
                Regencia = cc.Regencia,
                RegistroFrequencia = cc.RegistraFrequencia,
                TerritorioSaber = cc.TerritorioSaber,
                LancaNota = cc.LancaNota,
                BaseNacional = cc.BaseNacional,
                TurmaCodigo = cc.TurmaCodigo,
                GrupoMatriz = cc.GrupoMatriz.NaoEhNulo() ? new Integracoes.Respostas.GrupoMatriz() { Id = cc.GrupoMatriz.Id, Nome = cc.GrupoMatriz.Nome } : null,
                NomeComponenteInfantil = cc.DescricaoComponenteInfantil,
                Professor = cc.Professor,
                CodigosTerritoriosAgrupamento = cc.CodigosTerritoriosAgrupamento
            });
        }

        public async Task<IEnumerable<DisciplinaResposta>> ObterDisciplinasPerfilCJ(string codigoTurma, string login, bool verificaPerfilGestao = false, string codigoDre = null, string codigoUe = null)
        {
            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(codigoTurma));
            var atribuicoes = await ObterAtribuicoesCJ(codigoTurma, codigoDre, codigoUe, verificaPerfilGestao, login);
            if (atribuicoes.NaoPossuiRegistros())
                return Enumerable.Empty<DisciplinaResposta>();

            var disciplinasEol = await mediator.Send(new ObterComponentesCurricularesPorIdsQuery(atribuicoes.Select(a => a.DisciplinaId).Distinct().ToArray()));
            var professoresTitulares = await ObterProfessoresTitularesDisciplinasEol(codigoTurma);

            foreach (var d in disciplinasEol)
            {
                 var rfProfessor = professoresTitulares
                    .FirstOrDefault(pt => pt.DisciplinasId().Contains(d.CodigoComponenteCurricular))?.ProfessorRf;
                await PreencherInformacoesComponenteCurricularProfessor(codigoTurma, rfProfessor, d);
            }          
            return MapearDisciplinaResposta(disciplinasEol);
        }

        private async Task PreencherInformacoesComponenteCurricularProfessor(string codigoTurma, string rfProfessor, DisciplinaDto componenteCurricular)
        {
            componenteCurricular.Professor = rfProfessor;
            if (!string.IsNullOrWhiteSpace(componenteCurricular.Professor))
            {
                var componentesProfessor = await mediator.Send(new ObterComponentesCurricularesDoProfessorNaTurmaQuery(codigoTurma, componenteCurricular.Professor, Perfis.PERFIL_PROFESSOR));
                var componenteCorrepondente = componentesProfessor.FirstOrDefault(cp => cp.CodigoComponenteTerritorioSaber.Equals(componenteCurricular.CodigoComponenteCurricular));
                if (componenteCorrepondente.NaoEhNulo())
                {
                    componenteCurricular.CodigoComponenteCurricular = componenteCorrepondente.Codigo;
                    componenteCurricular.CodigoComponenteCurricularTerritorioSaber = componenteCorrepondente.CodigoComponenteTerritorioSaber;
                    componenteCurricular.Nome = componenteCorrepondente.Descricao;
                }
            }
        }

        private async Task<IEnumerable<ProfessorTitularDisciplinaEol>> ObterProfessoresTitularesDisciplinasEol(string codigoTurma)
        {
            if (string.IsNullOrEmpty(codigoTurma))
                return Enumerable.Empty<ProfessorTitularDisciplinaEol>();
            return await mediator.Send(new ObterProfessoresTitularesDisciplinasEolQuery(codigoTurma));
        }

        private async Task<IEnumerable<AtribuicaoCJ>> ObterAtribuicoesCJ(string codigoTurma, string codigoDre, string codigoUe, bool verificaPerfilGestao, string login)
        {
            if (verificaPerfilGestao)
                return await repositorioAtribuicaoCJ.ObterAtribuicaoCJPorDreUeTurmaRF(codigoTurma, codigoDre, codigoUe, string.Empty);
            return await repositorioAtribuicaoCJ.ObterPorFiltros(null, codigoTurma, string.Empty, 0, login, string.Empty, true);
        }

        public async Task<List<DisciplinaDto>> ObterDisciplinasPorProfessorETurma(string codigoTurma, bool turmaPrograma)
        {
            var disciplinasDto = new List<DisciplinaDto>();

            var login = await mediator.Send(ObterUsuarioLogadoQuery.Instance);
            var perfilAtual = await mediator.Send(ObterPerfilAtualQuery.Instance);
            var ehPefilCJ = perfilAtual == Perfis.PERFIL_CJ || perfilAtual == Perfis.PERFIL_CJ_INFANTIL;

            var chaveCache = string.Format(NomeChaveCache.COMPONENTES_TURMA_PROFESSOR_PERFIL, codigoTurma, login, perfilAtual);
            var disciplinasCacheString = await repositorioCache.ObterAsync(chaveCache);

            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(codigoTurma));

            if (turma.EhNulo())
                throw new NegocioException("Não foi possível encontrar a turma");

            bool ehEnsinoMedio = turma.ModalidadeCodigo == Modalidade.Medio;

            if (!string.IsNullOrWhiteSpace(disciplinasCacheString))
                return JsonConvert.DeserializeObject<List<DisciplinaDto>>(disciplinasCacheString);

            var disciplinas = ehPefilCJ ? await ObterDisciplinasPerfilCJ(codigoTurma, login.Login) :
                                          MapearDto(await mediator.Send(new ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQuery(codigoTurma, login.Login, perfilAtual)));

            if (disciplinas.EhNulo() || !disciplinas.Any())
                return disciplinasDto;

            disciplinasDto = MapearParaDto(disciplinas, ehEnsinoMedio, turmaPrograma);

            await repositorioCache.SalvarAsync(chaveCache, JsonConvert.SerializeObject(disciplinasDto));

            return disciplinasDto;
        }

        public async Task<List<DisciplinaDto>> ObterDisciplinasPorTurma(string codigoTurma, bool turmaPrograma)
        {
            var disciplinasDto = new List<DisciplinaDto>();
            var login = await mediator.Send(ObterUsuarioLogadoQuery.Instance);
            var perfilAtual = await mediator.Send(ObterPerfilAtualQuery.Instance);

            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(codigoTurma));
            if (turma.EhNulo())
                throw new NegocioException("Não foi possível encontrar a turma");

            bool ehEnsinoMedio = turma.ModalidadeCodigo == Modalidade.Medio;

            if (perfilAtual == Perfis.PERFIL_CJ || perfilAtual == Perfis.PERFIL_CJ_INFANTIL)
            {
                var disciplinasAtribuicaoCj = await ObterDisciplinasPerfilCJ(codigoTurma, login.Login);
                var atribuicoes = await repositorioAtribuicaoCJ.ObterPorFiltros(null, codigoTurma, string.Empty, 0, login.Login, string.Empty, true);
                if (atribuicoes.NaoEhNulo() && atribuicoes.Any())
                {
                    var disciplinasEol = await mediator.Send(new ObterComponentesCurricularesPorIdsQuery(atribuicoes.Select(a => a.DisciplinaId).Distinct().ToArray()));

                    disciplinasEol?.Where(disciplina => disciplina.TerritorioSaber)
                    .ToList()
                    .ForEach(disciplina =>
                    {
                        disciplina.Nome = disciplinasAtribuicaoCj.FirstOrDefault(d => d.CodigoComponenteCurricular == disciplina.CodigoComponenteCurricular || d.CodigoComponenteTerritorioSaber == disciplina.CodigoComponenteCurricular).Nome;
                    });

                    var disciplinasResposta = MapearDisciplinaResposta(disciplinasEol);
                    disciplinasDto = MapearParaDto(disciplinasResposta, ehEnsinoMedio, turmaPrograma);
                }
            }
            else
            {
                var chaveCache = string.Format(NomeChaveCache.COMPONENTES_TURMA, codigoTurma);
                var disciplinasCacheString = repositorioCache.Obter(chaveCache);
                if (!string.IsNullOrWhiteSpace(disciplinasCacheString))
                    return JsonConvert.DeserializeObject<List<DisciplinaDto>>(disciplinasCacheString);
                var disciplinasResposta = await mediator.Send(new ObterDisciplinasPorCodigoTurmaQuery(codigoTurma));
                if (disciplinasResposta.NaoEhNulo() && disciplinasResposta.Any())
                {
                    disciplinasDto = MapearParaDto(disciplinasResposta, ehEnsinoMedio, turmaPrograma);
                    await repositorioCache.SalvarAsync(chaveCache, JsonConvert.SerializeObject(disciplinasDto));
                }
            }
            return disciplinasDto;
        }

        public IEnumerable<DisciplinaResposta> MapearComponentes(IEnumerable<ComponenteCurricularEol> componentesCurriculares)
        {
            foreach (var componenteCurricular in componentesCurriculares)
                yield return new DisciplinaResposta()
                {
                    CodigoComponenteCurricularPai = componenteCurricular.CodigoComponenteCurricularPai,
                    CodigoComponenteCurricular = componenteCurricular.Codigo,
                    Nome = componenteCurricular.Descricao,
                    Regencia = componenteCurricular.Regencia,
                    TerritorioSaber = componenteCurricular.TerritorioSaber,
                    Compartilhada = componenteCurricular.Compartilhada,
                    LancaNota = componenteCurricular.LancaNota,

                };
        }

        private DisciplinaResposta MapearDisciplinaResposta(DisciplinaDto disciplinaEol) => new DisciplinaResposta()
        {
            CodigoComponenteCurricular = disciplinaEol.CodigoComponenteCurricular,
            CodigoComponenteCurricularPai = disciplinaEol.CdComponenteCurricularPai,
            CodigoComponenteTerritorioSaber = disciplinaEol.CodigoComponenteCurricularTerritorioSaber,
            Nome = disciplinaEol.Nome,
            Regencia = disciplinaEol.Regencia,
            Compartilhada = disciplinaEol.Compartilhada,
            RegistroFrequencia = disciplinaEol.RegistraFrequencia,
            LancaNota = disciplinaEol.LancaNota,
            NomeComponenteInfantil = disciplinaEol.NomeComponenteInfantil,
            Id = disciplinaEol.Id,
            TerritorioSaber = disciplinaEol.TerritorioSaber,
            Professor = disciplinaEol.Professor
        };

        private List<DisciplinaDto> MapearParaDto(IEnumerable<DisciplinaResposta> disciplinas, bool ehEnsinoMedio = false, bool turmaPrograma = false, bool ensinoEspecial = false)
        {
            var retorno = new List<DisciplinaDto>();

            if (disciplinas.NaoEhNulo())
            {
                foreach (var disciplina in disciplinas)
                    retorno.Add(MapearParaDto(disciplina, ehEnsinoMedio, turmaPrograma, ensinoEspecial));
            }
            return retorno;
        }

        private DisciplinaDto MapearParaDto(DisciplinaResposta disciplina, bool ehEnsinoMedio = false, bool turmaPrograma = false, bool ensinoEspecial = false) => new DisciplinaDto()
        {
            Id = (disciplina.Id > 0 ? disciplina.Id : disciplina.CodigoComponenteCurricular),
            CdComponenteCurricularPai = disciplina.CodigoComponenteCurricularPai,
            CodigoComponenteCurricular = disciplina.CodigoComponenteCurricular,
            CodigoComponenteCurricularTerritorioSaber = disciplina.CodigoComponenteTerritorioSaber ?? 0,
            Nome = disciplina.Nome,
            NomeComponenteInfantil = disciplina.NomeComponenteInfantil,
            Regencia = disciplina.Regencia,
            TerritorioSaber = disciplina.TerritorioSaber,
            Compartilhada = disciplina.Compartilhada,
            RegistraFrequencia = disciplina.RegistroFrequencia,
            LancaNota = disciplina.LancaNota,
            PossuiObjetivos = !turmaPrograma && !ehEnsinoMedio && consultasObjetivoAprendizagem.DisciplinaPossuiObjetivosDeAprendizagem(disciplina.CodigoComponenteCurricular),
            ObjetivosAprendizagemOpcionais = consultasObjetivoAprendizagem.ComponentePossuiObjetivosOpcionais(disciplina.CodigoComponenteCurricular, disciplina.Regencia, ensinoEspecial),
            Professor = disciplina.Professor
        };

        private IEnumerable<DisciplinaResposta> MapearDisciplinaResposta(IEnumerable<DisciplinaDto> disciplinasEol)
        {
            foreach (var disciplinaEol in disciplinasEol)
                yield return MapearDisciplinaResposta(disciplinaEol);
        }

        private async Task<IEnumerable<DisciplinaDto>> TratarRetornoDisciplinasPlanejamento(IEnumerable<DisciplinaDto> disciplinas, long codigoDisciplina, bool regencia, string codigoTurma = "", bool CJ = false)
        {
            if (codigoDisciplina == 0 && !regencia)
                return disciplinas;

            if (regencia)
            {
                if (!codigoTurma.Equals(""))
                {
                    var regencias = await mediator.Send(new ObterComponentesCurricularesRegenciaPorTurmaCodigoQuery(codigoTurma));
                    return CJ ? disciplinas.Where(x => regencias.Any(c => c.CodigoComponenteCurricular == x.CodigoComponenteCurricular))
                        : disciplinas.Where(x => x.Regencia && regencias.Any(c => c.CodigoComponenteCurricular == x.CodigoComponenteCurricular));
                }
                return CJ ? disciplinas : disciplinas.Where(x => x.Regencia);
            }

            return disciplinas.Where(x => x.CodigoComponenteCurricular == codigoDisciplina);
        }
    }
}