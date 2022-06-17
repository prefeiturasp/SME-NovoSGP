using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.Setup;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao
{
    public class Ao_registrar_aula_normal_repetir_bimestre_atual : AulaTeste
    {
        public Ao_registrar_aula_normal_repetir_bimestre_atual(CollectionFixture collectionFixture) : base(collectionFixture)
        {}

        [Fact]
        public async Task Ao_registrar_aula_unica_professor_especialista()
        {
            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio);

            await ExecuteTesteRegistre();
        }

        //[Fact]
        //public async Task Ao_registrar_aula_unica_regente_professor_EJA()
        //{
        //    await CarregueBase(ObtenhaPerfilEspecialista(), Modalidade.EJA, ModalidadeTipoCalendario.EJA);

        //    await ExecuteTesteRegistre(true);
        //}

        //[Fact]
        //public async Task Ao_registrar_aula_unica_regente_professor_educacao_infantil()
        //{
        //    await CarregueBase(ObtenhaPerfilEspecialista(), Modalidade.EducacaoInfantil, ModalidadeTipoCalendario.Infantil);

        //    await ExecuteTesteRegistre(true);
        //}

        //[Fact]
        //public async Task Ao_registrar_aula_unica_professor_CP()
        //{
        //    await CarregueBase(Perfis.PERFIL_CP.ToString(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio);

        //    await ExecuteTesteRegistre();
        //}

        //[Fact]
        //public async Task Ao_registrar_aula_com_evento_letivo()
        //{
        //    await CarregueBase(ObtenhaPerfilEspecialista(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio);
        //    await CrieEvento(EventoLetivo.Sim);

        //    await ExecuteTesteRegistre();
        //}

        //[Fact]
        //public async Task Nao_e_possivel_cadastrar_aula_com_periodo_encerrado()
        //{
        //    await CarregueBase(ObtenhaPerfilEspecialista(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, false);
        //    await CriarPeriodoEscolarEncerrado();

        //    var useCase = ServiceProvider.GetService<IInserirAulaUseCase>();
        //    var dto = ObtenhaDtoAula();

        //    var excecao = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(dto));

        //    excecao.Message.ShouldBe("Não é possível cadastrar aula do tipo 'Normal' para o dia selecionado!");
        //}

        //[Fact]
        //public async Task Nao_e_possivel_cadastrar_aula_no_domingo()
        //{
        //    await CarregueBase(ObtenhaPerfilEspecialista(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio);

        //    var useCase = ServiceProvider.GetService<IInserirAulaUseCase>();
        //    var dto = ObtenhaDtoAula();
        //    dto.DataAula = new DateTime(2022, 02, 13);

        //    var excecao = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(dto));

        //    excecao.Message.ShouldBe("Não é possível cadastrar aula no final de semana");
        //}

        //[Fact]
        //public async Task O_professor_nao_pode_fazer_alteracao_na_turma()
        //{
        //    await CarregueBase(ObtenhaPerfilEspecialista(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio);

        //    var useCase = ServiceProvider.GetService<IInserirAulaUseCase>();
        //    var dto = ObtenhaDtoAula();
        //    dto.DataAula = new DateTime(2022, 02, 02);

        //    var excecao = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(dto));

        //    excecao.Message.ShouldBe("Você não pode fazer alterações ou inclusões nesta turma, componente curricular e data.");
        //}
    }
}
