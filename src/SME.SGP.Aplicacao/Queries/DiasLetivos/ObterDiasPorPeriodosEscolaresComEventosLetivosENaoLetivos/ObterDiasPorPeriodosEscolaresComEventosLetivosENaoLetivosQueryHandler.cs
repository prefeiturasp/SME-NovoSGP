using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDiasPorPeriodosEscolaresComEventosLetivosENaoLetivosQueryHandler : IRequestHandler<ObterDiasPorPeriodosEscolaresComEventosLetivosENaoLetivosQuery, List<DiaLetivoDto>>
    {
        private readonly IRepositorioEvento repositorioEvento;

        public ObterDiasPorPeriodosEscolaresComEventosLetivosENaoLetivosQueryHandler(IRepositorioEvento repositorioEvento)
        {
            this.repositorioEvento = repositorioEvento ?? throw new ArgumentNullException(nameof(repositorioEvento));
        }

        public async Task<List<DiaLetivoDto>> Handle(ObterDiasPorPeriodosEscolaresComEventosLetivosENaoLetivosQuery request, CancellationToken cancellationToken)
        {
            var datasDosPeriodosEscolares = new List<DiaLetivoDto>();

            var eventos = (await repositorioEvento
                .ObterEventosPorTipoDeCalendarioAsync(request.TipoCalendarioId))?
                .Where(c => (c.EhEventoUE() || c.EhEventoSME()));            

            foreach (var periodoEscolar in request.PeriodosEscolares.OrderBy(c => c.Bimestre))
            {                
                foreach (var diaAtual in periodoEscolar.ObterIntervaloDatas())
                {
                    var diaLetivoDto = new DiaLetivoDto()
                    {
                        Data = diaAtual,
                        PossuiEvento = false
                    };

                    var eventosComData = eventos
                        .Where(e => diaAtual.Date >= e.DataInicio.Date && diaAtual.Date <= e.DataFim.Date)
                        .ToList();

                    if (!eventosComData.Any())
                    {
                        diaLetivoDto.EhLetivo = !diaAtual.FimDeSemana();
                        datasDosPeriodosEscolares.Add(diaLetivoDto);
                        continue;
                    }

                    var eventosSME = eventosComData
                        .Where(e => e.EhEventoSME());

                    if (eventosSME.Any())
                    {
                        diaLetivoDto.EhLetivo = eventosSME.Any(e => e.EhEventoLetivo());
                        diaLetivoDto.Motivo = eventosSME.First().Nome;
                        diaLetivoDto.PossuiEvento = true;
                        datasDosPeriodosEscolares.Add(diaLetivoDto);
                        continue;
                    }

                    var eventosDRE = eventosComData
                        .Where(e => e.EhEventoDRE());

                    if (eventosDRE.Any())
                    {
                        diaLetivoDto.EhLetivo = eventosDRE.Any(e => e.EhEventoLetivo());
                        diaLetivoDto.Motivo = eventosDRE.First().Nome;
                        diaLetivoDto.DreIds = eventosDRE.Select(e => e.DreId).ToList();
                        diaLetivoDto.PossuiEvento = true;
                        datasDosPeriodosEscolares.Add(diaLetivoDto);
                        continue;
                    }

                    var eventosLetivosNaoLetivosUE = eventosComData
                        .Where(e => e.EhEventoUE());

                    if (eventosLetivosNaoLetivosUE.Any())
                    {
                        eventosLetivosNaoLetivosUE.ToList().ForEach(elue =>
                        {
                            datasDosPeriodosEscolares.Add(new DiaLetivoDto()
                            {
                                Data = diaAtual,
                                PossuiEvento = true,
                                EhLetivo = elue.EhEventoLetivo(),
                                Motivo = elue.Nome,
                                UesIds = new List<string>() { elue.UeId }
                            });
                        });
                    }                          

                    diaLetivoDto.EhLetivo = !diaAtual.FimDeSemana();
                    datasDosPeriodosEscolares.Add(diaLetivoDto);
                }
            }           

            return datasDosPeriodosEscolares;
        }
    }
}