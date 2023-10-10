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
using SME.SGP.Dominio.Constantes.MensagensNegocio;
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

            var dataAula = DateTimeExtension.HorarioBrasilia().Date;
            
            await InserirNaBase(new RegistroFrequencia(){AulaId = AULA_ID_1, CriadoEm = dataAula, CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF});

            var registroFrequenciaAlunoDtos = ObterRegistroFrequenciaAlunoDto();
            
            var inserirRegistrosFrequenciasAlunosCommand = new InserirRegistrosFrequenciasAlunosCommand(registroFrequenciaAlunoDtos,REGISTRO_FREQUENCIA_ID_1,TURMA_ID_1,COMPONENTE_CURRICULAR_PORTUGUES_ID_138,AULA_ID_1,dataAula);
            
            var retorno = await mediator.Send(inserirRegistrosFrequenciasAlunosCommand);
            retorno.ShouldBeTrue();

            var registroFrequenciaAlunos = ObterTodos<Dominio.RegistroFrequenciaAluno>();
            registroFrequenciaAlunos.Count().ShouldBeEquivalentTo(12);
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
            frequenciaPreDefinidas.Count().ShouldBeEquivalentTo(4);
            (frequenciaPreDefinidas.FirstOrDefault(f=> f.CodigoAluno.Equals(ALUNO_CODIGO_1) && f.TurmaId == TURMA_ID_1 && f.ComponenteCurricularId == COMPONENTE_CURRICULAR_PORTUGUES_ID_138).TipoFrequencia == TipoFrequencia.C).ShouldBeTrue();
            (frequenciaPreDefinidas.FirstOrDefault(f=> f.CodigoAluno.Equals(ALUNO_CODIGO_2) && f.TurmaId == TURMA_ID_1 && f.ComponenteCurricularId == COMPONENTE_CURRICULAR_PORTUGUES_ID_138).TipoFrequencia == TipoFrequencia.F).ShouldBeTrue();
            (frequenciaPreDefinidas.FirstOrDefault(f=> f.CodigoAluno.Equals(ALUNO_CODIGO_3) && f.TurmaId == TURMA_ID_1 && f.ComponenteCurricularId == COMPONENTE_CURRICULAR_PORTUGUES_ID_138).TipoFrequencia == TipoFrequencia.R).ShouldBeTrue();
            (frequenciaPreDefinidas.FirstOrDefault(f=> f.CodigoAluno.Equals(ALUNO_CODIGO_4) && f.TurmaId == TURMA_ID_1 && f.ComponenteCurricularId == COMPONENTE_CURRICULAR_PORTUGUES_ID_138).TipoFrequencia == TipoFrequencia.C).ShouldBeTrue();
        }

        [Fact(DisplayName = "Frequência - Não deve permitir o registro de frequências dos alunos com frequência pré-definida quando com rollback")]
        public async Task Nao_deve_persistir_frequencia_aluno_frequencia_pre_definida_quando_com_rollback()
        {
            var mediator = ServiceProvider.GetService<IMediator>();

            await CriarDadosBasicos(ObterPerfilProfessor(), Modalidade.Fundamental,ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_07_08, BIMESTRE_2, DATA_02_05,
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), true, TIPO_CALENDARIO_1, false);

            var dataAula = DateTimeExtension.HorarioBrasilia().Date;

            var registroFrequenciaAlunoDtos = ObterRegistroFrequenciaAlunoDto();

            var inserirRegistrosFrequenciasAlunosCommand = new InserirRegistrosFrequenciasAlunosCommand(
                registroFrequenciaAlunoDtos, REGISTRO_FREQUENCIA_ID_2, TURMA_ID_1,
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138, AULA_ID_1, dataAula);

            var retorno = await mediator.Send(inserirRegistrosFrequenciasAlunosCommand);
            
            retorno.ShouldBeFalse();
            
            var registroFrequenciaAlunos = ObterTodos<Dominio.RegistroFrequenciaAluno>();
            registroFrequenciaAlunos.Count().ShouldBeEquivalentTo(0);
            
            var frequenciaPreDefinidas = ObterTodos<Dominio.FrequenciaPreDefinida>();
            frequenciaPreDefinidas.Count().ShouldBeEquivalentTo(0);
        }
        
        [Fact(DisplayName = "Frequência - Ao avaliar o que será alterado ou inserido na frequência")]
        public async Task Ao_avaliar_o_que_sera_alterado_inserido()
        {
            var mediator = ServiceProvider.GetService<IMediator>();
            
            await CriarDadosBasicos(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_07_08, BIMESTRE_2, DATA_02_05, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),true, TIPO_CALENDARIO_1,false);
            
            var dataAula = DateTimeExtension.HorarioBrasilia().Date;
            
            //Foram criadas 4 aulas - (1 no CriarDadosBasicos e 3 aqui)
            await CriarAula(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), dataAula.AddDays(-1), RecorrenciaAula.AulaUnica, NUMERO_AULA_2);
            await CriarAula(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), dataAula.AddDays(-2), RecorrenciaAula.AulaUnica, NUMERO_AULA_4);
            await CriarAula(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), dataAula.AddDays(-3), RecorrenciaAula.AulaUnica, NUMERO_AULA_2);
            
            //Frequência para aula 1
            await InserirNaBase(new RegistroFrequencia(){AulaId = AULA_ID_1, CriadoEm = dataAula, CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF});
            await InserirNaBase(ObterRegistroFrequenciaAluno(TipoFrequencia.C, ALUNO_CODIGO_1, NUMERO_AULA_1, REGISTRO_FREQUENCIA_ID_1, AULA_ID_1));
            await InserirNaBase(ObterRegistroFrequenciaAluno(TipoFrequencia.F, ALUNO_CODIGO_1, NUMERO_AULA_2, REGISTRO_FREQUENCIA_ID_1, AULA_ID_1));
            await InserirNaBase(ObterRegistroFrequenciaAluno(TipoFrequencia.R, ALUNO_CODIGO_1, NUMERO_AULA_3, REGISTRO_FREQUENCIA_ID_1, AULA_ID_1));
            
            await InserirNaBase(ObterRegistroFrequenciaAluno(TipoFrequencia.F, ALUNO_CODIGO_2, NUMERO_AULA_1, REGISTRO_FREQUENCIA_ID_1, AULA_ID_1));
            await InserirNaBase(ObterRegistroFrequenciaAluno(TipoFrequencia.C, ALUNO_CODIGO_2, NUMERO_AULA_2, REGISTRO_FREQUENCIA_ID_1, AULA_ID_1));
            await InserirNaBase(ObterRegistroFrequenciaAluno(TipoFrequencia.F, ALUNO_CODIGO_2, NUMERO_AULA_3, REGISTRO_FREQUENCIA_ID_1, AULA_ID_1));
            
            await InserirNaBase(ObterRegistroFrequenciaAluno(TipoFrequencia.R, ALUNO_CODIGO_3, NUMERO_AULA_1, REGISTRO_FREQUENCIA_ID_1, AULA_ID_1));
            await InserirNaBase(ObterRegistroFrequenciaAluno(TipoFrequencia.R, ALUNO_CODIGO_3, NUMERO_AULA_2, REGISTRO_FREQUENCIA_ID_1, AULA_ID_1));
            await InserirNaBase(ObterRegistroFrequenciaAluno(TipoFrequencia.C, ALUNO_CODIGO_3, NUMERO_AULA_3, REGISTRO_FREQUENCIA_ID_1, AULA_ID_1));
            
            await InserirNaBase(ObterRegistroFrequenciaAluno(TipoFrequencia.C, ALUNO_CODIGO_4, NUMERO_AULA_1, REGISTRO_FREQUENCIA_ID_1, AULA_ID_1));
            await InserirNaBase(ObterRegistroFrequenciaAluno(TipoFrequencia.C, ALUNO_CODIGO_4, NUMERO_AULA_2, REGISTRO_FREQUENCIA_ID_1, AULA_ID_1));
            await InserirNaBase(ObterRegistroFrequenciaAluno(TipoFrequencia.F, ALUNO_CODIGO_4, NUMERO_AULA_3, REGISTRO_FREQUENCIA_ID_1, AULA_ID_1));
            
            //Freqüência para aula 2
            await InserirNaBase(new RegistroFrequencia(){AulaId = AULA_ID_2, CriadoEm = dataAula.AddDays(-1), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF});
            await InserirNaBase(ObterRegistroFrequenciaAluno(TipoFrequencia.R, ALUNO_CODIGO_1, NUMERO_AULA_1, REGISTRO_FREQUENCIA_ID_2, AULA_ID_2));
            await InserirNaBase(ObterRegistroFrequenciaAluno(TipoFrequencia.R, ALUNO_CODIGO_1, NUMERO_AULA_2, REGISTRO_FREQUENCIA_ID_2, AULA_ID_2));
            await InserirNaBase(ObterRegistroFrequenciaAluno(TipoFrequencia.F, ALUNO_CODIGO_1, NUMERO_AULA_3, REGISTRO_FREQUENCIA_ID_2, AULA_ID_2));
            
            await InserirNaBase(ObterRegistroFrequenciaAluno(TipoFrequencia.C, ALUNO_CODIGO_2, NUMERO_AULA_1, REGISTRO_FREQUENCIA_ID_2, AULA_ID_2));
            await InserirNaBase(ObterRegistroFrequenciaAluno(TipoFrequencia.F, ALUNO_CODIGO_2, NUMERO_AULA_2, REGISTRO_FREQUENCIA_ID_2, AULA_ID_2));
            await InserirNaBase(ObterRegistroFrequenciaAluno(TipoFrequencia.R, ALUNO_CODIGO_2, NUMERO_AULA_3, REGISTRO_FREQUENCIA_ID_2, AULA_ID_2));
            
            await InserirNaBase(ObterRegistroFrequenciaAluno(TipoFrequencia.C, ALUNO_CODIGO_3, NUMERO_AULA_1, REGISTRO_FREQUENCIA_ID_2, AULA_ID_2));
            await InserirNaBase(ObterRegistroFrequenciaAluno(TipoFrequencia.F, ALUNO_CODIGO_3, NUMERO_AULA_2, REGISTRO_FREQUENCIA_ID_2, AULA_ID_2));
            await InserirNaBase(ObterRegistroFrequenciaAluno(TipoFrequencia.R, ALUNO_CODIGO_3, NUMERO_AULA_3, REGISTRO_FREQUENCIA_ID_2, AULA_ID_2));
            
            await InserirNaBase(ObterRegistroFrequenciaAluno(TipoFrequencia.F, ALUNO_CODIGO_4, NUMERO_AULA_1, REGISTRO_FREQUENCIA_ID_2, AULA_ID_2));
            await InserirNaBase(ObterRegistroFrequenciaAluno(TipoFrequencia.R, ALUNO_CODIGO_4, NUMERO_AULA_2, REGISTRO_FREQUENCIA_ID_2, AULA_ID_2));
            await InserirNaBase(ObterRegistroFrequenciaAluno(TipoFrequencia.C, ALUNO_CODIGO_4, NUMERO_AULA_3, REGISTRO_FREQUENCIA_ID_2, AULA_ID_2));
            
            //Freqüência para aula 3
            await InserirNaBase(new RegistroFrequencia(){AulaId = AULA_ID_3, CriadoEm = dataAula.AddDays(-2), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF});
            await InserirNaBase(ObterRegistroFrequenciaAluno(TipoFrequencia.R, ALUNO_CODIGO_1, NUMERO_AULA_1, REGISTRO_FREQUENCIA_ID_3, AULA_ID_3));
            await InserirNaBase(ObterRegistroFrequenciaAluno(TipoFrequencia.R, ALUNO_CODIGO_1, NUMERO_AULA_2, REGISTRO_FREQUENCIA_ID_3, AULA_ID_3));
            await InserirNaBase(ObterRegistroFrequenciaAluno(TipoFrequencia.F, ALUNO_CODIGO_1, NUMERO_AULA_3, REGISTRO_FREQUENCIA_ID_3, AULA_ID_3));
            
            await InserirNaBase(ObterRegistroFrequenciaAluno(TipoFrequencia.C, ALUNO_CODIGO_2, NUMERO_AULA_1, REGISTRO_FREQUENCIA_ID_3, AULA_ID_3));
            await InserirNaBase(ObterRegistroFrequenciaAluno(TipoFrequencia.F, ALUNO_CODIGO_2, NUMERO_AULA_2, REGISTRO_FREQUENCIA_ID_3, AULA_ID_3));
            await InserirNaBase(ObterRegistroFrequenciaAluno(TipoFrequencia.R, ALUNO_CODIGO_2, NUMERO_AULA_3, REGISTRO_FREQUENCIA_ID_3, AULA_ID_3));
            
            await InserirNaBase(ObterRegistroFrequenciaAluno(TipoFrequencia.C, ALUNO_CODIGO_3, NUMERO_AULA_1, REGISTRO_FREQUENCIA_ID_3, AULA_ID_3));
            await InserirNaBase(ObterRegistroFrequenciaAluno(TipoFrequencia.F, ALUNO_CODIGO_3, NUMERO_AULA_2, REGISTRO_FREQUENCIA_ID_3, AULA_ID_3));
            await InserirNaBase(ObterRegistroFrequenciaAluno(TipoFrequencia.R, ALUNO_CODIGO_3, NUMERO_AULA_3, REGISTRO_FREQUENCIA_ID_3, AULA_ID_3));
            
            await InserirNaBase(ObterRegistroFrequenciaAluno(TipoFrequencia.F, ALUNO_CODIGO_4, NUMERO_AULA_1, REGISTRO_FREQUENCIA_ID_3, AULA_ID_3));
            await InserirNaBase(ObterRegistroFrequenciaAluno(TipoFrequencia.R, ALUNO_CODIGO_4, NUMERO_AULA_2, REGISTRO_FREQUENCIA_ID_3, AULA_ID_3));
            await InserirNaBase(ObterRegistroFrequenciaAluno(TipoFrequencia.C, ALUNO_CODIGO_4, NUMERO_AULA_3, REGISTRO_FREQUENCIA_ID_3, AULA_ID_3));
            
            //Freqüência para aula 4
            await InserirNaBase(new RegistroFrequencia(){AulaId = AULA_ID_4, CriadoEm = dataAula.AddDays(-3), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF});
            await InserirNaBase(ObterRegistroFrequenciaAluno(TipoFrequencia.R, ALUNO_CODIGO_1, NUMERO_AULA_1, REGISTRO_FREQUENCIA_ID_4, AULA_ID_4));
            await InserirNaBase(ObterRegistroFrequenciaAluno(TipoFrequencia.R, ALUNO_CODIGO_1, NUMERO_AULA_2, REGISTRO_FREQUENCIA_ID_4, AULA_ID_4));
            await InserirNaBase(ObterRegistroFrequenciaAluno(TipoFrequencia.F, ALUNO_CODIGO_1, NUMERO_AULA_3, REGISTRO_FREQUENCIA_ID_4, AULA_ID_4));
            
            await InserirNaBase(ObterRegistroFrequenciaAluno(TipoFrequencia.C, ALUNO_CODIGO_2, NUMERO_AULA_1, REGISTRO_FREQUENCIA_ID_4, AULA_ID_4));
            await InserirNaBase(ObterRegistroFrequenciaAluno(TipoFrequencia.F, ALUNO_CODIGO_2, NUMERO_AULA_2, REGISTRO_FREQUENCIA_ID_4, AULA_ID_4));
            await InserirNaBase(ObterRegistroFrequenciaAluno(TipoFrequencia.R, ALUNO_CODIGO_2, NUMERO_AULA_3, REGISTRO_FREQUENCIA_ID_4, AULA_ID_4));
            
            await InserirNaBase(ObterRegistroFrequenciaAluno(TipoFrequencia.C, ALUNO_CODIGO_3, NUMERO_AULA_1, REGISTRO_FREQUENCIA_ID_4, AULA_ID_4));
            await InserirNaBase(ObterRegistroFrequenciaAluno(TipoFrequencia.F, ALUNO_CODIGO_3, NUMERO_AULA_2, REGISTRO_FREQUENCIA_ID_4, AULA_ID_4));
            await InserirNaBase(ObterRegistroFrequenciaAluno(TipoFrequencia.R, ALUNO_CODIGO_3, NUMERO_AULA_3, REGISTRO_FREQUENCIA_ID_4, AULA_ID_4));
            
            await InserirNaBase(ObterRegistroFrequenciaAluno(TipoFrequencia.F, ALUNO_CODIGO_4, NUMERO_AULA_1, REGISTRO_FREQUENCIA_ID_4, AULA_ID_4));
            await InserirNaBase(ObterRegistroFrequenciaAluno(TipoFrequencia.R, ALUNO_CODIGO_4, NUMERO_AULA_2, REGISTRO_FREQUENCIA_ID_4, AULA_ID_4));
            await InserirNaBase(ObterRegistroFrequenciaAluno(TipoFrequencia.C, ALUNO_CODIGO_4, NUMERO_AULA_3, REGISTRO_FREQUENCIA_ID_4, AULA_ID_4));

            //Alterando a aula e frequencia do aluno 1
            var registroFrequenciaAlunoAlteradosDtos = new List<RegistroFrequenciaAlunoDto>()
            {
                ObterRegistroFrequenciaAlunoPara3Aulas(ALUNO_CODIGO_1, TipoFrequencia.R.ShortName(), TipoFrequencia.C.ShortName(),TipoFrequencia.R.ShortName()),
                ObterRegistroFrequenciaAlunoPara3Aulas(ALUNO_CODIGO_2, TipoFrequencia.F.ShortName(), TipoFrequencia.F.ShortName(),TipoFrequencia.R.ShortName()),
                ObterRegistroFrequenciaAlunoPara3Aulas(ALUNO_CODIGO_3, TipoFrequencia.C.ShortName(), TipoFrequencia.F.ShortName(),TipoFrequencia.R.ShortName()),
                ObterRegistroFrequenciaAlunoPara3Aulas(ALUNO_CODIGO_4, TipoFrequencia.R.ShortName(), TipoFrequencia.C.ShortName(),TipoFrequencia.C.ShortName()),
                ObterRegistroFrequenciaAlunoPara3Aulas(ALUNO_CODIGO_5, TipoFrequencia.C.ShortName(), TipoFrequencia.F.ShortName(),TipoFrequencia.R.ShortName()),
                ObterRegistroFrequenciaAlunoPara3Aulas(ALUNO_CODIGO_6, TipoFrequencia.R.ShortName(), TipoFrequencia.R.ShortName(),TipoFrequencia.R.ShortName()),
            };

            var inserirRegistrosFrequenciasAlunosCommand = new InserirRegistrosFrequenciasAlunosCommand(registroFrequenciaAlunoAlteradosDtos,REGISTRO_FREQUENCIA_ID_1,TURMA_ID_1,COMPONENTE_CURRICULAR_PORTUGUES_ID_138,AULA_ID_1,dataAula);
            
            //Validações
            var retorno = await mediator.Send(inserirRegistrosFrequenciasAlunosCommand);
            retorno.ShouldBeTrue();

            var registroFrequenciaAlunos = ObterTodos<Dominio.RegistroFrequenciaAluno>();
            registroFrequenciaAlunos.Count().ShouldBeEquivalentTo(54);
            (registroFrequenciaAlunos.FirstOrDefault(f=> f.CodigoAluno.Equals(ALUNO_CODIGO_1) && f.NumeroAula == NUMERO_AULA_1 && f.RegistroFrequenciaId == REGISTRO_FREQUENCIA_ID_1).Valor == (int)TipoFrequencia.R).ShouldBeTrue();
            (registroFrequenciaAlunos.FirstOrDefault(f=> f.CodigoAluno.Equals(ALUNO_CODIGO_1) && f.NumeroAula == NUMERO_AULA_2 && f.RegistroFrequenciaId == REGISTRO_FREQUENCIA_ID_1).Valor == (int)TipoFrequencia.C).ShouldBeTrue();
            (registroFrequenciaAlunos.FirstOrDefault(f=> f.CodigoAluno.Equals(ALUNO_CODIGO_1) && f.NumeroAula == NUMERO_AULA_3 && f.RegistroFrequenciaId == REGISTRO_FREQUENCIA_ID_1).Valor == (int)TipoFrequencia.R).ShouldBeTrue();
            (registroFrequenciaAlunos.FirstOrDefault(f=> f.CodigoAluno.Equals(ALUNO_CODIGO_1) && f.NumeroAula == NUMERO_AULA_1 && f.RegistroFrequenciaId == REGISTRO_FREQUENCIA_ID_1).Valor == (int)TipoFrequencia.C).ShouldBeFalse();
            (registroFrequenciaAlunos.FirstOrDefault(f=> f.CodigoAluno.Equals(ALUNO_CODIGO_1) && f.NumeroAula == NUMERO_AULA_2 && f.RegistroFrequenciaId == REGISTRO_FREQUENCIA_ID_1).Valor == (int)TipoFrequencia.F).ShouldBeFalse();
            
            (registroFrequenciaAlunos.FirstOrDefault(f=> f.CodigoAluno.Equals(ALUNO_CODIGO_2) && f.NumeroAula == NUMERO_AULA_1 && f.RegistroFrequenciaId == REGISTRO_FREQUENCIA_ID_1).Valor == (int)TipoFrequencia.F).ShouldBeTrue();
            (registroFrequenciaAlunos.FirstOrDefault(f=> f.CodigoAluno.Equals(ALUNO_CODIGO_2) && f.NumeroAula == NUMERO_AULA_2 && f.RegistroFrequenciaId == REGISTRO_FREQUENCIA_ID_1).Valor == (int)TipoFrequencia.F).ShouldBeTrue();
            (registroFrequenciaAlunos.FirstOrDefault(f=> f.CodigoAluno.Equals(ALUNO_CODIGO_2) && f.NumeroAula == NUMERO_AULA_3 && f.RegistroFrequenciaId == REGISTRO_FREQUENCIA_ID_1).Valor == (int)TipoFrequencia.R).ShouldBeTrue();
            (registroFrequenciaAlunos.FirstOrDefault(f=> f.CodigoAluno.Equals(ALUNO_CODIGO_2) && f.NumeroAula == NUMERO_AULA_2 && f.RegistroFrequenciaId == REGISTRO_FREQUENCIA_ID_1).Valor == (int)TipoFrequencia.C).ShouldBeFalse();
            (registroFrequenciaAlunos.FirstOrDefault(f=> f.CodigoAluno.Equals(ALUNO_CODIGO_2) && f.NumeroAula == NUMERO_AULA_3 && f.RegistroFrequenciaId == REGISTRO_FREQUENCIA_ID_1).Valor == (int)TipoFrequencia.F).ShouldBeFalse();
            
            (registroFrequenciaAlunos.FirstOrDefault(f=> f.CodigoAluno.Equals(ALUNO_CODIGO_3) && f.NumeroAula == NUMERO_AULA_1 && f.RegistroFrequenciaId == REGISTRO_FREQUENCIA_ID_1).Valor == (int)TipoFrequencia.C).ShouldBeTrue();
            (registroFrequenciaAlunos.FirstOrDefault(f=> f.CodigoAluno.Equals(ALUNO_CODIGO_3) && f.NumeroAula == NUMERO_AULA_2 && f.RegistroFrequenciaId == REGISTRO_FREQUENCIA_ID_1).Valor == (int)TipoFrequencia.F).ShouldBeTrue();
            (registroFrequenciaAlunos.FirstOrDefault(f=> f.CodigoAluno.Equals(ALUNO_CODIGO_3) && f.NumeroAula == NUMERO_AULA_3 && f.RegistroFrequenciaId == REGISTRO_FREQUENCIA_ID_1).Valor == (int)TipoFrequencia.R).ShouldBeTrue();
            (registroFrequenciaAlunos.FirstOrDefault(f=> f.CodigoAluno.Equals(ALUNO_CODIGO_3) && f.NumeroAula == NUMERO_AULA_1 && f.RegistroFrequenciaId == REGISTRO_FREQUENCIA_ID_1).Valor == (int)TipoFrequencia.R).ShouldBeFalse();
            (registroFrequenciaAlunos.FirstOrDefault(f=> f.CodigoAluno.Equals(ALUNO_CODIGO_3) && f.NumeroAula == NUMERO_AULA_2 && f.RegistroFrequenciaId == REGISTRO_FREQUENCIA_ID_1).Valor == (int)TipoFrequencia.R).ShouldBeFalse();
            (registroFrequenciaAlunos.FirstOrDefault(f=> f.CodigoAluno.Equals(ALUNO_CODIGO_3) && f.NumeroAula == NUMERO_AULA_3 && f.RegistroFrequenciaId == REGISTRO_FREQUENCIA_ID_1).Valor == (int)TipoFrequencia.C).ShouldBeFalse();
            
            (registroFrequenciaAlunos.FirstOrDefault(f=> f.CodigoAluno.Equals(ALUNO_CODIGO_4) && f.NumeroAula == NUMERO_AULA_1 && f.RegistroFrequenciaId == REGISTRO_FREQUENCIA_ID_1).Valor == (int)TipoFrequencia.R).ShouldBeTrue();
            (registroFrequenciaAlunos.FirstOrDefault(f=> f.CodigoAluno.Equals(ALUNO_CODIGO_4) && f.NumeroAula == NUMERO_AULA_2 && f.RegistroFrequenciaId == REGISTRO_FREQUENCIA_ID_1).Valor == (int)TipoFrequencia.C).ShouldBeTrue();
            (registroFrequenciaAlunos.FirstOrDefault(f=> f.CodigoAluno.Equals(ALUNO_CODIGO_4) && f.NumeroAula == NUMERO_AULA_3 && f.RegistroFrequenciaId == REGISTRO_FREQUENCIA_ID_1).Valor == (int)TipoFrequencia.C).ShouldBeTrue();
            (registroFrequenciaAlunos.FirstOrDefault(f=> f.CodigoAluno.Equals(ALUNO_CODIGO_4) && f.NumeroAula == NUMERO_AULA_1 && f.RegistroFrequenciaId == REGISTRO_FREQUENCIA_ID_1).Valor == (int)TipoFrequencia.C).ShouldBeFalse();
            (registroFrequenciaAlunos.FirstOrDefault(f=> f.CodigoAluno.Equals(ALUNO_CODIGO_4) && f.NumeroAula == NUMERO_AULA_3 && f.RegistroFrequenciaId == REGISTRO_FREQUENCIA_ID_1).Valor == (int)TipoFrequencia.F).ShouldBeFalse();
            
            (registroFrequenciaAlunos.FirstOrDefault(f=> f.CodigoAluno.Equals(ALUNO_CODIGO_5) && f.NumeroAula == NUMERO_AULA_1 && f.RegistroFrequenciaId == REGISTRO_FREQUENCIA_ID_1).Valor == (int)TipoFrequencia.C).ShouldBeTrue();
            (registroFrequenciaAlunos.FirstOrDefault(f=> f.CodigoAluno.Equals(ALUNO_CODIGO_5) && f.NumeroAula == NUMERO_AULA_2 && f.RegistroFrequenciaId == REGISTRO_FREQUENCIA_ID_1).Valor == (int)TipoFrequencia.F).ShouldBeTrue();
            (registroFrequenciaAlunos.FirstOrDefault(f=> f.CodigoAluno.Equals(ALUNO_CODIGO_5) && f.NumeroAula == NUMERO_AULA_3 && f.RegistroFrequenciaId == REGISTRO_FREQUENCIA_ID_1).Valor == (int)TipoFrequencia.R).ShouldBeTrue();
            
            (registroFrequenciaAlunos.FirstOrDefault(f=> f.CodigoAluno.Equals(ALUNO_CODIGO_6) && f.NumeroAula == NUMERO_AULA_1 && f.RegistroFrequenciaId == REGISTRO_FREQUENCIA_ID_1).Valor == (int)TipoFrequencia.R).ShouldBeTrue();
            (registroFrequenciaAlunos.FirstOrDefault(f=> f.CodigoAluno.Equals(ALUNO_CODIGO_6) && f.NumeroAula == NUMERO_AULA_2 && f.RegistroFrequenciaId == REGISTRO_FREQUENCIA_ID_1).Valor == (int)TipoFrequencia.R).ShouldBeTrue();
            (registroFrequenciaAlunos.FirstOrDefault(f=> f.CodigoAluno.Equals(ALUNO_CODIGO_6) && f.NumeroAula == NUMERO_AULA_3 && f.RegistroFrequenciaId == REGISTRO_FREQUENCIA_ID_1).Valor == (int)TipoFrequencia.R).ShouldBeTrue();
            
            var frequenciaPreDefinidas = ObterTodos<Dominio.FrequenciaPreDefinida>();
            frequenciaPreDefinidas.Count().ShouldBeEquivalentTo(6);
            frequenciaPreDefinidas.Any(f=> f.TipoFrequencia == TipoFrequencia.C).ShouldBeTrue();
            frequenciaPreDefinidas.Any(f=> f.TipoFrequencia == TipoFrequencia.F).ShouldBeFalse();
            frequenciaPreDefinidas.Any(f=> f.TipoFrequencia == TipoFrequencia.R).ShouldBeFalse();
        }

        private RegistroFrequenciaAlunoDto ObterRegistroFrequenciaAlunoPara3Aulas(string alunoCodigo, string tipoFrequenciaAula1, string tipoFrequenciaAula2, string tipoFrequenciaAula3)
        {
            return new()
            {
                CodigoAluno = alunoCodigo, TipoFrequenciaPreDefinido = TipoFrequencia.C.ShortName(),
                Aulas = new List<FrequenciaAulaDto>()
                {
                    new() { NumeroAula = NUMERO_AULA_1, TipoFrequencia = tipoFrequenciaAula1 },
                    new() { NumeroAula = NUMERO_AULA_2, TipoFrequencia = tipoFrequenciaAula2 },
                    new() { NumeroAula = NUMERO_AULA_3, TipoFrequencia = tipoFrequenciaAula3 },
                }
            };
        }

        private RegistroFrequenciaAluno ObterRegistroFrequenciaAluno(TipoFrequencia tipoFrequencia, string alunoCodigo, int numeroAula, long registroFrequenciaId, long aulaId)
        {
            return new RegistroFrequenciaAluno()
            {
                Valor = (int)tipoFrequencia, 
                CodigoAluno = alunoCodigo, 
                NumeroAula = numeroAula, 
                RegistroFrequenciaId = registroFrequenciaId, 
                AulaId = aulaId, 
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            };
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
