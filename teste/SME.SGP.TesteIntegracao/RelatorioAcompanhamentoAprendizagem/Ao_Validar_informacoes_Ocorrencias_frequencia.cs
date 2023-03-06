using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.TesteIntegracao.RelatorioAcompanhamentoAprendizagem.ServicosFake;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;

namespace SME.SGP.TesteIntegracao.RelatorioAcompanhamentoAprendizagem
{
    public class Ao_Validar_informacoes_Ocorrencias_frequencia :  RelatorioAcompanhamentoAprendizagemTesteBase
    {
        private const string OCORRENCIA = "Ocorrencia";
        private const int OCORRENCIA_TIPO_INCIDENTE = 1;
        private const string TITULO_OCORRENCIA = "Titulo da Ocorrencia";
        private const string HORARIO_OCORRENCIA = "17:37";
        
        public Ao_Validar_informacoes_Ocorrencias_frequencia(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }
        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunoPorCodigoEAnoQuery, AlunoReduzidoDto>), typeof(ObterAlunoPorCodigoEAnoRelAcompanhamentoQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAcompanhamentoPorAlunoTurmaESemestreQuery, AcompanhamentoAlunoSemestre>), typeof(ObterAcompanhamentoPorAlunoTurmaESemestreAcompanhamentoQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterPeriodosFechamentoTurmaInfantilCalendarioIdBimestreQuery, IEnumerable<PeriodoFechamentoBimestre>>), typeof(ObterPeriodosFechamentoTurmaInfantilCalendarioIdBimestreQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTipoCalendarioIdPorTurmaQuery, long>), typeof(ObterTipoCalendarioIdPorTurmaQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterFrequenciaBimestresQuery, IEnumerable<FrequenciaBimestreAlunoDto>>), typeof(ObterFrequenciaBimestresQueryHandlerFake), ServiceLifetime.Scoped));
            
        }
        [Fact(DisplayName = "Relatório do Acompanhamento da Aprendizagem - Validar informações de frequência da criança")]
        public async Task Validar_Informações_de_frequencia_da_crianca()
        {
            await CriarDadosBasicos();
            var informacoesDeFrequenciaAlunoPorSemestreUseCase = ObterInformacoesDeFrequenciaAlunoPorSemestreUseCase();
            await CriarAula(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),DateTimeExtension.HorarioBrasilia().Date, RecorrenciaAula.AulaUnica);

            var obterFrequenciaAluno = await informacoesDeFrequenciaAlunoPorSemestreUseCase.Executar(new FiltroTurmaAlunoSemestreDto(1,1,2,138));
            obterFrequenciaAluno.ShouldNotBeNull();
            obterFrequenciaAluno.Count().ShouldBeGreaterThan(0);
        }
        
        [Fact(DisplayName = "Relatório do Acompanhamento da Aprendizagem - Validar as ocorrências da criança")]
        public async Task Validar_as_ocorrencias_da_crianca()
        {
            var useCaseOcorrencia = InserirOcorrenciaUseCase();
            var obterOcorrenciasPorAlunoUseCase = ObterOcorrenciasPorAlunoUseCase();
            await CriarDadosBasicos();

            var semestre = 1;
            var dtoOcorrencia = new InserirOcorrenciaDto
            {
                DataOcorrencia = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 02),
                Descricao = OCORRENCIA,
                OcorrenciaTipoId = OCORRENCIA_TIPO_INCIDENTE,
                Titulo = TITULO_OCORRENCIA,
                HoraOcorrencia = HORARIO_OCORRENCIA,
                TurmaId = TURMA_ID_1,
                UeId = UE_ID_1,
                CodigosAlunos = new List<long>{1}
            };
            await useCaseOcorrencia.Executar(dtoOcorrencia);
            
            var obeterOcorrencias = ObterTodos<Dominio.Ocorrencia>();
            obeterOcorrencias.Count.ShouldBeEquivalentTo(1);
            obeterOcorrencias.FirstOrDefault()?.DataOcorrencia.ShouldBeEquivalentTo(dtoOcorrencia.DataOcorrencia);
            obeterOcorrencias.FirstOrDefault()?.Titulo.ShouldBeEquivalentTo(dtoOcorrencia.Titulo);

            var ocorrenciaAluno = await obterOcorrenciasPorAlunoUseCase.Executar(new FiltroTurmaAlunoSemestreDto(dtoOcorrencia.TurmaId.GetValueOrDefault(), dtoOcorrencia.CodigosAlunos.First(), semestre));
            ocorrenciaAluno.Items.Count().ShouldBeEquivalentTo(1);
            ocorrenciaAluno.Items.FirstOrDefault()?.DataOcorrencia.ShouldBeEquivalentTo(dtoOcorrencia.DataOcorrencia);
            ocorrenciaAluno.Items.FirstOrDefault()?.Titulo.ShouldBeEquivalentTo(dtoOcorrencia.Titulo);
        }
        [Fact(DisplayName = "Relatório do Acompanhamento da Aprendizagem - Validar a sugestão do percurso individual a partir dos registros individuais")]
        public async Task Validar_a_sugestão_do_percurso_individual_a_partir_dos_registros_individuais()
        {
            await CriarDadosBasicos();
            var obterAcompanhamentoAlunoUseCase = ObterAcompanhamentoAlunoUseCase();

            var obterSugestao = await obterAcompanhamentoAlunoUseCase.Executar(new FiltroAcompanhamentoTurmaAlunoSemestreDto(1,"1",2,138));
            obterSugestao.ShouldNotBeNull();
            obterSugestao.PercursoIndividual.ShouldNotBeNull();
            obterSugestao.Auditoria.Id.ShouldBeEquivalentTo((long)0);
        }
    }
}