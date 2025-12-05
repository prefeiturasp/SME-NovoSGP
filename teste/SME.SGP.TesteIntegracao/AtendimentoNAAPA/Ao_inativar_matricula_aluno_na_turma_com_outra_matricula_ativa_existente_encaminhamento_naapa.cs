using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.AtendimentoNAAPA.ServicosFake;
using SME.SGP.TesteIntegracao.AtendimentoNAAPA.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.AtendimentoNAAPA
{
    public class Ao_inativar_matricula_aluno_na_turma_com_outra_matricula_ativa_existente_encaminhamento_naapa : AtendimentoNAAPATesteBase
    {
        public Ao_inativar_matricula_aluno_na_turma_com_outra_matricula_ativa_existente_encaminhamento_naapa(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosEolPorCodigosQuery, IEnumerable<TurmasDoAlunoDto>>), typeof(ObterAlunosEolPorCodigosQueryHandlerFakeNAAPA_Desistente_ReclassificadoSaida), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterFuncionariosDreOuUePorPerfisQuery, IEnumerable<FuncionarioUnidadeDto>>), typeof(ObterFuncionariosDreOuUePorPerfisQueryHandlerFake), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName = "Encaminhamento NAAPA - Ao receber atualização de situação matrícula do aluno a turma para Inativo porém com outra matrícula ativa, deve atualizar situação matrícula no encaminhamento e não notificar responsáveis")]
        public async Task Deve_atualizar_situacao_matricula_encaminhamento_naapa_e_nao_notificar_CP_NAAPA()
        {
            var filtroNAAPA = new FiltroNAAPADto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "2",
                DreId = 1,
                CodigoUe = "1",
                TurmaId = TURMA_ID_1,
                Situacao = (int)SituacaoNAAPA.Rascunho,
                Prioridade = NORMAL,
                CriarTurmaPadrao = false
            };

            await CriarDadosBase(filtroNAAPA);
            await CriarTurma(filtroNAAPA.Modalidade);
            await CriarEncaminhamentoNAAPA(ALUNO_CODIGO_1);

            var useCase = ObterServicoNotificacaoAtualizacaoMatriculaAlunoDoEncaminhamentoNAAPA();

            var mensagem = new MensagemRabbit(JsonSerializer.Serialize(ObterEncaminhamentoDto(ALUNO_CODIGO_1, ALUNO_NOME_1)));

            var gerouNotificacoes = (await useCase.Executar(mensagem));
            gerouNotificacoes.ShouldBe(false);

            var encaminhamentoNAAPA = ObterTodos<Dominio.EncaminhamentoNAAPA>().FirstOrDefault();
            encaminhamentoNAAPA.SituacaoMatriculaAluno.ShouldBe(SituacaoMatriculaAluno.ReclassificadoSaida);
            var notificacoes = ObterTodos<Dominio.Notificacao>();
            notificacoes.ShouldBeEmpty();
        }


        private AtendimentoNAAPADto ObterEncaminhamentoDto(string alunoCodigo, string nomeAluno)
        {
            return new AtendimentoNAAPADto()
            {
                Id = 1,
                AlunoCodigo = alunoCodigo,
                AlunoNome = nomeAluno,
                TurmaId = TURMA_ID_1,
                SituacaoMatriculaAluno = SituacaoMatriculaAluno.Ativo
            };
        }

        private async Task CriarEncaminhamentoNAAPA(string alunoCodigo)
        {
            await InserirNaBase(new Dominio.EncaminhamentoNAAPA()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = alunoCodigo,
                Situacao = SituacaoNAAPA.Rascunho,
                AlunoNome = "Nome do aluno 1",
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                SituacaoMatriculaAluno = SituacaoMatriculaAluno.Ativo
            });
        }
    }
}
