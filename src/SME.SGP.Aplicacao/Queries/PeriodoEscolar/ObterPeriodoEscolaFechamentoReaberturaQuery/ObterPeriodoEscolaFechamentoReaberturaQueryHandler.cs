using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries
{
    public class ObterPeriodoEscolaFechamentoReaberturaQueryHandler : IRequestHandler<ObterPeriodoEscolaFechamentoReaberturaQuery, (Dominio.PeriodoEscolar periodoEscolar, PeriodoDto periodoFechamento, Dominio.Aplicacao aplicacao)>
    {
        private readonly IRepositorioEvento repositorioEvento;
        private readonly IRepositorioEventoTipo repositorioEventoTipo;
        private readonly IServicoPeriodoFechamento servicoPeriodoFechamento;
        private readonly IRepositorioFechamentoReabertura repositorioFechamentoReabertura;
        private readonly IRepositorioPeriodoEscolarConsulta repositorioPeriodoEscolar;

        public ObterPeriodoEscolaFechamentoReaberturaQueryHandler(IRepositorioEvento repositorioEvento, IRepositorioEventoTipo repositorioEventoTipo, IServicoPeriodoFechamento servicoPeriodoFechamento, IRepositorioFechamentoReabertura repositorioFechamentoReabertura, IRepositorioPeriodoEscolarConsulta repositorioPeriodoEscolar)
        {
            this.repositorioEvento = repositorioEvento;
            this.repositorioEventoTipo = repositorioEventoTipo;
            this.servicoPeriodoFechamento = servicoPeriodoFechamento;
            this.repositorioFechamentoReabertura = repositorioFechamentoReabertura;
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar;
        }

        public async Task<(Dominio.PeriodoEscolar periodoEscolar, PeriodoDto periodoFechamento, Dominio.Aplicacao aplicacao)> Handle(ObterPeriodoEscolaFechamentoReaberturaQuery request, CancellationToken cancellationToken)
        {
            var periodoFechamento = await servicoPeriodoFechamento.ObterPorTipoCalendarioSme(request.TipoCalendarioId, request.Aplicacao);
            var periodoFechamentoBimestre = periodoFechamento?.FechamentosBimestres.FirstOrDefault(x => x.Bimestre == request.Bimestre);

            if (periodoFechamento.EhNulo() || periodoFechamentoBimestre.EhNulo())
            {
                var hoje = DateTime.Today;
                var tipodeEventoReabertura = ObterTipoEventoFechamentoBimestre();

                if (await repositorioEvento.TemEventoNosDiasETipo(hoje, hoje, (TipoEvento)tipodeEventoReabertura.Codigo, request.TipoCalendarioId, request.Ue.CodigoUe, request.Ue.Dre.CodigoDre))
                {
                    var fechamentoReabertura = await repositorioFechamentoReabertura.ObterReaberturaFechamentoBimestrePorDataReferencia(request.Bimestre, hoje, request.TipoCalendarioId, request.Ue.Dre.CodigoDre, request.Ue.CodigoUe);
                    if (fechamentoReabertura.EhNulo())
                        throw new NegocioException($"Não localizado período de fechamento em aberto para turma informada no {request.Bimestre}º Bimestre");

                    return ((await repositorioPeriodoEscolar.ObterPorTipoCalendario(request.TipoCalendarioId)).FirstOrDefault(a => a.Bimestre == request.Bimestre)
                        , new PeriodoDto(fechamentoReabertura.Inicio, fechamentoReabertura.Fim)
                        , request.Aplicacao);
                }
            }

            return (periodoFechamentoBimestre?.PeriodoEscolar
                , periodoFechamentoBimestre is null ?
                    null :
                    new PeriodoDto(periodoFechamentoBimestre.InicioDoFechamento.Value, periodoFechamentoBimestre.FinalDoFechamento.Value)
                , request.Aplicacao);
        }

        private EventoTipo ObterTipoEventoFechamentoBimestre()
        {
            EventoTipo tipoEvento = repositorioEventoTipo.ObterPorCodigo((int)TipoEvento.FechamentoBimestre);
            if (tipoEvento.EhNulo())
                throw new NegocioException($"Não foi possível localizar o tipo de evento {TipoEvento.FechamentoBimestre.ObterAtributo<DisplayAttribute>().Name}.");
            return tipoEvento;
        }
    }
}