using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodoPorComponenteUseCase : AbstractUseCase, IObterPeriodoPorComponenteUseCase
    {
        public ObterPeriodoPorComponenteUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<List<PeriodoEscolarComponenteDto>> Executar(string turmaCodigo, string componenteCodigo, int bimestre)
        {
            var periodoEscolar = await mediator.Send(new ObterPeriodosEscolaresPorComponenteBimestreTurmaQuery(turmaCodigo, componenteCodigo, bimestre));
            var listaPeriodos = new List<PeriodoEscolarComponenteDto>();

            if (periodoEscolar.FirstOrDefault().EhRegencia)
                listaPeriodos = SepararSemanasRegencia(periodoEscolar.FirstOrDefault().DataInicio, periodoEscolar.FirstOrDefault().DataFim);
            else
                listaPeriodos = SepararPeriodosAulas(periodoEscolar);

            return listaPeriodos;
        }

        public List<PeriodoEscolarComponenteDto> SepararSemanasRegencia(DateTime dataInicioPeriodo, DateTime dataFimPeriodo)
        {
            var retornaListaPeriodoSeparado = new List<PeriodoEscolarComponenteDto>();
            var dataInicio = DateTime.MinValue;
            var dataFim = DateTime.MinValue;
            long id = 1;
            bool jaPassouPorUltimosDias = false;

            while(dataInicioPeriodo <= dataFimPeriodo)
            {
                if(dataInicio == DateTime.MinValue)
                    dataInicio = dataInicioPeriodo;

                //para verificar ultimos dias do período
                TimeSpan calcularDiferencaDatas = dataFimPeriodo - dataInicio;

                if (dataInicioPeriodo.DayOfWeek == DayOfWeek.Saturday)
                    dataFim = dataInicioPeriodo;

                if(calcularDiferencaDatas.Days < 7 && dataFimPeriodo.DayOfWeek < DayOfWeek.Saturday && !jaPassouPorUltimosDias)
                {
                    dataFim = dataFimPeriodo;
                    jaPassouPorUltimosDias = true;
                }
                  
                if (dataInicio != DateTime.MinValue && dataFim != DateTime.MinValue)
                {
                    string formataDataInicio = dataInicio.Date.ToString("dd/MM/yy");
                    string formataDataFim = dataFim.Date.ToString("dd/MM/yy");

                    retornaListaPeriodoSeparado.Add(new PeriodoEscolarComponenteDto
                    {
                        Id = id++,
                        DataInicio = dataInicio,
                        DataFim = dataFim,
                        PeriodoEscolar = $"{formataDataInicio} - {formataDataFim}"
                    });

                    dataInicio = dataFim = DateTime.MinValue;
                }

                dataInicioPeriodo = dataInicioPeriodo.AddDays(1);
            }
            return retornaListaPeriodoSeparado;
        }

        public List<PeriodoEscolarComponenteDto> SepararPeriodosAulas(IEnumerable<PeriodoEscolarVerificaRegenciaDto> periodosAulas)
        {
            DateTime dataInicioPeriodo = DateTime.MinValue;

            var listaPeriodos = new List<PeriodoEscolarComponenteDto>();
            int qtdeDiasAulas = 0;
            long id = 1;
            int contador = 1;

            foreach (var periodo in periodosAulas)
            {
                if(qtdeDiasAulas == 0)
                    if (dataInicioPeriodo == DateTime.MinValue)
                             dataInicioPeriodo = periodo.DataAula;
                 
                if(qtdeDiasAulas < 5)
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
