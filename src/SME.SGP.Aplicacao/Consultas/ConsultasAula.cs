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
        private readonly IConsultasTurma consultasTurma;
        private readonly IConsultasPeriodoFechamento consultasPeriodoFechamento;
        private readonly IConsultasFrequencia consultasFrequencia;
        private readonly IConsultasPeriodoEscolar consultasPeriodoEscolar;
        private readonly IRepositorioTurma repositorioTurma;
        private readonly IRepositorioAula repositorio;
        private readonly IRepositorioPlanoAula repositorioPlanoAula;
        private readonly IServicoEOL servicoEol;
        private readonly IServicoUsuario servicoUsuario;

        public ConsultasAula(IRepositorioAula repositorio,
                             IConsultasPeriodoEscolar consultasPeriodoEscolar,
                             IConsultasFrequencia consultasFrequencia,
                             IRepositorioPlanoAula repositorioPlanoAula,
                             IRepositorioTurma repositorioTurma,
                             IServicoUsuario servicoUsuario,
                             IServicoEOL servicoEol,
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
            this.repositorioPlanoAula = repositorioPlanoAula ?? throw new ArgumentNullException(nameof(repositorioPlanoAula));
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
        }

        public async Task<AulaConsultaDto> BuscarPorId(long id)
        {
            var aula = repositorio.ObterPorId(id);

            if (aula == null)
                throw new NegocioException($"Aula de id {id} não encontrada");

            var aberto = await AulaDentroPeriodo(aula);

            var usuarioLogado = await servicoUsuario.ObterUsuarioLogado();

            string disciplinaId = await ObterDisciplinaIdAulaEOL(usuarioLogado, aula, usuarioLogado.EhProfessorCj());

            return MapearParaDto(aula, disciplinaId, aberto);
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

            var bimestreAtual = consultasPeriodoEscolar.ObterBimestre(DateTime.Now, turma.ModalidadeCodigo);
            var bimestreAula = consultasPeriodoEscolar.ObterBimestre(dataAula, turma.ModalidadeCodigo);

            var bimestreForaPeriodo = bimestreAtual == 0 || bimestreAula == 0;

            if (!bimestreForaPeriodo && bimestreAula >= bimestreAtual)
                return true;

            return await consultasPeriodoFechamento.TurmaEmPeriodoDeFechamentoAula(turma, DateTime.Now, bimestreAtual, bimestreAula);
        }

        public async Task<bool> ChecarFrequenciaPlanoAula(long aulaId)
        {
            var existeRegistro = await consultasFrequencia.FrequenciaAulaRegistrada(aulaId);
            if (!existeRegistro)
                existeRegistro = await repositorioPlanoAula.PlanoAulaRegistrado(aulaId);

            return existeRegistro;
        }

        public async Task<bool> ChecarFrequenciaPlanoNaRecorrencia(long aulaId)
        {
            var existeRegistro = await ChecarFrequenciaPlanoAula(aulaId);

            if (!existeRegistro)
            {
                var aulaAtual = repositorio.ObterPorId(aulaId);

                var aulasRecorrentes = await repositorio.ObterAulasRecorrencia(aulaAtual.AulaPaiId ?? aulaAtual.Id, aulaId);

                if (aulasRecorrentes != null)
                {
                    foreach (var aula in aulasRecorrentes)
                    {
                        existeRegistro = await ChecarFrequenciaPlanoAula(aula.Id);

                        if (existeRegistro)
                            break;
                    }
                }
            }

            return existeRegistro;
        }

        public async Task<AulaConsultaDto> ObterAulaDataTurmaDisciplina(DateTime data, string turmaId, string disciplinaId)
        {
            return await repositorio.ObterAulaDataTurmaDisciplina(data, turmaId, disciplinaId);
        }

        public async Task<IEnumerable<DataAulasProfessorDto>> ObterDatasDeAulasPorCalendarioTurmaEDisciplina(int anoLetivo, string turmaCodigo, string disciplina)
        {
            var usuarioLogado = await servicoUsuario.ObterUsuarioLogado();
            var usuarioRF = usuarioLogado.EhProfessor() ? usuarioLogado.CodigoRf : string.Empty;

            var turma = repositorioTurma.ObterPorCodigo(turmaCodigo);
            var periodosEscolares = await consultasPeriodoEscolar.ObterPeriodosEmAberto(turma.UeId, turma.ModalidadeCodigo, anoLetivo);

            return ObterAulasNosPeriodos(periodosEscolares, anoLetivo, turmaCodigo, disciplina, usuarioLogado, usuarioRF);
        }

        private IEnumerable<DataAulasProfessorDto> ObterAulasNosPeriodos(IEnumerable<PeriodoEscolarDto> periodosEscolares, int anoLetivo, string turmaCodigo, string disciplina, Usuario usuarioLogado, string usuarioRF)
        {
            foreach (var periodoEscolar in periodosEscolares.Distinct())
            {
                foreach (var aula in repositorio.ObterDatasDeAulasPorAnoTurmaEDisciplina(periodoEscolar.Id, anoLetivo, turmaCodigo, disciplina, usuarioRF))
                {
                    yield return new DataAulasProfessorDto
                    {
                        Data = aula.DataAula,
                        IdAula = aula.Id,
                        AulaCJ = aula.AulaCJ,
                        Bimestre = periodoEscolar.Bimestre
                    };
                }
            }
        }

        public async Task<int> ObterQuantidadeAulasRecorrentes(long aulaInicialId, RecorrenciaAula recorrencia)
        {
            var aulaInicioRecorrencia = repositorio.ObterPorId(aulaInicialId);
            var fimRecorrencia = consultasPeriodoEscolar.ObterFimPeriodoRecorrencia(aulaInicioRecorrencia.TipoCalendarioId, aulaInicioRecorrencia.DataAula, recorrencia);

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

        public async Task<int> ObterRecorrenciaDaSerie(long aulaId)
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

        private async Task<string> ObterDisciplinaIdAulaEOL(Usuario usuarioLogado, Aula aula, bool ehCJ)
        {
            IEnumerable<DisciplinaResposta> disciplinasUsuario;
            if (ehCJ)
                disciplinasUsuario = await consultasDisciplina.ObterComponentesCJ(null, aula.TurmaId, string.Empty, long.Parse(aula.DisciplinaId), usuarioLogado.CodigoRf);
            else
                disciplinasUsuario = await servicoEol.ObterDisciplinasPorCodigoTurmaLoginEPerfil(aula.TurmaId, usuarioLogado.CodigoRf, usuarioLogado.PerfilAtual);
            var disciplina = disciplinasUsuario?.FirstOrDefault(x => x.CodigoComponenteCurricular.ToString().Equals(aula.DisciplinaId));
            var disciplinaId = disciplina == null ? null : disciplina.CodigoComponenteCurricular.ToString();
            return disciplinaId;
        }
    }
}