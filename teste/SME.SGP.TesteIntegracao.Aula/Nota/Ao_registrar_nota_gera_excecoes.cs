using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SME.SGP.Infra.Dtos;
using SME.SGP.TesteIntegracao.Nota.ServicosFakes;
using Xunit;

namespace SME.SGP.TesteIntegracao.Nota
{
    public class Ao_registrar_nota_gera_excecoes : NotaTesteBase
    {
        private new DateTime DATA_01_01 = new(DateTimeExtension.HorarioBrasilia().Year, 01, 01);
        private const string CODIGO_ALUNO_INATIVO = "999";

        public Ao_registrar_nota_gera_excecoes(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosPorTurmaEAnoLetivoQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterAlunosPorTurmaEAnoLetivoQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQuery, IEnumerable<ComponenteCurricularEol>>), typeof(ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQueryHandlerFakePortugues), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesCurricularesPorCodigoTurmaLoginEPerfilParaPlanejamentoQuery, IEnumerable<ComponenteCurricularEol>>), typeof(ObterComponentesCurricularesPorCodigoTurmaLoginEPerfilParaPlanejamentoQueryHandlerFakePortugues), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosPorTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(SME.SGP.TesteIntegracao.Nota.ServicosFakes.ObterAlunosPorTurmaQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery, bool>), typeof(ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQueryHandlerComPermissaoFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosEolPorTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterAlunosEolPorTurmaQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosEolPorTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterAlunosEolPorTurmaQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterProfessoresTitularesDisciplinasEolQuery, IEnumerable<ProfessorTitularDisciplinaEol>>), typeof(ObterProfessoresTitularesDisciplinasEolQueryHandlerFakePortugues), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaPorTurmasEDatasAvaliacaoQuery, IEnumerable<UsuarioPossuiAtribuicaoEolDto>>), typeof(ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaPorTurmasEDatasAvaliacaoQueryHandlerFakeNotas), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterDadosTurmaEolPorCodigoQuery, DadosTurmaEolDto>), typeof(ObterDadosTurmaEolPorCodigoQueryHandlerFakeRegular), ServiceLifetime.Scoped));
        }

        //[Fact]
        public async Task Nao_e_possivel_atribuir_notas_para_avaliacao_com_data_futura()
        {
            var filtroNota = new FiltroNotasDto()
            {
                Perfil = ObterPerfilProfessor(),
                Modalidade = Modalidade.Fundamental,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Bimestre = BIMESTRE_2,
                ComponenteCurricular = COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                TipoNota = TipoNota.Nota
            };

            await CriarDadosBase(filtroNota);
            await CriarAula(filtroNota.ComponenteCurricular, DATA_02_05_INICIO_BIMESTRE_2, RecorrenciaAula.AulaUnica, NUMERO_AULA_1);
            await CrieTipoAtividade();
            await CriarAtividadeAvaliativa(DATA_25_07_INICIO_BIMESTRE_3, filtroNota.ComponenteCurricular, USUARIO_PROFESSOR_LOGIN_2222222, false, ATIVIDADE_AVALIATIVA_1);
            await ExecuteExecao(MensagensNegocioLancamentoNota.Nao_e_possivel_atribuir_notas_para_avaliacao_com_data_futura, ObterNotaDtoPadrao());
        }

        //[Fact]
        public async Task Periodo_escolar_da_atividade_avaliativa_nao_encontrado()
        {
            var filtroNota = new FiltroNotasDto()
            {
                Perfil = ObterPerfilProfessor(),
                Modalidade = Modalidade.Fundamental,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Bimestre = BIMESTRE_2,
                ComponenteCurricular = COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                TipoNota = TipoNota.Nota,
                AnoTurma = ANO_5
            };

            await CriarDadosBase(filtroNota);
            await CriarAula(filtroNota.ComponenteCurricular, DATA_01_01, RecorrenciaAula.AulaUnica, NUMERO_AULA_1);
            await CrieTipoAtividade();
            await CriarAtividadeAvaliativa(DATA_01_01, filtroNota.ComponenteCurricular, USUARIO_PROFESSOR_LOGIN_2222222, false, ATIVIDADE_AVALIATIVA_1);
            await ExecuteExecao(MensagensNegocioLancamentoNota.Periodo_escolar_da_atividade_avaliativa_nao_encontrado, ObterNotaDtoPadrao());
        }

        //[Fact]
        public async Task Nao_pode_lancar_nota_para_turma_com_bimestre_encerrado()
        {
            var filtroNota = new FiltroNotasDto()
            {
                Perfil = ObterPerfilProfessor(),
                Modalidade = Modalidade.Fundamental,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Bimestre = BIMESTRE_2,
                ComponenteCurricular = COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                TipoNota = TipoNota.Nota,
                AnoTurma = ANO_5,
                CriarPeriodoEscolar = false
            };

            await CriarDadosBase(filtroNota);
            await CriarPeriodoEscolar(DATA_01_01_INICIO_BIMESTRE_1, DATA_01_05_FIM_BIMESTRE_1, BIMESTRE_2);
            await CriarAula(filtroNota.ComponenteCurricular, DATA_02_05_INICIO_BIMESTRE_2, RecorrenciaAula.AulaUnica, NUMERO_AULA_1);
            await CrieTipoAtividade();
            await CriarAtividadeAvaliativa(DATA_02_05_INICIO_BIMESTRE_2, filtroNota.ComponenteCurricular, USUARIO_PROFESSOR_LOGIN_2222222, false, ATIVIDADE_AVALIATIVA_1);
            await ExecuteExecao(MensagensNegocioLancamentoNota.Periodo_escolar_da_atividade_avaliativa_nao_encontrado, ObterNotaDtoPadrao());
        }

        //[Fact]
        public async Task Nao_pode_lancar_nota_para_turma_de_ano_anterior()
        {
            var filtroNota = new FiltroNotasDto()
            {
                Perfil = ObterPerfilProfessor(),
                Modalidade = Modalidade.Fundamental,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Bimestre = BIMESTRE_2,
                ComponenteCurricular = COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                TipoNota = TipoNota.Nota,
                AnoTurma = ANO_5,
                ConsiderarAnoAnterior = true,
                CriarPeriodoEscolar = false
            };

            await CriarDadosBase(filtroNota);
            await CriarPeriodoEscolar(DATA_02_05_INICIO_BIMESTRE_2, DATA_24_07_FIM_BIMESTRE_2, BIMESTRE_2);
            await CriarAula(filtroNota.ComponenteCurricular, DATA_02_05_INICIO_BIMESTRE_2.AddYears(-1), RecorrenciaAula.AulaUnica, NUMERO_AULA_1);
            await CrieTipoAtividade();
            await CrieTipoNotaAnoAnterior();
            await CriarAtividadeAvaliativa(DATA_02_05_INICIO_BIMESTRE_2.AddYears(-1), filtroNota.ComponenteCurricular, USUARIO_PROFESSOR_LOGIN_2222222, false, ATIVIDADE_AVALIATIVA_1);
            await ExecuteExecao(MensagensNegocioLancamentoNota.Periodo_escolar_da_atividade_avaliativa_nao_encontrado, ObterNotaDtoPadrao());
        }

        //[Fact]
        public async Task Nao_pode_lancar_nota_para_aluno_inativo()
        {
            var filtroNota = new FiltroNotasDto()
            {
                Perfil = ObterPerfilProfessor(),
                Modalidade = Modalidade.Fundamental,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Bimestre = BIMESTRE_2,
                ComponenteCurricular = COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                TipoNota = TipoNota.Nota,
                AnoTurma = ANO_5
            };

            await CriarDadosBase(filtroNota);
            await CriarAula(filtroNota.ComponenteCurricular, DATA_02_05_INICIO_BIMESTRE_2, RecorrenciaAula.AulaUnica, NUMERO_AULA_1);
            await CrieTipoAtividade();
            await CriarAtividadeAvaliativa(DATA_02_05_INICIO_BIMESTRE_2, filtroNota.ComponenteCurricular, USUARIO_PROFESSOR_LOGIN_2222222, false, ATIVIDADE_AVALIATIVA_1);

            var dto = new NotaConceitoListaDto()
            {
                DisciplinaId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                TurmaId = TURMA_CODIGO_1,
                NotasConceitos = new List<NotaConceitoDto>()
                {
                    new NotaConceitoDto()
                    {
                        AlunoId = CODIGO_ALUNO_INATIVO,
                        Nota = 7,
                        AtividadeAvaliativaId = ATIVIDADE_AVALIATIVA_1
                    }
                }
            };

            await ExecuteExecao(String.Format(MensagensNegocioLancamentoNota.Nao_foi_encontrado_aluno_com_o_codigo, CODIGO_ALUNO_INATIVO), dto);
        }

        //[Fact]
        public async Task Nao_foi_encontrado_o_ciclo_da_turma_informada()
        {
            await CriaBaseCustom();
            await CriarAtividadeAvaliativa(DATA_02_05_INICIO_BIMESTRE_2, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), USUARIO_PROFESSOR_LOGIN_2222222, false, ATIVIDADE_AVALIATIVA_1);
            await CriarAbrangencia(ObterPerfilProfessor());
            await ExecuteExecao(MensagensNegocioLancamentoNota.Nao_foi_encontrado_o_ciclo_da_turma_informada, ObterNotaDtoPadrao());
        }

        //[Fact]
        public async Task Nao_foi_encontrada_a_turma_informada_sem_abrangencia()
        {
            await CriaBaseCustom();
            await CriarAtividadeAvaliativa(DATA_02_05_INICIO_BIMESTRE_2, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), USUARIO_PROFESSOR_LOGIN_2222222, false, ATIVIDADE_AVALIATIVA_1);
            await ExecuteExecao(MensagensNegocioLancamentoNota.Nao_foi_encontrada_a_turma_informada, ObterNotaDtoPadrao());
        }

        //[Fact]
        public async Task Nao_e_possivel_atribuir_nota_para_avaliacao_futura()
        {
            await CriaBaseCustom();
            await CriarAtividadeAvaliativa(DATA_25_07_INICIO_BIMESTRE_3, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), USUARIO_PROFESSOR_LOGIN_2222222, false, ATIVIDADE_AVALIATIVA_1);
            await CriarAbrangencia(ObterPerfilProfessor());
            await ExecuteExecao(MensagensNegocioLancamentoNota.Nao_e_possivel_atribuir_nota_para_avaliacao_futura, ObterNotaDtoPadrao());
        }

        //[Fact]
        public async Task Nao_foi_encontrado_tipo_de_nota_para_a_avaliacao()
        {
            await CriaBaseCustom();
            await CriarAtividadeAvaliativa(DATA_02_05_INICIO_BIMESTRE_2, COMPONENTE_REG_CLASSE_EJA_ETAPA_BASICA_ID_1114.ToString(), USUARIO_PROFESSOR_LOGIN_2222222, false, ATIVIDADE_AVALIATIVA_1);
            await CriarAbrangencia(ObterPerfilProfessor());
            await CriarCiclo();
            await ExecuteExecao(MensagensNegocioLancamentoNota.Nao_foi_encontrado_tipo_de_nota_para_a_avaliacao, ObterNotaDtoPadrao());
        }

        //[Fact]
        public async Task Nao_foi_encontrado_avaliacao()
        {
            await CriaBaseCustom();
            await ExecuteExecao(MensagensNegocioLancamentoNota.Nao_foi_encontrada_nenhuma_da_avaliacao_informada, ObterNotaDtoPadrao());
        }

        private async Task CriaBaseCustom()
        {
            var filtroNota = new FiltroNotasDto()
            {
                Perfil = ObterPerfilProfessor(),
                Modalidade = Modalidade.Fundamental,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Bimestre = BIMESTRE_2,
                ComponenteCurricular = COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                TipoNota = TipoNota.Nota,
                AnoTurma = ANO_5
            };

            await CriarDreUePerfilComponenteCurricular();
            CriarClaimUsuario(filtroNota.Perfil);
            await CriarUsuarios();
            await CriarTurmaTipoCalendario(filtroNota);
            await CriarPeriodoEscolar();
            await CriarAula(filtroNota.ComponenteCurricular, DATA_02_05_INICIO_BIMESTRE_2, RecorrenciaAula.AulaUnica, NUMERO_AULA_1);
            await CrieTipoAtividade();
        }

        private NotaConceitoListaDto ObterNotaDtoPadrao()
        {
            return new NotaConceitoListaDto()
            {
                DisciplinaId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                TurmaId = TURMA_CODIGO_1,
                NotasConceitos = new List<NotaConceitoDto>()
                {
                    new NotaConceitoDto()
                    {
                        AlunoId = ALUNO_CODIGO_1,
                        Nota = 7,
                        AtividadeAvaliativaId = ATIVIDADE_AVALIATIVA_1
                    }
                }
            };
        }

        private async Task ExecuteExecao(string mensagem, NotaConceitoListaDto dto)
        {
            var comando = ServiceProvider.GetService<IComandosNotasConceitos>();
            var excecao = await Assert.ThrowsAsync<NegocioException>(() => comando.Salvar(dto));

            excecao.Message.ShouldBe(mensagem);
        }

        private async Task CrieTipoNotaAnoAnterior()
        {
            await InserirNaBase(new NotaTipoValor()
            {
                Ativo = true,
                InicioVigencia = new DateTime(DateTimeExtension.HorarioBrasilia().AddYears(-1).Year, 01, 01),
                TipoNota = TipoNota.Nota,
                Descricao = NOTA,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new NotaConceitoCicloParametro()
            {
                CicloId = 2,
                TipoNotaId = 1,
                QtdMinimaAvalicoes = 1,
                PercentualAlerta = 50,
                Ativo = true,
                InicioVigencia = new DateTime(DateTimeExtension.HorarioBrasilia().AddYears(-1).Year, 01, 01),
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }
    }
}
