using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.TesteIntegracao.Fechamento.NotaFechamentoFinal.ServicosFakes;
using SME.SGP.TesteIntegracao.NotaFechamento.ServicosFakes;
using SME.SGP.TesteIntegracao.NotaFechamentoFinal.Base;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.ServicosFakes.Query;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.NotaFechamentoFinal
{
    /// <summary>
    /// Characterization: ConsultasFechamentoFinal.ObterFechamentos deve verificar o atendimento AEE de
    /// todos os alunos da turma numa única consulta, retornando EhAtendidoAEE = true apenas para os
    /// alunos com plano AEE ativo. Cenário reaproveita o mesmo setup de
    /// Ao_obter_nota.Deve_obter_nota_numerica_falta_ausencia_notaFinal_e_frenquencia, só acrescentando
    /// planos AEE para os alunos.
    /// </summary>
    public class Ao_verificar_atendimento_aee_no_fechamento_final : NotaFechamentoTesteBase
    {
        private const int REGISTRO_FREQUENCIA_ID = 1;

        public Ao_verificar_atendimento_aee_no_fechamento_final(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<PodePersistirTurmaDisciplinaQuery, bool>), typeof(PodePersistirTurmaDisciplinaQueryHandlerFakeRetornaTrue), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosPorTurmaEAnoLetivoQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterAlunosPorTurmaEAnoLetivoQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterDadosTurmaEolPorCodigoQuery, DadosTurmaEolDto>), typeof(ObterDadosTurmaEolPorCodigoQueryHandlerFakeRegular), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterValorParametroSistemaTipoEAnoQuery, string>), typeof(ObterValorParametroSistemaTipoEAnoQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesRegenciaPorAnoEolQuery, IEnumerable<ComponenteCurricularEol>>), typeof(ObterComponentesRegenciaPorAnoQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterPeriodoFechamentoPorCalendarioIdEBimestreQuery, PeriodoFechamentoBimestre>), typeof(ObterPeriodoFechamentoPorCalendarioIdEBimestreQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IServicoTelemetria), typeof(ContadorQueriesTelemetriaFake), ServiceLifetime.Singleton));
        }

        [Fact]
        public async Task Deve_disparar_uma_unica_consulta_de_plano_aee_para_toda_a_turma_no_fechamento_final()
        {
            await MontarCenario();

            var consulta = ServiceProvider.GetService<IConsultasFechamentoFinal>();
            var dto = new FechamentoFinalConsultaFiltroDto()
            {
                DisciplinaCodigo = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                TurmaCodigo = TURMA_CODIGO_1
            };

            ContadorQueriesTelemetriaFake.Limpar();
            await consulta.ObterFechamentos(dto);
            var consultasDePlanoAee = ContadorQueriesTelemetriaFake.ContarPorTrecho("from plano_aee");

            consultasDePlanoAee.ShouldBe(1);
        }

        [Fact]
        public async Task Deve_retornar_ehAtendidoAEE_consistente_com_a_consulta_unitaria_por_aluno()
        {
            await MontarCenario();

            var consulta = ServiceProvider.GetService<IConsultasFechamentoFinal>();
            var dto = new FechamentoFinalConsultaFiltroDto()
            {
                DisciplinaCodigo = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                TurmaCodigo = TURMA_CODIGO_1
            };

            var retorno = await consulta.ObterFechamentos(dto);

            retorno.Alunos.First(a => a.Codigo == CODIGO_ALUNO_1).EhAtendidoAEE.ShouldBeTrue();
            retorno.Alunos.First(a => a.Codigo == CODIGO_ALUNO_2).EhAtendidoAEE.ShouldBeFalse();
        }

        private async Task MontarCenario()
        {
            var filtroNotaFechamento = ObterFiltroNotas(
                    ObterPerfilProfessor(),
                    ANO_7,
                    COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                    TipoNota.Nota,
                    Modalidade.Fundamental,
                    ModalidadeTipoCalendario.FundamentalMedio,
                    false);
            await CriarDadosBase(filtroNotaFechamento);
            await CriaFechamentoBimestre(COMPONENTE_CURRICULAR_PORTUGUES_ID_138);
            await CriaFechamentoFinal(COMPONENTE_CURRICULAR_PORTUGUES_ID_138);
            await CriarAula(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString());
            await CriaRegistroDeFrenquencia();
            await CriaRegistroDeFrequenciaAluno(CODIGO_ALUNO_1, 3);
            await CriaRegistroDeFrequenciaAluno(CODIGO_ALUNO_2, 2);

            // Só CODIGO_ALUNO_1 tem plano AEE ativo.
            await CriarPlanoAEE(id: 1, versaoId: 1, aluno: CODIGO_ALUNO_1);
        }

        private async Task CriarAula(string componente)
        {
            await CriarAula(
                    DATA_INICIO_BIMESTRE_4,
                    RecorrenciaAula.AulaUnica,
                    TipoAula.Normal,
                    USUARIO_PROFESSOR_CODIGO_RF_2222222,
                    TURMA_CODIGO_1,
                    UE_CODIGO_1,
                    componente,
                    TIPO_CALENDARIO_1);
        }

        private async Task CriarPlanoAEE(long id, long versaoId, string aluno)
        {
            var dataCriacao = DateTimeExtension.HorarioBrasilia();

            await InserirNaBase(new Dominio.PlanoAEE
            {
                Id = id,
                TurmaId = TURMA_ID_1,
                AlunoCodigo = aluno,
                AlunoNome = $"Aluno {aluno}",
                AlunoNumero = 1,
                Situacao = SituacaoPlanoAEE.Validado,
                CriadoEm = dataCriacao,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new Dominio.PlanoAEEVersao
            {
                Id = versaoId,
                PlanoAEEId = id,
                Numero = 1,
                Excluido = false,
                CriadoEm = dataCriacao,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task CriaFechamentoFinal(long idDiciplina)
        {
            await CriaFechamentoTurma_Disciplina(null, idDiciplina, FECHAMENTO_TURMA_DISCIPLINA_ID_5);

            await CriaFechamentoAluno(FECHAMENTO_TURMA_DISCIPLINA_ID_5, CODIGO_ALUNO_1);
            await CriaFechamentoAluno(FECHAMENTO_TURMA_DISCIPLINA_ID_5, CODIGO_ALUNO_2);
        }

        private async Task CriaFechamentoBimestre(long idDiciplina)
        {
            await CriaFechamentoTurmaTodosBimestres(idDiciplina);
            await CriaFechamentoAlunoTodosBimestre(CODIGO_ALUNO_1);
            await CriaFechamentoAlunoTodosBimestre(CODIGO_ALUNO_2);
        }

        private async Task CriaFechamentoTurmaTodosBimestres(long idDiciplina)
        {
            await CriaFechamentoTurma_Disciplina(PERIODO_ESCOLAR_CODIGO_1, idDiciplina, FECHAMENTO_TURMA_ID_1);
            await CriaFechamentoTurma_Disciplina(PERIODO_ESCOLAR_CODIGO_2, idDiciplina, FECHAMENTO_TURMA_ID_2);
            await CriaFechamentoTurma_Disciplina(PERIODO_ESCOLAR_CODIGO_3, idDiciplina, FECHAMENTO_TURMA_ID_3);
            await CriaFechamentoTurma_Disciplina(PERIODO_ESCOLAR_CODIGO_4, idDiciplina, FECHAMENTO_TURMA_ID_4);
        }

        private async Task CriaFechamentoAlunoTodosBimestre(string codigoAluno)
        {
            await CriaFechamentoAluno(FECHAMENTO_TURMA_DISCIPLINA_ID_1, codigoAluno);
            await CriaFechamentoAluno(FECHAMENTO_TURMA_DISCIPLINA_ID_2, codigoAluno);
            await CriaFechamentoAluno(FECHAMENTO_TURMA_DISCIPLINA_ID_3, codigoAluno);
            await CriaFechamentoAluno(FECHAMENTO_TURMA_DISCIPLINA_ID_4, codigoAluno);
        }

        private async Task CriaFechamentoTurma_Disciplina(long? idPeriodo, long idDiciplina, long idFechamentoTurma)
        {
            await InserirNaBase(new FechamentoTurma()
            {
                TurmaId = TURMA_ID_1,
                PeriodoEscolarId = idPeriodo,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new FechamentoTurmaDisciplina()
            {
                DisciplinaId = idDiciplina,
                FechamentoTurmaId = idFechamentoTurma,
                Situacao = SituacaoFechamento.ProcessadoComSucesso,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task CriaFechamentoAluno(long idFechamenteoTurmaDiciplina, string codigoAluno)
        {
            await InserirNaBase(new FechamentoAluno()
            {
                AlunoCodigo = codigoAluno,
                FechamentoTurmaDisciplinaId = idFechamenteoTurmaDiciplina,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task CriaRegistroDeFrenquencia()
        {
            await InserirNaBase(new RegistroFrequencia
            {
                AulaId = AULA_ID,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task CriaRegistroDeFrequenciaAluno(string codigoAluno, int valor)
        {
            await InserirNaBase(new RegistroFrequenciaAluno
            {
                CodigoAluno = codigoAluno,
                RegistroFrequenciaId = REGISTRO_FREQUENCIA_ID,
                Valor = valor,
                NumeroAula = QUANTIDADE_AULA_4,
                AulaId = AULA_ID,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }
    }
}
