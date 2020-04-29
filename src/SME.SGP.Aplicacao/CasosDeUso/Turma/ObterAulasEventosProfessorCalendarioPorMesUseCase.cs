using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAulasEventosProfessorCalendarioPorMesUseCase
    {
        public static async Task<IEnumerable<EventoAulaDiaDto>> Executar(IMediator mediator, FiltroAulasEventosCalendarioDto filtroAulasEventosCalendarioDto, long tipoCalendarioId, 
            int mes, IServicoUsuario servicoUsuario)
        {

            var eventosDaUeSME = await mediator.Send(new ObterEventosDaUeSMEPorMesQuery()
            {
                UeCodigo = filtroAulasEventosCalendarioDto.UeCodigo,
                DreCodigo = filtroAulasEventosCalendarioDto.DreCodigo,
                TipoCalendarioId = tipoCalendarioId,
                Mes = mes
            });

            var usuarioRfLogado = servicoUsuario.ObterRf();

            var aulas = await mediator.Send(new ObterAulasCalendarioProfessorPorMesQuery()
            {
                UeCodigo = filtroAulasEventosCalendarioDto.UeCodigo,
                DreCodigo = filtroAulasEventosCalendarioDto.DreCodigo,
                TipoCalendarioId = tipoCalendarioId,
                TurmaCodigo = filtroAulasEventosCalendarioDto.TurmaCodigo,
                Mes = mes,
                CriadorRF = usuarioRfLogado

            });

            var avaliacoes = await mediator.Send(new ObterAtividadesAvaliativasCalendarioProfessorPorMesQuery()
            {
                UeCodigo = filtroAulasEventosCalendarioDto.UeCodigo,
                DreCodigo = filtroAulasEventosCalendarioDto.DreCodigo,
                TipoCalendarioId = tipoCalendarioId,
                TurmaCodigo = filtroAulasEventosCalendarioDto.TurmaCodigo,
                Mes = mes,
                AnoLetivo = filtroAulasEventosCalendarioDto.AnoLetivo,
                CriadorRF = usuarioRfLogado

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

                if (avaliacoes.Any(a => a.DataAvaliacao.Day == i))
                    eventoAula.TemAvaliacao = true;

                listaRetorno.Add(eventoAula);
            }

            return listaRetorno.ToArray();          
        }
    }
}
