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

        public async Task<RetornoBaseDto> Executar(PersistirAulaDto param)
        {
            var usuarioLogado = await mediator.Send(ObterUsuarioLogadoQuery.Instance);

            if (param.RecorrenciaAula == RecorrenciaAula.AulaUnica)
            {
                return await mediator.Send(new AlterarAulaUnicaCommand(usuarioLogado,
                                                                       param.Id,
                                                                       param.DataAula,
                                                                       param.Quantidade,
                                                                       param.CodigoTurma,
                                                                       param.CodigoComponenteCurricular,
                                                                       param.NomeComponenteCurricular,
                                                                       param.TipoCalendarioId,
                                                                       param.TipoAula,
                                                                       param.CodigoUe,
                                                                       param.EhRegencia));
            }
            else
            {
                try
                {
                    await mediator.Send(new IncluirFilaAlteracaoAulaRecorrenteCommand(usuarioLogado,
                                                                         param.Id,
                                                                         param.DataAula,
                                                                         param.Quantidade,
                                                                         param.CodigoTurma,
                                                                         param.CodigoComponenteCurricular,
                                                                         param.NomeComponenteCurricular,
                                                                         param.TipoCalendarioId,
                                                                         param.TipoAula,
                                                                         param.CodigoUe,
                                                                         param.EhRegencia,
                                                                         param.RecorrenciaAula));
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
