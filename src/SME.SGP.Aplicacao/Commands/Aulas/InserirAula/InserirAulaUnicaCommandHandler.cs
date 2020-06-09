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
        private readonly IConfiguration configuration;
        private readonly IComandosWorkflowAprovacao comandosWorkflowAprovacao;

        public InserirAulaUnicaCommandHandler(IRepositorioAula repositorioAula,
                                              IMediator mediator,
                                              IConfiguration configuration,
                                              IComandosWorkflowAprovacao comandosWorkflowAprovacao)
        {
            this.repositorioAula = repositorioAula ?? throw new ArgumentNullException(nameof(repositorioAula));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.comandosWorkflowAprovacao = comandosWorkflowAprovacao ?? throw new ArgumentNullException(nameof(comandosWorkflowAprovacao));
        }

        public async Task<RetornoBaseDto> Handle(InserirAulaUnicaCommand request, CancellationToken cancellationToken)
        {
            var retorno = new RetornoBaseDto();

            var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(request.CodigoTurma));
            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());

            var aulasExistentes = await mediator.Send(new ObterAulasPorDataTurmaComponenteCurricularEProfessorQuery(request.DataAula, request.CodigoTurma, request.CodigoComponenteCurricular, usuarioLogado.CodigoRf));
            if (aulasExistentes != null && aulasExistentes.Any(c => c.TipoAula == request.TipoAula))
                throw new NegocioException("Já existe uma aula criada neste dia para este componente curricular");

            await AplicarValidacoes(request, turma, usuarioLogado, aulasExistentes);

            var aula = MapearEntidade(request);

            repositorioAula.Salvar(aula);
            
            ValidarAulasDeReposicao(request, turma, aulasExistentes, aula);

            retorno.Mensagens.Add("Aula cadastrada com sucesso.");
            return retorno;
        }

        private void ValidarAulasDeReposicao(InserirAulaUnicaCommand request, Turma turma, IEnumerable<AulaConsultaDto> aulasExistentes, Aula aula)
        {
            if (request.TipoAula == TipoAula.Reposicao)
            {
                var quantidadeDeAulasExistentes = aulasExistentes.Where(x => x.DataAula.Date == request.DataAula.Date).Sum(x => x.Quantidade);

                if (turma.AulasReposicaoPrecisamAprovacao(quantidadeDeAulasExistentes + request.Quantidade))
                {
                    var idWorkflow = PersistirWorkflowReposicaoAula(request, turma, aula);
                    aula.EnviarParaWorkflowDeAprovacao(idWorkflow);
                }
            }
        }

        private async Task AplicarValidacoes(InserirAulaUnicaCommand inserirAulaUnicaCommand, Turma turma, Usuario usuarioLogado, IEnumerable<AulaConsultaDto> aulasExistentes)
        {
            await ValidarComponentesDoProfessor(inserirAulaUnicaCommand, usuarioLogado);

            await ValidarSeEhDiaLetivo(inserirAulaUnicaCommand, turma);

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
                AulaCJ = inserirAulaUnicaCommand.Usuario.EhProfessorCj()
            };
        }

        private async Task ValidarGrade(InserirAulaUnicaCommand inserirAulaUnicaCommand, Usuario usuarioLogado, IEnumerable<AulaConsultaDto> aulasExistentes, Turma turma)
        {
            if (inserirAulaUnicaCommand.EhRegencia)
            {
                if (aulasExistentes != null && aulasExistentes.Any(c => c.TipoAula != TipoAula.Reposicao))
                {
                    if (turma.ModalidadeCodigo == Modalidade.EJA)
                        throw new NegocioException("Para regência de EJA só é permitido a criação de 5 aulas por dia.");
                    else throw new NegocioException("Para regência de classe só é permitido a criação de 1 (uma) aula por dia.");
                }
            }
            else
            {
                var gradeAulas = await mediator.Send(new ObterGradeAulasPorTurmaEProfessorQuery(inserirAulaUnicaCommand.CodigoTurma, inserirAulaUnicaCommand.CodigoComponenteCurricular, inserirAulaUnicaCommand.DataAula, usuarioLogado.CodigoRf, inserirAulaUnicaCommand.EhRegencia));
                var quantidadeAulasRestantes = gradeAulas == null ? int.MaxValue : gradeAulas.QuantidadeAulasRestante;

                if (gradeAulas != null) 
                {
                    if (quantidadeAulasRestantes < inserirAulaUnicaCommand.Quantidade)
                        throw new NegocioException("Quantidade de aulas superior ao limíte de aulas da grade.");
                    if (!gradeAulas.PodeEditar && (inserirAulaUnicaCommand.Quantidade != gradeAulas.QuantidadeAulasRestante))
                        throw new NegocioException("Quantidade de aulas não pode ser diferente do valor da grade curricular."); ;
                }
            }
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
            if (usuarioLogado.EhProfessorCj())
            {
                if (usuarioLogado.EhProfessorCj() && inserirAulaUnicaCommand.Quantidade > 2)
                    throw new NegocioException("Quantidade de aulas por dia/disciplina excedido.");

                var componentesCurricularesDoProfessorCJ = await mediator.Send(new ObterComponentesCurricularesDoProfessorCJNaTurmaQuery(usuarioLogado.Login));
                if (componentesCurricularesDoProfessorCJ == null || !componentesCurricularesDoProfessorCJ.Any(c => c.TurmaId == inserirAulaUnicaCommand.CodigoTurma && c.DisciplinaId == inserirAulaUnicaCommand.CodigoComponenteCurricular))
                {
                    throw new NegocioException("Você não pode criar aulas para essa Turma.");
                }
            }
            else
            {
                var componentesCurricularesDoProfessor = await mediator.Send(new ObterComponentesCurricularesDoProfessorNaTurmaQuery(inserirAulaUnicaCommand.CodigoTurma, usuarioLogado.Login, usuarioLogado.PerfilAtual));
                if (componentesCurricularesDoProfessor == null || !componentesCurricularesDoProfessor.Any(c => c.Codigo == inserirAulaUnicaCommand.CodigoComponenteCurricular))
                {
                    throw new NegocioException("Você não pode criar aulas para essa Turma.");
                }

                var usuarioPodePersistirTurmaNaData = await mediator.Send(new ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery(inserirAulaUnicaCommand.CodigoComponenteCurricular, inserirAulaUnicaCommand.CodigoTurma, inserirAulaUnicaCommand.DataAula, usuarioLogado));
                if (!usuarioPodePersistirTurmaNaData)
                    throw new NegocioException("Você não pode fazer alterações ou inclusões nesta turma, disciplina e data.");
            }
        }

        private long PersistirWorkflowReposicaoAula(InserirAulaUnicaCommand command, Turma turma, Aula aula)
        {
            var linkParaReposicaoAula = $"{configuration["UrlFrontEnd"]}calendario-escolar/calendario-professor/cadastro-aula/editar/:{aula.Id}/";

            var wfAprovacaoAula = new WorkflowAprovacaoDto()
            {
                Ano = command.DataAula.Year,
                NotificacaoCategoria = NotificacaoCategoria.Workflow_Aprovacao,
                EntidadeParaAprovarId = aula.Id,
                Tipo = WorkflowAprovacaoTipo.ReposicaoAula,
                UeId = command.CodigoUe,
                DreId = turma.Ue.Dre.CodigoDre,
                NotificacaoTitulo = $"Criação de Aula de Reposição na turma {turma.Nome}",
                NotificacaoTipo = NotificacaoTipo.Calendario,
                NotificacaoMensagem = $"Foram criadas {command.Quantidade} aula(s) de reposição de {command.NomeComponenteCurricular} na turma {turma.Nome} da {turma.Ue.Nome} ({turma.Ue.Dre.Nome}). Para que esta aula seja considerada válida você precisa aceitar esta notificação. Para visualizar a aula clique  <a href='{linkParaReposicaoAula}'>aqui</a>."
            };

            wfAprovacaoAula.AdicionarNivel(Cargo.CP);
            wfAprovacaoAula.AdicionarNivel(Cargo.Diretor);

            return comandosWorkflowAprovacao.Salvar(wfAprovacaoAula);
        }
    }
}
