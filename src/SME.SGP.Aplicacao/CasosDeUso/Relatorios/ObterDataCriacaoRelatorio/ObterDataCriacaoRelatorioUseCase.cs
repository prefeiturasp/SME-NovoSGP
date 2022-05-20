﻿using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDataCriacaoRelatorioUseCase : AbstractUseCase, IObterDataCriacaoRelatorioUseCase
    {
        public ObterDataCriacaoRelatorioUseCase(MediatR.IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(Guid codigoRelatorio)
        {
            bool relatorioExiste = false;
            const int tempoParaExclusaoDoRelatorio = 24;
            var dataCriacaoRelatorio = await mediator.Send(new ObterDataCriacaoRelatorioPorCodigoQuery(codigoRelatorio));
            if (dataCriacaoRelatorio != null)
            {
                var tempoEmHorasDaCriacao = CalcularTempoEmHoras(dataCriacaoRelatorio.CriadoEm);

                if (tempoEmHorasDaCriacao < tempoParaExclusaoDoRelatorio)
                    relatorioExiste = true;
            }
            return relatorioExiste;
        }

        private double CalcularTempoEmHoras(DateTime criadoEm)
        {
            TimeSpan tempo = DateTime.Now - criadoEm;
            return tempo.TotalHours;
        }
    }
}
