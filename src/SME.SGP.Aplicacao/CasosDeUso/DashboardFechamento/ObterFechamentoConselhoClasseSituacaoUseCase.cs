using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterFechamentoConselhoClasseSituacaoUseCase : AbstractUseCase, IObterFechamentoConselhoClasseSituacaoUseCase
    {
        public ObterFechamentoConselhoClasseSituacaoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<GraficoBaseDto>> Executar(FiltroDashboardFechamentoDto param)
        {
            var conselhoClasse = await mediator.Send(new ObterConselhoClasseSituacaoQuery(param.UeId, param.AnoLetivo,
                param.DreId,
                param.Modalidade,
                param.Semestre,
                param.Bimestre));

            List<GraficoBaseDto> conselhos = new List<GraficoBaseDto>();

            foreach (var conseho in conselhoClasse)
            {
                var grupo = $"{conseho.AnoTurma}";
                if(conseho.Quantidade > 0)
                    conselhos.Add(new GraficoBaseDto(grupo, conseho.Quantidade, conseho.Situacao.Name()));
            }

            return conselhos.OrderBy(a => a.Grupo).ToList();
        }
    }
}