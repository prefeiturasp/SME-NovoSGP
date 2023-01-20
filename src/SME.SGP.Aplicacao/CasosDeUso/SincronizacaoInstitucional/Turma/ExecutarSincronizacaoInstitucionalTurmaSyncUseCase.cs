﻿using MediatR;
using Newtonsoft.Json;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutarSincronizacaoInstitucionalTurmaSyncUseCase : AbstractUseCase, IExecutarSincronizacaoInstitucionalTurmaSyncUseCase
    {
        public ExecutarSincronizacaoInstitucionalTurmaSyncUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var ueId = mensagemRabbit.Mensagem.ToString();

            if (string.IsNullOrEmpty(ueId))
                await mediator.Send(new SalvarLogViaRabbitCommand($"Não foi possível iniciar a sincronização das turmas. O codígo da Ue não foi informado", LogNivel.Negocio, LogContexto.SincronizacaoInstitucional));
            
            var anosComTurmasVigentes = await mediator
                .Send(new ObterAnoLetivoTurmasVigentesQuery(ueId));

            var codigosTurma = await mediator.Send(new ObterCodigosTurmasEOLPorUeIdParaSyncEstruturaInstitucionalQuery(ueId, anosComTurmasVigentes.ToArray()));

            if (!codigosTurma?.Any() ?? true) return true;

            foreach (var codigoTurma in codigosTurma)
            {
                try
                {
                    var mensagemSyncTurma = new MensagemSyncTurmaDto(ueId, codigoTurma);

                    var mensagemParaPublicar = JsonConvert.SerializeObject(mensagemSyncTurma);

                    var publicarFilaIncluirTurma = await mediator
                        .Send(new PublicarFilaSgpCommand(RotasRabbitSgpInstitucional.SincronizaEstruturaInstitucionalTurmaTratar, mensagemParaPublicar, mensagemRabbit.CodigoCorrelacao, null));

                    if (!publicarFilaIncluirTurma)
                        await mediator.Send(new SalvarLogViaRabbitCommand($"Não foi possível inserir a turma de codígo : {codigoTurma} na fila de inclusão.", LogNivel.Negocio, LogContexto.SincronizacaoInstitucional));
                }
                catch (Exception)
                {
                    await mediator.Send(new SalvarLogViaRabbitCommand($"Ocorreu um erro ao sincronizar a turma de código {codigosTurma}.", LogNivel.Critico, LogContexto.SincronizacaoInstitucional));
                }
            }
            return true;
        }
    }
}
