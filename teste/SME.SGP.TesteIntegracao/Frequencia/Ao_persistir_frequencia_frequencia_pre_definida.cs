using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using Xunit;


namespace SME.SGP.TesteIntegracao.Frequencia
{
    public class Ao_persistir_frequencia_frequencia_pre_definida : FrequenciaTesteBase
    {
        public Ao_persistir_frequencia_frequencia_pre_definida(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact(DisplayName = "Frequência - Deve permitir o registro de frequências dos alunos com frequência pré-definida")]
        public async Task Deve_persistir_frequencia_aluno_frequencia_pre_definida()
        {
            var mediator = ServiceProvider.GetService<IMediator>();
            
            await CriarDadosBasicos(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_07_08, BIMESTRE_2, DATA_02_05, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),true, TIPO_CALENDARIO_1,false);

            await InserirNaBase(new RegistroFrequencia(){AulaId = AULA_ID_1, CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF});

            var registroFrequenciaAlunoDtos = ObterRegistroFrequenciaAlunoDto();
            
            var inserirRegistrosFrequenciasAlunosCommand = new InserirRegistrosFrequenciasAlunosCommand(registroFrequenciaAlunoDtos,REGISTRO_FREQUENCIA_ID_1,TURMA_ID_1,COMPONENTE_CURRICULAR_PORTUGUES_ID_138,AULA_ID_1);
            
            var retorno = await mediator.Send(inserirRegistrosFrequenciasAlunosCommand);
            retorno.ShouldBeTrue();

            var registroFrequenciaAlunos = ObterTodos<Dominio.RegistroFrequenciaAluno>();
            registroFrequenciaAlunos.ShouldNotBeNull();
            (registroFrequenciaAlunos.FirstOrDefault(f=> f.CodigoAluno.Equals(ALUNO_CODIGO_1) && f.NumeroAula == NUMERO_AULA_1).Valor == (int)TipoFrequencia.C).ShouldBeTrue();
            (registroFrequenciaAlunos.FirstOrDefault(f=> f.CodigoAluno.Equals(ALUNO_CODIGO_1) && f.NumeroAula == NUMERO_AULA_2).Valor == (int)TipoFrequencia.F).ShouldBeTrue();
            (registroFrequenciaAlunos.FirstOrDefault(f=> f.CodigoAluno.Equals(ALUNO_CODIGO_1) && f.NumeroAula == NUMERO_AULA_3).Valor == (int)TipoFrequencia.R).ShouldBeTrue();
            
            (registroFrequenciaAlunos.FirstOrDefault(f=> f.CodigoAluno.Equals(ALUNO_CODIGO_2) && f.NumeroAula == NUMERO_AULA_1).Valor == (int)TipoFrequencia.C).ShouldBeTrue();
            (registroFrequenciaAlunos.FirstOrDefault(f=> f.CodigoAluno.Equals(ALUNO_CODIGO_2) && f.NumeroAula == NUMERO_AULA_2).Valor == (int)TipoFrequencia.F).ShouldBeTrue();
            (registroFrequenciaAlunos.FirstOrDefault(f=> f.CodigoAluno.Equals(ALUNO_CODIGO_2) && f.NumeroAula == NUMERO_AULA_3).Valor == (int)TipoFrequencia.R).ShouldBeTrue();
            
            (registroFrequenciaAlunos.FirstOrDefault(f=> f.CodigoAluno.Equals(ALUNO_CODIGO_3) && f.NumeroAula == NUMERO_AULA_1).Valor == (int)TipoFrequencia.C).ShouldBeTrue();
            (registroFrequenciaAlunos.FirstOrDefault(f=> f.CodigoAluno.Equals(ALUNO_CODIGO_3) && f.NumeroAula == NUMERO_AULA_2).Valor == (int)TipoFrequencia.F).ShouldBeTrue();
            (registroFrequenciaAlunos.FirstOrDefault(f=> f.CodigoAluno.Equals(ALUNO_CODIGO_3) && f.NumeroAula == NUMERO_AULA_3).Valor == (int)TipoFrequencia.R).ShouldBeTrue();
            
