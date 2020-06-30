using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasTurma: IConsultasTurma
    {
        private readonly IRepositorioTurma repositorioTurma;
        private readonly IConsultasTipoCalendario consultasTipoCalendario;
        private readonly IConsultasPeriodoFechamento consultasPeriodoFechamento;
        private readonly IConsultasPeriodoEscolar consultasPeriodoEscolar;
        private readonly IServicoEol servicoEOL;
        private readonly IServicoAluno servicoAluno;        

        public ConsultasTurma(IRepositorioTurma repositorioTurma,
                                IConsultasTipoCalendario consultasTipoCalendario,
                                IConsultasPeriodoFechamento consultasPeriodoFechamento,
                                IConsultasPeriodoEscolar consultasPeriodoEscolar,
                                IServicoEol servicoEOL,
                                IServicoAluno servicoAluno                                
            )
        {
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
            this.consultasTipoCalendario = consultasTipoCalendario ?? throw new ArgumentNullException(nameof(consultasTipoCalendario));
            this.consultasPeriodoFechamento = consultasPeriodoFechamento ?? throw new ArgumentNullException(nameof(consultasPeriodoFechamento));
            this.consultasPeriodoEscolar = consultasPeriodoEscolar ?? throw new ArgumentNullException(nameof(consultasPeriodoEscolar));
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
            this.servicoAluno = servicoAluno ?? throw new ArgumentNullException(nameof(servicoAluno));            
        }

        public async Task<bool> TurmaEmPeriodoAberto(string codigoTurma, DateTime dataReferencia, int bimestre = 0, TipoCalendario tipoCalendario = null)
        {
            var turma = await ObterComUeDrePorCodigo(codigoTurma);
            if (turma == null)
                throw new NegocioException($"Turma de código {codigoTurma} não localizada!");

            return await TurmaEmPeriodoAberto(turma, dataReferencia, bimestre, tipoCalendario: tipoCalendario);
        }

        public async Task<bool> TurmaEmPeriodoAberto(long turmaId, DateTime dataReferencia, int bimestre = 0, TipoCalendario tipoCalendario = null)
        {
            var turma = await ObterComUeDrePorId(turmaId);
            if (turma == null)
                throw new NegocioException($"Turma de ID {turmaId} não localizada!");

            return await TurmaEmPeriodoAberto(turma, dataReferencia, bimestre, tipoCalendario: tipoCalendario);
        }

        public async Task<bool> TurmaEmPeriodoAberto(Turma turma, DateTime dataReferencia, int bimestre = 0, bool ehAnoLetivo = false, TipoCalendario tipoCalendario = null)
        {
            if (tipoCalendario == null)
            {
                tipoCalendario = await consultasTipoCalendario.ObterPorTurma(turma);
                if (tipoCalendario == null)
                    throw new NegocioException($"Tipo de calendário para turma {turma.CodigoTurma} não localizado!");
            }

            var periodoEmAberto = await consultasTipoCalendario.PeriodoEmAberto(tipoCalendario, dataReferencia, bimestre, ehAnoLetivo);

            return periodoEmAberto || await consultasPeriodoFechamento.TurmaEmPeriodoDeFechamento(turma, tipoCalendario, dataReferencia, bimestre);
        }

        public async Task<Turma> ObterPorCodigo(string codigoTurma)
            => await repositorioTurma.ObterPorCodigo(codigoTurma);

        public async Task<Turma> ObterComUeDrePorCodigo(string codigoTurma)
            => await repositorioTurma.ObterTurmaComUeEDrePorCodigo(codigoTurma);

        public async Task<Turma> ObterComUeDrePorId(long turmaId)
            => await repositorioTurma.ObterTurmaComUeEDrePorId(turmaId);

        public async Task<IEnumerable<PeriodoEscolarAbertoDto>> PeriodosEmAbertoTurma(string turmaCodigo, DateTime dataReferencia, bool ehAnoLetivo = false)
        {
            var turma = await ObterComUeDrePorCodigo(turmaCodigo);
            if (turma == null)
                throw new NegocioException($"Turma de código {turmaCodigo} não localizada!");

            var tipoCalendario = await consultasTipoCalendario.ObterPorTurma(turma);
            var listaPeriodos = await consultasPeriodoEscolar.ObterPorTipoCalendario(tipoCalendario.Id);

            return await ObterPeriodosEmAberto(turma, dataReferencia, listaPeriodos.Periodos, ehAnoLetivo);
        }

        private async Task<IEnumerable<PeriodoEscolarAbertoDto>> ObterPeriodosEmAberto(Turma turma, DateTime dataReferencia, List<PeriodoEscolarDto> periodos, bool ehAnoLetivo = false)
        {
            var periodosAbertos = new List<PeriodoEscolarAbertoDto>();
            foreach (var periodo in periodos)
            {
                periodosAbertos.Add(new PeriodoEscolarAbertoDto()
                {
                    Bimestre = periodo.Bimestre,
                    Aberto = await TurmaEmPeriodoAberto(turma, dataReferencia, periodo.Bimestre, ehAnoLetivo)
                });
            }

            return periodosAbertos;
        }
        public async Task<TipoCalendarioSugestaoDto> ObterSugestaoTipoCalendarioPorTurma(string turmaCodigo)
        {
            var turma = await ObterComUeDrePorCodigo(turmaCodigo);
            if (turma == null)
                throw new NegocioException($"Turma de código {turmaCodigo} não localizada!");

            var tipoCalendario = await consultasTipoCalendario.ObterPorTurma(turma);
            if (tipoCalendario == null)
                throw new NegocioException($"Não foi possível obter o tipo de calendário da turma {turmaCodigo}");

            return new TipoCalendarioSugestaoDto() { Id = tipoCalendario.Id, Nome = tipoCalendario.Nome };
        }

        public async Task<IEnumerable<AlunoDadosBasicosDto>> ObterDadosAlunos(string turmaCodigo, int anoLetivo, PeriodoEscolar periodoEscolar = null)
        {
            var dadosAlunos = await servicoEOL.ObterAlunosPorTurma(turmaCodigo, anoLetivo);
            if (dadosAlunos == null || !dadosAlunos.Any())
                throw new NegocioException($"Não foram localizados dados dos alunos para turma {turmaCodigo} no EOL para o ano letivo {anoLetivo}");

            var dadosAlunosDto = new List<AlunoDadosBasicosDto>();

            foreach(var dadoAluno in dadosAlunos)
            {
                var dadosBasicos = (AlunoDadosBasicosDto)dadoAluno;
                // se informado periodo escolar carrega marcadores no periodo
                if (periodoEscolar != null)
                    dadosBasicos.Marcador = servicoAluno.ObterMarcadorAluno(dadoAluno, periodoEscolar);

                dadosAlunosDto.Add(dadosBasicos);
            }

            return dadosAlunosDto;
        }
    }
}
