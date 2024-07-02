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
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosDentroPeriodoQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterAlunosDentroPeriodoQueryMapEstudanteHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosAtivosTurmaProgramaPapEolQuery, IEnumerable<AlunosTurmaProgramaPapDto>>), typeof(ObterAlunosAtivosTurmaProgramaPapEolQueryMapEstudanteHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterSondagemLPAlunoQuery, SondagemLPAlunoDto>), typeof(ObterSondagemLPAlunoQueryNaoAlfabeticoFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAvaliacoesExternasProvaSPAlunoQuery, IEnumerable<AvaliacaoExternaProvaSPDto>>), typeof(ObterAvaliacoesExternasProvaSPAlunoQueryFake), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName = "Mapeamento Estudantes - Obter alunos com sinalização de prioridade")]
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

            for (int i = 1; i <= 2; i++)
                await InserirNaBaseAsync(new Dominio.MapeamentoEstudante()
                {
                    TurmaId = TURMA_ID_1,
                    AlunoCodigo = i.ToString(),
                    AlunoNome = $"Nome do aluno {i}",
                    Bimestre = 2,
                    CriadoEm = DateTimeExtension.HorarioBrasilia(),
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF
                });

            var retorno = await useCase.Executar(TURMA_ID_1, 2);
            retorno.ShouldNotBeNull();
            retorno.Count().ShouldBe(5);
            retorno.Count(r => r.PossuiMapeamentoEstudante).ShouldBe(2);
            string.Join(",", retorno.Where(r => r.PossuiMapeamentoEstudante)
                                    .OrderBy(r => r.CodigoAluno)
                                    .Select(r => r.CodigoAluno)).ShouldBe("1,2");
        }


    }
}