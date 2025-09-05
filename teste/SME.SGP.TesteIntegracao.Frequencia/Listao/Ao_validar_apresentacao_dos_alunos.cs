using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.Listao
{
    public class Ao_validar_apresentacao_dos_alunos : ListaoTesteBase
    {
        public Ao_validar_apresentacao_dos_alunos(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<VerificaPodePersistirTurmaDisciplinaEOLQuery, bool>),
                typeof(VerificaPodePersistirTurmaDisciplinaEOLQueryHandlerComPermissaoFake), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName = "Apresentação de alunos novos com tooltip por 15 dias")]
        public async Task Validar_aprensentacao_de_alunos_novos_devem_aparecer_com_o_tooltip_durante_15_dias()
        {
            var dataInicio = DateTimeExtension.HorarioBrasilia().AddDays(-10);
            var dataFim = dataInicio.AddMonths(2);

            await CriarDreUePerfil();
            await CriarComponenteCurricular();
            await CriarUsuarios();
            CriarClaimUsuario(ObterPerfilProfessor());
            await CriarTurma(Modalidade.Fundamental, ANO_8, false, TipoTurma.Regular);
            await CriarTipoCalendario(ModalidadeTipoCalendario.FundamentalMedio);
            await CriarPeriodoEscolar(dataInicio, dataFim, BIMESTRE_1);
            await CriarAula(dataInicio, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_LOGIN_2222222,
                    TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), TIPO_CALENDARIO_1);
            await InserirParametroSistema();
            await CriarMotivoAusencia();
            await CriarFrequenciaPreDefinida(COMPONENTE_CURRICULAR_PORTUGUES_ID_138);

            var frequenciasSalvar = new List<FrequenciaSalvarAulaAlunosDto>
            {
                new FrequenciaSalvarAulaAlunosDto
                {
                    AulaId = AULA_ID,
                    Alunos = new List<FrequenciaSalvarAlunoDto>
                    {
                        new FrequenciaSalvarAlunoDto
                        {
                            CodigoAluno = CODIGO_ALUNO_1,
                            Frequencias = ObterFrequenciaAula(CODIGO_ALUNO_1)
                        }
                    }
                }
};

            var useCaseSalvar = ServiceProvider.GetService<IInserirFrequenciaListaoUseCase>();
            useCaseSalvar.ShouldNotBeNull();
            await useCaseSalvar.Executar(frequenciasSalvar);

            var useCaseObterFrequencia = ServiceProvider.GetService<IObterFrequenciasPorPeriodoUseCase>();
            useCaseObterFrequencia.ShouldNotBeNull();

            var filtroFrequenciaPeriodo = new FiltroFrequenciaPorPeriodoDto
            {
                TurmaId = TURMA_CODIGO_1,
                DisciplinaId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                ComponenteCurricularId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                DataInicio = dataInicio,
                DataFim = dataFim
            };
            var frequencias = await useCaseObterFrequencia.Executar(filtroFrequenciaPeriodo);
            frequencias.ShouldNotBeNull();
            var novoAluno = frequencias.Alunos.First(aluno => aluno.CodigoAluno == CODIGO_ALUNO_1);
            novoAluno.ShouldNotBeNull();
            novoAluno.Marcador.Tipo.ShouldBe(TipoMarcadorFrequencia.Novo);
            var dataSituacao = $"{novoAluno.DataSituacao.Day}/{novoAluno.DataSituacao.Month}/{novoAluno.DataSituacao.Year}";
            var descricaoMarcador = $"Estudante Novo: Data da matrícula {dataSituacao}";
            novoAluno.Marcador.Descricao.ShouldBe(descricaoMarcador);
        }

        [Fact(DisplayName = "Apresentação de alunos novos sem tooltip após 15 dias")]
        public async Task Validar_aprensentacao_de_alunos_novos_nao_deve_aparecer_com_o_tooltip_apos_15_dias()
        {
            var dataInicio = DateTimeExtension.HorarioBrasilia().AddDays(11);
            var dataFim = dataInicio.AddMonths(2);

            await CriarDreUePerfil();
            await CriarComponenteCurricular();
            await CriarUsuarios();
            CriarClaimUsuario(ObterPerfilProfessor());
            await CriarTurma(Modalidade.Fundamental, ANO_8, false, TipoTurma.Regular);
            await CriarTipoCalendario(ModalidadeTipoCalendario.FundamentalMedio);
            await CriarPeriodoEscolar(dataInicio, dataFim, BIMESTRE_1);
            await CriarAula(dataInicio, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_LOGIN_2222222,
                    TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), TIPO_CALENDARIO_1);
            await InserirParametroSistema();
            await CriarMotivoAusencia();
            await CriarFrequenciaPreDefinida(COMPONENTE_CURRICULAR_PORTUGUES_ID_138);

            await CriarPeriodoReabertura(TIPO_CALENDARIO_1);

            var frequenciasSalvar = new List<FrequenciaSalvarAulaAlunosDto>
            {
                new FrequenciaSalvarAulaAlunosDto
                {
                    AulaId = AULA_ID,
                    Alunos = new List<FrequenciaSalvarAlunoDto>
                    {
                        new FrequenciaSalvarAlunoDto
                        {
                            CodigoAluno = CODIGO_ALUNO_1,
                            Frequencias = ObterFrequenciaAula(CODIGO_ALUNO_1)
                        }
                    }
                }
            };

            var useCaseSalvar = ServiceProvider.GetService<IInserirFrequenciaListaoUseCase>();
            useCaseSalvar.ShouldNotBeNull();
            await useCaseSalvar.Executar(frequenciasSalvar);

            var useCaseObterFrequencia = ServiceProvider.GetService<IObterFrequenciasPorPeriodoUseCase>();
            useCaseObterFrequencia.ShouldNotBeNull();

            var filtroFrequenciaPeriodo = new FiltroFrequenciaPorPeriodoDto
            {
                TurmaId = TURMA_CODIGO_1,
                DisciplinaId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                ComponenteCurricularId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                DataInicio = dataInicio,
                DataFim = dataFim
            };
            var frequencias = await useCaseObterFrequencia.Executar(filtroFrequenciaPeriodo);
            frequencias.ShouldNotBeNull();
            var novoAluno = frequencias.Alunos.First(aluno => aluno.CodigoAluno == CODIGO_ALUNO_1);
            novoAluno.ShouldNotBeNull();
            novoAluno.Marcador.ShouldBeNull();
        }

        [Fact(DisplayName = "Apresentação de alunos inativos com tooltip Inativo")]
        public async Task Validar_aprensentacao_de_alunos_inativo_devem_aparecer_com_o_tooltip_inativo()
        {
            var dataInicio = DateTimeExtension.HorarioBrasilia().AddDays(-10);
            var dataFim = dataInicio.AddMonths(2);

            await CriarDreUePerfil();
            await CriarComponenteCurricular();
            await CriarUsuarios();
            CriarClaimUsuario(ObterPerfilProfessor());
            await CriarTurma(Modalidade.Fundamental, ANO_8, false, TipoTurma.Regular);
            await CriarTipoCalendario(ModalidadeTipoCalendario.FundamentalMedio);
            await CriarPeriodoEscolar(dataInicio, dataFim, BIMESTRE_1);
            await CriarAula(dataInicio.AddDays(30), RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_LOGIN_2222222,
                    TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), TIPO_CALENDARIO_1);
            await InserirParametroSistema();
            await CriarMotivoAusencia();
            await CriarFrequenciaPreDefinida(COMPONENTE_CURRICULAR_PORTUGUES_ID_138);

            var frequenciasSalvar = new List<FrequenciaSalvarAulaAlunosDto>();
            frequenciasSalvar.Add(
                new FrequenciaSalvarAulaAlunosDto
                {
                    AulaId = AULA_ID,
                    Alunos = new List<FrequenciaSalvarAlunoDto>
                    {
                        new FrequenciaSalvarAlunoDto
                        {
                            CodigoAluno = CODIGO_ALUNO_11,
                            Frequencias = ObterFrequenciaAula(CODIGO_ALUNO_11)
                        }
                    }
                });

            var useCaseSalvar = ServiceProvider.GetService<IInserirFrequenciaListaoUseCase>();
            useCaseSalvar.ShouldNotBeNull();
            await useCaseSalvar.Executar(frequenciasSalvar);

            var useCaseObterFrequencia = ServiceProvider.GetService<IObterFrequenciasPorPeriodoUseCase>();
            useCaseObterFrequencia.ShouldNotBeNull();

            var filtroFrequenciaPeriodo = new FiltroFrequenciaPorPeriodoDto
            {
                TurmaId = TURMA_CODIGO_1,
                DisciplinaId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                ComponenteCurricularId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                DataInicio = dataInicio,
                DataFim = dataFim
            };
            var frequencias = await useCaseObterFrequencia.Executar(filtroFrequenciaPeriodo);
            frequencias.ShouldNotBeNull();
            var novoAluno = frequencias.Alunos.First(aluno => aluno.CodigoAluno == CODIGO_ALUNO_11);
            novoAluno.ShouldNotBeNull();
            novoAluno.Marcador.Tipo.ShouldBe(TipoMarcadorFrequencia.Inativo);
            var dataSituacao = $"{novoAluno.DataSituacao.Day}/{novoAluno.DataSituacao.Month}/{novoAluno.DataSituacao.Year}";
            var descricaoMarcador = $"Estudante Inativo em {dataSituacao}";
            novoAluno.Marcador.Descricao.ShouldBe(descricaoMarcador);
        }

        [Fact(DisplayName = "Não apresentar alunos inativos antes do início do período")]
        public async Task Validar_nao_deve_aprensentar_de_aluno_inativo_antes_do_inicio()
        {
            var dataInicio = DateTimeExtension.HorarioBrasilia();
            var dataFim = dataInicio.AddMonths(2);

            await CriarDreUePerfil();
            await CriarComponenteCurricular();
            await CriarUsuarios();
            CriarClaimUsuario(ObterPerfilProfessor());
            await CriarTurma(Modalidade.Fundamental, ANO_8, false, TipoTurma.Regular);
            await CriarTipoCalendario(ModalidadeTipoCalendario.FundamentalMedio);
            await CriarPeriodoEscolar(dataInicio, dataFim, BIMESTRE_1);
            await CriarAula(dataInicio, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_LOGIN_2222222,
                    TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), TIPO_CALENDARIO_1);
            await InserirParametroSistema();
            await CriarMotivoAusencia();
            await CriarFrequenciaPreDefinida(COMPONENTE_CURRICULAR_PORTUGUES_ID_138);

            var frequenciasSalvar = new List<FrequenciaSalvarAulaAlunosDto>();
            frequenciasSalvar.Add(
                new FrequenciaSalvarAulaAlunosDto
                {
                    AulaId = AULA_ID,
                    Alunos = new List<FrequenciaSalvarAlunoDto>
                    {
                        new FrequenciaSalvarAlunoDto
                        {
                            CodigoAluno = CODIGO_ALUNO_14,
                            Frequencias = ObterFrequenciaAula(CODIGO_ALUNO_14)
                        }
                    }
                });

            var useCaseSalvar = ServiceProvider.GetService<IInserirFrequenciaListaoUseCase>();
            useCaseSalvar.ShouldNotBeNull();
            await useCaseSalvar.Executar(frequenciasSalvar);

            var useCaseObterFrequencia = ServiceProvider.GetService<IObterFrequenciasPorPeriodoUseCase>();
            useCaseObterFrequencia.ShouldNotBeNull();

            var filtroFrequenciaPeriodo = new FiltroFrequenciaPorPeriodoDto
            {
                TurmaId = TURMA_CODIGO_1,
                DisciplinaId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                ComponenteCurricularId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                DataInicio = dataInicio,
                DataFim = dataFim
            };
            var frequencias = await useCaseObterFrequencia.Executar(filtroFrequenciaPeriodo);
            frequencias.ShouldNotBeNull();
            var alunos = frequencias.Alunos.ToList();
            alunos.ShouldNotBeNull();
            alunos.Exists(aluno => aluno.CodigoAluno == CODIGO_ALUNO_14).ShouldBeFalse();
        }
    }
}
