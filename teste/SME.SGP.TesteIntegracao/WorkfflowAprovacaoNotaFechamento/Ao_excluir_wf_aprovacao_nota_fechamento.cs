using Elastic.Apm.Api;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.WorkfflowAprovacaoNotaFechamento
{
    public class Ao_excluir_wf_aprovacao_nota_fechamento : TesteBaseComuns
    {
        public Ao_excluir_wf_aprovacao_nota_fechamento(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task Ao_excluir_logico_wf_aprovacao_nota_fechamento()
        {
            CriarClaimUsuario(ObterPerfilProfessor());
            await CriarUsuarios();
            await CriarDreUePerfil();
            await CriarTurma(Modalidade.Fundamental);
            await CriarTipoCalendario(ModalidadeTipoCalendario.FundamentalMedio);
            await CriarPeriodoEscolar(DATA_01_01_INICIO_BIMESTRE_1, DATA_01_05_FIM_BIMESTRE_1, BIMESTRE_1);
            await CriaFechamento();
            var wf = new WfAprovacaoNotaFechamento()
            {
                Id= 1,
                FechamentoNotaId = 1,
                Nota = NOTA_5,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            };
            await InserirNaBase(wf);

            var mediator = ServiceProvider.GetService<IMediator>();

            await mediator.Send(new ExcluirWFAprovacaoNotaFechamentoCommand(wf));

            var wfs = ObterTodos<Dominio.WfAprovacaoNotaFechamento>();
            wfs.ShouldNotBeNull();
            wfs.Exists(wf => wf.Excluido).ShouldBeTrue();
        }

        [Fact]
        public async Task Ao_obter_wf_aprovacao_nota_fechamento_nao_deve_apresentar_excluido()
        {
            CriarClaimUsuario(ObterPerfilProfessor());
            await CriarUsuarios();
            await CriarDreUePerfil();
            await CriarTurma(Modalidade.Fundamental);
            await CriarTipoCalendario(ModalidadeTipoCalendario.FundamentalMedio);
            await CriarPeriodoEscolar(DATA_01_01_INICIO_BIMESTRE_1, DATA_01_05_FIM_BIMESTRE_1, BIMESTRE_1);
            await CriaFechamento();
            await InserirNaBase(new WfAprovacaoNotaFechamento
            {
                FechamentoNotaId = 1,
                Nota = NOTA_5,
                Excluido = true,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
            await InserirNaBase(new WfAprovacaoNotaFechamento()
            {
                FechamentoNotaId = 1,
                Nota = NOTA_6,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var mediator = ServiceProvider.GetService<IMediator>();

            var wfs = await mediator.Send(new ObterWorkflowAprovacaoNotaFechamentoSemAprovacaoIdQuery());
            wfs.ShouldNotBeNull();
            wfs.ToList().Exists(wf => wf.WfAprovacao.Excluido).ShouldBeFalse();
        }

        [Fact]
        public async Task Ao_obter_nota_em_aprovacao_nao_deve_apresentar_excluido()
        {
            CriarClaimUsuario(ObterPerfilProfessor());
            await CriarUsuarios();
            await CriarDreUePerfil();
            await CriarTurma(Modalidade.Fundamental);
            await CriarTipoCalendario(ModalidadeTipoCalendario.FundamentalMedio);
            await CriarPeriodoEscolar(DATA_01_01_INICIO_BIMESTRE_1, DATA_01_05_FIM_BIMESTRE_1, BIMESTRE_1);
            await CriaFechamento();
            await InserirNaBase(new WfAprovacaoNotaFechamento
            {
                FechamentoNotaId = 1,
                Nota = NOTA_5,
                Excluido = true,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
            await InserirNaBase(new WfAprovacaoNotaFechamento()
            {
                FechamentoNotaId = 1,
                Nota = NOTA_6,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var mediator = ServiceProvider.GetService<IMediator>();

            var nota = await mediator.Send(new ObterNotaEmAprovacaoQuery(CODIGO_ALUNO_1, 1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138));
            nota.ShouldBe(NOTA_6);
        }

        [Fact]
        public async Task Ao_obter_nota_em_aprovacao_por_wf_nao_deve_apresentar_excluido()
        {
            CriarClaimUsuario(ObterPerfilProfessor());
            await CriarUsuarios();
            await CriarDreUePerfil();
            await CriarTurma(Modalidade.Fundamental);
            await CriarTipoCalendario(ModalidadeTipoCalendario.FundamentalMedio);
            await CriarPeriodoEscolar(DATA_01_01_INICIO_BIMESTRE_1, DATA_01_05_FIM_BIMESTRE_1, BIMESTRE_1);
            await CriaFechamento();
            await InserirNaBase(new WfAprovacaoNotaFechamento
            {
                FechamentoNotaId = 1,
                Nota = NOTA_5,
                Excluido = true,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var mediator = ServiceProvider.GetService<IMediator>();

            var wfs = await mediator.Send(new ObterNotaFechamentoEmAprovacaoPorWorkflowIdQuery(1));
            wfs.ShouldNotBeNull();
            wfs.Count().ShouldBe(0);
        }

        [Fact]
        public async Task Ao_obter_nota_em_aprovacao_por_fechamento_nota_id_nao_deve_apresentar_excluido()
        {
            CriarClaimUsuario(ObterPerfilProfessor());
            await CriarUsuarios();
            await CriarDreUePerfil();
            await CriarTurma(Modalidade.Fundamental);
            await CriarTipoCalendario(ModalidadeTipoCalendario.FundamentalMedio);
            await CriarPeriodoEscolar(DATA_01_01_INICIO_BIMESTRE_1, DATA_01_05_FIM_BIMESTRE_1, BIMESTRE_1);
            await CriaFechamento();
            await InserirNaBase(new WfAprovacaoNotaFechamento
            {
                FechamentoNotaId = 1,
                Nota = NOTA_5,
                Excluido = true,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var mediator = ServiceProvider.GetService<IMediator>();

            var wfs = await mediator.Send(new ObterNotaEmAprovacaoPorFechamentoNotaIdQuery() {  IdsFechamentoNota = new List<long>() { 1 } });
            wfs.ShouldNotBeNull();
            wfs.Count().ShouldBe(0);
        }


        [Fact]
        public async Task Ao_obter_atividade_avaliativa_nota_por_turma_nao_deve_apresentar_excluido()
        {
            CriarClaimUsuario(ObterPerfilProfessor());
            await CriarUsuarios();
            await CriarDreUePerfil();
            await CriarTurma(Modalidade.Fundamental);
            await CriarTipoCalendario(ModalidadeTipoCalendario.FundamentalMedio);
            await CriarPeriodoEscolar(DATA_01_01_INICIO_BIMESTRE_1, DATA_01_05_FIM_BIMESTRE_1, BIMESTRE_1);
            await CriaFechamento();
            await CriarTipoAvaliacao();
            await CriarAtividadeAvaliativa(DATA_01_01_INICIO_BIMESTRE_1);
            await CriarAtividadeAvaliativaDisciplina(ATIVIDADE_AVALIATIVA_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString());
            await InserirNaBase(new NotaTipoValor()
            {
                Ativo = true,
                InicioVigencia = DATA_01_01_INICIO_BIMESTRE_1,
                TipoNota = TipoNota.Nota,
                Descricao = NOTA,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
            await InserirNaBase(new NotaConceito()
            {
                AlunoId = ALUNO_CODIGO_1,
                AtividadeAvaliativaID = 1,
                Nota = NOTA_6,
                ConceitoId = null,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                DisciplinaId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                TipoNota = TipoNota.Nota
            });
            await InserirNaBase(new WfAprovacaoNotaFechamento
            {
                FechamentoNotaId = 1,
                Nota = NOTA_5,
                Excluido = true,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var mediator = ServiceProvider.GetService<IMediator>();

            var AvaliacoesNotas = await mediator.Send(new ObterAtividadesNotasAlunoPorTurmaPeriodoQuery(TURMA_ID_1, PERIODO_ESCOLAR_CODIGO_1, ALUNO_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString()));
            AvaliacoesNotas.ShouldNotBeNull();
            var avaliacao = AvaliacoesNotas.FirstOrDefault();
            avaliacao.NotaConceito.ShouldBe(NOTA_6);
        }
   
        private async Task CriaFechamento()
        {
            await InserirNaBase(new FechamentoTurma()
            {
                TurmaId = 1,
                PeriodoEscolarId = 1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new FechamentoTurmaDisciplina()
            {
                DisciplinaId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                Situacao = SituacaoFechamento.ProcessadoComSucesso,
                FechamentoTurmaId = 1
            });

            await InserirNaBase(new FechamentoAluno()
            {
                FechamentoTurmaDisciplinaId = 1,
                AlunoCodigo = CODIGO_ALUNO_1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new FechamentoNota()
            {
                DisciplinaId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                Nota = NOTA_5,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                FechamentoAlunoId = 1
            });
        }

        protected async Task CriarAtividadeAvaliativa(DateTime dataAvaliacao)
        {
            await InserirNaBase(new AtividadeAvaliativa
            {
                DreId = DRE_CODIGO_1,
                UeId = UE_CODIGO_1,
                ProfessorRf = USUARIO_PROFESSOR_CODIGO_RF_2222222,
                TurmaId = TURMA_CODIGO_1,
                Categoria = CategoriaAtividadeAvaliativa.Normal,
                TipoAvaliacaoId = 1,
                NomeAvaliacao = "Nome avaliação",
                DescricaoAvaliacao = "Descrição avaliação",
                DataAvaliacao = dataAvaliacao,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
        }

        protected async Task CriarAtividadeAvaliativaDisciplina(long atividadeAvaliativaId, string componenteCurricular)
        {
            await InserirNaBase(new AtividadeAvaliativaDisciplina
            {
                AtividadeAvaliativaId = atividadeAvaliativaId,
                DisciplinaId = componenteCurricular,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
        }

        protected async Task CriarTipoAvaliacao()
        {
            await InserirNaBase(new TipoAvaliacao
            {
                Nome = "Nome",
                Descricao = "Descrição",
                Situacao = true,
                AvaliacoesNecessariasPorBimestre = 1,
                Codigo = TipoAvaliacaoCodigo.AvaliacaoBimestral,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
        }
    }
}
