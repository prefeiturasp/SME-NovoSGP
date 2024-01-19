using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.Aulas.AlterarAulaUnica
{
    public class AlterarAulaUnicaCommandHandler : IRequestHandler<AlterarAulaUnicaCommand, RetornoBaseDto>
    {
        private readonly IRepositorioAula repositorioAula;
        private readonly IMediator mediator;

        public AlterarAulaUnicaCommandHandler(IRepositorioAula repositorioAula,
                                              IMediator mediator)
        {
            this.repositorioAula = repositorioAula ?? throw new ArgumentNullException(nameof(repositorioAula));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<RetornoBaseDto> Handle(AlterarAulaUnicaCommand request, CancellationToken cancellationToken)
        {
            var retorno = new RetornoBaseDto();
            var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(request.CodigoTurma));

            var professorConsiderado = !request.Usuario.EhProfessor() ?
                                       string.Empty : request.Usuario.Login;

            var codigosComponentesConsiderados = new List<long>() { request.ComponenteCurricularCodigo };

            var aulasExistentes = await mediator
                .Send(new ObterAulasPorDataTurmaComponenteCurricularEProfessorQuery(request.DataAula, request.CodigoTurma, codigosComponentesConsiderados.ToArray()));

            if (aulasExistentes.NaoEhNulo() && aulasExistentes.Any())
            {
                // Exclui a aula em alteração da lista
                aulasExistentes = aulasExistentes.Where(a => a.Id != request.Id);
                var existeAula = VerificaSeHaAulasExistentes(request.Usuario.EhProfessorCj(), request.Usuario.Login, request.TipoAula, aulasExistentes);
                if (existeAula)
                    throw new NegocioException("Já existe uma aula criada neste dia para este componente curricular");
            }

            var aula = await mediator.Send(new ObterAulaPorIdQuery(request.Id));

            var aulaAnteriorQnt = aula.Quantidade;

            await AplicarValidacoes(request, aula, turma, request.Usuario, aulasExistentes);

            MapearEntidade(aula, request, professorConsiderado);

            await ValidarAulasDeReposicao(request, turma, aulasExistentes, aula, retorno.Mensagens);

            repositorioAula.Salvar(aula);

            await TrataAlteracaoDeFrequencia(request.Usuario, aula, aulaAnteriorQnt);

            retorno.Mensagens.Add("Aula alterada com sucesso.");

            return retorno;
        }

        private bool VerificaSeHaAulasExistentes(bool ehProfessorCj, string login, TipoAula tipoAula, IEnumerable<AulaConsultaDto> aulasExistentes)
        {
            return !ehProfessorCj ? aulasExistentes.Any(c => c.TipoAula == tipoAula && c.AulaCJ == ehProfessorCj) :
                                    aulasExistentes.Any(c => c.TipoAula == tipoAula && c.AulaCJ == ehProfessorCj && c.CriadoRF == login);
        }

        private async Task TrataAlteracaoDeFrequencia(Usuario usuarioLogado, Aula aula, int aulaAnteriorQnt)
        {
            var trataFrequenciaAulaModificada = new AulaAlterarFrequenciaRequestDto(aula.Id, aulaAnteriorQnt);

            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpAula.RotaAlterarAulaFrequenciaTratar, trataFrequenciaAulaModificada, Guid.NewGuid(), usuarioLogado));
        }

        private async Task ValidarAulasDeReposicao(AlterarAulaUnicaCommand request, Turma turma, IEnumerable<AulaConsultaDto> aulasExistentes, Aula aula, List<string> mensagens)
        {
            if (request.TipoAula == TipoAula.Reposicao)
            {
                var quantidadeDeAulasExistentes = aulasExistentes.Where(x => x.DataAula.Date == request.DataAula.Date).Sum(x => x.Quantidade);

                if (turma.AulasReposicaoPrecisamAprovacao(quantidadeDeAulasExistentes + request.Quantidade))
                {
                    var idWorkflow = await PersistirWorkflowReposicaoAula(request, turma, aula);
                    aula.EnviarParaWorkflowDeAprovacao(idWorkflow);

                    mensagens.Add("Aula enviada para aprovação do workflow");
                }
            }
        }

        private void MapearEntidade(Aula aula, AlterarAulaUnicaCommand request, string professor)
        {
            aula.DataAula = request.DataAula;
            aula.Quantidade = request.Quantidade;
            aula.RecorrenciaAula = RecorrenciaAula.AulaUnica;
            aula.AulaPaiId = null;
            aula.DisciplinaId = request.ComponenteCurricularCodigo.ToString();

            if (!String.IsNullOrEmpty(professor))
                aula.ProfessorRf = professor;
        }

        private async Task AplicarValidacoes(AlterarAulaUnicaCommand request, Aula aula, Turma turma, Usuario usuarioLogado, IEnumerable<AulaConsultaDto> aulasExistentes)
        {
            if (!usuarioLogado.EhGestorEscolar())
                await ValidarComponentesDoProfessor(request, usuarioLogado);

            await ValidarSeEhDiaLetivo(request, turma);

            if (request.Quantidade > aula.Quantidade)
            {
                // Valida a diferença do numero de aulas pois a validação de grade já conta com a aula existente
                await ValidarGrade(request, usuarioLogado, aulasExistentes, turma, request.Quantidade - aula.Quantidade);
            }
        }

        private async Task ValidarGrade(AlterarAulaUnicaCommand request, Usuario usuarioLogado, IEnumerable<AulaConsultaDto> aulasExistentes, Turma turma, int quantidadeAdicional)
        {
            var retornoValidacao = await mediator.Send(new ValidarGradeAulaCommand(turma,
                                                                                   new long[] { request.ComponenteCurricularCodigo },
                                                                                   request.DataAula,
                                                                                   usuarioLogado,
                                                                                   quantidadeAdicional,
                                                                                   request.EhRegencia,
                                                                                   aulasExistentes));

            if (!retornoValidacao.resultado)
                throw new NegocioException(retornoValidacao.mensagem);
        }

        private async Task ValidarSeEhDiaLetivo(AlterarAulaUnicaCommand request, Turma turma)
        {
            var consultaPodeCadastrarAula = await mediator.Send(new ObterPodeCadastrarAulaPorDataQuery()
            {
                UeCodigo = turma.Ue.CodigoUe,
                DreCodigo = turma.Ue.Dre.CodigoDre,
                TipoCalendarioId = request.TipoCalendarioId,
                DataAula = request.DataAula,
                Turma = turma
            });

            if (!consultaPodeCadastrarAula.PodeCadastrar)
                throw new NegocioException(consultaPodeCadastrarAula.MensagemPeriodo);
        }

        private async Task ValidarComponentesDoProfessor(AlterarAulaUnicaCommand request, Usuario usuarioLogado)
        {
            var resultadoValidacao = await mediator.Send(new ValidarComponentesDoProfessorCommand(usuarioLogado, request.CodigoTurma, request.ComponenteCurricularCodigo, request.DataAula));
            if (!resultadoValidacao.resultado)
                throw new NegocioException(resultadoValidacao.mensagem);
        }

        private async Task<long> PersistirWorkflowReposicaoAula(AlterarAulaUnicaCommand request, Turma turma, Aula aula)
            => await mediator.Send(new InserirWorkflowReposicaoAulaCommand(request.DataAula.Year, aula, turma, request.ComponenteCurricularNome));
    }
}
