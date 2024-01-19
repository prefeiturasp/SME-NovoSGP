using MediatR;
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
                await ValidarPermissaoCJCadastroAulas(turma, request.DataAula, request.Usuario.CodigoRf);

            var aulasExistentes = await mediator
                .Send(new ObterAulasPorDataTurmaComponenteCurricularCJQuery(request.DataAula,
                                                                            request.CodigoTurma,
                                                                            request.CodigoComponenteCurricular,
                                                                            request.Usuario.EhProfessorCj()));

            if (request.Usuario.EhProfessorCj())
                ValidarAulasExistentes(aulasExistentes, request.TipoAula, request.Usuario.CodigoRf);

            await AplicarValidacoes(request, turma, request.Usuario, aulasExistentes);

            var aula = MapearEntidade(request);

            aula.Status = ObterStatusAprovadoDiretor(request.Usuario.PerfilAtual, aula.Status);
            aula.Id = await repositorioAula.SalvarAsync(aula);

            if (request.Usuario.PerfilAtual != Perfis.PERFIL_DIRETOR)
                await ValidarAulasDeReposicao(request,
                                              turma,
                                              aulasExistentes,
                                              aula,
                                              retorno.Mensagens,
                                              request.Usuario);

            await mediator.Send(new ExecutarExclusaoPendenciaProfessorComponenteSemAulaCommand(turma,
                                                                                               request.CodigoComponenteCurricular,
                                                                                               request.DataAula));

            retorno.Mensagens.Add("Aula cadastrada com sucesso.");

            return retorno;
        }

        private EntidadeStatus ObterStatusAprovadoDiretor(Guid perfilUsuario, EntidadeStatus statusAtual)
        {
            if (perfilUsuario == Perfis.PERFIL_DIRETOR)
                return EntidadeStatus.Aprovado;
            return statusAtual;
        }

        private void ValidarAulasExistentes(IEnumerable<AulaConsultaDto> aulasExistentes, TipoAula tipoAula, string codigoRf)
        {
            if (aulasExistentes.PossuiRegistros(c => c.TipoAula == tipoAula && c.ProfessorRf == codigoRf))
                throw new NegocioException("Já existe uma aula criada neste dia para este componente curricular");
        }

        private async Task ValidarPermissaoCJCadastroAulas(Turma turma, DateTime dataAula, string codigoRf)
        {
            var possuiAtribuicaoCJ = await mediator
                    .Send(new PossuiAtribuicaoCJPorDreUeETurmaQuery(turma.Ue.Dre.CodigoDre,
                    turma.Ue.CodigoUe,
                    turma.CodigoTurma,
                    codigoRf));

            if (possuiAtribuicaoCJ)
            {
                var atribuicoesEsporadica = await mediator
                .Send(new ObterAtribuicoesPorRFEAnoQuery(codigoRf,
                                                         false,
                                                         dataAula.Year,
                                                         turma.Ue.Dre.CodigoDre,
                                                         turma.Ue.CodigoUe));

                if (atribuicoesEsporadica.Any() && !atribuicoesEsporadica.Any(a => a.DataInicio <= dataAula.Date
                                                   && a.DataFim >= dataAula.Date
                                                   && a.DreId == turma.Ue.Dre.CodigoDre
                                                   && a.UeId == turma.Ue.CodigoUe))
                    throw new NegocioException("Você não possui permissão para cadastrar aulas neste período");
            }
        }

        private async Task ValidarAulasDeReposicao(InserirAulaUnicaCommand request,
                                                   Turma turma,
                                                   IEnumerable<AulaConsultaDto> aulasExistentes,
                                                   Aula aula,
                                                   List<string> mensagens,
                                                   Usuario usuarioLogado)
        {
            if (request.TipoAula == TipoAula.Reposicao)
            {
                var quantidadeDeAulasExistentes = aulasExistentes.Where(x => x.DataAula.Date == request.DataAula.Date)
                                                                 .Sum(x => x.Quantidade);

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

        private async Task AplicarValidacoes(InserirAulaUnicaCommand inserirAulaUnicaCommand,
                                             Turma turma,
                                             Usuario usuarioLogado,
                                             IEnumerable<AulaConsultaDto> aulasExistentes)
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

        private async Task ValidarGrade(InserirAulaUnicaCommand inserirAulaUnicaCommand,
                                        Usuario usuarioLogado,
                                        IEnumerable<AulaConsultaDto> aulasExistentes,
                                        Turma turma)
        {

            var codigosConsiderados = new List<long>() { inserirAulaUnicaCommand.CodigoComponenteCurricular };
            var retornoValidacao = await mediator.Send(new ValidarGradeAulaCommand(turma,
                                                                                   codigosConsiderados.ToArray(),
                                                                                   inserirAulaUnicaCommand.DataAula,
                                                                                   usuarioLogado,
                                                                                   inserirAulaUnicaCommand.Quantidade,
                                                                                   inserirAulaUnicaCommand.EhRegencia,
                                                                                   aulasExistentes));

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

        private async Task ValidarComponentesDoProfessor(InserirAulaUnicaCommand inserirAulaUnicaCommand, Usuario usuarioLogado, long? codigoTerritorioSaber = null)
        {
            var resultadoValidacao = await mediator
                .Send(new ValidarComponentesDoProfessorCommand(usuarioLogado,
                                                               inserirAulaUnicaCommand.CodigoTurma,
                                                               inserirAulaUnicaCommand.CodigoComponenteCurricular,
                                                               inserirAulaUnicaCommand.DataAula,
                                                               codigoTerritorioSaber));

            if (!resultadoValidacao.resultado)
                throw new NegocioException(resultadoValidacao.mensagem);
        }

        private async Task<long> PersistirWorkflowReposicaoAula(InserirAulaUnicaCommand command, Turma turma, Aula aula, Usuario usuarioLogado)
            => await mediator.Send(new InserirWorkflowReposicaoAulaCommand(command.DataAula.Year, aula, turma,
                                                                           command.NomeComponenteCurricular,
                                                                           usuarioLogado.PerfilAtual));
    }
}
