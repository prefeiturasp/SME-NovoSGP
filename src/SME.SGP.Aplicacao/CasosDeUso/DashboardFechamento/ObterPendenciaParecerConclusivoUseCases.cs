using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciaParecerConclusivoUseCases : AbstractUseCase, IObterPendenciaParecerConclusivoUseCases
    {
        public ObterPendenciaParecerConclusivoUseCases(IMediator mediator) : base(mediator)
        {

        }
        public async Task<IEnumerable<GraficoBaseDto>> Executar(FiltroDashboardFechamentoDto param)
        {
            var parecerConclusivo = await mediator.Send(new ObterPendenciaParecerConclusivoSituacaoQuery(param.UeId, param.AnoLetivo,
                        param.DreId,
                        param.Modalidade,
                        param.Semestre,
                        param.Bimestre));

            List<GraficoBaseDto> parecerConclusivos = new List<GraficoBaseDto>();

            foreach (var parecer in parecerConclusivo)
            {
                var grupo = $"{parecer.AnoTurma}";
                if (parecer.Quantidade > 0)
                    parecerConclusivos.Add(new GraficoBaseDto(grupo, parecer.Quantidade, parecer.Situacao.Name()));
            }

            return parecerConclusivos.OrderBy(a => a.Grupo).ToList();
        }
    }
}
