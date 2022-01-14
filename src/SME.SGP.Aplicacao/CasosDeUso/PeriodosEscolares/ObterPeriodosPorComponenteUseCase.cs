using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodosPorComponenteUseCase : AbstractUseCase, IObterPeriodosPorComponenteUseCase
    {
        public ObterPeriodosPorComponenteUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<PeriodoEscolarComponenteDto>> Executar(string turmaCodigo, long componenteCodigo, bool ehRegencia, int bimestre, bool exibirDataFutura = false)
        {
            var periodoEscolar = await mediator.Send(new ObterPeriodosEscolaresPorComponenteBimestreTurmaQuery(turmaCodigo, componenteCodigo, bimestre));
            var dadosTurma = await mediator.Send(new ObterTurmaPorCodigoQuery(turmaCodigo));
            var periodoBimestre = await mediator.Send(new ObterPeriodoEscolarPorTurmaBimestreQuery(dadosTurma, bimestre));
            var listaPeriodos = new List<PeriodoEscolarComponenteDto>();

            if (periodoEscolar.Any() && !ehRegencia)
                listaPeriodos = SepararPeriodosAulas(periodoEscolar.OrderBy(x => x.DataAula), exibirDataFutura);
            else if(periodoBimestre != null)
                listaPeriodos = SepararSemanasRegencia(periodoBimestre.PeriodoInicio, periodoBimestre.PeriodoFim, exibirDataFutura);

            return listaPeriodos;
        }

        private List<PeriodoEscolarComponenteDto> SepararSemanasRegencia(DateTime dataInicio, DateTime dataFim, bool exibirDataFutura)
        {
            long id = 1;
            var domingo = DateTimeExtension.ObterDomingo(dataInicio);
            var sabado = DateTimeExtension.ObterSabado(dataInicio);

            var retornaListaPeriodoSeparado = new List<PeriodoEscolarComponenteDto>();
            string formataDataInicio = dataInicio.Date.ToString("dd/MM/yy");
            string formataDataFim = sabado.Date.ToString("dd/MM/yy");

            retornaListaPeriodoSeparado.Add(new PeriodoEscolarComponenteDto
            {
                Id = id++,
                DataInicio = dataInicio,
                DataFim = sabado,
                PeriodoEscolar = $"{formataDataInicio} - {formataDataFim}"
            });

            domingo = domingo.AddDays(7);
            sabado = sabado.AddDays(7);

            dataFim = exibirDataFutura
                ? dataFim
                : dataFim < DateTimeExtension.HorarioBrasilia()
                                    ? dataFim
                                    : DateTimeExtension.HorarioBrasilia();

            while (domingo < dataFim)
            {
                if (sabado > dataFim)
                    sabado = dataFim;

                formataDataInicio = domingo.Date.ToString("dd/MM/yy");
                formataDataFim = sabado.Date.ToString("dd/MM/yy");

                retornaListaPeriodoSeparado.Add(new PeriodoEscolarComponenteDto
                {
                    Id = id++,
                    DataInicio = domingo,
                    DataFim = sabado,
                    PeriodoEscolar = $"{formataDataInicio} - {formataDataFim}"
                });
                domingo = domingo.AddDays(7);
                sabado = sabado.AddDays(7);
            }


            return retornaListaPeriodoSeparado;
        }

        private List<PeriodoEscolarComponenteDto> SepararPeriodosAulas(IEnumerable<PeriodoEscolarVerificaRegenciaDto> periodosAulas, bool exibirDataFutura)
        {
            DateTime dataInicioPeriodo = DateTime.MinValue;

            var listaPeriodos = new List<PeriodoEscolarComponenteDto>();
            int qtdeDiasAulas = 0;
            long id = 1;
            int contador = 1;

            periodosAulas = exibirDataFutura ? periodosAulas : periodosAulas.Where(w => w.DataAula <= DateTimeExtension.HorarioBrasilia());

            foreach (var periodo in periodosAulas)
            {
                if (qtdeDiasAulas == 0)
                    if (dataInicioPeriodo == DateTime.MinValue)
                        dataInicioPeriodo = periodo.DataAula;

                if (qtdeDiasAulas < 5)
                    qtdeDiasAulas++;

                if (qtdeDiasAulas == 5 || contador == periodosAulas.Count())
                {
                    string formataDataInicio = dataInicioPeriodo.Date.ToString("dd/MM/yy");
                    string formataDataFim = periodo.DataAula.Date.ToString("dd/MM/yy");
                    listaPeriodos.Add(new PeriodoEscolarComponenteDto
                    {
                        Id = id++,
                        DataInicio = dataInicioPeriodo,
                        DataFim = periodo.DataAula,
                        PeriodoEscolar = $"{formataDataInicio} - {formataDataFim}"
                    });

                    dataInicioPeriodo = DateTime.MinValue;
                    qtdeDiasAulas = 0;
                }
                contador++;
            }
            return listaPeriodos;
        }
    }
}
