using MediatR;
using Microsoft.Extensions.Configuration;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.Aulas.InserirAula
{
    public class InserirAulaUnicaCommandHandler : IRequestHandler<InserirAulaUnicaCommand, RetornoBaseDto>
    {
        private readonly IRepositorioAula repositorioAula;
        private readonly IMediator mediator;

        public InserirAulaUnicaCommandHandler(IRepositorioAula repositorioAula,
                                              IMediator mediator)
        {
            this.repositorioAula = repositorioAula ?? throw new ArgumentNullException(nameof(repositorioAula));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<RetornoBaseDto> Handle(InserirAulaUnicaCommand request, CancellationToken cancellationToken)
        {
            var retorno = new RetornoBaseDto();

            var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(request.CodigoTurma));

            if (request.Usuario.EhProfessorCj())
            {
                var possuiAtribuicaoCJ = await mediator.Send(new PossuiAtribuicaoCJPorDreUeETurmaQuery(turma.Ue.Dre.CodigoDre, turma.Ue.CodigoUe, turma.CodigoTurma, request.Usuario.CodigoRf));
                var atribuicoesEsporadica = await mediator.Send(new ObterAtribuicoesPorRFEAnoQuery(request.Usuario.CodigoRf, false, request.DataAula.Year, turma.Ue.Dre.CodigoDre, turma.Ue.CodigoUe));

                if (possuiAtribuicaoCJ && atribuicoesEsporadica.Any())
                {
                    if (!atribuicoesEsporadica.Where(a => a.DataInicio <= request.DataAula.Date && a.DataFim >= request.DataAula.Date && a.DreId == turma.Ue.Dre.CodigoDre && a.UeId == turma.Ue.CodigoUe).Any())
                        throw new NegocioException("Você não possui permissão para cadastrar aulas neste período");

                }                
            }

            var aulasExistentes = await mediator.Send(new ObterAulasPorDataTurmaComponenteCurricularQuery(request.DataAula, request.CodigoTurma, request.CodigoComponenteCurricular, request.Usuario.EhProfessorCj()));
            if (aulasExistentes != null && aulasExistentes.Any(c => c.TipoAula == request.TipoAula))
            {
                if (request.Usuario.EhProfessorCj())
                {
                    if (aulasExistentes.Any(a => a.ProfessorRf == request.Usuario.CodigoRf))
                        throw new NegocioException("Já existe uma aula criada neste dia para este componente curricular");
                }
                else
                    throw new NegocioException("Já existe uma aula criada neste dia para este componente curricular");
            }

            await AplicarValidacoes(request, turma, request.Usuario, aulasExistentes);

            var aula = MapearEntidade(request);

            if (request.Usuario.PerfilAtual == Perfis.PERFIL_DIRETOR)
                aula.Status = EntidadeStatus.Aprovado;

            aula.Id = await repositorioAula.SalvarAsync(aula);

            if (request.Usuario.PerfilAtual != Perfis.PERFIL_DIRETOR)
                await ValidarAulasDeReposicao(request, turma, aulasExistentes, aula, retorno.Mensagens, request.Usuario);

            retorno.Mensagens.Add("Aula cadastrada com sucesso.");
            return retorno;
        }

        private async Task ValidarAulasDeReposicao(InserirAulaUnicaCommand request, Turma turma, IEnumerable<AulaConsultaDto> aulasExistentes, Aula aula, List<string> mensagens, Usuario usuarioLogado)
        {
            if (request.TipoAula == TipoAula.Reposicao)
            {
                var quantidadeDeAulasExistentes = aulasExistentes.Where(x => x.DataAula.Date == request.DataAula.Date).Sum(x => x.Quantidade);

                if (AulasReposicaoPrecisamAprovacao(quantidadeDeAulasExistentes + request.Quantidade, request.EhRegencia))
                {
                    var idWorkflow = await PersistirWorkflowReposicaoAula(request, turma, aula, usuarioLogado);
                    aula.EnviarParaWorkflowDeAprovacao(idWorkflow);
                    await repositorioAula.SalvarAsync(aula);

                    mensagens.Add("Aula cadastrada e enviada para aprovação com sucesso.");
                }
            }
        }

        public bool AulasReposicaoPrecisamAprovacao(int qtdAulas, bool ehRegencia)
        {
            return qtdAulas >= 3 || (ehRegencia && qtdAulas >= 2);
        }

        private async Task AplicarValidacoes(InserirAulaUnicaCommand inserirAulaUnicaCommand, Turma turma, Usuario usuarioLogado, IEnumerable<AulaConsultaDto> aulasExistentes)
        {
            await ValidarComponentesDoProfessor(inserirAulaUnicaCommand, usuarioLogado);

            await ValidarSeEhDiaLetivo(inserirAulaUnicaCommand, turma);

            if (inserirAulaUnicaCommand.TipoAula != TipoAula.Reposicao)
                await ValidarGrade(inserirAulaUnicaCommand, usuarioLogado, aulasExistentes, turma);
        }

        private Aula MapearEntidade(InserirAulaUnicaCommand inserirAulaUnicaCommand)
        {
            return new Aula
            {
                ProfessorRf = inserirAulaUnicaCommand.Usuario.CodigoRf,
                UeId = inserirAulaUnicaCommand.CodigoUe,
                DisciplinaId = inserirAulaUnicaCommand.CodigoComponenteCurricular.ToString(),
                DisciplinaNome = inserirAulaUnicaCommand.NomeComponenteCurricular,
                TurmaId = inserirAulaUnicaCommand.CodigoTurma,
                TipoCalendarioId = inserirAulaUnicaCommand.TipoCalendarioId,
                DataAula = inserirAulaUnicaCommand.DataAula,
                Quantidade = inserirAulaUnicaCommand.Quantidade,
                TipoAula = inserirAulaUnicaCommand.TipoAula,
                RecorrenciaAula = RecorrenciaAula.AulaUnica,
                AulaCJ = inserirAulaUnicaCommand.Usuario.EhProfessorCj()
            };
        }

        private async Task ValidarGrade(InserirAulaUnicaCommand inserirAulaUnicaCommand, Usuario usuarioLogado, IEnumerable<AulaConsultaDto> aulasExistentes, Turma turma)
        {
            var retornoValidacao = await mediator.Send(new ValidarGradeAulaCommand(turma.CodigoTurma,
                                                                                   turma.ModalidadeCodigo,
                                                                                   inserirAulaUnicaCommand.CodigoComponenteCurricular,
                                                                                   inserirAulaUnicaCommand.DataAula,
                                                                                   usuarioLogado.CodigoRf,
                                                                                   inserirAulaUnicaCommand.Quantidade,
                                                                                   inserirAulaUnicaCommand.EhRegencia,
                                                                                   aulasExistentes, usuarioLogado.EhGestorEscolar()));

            if (!retornoValidacao.resultado)
                throw new NegocioException(retornoValidacao.mensagem);
        }

        private async Task ValidarSeEhDiaLetivo(InserirAulaUnicaCommand inserirAulaUnicaCommand, Turma turma)
        {
            var consultaPodeCadastrarAula = await mediator.Send(new ObterPodeCadastrarAulaPorDataQuery()
            {
                UeCodigo = turma.Ue.CodigoUe,
                DreCodigo = turma.Ue.Dre.CodigoDre,
                TipoCalendarioId = inserirAulaUnicaCommand.TipoCalendarioId,
                DataAula = inserirAulaUnicaCommand.DataAula,
                Turma = turma
            });

            if (!consultaPodeCadastrarAula.PodeCadastrar)
                throw new NegocioException(consultaPodeCadastrarAula.MensagemPeriodo);
        }

        private async Task ValidarComponentesDoProfessor(InserirAulaUnicaCommand inserirAulaUnicaCommand, Usuario usuarioLogado)
        {
            var resultadoValidacao = await mediator.Send(new ValidarComponentesDoProfessorCommand(usuarioLogado, inserirAulaUnicaCommand.CodigoTurma, inserirAulaUnicaCommand.CodigoComponenteCurricular, inserirAulaUnicaCommand.DataAula));
            if (!resultadoValidacao.resultado)
                throw new NegocioException(resultadoValidacao.mensagem);
        }

        private async Task<long> PersistirWorkflowReposicaoAula(InserirAulaUnicaCommand command, Turma turma, Aula aula, Usuario usuarioLogado)
            => await mediator.Send(new InserirWorkflowReposicaoAulaCommand(command.DataAula.Year,
                                                                           aula.Id,
                                                                           aula.Quantidade,
                                                                           turma.Ue.Dre.CodigoDre,
                                                                           turma.Ue.Dre.Nome,
                                                                           turma.Ue.CodigoUe,
                                                                           turma.Ue.Nome,
                                                                           turma.Nome,
                                                                           command.NomeComponenteCurricular,
                                                                           usuarioLogado.PerfilAtual));
    }
}
