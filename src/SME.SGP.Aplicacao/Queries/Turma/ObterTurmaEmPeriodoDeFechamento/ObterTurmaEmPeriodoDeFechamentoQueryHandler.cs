using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmaEmPeriodoDeFechamentoQueryHandler : IRequestHandler<ObterTurmaEmPeriodoDeFechamentoQuery, bool>
    {
        private readonly IRepositorioFechamentoReabertura repositorioFechamentoReabertura;
        private readonly IConsultasTipoCalendario consultasTipoCalendario;
        private readonly IRepositorioEventoFechamentoConsulta repositorioEventoFechamento;

        public ObterTurmaEmPeriodoDeFechamentoQueryHandler(IRepositorioFechamentoReabertura repositorioFechamentoReabertura,
            IConsultasTipoCalendario consultasTipoCalendario,IRepositorioEventoFechamentoConsulta repositorioEventoFechamento)
        {
            this.repositorioFechamentoReabertura = repositorioFechamentoReabertura ?? throw new ArgumentNullException(nameof(repositorioFechamentoReabertura));
            this.consultasTipoCalendario = consultasTipoCalendario ?? throw new System.ArgumentNullException(nameof(consultasTipoCalendario));
            this.repositorioEventoFechamento = repositorioEventoFechamento ?? throw new System.ArgumentNullException(nameof(repositorioEventoFechamento));
        }

        public async Task<bool> Handle(ObterTurmaEmPeriodoDeFechamentoQuery request, CancellationToken cancellationToken)
        {
            var tipoCalendario = await consultasTipoCalendario.ObterPorTurma(request.Turma);

            var ueEmFechamento = await UeEmFechamento(tipoCalendario, request.Turma.EhTurmaInfantil, request.Bimestre, request.DataReferencia);

            return ueEmFechamento || await UeEmReaberturaDeFechamento(tipoCalendario, request.Turma.Ue.CodigoUe, request.Turma.Ue.Dre.CodigoDre, request.BimestreAlteracao, request.DataReferencia);
        }
        
        private async Task<bool> UeEmReaberturaDeFechamento(TipoCalendario tipoCalendario, string ueCodigo, string dreCodigo, int bimestre, DateTime dataReferencia)
        {
            var reaberturaPeriodo = await UeEmReaberturaDeFechamentoVigente(tipoCalendario, ueCodigo, dreCodigo, bimestre, dataReferencia);
            return reaberturaPeriodo != null;
        }
        
        private async Task<bool> UeEmFechamento(TipoCalendario tipoCalendario, bool modalidadeEhInfantil, int bimestre, DateTime dataReferencia)
        {
            return await repositorioEventoFechamento.UeEmFechamento(dataReferencia, tipoCalendario.Id, modalidadeEhInfantil, bimestre);
        }
        
        private async Task<FechamentoReabertura> UeEmReaberturaDeFechamentoVigente(TipoCalendario tipoCalendario, string ueCodigo, string dreCodigo, int bimestre, DateTime dataReferencia)
        {
            return await repositorioFechamentoReabertura.ObterReaberturaFechamentoBimestrePorDataReferencia(
                bimestre,
                dataReferencia,
                tipoCalendario.Id,
                dreCodigo,
                ueCodigo);
        }
    }
}
