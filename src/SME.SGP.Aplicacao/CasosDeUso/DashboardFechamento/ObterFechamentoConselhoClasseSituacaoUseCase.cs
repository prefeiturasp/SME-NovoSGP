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
            var conselhoClasse = await mediator.Send(new ObterConselhoClasseSituacaoQuery(param.UeId,
                                                                                          param.AnoLetivo,
                                                                                          param.DreId,
                                                                                          param.Modalidade,
                                                                                          param.Semestre,
                                                                                          param.Bimestre));

            if (conselhoClasse == null || !conselhoClasse.Any())
                return default;

            List<GraficoBaseDto> conselhos = new List<GraficoBaseDto>();

            foreach (var conselho in conselhoClasse.Where(c => c.Quantidade > 0).ToList())
                conselhos.Add(new GraficoBaseDto(conselho.AnoTurma, conselho.Quantidade, conselho.Situacao.Name()));


            return conselhos.OrderBy(a => a.Grupo).ToList();
        }
    }
}