using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasAula : IConsultasAula
    {
        private readonly IConsultasDisciplina consultasDisciplina;
        private readonly IConsultasPeriodoEscolar consultasPeriodoEscolar;
        private readonly IConsultasTipoCalendario consultasTipoCalendario;
        private readonly IConsultasTurma consultasTurma;
        private readonly IRepositorioAulaConsulta repositorioConsulta;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IMediator mediator;

        public ConsultasAula(IRepositorioAulaConsulta repositorioConsulta,
                             IConsultasPeriodoEscolar consultasPeriodoEscolar,
                             IConsultasTipoCalendario consultasTipoCalendario,
                             IServicoUsuario servicoUsuario,
                             IConsultasDisciplina consultasDisciplina,
                             IConsultasTurma consultasTurma,
                             IMediator mediator)
        {
            this.repositorioConsulta = repositorioConsulta ?? throw new ArgumentNullException(nameof(repositorioConsulta));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.consultasDisciplina = consultasDisciplina ?? throw new ArgumentNullException(nameof(consultasDisciplina));
            this.consultasTurma = consultasTurma ?? throw new ArgumentNullException(nameof(consultasTurma));
            this.consultasPeriodoEscolar = consultasPeriodoEscolar ?? throw new ArgumentNullException(nameof(consultasPeriodoEscolar));
            this.consultasTipoCalendario = consultasTipoCalendario ?? throw new ArgumentNullException(nameof(consultasTipoCalendario));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }               

        public async Task<bool> AulaDentroPeriodo(Aula aula)
        {
            return await AulaDentroPeriodo(aula.TurmaId, aula.DataAula);
        }

        public async Task<bool> AulaDentroPeriodo(string turmaId, DateTime dataAula)
        {
            var turma = await consultasTurma.ObterComUeDrePorCodigo(turmaId);

            if (turma.EhNulo())
                throw new NegocioException($"Não foi possivel obter a turma da aula");

            var bimestreAtual = await consultasPeriodoEscolar.ObterBimestre(DateTime.Now, turma.ModalidadeCodigo, turma.Semestre);
            var bimestreAula = await consultasPeriodoEscolar.ObterBimestre(dataAula, turma.ModalidadeCodigo, turma.Semestre);

            var bimestreForaPeriodo = bimestreAtual == 0 || bimestreAula == 0;

            if (!bimestreForaPeriodo && bimestreAula >= bimestreAtual)
                return true;

            return await mediator.Send(new ObterTurmaEmPeriodoDeFechamentoQuery(turma, DateTime.Now, bimestreAtual, bimestreAula));
        }

        public async Task<AulaConsultaDto> BuscarPorId(long id)
        {
            var aula = repositorioConsulta.ObterPorId(id);

            if (aula.EhNulo() || aula.Excluido)
                throw new NegocioException($"Aula de id {id} não encontrada");

            if (aula.AulaPaiId.HasValue)
                aula.AulaPai = await repositorioConsulta.ObterCompletoPorIdAsync(aula.AulaPaiId.Value);

            var aberto = await AulaDentroPeriodo(aula);

            var usuarioLogado = await servicoUsuario.ObterUsuarioLogado();

            string disciplinaId = await ObterDisciplinaIdAulaEOL(usuarioLogado, aula, usuarioLogado.EhProfessorCj());

            return MapearParaDto(aula, disciplinaId, aberto);
        }

        public async Task<AulaConsultaDto> ObterAulaDataTurmaDisciplina(DateTime data, string turmaId, string disciplinaId)
        {
            return await repositorioConsulta.ObterAulaDataTurmaDisciplina(data, turmaId, disciplinaId);
        }

        public async Task<IEnumerable<DataAulasProfessorDto>> ObterDatasDeAulasPorCalendarioTurmaEDisciplina(int anoLetivo, string turmaCodigo, string disciplinaCodigo)
        {
            var usuarioLogado = await servicoUsuario.ObterUsuarioLogado();
            var usuarioRF = usuarioLogado.EhProfessor() && !usuarioLogado.EhProfessorInfantil() ? usuarioLogado.CodigoRf : string.Empty;
            var componenteCurricular = await mediator.Send(new ObterComponenteCurricularPorIdQuery(long.Parse(disciplinaCodigo)));

            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(turmaCodigo));
            if (turma.EhNulo())
                throw new NegocioException("Turma não encontrada");

            var tipoCalendario = await consultasTipoCalendario.BuscarPorAnoLetivoEModalidade(anoLetivo, turma.ModalidadeTipoCalendario, turma.Semestre);
            if (tipoCalendario.EhNulo())
                throw new NegocioException("Tipo de calendário não existe para turma selecionada");

            var periodosEscolares = await consultasPeriodoEscolar.ObterPorTipoCalendario(tipoCalendario.Id);

            var componentesCurriculares = await mediator.Send(new ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQuery(turmaCodigo, usuarioLogado.Login, usuarioLogado.PerfilAtual, true));

            var disciplinaCJ = (await mediator.Send(new ObterComponentesCurricularesDoProfessorCJNaTurmaQuery(usuarioLogado.CodigoRf)))?.FirstOrDefault(x => x.TurmaId == turma.CodigoTurma && 
                                                                                                                                                            x.DisciplinaId.ToString() == disciplinaCodigo);
            if (componentesCurriculares.EhNulo())
                componentesCurriculares = await mediator.Send(new ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQuery(turmaCodigo, usuarioLogado.Login, usuarioLogado.PerfilAtual, true, false));

            var dadosDisciplina = componentesCurriculares
                                .Where(c => c.Codigo.ToString() == disciplinaCodigo ||
                                            ((componenteCurricular?.TerritorioSaber ?? false) &&
                                            c.CodigoComponenteTerritorioSaber == componenteCurricular.CodigoComponenteCurricularTerritorioSaber))
                                .Select(c => new ComponenteCurricularTipoDto()
                                {
                                    CodigoComponenteCurricular = c.Codigo.ToString(),
                                    CodigoComponenteCurricularTerritorio = c.CodigoComponenteTerritorioSaber.ToString(),
                                    EhTerritorio = c.TerritorioSaber
                                })
                                .FirstOrDefault();

            if(dadosDisciplina.EhNulo() && disciplinaCJ.NaoEhNulo())
                return await ObterAulasNosPeriodos(periodosEscolares, anoLetivo, turmaCodigo, disciplinaCJ.DisciplinaId.ToString(), usuarioLogado, usuarioRF);
            else
                return await ObterAulasNosPeriodos(periodosEscolares, anoLetivo, turmaCodigo, dadosDisciplina.CodigoComponenteCurricular, usuarioLogado, usuarioRF);
        }

        public async Task<int> ObterQuantidadeAulasRecorrentes(long aulaInicialId, RecorrenciaAula recorrencia)
        {
            var aulaInicioRecorrencia = repositorioConsulta.ObterPorId(aulaInicialId);
            var fimRecorrencia = await consultasPeriodoEscolar.ObterFimPeriodoRecorrencia(aulaInicioRecorrencia.TipoCalendarioId, aulaInicioRecorrencia.DataAula, recorrencia);

            var aulaIdOrigemRecorrencia = aulaInicioRecorrencia.AulaPaiId.NaoEhNulo() ? aulaInicioRecorrencia.AulaPaiId.Value
                                            : aulaInicialId;
            var aulasRecorrentes = await repositorioConsulta.ObterAulasRecorrencia(aulaIdOrigemRecorrencia, aulaInicioRecorrencia.Id, fimRecorrencia);
            return aulasRecorrentes.Count() + 1;
        }

        public async Task<int> ObterQuantidadeAulasTurmaDiaProfessor(string turma, string disciplina, DateTime dataAula, string codigoRf)
        {
            IEnumerable<AulasPorTurmaDisciplinaDto> aulas;

            if (ExperienciaPedagogica(disciplina))
                aulas = await repositorioConsulta.ObterAulasTurmaExperienciasPedagogicasDia(turma, dataAula);
            else
                aulas = await repositorioConsulta.ObterAulasTurmaDisciplinaDiaProfessor(turma, disciplina, dataAula, codigoRf);

            return aulas.Sum(a => a.Quantidade);
        }

        public async Task<int> ObterQuantidadeAulasTurmaSemanaProfessor(string turma, string disciplina, int semana, string codigoRf)
        {
            IEnumerable<AulasPorTurmaDisciplinaDto> aulas;

            if (ExperienciaPedagogica(disciplina))
                aulas = await repositorioConsulta.ObterAulasTurmaExperienciasPedagogicasSemana(turma, semana);
            else
                aulas = await repositorioConsulta.ObterAulasTurmaDisciplinaSemanaProfessor(turma, new string[] { disciplina }, semana, codigoRf);

            return aulas.Sum(a => a.Quantidade);
        }

        public int ObterRecorrenciaDaSerie(long aulaId)
        {
            var aula = repositorioConsulta.ObterPorId(aulaId);

            if (aula.EhNulo())
                throw new NegocioException("Aula não encontrada");

            // se não possui aula pai é a propria origem da recorrencia
            if (!aula.AulaPaiId.HasValue)
                return (int)aula.RecorrenciaAula;

            // Busca aula origem da recorrencia
            var aulaOrigemRecorrencia = repositorioConsulta.ObterPorId(aula.AulaPaiId.Value);

            // retorna o tipo de recorrencia da aula origem
            return (int)aulaOrigemRecorrencia.RecorrenciaAula;
        }

        private bool ExperienciaPedagogica(string disciplina)
            => new string[] { "1214", "1215", "1216", "1217", "1218", "1219", "1220", "1221", "1222", "1223" }
                .Contains(disciplina);

        private AulaConsultaDto MapearParaDto(Aula aula, string disciplinaId, bool aberto)
        {
            AulaConsultaDto dto = new AulaConsultaDto()
            {
                Id = aula.Id,
                DisciplinaId = aula.DisciplinaId,
                DisciplinaCompartilhadaId = aula.DisciplinaCompartilhadaId,
                TurmaId = aula.TurmaId,
                UeId = aula.UeId,
                TipoCalendarioId = aula.TipoCalendarioId,
                DentroPeriodo = aberto,
                TipoAula = aula.TipoAula,
                Quantidade = aula.Quantidade,
                ProfessorRf = aula.ProfessorRf,
                DataAula = aula.DataAula.Local(),
                RecorrenciaAula = aula.RecorrenciaAula,
                RecorrenciaAulaPai = aula.AulaPai?.RecorrenciaAula,
                AlteradoEm = aula.AlteradoEm,
                AlteradoPor = aula.AlteradoPor,
                AlteradoRF = aula.AlteradoRF,
                CriadoEm = aula.CriadoEm,
                CriadoPor = aula.CriadoPor,
                CriadoRF = aula.CriadoRF
            };

            dto.VerificarSomenteLeitura(disciplinaId);

            return dto;
        }

        private async Task<IEnumerable<DataAulasProfessorDto>> ObterAulasNosPeriodos(PeriodoEscolarListaDto periodosEscolares, int anoLetivo, string turmaCodigo, string disciplinaCodigo, Usuario usuarioLogado, string usuarioRF)
        {
            if (disciplinaCodigo.ToCharArray().Any(a => !char.IsDigit(a)))
                throw new NegocioException("Código do componente curricular inválido");

            var disciplina = await consultasDisciplina.ObterDisciplina(Convert.ToInt64(disciplinaCodigo));
            if (disciplina.EhNulo())
                throw new NegocioException("Componente curricular não encontrado");

            var aulasRetorno = new List<DataAulasProfessorDto>();

            var aulas = repositorioConsulta.ObterDatasDeAulasPorAnoTurmaEDisciplina(periodosEscolares.Periodos.Select(p => p.Id).Distinct(),
                                                                                    anoLetivo,
                                                                                    turmaCodigo,
                                                                                    disciplinaCodigo,
                                                                                    string.Empty,
                                                                                    null,
                                                                                    null,
                                                                                    usuarioLogado.EhProfessorCj());

            aulas.ToList().ForEach(aula =>
            {
                var bimestre = ObterBimestre(periodosEscolares.Periodos, aula);

                if (!disciplina.Regencia)
                    aulasRetorno.Add(MapearParaDto(aula, bimestre));

                var rfsOrnedadosPorDataCriacaoAula = aulas.OrderBy(a => a.CriadoEm)
                    .Select(a => a.ProfessorRf).Distinct();

                var ultimoRegente = rfsOrnedadosPorDataCriacaoAula.Last();

                // se regente atual, titular anterior ou professor anterior visualiza a aula
                if (ultimoRegente.Equals(usuarioRF, StringComparison.InvariantCultureIgnoreCase) ||
                    aula.ProfessorRf.Equals(usuarioRF, StringComparison.InvariantCultureIgnoreCase) ||
                    aula.Turma.EhTurmaInfantil ||
                    usuarioLogado.PerfilAtual != Perfis.PERFIL_PROFESSOR ||
                    usuarioLogado.PerfilAtual != Perfis.PERFIL_CJ ||
                    usuarioLogado.PerfilAtual != Perfis.PERFIL_CJ_INFANTIL
                    )
                    aulasRetorno.Add(MapearParaDto(aula, bimestre));
            });

            return aulasRetorno.OrderBy(a => a.Data);
        }

        private int ObterBimestre(List<PeriodoEscolarDto> periodos, Aula aula)
        {
            return periodos.FirstOrDefault(w=> w.PeriodoInicio <= aula.DataAula && w.PeriodoFim>= aula.DataAula).Bimestre;
        }

        private async Task<string> ObterDisciplinaIdAulaEOL(Usuario usuarioLogado, Aula aula, bool ehCJ)
        {
            IEnumerable<DisciplinaResposta> disciplinasUsuario = Enumerable.Empty<DisciplinaResposta>();

            if (ehCJ)
                disciplinasUsuario = await consultasDisciplina.ObterComponentesCJ(null, aula.TurmaId, string.Empty, long.Parse(aula.DisciplinaId), usuarioLogado.CodigoRf, ignorarDeParaRegencia: true);
            else
            {
                var componentesEOL = await mediator.Send(new ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQuery(aula.TurmaId, usuarioLogado.CodigoRf, usuarioLogado.PerfilAtual, false));
                disciplinasUsuario = consultasDisciplina.MapearComponentes(componentesEOL);
            }

            var disciplina = disciplinasUsuario?.FirstOrDefault(x => x.CodigoComponenteCurricular.ToString().Equals(aula.DisciplinaId));
            var disciplinaId = disciplina.EhNulo() ? null : disciplina.CodigoComponenteCurricular.ToString();
            return disciplinaId;
        }

        private DataAulasProfessorDto MapearParaDto(Aula aula, int bimestre)
        {
            return new DataAulasProfessorDto
            {
                Data = aula.DataAula,
                IdAula = aula.Id,
                AulaCJ = aula.AulaCJ,
                Bimestre = bimestre
            };
        }
    }
}