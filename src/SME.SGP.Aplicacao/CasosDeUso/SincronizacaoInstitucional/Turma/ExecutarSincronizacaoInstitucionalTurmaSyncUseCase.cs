using MediatR;
using Newtonsoft.Json;
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

            //Verifica para o ano atual
            if (!anosComTurmasVigentes.Contains(DateTimeExtension.HorarioBrasilia().Year))
                anosComTurmasVigentes = anosComTurmasVigentes.Concat(new int[] { DateTimeExtension.HorarioBrasilia().Year });

            //É necessário incluir o ano anterior para verificação de turmas históricas que foram extintas posteriormente
            if (!anosComTurmasVigentes.Contains(DateTimeExtension.HorarioBrasilia().Year - 1))
                anosComTurmasVigentes = anosComTurmasVigentes.Concat(new int[] { DateTimeExtension.HorarioBrasilia().Year - 1 });

            var codigosTurma = await mediator
                .Send(new ObterCodigosTurmasEOLPorUeIdParaSyncEstruturaInstitucionalQuery(ueId, anosComTurmasVigentes.ToArray()));

            if (!codigosTurma?.Any() ?? true) 
                return true;

            var usuarioSistema = await mediator.Send(new ObterUsuarioPorRfQuery("Sistema"));

            if (codigosTurma == null)
                codigosTurma = new List<long>();

            foreach (var codigoTurma in codigosTurma)
            {
                try
                {
                    var mensagemSyncTurma = new MensagemSyncTurmaDto(ueId, codigoTurma);
                    var mensagemParaPublicar = JsonConvert.SerializeObject(mensagemSyncTurma);

                    var publicarFilaIncluirTurma = await mediator
                        .Send(new PublicarFilaSgpCommand(RotasRabbitSgpInstitucional.SincronizaEstruturaInstitucionalTurmaTratar, mensagemParaPublicar, mensagemRabbit.CodigoCorrelacao, usuarioSistema));

                    if (!publicarFilaIncluirTurma)
                        await mediator.Send(new SalvarLogViaRabbitCommand($"Não foi possível inserir a turma de código : {codigoTurma} na fila de inclusão.", LogNivel.Negocio, LogContexto.SincronizacaoInstitucional));
                }
                catch (Exception ex)
                {
                    await mediator.Send(new SalvarLogViaRabbitCommand($"Ocorreu um erro ao sincronizar a turma de código {codigoTurma}.", LogNivel.Critico, LogContexto.SincronizacaoInstitucional,
                                                                      ex.Message, rastreamento: ex.StackTrace, excecaoInterna: ex.InnerException?.ToString()));
                }
            }
            return true;
        }
    }
}
