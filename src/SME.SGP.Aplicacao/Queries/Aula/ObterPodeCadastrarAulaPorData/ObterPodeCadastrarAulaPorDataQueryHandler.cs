using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPodeCadastrarAulaPorDataQueryHandler : IRequestHandler<ObterPodeCadastrarAulaPorDataQuery, PodeCadastrarAulaPorDataRetornoDto>
    {
        private readonly IRepositorioEvento repositorioEvento;
        private readonly IRepositorioTipoCalendario repositorioTipoCalendario;
        private readonly IRepositorioPeriodoFechamento repositorioPeriodoFechamento;
        private readonly IRepositorioFechamentoReabertura repositorioFechamentoReabertura;

        public ObterPodeCadastrarAulaPorDataQueryHandler(IRepositorioEvento repositorioEvento, IRepositorioTipoCalendario repositorioTipoCalendario, 
            IRepositorioPeriodoFechamento repositorioPeriodoFechamento, IRepositorioFechamentoReabertura repositorioFechamentoReabertura)
        {
            this.repositorioEvento = repositorioEvento ?? throw new ArgumentNullException(nameof(repositorioEvento));
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new ArgumentNullException(nameof(repositorioTipoCalendario));
            this.repositorioPeriodoFechamento = repositorioPeriodoFechamento ?? throw new ArgumentNullException(nameof(repositorioPeriodoFechamento));
            this.repositorioFechamentoReabertura = repositorioFechamentoReabertura ?? throw new ArgumentNullException(nameof(repositorioFechamentoReabertura));
        }
        public async Task<PodeCadastrarAulaPorDataRetornoDto> Handle(ObterPodeCadastrarAulaPorDataQuery request, CancellationToken cancellationToken)
        {

            var hoje = DateTime.Today;

            var temEventoLetivoNoDia = !repositorioEvento.EhEventoLetivoPorTipoDeCalendarioDataDreUe(request.TipoCalendarioId, request.DataAula, request.DreCodigo, request.UeCodigo);

            if (temEventoLetivoNoDia)
                return new PodeCadastrarAulaPorDataRetornoDto(true);

            var periodoEscolar = await repositorioTipoCalendario.ObterPeriodoEscolarPorCalendarioEData(request.TipoCalendarioId, request.DataAula);

            if (request.DataAula.DayOfWeek == DayOfWeek.Sunday)
                return new PodeCadastrarAulaPorDataRetornoDto(false, "Não é possível cadastrar aula no domingo");

            else
            {
                var temEventoNaoLetivoNoDia = repositorioEvento.EhEventoNaoLetivoPorTipoDeCalendarioDataDreUe(request.TipoCalendarioId, request.DataAula, request.DreCodigo, request.UeCodigo);

                if (temEventoNaoLetivoNoDia)
                {
                    return new PodeCadastrarAulaPorDataRetornoDto(false, "Apenas é possível consultar este registro pois existe um evento de dia não letivo");
                }

                if (request.DataAula.Year == hoje.Year)
                {
                    if (request.DataAula <= hoje)
                    {
                        if (periodoEscolar != null)
                            return new PodeCadastrarAulaPorDataRetornoDto(true);
                        else
                        {

                            var periodoFechamento = await repositorioPeriodoFechamento.ObterPeriodoPorUeDataBimestreAsync(request.Turma.UeId, request.DataAula, request.Bimestre);

                            if (periodoFechamento != null)
                            {
                                if ( periodoFechamento.ExisteFechamentoEmAberto(hoje))
                                    return new PodeCadastrarAulaPorDataRetornoDto(true);
                            }
                            else
                            {
                                FechamentoReabertura periodoFechamentoReabertura = await ObterPeriodoFechamentoReabertura(request.TipoCalendarioId, request.Turma.UeId, hoje);
                                if (periodoFechamentoReabertura != null)
                                    return new PodeCadastrarAulaPorDataRetornoDto(true);
                                else return new PodeCadastrarAulaPorDataRetornoDto(false, "Apenas é possível consultar este registro pois o período deste bimestre não está aberto.");
                            }

                        }
                    }
                    else
                        return new PodeCadastrarAulaPorDataRetornoDto(true);

                }
                else
                {
                    FechamentoReabertura periodoFechamentoReabertura = await ObterPeriodoFechamentoReabertura(request.TipoCalendarioId, request.Turma.UeId, hoje);
                    if (periodoFechamentoReabertura != null)
                        return new PodeCadastrarAulaPorDataRetornoDto(true);
                    else return new PodeCadastrarAulaPorDataRetornoDto(false, "Apenas é possível consultar este registro pois o período deste bimestre não está aberto.");
                }
            }
            return new PodeCadastrarAulaPorDataRetornoDto(true, string.Empty);



        }

        private async Task<FechamentoReabertura> ObterPeriodoFechamentoReabertura(long tipoCalendarioId, long ueId, DateTime hoje)
        {
            return await repositorioFechamentoReabertura.ObterPorDataTurmaCalendarioAsync(ueId, hoje, tipoCalendarioId);
        }
    }
}
