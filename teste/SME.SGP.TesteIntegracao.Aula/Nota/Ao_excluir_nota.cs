using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SME.SGP.Infra.Dtos;
using SME.SGP.TesteIntegracao.Nota.ServicosFakes;
using Xunit;
using SME.SGP.TesteIntegracao.ServicosFakes;

namespace SME.SGP.TesteIntegracao.Nota
{
    public class Ao_excluir_nota : NotaTesteBase
    {
        public Ao_excluir_nota(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosPorTurmaEAnoLetivoQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterAlunosPorTurmaEAnoLetivoQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQuery, IEnumerable<ComponenteCurricularEol>>), typeof(ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQueryHandlerFakePortugues), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesCurricularesPorCodigoTurmaLoginEPerfilParaPlanejamentoQuery, IEnumerable<ComponenteCurricularEol>>), typeof(ObterComponentesCurricularesPorCodigoTurmaLoginEPerfilParaPlanejamentoQueryHandlerFakePortugues), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosPorTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ServicosFakes.ObterAlunosPorTurmaQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosEolPorTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterAlunosEolPorTurmaQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterProfessoresTitularesDisciplinasEolQuery, IEnumerable<ProfessorTitularDisciplinaEol>>), typeof(ObterProfessoresTitularesDisciplinasEolQueryHandlerFakePortugues), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaPorTurmasEDatasAvaliacaoQuery, IEnumerable<UsuarioPossuiAtribuicaoEolDto>>), typeof(ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaPorTurmasEDatasAvaliacaoQueryHandlerFakeNotas), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterDadosTurmaEolPorCodigoQuery, DadosTurmaEolDto>), typeof(ObterDadosTurmaEolPorCodigoQueryHandlerFakeRegular), ServiceLifetime.Scoped));
        }

        //[Fact]
        public async Task Ao_limpar_nota_numerica()
        {
            await CrieDados(TipoNota.Nota, ANO_5);
            await SalvarNota(false);
            await SalvarNota(true);
        }

        //[Fact]
        public async Task Ao_limpar_nota_conceito()
        {
            await CrieDados(TipoNota.Conceito, ANO_2);
            await SalvarConceito(false);
            await SalvarConceito(true);
        }

        private async Task SalvarConceito(bool limpaNota)
        {
            var dto = new NotaConceitoListaDto()
            {
                DisciplinaId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                TurmaId = TURMA_CODIGO_1,
                NotasConceitos = new List<NotaConceitoDto>()
                {
                    new NotaConceitoDto()
                    {
                        AlunoId = ALUNO_CODIGO_1,
                        Conceito = limpaNota ? null : 1,
                        AtividadeAvaliativaId = ATIVIDADE_AVALIATIVA_1
                    },
                    new NotaConceitoDto()
                    {
                        AlunoId = ALUNO_CODIGO_2,
                        Conceito = limpaNota ? null : 2,
                        AtividadeAvaliativaId = ATIVIDADE_AVALIATIVA_1
                    },
                     new NotaConceitoDto()
                    {
                        AlunoId = ALUNO_CODIGO_3,
                        Conceito = limpaNota ? null : 3,
                        AtividadeAvaliativaId = ATIVIDADE_AVALIATIVA_1
                    },
                }
            };

            await ExecuteTeste(TipoNota.Conceito, dto, limpaNota);
        }

        private async Task SalvarNota(bool limpaNota)
        {
            var dto = new NotaConceitoListaDto()
            {
                DisciplinaId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                TurmaId = TURMA_CODIGO_1,
                NotasConceitos = new List<NotaConceitoDto>()
                {
                    new NotaConceitoDto()
                    {
                        AlunoId = ALUNO_CODIGO_1,
                        Nota = limpaNota ? null : 7,
                        AtividadeAvaliativaId = ATIVIDADE_AVALIATIVA_1
                    },
                    new NotaConceitoDto()
                    {
                        AlunoId = ALUNO_CODIGO_2,
                        Nota = limpaNota ? null : 8,
                        AtividadeAvaliativaId = ATIVIDADE_AVALIATIVA_1
                    },
                    new NotaConceitoDto()
                    {
                        AlunoId = ALUNO_CODIGO_3,
                        Nota = limpaNota ? null : 9,
                        AtividadeAvaliativaId = ATIVIDADE_AVALIATIVA_1
                    }
                }
            };

            await ExecuteTeste(TipoNota.Nota, dto, limpaNota);
        }

        private async Task ExecuteTeste(TipoNota tipoNota, NotaConceitoListaDto dto, bool ehNotaLimpa)
        {
            var comando = ServiceProvider.GetService<IComandosNotasConceitos>();

            await comando.Salvar(dto);

            var notas = ObterTodos<NotaConceito>();

            notas.ShouldNotBeEmpty();
            notas.Count().ShouldBeGreaterThanOrEqualTo(1);
            notas.Exists(nota => nota.TipoNota == tipoNota).ShouldBe(true);
            var nota = notas.FirstOrDefault(nota => nota.AlunoId == ALUNO_CODIGO_1);
            nota.ShouldNotBeNull();
            TestaNota(tipoNota, nota, ehNotaLimpa);
        }

        private void TestaNota(TipoNota tipoNota, NotaConceito nota, bool ehNotaLimpa)
        {
            if (ehNotaLimpa)
                TestaNotaSemValor(tipoNota, nota);
            else
                TestaNotaComValor(tipoNota, nota);
        }

        private void TestaNotaComValor(TipoNota tipoNota, NotaConceito nota)
        {
            if (tipoNota == TipoNota.Nota)
                nota.Nota.ShouldBe(7);
            else
                nota.ConceitoId.ShouldBe(1);
        }

        private void TestaNotaSemValor(TipoNota tipoNota, NotaConceito nota)
        {
            if (tipoNota == TipoNota.Nota)
                nota.Nota.ShouldBeNull();
            else
                nota.ConceitoId.ShouldBeNull();
        }

        private async Task CrieDados(TipoNota tipo, string anoTurma)
        {
            var filtroNota = new FiltroNotasDto()
            {
                Perfil = ObterPerfilProfessor(),
                Modalidade = Modalidade.Fundamental,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Bimestre = BIMESTRE_2,
                ComponenteCurricular = COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                TipoNota = tipo,
                AnoTurma = anoTurma
            };

            await CriarDadosBase(filtroNota);
            await CriarAula(filtroNota.ComponenteCurricular, DATA_02_05_INICIO_BIMESTRE_2, RecorrenciaAula.AulaUnica, NUMERO_AULA_1);
            await CrieTipoAtividade();
            await CriarAtividadeAvaliativa(DATA_02_05_INICIO_BIMESTRE_2, filtroNota.ComponenteCurricular, USUARIO_PROFESSOR_LOGIN_2222222, false, ATIVIDADE_AVALIATIVA_1);
        }
    }
}
