using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.CompensacaoDeAusencia.Base;
using SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes;
using SME.SGP.TesteIntegracao.Nota.ServicosFakes;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;

namespace SME.SGP.TesteIntegracao.CompensacaoDeAusencia
{
    public class Ao_validar_exclusao_compensacao_ausencia: CompensacaoDeAusenciaTesteBase
    {
        public Ao_validar_exclusao_compensacao_ausencia(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosPorTurmaEDataMatriculaQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterAlunosPorTurmaEDataMatriculaQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterValorParametroSistemaTipoEAnoQuery, string>), typeof(ObterValorParametroSistemaTipoEAnoQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosEolPorTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterAlunosEolPorTurmaQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTodosAlunosNaTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterTodosAlunosNaTurmaQueryHandlerFake), ServiceLifetime.Scoped));            
        }
        
        [Fact(DisplayName = "Compensação de Ausência - Deve excluir todas as compensações de ausência que não tem compensação aluno e aula")]
        public async Task Deve_excluir_compensacao_sem_aluno_e_aula()
        {
            var dtoDadoBase = ObterDtoDadoBase(ObterPerfilProfessor(), COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString());
            await CriarDadosBase(dtoDadoBase);
            
            await InserirNaBase(new Dominio.Aula
                {
                    UeId = UE_CODIGO_1,
                    DisciplinaId = COMPONENTE_CURRICULAR_ARTES_ID_139.ToString(),
                    TurmaId = TURMA_CODIGO_1,
                    TipoCalendarioId = dtoDadoBase.TipoCalendarioId,
                    ProfessorRf = USUARIO_PROFESSOR_LOGIN_2222222,
                    Quantidade = dtoDadoBase.QuantidadeAula,
                    DataAula = dtoDadoBase.DataReferencia.GetValueOrDefault(),
                    RecorrenciaAula = RecorrenciaAula.AulaUnica,
                    TipoAula = TipoAula.Normal,CriadoEm = DateTime.Now,CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF,
                    AulaCJ = dtoDadoBase.AulaCj
                });

            await CriarFrequencia(true);
            await CriarCompensacaoAusencia(true);

            var mensagem = new MensagemRabbit(
                JsonConvert.SerializeObject(new FiltroCompensacaoAusenciaDto(new long[]{1,2})),
                Guid.NewGuid(),
                USUARIO_PROFESSOR_LOGIN_2222222,
                USUARIO_PROFESSOR_LOGIN_2222222,
                Guid.Parse(PerfilUsuario.PROFESSOR.Name()),
                false,
                TesteBaseComuns.USUARIO_ADMIN_RF);
             
            var excluirCompensacaoAusenciaPorIdsUseCase = ServiceProvider.GetService<IExcluirCompensacaoAusenciaPorIdsUseCase >();
            await excluirCompensacaoAusenciaPorIdsUseCase.Executar(mensagem);
            
            var compensacoesCompensacaoAusencia = ObterTodos<CompensacaoAusencia>();
            compensacoesCompensacaoAusencia.Count(a=> a.Excluido).Equals(2).ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Compensação de Ausência - Deve excluir somente a primeira compensação de ausência que não tem compensação aluno e aula")]
        public async Task Deve_excluir_compensacao_sem_aluno_e_aula_somente_primeira_compensacao()
        {
            var dtoDadoBase = ObterDtoDadoBase(ObterPerfilProfessor(), COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString());
            await CriarDadosBase(dtoDadoBase);
            
            await InserirNaBase(new Dominio.Aula
                {
                    UeId = UE_CODIGO_1,
                    DisciplinaId = COMPONENTE_CURRICULAR_ARTES_ID_139.ToString(),
                    TurmaId = TURMA_CODIGO_1,
                    TipoCalendarioId = dtoDadoBase.TipoCalendarioId,
                    ProfessorRf = USUARIO_PROFESSOR_LOGIN_2222222,
                    Quantidade = dtoDadoBase.QuantidadeAula,
                    DataAula = dtoDadoBase.DataReferencia.GetValueOrDefault(),
                    RecorrenciaAula = RecorrenciaAula.AulaUnica,
                    TipoAula = TipoAula.Normal,CriadoEm = DateTime.Now,CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF,
                    AulaCJ = dtoDadoBase.AulaCj
                });

            await CriarFrequencia(true);
            await CriarCompensacaoAusencia(true);

            var mensagem = new MensagemRabbit(
                JsonConvert.SerializeObject(new FiltroCompensacaoAusenciaDto(new long[]{1})),
                Guid.NewGuid(),
                USUARIO_PROFESSOR_LOGIN_2222222,
                USUARIO_PROFESSOR_LOGIN_2222222,
                Guid.Parse(PerfilUsuario.PROFESSOR.Name()),
                false,
                TesteBaseComuns.USUARIO_ADMIN_RF);
             
            var excluirCompensacaoAusenciaPorIdsUseCase = ServiceProvider.GetService<IExcluirCompensacaoAusenciaPorIdsUseCase >();
            await excluirCompensacaoAusenciaPorIdsUseCase.Executar(mensagem);
            
            var compensacoesCompensacaoAusencia = ObterTodos<CompensacaoAusencia>();
            compensacoesCompensacaoAusencia.Count(a=> a.Excluido).Equals(1).ShouldBeTrue();
            compensacoesCompensacaoAusencia.Count(a=> !a.Excluido).Equals(1).ShouldBeTrue();
        }
        
        private async Task CriarFrequencia(bool excluida = false)
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
                Excluido = excluida,
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
            
            await InserirNaBase(new RegistroFrequenciaAluno()
            {
                Valor = (int)TipoFrequencia.F, 
                CodigoAluno = ALUNO_CODIGO_1, 
                NumeroAula = NUMERO_AULA_2, 
                RegistroFrequenciaId = 1, 
                AulaId = AULA_ID_1, 
                Excluido = excluida,
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
            
            await InserirNaBase(new RegistroFrequenciaAluno()
            {
                Valor = (int)TipoFrequencia.F, 
                CodigoAluno = ALUNO_CODIGO_1, 
                NumeroAula = NUMERO_AULA_3, 
                RegistroFrequenciaId = 1, 
                AulaId = AULA_ID_1, 
                Excluido = excluida,
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
            
            //Segunda aula
            await InserirNaBase(new RegistroFrequencia
            {
                AulaId = AULA_ID_2,
                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF
            });
            
            await InserirNaBase(new RegistroFrequenciaAluno()
            {
                Valor = (int)TipoFrequencia.F, 
                CodigoAluno = ALUNO_CODIGO_1, 
                NumeroAula = NUMERO_AULA_1, 
                RegistroFrequenciaId = 2, 
                AulaId = AULA_ID_2, 
                Excluido = excluida,
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
            
            await InserirNaBase(new RegistroFrequenciaAluno()
            {
                Valor = (int)TipoFrequencia.F, 
                CodigoAluno = ALUNO_CODIGO_1, 
                NumeroAula = NUMERO_AULA_2, 
                RegistroFrequenciaId = 2, 
                AulaId = AULA_ID_2, 
                Excluido = excluida,
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
            
            await InserirNaBase(new RegistroFrequenciaAluno()
            {
                Valor = (int)TipoFrequencia.F, 
                CodigoAluno = ALUNO_CODIGO_1, 
                NumeroAula = NUMERO_AULA_3, 
                RegistroFrequenciaId = 2, 
                AulaId = AULA_ID_2, 
                Excluido = excluida,
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task CriarCompensacaoAusencia(bool excluida = false)
        {
            await InserirNaBase(new CompensacaoAusencia
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                DisciplinaId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                Bimestre = BIMESTRE_1,
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
                Excluido = excluida,
                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new CompensacaoAusenciaAlunoAula()
            {
                DataAula = DATA_02_05,
                NumeroAula = NUMERO_AULA_1,
                CompensacaoAusenciaAlunoId = 1,
                RegistroFrequenciaAlunoId = 1,
                Excluido = excluida,
                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
            
            await InserirNaBase(new CompensacaoAusenciaAlunoAula()
            {
                DataAula = DATA_02_05,
                NumeroAula = NUMERO_AULA_2,
                CompensacaoAusenciaAlunoId = 1,
                RegistroFrequenciaAlunoId = 2,
                Excluido = excluida,
                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
            
            await InserirNaBase(new CompensacaoAusenciaAlunoAula()
            {
                DataAula = DATA_02_05,
                NumeroAula = NUMERO_AULA_3,
                CompensacaoAusenciaAlunoId = 1,
                RegistroFrequenciaAlunoId = 3,
                Excluido = excluida,
                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
            
            //Segunda compensação
            await InserirNaBase(new CompensacaoAusencia
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                DisciplinaId = COMPONENTE_CURRICULAR_ARTES_ID_139.ToString(),
                Bimestre = BIMESTRE_1,
                TurmaId = TURMA_ID_1,
                Nome = "Atividade de compensação em artes",
                Descricao = "Breve descrição da atividade de compensação em artes",
                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new CompensacaoAusenciaAluno
            {
                CodigoAluno = CODIGO_ALUNO_1,
                CompensacaoAusenciaId = 2,
                QuantidadeFaltasCompensadas = NUMERO_AULA_3,
                Excluido = excluida,
                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new CompensacaoAusenciaAlunoAula()
            {
                DataAula = DATA_02_05,
                NumeroAula = NUMERO_AULA_1,
                CompensacaoAusenciaAlunoId = 2,
                RegistroFrequenciaAlunoId = 4,
                Excluido = excluida,
                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
            
            await InserirNaBase(new CompensacaoAusenciaAlunoAula()
            {
                DataAula = DATA_02_05,
                NumeroAula = NUMERO_AULA_2,
                CompensacaoAusenciaAlunoId = 2,
                RegistroFrequenciaAlunoId = 5,
                Excluido = excluida,
                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
            
            await InserirNaBase(new CompensacaoAusenciaAlunoAula()
            {
                DataAula = DATA_02_05,
                NumeroAula = NUMERO_AULA_3,
                CompensacaoAusenciaAlunoId = 2,
                RegistroFrequenciaAlunoId = 6,
                Excluido = excluida,
                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
        }
        
    }
}