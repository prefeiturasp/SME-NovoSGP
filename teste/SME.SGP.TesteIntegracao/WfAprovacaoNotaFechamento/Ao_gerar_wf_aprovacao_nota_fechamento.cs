using Shouldly;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao
{
    public class Ao_gerar_wf_aprovacao_nota_fechamento : TesteBase
    {
        private const string DRE_NOME = "DIRETORIA REGIONAL DE EDUCACAO JACANA/TREMEMBE";
        private const string DRE_CODIGO = "108800";
        private const string DRE_ABREVIACAO = "DRE - JT";

        private const string UE_NOME = "MAXIMO DE MOURA SANTOS, PROF.";
        private const string UE_CODIGO = "094765";

        private const string TURMA_NOME = "7B";
        private const string TURMA_FILTRO = "7B - 7º ANO";
        private const string TURMA_CODIGO = "2261179";
        private const string TURMA_ANO = "7";

        private const string TIPO_CALDENDARIO_NOME = "Calendário Escolar de 2022";

        private const string SISTEMA = "Sistema";

        private const int DISCIPLINA_ID = 1;

        private const int NOTA_5 = 5;

        private const string ALUNO_CODIGO = "4182555";

        private const string MENSAGEM_NOTIFICACAO_WF_APROVACAO = "Foram criadas 4 aula(s) de reposição de Língua Portuguesa na turma 7B da DERVILLE ALLEGRETTI, PROF. (DIRETORIA REGIONAL DE EDUCACAO JACANA/TREMEMBE). Para que esta aula seja considerada válida você precisa aceitar esta notificação. Para visualizar a aula clique  <a href='https://dev-novosgp.sme.prefeitura.sp.gov.br/calendario-escolar/calendario-professor/cadastro-aula/editar/:0/'>aqui</a>.";

        private const string MENSAGEM_TITULO_WF_APROVACAO = "Criação de Aula de Reposição na turma 7B";
        

        public Ao_gerar_wf_aprovacao_nota_fechamento(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task Deve_permitir_inserir_wf_sem_aprovacao_id()
        {
            await CirarDadosBasicos();

            await InserirNaBase(new WfAprovacaoNotaFechamento()
            {
                FechamentoNotaId = 1,
                Nota = NOTA_5,
                CriadoEm = System.DateTime.Now,
                CriadoPor = "Sistema",
                CriadoRF = "Sistema",
            });

            var resultadoWfAprovacao = ObterTodos<WfAprovacaoNotaFechamento>();

            resultadoWfAprovacao.ShouldNotBeEmpty();
            resultadoWfAprovacao.Count().ShouldBe(1);
            resultadoWfAprovacao.Any(a => a.WfAprovacaoId is null).ShouldBeTrue();
        }

        [Fact]
        public async Task Deve_permitir_inserir_wf_com_aprovacao_id()
        {
            await CirarDadosBasicos();

            await InserirNaBase(new WorkflowAprovacao()
            {
                UeId = UE_CODIGO,
                DreId = DRE_CODIGO,
                Ano = 2022,                
                NotificacaoTipo = NotificacaoTipo.Fechamento,
                NotifacaoMensagem = MENSAGEM_NOTIFICACAO_WF_APROVACAO,
                NotifacaoTitulo = MENSAGEM_TITULO_WF_APROVACAO,
                CriadoPor = SISTEMA,
                CriadoEm = DateTime.Now,
                CriadoRF = SISTEMA,
                Tipo = WorkflowAprovacaoTipo.Basica
            });

            await InserirNaBase(new WfAprovacaoNotaFechamento()
            {
                FechamentoNotaId = 1,
                Nota = NOTA_5,
                CriadoEm = System.DateTime.Now,
                CriadoPor = SISTEMA,
                CriadoRF = SISTEMA,
                WfAprovacaoId = 1
            });

            var resultadoWfAprovacao = ObterTodos<WfAprovacaoNotaFechamento>();

            resultadoWfAprovacao.ShouldNotBeEmpty();
            resultadoWfAprovacao.Count().ShouldBe(1);
            resultadoWfAprovacao.Any(a => a.WfAprovacaoId is null).ShouldBeFalse();
            resultadoWfAprovacao.Any(a => a.WfAprovacaoId is not null).ShouldBeTrue();            
        }

        private async Task CirarDadosBasicos()
        {
            await InserirNaBase(new Dre()
            {
                Nome = DRE_NOME,
                CodigoDre = DRE_CODIGO,
                Abreviacao = DRE_ABREVIACAO
            });

            await InserirNaBase(new Ue()
            {
                Nome = UE_NOME,
                DreId = 1,
                TipoEscola = TipoEscola.EMEF,
                CodigoUe = UE_CODIGO
            });

            await InserirNaBase(new Turma()
            {
                Nome = TURMA_NOME,
                CodigoTurma = TURMA_CODIGO,
                Ano = TURMA_ANO,
                AnoLetivo = 2022,
                TipoTurma = Dominio.Enumerados.TipoTurma.Regular,
                ModalidadeCodigo = Modalidade.Fundamental,
                UeId = 1,
                NomeFiltro = TURMA_FILTRO
            });

            await InserirNaBase(new TipoCalendario()
            {
                Situacao = true,
                Modalidade = ModalidadeTipoCalendario.FundamentalMedio,
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new DateTime(2022, 06, 06),
                Nome = TIPO_CALDENDARIO_NOME,
                Periodo = Periodo.Anual,
                AnoLetivo = 2022,
                Excluido = false
            });

            await InserirNaBase(new PeriodoEscolar()
            {
                Bimestre = 1,
                PeriodoFim = new DateTime(2022, 08, 20),
                PeriodoInicio = new DateTime(2022, 02, 01),
                TipoCalendarioId = 1,
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new DateTime(2022, 01, 01),
            });

            await InserirNaBase(new FechamentoTurma()
            {
                TurmaId = 1,
                PeriodoEscolarId = 1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA,
                CriadoRF = SISTEMA
            });

            await InserirNaBase(new FechamentoTurmaDisciplina()
            {
                DisciplinaId = DISCIPLINA_ID,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA,
                CriadoRF = SISTEMA,
                Situacao = SituacaoFechamento.ProcessadoComSucesso,
                FechamentoTurmaId = 1
            });

            await InserirNaBase(new FechamentoAluno()
            {
                FechamentoTurmaDisciplinaId = 1,
                AlunoCodigo = ALUNO_CODIGO,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA,
                CriadoRF = SISTEMA,
            });

            await InserirNaBase(new FechamentoNota()
            {
                DisciplinaId = DISCIPLINA_ID,
                Nota = NOTA_5,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA,
                CriadoRF = SISTEMA,
                FechamentoAlunoId = 1
            });
        }
    }
}
