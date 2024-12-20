﻿using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ComandosPeriodoEscolar : IComandosPeriodoEscolar
    {
        private readonly IRepositorioPeriodoEscolar repositorioPeriodo;
        private readonly IServicoPeriodoEscolar servicoPeriodoEscolar;
        private readonly IMediator mediator;

        public ComandosPeriodoEscolar(IRepositorioPeriodoEscolar repositorioPeriodo, IServicoPeriodoEscolar servicoPeriodoEscolar, IMediator mediator)
        {
            this.repositorioPeriodo = repositorioPeriodo ?? throw new ArgumentNullException(nameof(repositorioPeriodo));
            this.servicoPeriodoEscolar = servicoPeriodoEscolar ?? throw new ArgumentNullException(nameof(servicoPeriodoEscolar));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task Salvar(PeriodoEscolarListaDto periodosDto)
        {
            var periodosOrdenados = periodosDto.Periodos.OrderBy(p => p.Bimestre);
            periodosDto.Periodos = periodosOrdenados.ToList();
            if (periodosDto.TipoCalendario == 0) throw new NegocioException("É necessario informar o tipo de calendario");

            var listaPeriodoEscolar = MapearListaPeriodos(periodosDto);

            await servicoPeriodoEscolar.SalvarPeriodoEscolar(listaPeriodoEscolar, periodosDto.TipoCalendario);
            await mediator.Send(new CriarPeriodosConfiguracaoRelatorioPAPAnoAtualCommand(periodosDto.TipoCalendario));
        }

        private IEnumerable<PeriodoEscolar> MapearListaPeriodos(PeriodoEscolarListaDto periodosDto)
        {
            var retorno = new List<PeriodoEscolar>();

            foreach (var periodo in periodosDto.Periodos)
            {
                if (periodo.Id > 0)
                    retorno.Add(ObterPeriodo(periodo.Id, periodo));
                else
                    retorno.Add(MapearParaDominio(periodo, periodosDto.TipoCalendario));
            }

            return retorno;
        }

        private PeriodoEscolar MapearParaDominio(PeriodoEscolarDto periodoDto, long tipoCalendario)
        {
            return new PeriodoEscolar
            {
                Bimestre = periodoDto.Bimestre,
                PeriodoInicio = periodoDto.PeriodoInicio.Date,
                PeriodoFim = periodoDto.PeriodoFim.Date,
                TipoCalendarioId = tipoCalendario,
                Migrado = false,
            };
        }

        private PeriodoEscolar ObterPeriodo(long Id, PeriodoEscolarDto periodo)
        {
            var periodoSalvar = repositorioPeriodo.ObterPorId(Id);

            periodoSalvar.PeriodoInicio = periodo.PeriodoInicio.Date;
            periodoSalvar.PeriodoFim = periodo.PeriodoFim.Date;

            return periodoSalvar;
        }
    }
}