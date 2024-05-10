using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AlterarAulaUseCase : AbstractUseCase, IAlterarAulaUseCase
    {
        public AlterarAulaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<RetornoBaseDto> Executar(PersistirAulaDto aulaDto)
        {
            var usuarioLogado = await mediator.Send(ObterUsuarioLogadoQuery.Instance);

            if (aulaDto.RecorrenciaAula == RecorrenciaAula.AulaUnica)
            {
                return await mediator.Send(new AlterarAulaUnicaCommand(usuarioLogado, aulaDto));
            }
            else
            {
                try
                {
                    await mediator.Send(new IncluirFilaAlteracaoAulaRecorrenteCommand(usuarioLogado, aulaDto));

                    return await Task.FromResult(new RetornoBaseDto("Serão alteradas aulas recorrentes, em breve você receberá uma notificação com o resultado do processamento."));
                }
                catch (Exception ex)
                {
                    await mediator.Send(new SalvarLogViaRabbitCommand($"Erro em alteração de aulas recorrentes", LogNivel.Critico, LogContexto.Aula, ex.Message));                    
                }
                return await Task.FromResult(new RetornoBaseDto("Ocorreu um erro ao solicitar a alteração de aulas recorrentes, por favor tente novamente."));
            }
        }
    }
}
