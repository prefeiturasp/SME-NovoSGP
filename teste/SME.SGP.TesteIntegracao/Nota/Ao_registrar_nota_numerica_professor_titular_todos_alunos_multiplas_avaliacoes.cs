using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.Nota
{
    public class Ao_registrar_nota_numerica_professor_titular_todos_alunos_multiplas_avaliacoes : NotaBase
    {
        public Ao_registrar_nota_numerica_professor_titular_todos_alunos_multiplas_avaliacoes(CollectionFixture collectionFixture) : base(collectionFixture)
        { }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery, bool>), typeof(ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQueryHandlerComPermissaoFake), ServiceLifetime.Scoped));
        }

        [Fact]
        public async Task Deve_permitir_cadastrar_aula_normal_bimestral_professor_especialista_fundamental()
        {
            var filtroNota = new FiltroNotasDto()
            {
                Perfil = ObterPerfilProfessor(),
                Modalidade = Modalidade.Fundamental,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Bimestre = BIMESTRE_1,
                ComponenteCurricular = COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                TipoCalendarioId = TIPO_CALENDARIO_1,
                CriarPeriodoEscolar = true,
                CriarPeriodoAbertura = true,
            };

            await CriarDadosBase(filtroNota);

            await CriarAula(filtroNota.ComponenteCurricular, DATA_03_01, RecorrenciaAula.AulaUnica, NUMERO_AULA_1);

            await CriarTipoAvaliacao(TipoAvaliacaoCodigo.AvaliacaoBimestral, AVALIACAO_NOME_1);

            await CriarAtividadeAvaliativa(DATA_03_01, TIPO_AVALIACAO_CODIGO_1, AVALIACAO_NOME_1);

            await CriarAtividadeAvaliativaDisciplina(ATIVIDADE_AVALIATIVA_1, filtroNota.ComponenteCurricular);

            await CriarAula(filtroNota.ComponenteCurricular, DATA_04_01, RecorrenciaAula.AulaUnica, NUMERO_AULA_1);

            await CriarTipoAvaliacao(TipoAvaliacaoCodigo.AvaliacaoBimestral, AVALIACAO_NOME_2);

            await CriarAtividadeAvaliativa(DATA_04_01, TIPO_AVALIACAO_CODIGO_2, AVALIACAO_NOME_2);

            await CriarAtividadeAvaliativaDisciplina(ATIVIDADE_AVALIATIVA_2, filtroNota.ComponenteCurricular);

            var notaconceito = new NotaConceitoListaDto()
            {
                DisciplinaId = filtroNota.ComponenteCurricular,
                TurmaId = TURMA_CODIGO_1,
                NotasConceitos = new List<NotaConceitoDto>()
                {
                    new NotaConceitoDto() { AlunoId = ALUNO_CODIGO_1, AtividadeAvaliativaId = ATIVIDADE_AVALIATIVA_1, Nota = NOTA_10},
                    new NotaConceitoDto() { AlunoId = ALUNO_CODIGO_2, AtividadeAvaliativaId = ATIVIDADE_AVALIATIVA_1, Nota = NOTA_9},
                    new NotaConceitoDto() { AlunoId = ALUNO_CODIGO_3, AtividadeAvaliativaId = ATIVIDADE_AVALIATIVA_1, Nota = NOTA_8},
                    new NotaConceitoDto() { AlunoId = ALUNO_CODIGO_4, AtividadeAvaliativaId = ATIVIDADE_AVALIATIVA_1, Nota = NOTA_7},
                    new NotaConceitoDto() { AlunoId = ALUNO_CODIGO_5, AtividadeAvaliativaId = ATIVIDADE_AVALIATIVA_1, Nota = NOTA_6},
                    new NotaConceitoDto() { AlunoId = ALUNO_CODIGO_6, AtividadeAvaliativaId = ATIVIDADE_AVALIATIVA_1, Nota = NOTA_5},
                    new NotaConceitoDto() { AlunoId = ALUNO_CODIGO_7, AtividadeAvaliativaId = ATIVIDADE_AVALIATIVA_1, Nota = NOTA_4},
                    new NotaConceitoDto() { AlunoId = ALUNO_CODIGO_8, AtividadeAvaliativaId = ATIVIDADE_AVALIATIVA_1, Nota = NOTA_3},
                    new NotaConceitoDto() { AlunoId = ALUNO_CODIGO_9, AtividadeAvaliativaId = ATIVIDADE_AVALIATIVA_1, Nota = NOTA_2},
                    new NotaConceitoDto() { AlunoId = ALUNO_CODIGO_10, AtividadeAvaliativaId = ATIVIDADE_AVALIATIVA_1, Nota = NOTA_1},

                    new NotaConceitoDto() { AlunoId = ALUNO_CODIGO_1, AtividadeAvaliativaId = ATIVIDADE_AVALIATIVA_2, Nota = NOTA_9},
                    new NotaConceitoDto() { AlunoId = ALUNO_CODIGO_2, AtividadeAvaliativaId = ATIVIDADE_AVALIATIVA_2, Nota = NOTA_10},
                    new NotaConceitoDto() { AlunoId = ALUNO_CODIGO_3, AtividadeAvaliativaId = ATIVIDADE_AVALIATIVA_2, Nota = NOTA_7},
                    new NotaConceitoDto() { AlunoId = ALUNO_CODIGO_4, AtividadeAvaliativaId = ATIVIDADE_AVALIATIVA_2, Nota = NOTA_8},
                    new NotaConceitoDto() { AlunoId = ALUNO_CODIGO_5, AtividadeAvaliativaId = ATIVIDADE_AVALIATIVA_2, Nota = NOTA_5},
                    new NotaConceitoDto() { AlunoId = ALUNO_CODIGO_6, AtividadeAvaliativaId = ATIVIDADE_AVALIATIVA_2, Nota = NOTA_6},
                    new NotaConceitoDto() { AlunoId = ALUNO_CODIGO_7, AtividadeAvaliativaId = ATIVIDADE_AVALIATIVA_2, Nota = NOTA_3},
                    new NotaConceitoDto() { AlunoId = ALUNO_CODIGO_8, AtividadeAvaliativaId = ATIVIDADE_AVALIATIVA_2, Nota = NOTA_4},
                    new NotaConceitoDto() { AlunoId = ALUNO_CODIGO_9, AtividadeAvaliativaId = ATIVIDADE_AVALIATIVA_2, Nota = NOTA_1},
                    new NotaConceitoDto() { AlunoId = ALUNO_CODIGO_10, AtividadeAvaliativaId = ATIVIDADE_AVALIATIVA_2, Nota = NOTA_2},
                }
            };

            var listaNotaConceito = new ListaNotasConceitosDto() 
            {
                TurmaId = long.Parse(TURMA_CODIGO_1),
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                Bimestre = BIMESTRE_1,
                DisciplinaCodigo = long.Parse(filtroNota.ComponenteCurricular),
                Modalidade = filtroNota.Modalidade,
                PeriodoEscolarId = PERIODO_ESCOLAR_CODIGO_1,
                Semestre = SEMESTRE_1,
                TurmaCodigo = TURMA_CODIGO_1,
                TurmaHistorico = false,
            };

            await ExecutarNotasConceito(notaconceito, listaNotaConceito);

            //Criar 2 aulas e 2 avaliações

            //var retorno = await ExecutarNotasConceito();

        }
    }
}