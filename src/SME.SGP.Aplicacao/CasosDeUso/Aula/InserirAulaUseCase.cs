using MediatR;
using SME.SGP.Aplicacao.Commands.Aulas.InserirAula;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class InserirAulaUseCase : AbstractUseCase, IInserirAulaUseCase
    {
        private readonly IPodeCadastrarAulaUseCase podeCadastrarAulaUseCase;
        public InserirAulaUseCase(IMediator mediator, IPodeCadastrarAulaUseCase podeCadastrarAulaUseCase) : base(mediator)
        {
            this.podeCadastrarAulaUseCase = podeCadastrarAulaUseCase ?? throw new ArgumentNullException(nameof(podeCadastrarAulaUseCase));
        }

        public async Task<RetornoBaseDto> Executar(PersistirAulaDto inserirAulaDto)
        {
            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());

            var retornoPodeCadastrarAula = await podeCadastrarAulaUseCase.Executar(new FiltroPodeCadastrarAulaDto(0,
                inserirAulaDto.CodigoTurma,
                inserirAulaDto.CodigoComponenteCurricular,
                inserirAulaDto.DataAula,
                inserirAulaDto.EhRegencia,
                inserirAulaDto.TipoAula));

            if (retornoPodeCadastrarAula.PodeCadastrarAula)
            {
                if (inserirAulaDto.RecorrenciaAula == RecorrenciaAula.AulaUnica)
                {
                    return await mediator.Send(new InserirAulaUnicaCommand(usuarioLogado,
                                                                           inserirAulaDto.DataAula,
                                                                           inserirAulaDto.Quantidade,
                                                                           inserirAulaDto.CodigoTurma,
                                                                           inserirAulaDto.CodigoComponenteCurricular,
                                                                           inserirAulaDto.NomeComponenteCurricular,
                                                                           inserirAulaDto.TipoCalendarioId,
                                                                           inserirAulaDto.TipoAula,
                                                                           inserirAulaDto.CodigoUe,
                                                                           inserirAulaDto.EhRegencia));
                }
                else
                {
                    try
                    {
                        await mediator.Send(new IncluirFilaInserirAulaRecorrenteCommand(usuarioLogado,
                                                                             inserirAulaDto.DataAula,
                                                                             inserirAulaDto.Quantidade,
                                                                             inserirAulaDto.CodigoTurma,
                                                                             inserirAulaDto.CodigoComponenteCurricular,
                                                                             inserirAulaDto.NomeComponenteCurricular,
                                                                             inserirAulaDto.TipoCalendarioId,
                                                                             inserirAulaDto.TipoAula,
                                                                             inserirAulaDto.CodigoUe,
                                                                             inserirAulaDto.EhRegencia,
                                                                             inserirAulaDto.RecorrenciaAula));

                        return new RetornoBaseDto("Serão cadastradas aulas recorrentes, em breve você receberá uma notificação com o resultado do processamento.");
                    }
                    catch (Exception ex)
                    {
                        await mediator.Send(new SalvarLogViaRabbitCommand("Criação de aulas recorrentes", LogNivel.Critico, LogContexto.Aula, ex.Message));                        
                    }
                    return new RetornoBaseDto("Ocorreu um erro ao solicitar a criação de aulas recorrentes, por favor tente novamente.");
                }
            }
            else
            {
                throw new NegocioException($"Não é possível cadastrar aula do tipo '{inserirAulaDto.TipoAula.Name()}' para o dia selecionado!");
            }
        }
    }
}
