using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.TesteIntegracao.Setup;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.AtendimentoNAAPA
{
    public class Ao_obter_historico_alteracao_apresentacao_encaminhamento_naapa : AtendimentoNAAPATesteBase
    {
        public Ao_obter_historico_alteracao_apresentacao_encaminhamento_naapa(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact(DisplayName = "Encaminhamento NAAPA Histórico de alteração - Obter histórico de alteração do tipo inserido ")]
        public async Task Ao_obter_historico_alteracao_tipo_inserido()
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

            await CriarEncaminhamentoNAAPA();
            await CriarEncaminhamentoNAAPASecao();

            await InserirNaBase(new EncaminhamentoNAAPAHistoricoAlteracoes()
            {
                CamposAlterados = "",
                CamposInseridos = "Data de entrada da queixa | Prioridade",
                DataHistorico = DateTimeExtension.HorarioBrasilia(),
                EncaminhamentoNAAPAId= 1,
                SecaoEncaminhamentoNAAPAId= 1,
                TipoHistorico = TipoHistoricoAlteracoesAtendimentoNAAPA.Inserido,
                UsuarioId= 1
            });

            var useCase = ServiceProvider.GetService<IObterHistoricosDeAlteracoesApresentacaoAtendimentoNAAPAUseCase>();
            var historicos = await useCase.Executar(1);

            historicos.ShouldNotBeNull();
            var historico = historicos.Items.FirstOrDefault();
            historico.ShouldNotBeNull();
            historico.Descricao.ShouldBe($"Inserido por {historico.UsuarioNome} - {historico.UsuarioLogin} em {historico.DataHistorico.ToString("dd/MM/yyy HH:mm")}");
            historico.CamposInseridos.ShouldBe("Data de entrada da queixa | Prioridade");
        }

        [Fact(DisplayName = "Encaminhamento NAAPA Histórico de alteração - Obter histórico de alteração do tipo alterado ")]
        public async Task Ao_obter_historico_alteracao_tipo_alterado()
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

            await CriarEncaminhamentoNAAPA();
            await CriarEncaminhamentoNAAPASecao();

            await InserirNaBase(new EncaminhamentoNAAPAHistoricoAlteracoes()
            {
                CamposAlterados = "Data de entrada da queixa | Prioridade",
                CamposInseridos = "",
                DataHistorico = DateTimeExtension.HorarioBrasilia(),
                EncaminhamentoNAAPAId = 1,
                SecaoEncaminhamentoNAAPAId = 1,
                TipoHistorico = TipoHistoricoAlteracoesAtendimentoNAAPA.Alteracao,
                UsuarioId = 1
            });

            var useCase = ServiceProvider.GetService<IObterHistoricosDeAlteracoesApresentacaoAtendimentoNAAPAUseCase>();
            var historicos = await useCase.Executar(1);

            historicos.ShouldNotBeNull();
            var historico = historicos.Items.FirstOrDefault();
            historico.ShouldNotBeNull();
            historico.Descricao.ShouldBe($"Alterado por {historico.UsuarioNome} - {historico.UsuarioLogin} em {historico.DataHistorico.ToString("dd/MM/yyy HH:mm")}");
            historico.CamposAlterados.ShouldBe("Data de entrada da queixa | Prioridade");
        }

        [Fact(DisplayName = "Encaminhamento NAAPA Histórico de alteração - Obter histórico de alteração do tipo impressão ")]
        public async Task Ao_obter_historico_alteracao_tipo_impressao()
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

            await CriarEncaminhamentoNAAPA();
            await CriarEncaminhamentoNAAPASecao();

            await InserirNaBase(new EncaminhamentoNAAPAHistoricoAlteracoes()
            {
                DataHistorico = DateTimeExtension.HorarioBrasilia(),
                EncaminhamentoNAAPAId = 1,
                TipoHistorico = TipoHistoricoAlteracoesAtendimentoNAAPA.Impressao,
                UsuarioId = 1
            });

            var useCase = ServiceProvider.GetService<IObterHistoricosDeAlteracoesApresentacaoAtendimentoNAAPAUseCase>();
            var historicos = await useCase.Executar(1);

            historicos.ShouldNotBeNull();
            var historico = historicos.Items.FirstOrDefault();
            historico.ShouldNotBeNull();
            historico.Descricao.ShouldBe($"Impressão realizada por {historico.UsuarioNome} - {historico.UsuarioLogin} em {historico.DataHistorico.ToString("dd/MM/yyy HH:mm")}");
            historico.Secao.ShouldBeNullOrEmpty();
        }

        [Fact(DisplayName = "Encaminhamento NAAPA Histórico de alteração - Obter histórico de alteração do tipo exclusão do atendimento ")]
        public async Task Ao_obter_historico_alteracao_tipo_exclusao_atendimento()
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

            await CriarEncaminhamentoNAAPA();
            await CriarEncaminhamentoNAAPASecao();

            var dataAtendimento = DateTimeExtension.HorarioBrasilia().AddDays(-10).Date.ToString("dd/MM/yyyy");

            await InserirNaBase(new EncaminhamentoNAAPAHistoricoAlteracoes()
            {
                DataHistorico = DateTimeExtension.HorarioBrasilia(),
                EncaminhamentoNAAPAId = 1,
                SecaoEncaminhamentoNAAPAId = 1,
                DataAtendimento = dataAtendimento,
                TipoHistorico = TipoHistoricoAlteracoesAtendimentoNAAPA.Exclusao,
                UsuarioId = 1
            });

            var useCase = ServiceProvider.GetService<IObterHistoricosDeAlteracoesApresentacaoAtendimentoNAAPAUseCase>();
            var historicos = await useCase.Executar(1);

            historicos.ShouldNotBeNull();
            var historico = historicos.Items.FirstOrDefault();
            historico.ShouldNotBeNull();
            historico.Descricao.ShouldBe($"Excluído itinerância {dataAtendimento} por {historico.UsuarioNome} - {historico.UsuarioLogin} em {historico.DataHistorico.ToString("dd/MM/yyy HH:mm")}");
            historico.TipoHistoricoAlteracoes.ShouldBe(TipoHistoricoAlteracoesAtendimentoNAAPA.Exclusao);
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
                CriadoRF = SISTEMA_CODIGO_RF,
                SituacaoMatriculaAluno = SituacaoMatriculaAluno.Concluido
            });
        }
    }
}
