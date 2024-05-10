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
                    notaFinal.NotaAcimaMedia = notaFinal.Nota.Value >= double.Parse(notaMinima.Valor);

                if (!string.IsNullOrEmpty(notaFinal.Conceito))
                    notaFinal.NotaAcimaMedia = notaFinal.Conceito != "NS";
            }

            var groupKey = (param.DreId == 0 && param.UeId == 0) || (param.DreId != 0 && param.UeId == 0) ? "Ano" : "TurmaAnoNome";
            var notasFinaisGrupo = notasFinaisRetorno.GroupBy(nf => nf.GetType().GetProperty(groupKey).GetValue(nf).ToString());

            foreach (var notaFinalGrupo in notasFinaisGrupo)
            {
                var grupo = $"{notaFinalGrupo.Key}";

                var countBelow = notaFinalGrupo.Count(a => !a.NotaAcimaMedia);
                if (countBelow > 0)
                    notasFinais.Add(new GraficoBaseDto(grupo, countBelow, "Abaixo do mínimo"));

                var countAbove = notaFinalGrupo.Count(a => a.NotaAcimaMedia);
                if (countAbove > 0)
                    notasFinais.Add(new GraficoBaseDto(grupo, countAbove, "Acima do mínimo"));
            }

            return notasFinais.OrderBy(a => a.Grupo).ToList();
        }
    }
}
