using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterNotasFinaisUseCases : AbstractUseCase, IObterNotasFinaisUseCases
    {
        public ObterNotasFinaisUseCases(IMediator mediator) : base(mediator)
        {

        }
        public async Task<IEnumerable<GraficoBaseDto>> Executar(FiltroDashboardFechamentoDto param)
        {
            var notasFinaisRetorno = await mediator.Send(new ObterNotasFinaisFechamentoQuery(param.UeId, param.AnoLetivo,
                        param.DreId,
                        param.Modalidade,
                        param.Semestre,
                        param.Bimestre));

            List<GraficoBaseDto> notasFinais = new List<GraficoBaseDto>();
            var notaMinima = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(Dominio.TipoParametroSistema.MediaBimestre, param.AnoLetivo));

            foreach (var notaFinal in notasFinaisRetorno)
            {
                if (notaFinal.Nota.HasValue)
                    notaFinal.NotaAcimaMedia = notaFinal.Nota.Value > double.Parse(notaMinima.Valor);

                if (!string.IsNullOrEmpty(notaFinal.Conceito))
                    notaFinal.NotaAcimaMedia = notaFinal.Conceito != "NS";
            }

            foreach(var notaFinalTurma in notasFinaisRetorno.GroupBy(nf => nf.TurmaAnoNome))
            {
                var grupo = $"{notaFinalTurma.Key}";
                if(notaFinalTurma.Count(a => !a.NotaAcimaMedia) > 0)
                    notasFinais.Add(new GraficoBaseDto(grupo, notaFinalTurma.Count(a => !a.NotaAcimaMedia), "Abaixo do mínimo"));

                if(notaFinalTurma.Count(a => a.NotaAcimaMedia) > 0)
                    notasFinais.Add(new GraficoBaseDto(grupo, notaFinalTurma.Count(a => a.NotaAcimaMedia), "Acima do mínimo"));
            }

            return notasFinais.OrderBy(a => a.Grupo).ToList();
        }
    }
}
