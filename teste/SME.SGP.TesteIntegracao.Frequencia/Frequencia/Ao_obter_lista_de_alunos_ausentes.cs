using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SME.SGP.Dominio.Interfaces;
using Xunit;

namespace SME.SGP.TesteIntegracao.Frequencia
{
    public class Ao_obter_lista_de_alunos_ausentes : FrequenciaTesteBase
    {
        public Ao_obter_lista_de_alunos_ausentes(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosPorTurmaEDataMatriculaQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterAlunosPorTurmaEDataMatriculaQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterValorParametroSistemaTipoEAnoQuery, string>), typeof(ObterValorParametroSistemaTipoEAnoQueryHandlerFake), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName = "Frequência - Deve obter lista de alunos ausentes")]
        public async Task Deve_obter_lista_de_alunos_ausentes()
        {
            await CriarDadosBasicos(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_07_08, BIMESTRE_2, DATA_02_05, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), false, NUMERO_AULAS_1);
            await CriarDadosFrenqueciaAluno(CODIGO_ALUNO_1,TipoFrequenciaAluno.PorDisciplina);
            await CriarDadosFrenqueciaAluno(CODIGO_ALUNO_2,TipoFrequenciaAluno.PorDisciplina);

            var mediator = ServiceProvider.GetService<IMediator>();
            var lista = await mediator.Send(new ObterListaAlunosComAusenciaQuery(TURMA_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), BIMESTRE_2));

            lista.ShouldNotBeNull();
            lista.ToList().Exists(ausencia => ausencia.Id == CODIGO_ALUNO_1).ShouldBeTrue();
            lista.ToList().Exists(ausencia => ausencia.Id == CODIGO_ALUNO_3).ShouldBeFalse();
        }
        
        [Fact(DisplayName = "Frequência - Deve obter lista de alunos faltosos bimestre")]
        public async Task Deve_obter_lista_de_alunos_faltosos_bimestre()
        {
            await CriarDadosBasicos(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_07_08, BIMESTRE_2, DATA_02_05, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), false, NUMERO_AULAS_1);
            await CriarDadosFrenqueciaAluno(CODIGO_ALUNO_1,TipoFrequenciaAluno.Geral,12);
            await CriarDadosFrenqueciaAluno(CODIGO_ALUNO_2,TipoFrequenciaAluno.Geral);

            var repositorioFrequenciaAlunoDisciplinaPeriodoConsulta = ServiceProvider.GetService<IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta>();
            var alunosFaltososBimestre = repositorioFrequenciaAlunoDisciplinaPeriodoConsulta.ObterAlunosFaltososBimestre(ModalidadeTipoCalendario.FundamentalMedio,5,BIMESTRE_2, DATA_02_05.Year);
            alunosFaltososBimestre.ShouldNotBeNull();
            alunosFaltososBimestre.Count().ShouldBe(1);
        }
    }
}
