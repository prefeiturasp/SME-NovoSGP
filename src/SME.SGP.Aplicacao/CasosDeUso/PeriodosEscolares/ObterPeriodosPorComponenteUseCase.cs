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
            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());
            
            var periodoEscolar = await mediator.Send(new ObterPeriodosEscolaresPorComponenteBimestreTurmaQuery(turmaCodigo, componenteCodigo, bimestre,usuarioLogado.EhProfessorCj()));
            var dadosTurma = await mediator.Send(new ObterTurmaPorCodigoQuery(turmaCodigo));
            var periodoBimestre = await mediator.Send(new ObterPeriodoEscolaresPorTurmaBimestresAulaCjQuery(dadosTurma, bimestre,usuarioLogado.EhProfessorCj()));
            var listaPeriodos = new List<PeriodoEscolarComponenteDto>();

            if (periodoEscolar.Any() && !ehRegencia)
                listaPeriodos = SepararPeriodosAulas(periodoEscolar.OrderBy(x => x.DataAula), exibirDataFutura);
            else if(periodoBimestre != null)
                listaPeriodos = SepararSemanasRegencia(periodoBimestre, exibirDataFutura);

            return listaPeriodos;
        }

        private List<PeriodoEscolarComponenteDto> SepararSemanasRegencia(PeriodoEscolarBimestreDto periodoBimestre, bool exibirDataFutura)
        {
            long id = 1;
            var domingo = DateTimeExtension.ObterDomingo(periodoBimestre.PeriodoInicio);
            var sabado = DateTimeExtension.ObterSabado(periodoBimestre.PeriodoInicio);

            var retornaListaPeriodoSeparado = new List<PeriodoEscolarComponenteDto>();
            string formataDataInicio = periodoBimestre.PeriodoInicio.Date.ToString("dd/MM/yy");
            string formataDataFim = sabado.Date.ToString("dd/MM/yy");

            retornaListaPeriodoSeparado.Add(new PeriodoEscolarComponenteDto
            {
                Id = id++,
                DataInicio = periodoBimestre.PeriodoInicio,
                DataFim = sabado,
                PeriodoEscolar = $"{formataDataInicio} - {formataDataFim}",
                AulaCj = periodoBimestre.AulaCj
            });

            domingo = domingo.AddDays(7);
            sabado = sabado.AddDays(7);

            periodoBimestre.PeriodoFim = exibirDataFutura
                ? periodoBimestre.PeriodoFim
                : periodoBimestre.PeriodoFim < DateTimeExtension.HorarioBrasilia()
                                    ? periodoBimestre.PeriodoFim
                                    : DateTimeExtension.HorarioBrasilia();

            while (domingo < periodoBimestre.PeriodoFim)
            {
                if (sabado > periodoBimestre.PeriodoFim)
                    sabado = periodoBimestre.PeriodoFim;

                formataDataInicio = domingo.Date.ToString("dd/MM/yy");
                formataDataFim = sabado.Date.ToString("dd/MM/yy");

                retornaListaPeriodoSeparado.Add(new PeriodoEscolarComponenteDto
                {
                    Id = id++,
                    DataInicio = domingo,
                    DataFim = sabado,
                    PeriodoEscolar = $"{formataDataInicio} - {formataDataFim}",
                    AulaCj = periodoBimestre.AulaCj
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
                        PeriodoEscolar = $"{formataDataInicio} - {formataDataFim}",
                        AulaCj = periodo.AulaCj
                        
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
