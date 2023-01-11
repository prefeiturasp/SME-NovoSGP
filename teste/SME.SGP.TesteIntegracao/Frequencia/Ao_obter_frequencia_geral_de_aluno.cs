﻿using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.Frequencia
{
    public class Ao_obter_frequencia_geral_de_aluno : FrequenciaTesteBase
    {
        private const string VALOR_83 = "83,33";
        private const string VALOR_100 = "100";
        public Ao_obter_frequencia_geral_de_aluno(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTurmaItinerarioEnsinoMedioQuery, IEnumerable<TurmaItinerarioEnsinoMedioDto>>), typeof(ObterTurmaItinerarioEnsinoMedioQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery, string[]>), typeof(ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQueryHandlerFake), ServiceLifetime.Scoped));
        }

        [Fact]
        public async Task Deve_obter_frequencia_geral_de_aluno_com_ausencia()
        {
            await CriarDadosBasicos(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_07_08, BIMESTRE_2, DateTimeExtension.HorarioBrasilia().Date, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), false, NUMERO_AULAS_1);
            await CriarDadosFrenqueciaAluno(CODIGO_ALUNO_1,TipoFrequenciaAluno.Geral);

            await CrieRegistroDeFrenquencia();

            var mediator = ServiceProvider.GetService<IMediator>();
            var valor = await mediator.Send(new ObterConsultaFrequenciaGeralAlunoQuery(CODIGO_ALUNO_1, TURMA_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString()));
            
            valor.Replace(".",",").ShouldNotBeEmpty();
            valor.Replace(".",",").ShouldBe(VALOR_83);
        }

        [Fact]
        public async Task Deve_obter_frequencia_geral_de_aluno_sem_ausencia()
        {
            await CriarDadosBasicos(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_07_08, BIMESTRE_2, DATA_02_05, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), true, NUMERO_AULAS_1);
            await CriarDadosFrenqueciaAluno(CODIGO_ALUNO_2, TipoFrequenciaAluno.Geral,0);
            await CrieRegistroDeFrenquencia();

            var mediator = ServiceProvider.GetService<IMediator>();
            var valor = await mediator.Send(new ObterConsultaFrequenciaGeralAlunoQuery(CODIGO_ALUNO_2, TURMA_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString()));

            valor.ShouldNotBeEmpty();
            valor.ShouldBe(VALOR_100);
        }
    }
}
