using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.AtendimentoNAAPA.ServicosFakes;
using SME.SGP.TesteIntegracao.Constantes;
using SME.SGP.TesteIntegracao.Setup;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.RegistroAcaoBuscaAtiva
{
    public class Ao_listar_registros_acao_busca_ativa_filtro : RegistroAcaoBuscaAtivaTesteBase
    {
        
   
        public Ao_listar_registros_acao_busca_ativa_filtro(CollectionFixture collectionFixture) : base(collectionFixture)
        { }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunoPorTurmaAlunoCodigoQuery, AlunoPorTurmaResposta>), typeof(ObterAlunoPorTurmaAlunoCodigoQueryHandlerFakeNAAPA), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterConsultaFrequenciaGeralAlunoQuery, string>), typeof(ObterConsultaFrequenciaGeralAlunoQueryHandlerFake), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName = "Registro de Ação - Listar registros de ação por filtros")]
        public async Task Ao_listar_registros_acao_com_filtros()
        {
            var filtro = new FiltroRegistroAcaoDto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8"
            };

            await CriarDadosBase(filtro);
            var dataRegistro = DateTimeExtension.HorarioBrasilia().Date;
            await GerarDadosRegistroAcao_2PrimeirasQuestoes(dataRegistro.AddMonths(-1), true);
            await GerarDadosRegistroAcao_2PrimeirasQuestoes(dataRegistro, true);
            await GerarDadosRegistroAcao_2PrimeirasQuestoes(dataRegistro.AddMonths(1), true);
            var useCase = ObterUseCaseListagemRegistrosAcao();
            var retorno = await useCase.Executar(new FiltroRegistrosAcaoDto()
            {
                AnoLetivo = dataRegistro.Year,
                DreId = DRE_ID_1,
                UeId = UE_ID_1,
                TurmaId = TURMA_ID_1,
                Modalidade = (int)Modalidade.Fundamental,
                OrdemProcedimentoRealizado = ConstantesQuestionarioBuscaAtiva.QUESTAO_PROCEDIMENTO_REALIZADO_ORDEM_RESPOSTA_LIG_TELEFONICA,
                CodigoNomeAluno = "aluno 1",
                DataRegistroInicio = dataRegistro.AddMonths(-1),
                DataRegistroFim = dataRegistro.AddMonths(1)
            });
            retorno.ShouldNotBeNull();
            retorno.TotalRegistros.ShouldBe(3);
            retorno.Items.Count().ShouldBe(3);
            retorno.Items.Any(ra => ra.DataRegistro == dataRegistro.AddMonths(-1)).ShouldBeTrue();
            retorno.Items.Any(ra => ra.DataRegistro == dataRegistro).ShouldBeTrue();
            retorno.Items.Any(ra => ra.DataRegistro == dataRegistro.AddMonths(1)).ShouldBeTrue();

            retorno = await useCase.Executar(new FiltroRegistrosAcaoDto()
            {
                AnoLetivo = dataRegistro.Year,
                DreId = DRE_ID_1,
                UeId = UE_ID_1,
                TurmaId = TURMA_ID_1,
                Modalidade = (int)Modalidade.Fundamental,
                OrdemProcedimentoRealizado = ConstantesQuestionarioBuscaAtiva.QUESTAO_PROCEDIMENTO_REALIZADO_ORDEM_RESPOSTA_LIG_TELEFONICA,
                CodigoNomeAluno = ALUNO_CODIGO_1,
                DataRegistroInicio = dataRegistro.AddMonths(-1),
                DataRegistroFim = dataRegistro.AddMonths(1)
            });
            retorno.ShouldNotBeNull();
            retorno.TotalRegistros.ShouldBe(3);
            retorno.Items.Count().ShouldBe(3);
            retorno.Items.Any(ra => ra.DataRegistro == dataRegistro.AddMonths(-1)).ShouldBeTrue();
            retorno.Items.Any(ra => ra.DataRegistro == dataRegistro).ShouldBeTrue();
            retorno.Items.Any(ra => ra.DataRegistro == dataRegistro.AddMonths(1)).ShouldBeTrue();
        }

        [Fact(DisplayName = "Registro de Ação - Listar registros de ação por filtros - todas ues")]
        public async Task Ao_listar_registros_acao_com_filtros_todas_ues()
        {
            var filtro = new FiltroRegistroAcaoDto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8"
            };

            await CriarDadosBase(filtro);
            await CriarTurma(Modalidade.Fundamental, ANO_6, TURMA_CODIGO_2, Dominio.Enumerados.TipoTurma.Regular, UE_ID_2, DateTimeExtension.HorarioBrasilia().Year);
            await CriarTurma(Modalidade.Fundamental, ANO_6, TURMA_CODIGO_3, Dominio.Enumerados.TipoTurma.Regular, UE_ID_3, DateTimeExtension.HorarioBrasilia().Year);
            var dataRegistro = DateTimeExtension.HorarioBrasilia().Date;
            await GerarDadosRegistroAcao_2PrimeirasQuestoes(dataRegistro.AddMonths(-1), true, TURMA_ID_2);
            await GerarDadosRegistroAcao_2PrimeirasQuestoes(dataRegistro, true, TURMA_ID_3);
            await GerarDadosRegistroAcao_2PrimeirasQuestoes(dataRegistro.AddMonths(1), true, TURMA_ID_3);
            var useCase = ObterUseCaseListagemRegistrosAcao();
            
            var retorno = await useCase.Executar(new FiltroRegistrosAcaoDto()
            {
                AnoLetivo = dataRegistro.Year,
                DreId = DRE_ID_2,
                Modalidade = (int)Modalidade.Fundamental,
                OrdemProcedimentoRealizado = ConstantesQuestionarioBuscaAtiva.QUESTAO_PROCEDIMENTO_REALIZADO_ORDEM_RESPOSTA_LIG_TELEFONICA,
                CodigoNomeAluno = ALUNO_CODIGO_1,
                DataRegistroInicio = dataRegistro.AddMonths(-1),
                DataRegistroFim = dataRegistro.AddMonths(1)
            });
            retorno.ShouldNotBeNull();
            retorno.TotalRegistros.ShouldBe(3);
            retorno.Items.Count().ShouldBe(3);
            retorno.Items.Any(ra => ra.DataRegistro == dataRegistro.AddMonths(-1)).ShouldBeTrue();
            retorno.Items.Any(ra => ra.DataRegistro == dataRegistro).ShouldBeTrue();
            retorno.Items.Any(ra => ra.DataRegistro == dataRegistro.AddMonths(1)).ShouldBeTrue();
        }
    }
}

