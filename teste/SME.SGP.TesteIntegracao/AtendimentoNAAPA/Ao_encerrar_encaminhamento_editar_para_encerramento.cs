using Shouldly;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.AtendimentoNAAPA
{
    public class Ao_encerrar_encaminhamento_editar_para_encerramento : AtendimentoNAAPATesteBase
    {
        public Ao_encerrar_encaminhamento_editar_para_encerramento(CollectionFixture collectionFixture) : base(collectionFixture)
        {}

        [Fact(DisplayName = "Encaminhamento NAAPA - Deve realizar o encerramento do encaminhamento NAAPA pelo Coordenador NAAPA")]
        public async Task Ao_encerrar_encaminhamento_naapa()
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
            
            await CriarQuestoesEncaminhamentoNAAPA();

            await CriarRespostasEncaminhamentoNAAPA(new DateTime(DateTimeExtension.HorarioBrasilia().Date.Year, 11, 18));
            
            var encerrarEncaminhamentoNaapaUseCase = ObterServicoEncerrarEncaminhamento();

            var retorno = await encerrarEncaminhamentoNaapaUseCase.Executar(1, MOTIVO_ENCERRAMENTO);
            retorno.ShouldBeTrue();
            
            var encaminhamento = ObterTodos<Dominio.EncaminhamentoNAAPA>().FirstOrDefault();
            encaminhamento.ShouldNotBeNull();
            encaminhamento.Situacao.ShouldBe(SituacaoNAAPA.Encerrado);
            encaminhamento.MotivoEncerramento.ShouldBe(MOTIVO_ENCERRAMENTO);

            var historico = ObterTodos<Dominio.EncaminhamentoNAAPAHistoricoAlteracoes>().FirstOrDefault();
            historico.ShouldNotBeNull();
            historico.CamposAlterados.ShouldBe("Situação");
        } 
        
        [Fact(DisplayName = "Encaminhamento NAAPA - Não deve permitir o encerramento de um encaminhamento NAAPA que não existe pelo Coordenador NAAPA")]
        public async Task Ao_encerrar_encaminhamento_naapa_inexistente()
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
            
            await CriarQuestoesEncaminhamentoNAAPA();

            await CriarRespostasEncaminhamentoNAAPA(new DateTime(DateTimeExtension.HorarioBrasilia().Date.Year, 11, 18));
            
            var encerrarEncaminhamentoNaapaUseCase = ObterServicoEncerrarEncaminhamento();

            var excecao = await Assert.ThrowsAsync<NegocioException>(() => encerrarEncaminhamentoNaapaUseCase.Executar(2, MOTIVO_ENCERRAMENTO));

            excecao.Message.ShouldBe(MensagemNegocioAtendimentoNAAPA.ATENDIMENTO_NAO_ENCONTRADO);
        } 
        
        private async Task CriarRespostasEncaminhamentoNAAPA(DateTime dataQueixa)
        {
            await InserirNaBase(new Dominio.RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = 1,
                Texto = dataQueixa.ToString("dd/MM/yyyy"),
                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new Dominio.RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = 2,
                Texto = "1",
                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task CriarQuestoesEncaminhamentoNAAPA()
        {
            await InserirNaBase(new Dominio.QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 1,
                QuestaoId = 1,
                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new Dominio.QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 1,
                QuestaoId = 2,
                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task CriarEncaminhamentoNAAPASecao()
        {
            await InserirNaBase(new Dominio.EncaminhamentoNAAPASecao()
            {
                EncaminhamentoNAAPAId = 1,
                SecaoEncaminhamentoNAAPAId = 1,
                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
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
                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
        }
    }
}
