using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.CadastrarAulaRepetirBimestre
{
    public class Ao_registrar_aula_repetir_Bimestre : AulaTeste
    {
        public Ao_registrar_aula_repetir_Bimestre(CollectionFixture collectionFixture) : base(collectionFixture)
        { }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery, bool>), typeof(ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQueryHandlerComPermissaoFake), ServiceLifetime.Scoped));
        }

        [Fact]
        public async Task Deve_permitir_cadastrar_aula_normal_bimestral_professor_especialista_fundamental()
        {
            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_03_01, DATA_28_04, BIMESTRE_1,false);

            await CriarPeriodoEscolarEPeriodoReabertura();

            var retorno = await InserirAulaUseCaseSemValidacaoBasica(TipoAula.Normal, RecorrenciaAula.RepetirBimestreAtual, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, DATA_03_01, false, TIPO_CALENDARIO_1);

            var aulasCadastradas = ObterTodos<Dominio.Aula>();

            aulasCadastradas.Count().ShouldBeEquivalentTo(17);

            aulasCadastradas.Where(w=> !w.Excluido).Count().ShouldBeEquivalentTo(17);

            aulasCadastradas.Where(w => w.DisciplinaId == COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString()).Count().ShouldBeEquivalentTo(17);
        }

        [Fact]
        public async Task Deve_permitir_cadastrar_aula_normal_bimestral_professor_especialista_infantil()
        {
            await CriarDadosBasicosAula(ObterPerfilProfessorInfantil(), Modalidade.EducacaoInfantil, ModalidadeTipoCalendario.Infantil, DATA_03_01, DATA_28_04, BIMESTRE_1, false);

            await CriarPeriodoEscolarEPeriodoReabertura();

            var retorno = await InserirAulaUseCaseSemValidacaoBasica(TipoAula.Normal, RecorrenciaAula.RepetirBimestreAtual, COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_ID_1213, DATA_03_01, false, TIPO_CALENDARIO_1);

            var aulasCadastradas = ObterTodos<Dominio.Aula>();

            aulasCadastradas.Count().ShouldBeEquivalentTo(17);

            aulasCadastradas.Where(w => !w.Excluido).Count().ShouldBeEquivalentTo(17);

            aulasCadastradas.Where(w => w.DisciplinaId == COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_ID_1213.ToString()).Count().ShouldBeEquivalentTo(17);
        }

        [Fact]
        public async Task Deve_permitir_cadastrar_aula_normal_bimestral_professor_especialista_ensino_medio()
        {
            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Medio, ModalidadeTipoCalendario.FundamentalMedio, DATA_03_01, DATA_28_04, BIMESTRE_1, false);

            await CriarPeriodoEscolarEPeriodoReabertura();

            var retorno = await InserirAulaUseCaseSemValidacaoBasica(TipoAula.Normal, RecorrenciaAula.RepetirBimestreAtual, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, DATA_03_01, false, TIPO_CALENDARIO_1);

            var aulasCadastradas = ObterTodos<Dominio.Aula>();

            aulasCadastradas.Count().ShouldBeEquivalentTo(17);

            aulasCadastradas.Where(w => !w.Excluido).Count().ShouldBeEquivalentTo(17);

            aulasCadastradas.Where(w => w.DisciplinaId == COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString()).Count().ShouldBeEquivalentTo(17);
        }


        [Fact]
        public async Task Deve_permitir_cadastrar_aula_normal_bimestral_professor_cj_para_04_dias_fundamental()
        {
            await CriarDadosBasicosAula(ObterPerfilCJ(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_03_01, DATA_28_04, BIMESTRE_1, false);

            await CriarPeriodoEscolarEPeriodoReabertura();

            await CriarAtribuicaoCJ(Modalidade.Fundamental, COMPONENTE_CURRICULAR_PORTUGUES_ID_138);

            await CriarAtribuicaoEsporadica(DATA_03_01, DATA_24_01);

            var retorno = await InserirAulaUseCaseSemValidacaoBasica(TipoAula.Normal, RecorrenciaAula.RepetirBimestreAtual, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, DATA_03_01, false, TIPO_CALENDARIO_1);

            var aulasCadastradas = ObterTodos<Dominio.Aula>();

            aulasCadastradas.Count().ShouldBeEquivalentTo(4);

            aulasCadastradas.Where(w => !w.Excluido).Count().ShouldBeEquivalentTo(4);

            aulasCadastradas.Where(w => w.DisciplinaId == COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString()).Count().ShouldBeEquivalentTo(4);
        }

        [Fact]
        public async Task Deve_permitir_cadastrar_aula_normal_bimestral_professor_cj_para_04_dias_ensino_medio()
        {
            await CriarDadosBasicosAula(ObterPerfilCJ(), Modalidade.Medio, ModalidadeTipoCalendario.FundamentalMedio, DATA_03_01, DATA_28_04, BIMESTRE_1, false);

            await CriarPeriodoEscolarEPeriodoReabertura();

            await CriarAtribuicaoCJ(Modalidade.Fundamental, COMPONENTE_CURRICULAR_PORTUGUES_ID_138);

            await CriarAtribuicaoEsporadica(DATA_03_01, DATA_24_01);

            var retorno = await InserirAulaUseCaseSemValidacaoBasica(TipoAula.Normal, RecorrenciaAula.RepetirBimestreAtual, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, DATA_03_01, false, TIPO_CALENDARIO_1);

            var aulasCadastradas = ObterTodos<Dominio.Aula>();

            aulasCadastradas.Count().ShouldBeEquivalentTo(4);

            aulasCadastradas.Where(w => !w.Excluido).Count().ShouldBeEquivalentTo(4);

            aulasCadastradas.Where(w => w.DisciplinaId == COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString()).Count().ShouldBeEquivalentTo(4);
        }

        [Fact]
        public async Task Deve_permitir_cadastrar_aula_normal_bimestral_professor_cj_infantil_para_04_dias_infantil()
        {
            await CriarDadosBasicosAula(ObterPerfilCJInfantil(), Modalidade.EducacaoInfantil, ModalidadeTipoCalendario.Infantil, DATA_03_01, DATA_28_04, BIMESTRE_1, false);

            await CriarPeriodoEscolarEPeriodoReabertura();

            await CriarAtribuicaoCJ(Modalidade.EducacaoInfantil, COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_ID_1213);

            await CriarAtribuicaoEsporadica(DATA_03_01, DATA_24_01);

            var retorno = await InserirAulaUseCaseSemValidacaoBasica(TipoAula.Normal, RecorrenciaAula.RepetirBimestreAtual, COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_ID_1213, DATA_03_01, false, TIPO_CALENDARIO_1);

            var aulasCadastradas = ObterTodos<Dominio.Aula>();

            aulasCadastradas.Count().ShouldBeEquivalentTo(4);

            aulasCadastradas.Where(w => !w.Excluido).Count().ShouldBeEquivalentTo(4);

            aulasCadastradas.Where(w => w.DisciplinaId == COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_ID_1213.ToString()).Count().ShouldBeEquivalentTo(4);
        }


        [Fact]
        public async Task Deve_permitir_cadastrar_aula_normal_bimestral_professor_cj_para_01_dia_fundamental()
        {
            await CriarDadosBasicosAula(ObterPerfilCJ(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_03_01, DATA_28_04, BIMESTRE_1, false);

            await CriarPeriodoEscolarEPeriodoReabertura();

            await CriarAtribuicaoCJ(Modalidade.Fundamental, COMPONENTE_CURRICULAR_PORTUGUES_ID_138);

            await CriarAtribuicaoEsporadica(DATA_03_01, DATA_03_01);

            var retorno = await InserirAulaUseCaseSemValidacaoBasica(TipoAula.Normal, RecorrenciaAula.RepetirBimestreAtual, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, DATA_03_01, false, TIPO_CALENDARIO_1);

            var aulasCadastradas = ObterTodos<Dominio.Aula>();

            aulasCadastradas.Count().ShouldBeEquivalentTo(1);

            aulasCadastradas.Where(w => !w.Excluido).Count().ShouldBeEquivalentTo(1);

            aulasCadastradas.Where(w => w.DisciplinaId == COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString()).Count().ShouldBeEquivalentTo(1);
        }

        [Fact]
        public async Task Deve_permitir_cadastrar_aula_normal_bimestral_professor_cj_para_01_dia_ensino_medio()
        {
            await CriarDadosBasicosAula(ObterPerfilCJ(), Modalidade.Medio, ModalidadeTipoCalendario.FundamentalMedio, DATA_03_01, DATA_28_04, BIMESTRE_1, false);

            await CriarPeriodoEscolarEPeriodoReabertura();

            await CriarAtribuicaoCJ(Modalidade.Fundamental, COMPONENTE_CURRICULAR_PORTUGUES_ID_138);

            await CriarAtribuicaoEsporadica(DATA_03_01, DATA_03_01);

            var retorno = await InserirAulaUseCaseSemValidacaoBasica(TipoAula.Normal, RecorrenciaAula.RepetirBimestreAtual, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, DATA_03_01, false, TIPO_CALENDARIO_1);

            var aulasCadastradas = ObterTodos<Dominio.Aula>();

            aulasCadastradas.Count().ShouldBeEquivalentTo(1);

            aulasCadastradas.Where(w => !w.Excluido).Count().ShouldBeEquivalentTo(1);

            aulasCadastradas.Where(w => w.DisciplinaId == COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString()).Count().ShouldBeEquivalentTo(1);
        }

        [Fact]
        public async Task Deve_permitir_cadastrar_aula_normal_bimestral_professor_cj_para_01_dia_infantil()
        {
            await CriarDadosBasicosAula(ObterPerfilCJInfantil(), Modalidade.EducacaoInfantil, ModalidadeTipoCalendario.Infantil, DATA_03_01, DATA_28_04, BIMESTRE_1, false);

            await CriarPeriodoEscolarEPeriodoReabertura();

            await CriarAtribuicaoCJ(Modalidade.EducacaoInfantil, COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_ID_1213);

            await CriarAtribuicaoEsporadica(DATA_03_01, DATA_03_01);

            var retorno = await InserirAulaUseCaseSemValidacaoBasica(TipoAula.Normal, RecorrenciaAula.RepetirBimestreAtual, COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_ID_1213, DATA_03_01, false, TIPO_CALENDARIO_1);

            var aulasCadastradas = ObterTodos<Dominio.Aula>();

            aulasCadastradas.Count().ShouldBeEquivalentTo(1);

            aulasCadastradas.Where(w => !w.Excluido).Count().ShouldBeEquivalentTo(1);

            aulasCadastradas.Where(w => w.DisciplinaId == COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_ID_1213.ToString()).Count().ShouldBeEquivalentTo(1);
        }


        [Fact]
        public async Task Deve_permitir_cadastrar_aula_normal_bimestral_diretor_para_01_dia_fundamental()
        {
            await CriarDadosBasicosAula(ObterPerfilDiretor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_03_01, DATA_28_04, BIMESTRE_1, false);

            await CriarPeriodoEscolarEPeriodoReabertura();

            var retorno = await InserirAulaUseCaseSemValidacaoBasica(TipoAula.Normal, RecorrenciaAula.RepetirBimestreAtual, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, DATA_03_01, false, TIPO_CALENDARIO_1);

            var aulasCadastradas = ObterTodos<Dominio.Aula>();

            aulasCadastradas.Count().ShouldBeEquivalentTo(17);

            aulasCadastradas.Where(w => !w.Excluido).Count().ShouldBeEquivalentTo(17);

            aulasCadastradas.Where(w => w.DisciplinaId == COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString()).Count().ShouldBeEquivalentTo(17);
        }

        [Fact]
        public async Task Deve_permitir_cadastrar_aula_normal_bimestral_ad_para_01_dia_fundamental()
        {
            await CriarDadosBasicosAula(ObterPerfilAD(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_03_01, DATA_28_04, BIMESTRE_1, false);

            await CriarPeriodoEscolarEPeriodoReabertura();

            var retorno = await InserirAulaUseCaseSemValidacaoBasica(TipoAula.Normal, RecorrenciaAula.RepetirBimestreAtual, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, DATA_03_01, false, TIPO_CALENDARIO_1);

            var aulasCadastradas = ObterTodos<Dominio.Aula>();

            aulasCadastradas.Count().ShouldBeEquivalentTo(17);

            aulasCadastradas.Where(w => !w.Excluido).Count().ShouldBeEquivalentTo(17);

            aulasCadastradas.Where(w => w.DisciplinaId == COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString()).Count().ShouldBeEquivalentTo(17);
        }

        [Fact]
        public async Task Deve_permitir_cadastrar_aula_normal_bimestral_cp_para_01_dia_fundamental()
        {
            await CriarDadosBasicosAula(ObterPerfilCP(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_03_01, DATA_28_04, BIMESTRE_1, false);

            await CriarPeriodoEscolarEPeriodoReabertura();

            var retorno = await InserirAulaUseCaseSemValidacaoBasica(TipoAula.Normal, RecorrenciaAula.RepetirBimestreAtual, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, DATA_03_01, false, TIPO_CALENDARIO_1);

            var aulasCadastradas = ObterTodos<Dominio.Aula>();

            aulasCadastradas.Count().ShouldBeEquivalentTo(17);

            aulasCadastradas.Where(w => !w.Excluido).Count().ShouldBeEquivalentTo(17);

            aulasCadastradas.Where(w => w.DisciplinaId == COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString()).Count().ShouldBeEquivalentTo(17);
        }

        [Fact]
        public async Task Nao_Deve_permitir_cadastrar_aula_reposicao_com_recorrencia_no_bimestre_atual()
        {
            var mensagemEsperada = "Não é possível cadastrar aula de reposição com recorrência!";

            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_03_01, DATA_28_04, BIMESTRE_1, false);

            await CriarPeriodoEscolarEPeriodoReabertura();

            var excecao = await Assert.ThrowsAsync<NegocioException>(() => InserirAulaUseCaseSemValidacaoBasica(TipoAula.Reposicao, RecorrenciaAula.RepetirBimestreAtual, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, DATA_03_01, false, TIPO_CALENDARIO_1));

            excecao.Message.ShouldNotBeNullOrEmpty();

            excecao.Message.ShouldBeEquivalentTo(mensagemEsperada);
        }

        [Fact]
        public async Task Nao_Deve_permitir_cadastrar_aula_reposicao_com_recorrencia_em_todos_os_bimestre()
        {
            var mensagemEsperada = "Não é possível cadastrar aula de reposição com recorrência!";

            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_03_01, DATA_28_04, BIMESTRE_1, false);

            await CriarPeriodoEscolarEPeriodoReabertura();

            var excecao = await Assert.ThrowsAsync<NegocioException>(() => InserirAulaUseCaseSemValidacaoBasica(TipoAula.Reposicao, RecorrenciaAula.RepetirTodosBimestres, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, DATA_03_01, false, TIPO_CALENDARIO_1));

            excecao.Message.ShouldNotBeNullOrEmpty();

            excecao.Message.ShouldBeEquivalentTo(mensagemEsperada);
        }

        private async Task CriarPeriodoEscolarEPeriodoReabertura()
        {
            await CriarPeriodoEscolar(DATA_03_01, DATA_28_04, BIMESTRE_1, TIPO_CALENDARIO_1);

            await CriarPeriodoEscolar(DATA_02_05, DATA_08_07, BIMESTRE_2, TIPO_CALENDARIO_1);

            await CriarPeriodoEscolar(DATA_25_07, DATA_30_09, BIMESTRE_3, TIPO_CALENDARIO_1);

            await CriarPeriodoEscolar(DATA_03_10, DATA_22_12, BIMESTRE_4, TIPO_CALENDARIO_1);

            await CriarPeriodoReabertura(TIPO_CALENDARIO_1);
        }
    }
}