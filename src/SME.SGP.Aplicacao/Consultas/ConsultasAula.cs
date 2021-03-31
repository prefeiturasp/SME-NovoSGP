﻿using SME.SGP.Aplicacao.Integracoes;
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
        private readonly IConsultasFrequencia consultasFrequencia;
        private readonly IConsultasPeriodoEscolar consultasPeriodoEscolar;
        private readonly IConsultasPeriodoFechamento consultasPeriodoFechamento;
        private readonly IConsultasTipoCalendario consultasTipoCalendario;
        private readonly IConsultasTurma consultasTurma;
        private readonly IRepositorioAula repositorio;
        private readonly IRepositorioPlanoAula repositorioPlanoAula;
        private readonly IRepositorioTurma repositorioTurma;
        private readonly IServicoEol servicoEol;
        private readonly IServicoUsuario servicoUsuario;

        public ConsultasAula(IRepositorioAula repositorio,
                             IConsultasPeriodoEscolar consultasPeriodoEscolar,
                             IConsultasFrequencia consultasFrequencia,
                             IConsultasTipoCalendario consultasTipoCalendario,
                             IRepositorioPlanoAula repositorioPlanoAula,
                             IRepositorioTurma repositorioTurma,
                             IServicoUsuario servicoUsuario,
                             IServicoEol servicoEol,
                             IConsultasDisciplina consultasDisciplina,
                             IConsultasTurma consultasTurma,
                             IConsultasPeriodoFechamento consultasPeriodoFechamento)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.servicoEol = servicoEol ?? throw new ArgumentNullException(nameof(servicoEol));
            this.consultasDisciplina = consultasDisciplina ?? throw new ArgumentNullException(nameof(consultasDisciplina));
            this.consultasTurma = consultasTurma ?? throw new ArgumentNullException(nameof(consultasTurma));
            this.consultasPeriodoFechamento = consultasPeriodoFechamento ?? throw new ArgumentNullException(nameof(consultasPeriodoFechamento));
            this.consultasPeriodoEscolar = consultasPeriodoEscolar ?? throw new ArgumentNullException(nameof(consultasPeriodoEscolar));
            this.consultasFrequencia = consultasFrequencia ?? throw new ArgumentNullException(nameof(consultasFrequencia));
            this.consultasTipoCalendario = consultasTipoCalendario ?? throw new ArgumentNullException(nameof(consultasTipoCalendario));
            this.repositorioPlanoAula = repositorioPlanoAula ?? throw new ArgumentNullException(nameof(repositorioPlanoAula));
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
        }

        public async Task<bool> AulaDentroPeriodo(Aula aula)
        {
            return await AulaDentroPeriodo(aula.TurmaId, aula.DataAula);
        }

        public async Task<bool> AulaDentroPeriodo(string turmaId, DateTime dataAula)
        {
            var turma = await consultasTurma.ObterComUeDrePorCodigo(turmaId);

            if (turma == null)
                throw new NegocioException($"Não foi possivel obter a turma da aula");

            var bimestreAtual = await consultasPeriodoEscolar.ObterBimestre(DateTime.Now, turma.ModalidadeCodigo, turma.Semestre);
            var bimestreAula = await consultasPeriodoEscolar.ObterBimestre(dataAula, turma.ModalidadeCodigo, turma.Semestre);

            var bimestreForaPeriodo = bimestreAtual == 0 || bimestreAula == 0;

            if (!bimestreForaPeriodo && bimestreAula >= bimestreAtual)
                return true;

            return await consultasPeriodoFechamento.TurmaEmPeriodoDeFechamentoAula(turma, DateTime.Now, bimestreAtual, bimestreAula);
        }

        public async Task<AulaConsultaDto> BuscarPorId(long id)
        {
            var aula = repositorio.ObterPorId(id);

            if (aula == null || aula.Excluido)
                throw new NegocioException($"Aula de id {id} não encontrada");

            if (aula.AulaPaiId.HasValue)
                aula.AulaPai = await repositorio.ObterCompletoPorIdAsync(aula.AulaPaiId.Value);

            var aberto = await AulaDentroPeriodo(aula);

            var usuarioLogado = await servicoUsuario.ObterUsuarioLogado();

            string disciplinaId = await ObterDisciplinaIdAulaEOL(usuarioLogado, aula, usuarioLogado.EhProfessorCj());

            return MapearParaDto(aula, disciplinaId, aberto);
        }

        public async Task<AulaConsultaDto> ObterAulaDataTurmaDisciplina(DateTime data, string turmaId, string disciplinaId)
        {
            return await repositorio.ObterAulaDataTurmaDisciplina(data, turmaId, disciplinaId);
        }

        public async Task<IEnumerable<DataAulasProfessorDto>> ObterDatasDeAulasPorCalendarioTurmaEDisciplina(int anoLetivo, string turmaCodigo, string disciplinaCodigo)
        {
            var usuarioLogado = await servicoUsuario.ObterUsuarioLogado();
            var usuarioRF = usuarioLogado.EhProfessor() && !usuarioLogado.EhProfessorInfantil() ? usuarioLogado.CodigoRf : string.Empty;

            var turma = await repositorioTurma.ObterPorCodigo(turmaCodigo);
            if (turma == null)
                throw new NegocioException("Turma não encontrada");

            var tipoCalendario = await consultasTipoCalendario.BuscarPorAnoLetivoEModalidade(anoLetivo, turma.ModalidadeTipoCalendario, turma.Semestre);
            if (tipoCalendario == null)
                throw new NegocioException("Tipo de calendário não existe para turma selecionada");

            var periodosEscolares = await consultasPeriodoEscolar.ObterPorTipoCalendario(tipoCalendario.Id);

            return await ObterAulasNosPeriodos(periodosEscolares, anoLetivo, turmaCodigo, disciplinaCodigo, usuarioLogado, usuarioRF);
        }

        public async Task<int> ObterQuantidadeAulasRecorrentes(long aulaInicialId, RecorrenciaAula recorrencia)
        {
            var aulaInicioRecorrencia = repositorio.ObterPorId(aulaInicialId);
            var fimRecorrencia = await consultasPeriodoEscolar.ObterFimPeriodoRecorrencia(aulaInicioRecorrencia.TipoCalendarioId, aulaInicioRecorrencia.DataAula, recorrencia);

            var aulaIdOrigemRecorrencia = aulaInicioRecorrencia.AulaPaiId != null ? aulaInicioRecorrencia.AulaPaiId.Value
                                            : aulaInicialId;
            var aulasRecorrentes = await repositorio.ObterAulasRecorrencia(aulaIdOrigemRecorrencia, aulaInicioRecorrencia.Id, fimRecorrencia);
            return aulasRecorrentes.Count() + 1;
        }

        public async Task<int> ObterQuantidadeAulasTurmaDiaProfessor(string turma, string disciplina, DateTime dataAula, string codigoRf)
        {
            IEnumerable<AulasPorTurmaDisciplinaDto> aulas;

            if (ExperienciaPedagogica(disciplina))
                aulas = await repositorio.ObterAulasTurmaExperienciasPedagogicasDia(turma, dataAula);
            else
                aulas = await repositorio.ObterAulasTurmaDisciplinaDiaProfessor(turma, disciplina, dataAula, codigoRf);

            return aulas.Sum(a => a.Quantidade);
        }

        public async Task<int> ObterQuantidadeAulasTurmaSemanaProfessor(string turma, string disciplina, int semana, string codigoRf)
        {
            IEnumerable<AulasPorTurmaDisciplinaDto> aulas;

            if (ExperienciaPedagogica(disciplina))
                aulas = await repositorio.ObterAulasTurmaExperienciasPedagogicasSemana(turma, semana);
            else
                aulas = await repositorio.ObterAulasTurmaDisciplinaSemanaProfessor(turma, disciplina, semana, codigoRf);

            return aulas.Sum(a => a.Quantidade);
        }

        public int ObterRecorrenciaDaSerie(long aulaId)
        {
            var aula = repositorio.ObterPorId(aulaId);

            if (aula == null)
                throw new NegocioException("Aula não encontrada");

            // se não possui aula pai é a propria origem da recorrencia
            if (!aula.AulaPaiId.HasValue)
                return (int)aula.RecorrenciaAula;

            // Busca aula origem da recorrencia
            var aulaOrigemRecorrencia = repositorio.ObterPorId(aula.AulaPaiId.Value);

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
            if (disciplina == null)
                throw new NegocioException("Componente curricular não encontrado");

            var aulasRetorno = new List<DataAulasProfessorDto>();

            periodosEscolares.Periodos.ForEach(p =>
            {
                var aulas = repositorio
                    .ObterDatasDeAulasPorAnoTurmaEDisciplina(p.Id, anoLetivo, turmaCodigo, disciplinaCodigo, disciplina.Regencia ? string.Empty : usuarioRF, usuarioLogado.EhProfessorCj(), usuarioLogado.EhProfessor() || usuarioLogado.EhProfessorCj());

                aulas.ToList().ForEach(aula =>
                {
                    if (!disciplina.Regencia)
                        aulasRetorno.Add(MapearParaDto(aula, p.Bimestre));

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
                        aulasRetorno.Add(MapearParaDto(aula, p.Bimestre));
                });
            });            

            return aulasRetorno.OrderBy(a => a.Data);
        }

        private async Task<string> ObterDisciplinaIdAulaEOL(Usuario usuarioLogado, Aula aula, bool ehCJ)
        {
            IEnumerable<DisciplinaResposta> disciplinasUsuario = Enumerable.Empty<DisciplinaResposta>();

            if (ehCJ)
                disciplinasUsuario = await consultasDisciplina.ObterComponentesCJ(null, aula.TurmaId, string.Empty, long.Parse(aula.DisciplinaId), usuarioLogado.CodigoRf, ignorarDeParaRegencia: true);
            else
            {
                var componentesEOL = await servicoEol.ObterComponentesCurricularesPorCodigoTurmaLoginEPerfil(aula.TurmaId, usuarioLogado.CodigoRf, usuarioLogado.PerfilAtual);
                disciplinasUsuario = consultasDisciplina.MapearComponentes(componentesEOL);
            }

            var disciplina = disciplinasUsuario?.FirstOrDefault(x => x.CodigoComponenteCurricular.ToString().Equals(aula.DisciplinaId));
            var disciplinaId = disciplina == null ? null : disciplina.CodigoComponenteCurricular.ToString();
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