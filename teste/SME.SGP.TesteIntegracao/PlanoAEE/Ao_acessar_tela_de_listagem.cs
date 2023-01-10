using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.PlanoAEE
{
    public class Ao_acessar_tela_de_listagem : PlanoAEETesteBase
    {
        public Ao_acessar_tela_de_listagem(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }
        
        [Fact(DisplayName = "Plano AEE - Deve exibir o historico ao selecionar uma turma de 2021")]
        public async Task Deve_exibir_historico_ao_selecionar_turma_de_2021()
        {
            var servicoCadastrarPlanoAee = ObterServicoSalvarPlanoAEEUseCase();
            var servicoObterPlanoAEE = ObterServicoObterPlanosAEEUseCase();
            
            await CriarDadosBasicos(new FiltroPlanoAee()
            {
                Modalidade = Modalidade.Fundamental,
                Perfil = ObterPerfilProfessor(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio
            });

            await CriarTurma(Modalidade.Fundamental, TURMA_ANO_4, TURMA_CODIGO_4, TipoTurma.Regular, true);
            
            var planoAEEPersistenciaDto = new PlanoAEEPersistenciaDto()
            {
                AlunoCodigo = CODIGO_ALUNO_1,
                Situacao = SituacaoPlanoAEE.ParecerCP,
                TurmaCodigo = TURMA_ID_4.ToString(),
                TurmaId = TURMA_ID_4,
                ResponsavelRF = SISTEMA_CODIGO_RF,
                Questoes = ObterPlanoAeeQuestoes()
            };

            await servicoCadastrarPlanoAee.Executar(planoAEEPersistenciaDto);

            var filtroPlanoAeeDto = new FiltroPlanosAEEDto()
            {
                AlunoCodigo = CODIGO_ALUNO_1,
                DreId = DRE_ID_1,
                TurmaId = TURMA_ID_4,
                UeId = UE_ID_1
            };
            
            var retorno = await servicoObterPlanoAEE.Executar(filtroPlanoAeeDto);
            retorno.ShouldNotBeNull();
            var planoAee = retorno.Items.FirstOrDefault();
            planoAee.Id.ShouldBeGreaterThan(0);
        }

        [Fact(DisplayName = "Plano AEE - Deve filtrar por uma turma de com anterior")]
        public async Task Deve_filtrar_por_turma_ano_anterior()
        {
            var servicoCadastrarPlanoAee = ObterServicoSalvarPlanoAEEUseCase();
            var servicoObterPlanoAEE = ObterServicoObterPlanosAEEUseCase();
            
            await CriarDadosBasicos(new FiltroPlanoAee()
            {
                Modalidade = Modalidade.Fundamental,
                Perfil = ObterPerfilProfessor(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio
            });
            
            await CriarTurma(Modalidade.Fundamental, TURMA_ANO_4, TURMA_CODIGO_4, TipoTurma.Regular, true);
            
            var planoAEEPersistenciaDto = new PlanoAEEPersistenciaDto()
            {
                AlunoCodigo = CODIGO_ALUNO_1,
                Situacao = SituacaoPlanoAEE.ParecerCP,
                TurmaCodigo = TURMA_ID_4.ToString(),
                TurmaId = TURMA_ID_4,
                ResponsavelRF = SISTEMA_CODIGO_RF,
                Questoes = ObterPlanoAeeQuestoes()
            };

            await servicoCadastrarPlanoAee.Executar(planoAEEPersistenciaDto);
            var opa = ObterTodos<SME.SGP.Dominio.PlanoAEE>();
            
            var filtroPlanoAeeDto = new FiltroPlanosAEEDto()
            {
                AlunoCodigo = CODIGO_ALUNO_1,
                DreId = DRE_ID_1,
                TurmaId = TURMA_ID_4,
                UeId = UE_ID_1,
                Situacao = null
            };
            var retorno = await servicoObterPlanoAEE.Executar(filtroPlanoAeeDto);
            retorno.ShouldNotBeNull();
            
            retorno.Items.Count().ShouldBeGreaterThan(0);
        }

        [Fact(DisplayName = "Plano AEE - Deve filtrar por uma turma com ano atual")]
        public async Task Deve_filtrar_por_turma_ano_atual()
        {
            var servicoCadastrarPlanoAee = ObterServicoSalvarPlanoAEEUseCase();
            var servicoObterPlanoAEE = ObterServicoObterPlanosAEEUseCase();
            
            await CriarDadosBasicos(new FiltroPlanoAee()
            {
                Modalidade = Modalidade.Fundamental,
                Perfil = ObterPerfilProfessor(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio
            });
            
            var planoAEEPersistenciaDto = new PlanoAEEPersistenciaDto()
            {
                AlunoCodigo = CODIGO_ALUNO_1,
                Situacao = SituacaoPlanoAEE.ParecerCP,
                TurmaCodigo = TURMA_ID_1.ToString(),
                TurmaId = TURMA_ID_1,
                ResponsavelRF = SISTEMA_CODIGO_RF,
                Questoes = ObterPlanoAeeQuestoes()
            };

            await servicoCadastrarPlanoAee.Executar(planoAEEPersistenciaDto);

            var filtroPlanoAeeDto = new FiltroPlanosAEEDto()
            {
                AlunoCodigo = CODIGO_ALUNO_1,
                DreId = DRE_ID_1,
                TurmaId = TURMA_ID_1,
                UeId = UE_ID_1,
                Situacao = null
            };
            
            var retorno = await servicoObterPlanoAEE.Executar(filtroPlanoAeeDto);
            retorno.ShouldNotBeNull();
            retorno.Items.Count().ShouldBeGreaterThan(0);
        }

        [Fact(DisplayName = "Plano AEE - Deve filtrar pelo nome do estudante")]
        public async Task Deve_filtrar_por_nome_do_estudante()
        {
            var servicoCadastrarPlanoAee = ObterServicoSalvarPlanoAEEUseCase();
            var servicoObterPlanoAEE = ObterServicoObterPlanosAEEUseCase();
            
            await CriarDadosBasicos(new FiltroPlanoAee()
            {
                Modalidade = Modalidade.Fundamental,
                Perfil = ObterPerfilProfessor(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio
            });
            
            var planoAEEPersistenciaDto = new PlanoAEEPersistenciaDto()
            {
                AlunoCodigo = CODIGO_ALUNO_1,
                Situacao = SituacaoPlanoAEE.ParecerCP,
                TurmaCodigo = TURMA_ID_1.ToString(),
                TurmaId = TURMA_ID_1,
                ResponsavelRF = SISTEMA_CODIGO_RF,
                Questoes = ObterPlanoAeeQuestoes()
            };

            await servicoCadastrarPlanoAee.Executar(planoAEEPersistenciaDto);

            var filtroPlanoAeeDto = new FiltroPlanosAEEDto()
            {
                AlunoCodigo = CODIGO_ALUNO_1,
                DreId = DRE_ID_1,
                TurmaId = 0,
                UeId = UE_ID_1,
                Situacao = null
            };
            
            var retorno = await servicoObterPlanoAEE.Executar(filtroPlanoAeeDto);
            retorno.ShouldNotBeNull();
            retorno.Items.Count().ShouldBeGreaterThan(0);
        }

        [Fact(DisplayName = "Plano AEE - Deve filtrar pela situacao")]
        public async Task Deve_filtrar_por_situacao()
        {
            var servicoCadastrarPlanoAee = ObterServicoSalvarPlanoAEEUseCase();
            var servicoObterPlanoAEE = ObterServicoObterPlanosAEEUseCase();
            
            await CriarDadosBasicos(new FiltroPlanoAee()
            {
                Modalidade = Modalidade.Fundamental,
                Perfil = ObterPerfilProfessor(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio
            });
           
            var planoAEEPersistenciaDto = new PlanoAEEPersistenciaDto()
            {
                AlunoCodigo = CODIGO_ALUNO_1,
                Situacao = SituacaoPlanoAEE.ParecerCP,
                TurmaCodigo = TURMA_ID_1.ToString(),
                TurmaId = TURMA_ID_1,
                ResponsavelRF = SISTEMA_CODIGO_RF,
                Questoes = ObterPlanoAeeQuestoes()
            };

            await servicoCadastrarPlanoAee.Executar(planoAEEPersistenciaDto);

            var filtroPlanoAeeDto = new FiltroPlanosAEEDto()
            {
                AlunoCodigo = CODIGO_ALUNO_1,
                DreId = DRE_ID_1,
                TurmaId = 0,
                UeId = UE_ID_1,
                Situacao = SituacaoPlanoAEE.ParecerCP
            };
            var retorno = await servicoObterPlanoAEE.Executar(filtroPlanoAeeDto);
            retorno.ShouldNotBeNull();
            retorno.Items.Count().ShouldBeGreaterThan(0);
        }

        private List<PlanoAEEQuestaoDto> ObterPlanoAeeQuestoes()
        {
            return new List<PlanoAEEQuestaoDto>()
                { new PlanoAEEQuestaoDto
                    {   QuestaoId = 2,
                        Resposta = "Teste Resposta",
                        RespostaPlanoId = 1,
                        TipoQuestao = TipoQuestao.Frase
                    }
                };
        }
    }
}