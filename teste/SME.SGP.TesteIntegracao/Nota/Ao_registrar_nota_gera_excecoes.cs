using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.Nota
{
    public class Ao_registrar_nota_gera_excecoes : NotaBase
    {
        private DateTime DATA_01_01 = new(DateTimeExtension.HorarioBrasilia().Year, 01, 01);
        private const string CODIGO_ALUNO_INATIVO = "999";

        public Ao_registrar_nota_gera_excecoes(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task Nao_lancar_nota_do_professor_com_atribuicao_encerrada()
        {
            _collectionFixture.Services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery, bool>), typeof(ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQueryHandlerSemPermissaoFake), ServiceLifetime.Scoped));
            _collectionFixture.BuildServiceProvider();

            var filtroNota = new FiltroNotasDto()
            {
                Perfil = ObterPerfilProfessor(),
                Modalidade = Modalidade.Fundamental,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Bimestre = BIMESTRE_2,
                ComponenteCurricular = COMPONENTE_REG_CLASSE_EJA_ETAPA_BASICA_ID_1114.ToString(),
                TipoNota = TipoNota.Nota
            };

            await CriarDadosBase(filtroNota);
            await CriarAula(filtroNota.ComponenteCurricular, DATA_02_05_INICIO_BIMESTRE_2, RecorrenciaAula.AulaUnica, NUMERO_AULA_1);
            await CrieTipoAtividade();
            await CriarAtividadeAvaliativa(DATA_02_05_INICIO_BIMESTRE_2, filtroNota.ComponenteCurricular, USUARIO_PROFESSOR_LOGIN_2222222, false, ATIVIDADE_AVALIATIVA_1);
            await ExecuteExecao(MensagensNegocioLancamentoNota.Nao_pode_fazer_alteracoes_nesta_turma_componente_e_data, ObterNotaDtoPadrao());
        }

        [Fact]
        public async Task Nao_e_possivel_atribuir_notas_para_avaliacao_com_data_futura()
        {
            var filtroNota = new FiltroNotasDto()
            {
                Perfil = ObterPerfilProfessor(),
                Modalidade = Modalidade.Fundamental,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Bimestre = BIMESTRE_2,
                ComponenteCurricular = COMPONENTE_REG_CLASSE_EJA_ETAPA_BASICA_ID_1114.ToString(),
                TipoNota = TipoNota.Nota
            };

            await CriarDadosBase(filtroNota);
            await CriarAula(filtroNota.ComponenteCurricular, DATA_02_05_INICIO_BIMESTRE_2, RecorrenciaAula.AulaUnica, NUMERO_AULA_1);
            await CrieTipoAtividade();
            await CriarAtividadeAvaliativa(DATA_25_07_INICIO_BIMESTRE_3, filtroNota.ComponenteCurricular, USUARIO_PROFESSOR_LOGIN_2222222, false, ATIVIDADE_AVALIATIVA_1);
            await ExecuteExecao(MensagensNegocioLancamentoNota.Nao_e_possivel_atribuir_notas_para_avaliacao_com_data_futura, ObterNotaDtoPadrao());
        }

        [Fact]
        public async Task Periodo_escolar_da_atividade_avaliativa_nao_encontrado()
        {
            var filtroNota = new FiltroNotasDto()
            {
                Perfil = ObterPerfilProfessor(),
                Modalidade = Modalidade.Fundamental,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Bimestre = BIMESTRE_2,
                ComponenteCurricular = COMPONENTE_REG_CLASSE_EJA_ETAPA_BASICA_ID_1114.ToString(),
                TipoNota = TipoNota.Nota,
                AnoTurma = ANO_2
            };

            await CriarDadosBase(filtroNota);
            await CriarAula(filtroNota.ComponenteCurricular, DATA_01_01, RecorrenciaAula.AulaUnica, NUMERO_AULA_1);
            await CrieTipoAtividade();
            await CriarAtividadeAvaliativa(DATA_01_01, filtroNota.ComponenteCurricular, USUARIO_PROFESSOR_LOGIN_2222222, false, ATIVIDADE_AVALIATIVA_1);
            await ExecuteExecao(MensagensNegocioLancamentoNota.Periodo_escolar_da_atividade_avaliativa_nao_encontrado, ObterNotaDtoPadrao());
        }

        public async Task Nao_pode_lancar_nota_para_turma_de_ano_posterior()
        {
            var filtroNota = new FiltroNotasDto()
            {
                Perfil = ObterPerfilProfessor(),
                Modalidade = Modalidade.Fundamental,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Bimestre = BIMESTRE_2,
                ComponenteCurricular = COMPONENTE_REG_CLASSE_EJA_ETAPA_BASICA_ID_1114.ToString(),
                TipoNota = TipoNota.Nota,
                AnoTurma = ANO_2,
                ConsiderarAnoAnterior = true
            };

            await CriarDadosBase(filtroNota);
            await CriarPeriodoEscolar(DATA_02_05_INICIO_BIMESTRE_2.AddYears(-1), DATA_08_07_FIM_BIMESTRE_2.AddYears(-1), BIMESTRE_2);
            await CriarAula(filtroNota.ComponenteCurricular, DATA_02_05_INICIO_BIMESTRE_2.AddYears(-1), RecorrenciaAula.AulaUnica, NUMERO_AULA_1);
            await CrieTipoAtividade();
            await CriarAtividadeAvaliativa(DATA_02_05_INICIO_BIMESTRE_2.AddYears(-1), filtroNota.ComponenteCurricular, USUARIO_PROFESSOR_LOGIN_2222222, false, ATIVIDADE_AVALIATIVA_1);
            await ExecuteExecao(MensagensNegocioLancamentoNota.Periodo_escolar_da_atividade_avaliativa_nao_encontrado, ObterNotaDtoPadrao());
        }

        [Fact]
        public async Task Nao_pode_lancar_nota_para_aluno_inativo()
        {
            var filtroNota = new FiltroNotasDto()
            {
                Perfil = ObterPerfilProfessor(),
                Modalidade = Modalidade.Fundamental,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Bimestre = BIMESTRE_2,
                ComponenteCurricular = COMPONENTE_REG_CLASSE_EJA_ETAPA_BASICA_ID_1114.ToString(),
                TipoNota = TipoNota.Nota,
                AnoTurma = ANO_2
            };

            await CriarDadosBase(filtroNota);
            await CriarAula(filtroNota.ComponenteCurricular, DATA_02_05_INICIO_BIMESTRE_2, RecorrenciaAula.AulaUnica, NUMERO_AULA_1);
            await CrieTipoAtividade();
            await CriarAtividadeAvaliativa(DATA_02_05_INICIO_BIMESTRE_2, filtroNota.ComponenteCurricular, USUARIO_PROFESSOR_LOGIN_2222222, false, ATIVIDADE_AVALIATIVA_1);

            var dto = new NotaConceitoListaDto()
            {
                DisciplinaId = COMPONENTE_REG_CLASSE_EJA_ETAPA_BASICA_ID_1114.ToString(),
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

        [Fact]
        public async Task Nao_foi_encontrado_o_ciclo_da_turma_informada()
        {
            await CriaBaseCustom();
            await CriarAtividadeAvaliativa(DATA_02_05_INICIO_BIMESTRE_2, COMPONENTE_REG_CLASSE_EJA_ETAPA_BASICA_ID_1114.ToString(), USUARIO_PROFESSOR_LOGIN_2222222, false, ATIVIDADE_AVALIATIVA_1);
            await CriarAbrangencia(ObterPerfilProfessor());
            await ExecuteExecao(MensagensNegocioLancamentoNota.Nao_foi_encontrado_o_ciclo_da_turma_informada, ObterNotaDtoPadrao());
        }

        [Fact]
        public async Task Nao_foi_encontrada_a_turma_informada_sem_abrangencia()
        {
            await CriaBaseCustom();
            await CriarAtividadeAvaliativa(DATA_02_05_INICIO_BIMESTRE_2, COMPONENTE_REG_CLASSE_EJA_ETAPA_BASICA_ID_1114.ToString(), USUARIO_PROFESSOR_LOGIN_2222222, false, ATIVIDADE_AVALIATIVA_1);
            await ExecuteExecao(MensagensNegocioLancamentoNota.Nao_foi_encontrada_a_turma_informada, ObterNotaDtoPadrao());
        }

        [Fact]
        public async Task Nao_e_possivel_atribuir_nota_para_avaliacao_futura()
        {
            await CriaBaseCustom();
            await CriarAtividadeAvaliativa(DATA_25_07_INICIO_BIMESTRE_3, COMPONENTE_REG_CLASSE_EJA_ETAPA_BASICA_ID_1114.ToString(), USUARIO_PROFESSOR_LOGIN_2222222, false, ATIVIDADE_AVALIATIVA_1);
            await CriarAbrangencia(ObterPerfilProfessor());
            await ExecuteExecao(MensagensNegocioLancamentoNota.Nao_e_possivel_atribuir_nota_para_avaliacao_futura, ObterNotaDtoPadrao());
        }

        [Fact]
        public async Task Nao_foi_encontrado_tipo_de_nota_para_a_avaliacao()
        {
            await CriaBaseCustom();
            await CriarAtividadeAvaliativa(DATA_02_05_INICIO_BIMESTRE_2, COMPONENTE_REG_CLASSE_EJA_ETAPA_BASICA_ID_1114.ToString(), USUARIO_PROFESSOR_LOGIN_2222222, false, ATIVIDADE_AVALIATIVA_1);
            await CriarAbrangencia(ObterPerfilProfessor());
            await CriarCiclo();
            await ExecuteExecao(MensagensNegocioLancamentoNota.Nao_foi_encontrado_tipo_de_nota_para_a_avaliacao, ObterNotaDtoPadrao());
        }

        [Fact]
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
                ComponenteCurricular = COMPONENTE_REG_CLASSE_EJA_ETAPA_BASICA_ID_1114.ToString(),
                TipoNota = TipoNota.Nota,
                AnoTurma = ANO_2
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
                DisciplinaId = COMPONENTE_REG_CLASSE_EJA_ETAPA_BASICA_ID_1114.ToString(),
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
    }
}
