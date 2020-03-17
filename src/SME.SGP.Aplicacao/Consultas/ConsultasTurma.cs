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
            var turma = await ObterPorCodigo(codigoTurma);
            var tipoCalendario = await consultasTipoCalendario.ObterPorTurma(turma, dataReferencia);
            var periodoEmAberto = await consultasTipoCalendario.PeriodoEmAberto(tipoCalendario, dataReferencia);

            return periodoEmAberto || await consultasFechamento.TurmaEmPeriodoDeFechamento(turma, tipoCalendario, dataReferencia);
        }

        public async Task<Turma> ObterPorCodigo(string codigoTurma)
            => repositorioTurma.ObterPorCodigo(codigoTurma);
    }
}
