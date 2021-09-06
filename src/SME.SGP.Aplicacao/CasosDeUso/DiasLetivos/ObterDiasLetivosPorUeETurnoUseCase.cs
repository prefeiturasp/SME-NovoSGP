using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDiasLetivosPorUeETurnoUseCase : AbstractUseCase, IObterDiasLetivosPorUeETurnoUseCase
    {
        public ObterDiasLetivosPorUeETurnoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<DiaLetivoSimplesDto>> Executar(FiltroDiasLetivosPorUeEDataDTO param)
        {
            if (param.DataInicio > param.DataFim)
                throw new Exception("A data de início não pode ser maior que a data final.");

            var modalidadeCalendario = param.TipoTurno == (int)TipoTurno.Manha || param.TipoTurno == (int)TipoTurno.Tarde
                ?
                ModalidadeTipoCalendario.FundamentalMedio
                :
                ModalidadeTipoCalendario.EJA;           

            var diasLetivos = new List<DiaLetivoSimplesDto>();
            long tipoCalendarioId = 0;
            var semestre = 0;
            var anoLetivo = param.DataInicio.Year;            

            if (modalidadeCalendario == ModalidadeTipoCalendario.EJA)
                tipoCalendarioId = await mediator.Send(new ObterTipoCalendarioIdPorAnoLetivoModalidadeEDataReferenciaQuery(anoLetivo, modalidadeCalendario, param.DataInicio));
            else
                tipoCalendarioId = await mediator.Send(new ObterTipoCalendarioIdPorAnoLetivoEModalidadeQuery(modalidadeCalendario, anoLetivo, semestre));
            
            if (tipoCalendarioId == 0)
                throw new Exception("Tipo calendário não encontrado");

            var periodosEscolares = await mediator.Send(new ObterPeriodosEscolaresPorTipoCalendarioQuery(tipoCalendarioId));
            if (periodosEscolares == null || !periodosEscolares.Any())
                throw new Exception("Períodos escolares não encontrados");

            var eventos = await mediator.Send(new ObterDiasPorPeriodosEscolaresComEventosLetivosENaoLetivosQuery(periodosEscolares, tipoCalendarioId, false));

            for (var data = param.DataInicio.Date; data <= param.DataFim.Date; data = data.AddDays(1))
            {
                if (!DataDentroDoPeriodoEscolar(periodosEscolares, data))
                {
                    diasLetivos.Add(new DiaLetivoSimplesDto()
                    {
                        Data = data,
                        EhLetivo = false
                    });
                }
                else if (data.FimDeSemana())
                {
                    diasLetivos.Add(new DiaLetivoSimplesDto()
                    {
                        Data = data,
                        EhLetivo = eventos.Any(e => e.Data.Date == data.Date && e.EhLetivo)
                    });
                }
                else
                    diasLetivos.Add(new DiaLetivoSimplesDto()
                    {
                        Data = data,
                        EhLetivo = !eventos.Any(e => e.Data.Date == data.Date && e.EhNaoLetivo)
                    });
            }
            return diasLetivos;
        }

        private bool DataDentroDoPeriodoEscolar(IEnumerable<PeriodoEscolar> periodosEscolares, DateTime data)
        {
            foreach (var periodo in periodosEscolares)
                if (periodo.DataDentroPeriodo(data))
                    return true;

            return false;
        }
    }
}
