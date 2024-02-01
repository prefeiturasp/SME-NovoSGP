using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra.Dtos.ConsultaCriancasEstudantesAusentes;
using SME.SGP.TesteIntegracao.ConsultaCriancasEstudantesAusentes.Base;
using SME.SGP.TesteIntegracao.Setup;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.ConsultaCriancasEstudantesAusentes
{
    public class Ao_obter_alunos_ausentes : ConsultaAlunosAusentesBase
    {
        public Ao_obter_alunos_ausentes(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact(DisplayName = "ConsultaAlunosAusentes - Obter alunos ausentes no dia de hoje")]
        public async Task Ao_obter_alunos_ausentes_no_dia_de_hoje()
        {
            await CriarDadosBasicos();

            await CriarAula(DateTimeExtension.HorarioBrasilia(), RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), TIPO_CALENDARIO_1);

            await CriarFrequencia(AULA_ID_1, (int)TipoFrequencia.F);

            var useCase = ServiceProvider.GetService<IObterTurmasAlunosAusentesUseCase>();
            var filtro = new FiltroObterAlunosAusentesDto()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                CodigoTurma = TURMA_CODIGO_1,
                CodigoUe = UE_CODIGO_1,
                Ausencias = EnumAusencias.NoDiaDeHoje
            };

            var retorno = await useCase.Executar(filtro);
            retorno.ShouldNotBeNull();
            retorno.Count().ShouldBe(2);
            var aluno1 = retorno.FirstOrDefault(aluno => aluno.CodigoEol == ALUNO_CODIGO_1);
            aluno1.ShouldNotBeNull();
            aluno1.DiasSeguidosComAusencia.ShouldBe(1);
        }

        [Fact(DisplayName = "ConsultaAlunosAusentes - Obter alunos ausentes há dois dias seguidos")]
        public async Task Ao_obter_alunos_ausentes_ha_dois_dias_seguidos()
        {
            await CriarDadosBasicos();

            await CriarAula(DATA_02_05, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), TIPO_CALENDARIO_1);
            await CriarAula(DATA_08_07, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), TIPO_CALENDARIO_1);

            await CriarFrequencia(AULA_ID_1, (int)TipoFrequencia.F);
            await CriarFrequencia(AULA_ID_2, (int)TipoFrequencia.F);

            var useCase = ServiceProvider.GetService<IObterTurmasAlunosAusentesUseCase>();
            var filtro = new FiltroObterAlunosAusentesDto()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                CodigoTurma = TURMA_CODIGO_1,
                CodigoUe = UE_CODIGO_1,
                Ausencias = EnumAusencias.Ha2DiasSeguidos
            };

            var retorno = await useCase.Executar(filtro);
            retorno.ShouldNotBeNull();
            retorno.Count().ShouldBe(2);
            var aluno1 = retorno.FirstOrDefault(aluno => aluno.CodigoEol == ALUNO_CODIGO_1);
            aluno1.ShouldNotBeNull();
            aluno1.DiasSeguidosComAusencia.ShouldBe(2);
        }

        [Fact(DisplayName = "ConsultaAlunosAusentes - Obter alunos ausentes há três dias seguidos")]
        public async Task Ao_obter_alunos_ausentes_ha_tres_dias_seguidos()
        {
            await CriarDadosBasicos();

            await CriarAula(DATA_20_07, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), TIPO_CALENDARIO_1);
            await CriarAula(DATA_20_07, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_CIENCIAS_ID_89, TIPO_CALENDARIO_1);

            await CriarFrequencia(AULA_ID_1, (int)TipoFrequencia.F);
            await CriarFrequencia(AULA_ID_2, (int)TipoFrequencia.C);

            await CriarAula(DATA_21_07, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), TIPO_CALENDARIO_1);
            await CriarAula(DATA_22_07, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), TIPO_CALENDARIO_1);
            await CriarAula(DATA_23_07, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), TIPO_CALENDARIO_1);

            await CriarFrequencia(AULA_ID_3, (int)TipoFrequencia.F);
            await CriarFrequencia(AULA_ID_4, (int)TipoFrequencia.F);
            await CriarFrequencia(AULA_ID_5, (int)TipoFrequencia.F);

            var useCase = ServiceProvider.GetService<IObterTurmasAlunosAusentesUseCase>();
            var filtro = new FiltroObterAlunosAusentesDto()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                CodigoTurma = TURMA_CODIGO_1,
                CodigoUe = UE_CODIGO_1,
                Ausencias = EnumAusencias.Ha3DiasSeguidos
            };

            var retorno = await useCase.Executar(filtro);
            retorno.ShouldNotBeNull();
            retorno.Count().ShouldBe(2);
            var aluno1 = retorno.FirstOrDefault(aluno => aluno.CodigoEol == ALUNO_CODIGO_1);
            aluno1.ShouldNotBeNull();
            aluno1.DiasSeguidosComAusencia.ShouldBe(3);
        }

        [Fact(DisplayName = "ConsultaAlunosAusentes - Obter alunos ausentes há quatro dias seguidos")]
        public async Task Ao_obter_alunos_ausentes_ha_quatro_dias_seguidos()
        {
            await CriarDadosBasicos();

            await CriarAula(DATA_20_07, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), TIPO_CALENDARIO_1);
            await CriarAula(DATA_20_07, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_CIENCIAS_ID_89, TIPO_CALENDARIO_1);

            await CriarFrequencia(AULA_ID_1, (int)TipoFrequencia.F);
            await CriarFrequencia(AULA_ID_2, (int)TipoFrequencia.C);

            await CriarAula(DATA_21_07, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), TIPO_CALENDARIO_1);
            await CriarAula(DATA_22_07, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), TIPO_CALENDARIO_1);
            await CriarAula(DATA_23_07, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), TIPO_CALENDARIO_1);
            await CriarAula(DATA_24_07, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), TIPO_CALENDARIO_1);
            await CriarAula(DATA_24_07, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_CIENCIAS_ID_89.ToString(), TIPO_CALENDARIO_1);

            await CriarFrequencia(AULA_ID_3, (int)TipoFrequencia.F);
            await CriarFrequencia(AULA_ID_4, (int)TipoFrequencia.F);
            await CriarFrequencia(AULA_ID_5, (int)TipoFrequencia.F);
            await CriarFrequencia(AULA_ID_6, (int)TipoFrequencia.F);
            await CriarFrequencia(AULA_ID_7, (int)TipoFrequencia.F);

            var useCase = ServiceProvider.GetService<IObterTurmasAlunosAusentesUseCase>();
            var filtro = new FiltroObterAlunosAusentesDto()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                CodigoTurma = TURMA_CODIGO_1,
                CodigoUe = UE_CODIGO_1,
                Ausencias = EnumAusencias.Ha4DiasSeguidos
            };

            var retorno = await useCase.Executar(filtro);
            retorno.ShouldNotBeNull();
            retorno.Count().ShouldBe(2);
            var aluno1 = retorno.FirstOrDefault(aluno => aluno.CodigoEol == ALUNO_CODIGO_1);
            aluno1.ShouldNotBeNull();
            aluno1.DiasSeguidosComAusencia.ShouldBe(4);
        }

        [Fact(DisplayName = "ConsultaAlunosAusentes - Obter alunos ausentes há cinco dias seguidos")]
        public async Task Ao_obter_alunos_ausentes_ha_cinco_dias_seguidos()
        {
            await CriarDadosBasicos();

            await CriarAula(DATA_20_07, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), TIPO_CALENDARIO_1);
            await CriarAula(DATA_20_07, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_CIENCIAS_ID_89, TIPO_CALENDARIO_1);

            await CriarFrequencia(AULA_ID_1, (int)TipoFrequencia.F);
            await CriarFrequencia(AULA_ID_2, (int)TipoFrequencia.C);

            await CriarAula(DATA_21_07, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), TIPO_CALENDARIO_1);
            await CriarAula(DATA_22_07, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), TIPO_CALENDARIO_1);
            await CriarAula(DATA_23_07, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), TIPO_CALENDARIO_1);
            await CriarAula(DATA_24_07, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), TIPO_CALENDARIO_1);
            await CriarAula(DATA_24_07, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_CIENCIAS_ID_89.ToString(), TIPO_CALENDARIO_1);
            await CriarAula(DATA_25_07, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), TIPO_CALENDARIO_1);

            await CriarFrequencia(AULA_ID_3, (int)TipoFrequencia.F);
            await CriarFrequencia(AULA_ID_4, (int)TipoFrequencia.F);
            await CriarFrequencia(AULA_ID_5, (int)TipoFrequencia.F);
            await CriarFrequencia(AULA_ID_6, (int)TipoFrequencia.F);
            await CriarFrequencia(AULA_ID_7, (int)TipoFrequencia.F);
            await CriarFrequencia(AULA_ID_8, (int)TipoFrequencia.F);

            var useCase = ServiceProvider.GetService<IObterTurmasAlunosAusentesUseCase>();
            var filtro = new FiltroObterAlunosAusentesDto()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                CodigoTurma = TURMA_CODIGO_1,
                CodigoUe = UE_CODIGO_1,
                Ausencias = EnumAusencias.Ha5DiasSeguidos
            };

            var retorno = await useCase.Executar(filtro);
            retorno.ShouldNotBeNull();
            retorno.Count().ShouldBe(2);
            var aluno1 = retorno.FirstOrDefault(aluno => aluno.CodigoEol == ALUNO_CODIGO_1);
            aluno1.ShouldNotBeNull();
            aluno1.DiasSeguidosComAusencia.ShouldBe(5);
        }

        [Fact(DisplayName = "ConsultaAlunosAusentes - Obter alunos ausentes entre 6 e 10 dias seguidos")]
        public async Task Ao_obter_alunos_ausentes_entre_6_e_10_dias_seguidos()
        {
            await CriarDadosBasicos();

            await CriarAula(DATA_20_07, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), TIPO_CALENDARIO_1);
            await CriarAula(DATA_20_07, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_CIENCIAS_ID_89, TIPO_CALENDARIO_1);

            await CriarFrequencia(AULA_ID_1, (int)TipoFrequencia.F);
            await CriarFrequencia(AULA_ID_2, (int)TipoFrequencia.C);

            await CriarAula(DATA_21_07, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), TIPO_CALENDARIO_1);
            await CriarAula(DATA_22_07, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), TIPO_CALENDARIO_1);
            await CriarAula(DATA_23_07, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), TIPO_CALENDARIO_1);
            await CriarAula(DATA_24_07, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), TIPO_CALENDARIO_1);
            await CriarAula(DATA_24_07, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_CIENCIAS_ID_89.ToString(), TIPO_CALENDARIO_1);
            await CriarAula(DATA_25_07, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), TIPO_CALENDARIO_1);
            await CriarAula(DATA_26_07, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), TIPO_CALENDARIO_1);
            await CriarAula(DATA_27_07, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), TIPO_CALENDARIO_1);

            await CriarFrequencia(AULA_ID_3, (int)TipoFrequencia.F);
            await CriarFrequencia(AULA_ID_4, (int)TipoFrequencia.F);
            await CriarFrequencia(AULA_ID_5, (int)TipoFrequencia.F);
            await CriarFrequencia(AULA_ID_6, (int)TipoFrequencia.F);
            await CriarFrequencia(AULA_ID_7, (int)TipoFrequencia.F);
            await CriarFrequencia(AULA_ID_8, (int)TipoFrequencia.F);
            await CriarFrequencia(AULA_ID_9, (int)TipoFrequencia.F);

            await CriarFrequencia(AULA_ID_10);
            await CriarFrequenciaAluno(AULA_ID_10, (int)TipoFrequencia.F, ALUNO_CODIGO_2);

            var useCase = ServiceProvider.GetService<IObterTurmasAlunosAusentesUseCase>();
            var filtro = new FiltroObterAlunosAusentesDto()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                CodigoTurma = TURMA_CODIGO_1,
                CodigoUe = UE_CODIGO_1,
                Ausencias = EnumAusencias.Entre6e10DiasSeguidos
            };

            var retorno = await useCase.Executar(filtro);
            retorno.ShouldNotBeNull();
            retorno.Count().ShouldBe(2);
            var aluno1 = retorno.FirstOrDefault(aluno => aluno.CodigoEol == ALUNO_CODIGO_1);
            aluno1.ShouldNotBeNull();
            aluno1.DiasSeguidosComAusencia.ShouldBe(6);
            var aluno2 = retorno.FirstOrDefault(aluno => aluno.CodigoEol == ALUNO_CODIGO_2);
            aluno2.ShouldNotBeNull();
            aluno2.DiasSeguidosComAusencia.ShouldBe(7);
        }

        [Fact(DisplayName = "ConsultaAlunosAusentes - Obter alunos ausentes entre 11 e 15 dias seguidos")]
        public async Task Ao_obter_alunos_ausentes_entre_11_e_15_dias_seguidos()
        {
            await CriarDadosBasicos();

            await CriarAula(DATA_20_07, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), TIPO_CALENDARIO_1);
            await CriarAula(DATA_20_07, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_CIENCIAS_ID_89, TIPO_CALENDARIO_1);

            await CriarFrequencia(AULA_ID_1, (int)TipoFrequencia.F);
            await CriarFrequencia(AULA_ID_2, (int)TipoFrequencia.C);

            await CriarAula(DATA_21_07, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), TIPO_CALENDARIO_1);
            await CriarAula(DATA_22_07, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), TIPO_CALENDARIO_1);
            await CriarAula(DATA_23_07, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), TIPO_CALENDARIO_1);
            await CriarAula(DATA_24_07, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), TIPO_CALENDARIO_1);
            await CriarAula(DATA_24_07, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_CIENCIAS_ID_89.ToString(), TIPO_CALENDARIO_1);
            await CriarAula(DATA_25_07, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), TIPO_CALENDARIO_1);
            await CriarAula(DATA_26_07, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), TIPO_CALENDARIO_1);
            await CriarAula(DATA_27_07, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), TIPO_CALENDARIO_1);
            await CriarAula(DATA_28_07, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), TIPO_CALENDARIO_1);
            await CriarAula(DATA_29_07, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), TIPO_CALENDARIO_1);
            await CriarAula(DATA_30_07, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), TIPO_CALENDARIO_1);
            await CriarAula(DATA_31_07, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), TIPO_CALENDARIO_1);
            await CriarAula(DATA_01_08, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), TIPO_CALENDARIO_1);

            await CriarFrequencia(AULA_ID_3, (int)TipoFrequencia.F);
            await CriarFrequencia(AULA_ID_4, (int)TipoFrequencia.F);
            await CriarFrequencia(AULA_ID_5, (int)TipoFrequencia.F);
            await CriarFrequencia(AULA_ID_6, (int)TipoFrequencia.F);
            await CriarFrequencia(AULA_ID_7, (int)TipoFrequencia.F);
            await CriarFrequencia(AULA_ID_8, (int)TipoFrequencia.F);
            await CriarFrequencia(AULA_ID_9, (int)TipoFrequencia.F);
            await CriarFrequencia(AULA_ID_10, (int)TipoFrequencia.F);
            await CriarFrequencia(AULA_ID_11, (int)TipoFrequencia.F);
            await CriarFrequencia(AULA_ID_12, (int)TipoFrequencia.F);
            await CriarFrequencia(AULA_ID_13, (int)TipoFrequencia.F);
            await CriarFrequencia(AULA_ID_14, (int)TipoFrequencia.F);

            await CriarFrequencia(AULA_ID_15);
            await CriarFrequenciaAluno(AULA_ID_15, (int)TipoFrequencia.F, ALUNO_CODIGO_2);

            var useCase = ServiceProvider.GetService<IObterTurmasAlunosAusentesUseCase>();
            var filtro = new FiltroObterAlunosAusentesDto()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                CodigoTurma = TURMA_CODIGO_1,
                CodigoUe = UE_CODIGO_1,
                Ausencias = EnumAusencias.Entre11e15DiasSeguidos
            };

            var retorno = await useCase.Executar(filtro);
            retorno.ShouldNotBeNull();
            retorno.Count().ShouldBe(2);
            var aluno1 = retorno.FirstOrDefault(aluno => aluno.CodigoEol == ALUNO_CODIGO_1);
            aluno1.ShouldNotBeNull();
            aluno1.DiasSeguidosComAusencia.ShouldBe(11);
            var aluno2 = retorno.FirstOrDefault(aluno => aluno.CodigoEol == ALUNO_CODIGO_2);
            aluno2.ShouldNotBeNull();
            aluno2.DiasSeguidosComAusencia.ShouldBe(12);
        }

        [Fact(DisplayName = "ConsultaAlunosAusentes - Obter alunos ausentes mais de 15 dias seguidos")]
        public async Task Ao_obter_alunos_ausentes_mais_de_15_dias_seguidos()
        {
            await CriarDadosBasicos();

            await CriarAula(DATA_20_07, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), TIPO_CALENDARIO_1);
            await CriarAula(DATA_20_07, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_CIENCIAS_ID_89, TIPO_CALENDARIO_1);

            await CriarFrequencia(AULA_ID_1, (int)TipoFrequencia.F);
            await CriarFrequencia(AULA_ID_2, (int)TipoFrequencia.C);

            await CriarAula(DATA_21_07, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), TIPO_CALENDARIO_1);
            await CriarAula(DATA_22_07, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), TIPO_CALENDARIO_1);
            await CriarAula(DATA_23_07, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), TIPO_CALENDARIO_1);
            await CriarAula(DATA_24_07, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), TIPO_CALENDARIO_1);
            await CriarAula(DATA_24_07, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_CIENCIAS_ID_89.ToString(), TIPO_CALENDARIO_1);
            await CriarAula(DATA_25_07, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), TIPO_CALENDARIO_1);
            await CriarAula(DATA_26_07, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), TIPO_CALENDARIO_1);
            await CriarAula(DATA_27_07, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), TIPO_CALENDARIO_1);
            await CriarAula(DATA_28_07, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), TIPO_CALENDARIO_1);
            await CriarAula(DATA_29_07, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), TIPO_CALENDARIO_1);
            await CriarAula(DATA_30_07, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), TIPO_CALENDARIO_1);
            await CriarAula(DATA_31_07, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), TIPO_CALENDARIO_1);
            await CriarAula(DATA_01_08, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), TIPO_CALENDARIO_1);
            await CriarAula(DATA_02_08, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), TIPO_CALENDARIO_1);
            await CriarAula(DATA_03_08, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), TIPO_CALENDARIO_1);
            await CriarAula(DATA_04_08, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), TIPO_CALENDARIO_1);
            await CriarAula(DATA_05_08, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), TIPO_CALENDARIO_1);
            await CriarAula(DATA_06_08, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), TIPO_CALENDARIO_1);

            await CriarFrequencia(AULA_ID_3, (int)TipoFrequencia.F);
            await CriarFrequencia(AULA_ID_4, (int)TipoFrequencia.F);
            await CriarFrequencia(AULA_ID_5, (int)TipoFrequencia.F);
            await CriarFrequencia(AULA_ID_6, (int)TipoFrequencia.F);
            await CriarFrequencia(AULA_ID_7, (int)TipoFrequencia.F);
            await CriarFrequencia(AULA_ID_8, (int)TipoFrequencia.F);
            await CriarFrequencia(AULA_ID_9, (int)TipoFrequencia.F);
            await CriarFrequencia(AULA_ID_10, (int)TipoFrequencia.F);
            await CriarFrequencia(AULA_ID_11, (int)TipoFrequencia.F);
            await CriarFrequencia(AULA_ID_12, (int)TipoFrequencia.F);
            await CriarFrequencia(AULA_ID_13, (int)TipoFrequencia.F);
            await CriarFrequencia(AULA_ID_14, (int)TipoFrequencia.F);
            await CriarFrequencia(AULA_ID_15, (int) TipoFrequencia.F);
            await CriarFrequencia(AULA_ID_16, (int)TipoFrequencia.F);
            await CriarFrequencia(AULA_ID_17, (int)TipoFrequencia.F);
            await CriarFrequencia(AULA_ID_18, (int)TipoFrequencia.F);
            await CriarFrequencia(AULA_ID_19, (int)TipoFrequencia.F);

            await CriarFrequencia(AULA_ID_20);
            await CriarFrequenciaAluno(AULA_ID_20, (int)TipoFrequencia.F, ALUNO_CODIGO_2);

            var useCase = ServiceProvider.GetService<IObterTurmasAlunosAusentesUseCase>();
            var filtro = new FiltroObterAlunosAusentesDto()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                CodigoTurma = TURMA_CODIGO_1,
                CodigoUe = UE_CODIGO_1,
                Ausencias = EnumAusencias.HaMaisDe15DiasSeguidos
            };

            var retorno = await useCase.Executar(filtro);
            retorno.ShouldNotBeNull();
            retorno.Count().ShouldBe(2);
            var aluno1 = retorno.FirstOrDefault(aluno => aluno.CodigoEol == ALUNO_CODIGO_1);
            aluno1.ShouldNotBeNull();
            aluno1.DiasSeguidosComAusencia.ShouldBe(16);
            var aluno2 = retorno.FirstOrDefault(aluno => aluno.CodigoEol == ALUNO_CODIGO_2);
            aluno2.ShouldNotBeNull();
            aluno2.DiasSeguidosComAusencia.ShouldBe(17);
        }

        [Fact(DisplayName = "ConsultaAlunosAusentes - Obter alunos ausentes há 3 dias nos ultimos 10 dias")]
        public async Task Ao_obter_alunos_ausentes_ha_3_dias_nos_ultimos_10_dias()
        {
            await CriarDadosBasicos();

            await CriarAula(DateTimeExtension.HorarioBrasilia(), RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), TIPO_CALENDARIO_1);
            await CriarAula(DateTimeExtension.HorarioBrasilia().AddDays(-1), RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), TIPO_CALENDARIO_1);
            await CriarAula(DateTimeExtension.HorarioBrasilia().AddDays(-4), RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), TIPO_CALENDARIO_1); 
            await CriarAula(DateTimeExtension.HorarioBrasilia().AddDays(-5), RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), TIPO_CALENDARIO_1);
            await CriarAula(DateTimeExtension.HorarioBrasilia().AddDays(-20), RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), TIPO_CALENDARIO_1);

            await CriarFrequencia(AULA_ID_1, (int)TipoFrequencia.F);
            await CriarFrequencia(AULA_ID_2, (int)TipoFrequencia.F);
            await CriarFrequencia(AULA_ID_3, (int)TipoFrequencia.C);
            await CriarFrequencia(AULA_ID_4, (int)TipoFrequencia.F);
            await CriarFrequencia(AULA_ID_5, (int)TipoFrequencia.F);

            var useCase = ServiceProvider.GetService<IObterTurmasAlunosAusentesUseCase>();
            var filtro = new FiltroObterAlunosAusentesDto()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                CodigoTurma = TURMA_CODIGO_1,
                CodigoUe = UE_CODIGO_1,
                Ausencias = EnumAusencias.TresAusenciasNosUltimos10Dias
            };

            var retorno = await useCase.Executar(filtro);
            retorno.ShouldNotBeNull();
            retorno.Count().ShouldBe(2);
            var aluno1 = retorno.FirstOrDefault(aluno => aluno.CodigoEol == ALUNO_CODIGO_1);
            aluno1.ShouldNotBeNull();
            aluno1.DiasSeguidosComAusencia.ShouldBe(2);
        }

        private async Task CriarFrequencia(int idAula)
        {
            await InserirNaBase(new RegistroFrequencia
            {
                AulaId = idAula,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task CriarFrequencia(int idAula, int tipoFrequencia)
        {
            await CriarFrequencia(idAula);
            await CriarFrequenciaAluno(idAula, tipoFrequencia, ALUNO_CODIGO_1);
            await CriarFrequenciaAluno(idAula, tipoFrequencia, ALUNO_CODIGO_2);
        }

        private async Task CriarFrequenciaAluno(int idAula, int tipoFrequencia, string CodigoAluno)
        {
            await InserirNaBase(new RegistroFrequenciaAluno()
            {
                Valor = tipoFrequencia,
                CodigoAluno = CodigoAluno,
                NumeroAula = NUMERO_AULA_1,
                RegistroFrequenciaId = idAula,
                AulaId = idAula,
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }
    }
}
