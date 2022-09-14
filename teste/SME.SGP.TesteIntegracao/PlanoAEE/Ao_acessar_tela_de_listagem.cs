using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.PlanoAEE
{
    public class Ao_acessar_tela_de_listagem : PlanoAEETesteBase
    {
        public Ao_acessar_tela_de_listagem(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
        }

        [Fact]
        public async Task Deve_exibir_historico_ao_selecionar_turma_de_2021()
        {
            var servicoCadastrarPlanoAee = ObterServicoSalvarPlanoAEEUseCase();
            var servicoObterPlanoAEE = ObterServicoObterPlanosAEEUseCase();
            var filtroPlanoAeeDto = ObterFiltroPlanoAEEDto();
            var planoAEEPersistenciaDto = ObterPlanoAEEPersistenciaDto();
            await CriarDadosBasicos(new FiltroPlanoAee()
            {
                Modalidade = Modalidade.Fundamental,
                Perfil = ObterPerfilProfessor(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio
            });
            await servicoCadastrarPlanoAee.Executar(planoAEEPersistenciaDto);
            var retorno = await servicoObterPlanoAEE.Executar(filtroPlanoAeeDto);
            retorno.ShouldNotBeNull();
        }

        private FiltroPlanosAEEDto ObterFiltroPlanoAEEDto(SituacaoPlanoAEE situacao = SituacaoPlanoAEE.Validado)
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

        private PlanoAEEPersistenciaDto ObterPlanoAEEPersistenciaDto()
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
                    { QuestaoId = 1,
                        Resposta = "teset",
                        RespostaPlanoId = 1,
                        TipoQuestao = TipoQuestao.Frase
                    }
                };
        }
    }
}