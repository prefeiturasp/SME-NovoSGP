using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra.Dtos.ConsultaCriancasEstudantesAusentes;
using SME.SGP.TesteIntegracao.ConsultaCriancasEstudantesAusentes.Base;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.ConsultaCriancasEstudantesAusentes
{
    [Collection(nameof(ConsultaCriancaEstudantesAusentesTestFixture))]
    public class Ao_obter_alunos_ausentes : IDisposable
    {
        public DateTime DATA_02_05 = new(DateTimeExtension.HorarioBrasilia().Year, 05, 02);
        public DateTime DATA_08_07 = new(DateTimeExtension.HorarioBrasilia().Year, 07, 08);
        public DateTime DATA_07_08 = new(DateTimeExtension.HorarioBrasilia().Year, 08, 07);

        public DateTime DATA_20_07 = new(DateTimeExtension.HorarioBrasilia().Year, 07, 20);
        public DateTime DATA_21_07 = new(DateTimeExtension.HorarioBrasilia().Year, 07, 21);
        public DateTime DATA_22_07 = new(DateTimeExtension.HorarioBrasilia().Year, 07, 22);
        public DateTime DATA_23_07 = new(DateTimeExtension.HorarioBrasilia().Year, 07, 23);
        public DateTime DATA_24_07 = new(DateTimeExtension.HorarioBrasilia().Year, 07, 24);

        public DateTime DATA_25_07 = new(DateTimeExtension.HorarioBrasilia().Year, 07, 25);
        public DateTime DATA_26_07 = new(DateTimeExtension.HorarioBrasilia().Year, 07, 26);
        public DateTime DATA_27_07 = new(DateTimeExtension.HorarioBrasilia().Year, 07, 27);
        public DateTime DATA_28_07 = new(DateTimeExtension.HorarioBrasilia().Year, 07, 28);
        public DateTime DATA_29_07 = new(DateTimeExtension.HorarioBrasilia().Year, 07, 29);
        public DateTime DATA_30_07 = new(DateTimeExtension.HorarioBrasilia().Year, 07, 30);
        public DateTime DATA_31_07 = new(DateTimeExtension.HorarioBrasilia().Year, 07, 31);
        public DateTime DATA_01_08 = new(DateTimeExtension.HorarioBrasilia().Year, 08, 01);
        public DateTime DATA_02_08 = new(DateTimeExtension.HorarioBrasilia().Year, 08, 02);
        public DateTime DATA_03_08 = new(DateTimeExtension.HorarioBrasilia().Year, 08, 03);
        public DateTime DATA_04_08 = new(DateTimeExtension.HorarioBrasilia().Year, 08, 04);
        public DateTime DATA_05_08 = new(DateTimeExtension.HorarioBrasilia().Year, 08, 05);
        public DateTime DATA_06_08 = new(DateTimeExtension.HorarioBrasilia().Year, 08, 06);

        private readonly ConsultaCriancaEstudantesAusentesTestFixture _fixture;

        public Ao_obter_alunos_ausentes(ConsultaCriancaEstudantesAusentesTestFixture collectionFixture) 
        {
            _fixture = collectionFixture;
        }

        [Fact(DisplayName = "ConsultaAlunosAusentes - Obter alunos ausentes no dia de hoje")]
        public async Task Ao_obter_alunos_ausentes_no_dia_de_hoje()
        {
            //await CriarDadosBasicos();

            var aula1 = await _fixture.CriarAula(DateTimeExtension.HorarioBrasilia(), RecorrenciaAula.AulaUnica, TipoAula.Normal, Base.Constantes.USUARIO_PROFESSOR_CODIGO_RF_1111111, Base.Constantes.TURMA_CODIGO_1, Base.Constantes.UE_CODIGO_1, Base.Constantes.COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), Base.Constantes.TIPO_CALENDARIO_1);

            var RegistroFrequenciaAluno = _fixture.ObterTodos<RegistroFrequenciaAluno>();
            var RegistroFrequencia = _fixture.ObterTodos<RegistroFrequencia>();
            await CriarFrequencia(aula1, (int)TipoFrequencia.F);

            var useCase = _fixture.ServiceProvider.GetService<IObterTurmasAlunosAusentesUseCase>();
            var filtro = new FiltroObterAlunosAusentesDto()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                CodigoTurma = Base.Constantes.TURMA_CODIGO_1,
                CodigoUe = Base.Constantes.UE_CODIGO_1,
                Ausencias = EnumAusencias.NoDiaDeHoje
            };

            var retorno = await useCase.Executar(filtro);
            retorno.ShouldNotBeNull();
            retorno.Count().ShouldBe(2);
            var aluno1 = retorno.FirstOrDefault(aluno => aluno.CodigoEol == Base.Constantes.ALUNO_CODIGO_1);
            aluno1.ShouldNotBeNull();
            aluno1.DiasSeguidosComAusencia.ShouldBe(1);

            await _fixture.ExcluirTodos<RegistroFrequenciaAluno>();
            await _fixture.ExcluirTodos<RegistroFrequencia>();
        }

        [Fact(DisplayName = "ConsultaAlunosAusentes - Obter alunos ausentes há dois dias seguidos")]
        public async Task Ao_obter_alunos_ausentes_ha_dois_dias_seguidos()
        {

            var aulaId1 = await _fixture.CriarAula(DATA_02_05, RecorrenciaAula.AulaUnica, TipoAula.Normal, Base.Constantes.USUARIO_PROFESSOR_CODIGO_RF_1111111, Base.Constantes.TURMA_CODIGO_1, Base.Constantes.UE_CODIGO_1, Base.Constantes.COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), Base.Constantes.TIPO_CALENDARIO_1);
            var aulaId2 = await _fixture.CriarAula(DATA_08_07, RecorrenciaAula.AulaUnica, TipoAula.Normal, Base.Constantes.USUARIO_PROFESSOR_CODIGO_RF_1111111, Base.Constantes.TURMA_CODIGO_1, Base.Constantes.UE_CODIGO_1, Base.Constantes.COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), Base.Constantes.TIPO_CALENDARIO_1);

            await CriarFrequencia(aulaId1, (int)TipoFrequencia.F);
            await CriarFrequencia(aulaId2, (int)TipoFrequencia.F);

            var useCase = _fixture.ServiceProvider.GetService<IObterTurmasAlunosAusentesUseCase>();
            var filtro = new FiltroObterAlunosAusentesDto()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                CodigoTurma = Base.Constantes.TURMA_CODIGO_1,
                CodigoUe = Base.Constantes.UE_CODIGO_1,
                Ausencias = EnumAusencias.Ha2DiasSeguidos
            };

            var retorno = await useCase.Executar(filtro);
            retorno.ShouldNotBeNull();
            retorno.Count().ShouldBe(2);
            var aluno1 = retorno.FirstOrDefault(aluno => aluno.CodigoEol == Base.Constantes.ALUNO_CODIGO_1);
            aluno1.ShouldNotBeNull();
            aluno1.DiasSeguidosComAusencia.ShouldBe(2);

            await _fixture.ExcluirTodos<RegistroFrequenciaAluno>();
            var RegistroFrequenciaAluno =_fixture.ObterTodos<RegistroFrequenciaAluno>();
            await _fixture.ExcluirTodos<RegistroFrequencia>();
            var RegistroFrequencia = _fixture.ObterTodos<RegistroFrequencia>();
        }

        [Fact(DisplayName = "ConsultaAlunosAusentes - Obter alunos ausentes há três dias seguidos")]
        public async Task Ao_obter_alunos_ausentes_ha_tres_dias_seguidos()
        {
            var aula1 = await _fixture.CriarAula(DATA_20_07, RecorrenciaAula.AulaUnica, TipoAula.Normal, Base.Constantes.USUARIO_PROFESSOR_CODIGO_RF_1111111, Base.Constantes.TURMA_CODIGO_1, Base.Constantes.UE_CODIGO_1, Base.Constantes.COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), Base.Constantes.TIPO_CALENDARIO_1);
            var aula2 = await _fixture.CriarAula(DATA_20_07, RecorrenciaAula.AulaUnica, TipoAula.Normal, Base.Constantes.USUARIO_PROFESSOR_CODIGO_RF_1111111, Base.Constantes.TURMA_CODIGO_1, Base.Constantes.UE_CODIGO_1, Base.Constantes.COMPONENTE_CIENCIAS_ID_89, Base.Constantes.TIPO_CALENDARIO_1);

            var RegistroFrequenciaAluno = _fixture.ObterTodos<RegistroFrequenciaAluno>();
            var RegistroFrequencia = _fixture.ObterTodos<RegistroFrequencia>();

            await CriarFrequencia(aula1, (int)TipoFrequencia.F);
            await CriarFrequencia(aula2, (int)TipoFrequencia.C);

            var aula3 = await _fixture.CriarAula(DATA_21_07, RecorrenciaAula.AulaUnica, TipoAula.Normal, Base.Constantes.USUARIO_PROFESSOR_CODIGO_RF_1111111, Base.Constantes.TURMA_CODIGO_1, Base.Constantes.UE_CODIGO_1, Base.Constantes.COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), Base.Constantes.TIPO_CALENDARIO_1);
            var aula4 = await _fixture.CriarAula(DATA_22_07, RecorrenciaAula.AulaUnica, TipoAula.Normal, Base.Constantes.USUARIO_PROFESSOR_CODIGO_RF_1111111, Base.Constantes.TURMA_CODIGO_1, Base.Constantes.UE_CODIGO_1, Base.Constantes.COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), Base.Constantes.TIPO_CALENDARIO_1);
            var aula5 = await _fixture.CriarAula(DATA_23_07, RecorrenciaAula.AulaUnica, TipoAula.Normal, Base.Constantes.USUARIO_PROFESSOR_CODIGO_RF_1111111, Base.Constantes.TURMA_CODIGO_1, Base.Constantes.UE_CODIGO_1, Base.Constantes.COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), Base.Constantes.TIPO_CALENDARIO_1);

             await CriarFrequencia(aula3, (int)TipoFrequencia.F);
            await CriarFrequencia(aula4, (int)TipoFrequencia.F);
            await CriarFrequencia(aula5, (int)TipoFrequencia.F);

            var useCase = _fixture.ServiceProvider.GetService<IObterTurmasAlunosAusentesUseCase>();
            var filtro = new FiltroObterAlunosAusentesDto()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                CodigoTurma = Base.Constantes.TURMA_CODIGO_1,
                CodigoUe = Base.Constantes.UE_CODIGO_1,
                Ausencias = EnumAusencias.Ha3DiasSeguidos
            };

            var retorno = await useCase.Executar(filtro);
            retorno.ShouldNotBeNull();
            retorno.Count().ShouldBe(2);
            var aluno1 = retorno.FirstOrDefault(aluno => aluno.CodigoEol == Base.Constantes.ALUNO_CODIGO_1);
            aluno1.ShouldNotBeNull();
            aluno1.DiasSeguidosComAusencia.ShouldBe(3);

            await _fixture.ExcluirTodos<RegistroFrequenciaAluno>();
            await _fixture.ExcluirTodos<RegistroFrequencia>();
        }

        private async Task<long> CriarFrequencia(long idAula)
        {
            return await _fixture.SalvarNaBase(new RegistroFrequencia
            {
                AulaId = idAula,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = Base.Constantes.SISTEMA_NOME,
                CriadoRF = Base.Constantes.SISTEMA_CODIGO_RF
            });
        }

        private async Task CriarFrequencia(long idAula, int tipoFrequencia)
        {
            var idfrequencia = await CriarFrequencia(idAula);
            await CriarFrequenciaAluno(idfrequencia, idAula, tipoFrequencia, Base.Constantes.ALUNO_CODIGO_1);
            await CriarFrequenciaAluno(idfrequencia, idAula, tipoFrequencia, Base.Constantes.ALUNO_CODIGO_2);
        }

        private async Task CriarFrequenciaAluno(long idfrequencia, long idAula, int tipoFrequencia, string CodigoAluno)
        {
            await _fixture.InserirNaBase(new RegistroFrequenciaAluno()
            {
                Valor = tipoFrequencia,
                CodigoAluno = CodigoAluno,
                NumeroAula = Base.Constantes.NUMERO_AULA_1,
                RegistroFrequenciaId = idfrequencia,
                AulaId = idAula,
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date,
                CriadoPor = Base.Constantes.SISTEMA_NOME,
                CriadoRF = Base.Constantes.SISTEMA_CODIGO_RF
            });
        }

        public void Dispose()
        {
        }
    }
}
