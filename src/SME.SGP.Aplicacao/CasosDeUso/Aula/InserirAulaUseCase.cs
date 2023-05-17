using MediatR;
using SME.SGP.Aplicacao.Commands.Aulas.InserirAula;
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

            if (inserirAulaDto.TipoAula == TipoAula.Reposicao && inserirAulaDto.RecorrenciaAula != RecorrenciaAula.AulaUnica)
                throw new NegocioException("Não é possível cadastrar aula de reposição com recorrência!");

            var codigosTerritorioEquivalentes = await mediator
                .Send(new ObterCodigosComponentesCurricularesTerritorioSaberEquivalentesPorTurmaQuery(inserirAulaDto.CodigoComponenteCurricular, inserirAulaDto.CodigoTurma, usuarioLogado.EhProfessor() ? usuarioLogado.Login : null));

            var codigosComponentesConsiderados = new List<long>() { inserirAulaDto.CodigoComponenteCurricular };

            if (codigosTerritorioEquivalentes != default)
                codigosComponentesConsiderados.AddRange(codigosTerritorioEquivalentes.Select(c => long.Parse(c.codigoComponente)).Except(codigosComponentesConsiderados));

            var retornoPodeCadastrarAula = await podeCadastrarAulaUseCase
                .Executar(new FiltroPodeCadastrarAulaDto(0, inserirAulaDto.CodigoTurma,
                                                         codigosComponentesConsiderados.ToArray(),
                                                         inserirAulaDto.DataAula,
                                                         inserirAulaDto.EhRegencia,
                                                         inserirAulaDto.TipoAula));

            var professorConsiderado = codigosTerritorioEquivalentes != default && usuarioLogado.EhGestorEscolar() ?
                                       codigosTerritorioEquivalentes.First().professor : usuarioLogado.Login;

            // Para inserção considera o código extenso de território se for o caso
            var codigoComponente = codigosComponentesConsiderados.OrderBy(c => c).Last();
            // Caso tenha território, considera o código curto para verificações
            var codigoTerritorioEquivalente = codigosComponentesConsiderados.Count > 1 ? codigosComponentesConsiderados.OrderBy(c => c).First() : (long?)null;

            if (retornoPodeCadastrarAula.PodeCadastrarAula)
            {
                if (inserirAulaDto.RecorrenciaAula == RecorrenciaAula.AulaUnica)
                {
                    return await mediator.Send(new InserirAulaUnicaCommand(usuarioLogado,
                                                                           inserirAulaDto.DataAula,
                                                                           inserirAulaDto.Quantidade,
                                                                           inserirAulaDto.CodigoTurma,
                                                                           codigoComponente, 
                                                                           inserirAulaDto.NomeComponenteCurricular,
                                                                           inserirAulaDto.TipoCalendarioId,
                                                                           inserirAulaDto.TipoAula,
                                                                           inserirAulaDto.CodigoUe,
                                                                           inserirAulaDto.EhRegencia,
                                                                           codigoTerritorioEquivalente,
                                                                           professorConsiderado));
                }
                else
                {
                    string mensagemDeExcecao;
                    try
                    {
                        await mediator.Send(new IncluirFilaInserirAulaRecorrenteCommand(usuarioLogado,
                                                                                        inserirAulaDto.DataAula,
                                                                                        inserirAulaDto.Quantidade,
                                                                                        inserirAulaDto.CodigoTurma,
                                                                                        codigoComponente,
                                                                                        inserirAulaDto.NomeComponenteCurricular,
                                                                                        inserirAulaDto.TipoCalendarioId,
                                                                                        inserirAulaDto.TipoAula,
                                                                                        inserirAulaDto.CodigoUe,
                                                                                        inserirAulaDto.EhRegencia,
                                                                                        inserirAulaDto.RecorrenciaAula,
                                                                                        codigoTerritorioEquivalente));

                        return await Task.FromResult(new RetornoBaseDto("Serão cadastradas aulas recorrentes, em breve você receberá uma notificação com o resultado do processamento."));
                    }
                    catch (Exception ex)
                    {
                        mensagemDeExcecao = ex.Message;
                        await mediator.Send(new SalvarLogViaRabbitCommand("Criação de aulas recorrentes", LogNivel.Critico, LogContexto.Aula, ex.Message));
                    }
                    return await Task.FromResult(new RetornoBaseDto($"Ocorreu um erro ao solicitar a criação de aulas recorrentes, por favor tente novamente. Detalhes: {mensagemDeExcecao}"));
                }
            }
            else
                throw new NegocioException($"Não é possível cadastrar aula do tipo '{inserirAulaDto.TipoAula.Name()}' para o dia selecionado!");
        }
    }
}
