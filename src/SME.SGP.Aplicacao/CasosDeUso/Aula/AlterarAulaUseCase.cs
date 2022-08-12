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
            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());

            if (aulaDto.RecorrenciaAula == RecorrenciaAula.AulaUnica)
            {
                return await mediator.Send(new AlterarAulaUnicaCommand(usuarioLogado,
                                                                       aulaDto.Id,
                                                                       aulaDto.DataAula,
                                                                       aulaDto.Quantidade,
                                                                       aulaDto.CodigoTurma,
                                                                       aulaDto.CodigoComponenteCurricular,
                                                                       aulaDto.NomeComponenteCurricular,
                                                                       aulaDto.TipoCalendarioId,
                                                                       aulaDto.TipoAula,
                                                                       aulaDto.CodigoUe,
                                                                       aulaDto.EhRegencia));
            }
            else
            {
                try
                {
                    await mediator.Send(new IncluirFilaAlteracaoAulaRecorrenteCommand(usuarioLogado,
                                                                         aulaDto.Id,
                                                                         aulaDto.DataAula,
                                                                         aulaDto.Quantidade,
                                                                         aulaDto.CodigoTurma,
                                                                         aulaDto.CodigoComponenteCurricular,
                                                                         aulaDto.NomeComponenteCurricular,
                                                                         aulaDto.TipoCalendarioId,
                                                                         aulaDto.TipoAula,
                                                                         aulaDto.CodigoUe,
                                                                         aulaDto.EhRegencia,
                                                                         aulaDto.RecorrenciaAula));
                    return new RetornoBaseDto("Serão alteradas aulas recorrentes, em breve você receberá uma notificação com o resultado do processamento.");
                }
                catch (Exception ex)
                {
                    await mediator.Send(new SalvarLogViaRabbitCommand($"Erro em alteração de aulas recorrentes", LogNivel.Critico, LogContexto.Aula, ex.Message));                    
                }
                return new RetornoBaseDto("Ocorreu um erro ao solicitar a alteração de aulas recorrentes, por favor tente novamente.");
            }
        }
    }
}
