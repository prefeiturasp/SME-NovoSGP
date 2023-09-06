using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.PlanoAEE.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.PlanoAEE
{
    public class Ao_ocorrer_analise_cefai : PlanoAEETesteBase
    {
        public Ao_ocorrer_analise_cefai(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterParametroSistemaPorTipoEAnoQuery, ParametrosSistema>), typeof(ObterParametroSistemaPorTipoEAnoQueryHanlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTurmaRegularESrmPorAlunoQuery, IEnumerable<TurmasDoAlunoDto>>), typeof(ObterTurmaRegularESrmPorAlunoQueryHandlerFake), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName = "Plano AEE - Deve alterar a pendência do paai para validado")]
        public async Task Deve_alterar_a_pendencia_do_paai_para_validado()
        {
            await CriarDadosBasicos(new FiltroPlanoAee()
            {
                Modalidade = Modalidade.Fundamental,
                Perfil = ObterPerfilPaai(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio
            });

            var servicoCadastrarPlanoAee = ObterServicoSalvarPlanoAEEUseCase();
            var servicoObterPlanoAEE = ObterServicoObterPlanosAEEUseCase();

            var planoAEEPersistenciaDto = ObterPlanoAEEDto();
            var filtroPlanoAeeDto = ObterFiltroPlanoAEEDto();

            await servicoCadastrarPlanoAee.Executar(planoAEEPersistenciaDto);
            var retorno = await servicoObterPlanoAEE.Executar(filtroPlanoAeeDto);
            var parecerDto = ObterPlanoAEECadastroParecerDto();

            var servicoAtribuirResponsavelPlano = ObterServicoAtribuirResponsavelPlanoAEEUseCase();
            var retornoAtribuicao = await servicoAtribuirResponsavelPlano.Executar(retorno.Items.FirstOrDefault().Id, USUARIO_LOGIN_PAAI);

            var servicoCadastrarParecerPAAI = ObterServicoCadastrarParecerPAAIPlanoAEEUseCase();
            await servicoCadastrarParecerPAAI.Executar(retorno.Items.FirstOrDefault().Id, parecerDto);

            filtroPlanoAeeDto.Situacao = SituacaoPlanoAEE.Validado;

            var retornoPlanoValidado = await servicoObterPlanoAEE.Executar(filtroPlanoAeeDto);
            retornoPlanoValidado.ShouldNotBeNull();
            retornoPlanoValidado.Items.Count().ShouldBeEquivalentTo(1);
        }

        private PlanoAEECadastroParecerDto ObterPlanoAEECadastroParecerDto()
        {
            return new PlanoAEECadastroParecerDto() { Parecer = "Teste Parecer paai" };
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
                ResponsavelRF = USUARIO_LOGIN_PAAI,
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