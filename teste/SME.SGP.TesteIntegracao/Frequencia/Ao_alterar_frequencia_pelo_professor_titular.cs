using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace SME.SGP.TesteIntegracao.Frequencia
{
    public class Ao_alterar_frequencia_pelo_professor_titular : FrequenciaTesteBase
    {
        public Ao_alterar_frequencia_pelo_professor_titular(CollectionFixture collectionFixture) : base(collectionFixture) { }
        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<VerificaPodePersistirTurmaDisciplinaEOLQuery, bool>), typeof(VerificaPodePersistirTurmaDisciplinaEOLQueryHandlerComPermissaoFake), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName = "Frequência - Deve alterar a Frequencia quando Professor Titular")]
        public async Task Deve_alterar_a_Frequencia()
        {
            await CriarDadosBasicos(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_08_07, BIMESTRE_2, DATA_02_05, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), false);

            var frequencia = new FrequenciaDto()
            {
                AulaId = AULA_ID_1,
                ListaFrequencia = new List<RegistroFrequenciaAlunoDto>()
                    {
                       new RegistroFrequenciaAlunoDto() {
                           Aulas = new List<FrequenciaAulaDto>() { new FrequenciaAulaDto() { NumeroAula = NUMERO_AULAS_1, TipoFrequencia = TIPO_FREQUENCIA_COMPARECEU}},
                           CodigoAluno = CODIGO_ALUNO_99999,
                           TipoFrequenciaPreDefinido = TIPO_FREQUENCIA_COMPARECEU
                       },
                    }
            };

            await InserirFrequenciaUseCaseComValidacaoBasica(frequencia);

            var frequenciaAlterada = new FrequenciaDto()
            {
                AulaId = AULA_ID_1,
                ListaFrequencia = new List<RegistroFrequenciaAlunoDto>()
                    {
                       new RegistroFrequenciaAlunoDto() {
                           Aulas = new List<FrequenciaAulaDto>() { new FrequenciaAulaDto() { NumeroAula = NUMERO_AULAS_1, TipoFrequencia = TIPO_FREQUENCIA_REMOTO}},
                           CodigoAluno = CODIGO_ALUNO_99999,
                           TipoFrequenciaPreDefinido = TIPO_FREQUENCIA_COMPARECEU
                       },
                    }
            };

            await InserirFrequenciaUseCaseComValidacaoBasica(frequenciaAlterada);

        }
        
        [Fact(DisplayName = "Frequência - Deve alterar a Frequencia e remover a compensação quando alterado de Falta para Remoto/Compareceu")]
        public async Task Deve_alterar_a_frequencia_e_remover_compensacao()
        {
            await CriarDadosBasicos(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_08_07, BIMESTRE_2, DATA_02_05, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), false);
            
            await CriarAula(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), DATA_02_05.AddDays(1), RecorrenciaAula.AulaUnica, 2);
        
            //Inserindo frequência de aula 1
            var inserindoFrequenciaAula1 = new FrequenciaDto()
            {
                AulaId = AULA_ID_1,
                ListaFrequencia = new List<RegistroFrequenciaAlunoDto>()
                {
                    new () {
                        Aulas = new List<FrequenciaAulaDto>() { 
                            new () { NumeroAula = NUMERO_AULAS_1, TipoFrequencia = TIPO_FREQUENCIA_COMPARECEU},
                            new () { NumeroAula = NUMERO_AULAS_2, TipoFrequencia = TIPO_FREQUENCIA_FALTOU},
                            new () { NumeroAula = NUMERO_AULAS_3, TipoFrequencia = TIPO_FREQUENCIA_REMOTO}
                        },
                        CodigoAluno = CODIGO_ALUNO_1,
                        TipoFrequenciaPreDefinido = TIPO_FREQUENCIA_COMPARECEU
                    },
                    new () {
                        Aulas = new List<FrequenciaAulaDto>() { 
                            new () { NumeroAula = NUMERO_AULAS_1, TipoFrequencia = TIPO_FREQUENCIA_FALTOU},
                            new () { NumeroAula = NUMERO_AULAS_2, TipoFrequencia = TIPO_FREQUENCIA_COMPARECEU},
                            new () { NumeroAula = NUMERO_AULAS_3, TipoFrequencia = TIPO_FREQUENCIA_FALTOU}
                        },
                        CodigoAluno = CODIGO_ALUNO_2,
                        TipoFrequenciaPreDefinido = TIPO_FREQUENCIA_COMPARECEU
                    },
                    new () {
                        Aulas = new List<FrequenciaAulaDto>() { 
                            new () { NumeroAula = NUMERO_AULAS_1, TipoFrequencia = TIPO_FREQUENCIA_FALTOU},
                            new () { NumeroAula = NUMERO_AULAS_2, TipoFrequencia = TIPO_FREQUENCIA_REMOTO},
                            new () { NumeroAula = NUMERO_AULAS_3, TipoFrequencia = TIPO_FREQUENCIA_FALTOU}
                        },
                        CodigoAluno = CODIGO_ALUNO_3,
                        TipoFrequenciaPreDefinido = TIPO_FREQUENCIA_COMPARECEU
                    },
                    new () {
                        Aulas = new List<FrequenciaAulaDto>() { 
                            new () { NumeroAula = NUMERO_AULAS_1, TipoFrequencia = TIPO_FREQUENCIA_FALTOU},
                            new () { NumeroAula = NUMERO_AULAS_2, TipoFrequencia = TIPO_FREQUENCIA_FALTOU},
                            new () { NumeroAula = NUMERO_AULAS_3, TipoFrequencia = TIPO_FREQUENCIA_FALTOU}
                        },
                        CodigoAluno = CODIGO_ALUNO_4,
                        TipoFrequenciaPreDefinido = TIPO_FREQUENCIA_COMPARECEU
                    },
                }
            };
        
            var retorno = await ExecutarInserirFrequenciaUseCaseSemValidacaoBasica(inserindoFrequenciaAula1);
            retorno.ShouldNotBeNull();

            //Validando as inserções de frequência de aula 1
            var registroFrequencia = ObterTodos<RegistroFrequencia>();
            registroFrequencia.ShouldNotBeEmpty();
            registroFrequencia.Count(a=> a.AulaId == AULA_ID_1).Equals(1).ShouldBeTrue();

            var registroFrequenciaAluno = ObterTodos<RegistroFrequenciaAluno>();
            registroFrequenciaAluno.ShouldNotBeEmpty();
            registroFrequenciaAluno.Any(a=> a.RegistroFrequenciaId == REGISTRO_FREQUENCIA_1 && a.AulaId == AULA_ID_1 && a.CodigoAluno.Equals(ALUNO_CODIGO_1) && a.NumeroAula == NUMERO_AULAS_1 && a.Valor == TIPO_FREQUENCIA_COMPARECEU_NUMERO).ShouldBeTrue();
            registroFrequenciaAluno.Any(a=> a.RegistroFrequenciaId == REGISTRO_FREQUENCIA_1 && a.AulaId == AULA_ID_1 && a.CodigoAluno.Equals(ALUNO_CODIGO_1) && a.NumeroAula == NUMERO_AULAS_2 && a.Valor == TIPO_FREQUENCIA_FALTOU_NUMERO).ShouldBeTrue();
            registroFrequenciaAluno.Any(a=> a.RegistroFrequenciaId == REGISTRO_FREQUENCIA_1 && a.AulaId == AULA_ID_1 && a.CodigoAluno.Equals(ALUNO_CODIGO_1) && a.NumeroAula == NUMERO_AULAS_3 && a.Valor == TIPO_FREQUENCIA_REMOTO_NUMERO).ShouldBeTrue();
            
            registroFrequenciaAluno.Any(a=> a.RegistroFrequenciaId == REGISTRO_FREQUENCIA_1 && a.AulaId == AULA_ID_1 && a.CodigoAluno.Equals(ALUNO_CODIGO_2) && a.NumeroAula == NUMERO_AULAS_1 && a.Valor == TIPO_FREQUENCIA_FALTOU_NUMERO).ShouldBeTrue();
            registroFrequenciaAluno.Any(a=> a.RegistroFrequenciaId == REGISTRO_FREQUENCIA_1 && a.AulaId == AULA_ID_1 && a.CodigoAluno.Equals(ALUNO_CODIGO_2) && a.NumeroAula == NUMERO_AULAS_2 && a.Valor == TIPO_FREQUENCIA_COMPARECEU_NUMERO).ShouldBeTrue();
            registroFrequenciaAluno.Any(a=> a.RegistroFrequenciaId == REGISTRO_FREQUENCIA_1 && a.AulaId == AULA_ID_1 && a.CodigoAluno.Equals(ALUNO_CODIGO_2) && a.NumeroAula == NUMERO_AULAS_3 && a.Valor == TIPO_FREQUENCIA_FALTOU_NUMERO).ShouldBeTrue();
            
            registroFrequenciaAluno.Any(a=> a.RegistroFrequenciaId == REGISTRO_FREQUENCIA_1 && a.AulaId == AULA_ID_1 && a.CodigoAluno.Equals(ALUNO_CODIGO_3) && a.NumeroAula == NUMERO_AULAS_1 && a.Valor == TIPO_FREQUENCIA_FALTOU_NUMERO).ShouldBeTrue();
            registroFrequenciaAluno.Any(a=> a.RegistroFrequenciaId == REGISTRO_FREQUENCIA_1 && a.AulaId == AULA_ID_1 && a.CodigoAluno.Equals(ALUNO_CODIGO_3) && a.NumeroAula == NUMERO_AULAS_2 && a.Valor == TIPO_FREQUENCIA_REMOTO_NUMERO).ShouldBeTrue();
            registroFrequenciaAluno.Any(a=> a.RegistroFrequenciaId == REGISTRO_FREQUENCIA_1 && a.AulaId == AULA_ID_1 && a.CodigoAluno.Equals(ALUNO_CODIGO_3) && a.NumeroAula == NUMERO_AULAS_3 && a.Valor == TIPO_FREQUENCIA_FALTOU_NUMERO).ShouldBeTrue();
            
            registroFrequenciaAluno.Any(a=> a.RegistroFrequenciaId == REGISTRO_FREQUENCIA_1 && a.AulaId == AULA_ID_1 && a.CodigoAluno.Equals(ALUNO_CODIGO_4) && a.NumeroAula == NUMERO_AULAS_1 && a.Valor == TIPO_FREQUENCIA_FALTOU_NUMERO).ShouldBeTrue();
            registroFrequenciaAluno.Any(a=> a.RegistroFrequenciaId == REGISTRO_FREQUENCIA_1 && a.AulaId == AULA_ID_1 && a.CodigoAluno.Equals(ALUNO_CODIGO_4) && a.NumeroAula == NUMERO_AULAS_2 && a.Valor == TIPO_FREQUENCIA_FALTOU_NUMERO).ShouldBeTrue();
            registroFrequenciaAluno.Any(a=> a.RegistroFrequenciaId == REGISTRO_FREQUENCIA_1 && a.AulaId == AULA_ID_1 && a.CodigoAluno.Equals(ALUNO_CODIGO_4) && a.NumeroAula == NUMERO_AULAS_3 && a.Valor == TIPO_FREQUENCIA_FALTOU_NUMERO).ShouldBeTrue();
            
            //Inserindo frequência de aula 2
            var inserindoFrequenciaAula2 = new FrequenciaDto()
            {
                AulaId = AULA_ID_2,
                ListaFrequencia = new List<RegistroFrequenciaAlunoDto>()
                {
                    new () {
                        Aulas = new List<FrequenciaAulaDto>() { 
                            new () { NumeroAula = NUMERO_AULAS_1, TipoFrequencia = TIPO_FREQUENCIA_FALTOU},
                            new () { NumeroAula = NUMERO_AULAS_2, TipoFrequencia = TIPO_FREQUENCIA_FALTOU},
                        },
                        CodigoAluno = CODIGO_ALUNO_1,
                        TipoFrequenciaPreDefinido = TIPO_FREQUENCIA_COMPARECEU
                    },
                    new () {
                        Aulas = new List<FrequenciaAulaDto>() { 
                            new () { NumeroAula = NUMERO_AULAS_1, TipoFrequencia = TIPO_FREQUENCIA_FALTOU},
                            new () { NumeroAula = NUMERO_AULAS_2, TipoFrequencia = TIPO_FREQUENCIA_REMOTO},
                        },
                        CodigoAluno = CODIGO_ALUNO_2,
                        TipoFrequenciaPreDefinido = TIPO_FREQUENCIA_COMPARECEU
                    },
                    new () {
                        Aulas = new List<FrequenciaAulaDto>() { 
                            new () { NumeroAula = NUMERO_AULAS_1, TipoFrequencia = TIPO_FREQUENCIA_REMOTO},
                            new () { NumeroAula = NUMERO_AULAS_2, TipoFrequencia = TIPO_FREQUENCIA_COMPARECEU},
                        },
                        CodigoAluno = CODIGO_ALUNO_3,
                        TipoFrequenciaPreDefinido = TIPO_FREQUENCIA_COMPARECEU
                    },     
                    new () {
                        Aulas = new List<FrequenciaAulaDto>() { 
                            new () { NumeroAula = NUMERO_AULAS_1, TipoFrequencia = TIPO_FREQUENCIA_COMPARECEU},
                            new () { NumeroAula = NUMERO_AULAS_2, TipoFrequencia = TIPO_FREQUENCIA_FALTOU},
                        },
                        CodigoAluno = CODIGO_ALUNO_4,
                        TipoFrequenciaPreDefinido = TIPO_FREQUENCIA_COMPARECEU
                    },     
                }
            };
           
            await InserirFrequenciaUseCaseComValidacaoBasica(inserindoFrequenciaAula2);
            
            //Validando as inserções de frequência de aula 2
            registroFrequenciaAluno = ObterTodos<RegistroFrequenciaAluno>();
            registroFrequenciaAluno.ShouldNotBeEmpty();
            registroFrequenciaAluno.Any(a=> a.RegistroFrequenciaId == REGISTRO_FREQUENCIA_2 && a.AulaId == AULA_ID_2 && a.CodigoAluno.Equals(ALUNO_CODIGO_1) && a.NumeroAula == NUMERO_AULAS_1 && a.Valor == TIPO_FREQUENCIA_FALTOU_NUMERO).ShouldBeTrue();
            registroFrequenciaAluno.Any(a=> a.RegistroFrequenciaId == REGISTRO_FREQUENCIA_2 && a.AulaId == AULA_ID_2 && a.CodigoAluno.Equals(ALUNO_CODIGO_1) && a.NumeroAula == NUMERO_AULAS_2 && a.Valor == TIPO_FREQUENCIA_FALTOU_NUMERO).ShouldBeTrue();
            
            registroFrequenciaAluno.Any(a=> a.RegistroFrequenciaId == REGISTRO_FREQUENCIA_2 && a.AulaId == AULA_ID_2 && a.CodigoAluno.Equals(ALUNO_CODIGO_2) && a.NumeroAula == NUMERO_AULAS_1 && a.Valor == TIPO_FREQUENCIA_FALTOU_NUMERO).ShouldBeTrue();
            registroFrequenciaAluno.Any(a=> a.RegistroFrequenciaId == REGISTRO_FREQUENCIA_2 && a.AulaId == AULA_ID_2 && a.CodigoAluno.Equals(ALUNO_CODIGO_2) && a.NumeroAula == NUMERO_AULAS_2 && a.Valor == TIPO_FREQUENCIA_REMOTO_NUMERO).ShouldBeTrue();
            
            registroFrequenciaAluno.Any(a=> a.RegistroFrequenciaId == REGISTRO_FREQUENCIA_2 && a.AulaId == AULA_ID_2 && a.CodigoAluno.Equals(ALUNO_CODIGO_3) && a.NumeroAula == NUMERO_AULAS_1 && a.Valor == TIPO_FREQUENCIA_REMOTO_NUMERO).ShouldBeTrue();
            registroFrequenciaAluno.Any(a=> a.RegistroFrequenciaId == REGISTRO_FREQUENCIA_2 && a.AulaId == AULA_ID_2 && a.CodigoAluno.Equals(ALUNO_CODIGO_3) && a.NumeroAula == NUMERO_AULAS_2 && a.Valor == TIPO_FREQUENCIA_COMPARECEU_NUMERO).ShouldBeTrue();
            
            registroFrequenciaAluno.Any(a=> a.RegistroFrequenciaId == REGISTRO_FREQUENCIA_2 && a.AulaId == AULA_ID_2 && a.CodigoAluno.Equals(ALUNO_CODIGO_4) && a.NumeroAula == NUMERO_AULAS_1 && a.Valor == TIPO_FREQUENCIA_COMPARECEU_NUMERO).ShouldBeTrue();
            registroFrequenciaAluno.Any(a=> a.RegistroFrequenciaId == REGISTRO_FREQUENCIA_2 && a.AulaId == AULA_ID_2 && a.CodigoAluno.Equals(ALUNO_CODIGO_4) && a.NumeroAula == NUMERO_AULAS_2 && a.Valor == TIPO_FREQUENCIA_FALTOU_NUMERO).ShouldBeTrue();
            
            //Realizando compensações
            await CriarCompensacaoAusencia();

            //Aluno_1
            await CriarCompensacaoAusenciaAluno(CODIGO_ALUNO_1,COMPENSACAO_AUSENCIA_ID_1,NUMERO_AULA_3);
            await CriarCompensacaoAusenciaAlunoAula(DATA_02_05,NUMERO_AULA_2,1,2);
            await CriarCompensacaoAusenciaAlunoAula(DATA_02_05,NUMERO_AULA_1,1,13);
            await CriarCompensacaoAusenciaAlunoAula(DATA_02_05,NUMERO_AULA_1,1,14);
            
            //Aluno_2
            await CriarCompensacaoAusenciaAluno(CODIGO_ALUNO_2,COMPENSACAO_AUSENCIA_ID_1,NUMERO_AULA_3);
            await CriarCompensacaoAusenciaAlunoAula(DATA_02_05,NUMERO_AULA_1,2,4);
            await CriarCompensacaoAusenciaAlunoAula(DATA_02_05,NUMERO_AULA_3,2,6);
            
            //Aluno_3
            await CriarCompensacaoAusenciaAluno(CODIGO_ALUNO_3,COMPENSACAO_AUSENCIA_ID_1,NUMERO_AULA_2);
            await CriarCompensacaoAusenciaAlunoAula(DATA_02_05,NUMERO_AULA_1,3,7);
            await CriarCompensacaoAusenciaAlunoAula(DATA_02_05,NUMERO_AULA_3,3,9);
            
            //Aluno_4
            await CriarCompensacaoAusenciaAluno(CODIGO_ALUNO_4,COMPENSACAO_AUSENCIA_ID_1,NUMERO_AULA_4);
            await CriarCompensacaoAusenciaAlunoAula(DATA_02_05,NUMERO_AULA_1,4,10);
            await CriarCompensacaoAusenciaAlunoAula(DATA_02_05,NUMERO_AULA_2,4,11);
            await CriarCompensacaoAusenciaAlunoAula(DATA_02_05,NUMERO_AULA_3,4,12);
            await CriarCompensacaoAusenciaAlunoAula(DATA_02_05,NUMERO_AULA_2,4,20);

            //Validando compensações
            var compensacaoAusencias = ObterTodos<CompensacaoAusencia>();
            compensacaoAusencias.Count.Equals(1).ShouldBeTrue();
            
            var compensacaoAusenciasAlunos = ObterTodos<CompensacaoAusenciaAluno>();
            compensacaoAusenciasAlunos.Count(a=> !a.Excluido).Equals(4).ShouldBeTrue();
            
            var compensacaoAusenciasAlunosAulas = ObterTodos<CompensacaoAusenciaAlunoAula>();
            compensacaoAusenciasAlunosAulas.Count(a=> !a.Excluido).Equals(11).ShouldBeTrue();
            
            //Realizando a alteração das frequências para validar a remoção das compensações que foram criadas
            
            //Alterando a frequência de aula 1
            var alterandoFrequenciaAula1 = new FrequenciaDto()
            {
                AulaId = AULA_ID_1,
                ListaFrequencia = new List<RegistroFrequenciaAlunoDto>()
                {
                    new () {
                        Aulas = new List<FrequenciaAulaDto>() { 
                            new () { NumeroAula = NUMERO_AULAS_1, TipoFrequencia = TIPO_FREQUENCIA_COMPARECEU},
                            new () { NumeroAula = NUMERO_AULAS_2, TipoFrequencia = TIPO_FREQUENCIA_COMPARECEU},
                            new () { NumeroAula = NUMERO_AULAS_3, TipoFrequencia = TIPO_FREQUENCIA_REMOTO}
                        },
                        CodigoAluno = CODIGO_ALUNO_1,
                        TipoFrequenciaPreDefinido = TIPO_FREQUENCIA_COMPARECEU
                    },
                    new () {
                        Aulas = new List<FrequenciaAulaDto>() { 
                            new () { NumeroAula = NUMERO_AULAS_1, TipoFrequencia = TIPO_FREQUENCIA_FALTOU},
                            new () { NumeroAula = NUMERO_AULAS_2, TipoFrequencia = TIPO_FREQUENCIA_COMPARECEU},
                            new () { NumeroAula = NUMERO_AULAS_3, TipoFrequencia = TIPO_FREQUENCIA_REMOTO}
                        },
                        CodigoAluno = CODIGO_ALUNO_2,
                        TipoFrequenciaPreDefinido = TIPO_FREQUENCIA_COMPARECEU
                    },
                    new () {
                        Aulas = new List<FrequenciaAulaDto>() { 
                            new () { NumeroAula = NUMERO_AULAS_1, TipoFrequencia = TIPO_FREQUENCIA_COMPARECEU},
                            new () { NumeroAula = NUMERO_AULAS_2, TipoFrequencia = TIPO_FREQUENCIA_FALTOU},
                            new () { NumeroAula = NUMERO_AULAS_3, TipoFrequencia = TIPO_FREQUENCIA_COMPARECEU}
                        },
                        CodigoAluno = CODIGO_ALUNO_3,
                        TipoFrequenciaPreDefinido = TIPO_FREQUENCIA_COMPARECEU
                    },
                    new () {
                        Aulas = new List<FrequenciaAulaDto>() { 
                            new () { NumeroAula = NUMERO_AULAS_1, TipoFrequencia = TIPO_FREQUENCIA_REMOTO},
                            new () { NumeroAula = NUMERO_AULAS_2, TipoFrequencia = TIPO_FREQUENCIA_REMOTO},
                            new () { NumeroAula = NUMERO_AULAS_3, TipoFrequencia = TIPO_FREQUENCIA_REMOTO}
                        },
                        CodigoAluno = CODIGO_ALUNO_4,
                        TipoFrequenciaPreDefinido = TIPO_FREQUENCIA_COMPARECEU
                    },
                }
            };
        
            retorno = await ExecutarInserirFrequenciaUseCaseSemValidacaoBasica(alterandoFrequenciaAula1);
            retorno.ShouldNotBeNull();

            //Validando as alterações de frequência de aula 1
            registroFrequencia = ObterTodos<RegistroFrequencia>();
            registroFrequencia.ShouldNotBeEmpty();
            registroFrequencia.Count(a=> a.AulaId == AULA_ID_1).Equals(1).ShouldBeTrue();

            registroFrequenciaAluno = ObterTodos<RegistroFrequenciaAluno>();
            registroFrequenciaAluno.ShouldNotBeEmpty();
            registroFrequenciaAluno.Any(a=> a.RegistroFrequenciaId == REGISTRO_FREQUENCIA_1 && a.AulaId == AULA_ID_1 && a.CodigoAluno.Equals(ALUNO_CODIGO_1) && a.NumeroAula == NUMERO_AULAS_1 && a.Valor == TIPO_FREQUENCIA_COMPARECEU_NUMERO).ShouldBeTrue();
            registroFrequenciaAluno.Any(a=> a.RegistroFrequenciaId == REGISTRO_FREQUENCIA_1 && a.AulaId == AULA_ID_1 && a.CodigoAluno.Equals(ALUNO_CODIGO_1) && a.NumeroAula == NUMERO_AULAS_2 && a.Valor == TIPO_FREQUENCIA_COMPARECEU_NUMERO).ShouldBeTrue();
            registroFrequenciaAluno.Any(a=> a.RegistroFrequenciaId == REGISTRO_FREQUENCIA_1 && a.AulaId == AULA_ID_1 && a.CodigoAluno.Equals(ALUNO_CODIGO_1) && a.NumeroAula == NUMERO_AULAS_3 && a.Valor == TIPO_FREQUENCIA_REMOTO_NUMERO).ShouldBeTrue();
            
            registroFrequenciaAluno.Any(a=> a.RegistroFrequenciaId == REGISTRO_FREQUENCIA_1 && a.AulaId == AULA_ID_1 && a.CodigoAluno.Equals(ALUNO_CODIGO_2) && a.NumeroAula == NUMERO_AULAS_1 && a.Valor == TIPO_FREQUENCIA_FALTOU_NUMERO).ShouldBeTrue();
            registroFrequenciaAluno.Any(a=> a.RegistroFrequenciaId == REGISTRO_FREQUENCIA_1 && a.AulaId == AULA_ID_1 && a.CodigoAluno.Equals(ALUNO_CODIGO_2) && a.NumeroAula == NUMERO_AULAS_2 && a.Valor == TIPO_FREQUENCIA_COMPARECEU_NUMERO).ShouldBeTrue();
            registroFrequenciaAluno.Any(a=> a.RegistroFrequenciaId == REGISTRO_FREQUENCIA_1 && a.AulaId == AULA_ID_1 && a.CodigoAluno.Equals(ALUNO_CODIGO_2) && a.NumeroAula == NUMERO_AULAS_3 && a.Valor == TIPO_FREQUENCIA_REMOTO_NUMERO).ShouldBeTrue();
            
            registroFrequenciaAluno.Any(a=> a.RegistroFrequenciaId == REGISTRO_FREQUENCIA_1 && a.AulaId == AULA_ID_1 && a.CodigoAluno.Equals(ALUNO_CODIGO_3) && a.NumeroAula == NUMERO_AULAS_1 && a.Valor == TIPO_FREQUENCIA_COMPARECEU_NUMERO).ShouldBeTrue();
            registroFrequenciaAluno.Any(a=> a.RegistroFrequenciaId == REGISTRO_FREQUENCIA_1 && a.AulaId == AULA_ID_1 && a.CodigoAluno.Equals(ALUNO_CODIGO_3) && a.NumeroAula == NUMERO_AULAS_2 && a.Valor == TIPO_FREQUENCIA_FALTOU_NUMERO).ShouldBeTrue();
            registroFrequenciaAluno.Any(a=> a.RegistroFrequenciaId == REGISTRO_FREQUENCIA_1 && a.AulaId == AULA_ID_1 && a.CodigoAluno.Equals(ALUNO_CODIGO_3) && a.NumeroAula == NUMERO_AULAS_3 && a.Valor == TIPO_FREQUENCIA_COMPARECEU_NUMERO).ShouldBeTrue();
            
            registroFrequenciaAluno.Any(a=> a.RegistroFrequenciaId == REGISTRO_FREQUENCIA_1 && a.AulaId == AULA_ID_1 && a.CodigoAluno.Equals(ALUNO_CODIGO_4) && a.NumeroAula == NUMERO_AULAS_1 && a.Valor == TIPO_FREQUENCIA_REMOTO_NUMERO).ShouldBeTrue();
            registroFrequenciaAluno.Any(a=> a.RegistroFrequenciaId == REGISTRO_FREQUENCIA_1 && a.AulaId == AULA_ID_1 && a.CodigoAluno.Equals(ALUNO_CODIGO_4) && a.NumeroAula == NUMERO_AULAS_2 && a.Valor == TIPO_FREQUENCIA_REMOTO_NUMERO).ShouldBeTrue();
            registroFrequenciaAluno.Any(a=> a.RegistroFrequenciaId == REGISTRO_FREQUENCIA_1 && a.AulaId == AULA_ID_1 && a.CodigoAluno.Equals(ALUNO_CODIGO_4) && a.NumeroAula == NUMERO_AULAS_3 && a.Valor == TIPO_FREQUENCIA_REMOTO_NUMERO).ShouldBeTrue();
            
            //Alterando frequência de aula 2
            var alterandoFrequenciaAula2 = new FrequenciaDto()
            {
                AulaId = AULA_ID_2,
                ListaFrequencia = new List<RegistroFrequenciaAlunoDto>()
                {
                    new () {
                        Aulas = new List<FrequenciaAulaDto>() { 
                            new () { NumeroAula = NUMERO_AULAS_1, TipoFrequencia = TIPO_FREQUENCIA_REMOTO},
                            new () { NumeroAula = NUMERO_AULAS_2, TipoFrequencia = TIPO_FREQUENCIA_FALTOU},
                        },
                        CodigoAluno = CODIGO_ALUNO_1,
                        TipoFrequenciaPreDefinido = TIPO_FREQUENCIA_COMPARECEU
                    },
                    new () {
                        Aulas = new List<FrequenciaAulaDto>() { 
                            new () { NumeroAula = NUMERO_AULAS_1, TipoFrequencia = TIPO_FREQUENCIA_FALTOU},
                            new () { NumeroAula = NUMERO_AULAS_2, TipoFrequencia = TIPO_FREQUENCIA_REMOTO},
                        },
                        CodigoAluno = CODIGO_ALUNO_2,
                        TipoFrequenciaPreDefinido = TIPO_FREQUENCIA_COMPARECEU
                    },
                    new () {
                        Aulas = new List<FrequenciaAulaDto>() { 
                            new () { NumeroAula = NUMERO_AULAS_1, TipoFrequencia = TIPO_FREQUENCIA_REMOTO},
                            new () { NumeroAula = NUMERO_AULAS_2, TipoFrequencia = TIPO_FREQUENCIA_COMPARECEU},
                        },
                        CodigoAluno = CODIGO_ALUNO_3,
                        TipoFrequenciaPreDefinido = TIPO_FREQUENCIA_COMPARECEU
                    },     
                    new () {
                        Aulas = new List<FrequenciaAulaDto>() { 
                            new () { NumeroAula = NUMERO_AULAS_1, TipoFrequencia = TIPO_FREQUENCIA_REMOTO},
                            new () { NumeroAula = NUMERO_AULAS_2, TipoFrequencia = TIPO_FREQUENCIA_FALTOU},
                        },
                        CodigoAluno = CODIGO_ALUNO_4,
                        TipoFrequenciaPreDefinido = TIPO_FREQUENCIA_COMPARECEU
                    },     
                }
            };
           
            await InserirFrequenciaUseCaseComValidacaoBasica(alterandoFrequenciaAula2);
            
            //Validando as alterações de frequência de aula 2
            registroFrequenciaAluno = ObterTodos<RegistroFrequenciaAluno>();
            registroFrequenciaAluno.ShouldNotBeEmpty();
            registroFrequenciaAluno.Any(a=> a.RegistroFrequenciaId == REGISTRO_FREQUENCIA_2 && a.AulaId == AULA_ID_2 && a.CodigoAluno.Equals(ALUNO_CODIGO_1) && a.NumeroAula == NUMERO_AULAS_1 && a.Valor == TIPO_FREQUENCIA_REMOTO_NUMERO).ShouldBeTrue();
            registroFrequenciaAluno.Any(a=> a.RegistroFrequenciaId == REGISTRO_FREQUENCIA_2 && a.AulaId == AULA_ID_2 && a.CodigoAluno.Equals(ALUNO_CODIGO_1) && a.NumeroAula == NUMERO_AULAS_2 && a.Valor == TIPO_FREQUENCIA_FALTOU_NUMERO).ShouldBeTrue();
            
            registroFrequenciaAluno.Any(a=> a.RegistroFrequenciaId == REGISTRO_FREQUENCIA_2 && a.AulaId == AULA_ID_2 && a.CodigoAluno.Equals(ALUNO_CODIGO_2) && a.NumeroAula == NUMERO_AULAS_1 && a.Valor == TIPO_FREQUENCIA_FALTOU_NUMERO).ShouldBeTrue();
            registroFrequenciaAluno.Any(a=> a.RegistroFrequenciaId == REGISTRO_FREQUENCIA_2 && a.AulaId == AULA_ID_2 && a.CodigoAluno.Equals(ALUNO_CODIGO_2) && a.NumeroAula == NUMERO_AULAS_2 && a.Valor == TIPO_FREQUENCIA_REMOTO_NUMERO).ShouldBeTrue();
            
            registroFrequenciaAluno.Any(a=> a.RegistroFrequenciaId == REGISTRO_FREQUENCIA_2 && a.AulaId == AULA_ID_2 && a.CodigoAluno.Equals(ALUNO_CODIGO_3) && a.NumeroAula == NUMERO_AULAS_1 && a.Valor == TIPO_FREQUENCIA_REMOTO_NUMERO).ShouldBeTrue();
            registroFrequenciaAluno.Any(a=> a.RegistroFrequenciaId == REGISTRO_FREQUENCIA_2 && a.AulaId == AULA_ID_2 && a.CodigoAluno.Equals(ALUNO_CODIGO_3) && a.NumeroAula == NUMERO_AULAS_2 && a.Valor == TIPO_FREQUENCIA_COMPARECEU_NUMERO).ShouldBeTrue();
            
            registroFrequenciaAluno.Any(a=> a.RegistroFrequenciaId == REGISTRO_FREQUENCIA_2 && a.AulaId == AULA_ID_2 && a.CodigoAluno.Equals(ALUNO_CODIGO_4) && a.NumeroAula == NUMERO_AULAS_1 && a.Valor == TIPO_FREQUENCIA_REMOTO_NUMERO).ShouldBeTrue();
            registroFrequenciaAluno.Any(a=> a.RegistroFrequenciaId == REGISTRO_FREQUENCIA_2 && a.AulaId == AULA_ID_2 && a.CodigoAluno.Equals(ALUNO_CODIGO_4) && a.NumeroAula == NUMERO_AULAS_2 && a.Valor == TIPO_FREQUENCIA_FALTOU_NUMERO).ShouldBeTrue();
            
            //Validando compensações após as alterações - deve excluir algumas compensações
            compensacaoAusencias = ObterTodos<CompensacaoAusencia>();
            compensacaoAusencias.Count.Equals(1).ShouldBeTrue();
            
            compensacaoAusenciasAlunos = ObterTodos<CompensacaoAusenciaAluno>();
            compensacaoAusenciasAlunos.Count.Equals(4).ShouldBeTrue();
            compensacaoAusenciasAlunos.Any(a=> a.CodigoAluno.Equals(ALUNO_CODIGO_1) && a.QuantidadeFaltasCompensadas.Equals(NUMERO_AULA_1)).ShouldBeTrue();
            compensacaoAusenciasAlunos.Any(a=> a.CodigoAluno.Equals(ALUNO_CODIGO_2) && a.QuantidadeFaltasCompensadas.Equals(NUMERO_AULA_2)).ShouldBeTrue();
            compensacaoAusenciasAlunos.Any(a=> a.CodigoAluno.Equals(ALUNO_CODIGO_3) && a.Excluido).ShouldBeTrue();
            compensacaoAusenciasAlunos.Any(a=> a.CodigoAluno.Equals(ALUNO_CODIGO_4) && a.QuantidadeFaltasCompensadas.Equals(NUMERO_AULA_1)).ShouldBeTrue();
            
            compensacaoAusenciasAlunosAulas = ObterTodos<CompensacaoAusenciaAlunoAula>();
            compensacaoAusenciasAlunosAulas.Count(a=> !a.Excluido).Equals(3).ShouldBeTrue();
            compensacaoAusenciasAlunosAulas.Count(a=> a.Excluido).Equals(8).ShouldBeTrue();
        }

        private async Task CriarCompensacaoAusenciaAlunoAula(DateTime data, int numeroAula, int compensacaoAusenciaAlunoId, int registroFrequenciaAlunoId)
        {
            await InserirNaBase(new CompensacaoAusenciaAlunoAula()
            {
                DataAula = data,
                NumeroAula = numeroAula,
                CompensacaoAusenciaAlunoId = compensacaoAusenciaAlunoId,
                RegistroFrequenciaAlunoId = registroFrequenciaAlunoId,
                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task CriarCompensacaoAusenciaAluno(string alunoCodigo, long compensacaoAusenciaId, int quantidadeFaltasCompensadas)
        {
            await InserirNaBase(new CompensacaoAusenciaAluno
            {
                CodigoAluno = alunoCodigo,
                CompensacaoAusenciaId = compensacaoAusenciaId,
                QuantidadeFaltasCompensadas = quantidadeFaltasCompensadas,
                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task CriarCompensacaoAusencia()
        {
            await InserirNaBase(new CompensacaoAusencia
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                DisciplinaId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                Bimestre = BIMESTRE_2, TurmaId = TURMA_ID_1,
                Nome = "Atividade de compensação", Descricao = "Breve descrição da atividade de compensação",
                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
        }
    }
}
