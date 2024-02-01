﻿using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class GerarCacheAtribuicaoResponsaveisUseCase : IGerarCacheAtribuicaoResponsaveisUseCase
    {
        private readonly IMediator mediator;

        public GerarCacheAtribuicaoResponsaveisUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            try
            {
                var supervisoresEscolares = await mediator.Send(new ObterResponsaveisAtribuidosUeTiposQuery(new TipoResponsavelAtribuicao[] { TipoResponsavelAtribuicao.SupervisorEscolar }));               
                if (supervisoresEscolares.Any())
                    await mediator.Send(new SalvarCachePorValorObjetoCommand(string.Format(NomeChaveCache.ATRIBUICOES_RESPONSAVEIS_ATIVAS, TipoResponsavelAtribuicao.SupervisorEscolar), 
                                                                             supervisoresEscolares.Select(sup => new AtribuicaoResponsavelVigenteProfDto()
                                                                             {
                                                                                 TipoAtribuicao = sup.TipoAtribuicao,
                                                                                 Dre = sup.DreId,
                                                                                 Ue = sup.UeId,
                                                                                 UsuarioRf = sup.SupervisorId
                                                                             })));

                var atribuicoesEsporadicas = await mediator.Send(new ObterAtribuicoesVigentesPorAnoQuery(DateTimeExtension.HorarioBrasilia().Year, DateTimeExtension.HorarioBrasilia().Date));
                if (atribuicoesEsporadicas.Any())
                    await mediator.Send(new SalvarCachePorValorObjetoCommand(NomeChaveCache.ATRIBUICOES_ESPORADICAS_ATIVAS, atribuicoesEsporadicas));
                return true;
            }
            catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand("Não foi possível executar consulta para geração de cache de atribuições de responsavel", LogNivel.Critico, LogContexto.AtribuicaoReponsavel, ex.Message));
                return false;
            }
        }

    }
}