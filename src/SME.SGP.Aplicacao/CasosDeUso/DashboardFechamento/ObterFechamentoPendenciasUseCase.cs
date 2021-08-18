using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.Aplicacao
{
    public class ObterFechamentoPendenciasUseCase : AbstractUseCase, IObterFechamentoPendenciasUseCase
    {
        public ObterFechamentoPendenciasUseCase(IMediator mediator) : base(mediator)
        {
        }


        public async Task<IEnumerable<FechamentoPendeciaDto>> Executar(FiltroDashboardFechamentoDto param)
        {
            var fechamentosPendenteRetorno = await mediator.Send(new ObterFechamentoPendenciasQuery(
                param.UeId,
                param.AnoLetivo,
                param.DreId,
                param.Modalidade,
                param.Semestre,
                param.Bimestre)
            );


            return fechamentosPendenteRetorno.Select(pendente => new FechamentoPendeciaDto()
            {
                Descricao = $"{pendente.Modalidade.ShortName()} - {pendente.Ano}",
                Ordem = pendente.Ano,
                Quantidade = pendente.Quantidade
            }).ToList();
        }
    }
}