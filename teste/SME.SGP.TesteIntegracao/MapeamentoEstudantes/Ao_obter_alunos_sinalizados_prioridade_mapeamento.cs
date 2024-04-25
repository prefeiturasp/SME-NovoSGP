using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.ProvaSP;
using SME.SGP.Infra.Dtos.Sondagem;
using SME.SGP.TesteIntegracao.ConsolidacaoConselhoDeClasse.ServicosFakes;
using SME.SGP.TesteIntegracao.MapeamentoEstudantes.Base;
using SME.SGP.TesteIntegracao.MapeamentoEstudantes.ServicosFakes;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.MapeamentoEstudantes
{
    public class Ao_obter_alunos_sinalizados_prioridade_mapeamento : MapeamentoBase
    {

        public Ao_obter_alunos_sinalizados_prioridade_mapeamento(CollectionFixture collectionFixture) : base(collectionFixture)
        {}

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosAtivosPorTurmaCodigoQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterAlunosAtivosPorTurmaCodigoQueryMapEstudanteHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosAtivosTurmaProgramaPapEolQuery, IEnumerable<AlunosTurmaProgramaPapDto>>), typeof(ObterAlunosAtivosTurmaProgramaPapEolQueryMapEstudanteHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterSondagemLPAlunoQuery, SondagemLPAlunoDto>), typeof(ObterSondagemLPAlunoQueryNaoAlfabeticoFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAvaliacoesExternasProvaSPAlunoQuery, IEnumerable<AvaliacaoExternaProvaSPDto>>), typeof(ObterAvaliacoesExternasProvaSPAlunoQueryFake), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName = "Mapeamento Estudantes - Obter códigos alunos com sindalização de prioridade")]
        public async Task Ao_listar_alunos_sinalizacao_prioridade()
        {
            await CriarDadosBase();
            var useCase = ServiceProvider.GetService<IObterAlunosSinalizadosPrioridadeMapeamentoEstudanteUseCase>();
            await InserirNaBase(new PlanoAEE()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_3,
                Situacao = SituacaoPlanoAEE.Validado,
                AlunoNome = ALUNO_NOME_1,
                AlunoNumero = 1,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var retorno = await useCase.Executar(TURMA_ID_1);
            retorno.ShouldNotBeNull();
            retorno.Count().ShouldBe(5);

        }

    }
}