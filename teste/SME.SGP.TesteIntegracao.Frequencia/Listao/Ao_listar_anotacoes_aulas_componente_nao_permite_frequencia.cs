using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;

namespace SME.SGP.TesteIntegracao.Listao
{
    public class Ao_listar_anotacoes_aulas_componente_nao_permite_frequencia : ListaoTesteBase
    {
        public Ao_listar_anotacoes_aulas_componente_nao_permite_frequencia(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact(DisplayName = "Verificar se componentes que não permitem frequência apresentam aulas e flag anotação no listão")]
        public async Task Deve_apresentar_aulas_aluno_componente_nao_permite_frequencia()
        {
            var filtroListao = new FiltroListao
            {
                Bimestre = 3,
                Modalidade = Modalidade.Fundamental,
                Perfil = ObterPerfilProfessor(),
                AnoTurma = ANO_8,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                TipoTurma = TipoTurma.Regular,
                TurmaHistorica = false,
                ComponenteCurricularId = COMPONENTE_CURRICULAR_LIBRAS_COMPARTILHADA_ID_1116
            };

            await CriarDadosBasicos(filtroListao);
            await CriarAnotacoesFrequencia(CODIGO_ALUNO_1);

            var useCasePeriodo = ServiceProvider.GetService<IObterPeriodosPorComponenteUseCase>(); 
            var listaPeriodo = (await useCasePeriodo.Executar(TURMA_CODIGO_1, filtroListao.ComponenteCurricularId, false,
                filtroListao.Bimestre, true)).ToList();
            listaPeriodo.ShouldNotBeNull();

            var periodoSelecionado = listaPeriodo.FirstOrDefault();
            periodoSelecionado.ShouldNotBeNull();

            var useCase = ServiceProvider.GetService<IObterFrequenciasPorPeriodoUseCase>();
            useCase.ShouldNotBeNull();

            var frequencia = await useCase.Executar(new FiltroFrequenciaPorPeriodoDto
            {
                DataInicio = periodoSelecionado.DataInicio,
                DataFim = periodoSelecionado.DataFim,
                DisciplinaId = filtroListao.ComponenteCurricularId.ToString(),
                ComponenteCurricularId = filtroListao.ComponenteCurricularId.ToString(),
                TurmaId = TURMA_CODIGO_1
            });

            frequencia.ShouldNotBeNull();

            var listaCodigoAluno = frequencia.Alunos.Select(c => c.CodigoAluno).Distinct().ToList();
            listaCodigoAluno.ShouldNotBeNull();

            foreach (var codigoAluno in listaCodigoAluno)
            {
                var aulasAluno = frequencia.Alunos.FirstOrDefault(c => c.CodigoAluno == codigoAluno)?.Aulas;
                aulasAluno.ShouldNotBeNull();
                if (codigoAluno.Equals(CODIGO_ALUNO_1))
                    aulasAluno.All(a => a.PossuiAnotacao).ShouldBeTrue();
                else aulasAluno.All(a => a.PossuiAnotacao).ShouldBeFalse();
            }
        }

        protected async Task CriarAnotacoesFrequencia(string codigoAluno)
        {
            var codigoTurma = ObterTodos<Dominio.Turma>().Select(c => c.CodigoTurma).FirstOrDefault();
            var aulas = ObterTodos<Dominio.Aula>().Where(a => a.TurmaId == codigoTurma).OrderBy(a => a.DataAula).Select(c => c.Id);
            foreach (var idAula in aulas)
            {
                await InserirNaBase(new AnotacaoFrequenciaAluno()
                {
                    CodigoAluno = codigoAluno,
                    AulaId = idAula,
                    Anotacao = $"Anotação aula {idAula} - aluno {codigoAluno}",
                    CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01),
                    CriadoPor = "",
                    CriadoRF = ""
                });
            }
        }
    }
}