            (registroFrequenciaAlunos.FirstOrDefault(f=> f.CodigoAluno.Equals(ALUNO_CODIGO_4) && f.NumeroAula == NUMERO_AULA_1).Valor == (int)TipoFrequencia.C).ShouldBeTrue();
            (registroFrequenciaAlunos.FirstOrDefault(f=> f.CodigoAluno.Equals(ALUNO_CODIGO_4) && f.NumeroAula == NUMERO_AULA_2).Valor == (int)TipoFrequencia.F).ShouldBeTrue();
            (registroFrequenciaAlunos.FirstOrDefault(f=> f.CodigoAluno.Equals(ALUNO_CODIGO_4) && f.NumeroAula == NUMERO_AULA_3).Valor == (int)TipoFrequencia.R).ShouldBeTrue();
            
            var frequenciaPreDefinidas = ObterTodos<Dominio.FrequenciaPreDefinida>();
            frequenciaPreDefinidas.ShouldNotBeNull();
            (frequenciaPreDefinidas.FirstOrDefault(f=> f.CodigoAluno.Equals(ALUNO_CODIGO_1) && f.TurmaId == TURMA_ID_1 && f.ComponenteCurricularId == COMPONENTE_CURRICULAR_PORTUGUES_ID_138).TipoFrequencia == TipoFrequencia.C).ShouldBeTrue();
            (frequenciaPreDefinidas.FirstOrDefault(f=> f.CodigoAluno.Equals(ALUNO_CODIGO_2) && f.TurmaId == TURMA_ID_1 && f.ComponenteCurricularId == COMPONENTE_CURRICULAR_PORTUGUES_ID_138).TipoFrequencia == TipoFrequencia.F).ShouldBeTrue();
            (frequenciaPreDefinidas.FirstOrDefault(f=> f.CodigoAluno.Equals(ALUNO_CODIGO_3) && f.TurmaId == TURMA_ID_1 && f.ComponenteCurricularId == COMPONENTE_CURRICULAR_PORTUGUES_ID_138).TipoFrequencia == TipoFrequencia.R).ShouldBeTrue();
            (frequenciaPreDefinidas.FirstOrDefault(f=> f.CodigoAluno.Equals(ALUNO_CODIGO_4) && f.TurmaId == TURMA_ID_1 && f.ComponenteCurricularId == COMPONENTE_CURRICULAR_PORTUGUES_ID_138).TipoFrequencia == TipoFrequencia.C).ShouldBeTrue();
        }
        
        private List<RegistroFrequenciaAlunoDto> ObterRegistroFrequenciaAlunoDto()
        {
            var registroFrequenciaAlunoDtos = new List<RegistroFrequenciaAlunoDto>();
            
            var frequenciaAulaDtos = new List<FrequenciaAulaDto>()
            {
                new () {NumeroAula = NUMERO_AULA_1, TipoFrequencia = TipoFrequencia.C.ShortName()},
                new () {NumeroAula = NUMERO_AULA_2, TipoFrequencia = TipoFrequencia.F.ShortName()},
                new () {NumeroAula = NUMERO_AULA_3, TipoFrequencia = TipoFrequencia.R.ShortName()},
            };
            
            registroFrequenciaAlunoDtos.Add(new RegistroFrequenciaAlunoDto() {CodigoAluno = ALUNO_CODIGO_1, Aulas = frequenciaAulaDtos, TipoFrequenciaPreDefinido = TipoFrequencia.C.ShortName()});
            registroFrequenciaAlunoDtos.Add(new RegistroFrequenciaAlunoDto() {CodigoAluno = ALUNO_CODIGO_2, Aulas = frequenciaAulaDtos, TipoFrequenciaPreDefinido = TipoFrequencia.F.ShortName()});
            registroFrequenciaAlunoDtos.Add(new RegistroFrequenciaAlunoDto() {CodigoAluno = ALUNO_CODIGO_3, Aulas = frequenciaAulaDtos, TipoFrequenciaPreDefinido = TipoFrequencia.R.ShortName()});
            registroFrequenciaAlunoDtos.Add(new RegistroFrequenciaAlunoDto() {CodigoAluno = ALUNO_CODIGO_4, Aulas = frequenciaAulaDtos, TipoFrequenciaPreDefinido = TipoFrequencia.C.ShortName()});

            return registroFrequenciaAlunoDtos;
        }
    }
}
