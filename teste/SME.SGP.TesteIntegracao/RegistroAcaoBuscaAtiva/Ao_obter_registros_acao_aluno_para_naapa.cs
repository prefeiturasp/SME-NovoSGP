using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.Commands;
using SME.SGP.TesteIntegracao.Setup;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.RegistroAcaoBuscaAtiva
{
    public class Ao_obter_registros_acao_aluno_para_naapa : RegistroAcaoBuscaAtivaTesteBase
    {
        public Ao_obter_registros_acao_aluno_para_naapa(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact(DisplayName = "Registro de Ação - Listar registros de ação por aluno para encaminhamento naapa")]
        public async Task Ao_obter_registros_acao_aluno()
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

            var useCase = ServiceProvider.GetService<IObterRegistrosDeAcaoParaNAAPAUseCase>();
            var resultado = await useCase.Executar(ALUNO_CODIGO_1);

            resultado.ShouldNotBeNull();
            resultado.TotalRegistros.ShouldBe(3);
            var item1 = resultado.Items.First();
            item1.ConseguiuContatoResponsavel.ShouldBe("Sim");
            item1.ProcedimentoRealizado.ShouldBe("Ligação telefonica");
            item1.Turma.ShouldBe("EF-Turma Nome 1");
            item1.Usuario.ShouldBe("Sistema (1)");
        }


        [Fact(DisplayName = "Registro de Ação - Listar registros de ação por aluno com paginacao para encaminhamento naapa")]
        public async Task Ao_obter_registros_acao_aluno_paginacao()
        {
            var filtro = new FiltroRegistroAcaoDto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8"
            };

            await CriarDreUePerfilComponenteCurricular();
            CriarClaimUsuario(ObterPerfilCP(), "1", "2");
            await CriarUsuarios();
            await CriarTurmaTipoCalendario(filtro);

            await Executor.ExecutarComando(new PublicarQuestionarioBuscaAtivaComando(this));

            var dataRegistro = DateTimeExtension.HorarioBrasilia().Date;
            await GerarDadosRegistroAcao_2PrimeirasQuestoes(dataRegistro.AddMonths(-1), true);
            await GerarDadosRegistroAcao_2PrimeirasQuestoes(dataRegistro, true);
            await GerarDadosRegistroAcao_2PrimeirasQuestoes(dataRegistro.AddMonths(1), true);

            var useCase = ServiceProvider.GetService<IObterRegistrosDeAcaoParaNAAPAUseCase>();
            var resultado = await useCase.Executar(ALUNO_CODIGO_1);

            resultado.ShouldNotBeNull();
            resultado.TotalRegistros.ShouldBe(3);
            resultado.TotalPaginas.ShouldBe(2);
            resultado.Items.Count().ShouldBe(2);
            var item1 = resultado.Items.First();
            item1.ConseguiuContatoResponsavel.ShouldBe("Sim");
            item1.ProcedimentoRealizado.ShouldBe("Ligação telefonica");
            item1.Turma.ShouldBe("EF-Turma Nome 1");
            item1.Usuario.ShouldBe("Sistema (1)");
        }

    }
}
