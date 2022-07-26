using Shouldly;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.NotaFechamentoFinal.Base;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SGP.Aplicacao;
using SME.SGP.TesteIntegracao.ServicosFakes;
using Xunit;

namespace SME.SGP.TesteIntegracao.NotaFechamentoFinal
{
    public class Ao_lancar_nota_conceito : NotaFechamentoTesteBase
    {
        public Ao_lancar_nota_conceito(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery, bool>), typeof(ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQueryHandlerComPermissaoFake), ServiceLifetime.Scoped));
        }
        
        [Fact]
        public async Task Deve_Lancar_nota_conceito_por_professor_titular_para_componente_diferente_de_regencia()
        {
            await ExecuteTesteConceitoInsercao(ObterPerfilProfessor(), COMPONENTE_CURRICULAR_ARTES_ID_139, Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, false, TipoNota.Conceito);
        }

        [Fact]
        public async Task Deve_Lancar_nota_conceito_por_professor_titular_para_componente_de_regencia_Fundamental() 
        {
            await ExecuteTesteConceitoInsercao(ObterPerfilProfessor(), COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105, Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, true, TipoNota.Conceito);
        }

        [Fact]
        public async Task Deve_Lancar_nota_conceito_por_professor_titular_para_componente_de_regencia_EJA()
        {
            await ExecuteTesteConceitoInsercao(ObterPerfilProfessor(), COMPONENTE_REGENCIA_CLASSE_EJA_BASICA_ID_1114, Modalidade.EJA, ModalidadeTipoCalendario.EJA, true, TipoNota.Conceito);
        }

        [Fact]
        public async Task Deve_Lancar_nota_conceito_cp() 
        {
            await ExecuteTesteConceitoInsercao(ObterPerfilCP(), COMPONENTE_CURRICULAR_ARTES_ID_139, Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, false, TipoNota.Conceito);
        }

        [Fact]
        public async Task Deve_Lancar_nota_conceito_diretor()
        {
            await ExecuteTesteConceitoInsercao(ObterPerfilDiretor(), COMPONENTE_CURRICULAR_ARTES_ID_139, Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, false, TipoNota.Conceito);
        }

        [Fact]
        public async Task Deve_alterar_nota_conceito_lancada_professor_titular()
        {
            await ExecuteTesteConceitoAlteracao(ObterPerfilProfessor(), COMPONENTE_CURRICULAR_ARTES_ID_139, Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, false, TipoNota.Conceito);
        }

        [Fact]
        public async Task Deve_alterar_nota_conceito_lancada_cp()
        {
            await ExecuteTesteConceitoAlteracao(ObterPerfilCP(), COMPONENTE_CURRICULAR_ARTES_ID_139, Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, false, TipoNota.Conceito);
        }

        [Fact]
        public async Task Deve_alterar_nota_conceito_lancada_diretor()
        {
            await ExecuteTesteConceitoAlteracao(ObterPerfilDiretor(), COMPONENTE_CURRICULAR_ARTES_ID_139, Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, false, TipoNota.Conceito);
        }

        [Fact]
        public async Task Deve_alterar_nota_conceito_em_turma_do_ano_anterior()
        {
            await CriarDadosBase(ObterFiltroNotas(ObterPerfilProfessor(), ANO_3, COMPONENTE_CURRICULAR_ARTES_ID_139.ToString(), TipoNota.Conceito, Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, true));

            await ExecutarComandosFechamentoFinal(ObtenhaFechamentoFinalConceitoDto(COMPONENTE_CURRICULAR_ARTES_ID_139, false));

            var dto = new FechamentoFinalSalvarDto()
            {
                DisciplinaId = COMPONENTE_CURRICULAR_ARTES_ID_139.ToString(),
                TurmaCodigo = TURMA_CODIGO_1,
                EhRegencia = false,
                Itens = new List<FechamentoFinalSalvarItemDto>()
                {
                    new FechamentoFinalSalvarItemDto()
                    {
                        AlunoRf = ALUNO_CODIGO_1,
                        ComponenteCurricularCodigo = COMPONENTE_CURRICULAR_ARTES_ID_139,
                        ConceitoId = (int)ConceitoValores.P
                    }
                }
            };

            var retorno = await ExecutarComandosFechamentoFinal(dto);

            var alunoFechamento = ObterTodos<FechamentoAluno>();
            alunoFechamento.ShouldNotBeNull();
            var aluno = alunoFechamento.FirstOrDefault(aluno => aluno.AlunoCodigo == ALUNO_CODIGO_1);
            aluno.ShouldNotBeNull();
            var notas = ObterTodos<FechamentoNota>();
            notas.ShouldNotBeNull();
            var nota = notas.FirstOrDefault(nota => nota.FechamentoAlunoId == aluno.Id);
            nota.ShouldNotBeNull();
            var listaAprovacao = ObterTodos<WfAprovacaoNotaFechamento>();
            var aprovacao = listaAprovacao.FirstOrDefault(wf => wf.FechamentoNotaId == nota.Id);
            aprovacao.ShouldNotBeNull();
            aprovacao.ConceitoId.ShouldBe((int)ConceitoValores.P);

            retorno.MensagemConsistencia.ShouldBe(string.Format(MensagemNegocioFechamentoNota.REGISTRADO_COM_SUCESSO_EM_24_HORAS_SERA_ENVIADO_PARA_APROVACAO, TipoNota.Conceito.Name()));
        }
    }
}
