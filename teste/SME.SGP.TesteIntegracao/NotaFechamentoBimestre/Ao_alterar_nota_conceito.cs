using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SME.SGP.TesteIntegracao.NotaFechamentoBimestre.ServicosFakes;
using Xunit;
using SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes;

namespace SME.SGP.TesteIntegracao.NotaFechamentoBimestre
{
    public class Ao_alterar_nota_conceito : NotaFechamentoBimestreTesteBase
    {
        public Ao_alterar_nota_conceito(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery, bool>),
                typeof(ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQueryHandlerComPermissaoFake), ServiceLifetime.Scoped));
            
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosPorTurmaEAnoLetivoQuery, IEnumerable<AlunoPorTurmaResposta>>),
                typeof(ObterAlunosPorTurmaEAnoLetivoQueryHandlerFakeValidarAlunos), ServiceLifetime.Scoped));

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTodosAlunosNaTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>),
                typeof(ObterTodosAlunosNaTurmaQueryHandlerAnoAnteriorFake), ServiceLifetime.Scoped));

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesCurricularesEOLPorTurmasCodigoQuery, IEnumerable<ComponenteCurricularDto>>),
                typeof(ObterComponentesCurricularesEOLPorTurmasCodigoQueryHandlerFake), ServiceLifetime.Scoped));            
        }

        [Fact]
        public async Task Deve_alterar_nota_conceito_lancada_professor_titular()
        {
            await ExecuteTesteConceitoAlteracao(ObterPerfilProfessor(), COMPONENTE_CURRICULAR_ARTES_ID_139, false);
        }

        [Fact]
        public async Task Deve_alterar_nota_conceito_lancada_cp()
        {
            await ExecuteTesteConceitoAlteracao(ObterPerfilCP(), COMPONENTE_CURRICULAR_ARTES_ID_139, false);
        }

        [Fact]
        public async Task Deve_alterar_nota_conceito_lancada_diretor()
        {
            await ExecuteTesteConceitoAlteracao(ObterPerfilDiretor(), COMPONENTE_CURRICULAR_ARTES_ID_139, false);
        }

        [Fact]
        public async Task Deve_alterar_nota_conceito_em_turma_do_ano_anterior()
        {
            await CriarDadosBase(ObterFiltroFechamentoNotaDto(ObterPerfilProfessor(), ANO_3, true));

            var comando = ServiceProvider.GetService<IComandosFechamentoTurmaDisciplina>();
            var dto = ObterListaFechamentoTurma(ObterListaDeFechamentoConceito(COMPONENTE_CURRICULAR_ARTES_ID_139), COMPONENTE_CURRICULAR_ARTES_ID_139);
            var retorno = await comando.Salvar(dto);

            dto.FirstOrDefault().Id = retorno.FirstOrDefault().Id;
            dto.FirstOrDefault().NotaConceitoAlunos = new List<FechamentoNotaDto>()
                                                        {
                                                            new FechamentoNotaDto()
                                                            {
                                                                CodigoAluno = CODIGO_ALUNO_1,
                                                                DisciplinaId = COMPONENTE_CURRICULAR_ARTES_ID_139,
                                                                ConceitoId = (int)ConceitoValores.NS
                                                            }
                                                        };
            var retornoAlteracao = await comando.Salvar(dto);
            retornoAlteracao.ShouldNotBeNull();
            var alunoFechamento = ObterTodos<FechamentoAluno>();
            alunoFechamento.ShouldNotBeNull();
            var aluno = alunoFechamento.LastOrDefault(aluno => aluno.AlunoCodigo == CODIGO_ALUNO_1);
            aluno.ShouldNotBeNull();
            var notas = ObterTodos<FechamentoNota>();
            notas.ShouldNotBeNull();
            var nota = notas.FirstOrDefault(nota => nota.FechamentoAlunoId == aluno.Id);
            nota.ShouldNotBeNull();
            var listaAprovacao = ObterTodos<WfAprovacaoNotaFechamento>();
            var aprovacao = listaAprovacao.FirstOrDefault(wf => wf.FechamentoNotaId == nota.Id);
            aprovacao.ShouldNotBeNull();
            aprovacao.ConceitoId.ShouldBe((int)ConceitoValores.NS);

            retornoAlteracao.FirstOrDefault().MensagemConsistencia.ShouldBe(MensagensNegocioLancamentoNota.REGISTRADO_COM_SUCESSO_EM_24_HORAS_SERA_ENVIADO_PARA_APROVACAO);
        }

        protected async Task ExecuteTesteConceitoAlteracao(string perfil, long disciplina, bool consideraAnorAnterior = false)
        {
            await CriarDadosBase(ObterFiltroFechamentoNotaDto(perfil, ANO_3, consideraAnorAnterior));

            var dto = ObterListaFechamentoTurma(ObterListaDeFechamentoConceito(disciplina), disciplina);
            var retorno = await ExecutarTesteComValidacaoNota(dto, TipoNota.Nota);
            var fechamento = dto.FirstOrDefault();

            retorno.ShouldNotBeNull();
            fechamento.Id = retorno.FirstOrDefault().Id;
            fechamento.NotaConceitoAlunos.FirstOrDefault(aluno => aluno.CodigoAluno == CODIGO_ALUNO_1).ConceitoId = (long)ConceitoValores.NS;
            fechamento.NotaConceitoAlunos.FirstOrDefault(aluno => aluno.CodigoAluno == CODIGO_ALUNO_2).ConceitoId = (long)ConceitoValores.S;
            fechamento.NotaConceitoAlunos.FirstOrDefault(aluno => aluno.CodigoAluno == CODIGO_ALUNO_3).ConceitoId = (long)ConceitoValores.S;
            fechamento.NotaConceitoAlunos.FirstOrDefault(aluno => aluno.CodigoAluno == CODIGO_ALUNO_4).ConceitoId = (long)ConceitoValores.P;

            await ExecutarTesteComValidacaoNota(dto, TipoNota.Conceito);
        }

        private List<FechamentoNotaDto> ObterListaDeFechamentoConceito(long disciplina)
        {
            return new List<FechamentoNotaDto>()
            {
                ObterNotaConceito(CODIGO_ALUNO_1, disciplina, (long)ConceitoValores.P),
                ObterNotaConceito(CODIGO_ALUNO_2, disciplina, (long)ConceitoValores.NS),
                ObterNotaConceito(CODIGO_ALUNO_3, disciplina, (long)ConceitoValores.NS),
                ObterNotaConceito(CODIGO_ALUNO_4, disciplina, (long)ConceitoValores.S),
            };
        }
    }
}
