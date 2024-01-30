using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.ConsultaCriancasEstudantesAusentes.ServicosFake;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.ConsultaCriancasEstudantesAusentes.Base
{
    public abstract class ConsultaAlunosAusentesBase : TesteBaseComuns
    {
        protected ConsultaAlunosAusentesBase(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosPorTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterAlunosPorTurmaQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosEolPorTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterAlunosEolPorTurmaQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTurmaItinerarioEnsinoMedioQuery, IEnumerable<TurmaItinerarioEnsinoMedioDto>>), typeof(ServicosFake.ObterTurmaItinerarioEnsinoMedioQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery, string[]>), typeof(ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaFreqGeralQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterDisciplinasPorCodigoTurmaQuery, IEnumerable<DisciplinaResposta>>), typeof(ObterDisciplinasPorCodigoTurmaQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTurmasPorCodigosQuery, IEnumerable<Dominio.Turma>>), typeof(ObterTurmasPorCodigosQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunoPorTurmaAlunoCodigoQuery, AlunoPorTurmaResposta>), typeof(ObterAlunoPorTurmaAlunoCodigoFrequenciaGlobalQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTodosAlunosNaTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ServicosFake.ObterTodosAlunosNaTurmaQueryHandlerFake), ServiceLifetime.Scoped));
        }

        protected async Task CriarDadosBasicos()
        {
            await CriarTipoCalendario(ModalidadeTipoCalendario.FundamentalMedio);
            await CriarItensComuns(true, DATA_02_05, DATA_08_07, BIMESTRE_2, TIPO_CALENDARIO_1);
            CriarClaimUsuario(ObterPerfilCoordenadorNAAPA());
            await CriarUsuarios();
            await CriarTurma(Modalidade.Medio);
        }
    }
}
