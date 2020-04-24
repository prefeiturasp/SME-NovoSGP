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
        public static async Task<int[]> Executar(IMediator mediator, FiltroAulasEventosCalendarioDto filtroAulasEventosCalendarioDto, long tipoCalendarioId, int mes)
        {

            var eventosDaUeSME = await mediator.Send(new ObterEventosDaUeSMEPorMesQuery()
            {
                UeCodigo = filtroAulasEventosCalendarioDto.UeCodigo,
                DreCodigo = filtroAulasEventosCalendarioDto.DreCodigo,
                TipoCalendarioId = tipoCalendarioId,
                Mes = mes
            });

            var qntDiasMes = DateTime.DaysInMonth(filtroAulasEventosCalendarioDto.AnoLetivo, mes);

            var listaRetorno = new List<int>(); 
            for (int i = 1; i < qntDiasMes + 1; i++)
            {
                if (eventosDaUeSME.Any(a =>  i >= a.DataInicio.Day  && i <= a.DataFim.Day ))
                    listaRetorno.Add( i);
            }

            return listaRetorno.ToArray();          
        }
    }
}
