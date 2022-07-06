using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.Frequencia
{
    public class Ao_apresentar_os_alunos_na_tela : FrequenciaBase
    {
        public Ao_apresentar_os_alunos_na_tela(CollectionFixture collectionFixture) : base(collectionFixture) { }
        protected readonly DateTime DATA_02_09 = new(DateTimeExtension.HorarioBrasilia().Year, 09, 02);


        [Fact]
        public async Task Alunos_novos_devem_aparecer_com_tooltip_durante_15_dias()
        {
            await CriarDadosBasicos(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_07_08, BIMESTRE_2, DATA_02_05, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), false, NUMERO_AULAS_1);
            await InserirParametroSistema();

            var useCase = ServiceProvider.GetService<IObterFrequenciaPorAulaUseCase>();

            var filtroFrequencia = new FiltroFrequenciaDto()
            {
                AulaId = AULA_ID_1,
                ComponenteCurricularId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138
            };

            var retorno = await useCase.Executar(filtroFrequencia);

            var marcador = retorno.ListaFrequencia.FirstOrDefault(x => x.SituacaoMatricula == ((int)SituacaoMatriculaAluno.Ativo).ToString()).Marcador;

            marcador.ShouldNotBeNull();
        }

        [Fact]
        public async Task Alunos_novos_nao_devem_aparecer_com_tooltip_apos_15_dias()
        {
            await CriarDadosBasicos(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_07_08, BIMESTRE_2, DATA_02_05, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), false, NUMERO_AULAS_1);
            await InserirParametroSistema();

            var useCase = ServiceProvider.GetService<IObterFrequenciaPorAulaUseCase>();

            var filtroFrequencia = new FiltroFrequenciaDto()
            {
                AulaId = AULA_ID_1,
                ComponenteCurricularId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138
            };

            var retorno = await useCase.Executar(filtroFrequencia);
            var marcador = retorno.ListaFrequencia.FirstOrDefault(x => x.CodigoAluno == CODIGO_ALUNO_4).Marcador;

            marcador.ShouldBeNull();
        }

        [Fact]
        public async Task Alunos_inativos_devem_aparecer_com_tooltip_ate_data_de_inativacao()
        {
            await CriarDadosBasicos(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_07_08, BIMESTRE_2, DATA_02_09, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), false, NUMERO_AULAS_1);
            await InserirParametroSistema();

            var useCase = ServiceProvider.GetService<IObterFrequenciaPorAulaUseCase>();

            var filtroFrequencia = new FiltroFrequenciaDto()
            {
                AulaId = AULA_ID_1,
                ComponenteCurricularId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138
            };

            var retorno = await useCase.Executar(filtroFrequencia);

            var marcador = retorno.ListaFrequencia.FirstOrDefault(x => x.CodigoAluno == CODIGO_ALUNO_2).Marcador;

            marcador.ShouldNotBeNull();
        }

        [Fact]
        public async Task Alunos_inativos_antes_do_inicio_do_ano_letivo_nao_devem_aparecer_na_tela()
        {
            await CriarDadosBasicos(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_07_08, BIMESTRE_2, DATA_02_09, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), false, NUMERO_AULAS_1);
            await InserirParametroSistema();

            var useCase = ServiceProvider.GetService<IObterFrequenciaPorAulaUseCase>();

            var filtroFrequencia = new FiltroFrequenciaDto()
            {
                AulaId = AULA_ID_1,
                ComponenteCurricularId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138
            };

            var retorno = await useCase.Executar(filtroFrequencia);

            var aluno = retorno.ListaFrequencia.FirstOrDefault(x => x.CodigoAluno == CODIGO_ALUNO_3);

            aluno.Desabilitado.ShouldBeTrue();
        }


    }
}
