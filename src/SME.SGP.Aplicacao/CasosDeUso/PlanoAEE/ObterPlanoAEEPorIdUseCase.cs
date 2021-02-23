using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPlanoAEEPorIdUseCase : AbstractUseCase, IObterPlanoAEEPorIdUseCase
    {
        private readonly IRepositorioPlanoAEE repositorioPlanoAEE;

        public ObterPlanoAEEPorIdUseCase(IMediator mediator, IRepositorioPlanoAEE repositorioPlanoAEE) : base(mediator)
        {
            this.repositorioPlanoAEE = repositorioPlanoAEE ?? throw new ArgumentNullException(nameof(repositorioPlanoAEE));
        }

        public async Task<PlanoAEEDto> Executar(long? planoAEEId)
        {
            var plano = new PlanoAEEDto();
            var respostasPlano = Enumerable.Empty<RespostaQuestaoDto>();

            if (planoAEEId.HasValue && planoAEEId > 0)
            {
                var entidadePlano = await repositorioPlanoAEE.ObterPlanoComTurmaPorId(planoAEEId.Value);
                var alunoPorTurmaResposta = await mediator.Send(new ObterAlunoPorCodigoEolQuery(entidadePlano.AlunoCodigo, entidadePlano.Turma.AnoLetivo));

                if (alunoPorTurmaResposta == null)
                    throw new NegocioException("Aluno não localizado");

                var aluno = new AlunoReduzidoDto()
                {
                    Nome = !string.IsNullOrEmpty(alunoPorTurmaResposta.NomeAluno) ? alunoPorTurmaResposta.NomeAluno : alunoPorTurmaResposta.NomeSocialAluno,
                    NumeroAlunoChamada = alunoPorTurmaResposta.NumeroAlunoChamada,
                    DataNascimento = alunoPorTurmaResposta.DataNascimento,
                    DataSituacao = alunoPorTurmaResposta.DataSituacao,
                    CodigoAluno = alunoPorTurmaResposta.CodigoAluno,
                    Situacao = alunoPorTurmaResposta.SituacaoMatricula,
                    TurmaEscola = await OberterNomeTurmaFormatado(alunoPorTurmaResposta.CodigoTurma.ToString()),
                    NomeResponsavel = alunoPorTurmaResposta.NomeResponsavel,
                    TipoResponsavel = alunoPorTurmaResposta.TipoResponsavel,
                    CelularResponsavel = alunoPorTurmaResposta.CelularResponsavel,
                    DataAtualizacaoContato = alunoPorTurmaResposta.DataAtualizacaoContato
                };


                plano.Id = planoAEEId.Value;
                plano.Auditoria = (AuditoriaDto)entidadePlano;
                plano.Versoes = await mediator.Send(new ObterVersoesPlanoAEEQuery(planoAEEId.Value));
                plano.Aluno = aluno;
                plano.Situacao = entidadePlano.Situacao;
                plano.SituacaoDescricao = entidadePlano.Situacao.Name();
                plano.Turma = new TurmaAnoDto()
                {
                    Id = entidadePlano.Turma.Id,
                    Codigo = entidadePlano.Turma.CodigoTurma,
                    AnoLetivo = entidadePlano.Turma.AnoLetivo
                };

                var ultimaVersaoId = plano.Versoes
                    .OrderByDescending(a => a.Numero)
                    .Select(a => a.Id)
                    .First();

                plano.UltimaVersaoNumero = plano.Versoes
                    .OrderByDescending(a => a.Numero)
                    .Select(a => a.Numero)
                    .First();

                respostasPlano = await mediator.Send(new ObterRespostasPlanoAEEPorVersaoQuery(ultimaVersaoId));
            }

            var questionarioId = await mediator.Send(new ObterQuestionarioPlanoAEEIdQuery());

            plano.Questoes = await mediator.Send(new ObterQuestoesPorQuestionarioPorIdQuery(questionarioId, questaoId =>
               respostasPlano.Where(c => c.QuestaoId == questaoId)));

            plano.QuestionarioId = questionarioId;

            return plano;
        }

        private async Task<string> OberterNomeTurmaFormatado(string turmaId)
        {
            var turmaNome = "";
            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(turmaId));

            if (turma != null)
                turmaNome = $"{turma.ModalidadeCodigo.ShortName()} - {turma.Nome}";

            return turmaNome;
        }
    }
}
