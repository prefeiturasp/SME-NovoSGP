﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.Aplicacao
{
    public class ExecutarBuscarUesConsolidadoEncaminhamentoNAAPAUseCase: AbstractUseCase,IExecutarBuscarUesConsolidadoEncaminhamentoNAAPAUseCase
    {
        public ExecutarBuscarUesConsolidadoEncaminhamentoNAAPAUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var filtro = param.ObterObjetoMensagem<FiltroBuscarUesConsolidadoEncaminhamentoNAAPADto>();
            var encaminhamentos = await mediator.Send(new ObterEncaminhamentosNAAPAConsolidadoCargaQuery(filtro.UeId,filtro.AnoLetivo));
            foreach (var encaminhamento in encaminhamentos)
            {
                var entidade = new ConsolidadoEncaminhamentoNAAPA
                {
                    UeId = encaminhamento.UeId,
                    AnoLetivo = encaminhamento.AnoLetivo,
                    Quantidade = encaminhamento.Quantidade,
                    Situacao = encaminhamento.Situacao,
                };
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpNAAPA.ExecutarInserirConsolidadoEncaminhamentoNAAPA, entidade, Guid.NewGuid()));
            }
            await PublicarExclusaoConsolidacao(filtro.UeId, filtro.AnoLetivo, encaminhamentos);         
            return true;
        }

        private async Task PublicarExclusaoConsolidacao(long ueId, int anoLetivo, IEnumerable<EncaminhamentosNAAPAConsolidadoDto> encaminhamentosConsolidacao)
        {
            var situacoesEncaminhamentosConsolidacao = encaminhamentosConsolidacao.Select(e => e.Situacao).Distinct();
            var situacoesNAAPA = EnumExtensao.ListarDto<SituacaoNAAPA>().ToList().Select(s => s.Id);
            if (situacoesNAAPA.Except(situacoesEncaminhamentosConsolidacao.Select(s => (int)s)).Any())
            {
                var param = new FiltroExcluirUesConsolidadoEncaminhamentoNAAPADto(ueId, anoLetivo, situacoesEncaminhamentosConsolidacao.ToArray());
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpNAAPA.ExecutarExcluirConsolidadoEncaminhamentoNAAPA, param, Guid.NewGuid()));
            }
        }
    }
}