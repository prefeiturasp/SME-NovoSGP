using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.Infra.Dtos.DashboardFechamento;

namespace SME.SGP.Aplicacao
{
    public class ObterFechamentoSituacaoUseCase : AbstractUseCase, IObterFechamentoSituacaoUseCase
    {
        public ObterFechamentoSituacaoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<FechamentoSituacaoDto>> Executar(FiltroDashboardFechamentoDto param)
        {
            var fechamentosRetorno = await mediator.Send(new ObterFechamentoSituacaoQuery(param.UeId, param.AnoLetivo, param.DreId,
                param.Modalidade,
                param.Semestre,
                param.Bimestre));

            List<FechamentoSituacaoDto> fechamentos = new List<FechamentoSituacaoDto>();
            if (!fechamentosRetorno.Any())
                return fechamentos;

            foreach (var fechamentoRetorno in fechamentosRetorno.GroupBy(a => a.Ano))
            {
                var novoFechamento = new FechamentoSituacaoDto();
                novoFechamento.Ordem = fechamentoRetorno.FirstOrDefault().Ano; 
                novoFechamento.MontarDescricao(fechamentoRetorno.FirstOrDefault().Modalidade.ShortName(), fechamentoRetorno.FirstOrDefault().Ano);
                foreach (var fechamentoGroup in fechamentoRetorno)
                {
                    
                    switch (fechamentoGroup.Situacao)
                    {
                        case 1:
                            novoFechamento.AdicionarValorProcessadoNaoIniciado(fechamentoGroup.Quantidade);
                            break;
                        case 2:
                            novoFechamento.AdicionarValorProcessadoPendente(
                                fechamentoGroup.Quantidade);
                            break;
                        case 3:
                            novoFechamento.AdicionarValorProcessadoSucesso(
                                fechamentoGroup.Quantidade);
                            break;
                    }
                }
                fechamentos.Add(novoFechamento);
            }

            return fechamentos;
        }
    }
}