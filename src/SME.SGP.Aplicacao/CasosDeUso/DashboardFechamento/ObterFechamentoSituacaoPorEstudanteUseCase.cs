using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.Aplicacao
{
    public class ObterFechamentoSituacaoPorEstudanteUseCase : AbstractUseCase,
        IObterFechamentoSituacaoPorEstudanteUseCase
    {
        public ObterFechamentoSituacaoPorEstudanteUseCase(IMediator mediator) : base(mediator)
        {
        }


        public async Task<IEnumerable<FechamentoSituacaoPorEstudanteDto>> Executar(FiltroDashboardFechamentoDto param)
        {
            var fechamentosRetorno = await mediator.Send(new ObterFechamentoSituacaoQuery(param.UeId, param.AnoLetivo,
                param.DreId,
                param.Modalidade,
                param.Semestre,
                param.Bimestre));

            var fechamentos = new List<FechamentoSituacaoPorEstudanteDto>();
            if (fechamentosRetorno == null || !fechamentosRetorno.Any())
                return fechamentos;

             
            //var alunos = await mediator.Send(new ObterEstudantesAtivosPorTurmaEDataReferenciaQuery());

            // foreach (var fechamentoRetorno in fechamentosRetorno.GroupBy(a => a.Ano))
            // {
            //     var novoFechamento = new FechamentoSituacaoDto();
            //     novoFechamento.Ordem = fechamentoRetorno.FirstOrDefault().Ano;
            //     novoFechamento.MontarDescricao(fechamentoRetorno.FirstOrDefault().Modalidade.ShortName(),
            //         fechamentoRetorno.FirstOrDefault().Ano);
            //     foreach (var fechamentoGroup in fechamentoRetorno)
            //     {
            //         switch (fechamentoGroup.Situacao)
            //         {
            //             case 1:
            //                 novoFechamento.AdicionarValorProcessadoNaoIniciado(fechamentoGroup.Quantidade);
            //                 break;
            //             case 2:
            //                 novoFechamento.AdicionarValorProcessadoPendente(
            //                     fechamentoGroup.Quantidade);
            //                 break;
            //             case 3:
            //                 novoFechamento.AdicionarValorProcessadoSucesso(
            //                     fechamentoGroup.Quantidade);
            //                 break;
            //         }
            //     }
            //
            //     fechamentos.Add(novoFechamento);
            // }

            return fechamentos;
        }
    }
}