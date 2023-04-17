using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SME.SGP.Infra;
using Xunit;

namespace SME.SGP.TesteIntegracao.AulaRecorrencia
{
    public class Ao_excluir_aula_com_recorrencia : AulaTeste
    {
        public Ao_excluir_aula_com_recorrencia(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }
        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery, bool>), typeof(ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQueryHandlerComPermissaoFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQuery, IEnumerable<ComponenteCurricularEol>>), typeof(ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQueryHandlerFakePortugues), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName = "Aula - Deve permitir excluir aula recorrente no bimestre")]
        public async Task Excluir_aula_com_recorrencia_no_bimestre()
        {
            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, false);

            await CriarAula(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), DATA_02_05, RecorrenciaAula.RepetirBimestreAtual);
            
            await CriaAulaRecorrentePortugues(RecorrenciaAula.RepetirBimestreAtual);

            await CriarPeriodoEscolarEAbertura();
            
            await CriarFrequencia(); 
                
            await CriarCompensacaoAusencia();

            var useCaseExcluirAulaUseCase = ServiceProvider.GetService<IExcluirAulaUseCase>();
            
            var excluirAulaDto = ObterExcluirAulaDto(RecorrenciaAula.RepetirBimestreAtual);

            var retorno = await useCaseExcluirAulaUseCase.Executar(excluirAulaDto);
            retorno.ShouldNotBeNull();

            var aulas = ObterTodos<Dominio.Aula>();
            aulas.ShouldNotBeEmpty();
            aulas.FirstOrDefault().Excluido.ShouldBe(true);
            
            var mensagem = new MensagemRabbit(
                JsonConvert.SerializeObject(new FiltroIdDto(AULA_ID)),
                Guid.NewGuid(),
                USUARIO_PROFESSOR_LOGIN_2222222,
                USUARIO_PROFESSOR_LOGIN_2222222,
                Guid.Parse(PerfilUsuario.PROFESSOR.Name()),
                false,
                TesteBaseComuns.USUARIO_ADMIN_RF);
             
            //Essa fila está dentro do processo do ExcluirAulaUseCase e está sendo chamada aqui de forma exclusiva
            var excluirCompensacaoAusenciaPorAulaIdUseCase = ServiceProvider.GetService<IExcluirCompensacaoAusenciaAlunoEAulaPorAulaIdUseCase>();
            await excluirCompensacaoAusenciaPorAulaIdUseCase.Executar(mensagem);
            
            var compensacoesCompensacaoAusenciaAlunos = ObterTodos<Dominio.CompensacaoAusenciaAluno>();
            compensacoesCompensacaoAusenciaAlunos.Any(a=> a.Excluido).ShouldBeTrue();
            compensacoesCompensacaoAusenciaAlunos.Any(a=> !a.Excluido).ShouldBeFalse();
            
            var compensacaoAusenciaAlunoAula = ObterTodos<Dominio.CompensacaoAusenciaAlunoAula>();
            compensacaoAusenciaAlunoAula.Any(a=> a.Excluido).ShouldBeTrue();
            compensacaoAusenciaAlunoAula.Any(a=> !a.Excluido).ShouldBeFalse();
        }

        [Fact(DisplayName = "Aula - Deve permitir excluir aula recorrente em todos os bimestre")]
        public async Task Excluir_aula_com_recorrencia_em_todos_os_bimestres()
        {
            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, false);

            await CriarAula(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), DATA_02_05, RecorrenciaAula.RepetirTodosBimestres);
            
            await CriaAulaRecorrentePortugues(RecorrenciaAula.RepetirTodosBimestres);

            await CriarPeriodoEscolarEAbertura();
            
            await CriarFrequencia(); 
                
            await CriarCompensacaoAusencia();

            var useCaseExcluirAulaUseCase = ServiceProvider.GetService<IExcluirAulaUseCase>();
            
            var excluirAulaDto = ObterExcluirAulaDto(RecorrenciaAula.RepetirTodosBimestres);

            var retorno = await useCaseExcluirAulaUseCase.Executar(excluirAulaDto);
            retorno.ShouldNotBeNull();

            var aulas = ObterTodos<Dominio.Aula>();
            aulas.ShouldNotBeEmpty();
            aulas.FirstOrDefault().Excluido.ShouldBe(true);
            
            var mensagem = new MensagemRabbit(
                JsonConvert.SerializeObject(new FiltroIdDto(AULA_ID)),
                Guid.NewGuid(),
                USUARIO_PROFESSOR_LOGIN_2222222,
                USUARIO_PROFESSOR_LOGIN_2222222,
                Guid.Parse(PerfilUsuario.PROFESSOR.Name()),
                false,
                TesteBaseComuns.USUARIO_ADMIN_RF);
             
            //Essa fila está dentro do processo do ExcluirAulaUseCase e está sendo chamada aqui de forma exclusiva 
            var excluirCompensacaoAusenciaPorAulaIdUseCase = ServiceProvider.GetService<IExcluirCompensacaoAusenciaAlunoEAulaPorAulaIdUseCase>();
            await excluirCompensacaoAusenciaPorAulaIdUseCase.Executar(mensagem);
            
            var compensacoesCompensacaoAusenciaAlunos = ObterTodos<Dominio.CompensacaoAusenciaAluno>();
            compensacoesCompensacaoAusenciaAlunos.Any(a=> a.Excluido).ShouldBeTrue();
            compensacoesCompensacaoAusenciaAlunos.Any(a=> !a.Excluido).ShouldBeFalse();
            
            var compensacaoAusenciaAlunoAula = ObterTodos<Dominio.CompensacaoAusenciaAlunoAula>();
            compensacaoAusenciaAlunoAula.Any(a=> a.Excluido).ShouldBeTrue();
            compensacaoAusenciaAlunoAula.Any(a=> !a.Excluido).ShouldBeFalse();
        }

        [Fact(DisplayName = "Aula - Não deve permitir excluir aula que tenha avaliação vinculada")]
        public async Task Aula_com_avaliacao_vinculada()
        {
            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, false);
            
            await CriarAula(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), DATA_02_05, RecorrenciaAula.RepetirBimestreAtual);
            
            await CriaAulaRecorrentePortugues(RecorrenciaAula.RepetirBimestreAtual);
            
            await CriarAtividadeAvaliativaFundamental(DATA_02_05, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString());

            await CriarPeriodoEscolarEAbertura();

            var useCaseExcluirAulaUseCase = ServiceProvider.GetService<IExcluirAulaUseCase>();
            
            var excluirAulaDto = ObterExcluirAulaDto(RecorrenciaAula.RepetirBimestreAtual);
            
            var retorno = await useCaseExcluirAulaUseCase.Executar(excluirAulaDto);
            retorno.ShouldNotBeNull();

            var aulas = ObterTodos<Notificacao>();
            aulas.ShouldNotBeEmpty();
            aulas.FirstOrDefault().Mensagem.ShouldContain("Aula com avaliação vinculada. Para excluir esta aula primeiro deverá ser excluída a avaliação.");
        }
        
        private async Task CriarPeriodoEscolarEAbertura()
        {
            await CriarPeriodoEscolar(DATA_03_01_INICIO_BIMESTRE_1, DATA_28_04_FIM_BIMESTRE_1, BIMESTRE_1);

            await CriarPeriodoEscolar(DATA_02_05_INICIO_BIMESTRE_2, DATA_08_07_FIM_BIMESTRE_2, BIMESTRE_2);

            await CriarPeriodoEscolar(DATA_25_07_INICIO_BIMESTRE_3, DATA_30_09_FIM_BIMESTRE_3, BIMESTRE_3);

            await CriarPeriodoEscolar(DATA_03_10_INICIO_BIMESTRE_4, DATA_22_12_FIM_BIMESTRE_4, BIMESTRE_4);

            await CriarPeriodoReabertura(TIPO_CALENDARIO_1);
        }
        
        private async Task CriarFrequencia()
        {
            await InserirNaBase(new RegistroFrequencia
            {
                AulaId = AULA_ID,
                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF
            });
            
            await InserirNaBase(new RegistroFrequenciaAluno()
            {
                Valor = (int)TipoFrequencia.F, 
                CodigoAluno = ALUNO_CODIGO_1, 
                NumeroAula = NUMERO_AULA_1, 
                RegistroFrequenciaId = 1, 
                AulaId = AULA_ID_1, 
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
            
            await InserirNaBase(new RegistroFrequenciaAluno()
            {
                Valor = (int)TipoFrequencia.F, 
                CodigoAluno = ALUNO_CODIGO_1, 
                NumeroAula = NUMERO_AULA_2, 
                RegistroFrequenciaId = 1, 
                AulaId = AULA_ID_1, 
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
            
            await InserirNaBase(new RegistroFrequenciaAluno()
            {
                Valor = (int)TipoFrequencia.F, 
                CodigoAluno = ALUNO_CODIGO_1, 
                NumeroAula = NUMERO_AULA_3, 
                RegistroFrequenciaId = 1, 
                AulaId = AULA_ID_1, 
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task CriarCompensacaoAusencia()
        {
            await InserirNaBase(new CompensacaoAusencia
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                DisciplinaId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                Bimestre = BIMESTRE_2,
                TurmaId = TURMA_ID_1,
                Nome = "Atividade de compensação",
                Descricao = "Breve descrição da atividade de compensação",
                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new CompensacaoAusenciaAluno
            {
                CodigoAluno = CODIGO_ALUNO_1,
                CompensacaoAusenciaId = 1,
                QuantidadeFaltasCompensadas = NUMERO_AULA_3,
                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new CompensacaoAusenciaAlunoAula()
            {
                DataAula = DATA_02_05,
                NumeroAula = NUMERO_AULA_1,
                CompensacaoAusenciaAlunoId = 1,
                RegistroFrequenciaAlunoId = 1,
                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
            
            await InserirNaBase(new CompensacaoAusenciaAlunoAula()
            {
                DataAula = DATA_02_05,
                NumeroAula = NUMERO_AULA_2,
                CompensacaoAusenciaAlunoId = 1,
                RegistroFrequenciaAlunoId = 2,
                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
            
            await InserirNaBase(new CompensacaoAusenciaAlunoAula()
            {
                DataAula = DATA_02_05,
                NumeroAula = NUMERO_AULA_3,
                CompensacaoAusenciaAlunoId = 1,
                RegistroFrequenciaAlunoId = 3,
                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
        }
    }
}
