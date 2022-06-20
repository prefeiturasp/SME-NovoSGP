using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.TestarAulaUnica
{
    public class Ao_registrar_aula_unica : AulaTeste
    {
        public Ao_registrar_aula_unica(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task Ao_registrar_aula_unica_professor_especialista()
        {
            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio);

            await InserirAulaUseCaseComValidacaoBasica(TipoAula.Normal, RecorrenciaAula.AulaUnica);
        }

        [Fact]
        public async Task Ao_registrar_aula_unica_regente_professor_EJA()
        {
            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.EJA, ModalidadeTipoCalendario.EJA);

            await InserirAulaUseCaseComValidacaoBasica(TipoAula.Normal, RecorrenciaAula.AulaUnica,true);
        }

        [Fact]
        public async Task Ao_registrar_aula_unica_regente_professor_educacao_infantil()
        {
            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.EducacaoInfantil, ModalidadeTipoCalendario.Infantil);

            await InserirAulaUseCaseComValidacaoBasica(TipoAula.Normal, RecorrenciaAula.AulaUnica,true);
        }

        [Fact]
        public async Task Ao_registrar_aula_unica_professor_CP()
        {
            await CriarDadosBasicosAula(Perfis.PERFIL_CP.ToString(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio);

            await InserirAulaUseCaseComValidacaoBasica(TipoAula.Normal, RecorrenciaAula.AulaUnica);
        }

        [Fact]
        public async Task Ao_registrar_aula_com_evento_letivo()
        {
            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio);
            await CrieEvento(EventoLetivo.Sim);

            await InserirAulaUseCaseComValidacaoBasica(TipoAula.Normal, RecorrenciaAula.AulaUnica);
        }

        [Fact]
        public async Task Nao_e_possivel_cadastrar_aula_com_periodo_encerrado()
        {
            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, false);
            await CriarPeriodoEscolarEncerrado();

            var useCase = ServiceProvider.GetService<IInserirAulaUseCase>();
            var dto = ObterAula(TipoAula.Normal, RecorrenciaAula.AulaUnica);

            var excecao = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(dto));

            excecao.Message.ShouldBe("Não é possível cadastrar aula do tipo 'Normal' para o dia selecionado!");
        }

        [Fact]
        public async Task Nao_e_possivel_cadastrar_aula_no_domingo()
        {
            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio);

            var useCase = ServiceProvider.GetService<IInserirAulaUseCase>();
            var dto = ObterAula(TipoAula.Normal, RecorrenciaAula.AulaUnica);
            dto.DataAula = new DateTime(2022, 02, 13);

            var excecao = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(dto));

            excecao.Message.ShouldBe("Não é possível cadastrar aula no final de semana");
        }

        [Fact]
        public async Task O_professor_nao_pode_fazer_alteracao_na_turma()
        {
            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio);

            var useCase = ServiceProvider.GetService<IInserirAulaUseCase>();
            var dto = ObterAula(TipoAula.Normal, RecorrenciaAula.AulaUnica);
            dto.DataAula = new DateTime(2022, 02, 02);

            var excecao = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(dto));

            excecao.Message.ShouldBe("Você não pode fazer alterações ou inclusões nesta turma, componente curricular e data.");
        }
    }
}
