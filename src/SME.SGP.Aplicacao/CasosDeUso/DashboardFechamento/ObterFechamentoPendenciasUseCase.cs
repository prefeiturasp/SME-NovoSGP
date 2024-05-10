﻿using System.Collections.Generic;
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


        public async Task<IEnumerable<GraficoBaseDto>> Executar(FiltroDashboardFechamentoDto param)
        {
            var fechamentosPendenteRetorno = await mediator.Send(new ObterFechamentoPendenciasQuery(
                param.UeId,
                param.AnoLetivo,
                param.DreId,
                param.Modalidade,
                param.Semestre,
                param.Bimestre)
            );


            List<GraficoBaseDto> fechamentos = new List<GraficoBaseDto>();

            foreach (var fechamento in fechamentosPendenteRetorno)
            {
                if (fechamento.Quantidade > 0)
                {
                    fechamentos.Add(new GraficoBaseDto()
                    {
                        Descricao = $"{fechamento.Ano}",
                        Quantidade = fechamento.Quantidade
                    });
                }
                
            }
            return fechamentos.OrderBy(a => a.Descricao).ToList();
        }
    }
}