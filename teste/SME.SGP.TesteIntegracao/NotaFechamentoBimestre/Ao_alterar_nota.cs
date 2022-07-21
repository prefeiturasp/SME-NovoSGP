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
using Xunit;

namespace SME.SGP.TesteIntegracao.NotaFechamentoBimestre
{
    public class Ao_alterar_nota : NotaFechamentoBimestreTesteBase
    {
        public Ao_alterar_nota(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery, bool>), typeof(ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQueryHandlerComPermissaoFake), ServiceLifetime.Scoped));
        }

        [Fact]
        public async Task Deve_alterar_nota_conceito_lancada_professor_titular()
        {
            await ExecuteTesteConceitoAlteracao(ObterPerfilProfessor(), COMPONENTE_CURRICULAR_ARTES_ID_139, false);
        }

        [Fact]
        public async Task Deve_alterar_nota_conceito_lancada_cp()
        {
            await ExecuteTesteConceitoAlteracao(ObterPerfilCP(), COMPONENTE_CURRICULAR_ARTES_ID_139,  false);
        }

        [Fact]
        public async Task Deve_alterar_nota_conceito_lancada_diretor()
        {
            await ExecuteTesteConceitoAlteracao(ObterPerfilDiretor(), COMPONENTE_CURRICULAR_ARTES_ID_139, false);
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
            var comando = ServiceProvider.GetService<IComandosFechamentoTurmaDisciplina>();
            var retorno = await comando.Salvar(dto);
            var fechamento = dto.FirstOrDefault();

            retorno.ShouldNotBeNull();
            fechamento.Id = retorno.FirstOrDefault().Id;
            fechamento.NotaConceitoAlunos.FirstOrDefault(aluno => aluno.CodigoAluno == CODIGO_ALUNO_1).ConceitoId = (long)ConceitoValores.NS;
            fechamento.NotaConceitoAlunos.FirstOrDefault(aluno => aluno.CodigoAluno == CODIGO_ALUNO_2).ConceitoId = (long)ConceitoValores.S;
            fechamento.NotaConceitoAlunos.FirstOrDefault(aluno => aluno.CodigoAluno == CODIGO_ALUNO_3).ConceitoId = (long)ConceitoValores.S;
            fechamento.NotaConceitoAlunos.FirstOrDefault(aluno => aluno.CodigoAluno == CODIGO_ALUNO_4).ConceitoId = (long)ConceitoValores.P;

            await ExecutarTesteComValidacaoNota(dto);
        }

        protected async Task ExecuteTesteNumericoAlteracao(string perfil, long disciplina)
        {
            await CriarDadosBase(ObterFiltroFechamentoNotaDto(perfil, ANO_7));

            var dto = ObterListaFechamentoTurma(ObterListaDeFechamentoNumerica(disciplina), disciplina);
            var comando = ServiceProvider.GetService<IComandosFechamentoTurmaDisciplina>();
            var retorno = await comando.Salvar(dto);
            var fechamento = dto.FirstOrDefault();

            retorno.ShouldNotBeNull();
            fechamento.Id = retorno.FirstOrDefault().Id;
            fechamento.NotaConceitoAlunos.FirstOrDefault(aluno => aluno.CodigoAluno == CODIGO_ALUNO_1).Nota = NOTA_9;
            fechamento.NotaConceitoAlunos.FirstOrDefault(aluno => aluno.CodigoAluno == CODIGO_ALUNO_2).Nota = NOTA_8;
            fechamento.NotaConceitoAlunos.FirstOrDefault(aluno => aluno.CodigoAluno == CODIGO_ALUNO_3).Nota = NOTA_7;
            fechamento.NotaConceitoAlunos.FirstOrDefault(aluno => aluno.CodigoAluno == CODIGO_ALUNO_4).Nota = NOTA_6;

            await ExecutarTesteComValidacaoNota(dto);
        }

        private FiltroFechamentoNotaDto ObterFiltroFechamentoNotaDto(string perfil, string anoTurma, bool consideraAnorAnterior = false)
        {
            return new FiltroFechamentoNotaDto()
            {
                Perfil = perfil,
                AnoTurma = anoTurma,
                ConsiderarAnoAnterior = consideraAnorAnterior,
                Modalidade = Modalidade.Fundamental,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                TipoFrequenciaAluno = TipoFrequenciaAluno.PorDisciplina
            };
        }

        private FechamentoNotaDto ObterFechamentoNotaDto(string codigoAluno, long disciplina)
        {
            return new FechamentoNotaDto()
            {
                Anotacao = "",
                CodigoAluno = codigoAluno,
                DisciplinaId = disciplina,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRf = SISTEMA_CODIGO_RF
            };
        }

        private FechamentoNotaDto ObterNotaConceito(string codigoAluno, long disciplina, long conceitoId)
        {
            var dto = ObterFechamentoNotaDto(codigoAluno, disciplina);
            dto.ConceitoId = conceitoId;

            return dto;
        }

        private FechamentoNotaDto ObterNotaNumerica(string codigoAluno, long disciplina, long nota)
        {
            var dto = ObterFechamentoNotaDto(codigoAluno, disciplina);
            dto.Nota = nota;

            return dto;
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

        private List<FechamentoTurmaDisciplinaDto> ObterListaFechamentoTurma(List<FechamentoNotaDto> listaDeNota, long disciplina)
        {
            return new List<FechamentoTurmaDisciplinaDto>()
            {
                new FechamentoTurmaDisciplinaDto()
                {
                    Bimestre = BIMESTRE_1,
                    DisciplinaId = disciplina,
                    Justificativa = "" ,
                    TurmaId = TURMA_CODIGO_1 ,
                    NotaConceitoAlunos = listaDeNota
                }
            };
        }
    }
}
