using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;

namespace SME.SGP.TesteIntegracao.RelatorioAcompanhamentoAprendizagem
{
    public class Ao_registrar_percurso_individual : RelatorioAcompanhamentoAprendizagemTesteBase
    {
        public Ao_registrar_percurso_individual(CollectionFixture collectionFixture) : base(collectionFixture)
        {}

        [Fact(DisplayName = "Relatorio Acompanhamento Aprendizagem - Deve registrar o percurso individual no 1º semestre")]
        public async Task Deve_registrar_percurso_individual_para_primeiro_semestre()
        {
            await CriarDadosBasicos(abrirPeriodos:false);
            var salvarAcompanhamentoAlunoUseCase = ObterServicoSalvarAcompanhamentoAlunoUseCase();
                
            var acompanhamentoAlunoDto = new AcompanhamentoAlunoDto { 
                TurmaId = TURMA_ID_1, 
                Semestre = 1, 
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
            acompanhamentoAluno.FirstOrDefault().TurmaId.ShouldBe(TURMA_ID_1);
            acompanhamentoAluno.FirstOrDefault().AlunoCodigo.ShouldBe(ALUNO_CODIGO_1);
            
            var acompanhamentoAlunoSemestres = ObterTodos<AcompanhamentoAlunoSemestre>();
            acompanhamentoAlunoSemestres.ShouldNotBeNull();
            acompanhamentoAlunoSemestres.FirstOrDefault().Semestre.ShouldBe(1);
            acompanhamentoAlunoSemestres.FirstOrDefault().PercursoIndividual.ShouldBe(TEXTO_PADRAO_PERCURSO_INDIVIDUAL);
            acompanhamentoAlunoSemestres.FirstOrDefault().AcompanhamentoAlunoId.ShouldBe(acompanhamentoAluno.FirstOrDefault().Id);
        }

        [Fact(DisplayName = "Relatorio Acompanhamento Aprendizagem - Deve registrar o percurso individual no 2º semestre")]
        public async Task Deve_registrar_percurso_individual_para_segundo_semestre()
        {
            await CriarDadosBasicos(abrirPeriodos:false);
            var salvarAcompanhamentoAlunoUseCase = ObterServicoSalvarAcompanhamentoAlunoUseCase();
                
            var acompanhamentoAlunoDto = new AcompanhamentoAlunoDto { 
                TurmaId = TURMA_ID_1, 
                Semestre = 2, 
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
            acompanhamentoAluno.FirstOrDefault().TurmaId.ShouldBe(TURMA_ID_1);
            acompanhamentoAluno.FirstOrDefault().AlunoCodigo.ShouldBe(ALUNO_CODIGO_1);
            
            var acompanhamentoAlunoSemestres = ObterTodos<AcompanhamentoAlunoSemestre>();
            acompanhamentoAlunoSemestres.ShouldNotBeNull();
            acompanhamentoAlunoSemestres.FirstOrDefault().Semestre.ShouldBe(2);
            acompanhamentoAlunoSemestres.FirstOrDefault().PercursoIndividual.ShouldBe(TEXTO_PADRAO_PERCURSO_INDIVIDUAL);
            acompanhamentoAlunoSemestres.FirstOrDefault().AcompanhamentoAlunoId.ShouldBe(acompanhamentoAluno.FirstOrDefault().Id);
        }
        
        [Fact(DisplayName = "Relatorio Acompanhamento Aprendizagem - Deve registrar o percurso individual  no período de fechamento (após o término do bimestre)")]
        public async Task Deve_registrar_o_percurso_individual_em_periodo_fechamento_pos_termino_bimestre()
        {
            await CriarDadosBasicos(abrirPeriodos:false);
            var salvarAcompanhamentoAlunoUseCase = ObterServicoSalvarAcompanhamentoAlunoUseCase();
                
            var acompanhamentoAlunoDto = new AcompanhamentoAlunoDto { 
                TurmaId = TURMA_ID_1, 
                Semestre = 2, 
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
            acompanhamentoAluno.FirstOrDefault().TurmaId.ShouldBe(TURMA_ID_1);
            acompanhamentoAluno.FirstOrDefault().AlunoCodigo.ShouldBe(ALUNO_CODIGO_1);
            
            var acompanhamentoAlunoSemestres = ObterTodos<AcompanhamentoAlunoSemestre>();
            acompanhamentoAlunoSemestres.ShouldNotBeNull();
            acompanhamentoAlunoSemestres.FirstOrDefault().Semestre.ShouldBe(2);
            acompanhamentoAlunoSemestres.FirstOrDefault().PercursoIndividual.ShouldBe(TEXTO_PADRAO_PERCURSO_INDIVIDUAL);
            acompanhamentoAlunoSemestres.FirstOrDefault().AcompanhamentoAlunoId.ShouldBe(acompanhamentoAluno.FirstOrDefault().Id);
        }
        //
        // [Fact(DisplayName = "Relatorio Acompanhamento Aprendizagem - Registrar o percurso individual  para semestre e ano anterior com reabertura")]
        // public async Task Deve_registrar_o_percurso_individual_para_semestre_ano_anterior_com_reabertura()
        // {
        //     await CriarDadosBasicos();
        //     var useCase = SalvarAcompanhamentoTurmaUseCase();
        //     
        //     var dto = new AcompanhamentoTurmaDto
        //     {
        //         TurmaId = 1,
        //         Semestre = 1,
        //         ApanhadoGeral = $@"<html><body>
        //                                  teste
        //                             <img src='http://www.localhost.com.br/imagem.png'>
        //                             <img src='http://www.localhost.com.br/imagem.png'>
        //                             <img src='http://www.localhost.com.br/imagem.png'>
        //                           <body/><html/>"
        //     };
        //     var ex = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(dto));
        //     ex.ShouldNotBeNull();
        //     
        //     var obterTodos = ObterTodos<AcompanhamentoTurma>();
        //     obterTodos.ShouldNotBeNull();
        //     obterTodos.Count.ShouldBeEquivalentTo(0);
        // }

        
    }
}