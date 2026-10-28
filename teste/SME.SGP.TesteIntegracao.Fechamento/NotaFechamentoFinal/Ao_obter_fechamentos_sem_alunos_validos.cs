using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.TesteIntegracao.Fechamento.NotaFechamentoFinal.ServicosFakes;
using SME.SGP.TesteIntegracao.NotaFechamento.ServicosFakes;
using SME.SGP.TesteIntegracao.NotaFechamentoBimestre.ServicosFakes;
using SME.SGP.TesteIntegracao.NotaFechamentoFinal.Base;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.ServicosFakes.Query;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.NotaFechamentoFinal
{
    /// <summary>
    /// A consulta de frequência passou a ser feita em lote, fora do foreach. Como a query exige ao
    /// menos um código de aluno, uma turma sem aluno válido no último bimestre não pode disparar
    /// exceção de validação — deve devolver a listagem vazia.
    /// </summary>
    public class Ao_obter_fechamentos_sem_alunos_validos : NotaFechamentoTesteBase
    {
        public Ao_obter_fechamentos_sem_alunos_validos(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<PodePersistirTurmaDisciplinaQuery, bool>), typeof(PodePersistirTurmaDisciplinaQueryHandlerFakeRetornaTrue), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosPorTurmaEAnoLetivoQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterAlunosPorTurmaEAnoLetivoQueryHandlerFakeAlunosInativos), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterDadosTurmaEolPorCodigoQuery, DadosTurmaEolDto>), typeof(ObterDadosTurmaEolPorCodigoQueryHandlerFakeRegular), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterValorParametroSistemaTipoEAnoQuery, string>), typeof(ObterValorParametroSistemaTipoEAnoQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesRegenciaPorAnoEolQuery, IEnumerable<ComponenteCurricularEol>>), typeof(ObterComponentesRegenciaPorAnoQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterPeriodoFechamentoPorCalendarioIdEBimestreQuery, PeriodoFechamentoBimestre>), typeof(ObterPeriodoFechamentoPorCalendarioIdEBimestreQueryHandlerFake), ServiceLifetime.Scoped));
        }

        [Fact]
        public async Task Deve_retornar_listagem_vazia_quando_nenhum_aluno_e_valido()
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
            await CriarFechamentoUltimoBimestre();

            var consulta = ServiceProvider.GetService<IConsultasFechamentoFinal>();

            var retorno = await consulta.ObterFechamentos(new FechamentoFinalConsultaFiltroDto
            {
                DisciplinaCodigo = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                TurmaCodigo = TURMA_CODIGO_1
            });

            retorno.ShouldNotBeNull();
            retorno.Alunos.ShouldBeEmpty();
        }

        /// <summary>
        /// Sem o fechamento do último bimestre a consulta lança exceção de negócio antes de chegar
        /// na listagem de alunos. O fechamento_aluno é exigido pelo inner join da consulta.
        /// </summary>
        private async Task CriarFechamentoUltimoBimestre()
        {
            var ultimoPeriodoEscolar = ObterTodos<PeriodoEscolar>().OrderByDescending(p => p.Bimestre).First();

            var fechamentoTurmaId = await InserirNaBaseAsync(new FechamentoTurma
            {
                TurmaId = TURMA_ID_1,
                PeriodoEscolarId = ultimoPeriodoEscolar.Id,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var fechamentoTurmaDisciplinaId = await InserirNaBaseAsync(new FechamentoTurmaDisciplina
            {
                DisciplinaId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                FechamentoTurmaId = fechamentoTurmaId,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new FechamentoAluno
            {
                FechamentoTurmaDisciplinaId = fechamentoTurmaDisciplinaId,
                AlunoCodigo = CODIGO_ALUNO_1,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }
    }
}
