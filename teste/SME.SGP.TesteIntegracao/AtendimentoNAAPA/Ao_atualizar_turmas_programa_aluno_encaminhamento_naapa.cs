using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.TesteIntegracao.AtendimentoNAAPA.ServicosFake;
using SME.SGP.TesteIntegracao.Estudante.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.AtendimentoNAAPA
{
    public class Ao_atualizar_turmas_programa_aluno_encaminhamento_naapa : AtendimentoNAAPATesteBase
    {
        private const string TURMA_PROGRAMA_ALUNO_NAAPA_DIFERENTE = "[{\"dreUe\":\"Ue/Dre 02\",\"turma\":\"Turma 10\",\"componenteCurricular\":\"0002\"}]";
        private const string TURMA_PROGRAMA_ALUNO_NAAPA_IGUAL = "[{\"dreUe\":\"Ue/Dre 01\",\"turma\":\"Turma 09\",\"componenteCurricular\":\"0001\"}]";
        private const string TURMA_PROGRAMA_ALUNO_EOL = "[{\"dreUe\":\"Ue/Dre 01\",\"turma\":\"Turma 09\",\"componenteCurricular\":\"0001\"}]";

        public Ao_atualizar_turmas_programa_aluno_encaminhamento_naapa(CollectionFixture collectionFixture) : base(collectionFixture)
        { }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTurmasAlunoPorFiltroQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterTurmasAlunoPorFiltroQueryHandler_TurmasProgramaEstudanteFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTurmasProgramaAlunoQuery, IEnumerable<AlunoTurmaProgramaDto>>), typeof(ObterTurmasProgramaAlunoQueryHandlerFakeNAAPA), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName = "Encaminhamento NAAPA - Execução worker de alteração de turmas de programa aluno com turma de programa alterada EOL")]
        public async Task Ao_encontrar_turmas_programa_diferente_eol_deve_alterar_resposta_naapa()
        {
            var filtroNAAPA = new FiltroNAAPADto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8",
                DreId = 1,
                CodigoUe = "1",
                TurmaId = TURMA_ID_1,
                Situacao = (int)SituacaoNAAPA.Rascunho,
                Prioridade = NORMAL
            };

            await CriarDadosBase(filtroNAAPA);
            var usecase = ObterServicoAtualizarTurmasProgramaDoEncaminhamentoNAAPA();
            await GerarDadosEncaminhamentoNAAPA(DateTimeExtension.HorarioBrasilia().Date, TURMA_PROGRAMA_ALUNO_NAAPA_DIFERENTE);

            var questoesQuestionarioNAAPA = ObterTodos<Dominio.Questao>().Where(questao => questao.NomeComponente == NOME_COMPONENTE_QUESTAO_TURMAS_PROGRAMA);
            questoesQuestionarioNAAPA.ShouldBeUnique("Qdade registros Questão [Turma programa] inválidos");

            var questoesEncaminhamentoNAAPA = ObterTodos<Dominio.QuestaoEncaminhamentoNAAPA>().Where(questao => questao.QuestaoId == questoesQuestionarioNAAPA.FirstOrDefault().Id);
            questoesEncaminhamentoNAAPA.ShouldBeUnique("Qdade registros Questão Encaminhamento NAAPA [Turma programa] inválidos");

            var respostasEncaminhamentoNAAPA = ObterTodos<Dominio.RespostaEncaminhamentoNAAPA>().Where(resposta => resposta.QuestaoEncaminhamentoId == questoesEncaminhamentoNAAPA.FirstOrDefault().Id);
            respostasEncaminhamentoNAAPA.ShouldBeUnique("Qdade registros Resposta Encaminhamento NAAPA [Turma programa] inválidos");
            var respostaEncaminhamentoNAAPA = respostasEncaminhamentoNAAPA.FirstOrDefault();
            respostaEncaminhamentoNAAPA.Texto.ShouldBe(TURMA_PROGRAMA_ALUNO_NAAPA_DIFERENTE);

            var encaminhamentoNAAPADto = new AtendimentoNAAPADto { Id = 1, AlunoCodigo = ALUNO_CODIGO_1, TurmaId = TURMA_ID_1, AlunoNome = "Nome do aluno 1"};
            var mensagem = new MensagemRabbit { Mensagem = JsonConvert.SerializeObject(encaminhamentoNAAPADto) };
            var retorno = await usecase.Executar(mensagem);

            retorno.ShouldBe(true);
            respostasEncaminhamentoNAAPA = ObterTodos<Dominio.RespostaEncaminhamentoNAAPA>().Where(resposta => resposta.QuestaoEncaminhamentoId == questoesEncaminhamentoNAAPA.FirstOrDefault().Id);
            respostasEncaminhamentoNAAPA.ShouldBeUnique("Qdade registros Resposta Encaminhamento NAAPA [Turma programa] inválidos");
            respostaEncaminhamentoNAAPA = respostasEncaminhamentoNAAPA.FirstOrDefault();
            respostaEncaminhamentoNAAPA.Texto.ShouldBe(TURMA_PROGRAMA_ALUNO_EOL);
        }

        [Fact(DisplayName = "Encaminhamento NAAPA - Execução worker de alteração de turmas de programa aluno com nova turma programa alterada EOL quando não existir resposta de turma programa no encaminhamento")]
        public async Task Ao_encontrar_turma_programa_diferente_eol_deve_incluir_resposta_naapa()
        {
            var filtroNAAPA = new FiltroNAAPADto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8",
                DreId = 1,
                CodigoUe = "1",
                TurmaId = TURMA_ID_1,
                Situacao = (int)SituacaoNAAPA.Rascunho,
                Prioridade = NORMAL
            };

            await CriarDadosBase(filtroNAAPA);
            var usecase = ObterServicoAtualizarTurmasProgramaDoEncaminhamentoNAAPA();
            await GerarDadosEncaminhamentoNAAPA(DateTimeExtension.HorarioBrasilia().Date, null);

            var questoesQuestionarioNAAPA = ObterTodos<Dominio.Questao>().Where(questao => questao.NomeComponente == NOME_COMPONENTE_QUESTAO_TURMAS_PROGRAMA);
            questoesQuestionarioNAAPA.ShouldBeUnique("Qdade registros Questão [Turma programa] inválidos");

            var questoesEncaminhamentoNAAPA = ObterTodos<Dominio.QuestaoEncaminhamentoNAAPA>().Where(questao => questao.QuestaoId == questoesQuestionarioNAAPA.FirstOrDefault().Id);
            questoesEncaminhamentoNAAPA.ShouldBeUnique("Qdade registros Questão Encaminhamento NAAPA [Turma programa] inválidos");

            var respostasEncaminhamentoNAAPA = ObterTodos<Dominio.RespostaEncaminhamentoNAAPA>().Where(resposta => resposta.QuestaoEncaminhamentoId == questoesEncaminhamentoNAAPA.FirstOrDefault().Id);
            respostasEncaminhamentoNAAPA.ShouldBeEmpty();

            var encaminhamentoNAAPADto = new AtendimentoNAAPADto { Id = 1, AlunoCodigo = ALUNO_CODIGO_1, TurmaId = TURMA_ID_1, AlunoNome = "Nome do aluno 1" };
            var mensagem = new MensagemRabbit { Mensagem = JsonConvert.SerializeObject(encaminhamentoNAAPADto) };
            var retorno = await usecase.Executar(mensagem);

            retorno.ShouldBe(true);
            respostasEncaminhamentoNAAPA = ObterTodos<Dominio.RespostaEncaminhamentoNAAPA>().Where(resposta => resposta.QuestaoEncaminhamentoId == questoesEncaminhamentoNAAPA.FirstOrDefault().Id);
            respostasEncaminhamentoNAAPA.ShouldBeUnique("Qdade registros Resposta Encaminhamento NAAPA [Turma programa] inválidos");
            var respostaEncaminhamentoNAAPA = respostasEncaminhamentoNAAPA.FirstOrDefault();
            respostaEncaminhamentoNAAPA.Texto.ShouldBe(TURMA_PROGRAMA_ALUNO_EOL);
        }

        [Fact(DisplayName = "Encaminhamento NAAPA - Execução worker de alteração de turmas de programa aluno com nova turma programa alterada EOL quando existir resposta de turma programa vazia no encaminhamento")]
        public async Task Ao_encontrar_turma_programa_diferente_eol_deve_alterar_resposta_vazia_naapa()
        {
            var filtroNAAPA = new FiltroNAAPADto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8",
                DreId = 1,
                CodigoUe = "1",
                TurmaId = TURMA_ID_1,
                Situacao = (int)SituacaoNAAPA.Rascunho,
                Prioridade = NORMAL
            };

            await CriarDadosBase(filtroNAAPA);
            var useCase = ObterServicoAtualizarTurmasProgramaDoEncaminhamentoNAAPA();
            await GerarDadosEncaminhamentoNAAPA(DateTimeExtension.HorarioBrasilia().Date, "");

            var questoesQuestionarioNAAPA = ObterTodos<Dominio.Questao>().Where(questao => questao.NomeComponente == NOME_COMPONENTE_QUESTAO_TURMAS_PROGRAMA);
            questoesQuestionarioNAAPA.ShouldBeUnique("Qdade registros Questão [Turma programa] inválidos");

            var questoesEncaminhamentoNAAPA = ObterTodos<Dominio.QuestaoEncaminhamentoNAAPA>().Where(questao => questao.QuestaoId == questoesQuestionarioNAAPA.FirstOrDefault().Id);
            questoesEncaminhamentoNAAPA.ShouldBeUnique("Qdade registros Questão Encaminhamento NAAPA [Turma programa] inválidos");

            var respostasEncaminhamentoNAAPA = ObterTodos<Dominio.RespostaEncaminhamentoNAAPA>().Where(resposta => resposta.QuestaoEncaminhamentoId == questoesEncaminhamentoNAAPA.FirstOrDefault().Id);
            respostasEncaminhamentoNAAPA.ShouldBeUnique("Qdade registros Resposta Encaminhamento NAAPA [Turma programa] inválidos");
            var respostaEncaminhamentoNAAPA = respostasEncaminhamentoNAAPA.FirstOrDefault();
            respostaEncaminhamentoNAAPA.Texto.ShouldBe("");

            var encaminhamentoNAAPADto = new AtendimentoNAAPADto { Id = 1, AlunoCodigo = ALUNO_CODIGO_1, TurmaId = TURMA_ID_1, AlunoNome = "Nome do aluno 1" };
            var mensagem = new MensagemRabbit { Mensagem = JsonConvert.SerializeObject(encaminhamentoNAAPADto) };
            var retorno = await useCase.Executar(mensagem);

            retorno.ShouldBe(true);
            respostasEncaminhamentoNAAPA = ObterTodos<Dominio.RespostaEncaminhamentoNAAPA>().Where(resposta => resposta.QuestaoEncaminhamentoId == questoesEncaminhamentoNAAPA.FirstOrDefault().Id);
            respostasEncaminhamentoNAAPA.ShouldBeUnique("Qdade registros Resposta Encaminhamento NAAPA [Turma programa] inválidos");
            respostaEncaminhamentoNAAPA = respostasEncaminhamentoNAAPA.FirstOrDefault();
            respostaEncaminhamentoNAAPA.Texto.ShouldBe(TURMA_PROGRAMA_ALUNO_EOL);
        }

        [Fact(DisplayName = "Encaminhamento NAAPA - Execução worker de alteração de turmas de programa aluno com turma de programa não alterada EOL")]
        public async Task Ao_encontrar_turma_programa_igual_eol_nao_deve_alterar_resposta_naapa()
        {
            var filtroNAAPA = new FiltroNAAPADto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8",
                DreId = 1,
                CodigoUe = "1",
                TurmaId = TURMA_ID_1,
                Situacao = (int)SituacaoNAAPA.Rascunho,
                Prioridade = NORMAL
            };

            await CriarDadosBase(filtroNAAPA);
            var useCase = ObterServicoAtualizarTurmasProgramaDoEncaminhamentoNAAPA();
            await GerarDadosEncaminhamentoNAAPA(DateTimeExtension.HorarioBrasilia().Date, TURMA_PROGRAMA_ALUNO_NAAPA_IGUAL);

            var questoesQuestionarioNAAPA = ObterTodos<Dominio.Questao>().Where(questao => questao.NomeComponente == NOME_COMPONENTE_QUESTAO_TURMAS_PROGRAMA);
            questoesQuestionarioNAAPA.ShouldBeUnique("Qdade registros Questão [Turma programa] inválidos");

            var questoesEncaminhamentoNAAPA = ObterTodos<Dominio.QuestaoEncaminhamentoNAAPA>().Where(questao => questao.QuestaoId == questoesQuestionarioNAAPA.FirstOrDefault().Id);
            questoesEncaminhamentoNAAPA.ShouldBeUnique("Qdade registros Questão Encaminhamento NAAPA [Turma programa] inválidos");

            var respostasEncaminhamentoNAAPA = ObterTodos<Dominio.RespostaEncaminhamentoNAAPA>().Where(resposta => resposta.QuestaoEncaminhamentoId == questoesEncaminhamentoNAAPA.FirstOrDefault().Id);
            respostasEncaminhamentoNAAPA.ShouldBeUnique("Qdade registros Resposta Encaminhamento NAAPA [Turma programa] inválidos");
            var respostaEncaminhamentoNAAPA = respostasEncaminhamentoNAAPA.FirstOrDefault();
            respostaEncaminhamentoNAAPA.Texto.ShouldBe(TURMA_PROGRAMA_ALUNO_NAAPA_IGUAL);

            var encaminhamentoNAAPADto = new AtendimentoNAAPADto { Id = 1, AlunoCodigo = ALUNO_CODIGO_1, TurmaId = TURMA_ID_1, AlunoNome = "Nome do aluno 1" };
            var mensagem = new MensagemRabbit { Mensagem = JsonConvert.SerializeObject(encaminhamentoNAAPADto) };
            var retorno = await useCase.Executar(mensagem);

            retorno.ShouldBe(false);
            respostasEncaminhamentoNAAPA = ObterTodos<Dominio.RespostaEncaminhamentoNAAPA>().Where(resposta => resposta.QuestaoEncaminhamentoId == questoesEncaminhamentoNAAPA.FirstOrDefault().Id);
            respostasEncaminhamentoNAAPA.ShouldBeUnique("Qdade registros Resposta Encaminhamento NAAPA [Turma programa] inválidos");
            respostaEncaminhamentoNAAPA = respostasEncaminhamentoNAAPA.FirstOrDefault();
            respostaEncaminhamentoNAAPA.Texto.ShouldBe(TURMA_PROGRAMA_ALUNO_EOL);
        }

        private async Task GerarDadosEncaminhamentoNAAPA(DateTime dataQueixa, string? turmasPrograma)
        {
            await CriarEncaminhamentoNAAPA();
            await CriarEncaminhamentoNAAPASecao();
            await CriarQuestoesEncaminhamentoNAAPA();
            await CriarRespostasEncaminhamentoNAAPA(dataQueixa, turmasPrograma);
        }

        private async Task CriarRespostasEncaminhamentoNAAPA(DateTime dataQueixa, string? turmasPrograma)
        {
            await InserirNaBase(new Dominio.RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = 1,
                Texto = dataQueixa.ToString("dd/MM/yyyy"),
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new Dominio.RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = 2,
                Texto = "1",
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            if (turmasPrograma.NaoEhNulo())
                await InserirNaBase(new Dominio.RespostaEncaminhamentoNAAPA()
                {
                    QuestaoEncaminhamentoId = 3,
                    Texto = turmasPrograma,
                    CriadoEm = DateTimeExtension.HorarioBrasilia(),
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF
                });
        }

        private async Task CriarQuestoesEncaminhamentoNAAPA()
        {
            //ID 01
            await InserirNaBase(new Dominio.QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 1,
                QuestaoId = 1,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            //ID 02
            await InserirNaBase(new Dominio.QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 1,
                QuestaoId = 2,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            //ID 03
            await InserirNaBase(new Dominio.QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 1,
                QuestaoId = ID_QUESTAO_TURMAS_PROGRAMA,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

        }

        private async Task CriarEncaminhamentoNAAPASecao()
        {
            await InserirNaBase(new Dominio.EncaminhamentoNAAPASecao()
            {
                EncaminhamentoNAAPAId = 1,
                SecaoEncaminhamentoNAAPAId = 1,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task CriarEncaminhamentoNAAPA()
        {
            await InserirNaBase(new Dominio.EncaminhamentoNAAPA()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                Situacao = SituacaoNAAPA.Rascunho,
                AlunoNome = "Nome do aluno 1",
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }
    }

}

