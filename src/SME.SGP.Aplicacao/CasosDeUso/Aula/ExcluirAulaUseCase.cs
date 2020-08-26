using MediatR;
using Sentry;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirAulaUseCase : AbstractUseCase, IExcluirAulaUseCase
    {
        public ExcluirAulaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<RetornoBaseDto> Executar(ExcluirAulaDto excluirDto)
        {
            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());

            if (excluirDto.RecorrenciaAula == RecorrenciaAula.AulaUnica)
                return await mediator.Send(new ExcluirAulaUnicaCommand(usuarioLogado,
                                                                       excluirDto.AulaId,
                                                                       excluirDto.ComponenteCurricularNome));
            else
            {
                try
                {
                    // TODO alterar para fila do RabbitMQ
                    await mediator.Send(new IncluirFilaExclusaoAulaRecorrenteCommand(excluirDto.AulaId,
                                                                                     excluirDto.RecorrenciaAula,
                                                                                     excluirDto.ComponenteCurricularNome,
                                                                                     usuarioLogado));
                    return new RetornoBaseDto("Serão excluidas aulas recorrentes, em breve você receberá uma notificação com o resultado do processamento.");
                }
                catch (Exception ex)
                {
                    SentrySdk.AddBreadcrumb("Exclusão de aulas recorrentes", "RabbitMQ");
                    SentrySdk.CaptureException(ex);
                }
                return new RetornoBaseDto("Ocorreu um erro ao solicitar a exclusão de aulas recorrentes, por favor tente novamente.");
            }
        }
    }
}
