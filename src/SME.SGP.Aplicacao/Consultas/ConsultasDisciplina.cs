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
        private readonly IRepositorioComponenteCurricularConsulta repositorioComponenteCurricular;
        private readonly IServicoUsuario servicoUsuario;
        private string[] componentesParaObjetivosAprendizagemOpcionais = Array.Empty<string>();
        public ConsultasDisciplina(IRepositorioCache repositorioCache,
            IConsultasObjetivoAprendizagem consultasObjetivoAprendizagem,
            IServicoUsuario servicoUsuario,
            IRepositorioComponenteCurricularJurema repositorioComponenteCurricularJurema,
            IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ,
            IRepositorioComponenteCurricularConsulta repositorioComponenteCurricular,
            IMediator mediator) : base(mediator)
        {
            this.repositorioCache = repositorioCache ??
                throw new System.ArgumentNullException(nameof(repositorioCache));
            this.consultasObjetivoAprendizagem = consultasObjetivoAprendizagem ??
                throw new System.ArgumentNullException(nameof(consultasObjetivoAprendizagem));
            this.servicoUsuario = servicoUsuario ??
                throw new System.ArgumentNullException(nameof(servicoUsuario));
            this.repositorioAtribuicaoCJ = repositorioAtribuicaoCJ ??
                throw new System.ArgumentNullException(nameof(repositorioAtribuicaoCJ));
            this.repositorioComponenteCurricular = repositorioComponenteCurricular ??
                throw new System.ArgumentNullException(nameof(repositorioComponenteCurricular));
            this.repositorioComponenteCurricularJurema = repositorioComponenteCurricularJurema ??
                throw new System.ArgumentNullException(nameof(repositorioComponenteCurricularJurema));
        }

        public IEnumerable<DisciplinaDto> MapearParaDto(IEnumerable<DisciplinaResposta> disciplinas, bool ehEnsinoMedio = false)
        {
            foreach (var disciplina in disciplinas)
                yield return MapearParaDto(disciplina, ehEnsinoMedio);
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

            if (atribuicoes == null || !atribuicoes.Any())
                return null;

            var disciplinasEol = await repositorioComponenteCurricular.ObterDisciplinasPorIds(atribuicoes.Select(a => a.DisciplinaId).Distinct().ToArray());

            var componenteRegencia = disciplinasEol?.FirstOrDefault(c => c.Regencia);
            if (componenteRegencia == null || ignorarDeParaRegencia)
                return TransformarListaDisciplinaEolParaRetornoDto(disciplinasEol);

            var componentesRegencia = await repositorioComponenteCurricular.ObterDisciplinasPorIds(IDS_COMPONENTES_REGENCIA);
            if (componentesRegencia != null)
                return TransformarListaDisciplinaEolParaRetornoDto(componentesRegencia);

            return componentes;
        }

        public async Task<List<DisciplinaDto>> ObterComponentesCurricularesPorProfessorETurma(string codigoTurma, bool turmaPrograma, bool realizarAgrupamentoComponente = false)
        {
            List<DisciplinaDto> disciplinasDto;
            var disciplinasEol = new List<DisciplinaResposta>();

            var usuarioLogado = await servicoUsuario.ObterUsuarioLogado();

            var dataInicioNovoSGP = await mediator.Send(new ObterParametroSistemaPorTipoQuery(TipoParametroSistema.DataInicioSGP));

            var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(codigoTurma));
            if (turma == null)
                throw new NegocioException("Não foi possível encontrar a turma");

            bool ehEnsinoMedio = turma.ModalidadeCodigo == Modalidade.Medio;

            IEnumerable<DisciplinaResposta> disciplinasAtribuicaoCj;

            if (turma == null)
                throw new NegocioException("Não foi possível encontrar a turma");

            await CarregueComponentesObjetivoApredizagemOpcionais(turma.AnoLetivo);

            if (usuarioLogado.EhProfessorCj())
            {
                disciplinasAtribuicaoCj = (await ObterDisciplinasPerfilCJ(codigoTurma, usuarioLogado.Login)).ToList();

                var componentesCurricularesAtribuicaoEol = await mediator
                    .Send(new ObterComponentesCurricularesDoProfessorNaTurmaQuery(turma.CodigoTurma, usuarioLogado.Login, usuarioLogado.PerfilAtual));

                foreach (var componenteAtual in componentesCurricularesAtribuicaoEol)
                {

                    if (componenteAtual.TerritorioSaber)
                    {
                        // remove territórios replicados definidos pela atribuição cj com base nos códigos de agrupamento
                        var codigosTerritorioReplicados = componenteAtual.CodigosTerritoriosAgrupamento
                            .Intersect(disciplinasAtribuicaoCj.Where(d => d.TerritorioSaber).Select(d => d.CodigoComponenteCurricular));

                        disciplinasAtribuicaoCj = disciplinasAtribuicaoCj
                            .Except(disciplinasAtribuicaoCj.Where(d => d.TerritorioSaber && codigosTerritorioReplicados.Contains(d.CodigoComponenteCurricular)));
                    }

                    var registraFrequencia = await mediator
                        .Send(new ObterComponenteRegistraFrequenciaQuery(componenteAtual.Codigo, componenteAtual.TerritorioSaber ? componenteAtual.CodigoComponenteTerritorioSaber : componenteAtual.Codigo));

                    disciplinasAtribuicaoCj = disciplinasAtribuicaoCj.Append(new DisciplinaResposta()
                    {
                        CodigoComponenteCurricular = componenteAtual.Codigo,
                        Compartilhada = componenteAtual.Compartilhada,
                        CodigoComponenteCurricularPai = componenteAtual.CodigoComponenteCurricularPai,
                        CodigoComponenteTerritorioSaber = componenteAtual.CodigoComponenteTerritorioSaber,
                        NomeComponenteInfantil = turma.EhTurmaInfantil ? componenteAtual.DescricaoComponenteInfantil : null,
                        Nome = componenteAtual.Descricao,
                        Regencia = componenteAtual.Regencia,
                        RegistroFrequencia = registraFrequencia,
                        TerritorioSaber = componenteAtual.TerritorioSaber,
                        LancaNota = componenteAtual.LancaNota,
                        TurmaCodigo = componenteAtual.TurmaCodigo,
                        Professor = componenteAtual.Professor
                    });
                }

                var disciplinasEolTratadas = realizarAgrupamentoComponente ?
                    disciplinasAtribuicaoCj?.DistinctBy(s => (s.Nome.ToUpper(), s.TerritorioSaber ? s.Professor : null)).OrderBy(s => s.Nome) :
                    disciplinasAtribuicaoCj.OrderBy(s => s.Nome);

                disciplinasDto = MapearParaDto(disciplinasEolTratadas, ehEnsinoMedio, turmaPrograma, turma.EnsinoEspecial)?.OrderBy(c => c.Nome)?.ToList();
            }
            else
            {
                var componentesCurriculares = new List<ComponenteCurricularEol>();

                if (!usuarioLogado.TemPerfilGestaoUes())
                {
                    componentesCurriculares = (await mediator
                        .Send(new ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQuery(codigoTurma, usuarioLogado.Login, usuarioLogado.PerfilAtual, realizarAgrupamentoComponente))).ToList();

                    componentesCurriculares ??= (await mediator
                        .Send(new ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQuery(codigoTurma, usuarioLogado.Login, usuarioLogado.PerfilAtual, realizarAgrupamentoComponente, false))).ToList();

                    componentesCurriculares.ForEach(c =>
                    {
                        var codigoTerritorio = c.Codigo;
                        c.Codigo = c.TerritorioSaber ? c.CodigoComponenteTerritorioSaber : c.Codigo;
                        c.CodigoComponenteTerritorioSaber = c.TerritorioSaber ? codigoTerritorio : c.CodigoComponenteTerritorioSaber;
                    });
                }
                else
                {
                    componentesCurriculares = (await mediator
                        .Send(new ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQuery(codigoTurma, usuarioLogado.Login, usuarioLogado.PerfilAtual, realizarAgrupamentoComponente))).ToList();

                    if (!componentesCurriculares.Any())
                    {
                        var componentesCurricularesDaTurma = await mediator.Send(new ObterDisciplinasPorCodigoTurmaQuery(codigoTurma));

                        if (componentesCurricularesDaTurma.Any() && componentesCurricularesDaTurma != null)
                        {
                            componentesCurriculares = componentesCurricularesDaTurma.Select(c => new ComponenteCurricularEol()
                            {
                                Codigo = c.TerritorioSaber ? c.CodigoComponenteTerritorioSaber.Value : c.CodigoComponenteCurricular,
                                TerritorioSaber = c.TerritorioSaber,
                                CodigoComponenteTerritorioSaber = c.TerritorioSaber ? c.CodigoComponenteCurricular : 0,
                                Descricao = c.Nome,
                                GrupoMatriz = new Dominio.GrupoMatriz() { Id = c.GrupoMatriz.Id, Nome = c.GrupoMatriz.Nome },
                                TurmaCodigo = c.TurmaCodigo,
                                Regencia = c.Regencia
                            }).ToList();
                        }
                    }
                }

                var idsDisciplinas = componentesCurriculares?.Select(a => a.Codigo).ToArray();
                idsDisciplinas = idsDisciplinas.Concat(componentesCurriculares.Where(c => c.TerritorioSaber).Select(c => c.CodigoComponenteTerritorioSaber)).ToArray();

                if (usuarioLogado.TemPerfilAdmUE() || usuarioLogado.TemPerfilGestaoUes() && !usuarioLogado.EhCP())
                    idsDisciplinas = await ObterDisciplinasAtribuicaoCJParaTurma(codigoTurma, componentesCurriculares, idsDisciplinas);

                disciplinasDto = (await repositorioComponenteCurricular.ObterDisciplinasPorIds(idsDisciplinas))?.OrderBy(c => c.Nome)?.ToList();

                var componentesCurricularesJurema = await repositorioCache.ObterAsync(NomeChaveCache.COMPONENTES_JUREMA, () => Task.FromResult(repositorioComponenteCurricularJurema.Listar()));

                if (componentesCurricularesJurema == null)
                    throw new NegocioException("Não foi possível recuperar a lista de componentes curriculares.");

                disciplinasDto.ForEach(d =>
                {
                    var componenteEOL = componentesCurriculares.FirstOrDefault(a => a.Codigo == d.CodigoComponenteCurricular || a.CodigoComponenteTerritorioSaber == d.CodigoComponenteCurricular);

                    d.PossuiObjetivos = PossuiObjetivos(turma, Convert.ToInt32(dataInicioNovoSGP), componenteEOL, componentesCurricularesJurema);
                    d.CodigoComponenteCurricular = componenteEOL.Codigo;
                    d.CodigoTerritorioSaber = componenteEOL.CodigoComponenteTerritorioSaber;
                    d.Regencia = componenteEOL.Regencia;

                    if (d.TerritorioSaber)
                        d.Nome = componenteEOL.Descricao;

                    d.ObjetivosAprendizagemOpcionais = componentesParaObjetivosAprendizagemOpcionais.Contains(componenteEOL.Codigo.ToString()) || componenteEOL.PossuiObjetivosDeAprendizagemOpcionais(componentesCurricularesJurema, turma.EnsinoEspecial);
                    d.CdComponenteCurricularPai = componenteEOL.CodigoComponenteCurricularPai;
                    d.NomeComponenteInfantil = componenteEOL.ExibirComponenteEOL && !string.IsNullOrEmpty(d.NomeComponenteInfantil) ? d.NomeComponenteInfantil : d.Nome;
                    d.Professor = componenteEOL.Professor;
                });

                if (usuarioLogado.TemPerfilGestaoUes())
                {
                    disciplinasAtribuicaoCj = await ObterDisciplinasPerfilCJ(codigoTurma, usuarioLogado.Login, usuarioLogado.TemPerfilGestaoUes(), turma.Ue.Dre.CodigoDre, turma.Ue.CodigoUe);

                    // desconsidera atribuição cj de território atribuido no EOL
                    var codigosTerritorioAgrupamentos = componentesCurriculares
                        .Where(c => c.TerritorioSaber && c.CodigosTerritoriosAgrupamento != null && c.CodigosTerritoriosAgrupamento.Any())
                        .SelectMany(c => c.CodigosTerritoriosAgrupamento);

                    var codigosComponentesPai = disciplinasDto
                        .Where(d => d.CdComponenteCurricularPai.HasValue && d.CdComponenteCurricularPai.Value > 0)
                        .Select(d => d.CdComponenteCurricularPai.Value);

                    var disciplinasAdicionar = disciplinasAtribuicaoCj?
                        .Where(d => !codigosTerritorioAgrupamentos.Contains(d.CodigoComponenteCurricular) && (d.CodigoComponenteCurricularPai.HasValue && d.CodigoComponenteCurricularPai.Value > 0 && !codigosComponentesPai.Contains(d.CodigoComponenteCurricularPai.Value)));

                    if (disciplinasAdicionar != null && disciplinasAdicionar.Any())
                        disciplinasDto.AddRange(MapearParaDto(disciplinasAdicionar, turmaPrograma, turma.EnsinoEspecial));

                    disciplinasDto = disciplinasDto != null && disciplinasDto.Any() ? disciplinasDto.Where(d => d.TerritorioSaber).DistinctBy(d => (d.CodigoComponenteCurricular, d.Professor))
                        .Concat(disciplinasDto.Where(d => !d.TerritorioSaber).DistinctBy(d => d.CodigoComponenteCurricular))
                        .OrderBy(c => c.Nome).ToList() : null;
                }
            }

            //Exceção para disciplinas 1060 e 1061 que são compartilhadas entre EF e EJA
            if (turma.ModalidadeCodigo == Modalidade.EJA && disciplinasDto.Any())
            {
                var idComponenteInformaticaOie = 1060;

                var idComponenteLeituraOsl = 1061;

                foreach (var disciplina in disciplinasDto)
                {
                    disciplina.PossuiObjetivos = false;
                    if (disciplina.CodigoComponenteCurricular == idComponenteInformaticaOie || disciplina.CodigoComponenteCurricular == idComponenteLeituraOsl)
                        disciplina.RegistraFrequencia = false;
                }
            }

            if (turma.ModalidadeCodigo == Modalidade.Medio && turma.TipoTurno == (int)TipoTurnoEOL.Noite && disciplinasDto.Any())
            {
                var idComponenteSalaLeituraEM = 1347;
                var idComponenteTecAprendizagem = 1359;

                foreach (var disciplina in disciplinasDto)
                {
                    if (disciplina.CodigoComponenteCurricular == idComponenteSalaLeituraEM || disciplina.CodigoComponenteCurricular == idComponenteTecAprendizagem)
                        disciplina.RegistraFrequencia = false;
                }
            }

            if (disciplinasDto.Any(x => x.TerritorioSaber))
                await tratarDisciplinasTerritorioSaber(disciplinasDto.Where(x => x.TerritorioSaber), turma.CodigoTurma);

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
            const long PAP_RECUPERACAO_APRENDIZAGENS = 1322;
            const long PAP_PROJETO_COLABORATIVO = 1770;

            var componentesPAPs = new long[] { PAP_RECUPERACAO_APRENDIZAGENS, PAP_PROJETO_COLABORATIVO };

            return turma.AnoLetivo >= anoInicioSgp
                    && (turma.TipoTurma != TipoTurma.Programa || componentesPAPs.Contains(componenteEOL.Codigo))
                    && componenteEOL.PossuiObjetivosDeAprendizagem(componentesCurricularesJurema, turma.ModalidadeCodigo);
        }


        private async Task tratarDisciplinasTerritorioSaber(IEnumerable<DisciplinaDto> disciplinasDto, string codigoTurma)
        {
            foreach (var disciplina in disciplinasDto)
            {
                var componenteCurricularCorrespondente = await mediator.Send(new ObterCodigosComponentesCurricularesTerritorioSaberEquivalentesPorTurmaQuery(disciplina.CodigoComponenteCurricular, codigoTurma, string.Empty));
                disciplina.CodigoTerritorioSaber = long.Parse(componenteCurricularCorrespondente.FirstOrDefault().codigoComponente);
            }

        }

        private async Task<long[]> ObterDisciplinasAtribuicaoCJParaTurma(string codigoTurma, List<ComponenteCurricularEol> componentesCurriculares, long[] idsDisciplinas)
        {
            var atribuicoesCJTurma = await ObterDisciplinasPerfilCJ(codigoTurma, null);
            var codigosDisciplinasAtribuicao = atribuicoesCJTurma != null && atribuicoesCJTurma.Any() ?
                (from a in atribuicoesCJTurma
                 where !idsDisciplinas.Any(id => a.CodigoComponenteCurricular == id || a.CodigoComponenteTerritorioSaber == id)
                 select a.TerritorioSaber ? a.CodigoComponenteCurricular : a.CodigoComponenteTerritorioSaber)
                .Where(a => a.HasValue)
                .Select(a => a.Value)
                .Distinct()
                .ToArray() : new long[] { };

            if (codigosDisciplinasAtribuicao.Any())
            {
                idsDisciplinas = idsDisciplinas.Union(codigosDisciplinasAtribuicao).ToArray();
                var componentesInclusao = await mediator.Send(new ObterComponentesCurricularesEOLComSemAgrupamentoTurmaQuery(codigosDisciplinasAtribuicao, codigoTurma));

                componentesInclusao.ToList().ForEach(ci =>
                {
                    componentesCurriculares.Add(new ComponenteCurricularEol()
                    {
                        Codigo = ci.CodigoComponenteCurricular,
                        CodigoComponenteCurricularPai = ci.CdComponenteCurricularPai,
                        CodigoComponenteTerritorioSaber = ci.CodigoTerritorioSaber,
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
            var usuario = await servicoUsuario.ObterUsuarioLogado();
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
            if (componentesCurricularesJurema == null)
            {
                throw new NegocioException("Não foi possível recuperar a lista de componentes curriculares.");
            }

            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(codigoTurma));
            if (turma == null)
                throw new NegocioException("Não foi possível encontrar a turma");

            bool ehEnsinoMedio = turma.ModalidadeCodigo == Modalidade.Medio;

            if (usuario.EhProfessorCj())
            {
                var componentesCJ = await ObterComponentesCJ(null, codigoTurma,
                    string.Empty,
                    codigoDisciplina,
                    usuario.Login);
                disciplinasDto = MapearParaDto(componentesCJ, ehEnsinoMedio, turmaPrograma)?.OrderBy(c => c.Nome)?.ToList();
            }
            else
            {
                var componentesCurriculares = await mediator.Send(new ObterComponentesCurricularesPorCodigoTurmaLoginEPerfilParaPlanejamentoQuery(codigoTurma, usuario.Login, usuario.PerfilAtual));

                if (turma.ModalidadeCodigo == Modalidade.EJA)
                    componentesCurriculares = RemoverEdFisicaEJA(componentesCurriculares);

                disciplinasDto = (await repositorioComponenteCurricular.ObterDisciplinasPorIds(componentesCurriculares.Select(a => a.Codigo).ToArray()))?.OrderBy(c => c.Nome)?.ToList();

                disciplinasDto.ForEach(d =>
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
            var disciplinas = await repositorioComponenteCurricular.ObterDisciplinasPorIds(new long[] { disciplinaId });
            if (disciplinas == null || !disciplinas.Any())
                throw new NegocioException($"Componente curricular não localizado no SGP [{disciplinaId}]");

            return disciplinas.FirstOrDefault();
        }

        public async Task<List<DisciplinaDto>> ObterDisciplinasAgrupadasPorProfessorETurma(string codigoTurma, bool turmaPrograma)
        {
            var disciplinasDto = new List<DisciplinaDto>();

            var login = servicoUsuario.ObterLoginAtual();
            var perfilAtual = servicoUsuario.ObterPerfilAtual();

            var chaveCache = string.Format(NomeChaveCache.COMPONENTES_AGRUPADOS_TURMA_PROFESSOR_PERFIL, codigoTurma, login, perfilAtual);
            var disciplinasCacheString = repositorioCache.Obter(chaveCache);

            if (!string.IsNullOrWhiteSpace(disciplinasCacheString))
            {
                disciplinasDto = JsonConvert.DeserializeObject<List<DisciplinaDto>>(disciplinasCacheString);
            }
            else
            {
                if (perfilAtual == Perfis.PERFIL_CJ || perfilAtual == Perfis.PERFIL_CJ_INFANTIL)
                {
                    // Carrega Disciplinas da Atribuição do CJ
                    var atribuicoes = await repositorioAtribuicaoCJ.ObterPorFiltros(null, codigoTurma, string.Empty, 0, login, string.Empty, true);
                    if (atribuicoes != null && atribuicoes.Any())
                    {
                        var disciplinasEol = await repositorioComponenteCurricular.ObterDisciplinasPorIds(atribuicoes.Select(a => a.DisciplinaId).Distinct().ToArray());

                        foreach (var disciplinaEOL in disciplinasEol)
                        {
                            if (disciplinaEOL.CdComponenteCurricularPai > 0)
                            {
                                // TODO Consulta por disciplina pai não esta funcionando no EOL. Refatorar na proxima sprint
                                disciplinaEOL.CdComponenteCurricularPai = 11211124;
                                disciplinaEOL.Nome = "REG CLASSE INTEGRAL";
                            }
                            else
                                disciplinasDto.Add(disciplinaEOL);
                        }
                    }
                }
                else
                {
                    // Carrega disciplinas do professor
                    IEnumerable<DisciplinaResposta> disciplinas = MapearDto(await mediator.Send(new ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQuery(codigoTurma, login, perfilAtual)));

                    foreach (var disciplina in disciplinas)
                    {
                        if (disciplina.CodigoComponenteCurricularPai.HasValue)
                        {
                            // TODO Consulta por disciplina pai não esta funcionando no EOL. Refatorar na proxima sprint
                            disciplina.CodigoComponenteCurricular = 11211124;
                            disciplina.Nome = "REG CLASSE INTEGRAL";
                        }
                        var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(codigoTurma));
                        if (turma == null)
                            throw new NegocioException("Não foi possível encontrar a turma");

                        bool ehEnsinoMedio = turma.ModalidadeCodigo == Modalidade.Medio;

                        disciplinasDto.Add(MapearParaDto(disciplina, ehEnsinoMedio, true, turma.EnsinoEspecial));
                    }
                }

                if (disciplinasDto.Any())
                    await repositorioCache.SalvarAsync(chaveCache, JsonConvert.SerializeObject(disciplinasDto));
            }

            return disciplinasDto;
        }

        private IEnumerable<DisciplinaResposta> MapearDto(IEnumerable<ComponenteCurricularEol> componentesCurriculares)
        {
            if (componentesCurriculares == null || !componentesCurriculares.Any())
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
                GrupoMatriz = cc.GrupoMatriz != null ? new Integracoes.Respostas.GrupoMatriz() { Id = cc.GrupoMatriz.Id, Nome = cc.GrupoMatriz.Nome } : null,
                NomeComponenteInfantil = cc.DescricaoComponenteInfantil,
                Professor = cc.Professor,
                CodigosTerritoriosAgrupamento = cc.CodigosTerritoriosAgrupamento
            });
        }

        public async Task<IEnumerable<DisciplinaResposta>> ObterDisciplinasPerfilCJ(string codigoTurma, string login, bool verificaPerfilGestao = false, string codigoDre = null, string codigoUe = null)
        {
            var atribuicoes = verificaPerfilGestao ? await repositorioAtribuicaoCJ.ObterAtribuicaoCJPorDreUeTurmaRF(codigoTurma, codigoDre, codigoUe, string.Empty) :
               await repositorioAtribuicaoCJ.ObterPorFiltros(null, codigoTurma, string.Empty, 0, login, string.Empty, true);

            if (atribuicoes == null || !atribuicoes.Any())
                return null;

            var disciplinasEol = await repositorioComponenteCurricular
                .ObterDisciplinasPorIds(atribuicoes.Select(a => a.DisciplinaId).Distinct().ToArray());

            var professoresTitulares = !string.IsNullOrEmpty(codigoTurma) ? await mediator.Send(new ObterProfessoresTitularesDisciplinasEolQuery(codigoTurma)) : Enumerable.Empty<ProfessorTitularDisciplinaEol>();

            disciplinasEol.ToList().ForEach(d =>
            {
                d.Professor = professoresTitulares
                    .FirstOrDefault(pt => pt.DisciplinasId.Contains(d.CodigoComponenteCurricular))?.ProfessorRf;

                if (!string.IsNullOrWhiteSpace(d.Professor))
                {
                    var componentesProfessor = mediator.Send(new ObterComponentesCurricularesDoProfessorNaTurmaQuery(codigoTurma, d.Professor, Perfis.PERFIL_PROFESSOR)).Result;
                    var componenteCorrepondente = componentesProfessor.FirstOrDefault(cp => cp.CodigoComponenteTerritorioSaber.Equals(d.CodigoComponenteCurricular));
                    if (componenteCorrepondente != null)
                    {
                        d.CodigoComponenteCurricular = componenteCorrepondente.Codigo;
                        d.CodigoTerritorioSaber = componenteCorrepondente.CodigoComponenteTerritorioSaber;
                        d.Nome = componenteCorrepondente.Descricao;
                    }
                }
            });

            var contemComponenteTerritorioSemCodigo = disciplinasEol.Any(d => d.TerritorioSaber && d.CodigoTerritorioSaber == 0);
            if (contemComponenteTerritorioSemCodigo)
            {
                var componentesDaTurma = await mediator.Send(new ObterDisciplinasPorCodigoTurmaQuery(codigoTurma));

                foreach (var componente in disciplinasEol.Where(d => d.TerritorioSaber))
                {
                    var componenteCorrespondente = componentesDaTurma.FirstOrDefault(c => c.CodigoComponenteTerritorioSaber == componente.CodigoComponenteCurricular);

                    if (componenteCorrespondente != null)
                    {
                        componente.CodigoComponenteCurricular = componenteCorrespondente.CodigoComponenteTerritorioSaber.Value;
                        componente.CodigoTerritorioSaber = componenteCorrespondente.CodigoComponenteCurricular;
                        componente.Nome = componenteCorrespondente.Nome;
                    }
                }
            }

            return TransformarListaDisciplinaEolParaRetornoDto(disciplinasEol);
        }

        public async Task<List<DisciplinaDto>> ObterDisciplinasPorProfessorETurma(string codigoTurma, bool turmaPrograma)
        {
            var disciplinasDto = new List<DisciplinaDto>();

            var login = servicoUsuario.ObterLoginAtual();
            var perfilAtual = servicoUsuario.ObterPerfilAtual();
            var ehPefilCJ = perfilAtual == Perfis.PERFIL_CJ || perfilAtual == Perfis.PERFIL_CJ_INFANTIL;

            var chaveCache = string.Format(NomeChaveCache.COMPONENTES_TURMA_PROFESSOR_PERFIL, codigoTurma, login, perfilAtual);
            var disciplinasCacheString = await repositorioCache.ObterAsync(chaveCache);

            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(codigoTurma));

            if (turma == null)
                throw new NegocioException("Não foi possível encontrar a turma");

            bool ehEnsinoMedio = turma.ModalidadeCodigo == Modalidade.Medio;

            if (!string.IsNullOrWhiteSpace(disciplinasCacheString))
                return JsonConvert.DeserializeObject<List<DisciplinaDto>>(disciplinasCacheString);

            var disciplinas = ehPefilCJ ? await ObterDisciplinasPerfilCJ(codigoTurma, login) :
                                          MapearDto(await mediator.Send(new ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQuery(codigoTurma, login, perfilAtual)));

            if (disciplinas == null || !disciplinas.Any())
                return disciplinasDto;

            disciplinasDto = MapearParaDto(disciplinas, ehEnsinoMedio, turmaPrograma);

            await repositorioCache.SalvarAsync(chaveCache, JsonConvert.SerializeObject(disciplinasDto));

            return disciplinasDto;
        }

        public async Task<List<DisciplinaDto>> ObterDisciplinasPorTurma(string codigoTurma, bool turmaPrograma)
        {
            var disciplinasDto = new List<DisciplinaDto>();

            var login = servicoUsuario.ObterLoginAtual();
            var perfilAtual = servicoUsuario.ObterPerfilAtual();

            var chaveCache = string.Format(NomeChaveCache.COMPONENTES_TURMA, codigoTurma);
            var disciplinasCacheString = repositorioCache.Obter(chaveCache);

            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(codigoTurma));
            if (turma == null)
                throw new NegocioException("Não foi possível encontrar a turma");

            bool ehEnsinoMedio = turma.ModalidadeCodigo == Modalidade.Medio;

            if (!string.IsNullOrWhiteSpace(disciplinasCacheString))
            {
                disciplinasDto = JsonConvert.DeserializeObject<List<DisciplinaDto>>(disciplinasCacheString);
            }
            else
            {
                IEnumerable<DisciplinaResposta> disciplinas;

                if (perfilAtual == Perfis.PERFIL_CJ || perfilAtual == Perfis.PERFIL_CJ_INFANTIL)
                {
                    var disciplinasAtribuicaoCj = await ObterDisciplinasPerfilCJ(codigoTurma, login);
                    var atribuicoes = await repositorioAtribuicaoCJ.ObterPorFiltros(null, codigoTurma, string.Empty, 0, login, string.Empty, true);
                    if (atribuicoes != null && atribuicoes.Any())
                    {
                        var disciplinasEol = await repositorioComponenteCurricular.ObterDisciplinasPorIds(atribuicoes.Select(a => a.DisciplinaId).Distinct().ToArray());

                        disciplinasEol?.Where(disciplina => disciplina.TerritorioSaber)
                        .ToList()
                        .ForEach(disciplina =>
                        {
                            disciplina.Nome = disciplinasAtribuicaoCj.FirstOrDefault(d => d.CodigoComponenteCurricular == disciplina.CodigoComponenteCurricular || d.CodigoComponenteTerritorioSaber == disciplina.CodigoComponenteCurricular).Nome;
                        });

                        disciplinas = TransformarListaDisciplinaEolParaRetornoDto(disciplinasEol);
                    }
                    else disciplinas = null;
                }
                else
                    disciplinas = await mediator.Send(new ObterDisciplinasPorCodigoTurmaQuery(codigoTurma));

                if (disciplinas != null && disciplinas.Any())
                {
                    disciplinasDto = MapearParaDto(disciplinas, ehEnsinoMedio, turmaPrograma);

                    await repositorioCache.SalvarAsync(chaveCache, JsonConvert.SerializeObject(disciplinasDto));
                }
            }

            if (disciplinasDto.Any(x => x.TerritorioSaber))
                await tratarDisciplinasTerritorioSaber(disciplinasDto.Where(x => x.TerritorioSaber), turma.CodigoTurma);

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

        private IEnumerable<DisciplinaResposta> MapearComponentesComComponentesSgp(IEnumerable<ComponenteCurricularEol> componentesCurriculares, IEnumerable<DisciplinaDto> componentesSgp)
        {
            foreach (var componenteCurricular in componentesCurriculares)
            {
                var componenteSgp = componentesSgp.FirstOrDefault(c => c.CodigoComponenteCurricular == componenteCurricular.Codigo);
                yield return new DisciplinaResposta()
                {
                    CodigoComponenteCurricularPai = componenteCurricular.CodigoComponenteCurricularPai,
                    CodigoComponenteCurricular = componenteCurricular.Codigo,
                    Nome = componenteSgp != null ? componenteSgp.Nome : componenteCurricular.Descricao,
                    Regencia = componenteCurricular.Regencia,
                    TerritorioSaber = componenteCurricular.TerritorioSaber,
                    Compartilhada = componenteCurricular.Compartilhada,
                    LancaNota = componenteCurricular.LancaNota,
                };
            }
        }

        private DisciplinaResposta MapearDisciplinaResposta(DisciplinaDto disciplinaEol) => new DisciplinaResposta()
        {
            CodigoComponenteCurricular = disciplinaEol.CodigoComponenteCurricular,
            CodigoComponenteCurricularPai = disciplinaEol.CdComponenteCurricularPai,
            CodigoComponenteTerritorioSaber = disciplinaEol.CodigoTerritorioSaber,
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

            if (disciplinas != null)
            {
                foreach (var disciplina in disciplinas)
                    retorno.Add(MapearParaDto(disciplina, ehEnsinoMedio, turmaPrograma, ensinoEspecial));
            }
            return retorno;
        }

        private DisciplinaDto MapearParaDto(DisciplinaResposta disciplina, bool ehEnsinoMedio = false, bool turmaPrograma = false, bool ensinoEspecial = false) => new DisciplinaDto()
        {
            Id = disciplina.TerritorioSaber && disciplina.CodigoComponenteTerritorioSaber.HasValue && disciplina.CodigoComponenteTerritorioSaber.Value > 0 ? disciplina.CodigoComponenteTerritorioSaber.Value : (disciplina.Id > 0 ? disciplina.Id : disciplina.CodigoComponenteCurricular),
            CdComponenteCurricularPai = disciplina.CodigoComponenteCurricularPai,
            CodigoComponenteCurricular = disciplina.CodigoComponenteCurricular,
            CodigoTerritorioSaber = disciplina.CodigoComponenteTerritorioSaber ?? 0,
            Nome = disciplina.Nome,
            NomeComponenteInfantil = disciplina.NomeComponenteInfantil,
            Regencia = disciplina.Regencia,
            TerritorioSaber = disciplina.TerritorioSaber,
            Compartilhada = disciplina.Compartilhada,
            RegistraFrequencia = disciplina.RegistroFrequencia,
            LancaNota = disciplina.LancaNota,
            PossuiObjetivos = !turmaPrograma && !ehEnsinoMedio && consultasObjetivoAprendizagem.DisciplinaPossuiObjetivosDeAprendizagem(disciplina.CodigoComponenteCurricular),
            ObjetivosAprendizagemOpcionais = componentesParaObjetivosAprendizagemOpcionais.Contains(disciplina.CodigoComponenteCurricular.ToString()) || consultasObjetivoAprendizagem.ComponentePossuiObjetivosOpcionais(disciplina.CodigoComponenteCurricular, disciplina.Regencia, ensinoEspecial).Result,
            Professor = disciplina.Professor
        };

        private IEnumerable<DisciplinaResposta> TransformarListaDisciplinaEolParaRetornoDto(IEnumerable<DisciplinaDto> disciplinasEol)
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

        private async Task CarregueComponentesObjetivoApredizagemOpcionais(int anoLetivo)
        {
            var parametro = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.ComponentesParaObjetivosAprendizagemOpcionais, anoLetivo));

            if (parametro != null)
            {
                componentesParaObjetivosAprendizagemOpcionais = parametro.Valor.Split(",");
            }
        }
    }
}