using MediatR;
using Sentry;
using SME.SGP.Aplicacao.Commands.Aulas.InserirAula;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.Aula;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class InserirAulaUseCase : IInserirAulaUseCase
    {
        public async Task<RetornoBaseDto> Executar(IMediator mediator, InserirAulaDto inserirAulaDto)
        {
            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());

            if (inserirAulaDto.RecorrenciaAula == RecorrenciaAula.AulaUnica)
            {
                return await mediator.Send(new InserirAulaUnicaCommand(usuarioLogado, inserirAulaDto.DataAula, inserirAulaDto.Quantidade, inserirAulaDto.CodigoTurma, inserirAulaDto.CodigoComponenteCurricular, inserirAulaDto.NomeComponenteCurricular, inserirAulaDto.TipoCalendarioId, inserirAulaDto.TipoAula, inserirAulaDto.CodigoUe, inserirAulaDto.EhRegencia));
            }
            else
            {
                try
                {
                    //TODO TESTAR TRATAMENTO DE ERRO COM ASYNC
                    //TODO IMPLEMENTAR HANDLER DE AULAS RECORRENTES
                    mediator.Enfileirar(new InserirAulaRecorrenteCommand());
                    return new RetornoBaseDto("Serão cadastradas aulas recorrentes, em breve você receberá uma notificação com o resultado do processamento.");
                }
                catch (Exception ex)
                {
                    SentrySdk.AddBreadcrumb("Criação de aulas recorrentes", "Hangfire");
                    SentrySdk.CaptureException(ex);
                }
                return new RetornoBaseDto("Ocorreu um erro ao solicitar a criação de aulas recorrentes, por favor tente novamente.");
            }
        }
    }
}
