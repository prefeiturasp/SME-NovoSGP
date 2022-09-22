using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.PlanoAEE
{
    public class Ao_ocorrer_a_rotina_de_expiracao_dos_planos : PlanoAEETesteBase
    {
        public Ao_ocorrer_a_rotina_de_expiracao_dos_planos(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task Deve_alterar_status_validado_para_expirado()
        {
            await CriarDadosBasicos(new FiltroPlanoAee()
            {
                Modalidade = Modalidade.Fundamental,
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio
            });
            var servicoCadastrarPlanoAee = ObterServicoSalvarPlanoAEEUseCase();
            var servicoObterPlanoAEE = ObterServicoObterPlanosAEEUseCase();

            var servicoValidadePendencia = ObterServicoGerarPendenciaValidadePlanoAEEUseCase();

            var planoAEEPersistenciaDto = ObterPlanoAEEDto();
            var filtroPlanoAeeDto = ObterFiltroPlanoAEEDto();

            await servicoCadastrarPlanoAee.Executar(planoAEEPersistenciaDto);
            var retorno = await servicoObterPlanoAEE.Executar(filtroPlanoAeeDto);
            await servicoValidadePendencia.Executar(new MensagemRabbit() { Mensagem = "" });
        }

        private FiltroPlanosAEEDto ObterFiltroPlanoAEEDto(SituacaoPlanoAEE situacao = SituacaoPlanoAEE.ParecerCP)
        {
            return new FiltroPlanosAEEDto()
            {
                AlunoCodigo = CODIGO_ALUNO_1,
                DreId = DRE_ID_1,
                TurmaId = TURMA_ID_1,
                UeId = UE_ID_1,
                Situacao = situacao
            };
        }

        private PlanoAEEPersistenciaDto ObterPlanoAEEDto()
        {
            return new PlanoAEEPersistenciaDto()
            {
                AlunoCodigo = CODIGO_ALUNO_1,
                Situacao = SituacaoPlanoAEE.Validado,
                TurmaCodigo = TURMA_ID_1.ToString(),
                TurmaId = TURMA_ID_1,
                ResponsavelRF = SISTEMA_CODIGO_RF,
                Questoes = ObterPlanoAeeQuestoes()
            };
        }

        private List<PlanoAEEQuestaoDto> ObterPlanoAeeQuestoes()
        {
            return new List<PlanoAEEQuestaoDto>()
                { new PlanoAEEQuestaoDto()
                    {   QuestaoId = 2,
                        Resposta = "Teste Resposta",
                        RespostaPlanoId = 1,
                        TipoQuestao = TipoQuestao.Frase
                    }
                };
        }
    }
}