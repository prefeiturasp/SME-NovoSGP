using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterFechamentoSituacaoUseCase : AbstractUseCase, IObterFechamentoSituacaoUseCase
    {
        public ObterFechamentoSituacaoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<GraficoBaseDto>> Executar(FiltroDashboardFechamentoDto param)
        {
            var fechamentosRetorno = await mediator.Send(new ObterFechamentoSituacaoQuery(param.UeId, param.AnoLetivo,
                param.DreId,
                param.Modalidade,
                param.Semestre,
                param.Bimestre));

            List<GraficoBaseDto> fechamentos = new List<GraficoBaseDto>();

            foreach (var fechamento in fechamentosRetorno)
            {
                var grupo = $"{fechamento.AnoTurma}";
                fechamentos.Add(new GraficoBaseDto(grupo, fechamento.Quantidade, fechamento.Situacao.Name()));
            }

            return fechamentos.OrderBy(a => a.Grupo).ToList();
        }
    }
}