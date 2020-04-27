using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDiasDosEventosCalendarioProfessorUseCase
    {
        public static async Task<IEnumerable<EventoAulaDiaDto>> Executar(IMediator mediator, FiltroAulasEventosCalendarioDto filtroAulasEventosCalendarioDto, long tipoCalendarioId, int mes)
        {

            var eventosDaUeSME = await mediator.Send(new ObterEventosDaUeSMEPorMesQuery()
            {
                UeCodigo = filtroAulasEventosCalendarioDto.UeCodigo,
                DreCodigo = filtroAulasEventosCalendarioDto.DreCodigo,
                TipoCalendarioId = tipoCalendarioId,
                Mes = mes
            });

            var aulas =  await mediator.Send(new ObterAulasQuery()
            {
                UeCodigo = filtroAulasEventosCalendarioDto.UeCodigo,
                DreCodigo = filtroAulasEventosCalendarioDto.DreCodigo,
                TipoCalendarioId = tipoCalendarioId,
                TurmaCodigo = filtroAulasEventosCalendarioDto.TurmaCodigo,
                Mes = mes
            });

            var qntDiasMes = DateTime.DaysInMonth(filtroAulasEventosCalendarioDto.AnoLetivo, mes);

            var listaRetorno = new List<EventoAulaDiaDto>(); 
            for (int i = 1; i < qntDiasMes + 1; i++)
            {
                var eventoAula = new EventoAulaDiaDto() { Dia = i };

                if (eventosDaUeSME.Any(a => i >= a.DataInicio.Day && i <= a.DataFim.Day))
                    eventoAula.TemEvento = true;

                var aulasDoDia = aulas.Where(a => a.DataAula.Day == i).ToList();
                if (aulasDoDia.Any())
                {
                    if (aulasDoDia.Any(a => a.AulaCJ))
                        eventoAula.TemAulaCJ = true;
                    
                    if (aulasDoDia.Any(a => a.AulaCJ == false))
                        eventoAula.TemAula = true;
                }

                listaRetorno.Add(eventoAula);
            }

            return listaRetorno.ToArray();          
        }
    }
}
