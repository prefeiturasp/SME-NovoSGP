using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAulasEventosProfessorCalendarioPorDiaMesUseCase
    {
        public static async Task<int[]> Executar(IMediator mediator, FiltroAulasEventosCalendarioDto filtroAulasEventosCalendarioDto, long tipoCalendarioId, int mes, int dia, int anoLetivo, IServicoUsuario servicoUsuario)
        {

            var dataConsulta = new DateTime(anoLetivo, mes, dia);

            var usuarioLogado = await servicoUsuario.ObterUsuarioLogado();

            var eventosDaUeSME = await mediator.Send(new ObterEventosCalendarioProfessorPorMesDiaQuery()
            {
                UeCodigo = filtroAulasEventosCalendarioDto.UeCodigo,
                DreCodigo = filtroAulasEventosCalendarioDto.DreCodigo,
                TipoCalendarioId = tipoCalendarioId,
                DataConsulta = dataConsulta
            });

            var aulasDoDia = await mediator.Send(new ObterAulasCalendarioProfessorPorMesDiaQuery()
            {
                UeCodigo = filtroAulasEventosCalendarioDto.UeCodigo,
                DreCodigo = filtroAulasEventosCalendarioDto.DreCodigo,
                TurmaCodigo = filtroAulasEventosCalendarioDto.TurmaCodigo,
                TipoCalendarioId = tipoCalendarioId,
                DiaConsulta = dataConsulta,
                CriadorRF = usuarioLogado.CodigoRf
            });

            //var qntDiasMes = DateTime.DaysInMonth(filtroAulasEventosCalendarioDto.AnoLetivo, mes);

            //var listaRetorno = new List<int>(); 
            //for (int i = 1; i < qntDiasMes + 1; i++)
            //{
            //    if (eventosDaUeSME.Any(a =>  i >= a.DataInicio.Day  && i <= a.DataFim.Day ))
            //        listaRetorno.Add( i);
            //}

            return default;          
        }
    }
}
