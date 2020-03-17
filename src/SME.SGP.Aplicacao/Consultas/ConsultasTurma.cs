using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasTurma: IConsultasTurma
    {
        private readonly IRepositorioTurma repositorioTurma;
        private readonly IConsultasTipoCalendario consultasTipoCalendario;
        private readonly IConsultasFechamento consultasFechamento;

        public ConsultasTurma(IRepositorioTurma repositorioTurma,
                                IConsultasTipoCalendario consultasTipoCalendario,
                                IConsultasFechamento consultasFechamento)
        {
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
            this.consultasTipoCalendario = consultasTipoCalendario ?? throw new ArgumentNullException(nameof(consultasTipoCalendario));
            this.consultasFechamento = consultasFechamento ?? throw new ArgumentNullException(nameof(consultasFechamento));
        }

        public async Task<bool> TurmaEmPeriodoAberto(string codigoTurma, DateTime dataReferencia)
        {
            var turma = await ObterComUeDrePorCodigo(codigoTurma);
            if (turma == null)
                throw new NegocioException($"Turma de código {codigoTurma} não localizada!");

            return await TurmaEmPeriodoAberto(turma, dataReferencia);
        }

        public async Task<bool> TurmaEmPeriodoAberto(long turmaId, DateTime dataReferencia)
        {
            var turma = await ObterComUeDrePorId(turmaId);
            if (turma == null)
                throw new NegocioException($"Turma de ID {turmaId} não localizada!");

            return await TurmaEmPeriodoAberto(turma, dataReferencia);
        }

        public async Task<bool> TurmaEmPeriodoAberto(Turma turma, DateTime dataReferencia)
        {
            var tipoCalendario = await consultasTipoCalendario.ObterPorTurma(turma, dataReferencia);
            if (tipoCalendario == null)
                throw new NegocioException($"Tipo de calendário para turma {turma.CodigoTurma} não localizado!");

            var periodoEmAberto = await consultasTipoCalendario.PeriodoEmAberto(tipoCalendario, dataReferencia);

            return periodoEmAberto || await consultasFechamento.TurmaEmPeriodoDeFechamento(turma, tipoCalendario, dataReferencia);
        }

        public async Task<Turma> ObterPorCodigo(string codigoTurma)
            => repositorioTurma.ObterPorCodigo(codigoTurma);

        public async Task<Turma> ObterComUeDrePorCodigo(string codigoTurma)
            => repositorioTurma.ObterTurmaComUeEDrePorCodigo(codigoTurma);

        public async Task<Turma> ObterComUeDrePorId(long turmaId)
            => repositorioTurma.ObterTurmaComUeEDrePorId(turmaId);
    }
}
