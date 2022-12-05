﻿using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Shouldly;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.Frequencia
{
    public class Ao_excluir_ou_alterar_aula_com_frequencia : FrequenciaTesteBase
    {
        public Ao_excluir_ou_alterar_aula_com_frequencia(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        public async Task Ao_excluir_aula_com_frequencia_e_calculo()
        {
            await CriarDadosBasicos(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05_INICIO_BIMESTRE_2, DATA_08_07_FIM_BIMESTRE_2, BIMESTRE_2, DATA_02_05_INICIO_BIMESTRE_2, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), true);
            await CriarRegistrosConsolidacaoFrequenciaAlunoMensal();
            await CrieRegistroDeFrenquencia();
            await CrieFrenquenciaAluno(CODIGO_ALUNO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), DATA_02_05_INICIO_BIMESTRE_2, DATA_08_07_FIM_BIMESTRE_2, BIMESTRE_2);
            await CrieFrenquenciaAluno(CODIGO_ALUNO_2, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), DATA_02_05_INICIO_BIMESTRE_2, DATA_08_07_FIM_BIMESTRE_2, BIMESTRE_2);

            var useCase = ServiceProvider.GetService<IExcluirAulaUseCase>();

            var dto = new ExcluirAulaDto()
            {
                AulaId = 1,
                RecorrenciaAula = RecorrenciaAula.AulaUnica
            };

            await useCase.Executar(dto);

            var consolidacaoDashBoardFrequencias = ObterTodos<Dominio.FrequenciaAluno>();
            consolidacaoDashBoardFrequencias.ShouldNotBeEmpty();
            consolidacaoDashBoardFrequencias.Count().ShouldBe(0);
        }

        [Fact]
        public async Task Ao_diminuir_quantidade_de_aula_a_frequencia_deve_ser_excluida()
        {
            await CriarDadosBasicos(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_25_07_INICIO_BIMESTRE_3, DATA_30_09_FIM_BIMESTRE_3, BIMESTRE_3, DATA_30_09_FIM_BIMESTRE_3, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), true, TIPO_CALENDARIO_1, false, QUANTIDADE_AULA_2);
            await CriarRegistrosConsolidacaoFrequenciaAlunoMensal();
            await CrieRegistroDeFrenquencia();

            var dto = new AulaAlterarFrequenciaRequestDto(AULA_ID_1, QUANTIDADE_AULA_3);
            var mensagem = new MensagemRabbit()
            {
                Mensagem = JsonConvert.SerializeObject(dto)
            };
            var useCase = ServiceProvider.GetService<IAlterarAulaFrequenciaTratarUseCase>();

            await useCase.Executar(mensagem);

            var listaDeRegistroFrequencia = ObterTodos<RegistroFrequenciaAluno>();
            listaDeRegistroFrequencia.ShouldNotBeEmpty();
            listaDeRegistroFrequencia.Exists(frequencia => frequencia.NumeroAula == QUANTIDADE_AULA_3).ShouldBe(false);
        }

        public async Task Ao_aumentar_quantidade_de_aula_a_frequencia_anterior_deve_ser_replicada()
        {
            await CriarDadosBasicos(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05_INICIO_BIMESTRE_2, DATA_08_07_FIM_BIMESTRE_2, BIMESTRE_2, DATA_02_05_INICIO_BIMESTRE_2, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), true, TIPO_CALENDARIO_1, false, QUANTIDADE_AULA_4);
            await CriarRegistrosConsolidacaoFrequenciaAlunoMensal();
            await CrieRegistroDeFrenquencia();

            var dto = new AulaAlterarFrequenciaRequestDto(AULA_ID_1, QUANTIDADE_AULA_3);
            var mensagem = new MensagemRabbit()
            {
                Mensagem = JsonConvert.SerializeObject(dto)
            };
            var useCase = ServiceProvider.GetService<IAlterarAulaFrequenciaTratarUseCase>();

            await useCase.Executar(mensagem);

            var listaDeRegistroFrequencia = ObterTodos<RegistroFrequenciaAluno>();
            listaDeRegistroFrequencia.ShouldNotBeEmpty();
            var registroAula4 = listaDeRegistroFrequencia.Find(frequencia => frequencia.NumeroAula == QUANTIDADE_AULA_4);
            registroAula4.ShouldNotBeNull();
            registroAula4.Valor.ShouldBe((int)TipoFrequencia.F);
        }
    }
}
