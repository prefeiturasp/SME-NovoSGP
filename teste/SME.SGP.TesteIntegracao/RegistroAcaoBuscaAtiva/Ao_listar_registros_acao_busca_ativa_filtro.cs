using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.EncaminhamentoNAAPA.ServicosFakes;
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
            await GerarDadosRegistroAcao_3PrimeirasQuestoes(dataRegistro.AddMonths(-1), true);
            await GerarDadosRegistroAcao_3PrimeirasQuestoes(dataRegistro, true);
            await GerarDadosRegistroAcao_3PrimeirasQuestoes(dataRegistro.AddMonths(1), true);
            var useCase = ObterUseCaseListagemRegistrosAcao();
            var retorno = await useCase.Executar(new FiltroRegistrosAcaoDto()
            {
                AnoLetivo = dataRegistro.Year,
                DreId = DRE_ID_1,
                UeId = UE_ID_1,
                TurmaId = TURMA_ID_1,
                Modalidade = (int)Modalidade.Fundamental,
                OrdemProcedimentoRealizado = QUESTAO_PROCEDIMENTO_REALIZADO_ORDEM_RESPOSTA_LIG_TELEFONICA,
                NomeAluno = "aluno 1",
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

