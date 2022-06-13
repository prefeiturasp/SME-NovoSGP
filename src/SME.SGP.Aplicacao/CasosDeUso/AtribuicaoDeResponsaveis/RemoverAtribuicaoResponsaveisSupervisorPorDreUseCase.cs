﻿using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RemoverAtribuicaoResponsaveisSupervisorPorDreUseCase : IRemoverAtribuicaoResponsaveisSupervisorPorDreUseCase
    {
        private readonly IMediator mediator;
        public RemoverAtribuicaoResponsaveisSupervisorPorDreUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            try
            {
                var codigoDre = param.ObterObjetoMensagem<string>();
                var supervisoresEscolasDres = await mediator.Send(new ObterSupervisoresPorDreAsyncQuery(codigoDre, TipoResponsavelAtribuicao.SupervisorEscolar));

                if (supervisoresEscolasDres.Any())
                {
                    var supervisoresIds = supervisoresEscolasDres.GroupBy(a => a.SupervisorId).Select(a => a.Key);
                    var supervisores = await mediator.Send(new ObterSupervisorPorCodigoQuery(supervisoresIds.ToArray()));

                    if (supervisores != null && supervisores.Any())
                    {
                        await RemoverSupervisorSemAtribuicao(supervisoresEscolasDres, supervisores);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand("Não foi possível executar a remoção da atribuição de responsavel Supervisor por DRE", LogNivel.Critico, LogContexto.AtribuicaoReponsavel, ex.Message));
                return false;
            }
        }

        private async Task<bool> RemoverSupervisorSemAtribuicao(IEnumerable<SupervisorEscolasDreDto> supervisoresEscolasDres, IEnumerable<SupervisoresRetornoDto> supervisoresEol)
        {
            var responsavelSupervisor = supervisoresEscolasDres;

            if (supervisoresEol != null)
            {
                responsavelSupervisor = responsavelSupervisor
                    .Where(s => s.Tipo == (int)TipoResponsavelAtribuicao.SupervisorEscolar &&
                        !supervisoresEol.Select(e => e.CodigoRf)
                    .Contains(s.SupervisorId));
            }

            if (responsavelSupervisor != null && responsavelSupervisor.Any())
            {
                foreach (var supervisor in responsavelSupervisor)
                {
                    var supervisorEntidadeExclusao = MapearDtoParaEntidade(supervisor);
                    await mediator.Send(new RemoverAtribuicaoSupervisorCommand(supervisorEntidadeExclusao));
                }
                return true;
            }
            await mediator.Send(new SalvarLogViaRabbitCommand("Não foram encontrados responsáveis para atribuição no EOL", LogNivel.Critico, LogContexto.AtribuicaoReponsavel, "Responsável Supervisor"));
            return false;
        }
        private static SupervisorEscolaDre MapearDtoParaEntidade(SupervisorEscolasDreDto dto)
        {
            return new SupervisorEscolaDre()
            {
                DreId = dto.DreId,
                SupervisorId = dto.SupervisorId,
                EscolaId = dto.EscolaId,
                Id = dto.Id,
                Excluido = dto.Excluido,
                AlteradoEm = dto.AlteradoEm,
                AlteradoPor = dto.AlteradoPor,
                AlteradoRF = dto.AlteradoRF,
                CriadoEm = dto.CriadoEm,
                CriadoPor = dto.CriadoPor,
                CriadoRF = dto.CriadoRF,
                Tipo = dto.Tipo
            };
        }
    }
}