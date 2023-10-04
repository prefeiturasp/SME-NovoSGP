using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.TesteIntegracao.NotaFechamentoFinal.Base;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.NotaFechamentoFinal
{
    public class Ao_atualizar_cache_fechamento_nota : NotaFechamentoTesteBase
    {
        public Ao_atualizar_cache_fechamento_nota(CollectionFixture collectionFixture) : base(collectionFixture)
        {

        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRepositorioCache), typeof(RepositorioCacheMemoria), ServiceLifetime.Scoped));   
        }

        [Fact]
        public async Task Ao_atualizar_nota_cache_deve_ser_atualizado()
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
            await CriaFechamentoTurma_Disciplina(COMPONENTE_CURRICULAR_PORTUGUES_ID_138);
            await CriaFechamentoAluno();
            await CriaFechamentoNota(COMPONENTE_CURRICULAR_PORTUGUES_ID_138, NOTA_6);

            var mediator = ServiceProvider.GetService<IMediator>();
            //Carrega cache
            var fechamentos = await mediator.Send(new ObterPorFechamentosTurmaQuery(new long[] { FECHAMENTO_TURMA_DISCIPLINA_ID_1 }, TURMA_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString()));
            fechamentos.ShouldNotBeNull();
            fechamentos.Count().ShouldBeGreaterThan(0);

            var fechamentoNota = ObterTodos<FechamentoNota>().FirstOrDefault();

            fechamentoNota.Nota = NOTA_8;

            await mediator.Send(new AtualizarCacheFechamentoNotaCommand(
                                                fechamentoNota,
                                                CODIGO_ALUNO_1,
                                                TURMA_CODIGO_1,
                                                COMPONENTE_CURRICULAR_PORTUGUES_ID_138));

            var fechamentosNotas = await mediator.Send(new ObterPorFechamentosTurmaQuery(new long[] { FECHAMENTO_TURMA_DISCIPLINA_ID_1 }, TURMA_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString()));
            fechamentosNotas.ShouldNotBeNull();
            var fechamentoNotaAluno = fechamentosNotas.FirstOrDefault();
            fechamentoNotaAluno.ShouldNotBeNull();
            fechamentoNotaAluno.Nota.ShouldBe(NOTA_8);
        }

        private async Task CriaFechamentoNota(
                    long idDiciplina,
                    double nota)
        {
            await InserirNaBase(new FechamentoNota()
            {
                FechamentoAlunoId = FECHAMENTO_ALUNO_ID_1,
                DisciplinaId = idDiciplina,
                Nota = nota,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task CriaFechamentoAluno()
        {
            await InserirNaBase(new FechamentoAluno()
            {
                AlunoCodigo = CODIGO_ALUNO_1,
                FechamentoTurmaDisciplinaId = FECHAMENTO_TURMA_DISCIPLINA_ID_1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task CriaFechamentoTurma_Disciplina(long idDiciplina)
        {
            await InserirNaBase(new FechamentoTurma()
            {
                TurmaId = TURMA_ID_1,
                PeriodoEscolarId = null,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new FechamentoTurmaDisciplina()
            {
                DisciplinaId = idDiciplina,
                FechamentoTurmaId = FECHAMENTO_TURMA_DISCIPLINA_ID_1,
                Situacao = SituacaoFechamento.ProcessadoComSucesso,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }
    }
}
