using System;
using System.Threading.Tasks;
using MediatR;
using Sentry;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ExcluirAulaUseCase : AbstractUseCase, IExcluirAulaUseCase
    {
        public ExcluirAulaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<RetornoBaseDto> Executar(ExcluirAulaDto aulaDto)
        {
            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());

            if (aulaDto.RecorrenciaAula == RecorrenciaAula.AulaUnica)
                return await mediator.Send(new ExcluirAulaUnicaCommand(usuarioLogado, aulaDto.AulaId, aulaDto.ComponenteCurricularNome));
            else
            {
                try
                {
                    // TODO Excluir Recorrencia
                    //mediator.Enfileirar(new AlterarAulaRecorrenteCommand(usuarioLogado, aulaDto.Id, aulaDto.DataAula, aulaDto.Quantidade, aulaDto.CodigoTurma, aulaDto.CodigoComponenteCurricular, aulaDto.NomeComponenteCurricular, aulaDto.TipoCalendarioId, aulaDto.TipoAula, aulaDto.CodigoUe, aulaDto.EhRegencia, aulaDto.RecorrenciaAula));
                    return new RetornoBaseDto("Serão excluidas aulas recorrentes, em breve você receberá uma notificação com o resultado do processamento.");
                }
                catch (Exception ex)
                {
                    SentrySdk.AddBreadcrumb("Exclusão de aulas recorrentes", "Hangfire");
                    SentrySdk.CaptureException(ex);
                }
                return new RetornoBaseDto("Ocorreu um erro ao solicitar a exclusão de aulas recorrentes, por favor tente novamente.");
            }
        }
    }
}
