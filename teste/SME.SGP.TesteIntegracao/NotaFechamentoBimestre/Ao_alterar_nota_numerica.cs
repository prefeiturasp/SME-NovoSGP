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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SME.SGP.TesteIntegracao.NotaFechamentoBimestre.ServicosFakes;
using Xunit;
using SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes;

namespace SME.SGP.TesteIntegracao.NotaFechamentoBimestre
{
    public class Ao_alterar_nota_numerica : NotaFechamentoBimestreTesteBase
    {
        public Ao_alterar_nota_numerica(CollectionFixture collectionFixture) : base(collectionFixture)
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
        public async Task Deve_alterar_nota_numerico_lancada_professor_titular()
        {
            await ExecuteTesteNumericoAlteracao(ObterPerfilProfessor(), COMPONENTE_CURRICULAR_PORTUGUES_ID_138);
        }

        [Fact]
        public async Task Deve_alterar_nota_numerico_lancada_cp()
        {
            await ExecuteTesteNumericoAlteracao(ObterPerfilCP(), COMPONENTE_CURRICULAR_PORTUGUES_ID_138);
        }

        [Fact]
        public async Task Deve_alterar_nota_numerico_lancada_diretor()
        {
            await ExecuteTesteNumericoAlteracao(ObterPerfilDiretor(), COMPONENTE_CURRICULAR_PORTUGUES_ID_138);
        }

        [Fact]
        public async Task Deve_alterar_nota_numerico_em_turma_do_ano_anterior()
        {
            await CriarDadosBase(ObterFiltroFechamentoNotaDto(ObterPerfilProfessor(), ANO_3, true));
            
            var comando = ServiceProvider.GetService<IComandosFechamentoTurmaDisciplina>();
            var dto = ObterListaFechamentoTurma(ObterListaDeFechamentoNumerica(COMPONENTE_CURRICULAR_PORTUGUES_ID_138), COMPONENTE_CURRICULAR_PORTUGUES_ID_138);
            var retorno = await comando.Salvar(dto);

            dto.FirstOrDefault().Id = retorno.FirstOrDefault().Id;
            dto.FirstOrDefault().NotaConceitoAlunos = new List<FechamentoNotaDto>()
                                                        {
                                                            new FechamentoNotaDto()
                                                            {
                                                                CodigoAluno = CODIGO_ALUNO_1,
                                                                DisciplinaId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                                                                Nota = (long)NOTA_8
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
            aprovacao.Nota.ShouldBe((long)NOTA_8);

            retornoAlteracao.FirstOrDefault().MensagemConsistencia.ShouldBe(MensagensNegocioLancamentoNota.REGISTRADO_COM_SUCESSO_EM_24_HORAS_SERA_ENVIADO_PARA_APROVACAO);
        }

        protected async Task ExecuteTesteNumericoAlteracao(string perfil, long disciplina)
        {
            await CriarDadosBase(ObterFiltroFechamentoNotaDto(perfil, ANO_7));

            var dto = ObterListaFechamentoTurma(ObterListaDeFechamentoNumerica(disciplina), disciplina);
            var retorno = await ExecutarTesteComValidacaoNota(dto, TipoNota.Nota);
            var fechamento = dto.FirstOrDefault();

            retorno.ShouldNotBeNull();
            fechamento.Id = retorno.FirstOrDefault().Id;
            fechamento.NotaConceitoAlunos.FirstOrDefault(aluno => aluno.CodigoAluno == CODIGO_ALUNO_1).Nota = NOTA_9;
            fechamento.NotaConceitoAlunos.FirstOrDefault(aluno => aluno.CodigoAluno == CODIGO_ALUNO_2).Nota = NOTA_8;
            fechamento.NotaConceitoAlunos.FirstOrDefault(aluno => aluno.CodigoAluno == CODIGO_ALUNO_3).Nota = NOTA_7;
            fechamento.NotaConceitoAlunos.FirstOrDefault(aluno => aluno.CodigoAluno == CODIGO_ALUNO_4).Nota = NOTA_6;

            await ExecutarTesteComValidacaoNota(dto, TipoNota.Nota);
        }

        private List<FechamentoNotaDto> ObterListaDeFechamentoNumerica(long disciplina)
        {
            return new List<FechamentoNotaDto>()
            {
                ObterNotaNumerica(CODIGO_ALUNO_1, disciplina, (long)NOTA_6),
                ObterNotaNumerica(CODIGO_ALUNO_2, disciplina, (long)NOTA_7),
                ObterNotaNumerica(CODIGO_ALUNO_3, disciplina, (long)NOTA_8),
                ObterNotaNumerica(CODIGO_ALUNO_4, disciplina, (long)NOTA_9),
            };
        }
    }
}
