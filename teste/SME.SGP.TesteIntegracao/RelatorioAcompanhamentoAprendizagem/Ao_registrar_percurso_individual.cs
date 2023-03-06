using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.RelatorioAcompanhamentoAprendizagem.ServicosFake;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;

namespace SME.SGP.TesteIntegracao.RelatorioAcompanhamentoAprendizagem
{
    public class Ao_registrar_percurso_individual : RelatorioAcompanhamentoAprendizagemTesteBase
    {
        public Ao_registrar_percurso_individual(CollectionFixture collectionFixture) : base(collectionFixture)
        {}

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunoPorCodigoEolQuery, AlunoPorTurmaResposta>),
                typeof(ObterAlunoPorCodigoEolQueryHandlerAlunoAtivoFake), ServiceLifetime.Scoped));
        }
        [Fact(DisplayName = "Relatorio Acompanhamento Aprendizagem - Deve registrar o percurso individual no 1º semestre")]
        public async Task Deve_registrar_percurso_individual_para_primeiro_semestre()
        {
            await CriarDadosBasicos(abrirPeriodos:false);
            var salvarAcompanhamentoAlunoUseCase = ObterServicoSalvarAcompanhamentoAlunoUseCase();
                
            await CriarPeriodoEscolarCustomizadoSegundoBimestre(true);
            
            var acompanhamentoAlunoDto = new AcompanhamentoAlunoDto { 
                TurmaId = TURMA_ID_1, 
                Semestre = PRIMEIRO_SEMESTRE, 
                TextoSugerido = true,
                PercursoIndividual = TEXTO_PADRAO_PERCURSO_INDIVIDUAL,
                AlunoCodigo = ALUNO_CODIGO_1
            };
            
            var retorno = await salvarAcompanhamentoAlunoUseCase.Executar(acompanhamentoAlunoDto);
            retorno.ShouldNotBeNull();
            retorno.AcompanhamentoAlunoId.ShouldBe(1);
            retorno.AcompanhamentoAlunoSemestreId.ShouldBe(1);
            retorno.Auditoria.ShouldNotBeNull();
            retorno.Auditoria.Id.ShouldBe(1);
            
            var acompanhamentoAluno = ObterTodos<AcompanhamentoAluno>();
            acompanhamentoAluno.ShouldNotBeNull();
            acompanhamentoAluno.FirstOrDefault()?.TurmaId.ShouldBe(TURMA_ID_1);
            acompanhamentoAluno.FirstOrDefault()?.AlunoCodigo.ShouldBe(ALUNO_CODIGO_1);
            
            var acompanhamentoAlunoSemestres = ObterTodos<AcompanhamentoAlunoSemestre>();
            acompanhamentoAlunoSemestres.ShouldNotBeNull();
            acompanhamentoAlunoSemestres.FirstOrDefault()?.Semestre.ShouldBe(PRIMEIRO_SEMESTRE);
            acompanhamentoAlunoSemestres.FirstOrDefault()?.PercursoIndividual.ShouldBe(TEXTO_PADRAO_PERCURSO_INDIVIDUAL);
            acompanhamentoAlunoSemestres.FirstOrDefault()?.AcompanhamentoAlunoId.ShouldBe(acompanhamentoAluno.FirstOrDefault()!.Id);
        }

        
        [Fact(DisplayName = "Relatorio Acompanhamento Aprendizagem - Deve registrar o percurso individual no 2º semestre")]
        public async Task Deve_registrar_percurso_individual_para_segundo_semestre()
        {
            await CriarDadosBasicos(abrirPeriodos:false);
            var salvarAcompanhamentoAlunoUseCase = ObterServicoSalvarAcompanhamentoAlunoUseCase();
            
            await CriarPeriodoEscolarCustomizadoQuartoBimestre(true);
                
            var acompanhamentoAlunoDto = new AcompanhamentoAlunoDto { 
                TurmaId = TURMA_ID_1, 
                Semestre = SEGUNDO_SEMESTRE, 
                TextoSugerido = true,
                PercursoIndividual = TEXTO_PADRAO_PERCURSO_INDIVIDUAL,
                AlunoCodigo = ALUNO_CODIGO_1
            };
            
            var retorno = await salvarAcompanhamentoAlunoUseCase.Executar(acompanhamentoAlunoDto);
            retorno.ShouldNotBeNull();
            retorno.AcompanhamentoAlunoId.ShouldBe(1);
            retorno.AcompanhamentoAlunoSemestreId.ShouldBe(1);
            retorno.Auditoria.ShouldNotBeNull();
            retorno.Auditoria.Id.ShouldBe(1);
            
            var acompanhamentoAluno = ObterTodos<AcompanhamentoAluno>();
            acompanhamentoAluno.ShouldNotBeNull();
            acompanhamentoAluno.FirstOrDefault()?.TurmaId.ShouldBe(TURMA_ID_1);
            acompanhamentoAluno.FirstOrDefault()?.AlunoCodigo.ShouldBe(ALUNO_CODIGO_1);
            
            var acompanhamentoAlunoSemestres = ObterTodos<AcompanhamentoAlunoSemestre>();
            acompanhamentoAlunoSemestres.ShouldNotBeNull();
            acompanhamentoAlunoSemestres.FirstOrDefault()?.Semestre.ShouldBe(SEGUNDO_SEMESTRE);
            acompanhamentoAlunoSemestres.FirstOrDefault()?.PercursoIndividual.ShouldBe(TEXTO_PADRAO_PERCURSO_INDIVIDUAL);
            acompanhamentoAlunoSemestres.FirstOrDefault()?.AcompanhamentoAlunoId.ShouldBe(acompanhamentoAluno.FirstOrDefault().Id);
        }
        
        
        [Fact(DisplayName = "Relatorio Acompanhamento Aprendizagem - Deve registrar o percurso individual  no período de fechamento (após o término do bimestre)")]
        public async Task Deve_registrar_o_percurso_individual_em_periodo_fechamento_pos_termino_bimestre()
        {
            await CriarDadosBasicos(abrirPeriodos:false);

            await CriarPeriodoEscolarCustomizadoQuartoBimestre();

            await CriarPeriodoAberturaCustomizadoQuartoBimestre();
            
            var salvarAcompanhamentoAlunoUseCase = ObterServicoSalvarAcompanhamentoAlunoUseCase();
                
            var acompanhamentoAlunoDto = new AcompanhamentoAlunoDto { 
                TurmaId = TURMA_ID_1, 
                Semestre = SEGUNDO_SEMESTRE, 
                TextoSugerido = true,
                PercursoIndividual = TEXTO_PADRAO_PERCURSO_INDIVIDUAL,
                AlunoCodigo = ALUNO_CODIGO_1
            };
            
            var retorno = await salvarAcompanhamentoAlunoUseCase.Executar(acompanhamentoAlunoDto);
            retorno.ShouldNotBeNull();
            retorno.AcompanhamentoAlunoId.ShouldBe(1);
            retorno.AcompanhamentoAlunoSemestreId.ShouldBe(1);
            retorno.Auditoria.ShouldNotBeNull();
            retorno.Auditoria.Id.ShouldBe(1);
            
            var acompanhamentoAluno = ObterTodos<AcompanhamentoAluno>();
            acompanhamentoAluno.ShouldNotBeNull();
            acompanhamentoAluno.FirstOrDefault()?.TurmaId.ShouldBe(TURMA_ID_1);
            acompanhamentoAluno.FirstOrDefault()?.AlunoCodigo.ShouldBe(ALUNO_CODIGO_1);
            
            var acompanhamentoAlunoSemestres = ObterTodos<AcompanhamentoAlunoSemestre>();
            acompanhamentoAlunoSemestres.ShouldNotBeNull();
            acompanhamentoAlunoSemestres.FirstOrDefault()?.Semestre.ShouldBe(SEGUNDO_SEMESTRE);
            acompanhamentoAlunoSemestres.FirstOrDefault()?.PercursoIndividual.ShouldBe(TEXTO_PADRAO_PERCURSO_INDIVIDUAL);
            acompanhamentoAlunoSemestres.FirstOrDefault()?.AcompanhamentoAlunoId.ShouldBe(acompanhamentoAluno.FirstOrDefault()!.Id);
        }
        
        
        [Fact(DisplayName = "Relatorio Acompanhamento Aprendizagem - Não deve registrar o percurso individual fora do período de fechamento (após o término do bimestre)")]
        public async Task Nao_deve_registrar_o_percurso_individual_fora_periodo_fechamento_pos_termino_bimestre()
        {
            await CriarDadosBasicos(abrirPeriodos:false);

            await CriarPeriodoEscolarCustomizadoQuartoBimestre();
            
            await CriarPeriodoAberturaCustomizadoQuartoBimestre(false);

            var salvarAcompanhamentoAlunoUseCase = ObterServicoSalvarAcompanhamentoAlunoUseCase();
                
            var acompanhamentoAlunoDto = new AcompanhamentoAlunoDto { 
                TurmaId = TURMA_ID_1, 
                Semestre = SEGUNDO_SEMESTRE, 
                TextoSugerido = true,
                PercursoIndividual = TEXTO_PADRAO_PERCURSO_INDIVIDUAL,
                AlunoCodigo = ALUNO_CODIGO_1
            };
            
            await Assert.ThrowsAsync<NegocioException>(() => salvarAcompanhamentoAlunoUseCase.Executar(acompanhamentoAlunoDto));
        }
        
        
        [Fact(DisplayName = "Relatorio Acompanhamento Aprendizagem - Não deve registrar o percurso individual sem período de fechamento (após o término do bimestre)")]
        public async Task Nao_deve_registrar_o_percurso_individual_sem_periodo_fechamento_pos_termino_bimestre()
        {
            await CriarDadosBasicos(abrirPeriodos:false);

            await CriarPeriodoEscolarCustomizadoQuartoBimestre();

            var salvarAcompanhamentoAlunoUseCase = ObterServicoSalvarAcompanhamentoAlunoUseCase();
                
            var acompanhamentoAlunoDto = new AcompanhamentoAlunoDto { 
                TurmaId = TURMA_ID_1, 
                Semestre = SEGUNDO_SEMESTRE, 
                TextoSugerido = true,
                PercursoIndividual = TEXTO_PADRAO_PERCURSO_INDIVIDUAL,
                AlunoCodigo = ALUNO_CODIGO_1
            };
            
            await Assert.ThrowsAsync<NegocioException>(() => salvarAcompanhamentoAlunoUseCase.Executar(acompanhamentoAlunoDto));
        }
        
        
        [Fact(DisplayName = "Relatorio Acompanhamento Aprendizagem - Registrar o percurso individual  para semestre e ano anterior com reabertura")]
        public async Task Deve_registrar_o_percurso_individual_para_semestre_ano_anterior_com_reabertura()
        {
            await CriarDadosBasicos(abrirPeriodos:false);
            
            await CriarTurma(Modalidade.EducacaoInfantil, "1", "2", TipoTurma.Regular, 1, DateTimeExtension.HorarioBrasilia().AddYears(-1).Year,true);

            await CriarPeriodoEscolarCustomizadoQuartoBimestre();

            await CriarPeriodoReabertura(TIPO_CALENDARIO_1);
            
            var salvarAcompanhamentoAlunoUseCase = ObterServicoSalvarAcompanhamentoAlunoUseCase();
                
            var acompanhamentoAlunoDto = new AcompanhamentoAlunoDto { 
                TurmaId = TURMA_ID_1, 
                Semestre = SEGUNDO_SEMESTRE, 
                TextoSugerido = true,
                PercursoIndividual = TEXTO_PADRAO_PERCURSO_INDIVIDUAL,
                AlunoCodigo = ALUNO_CODIGO_1
            };
            
            var retorno = await salvarAcompanhamentoAlunoUseCase.Executar(acompanhamentoAlunoDto);
            retorno.ShouldNotBeNull();
            retorno.AcompanhamentoAlunoId.ShouldBe(1);
            retorno.AcompanhamentoAlunoSemestreId.ShouldBe(1);
            retorno.Auditoria.ShouldNotBeNull();
            retorno.Auditoria.Id.ShouldBe(1);
            
            var acompanhamentoAluno = ObterTodos<AcompanhamentoAluno>();
            acompanhamentoAluno.ShouldNotBeNull();
            acompanhamentoAluno.FirstOrDefault()?.TurmaId.ShouldBe(TURMA_ID_1);
            acompanhamentoAluno.FirstOrDefault()?.AlunoCodigo.ShouldBe(ALUNO_CODIGO_1);
            
            var acompanhamentoAlunoSemestres = ObterTodos<AcompanhamentoAlunoSemestre>();
            acompanhamentoAlunoSemestres.ShouldNotBeNull();
            acompanhamentoAlunoSemestres.FirstOrDefault()?.Semestre.ShouldBe(SEGUNDO_SEMESTRE);
            acompanhamentoAlunoSemestres.FirstOrDefault()?.PercursoIndividual.ShouldBe(TEXTO_PADRAO_PERCURSO_INDIVIDUAL);
            acompanhamentoAlunoSemestres.FirstOrDefault()?.AcompanhamentoAlunoId.ShouldBe(acompanhamentoAluno.FirstOrDefault()!.Id);
        }
        
        
        [Fact(DisplayName = "Relatorio Acompanhamento Aprendizagem - Não deve registrar o percurso individual  para semestre e ano anterior sem reabertura")]
        public async Task Nao_deve_registrar_o_percurso_individual_para_semestre_ano_anterior_sem_reabertura()
        {
            await CriarDadosBasicos(abrirPeriodos:false);
            
            await CriarTurma(Modalidade.EducacaoInfantil, "1", "2", TipoTurma.Regular, 1, DateTimeExtension.HorarioBrasilia().AddYears(-1).Year,true);

            await CriarPeriodoEscolarCustomizadoQuartoBimestre();
            
            var salvarAcompanhamentoAlunoUseCase = ObterServicoSalvarAcompanhamentoAlunoUseCase();
                
            var acompanhamentoAlunoDto = new AcompanhamentoAlunoDto { 
                TurmaId = TURMA_ID_1, 
                Semestre = SEGUNDO_SEMESTRE, 
                TextoSugerido = true,
                PercursoIndividual = TEXTO_PADRAO_PERCURSO_INDIVIDUAL,
                AlunoCodigo = ALUNO_CODIGO_1
            };
            
            await Assert.ThrowsAsync<NegocioException>(() => salvarAcompanhamentoAlunoUseCase.Executar(acompanhamentoAlunoDto));
        }
        
       
        [Fact(DisplayName = "Relatorio Acompanhamento Aprendizagem - Não deve registrar o percurso individual no 1º semestre com 3 imagens")]
        public async Task Nao_deve_registrar_o_percurso_individual_para_primeiro_semestre_com_tres_imagens()
        {
            await CriarDadosBasicos(abrirPeriodos:false);
            
            await CriarPeriodoEscolarCustomizadoSegundoBimestre(true);
            
            var salvarAcompanhamentoAlunoUseCase = ObterServicoSalvarAcompanhamentoAlunoUseCase();
                
            var acompanhamentoAlunoDto = new AcompanhamentoAlunoDto { 
                TurmaId = TURMA_ID_1, 
                Semestre = PRIMEIRO_SEMESTRE, 
                TextoSugerido = true,
                PercursoIndividual = TEXTO_PADRAO_COM_4_IMAGENS,
                AlunoCodigo = ALUNO_CODIGO_1
            };
            
            await Assert.ThrowsAsync<NegocioException>(() => salvarAcompanhamentoAlunoUseCase.Executar(acompanhamentoAlunoDto));
        }
        
        [Fact(DisplayName = "Relatorio Acompanhamento Aprendizagem - Deve registrar o percurso individual no 1º semestre com 2 imagens")]
        public async Task Deve_registrar_o_percurso_individual_para_primeiro_semestre_com_duas_imagens()
        {
            await CriarDadosBasicos(abrirPeriodos:false);
            
            await CriarPeriodoEscolarCustomizadoSegundoBimestre(true);
            
            var salvarAcompanhamentoAlunoUseCase = ObterServicoSalvarAcompanhamentoAlunoUseCase();
                
            var acompanhamentoAlunoDto = new AcompanhamentoAlunoDto { 
                TurmaId = TURMA_ID_1, 
                Semestre = PRIMEIRO_SEMESTRE, 
                TextoSugerido = true,
                PercursoIndividual = TEXTO_PADRAO_COM_2_IMAGENS,
                AlunoCodigo = ALUNO_CODIGO_1
            };
            
            var retorno = await salvarAcompanhamentoAlunoUseCase.Executar(acompanhamentoAlunoDto);
            retorno.ShouldNotBeNull();
            retorno.AcompanhamentoAlunoId.ShouldBe(1);
            retorno.AcompanhamentoAlunoSemestreId.ShouldBe(1);
            retorno.Auditoria.ShouldNotBeNull();
            retorno.Auditoria.Id.ShouldBe(1);
            
            var acompanhamentoAluno = ObterTodos<AcompanhamentoAluno>();
            acompanhamentoAluno.ShouldNotBeNull();
            acompanhamentoAluno.FirstOrDefault()?.TurmaId.ShouldBe(TURMA_ID_1);
            acompanhamentoAluno.FirstOrDefault()?.AlunoCodigo.ShouldBe(ALUNO_CODIGO_1);
            
            var acompanhamentoAlunoSemestres = ObterTodos<AcompanhamentoAlunoSemestre>();
            acompanhamentoAlunoSemestres.ShouldNotBeNull();
            acompanhamentoAlunoSemestres.FirstOrDefault()?.Semestre.ShouldBe(PRIMEIRO_SEMESTRE);
            acompanhamentoAlunoSemestres.FirstOrDefault()?.PercursoIndividual.ShouldBe(TEXTO_PADRAO_COM_2_IMAGENS);
            acompanhamentoAlunoSemestres.FirstOrDefault()?.AcompanhamentoAlunoId.ShouldBe(acompanhamentoAluno.FirstOrDefault()!.Id);
        }
    }
}