using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.CompensacaoDeAusencia.ServicosFake;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.CompensacaoDeAusencia.Base
{
    public abstract class Ao_lancar_compensacao_de_ausencia_base : CompensacaoDeAusenciaTesteBase
    {
        private const string DESCRICAO_ALTERADA = "OUTRA DESCRICAO";

        protected Ao_lancar_compensacao_de_ausencia_base(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected async Task ExecuteInserirCompensasaoAusenciaSemAulasSelecionadas(CompensacaoDeAusenciaDBDto dtoDadoBase)
        {
            await CriarDadosBase(dtoDadoBase);
            await CriaFrequenciaAlunos(dtoDadoBase);
            await CriaRegistroDeFrequencia();

            var dto = ObtenhaCompensacaoAusenciaDto(dtoDadoBase, ObtenhaListaDeAlunosSemAulasSelecionadas());

            var casoDeUso = ServiceProvider.GetService<ISalvarCompensacaoAusenciaUseCase>();
            await casoDeUso.Executar(0, dto);

            var listaDeCompensacaoAusencia = ObterTodos<CompensacaoAusencia>();
            listaDeCompensacaoAusencia.ShouldNotBeNull();
            var compensacao = listaDeCompensacaoAusencia.FirstOrDefault();
            compensacao.ShouldNotBeNull();

            var listaDeCompensacaoAusenciaAluno = ObterTodos<CompensacaoAusenciaAluno>();
            listaDeCompensacaoAusenciaAluno.ShouldNotBeNull();
            var listaDaCompensacaoAluno = listaDeCompensacaoAusenciaAluno.FindAll(aluno => aluno.CompensacaoAusenciaId == compensacao.Id);
            listaDaCompensacaoAluno.ShouldNotBeNull();

            var listaDeCompensacaoAusenciaAlunoAula = ObterTodos<CompensacaoAusenciaAlunoAula>();
            listaDeCompensacaoAusenciaAlunoAula.ShouldNotBeNull();
            var listaDaCompensacaoAusenciaAula = listaDeCompensacaoAusenciaAlunoAula.FindAll(aula => listaDaCompensacaoAluno.Any(x => x.Id == aula.CompensacaoAusenciaAlunoId));
            listaDaCompensacaoAusenciaAula.ShouldNotBeNull();

            TesteCompensacaoAluno(listaDaCompensacaoAluno, listaDaCompensacaoAusenciaAula, CODIGO_ALUNO_1, QUANTIDADE_AULA);
            TesteCompensacaoAluno(listaDaCompensacaoAluno, listaDaCompensacaoAusenciaAula, CODIGO_ALUNO_2, QUANTIDADE_AULA_2);
            TesteCompensacaoAluno(listaDaCompensacaoAluno, listaDaCompensacaoAusenciaAula, CODIGO_ALUNO_3, QUANTIDADE_AULA);
            TesteCompensacaoAluno(listaDaCompensacaoAluno, listaDaCompensacaoAusenciaAula, CODIGO_ALUNO_4, QUANTIDADE_AULA);
        }

        protected async Task ExecuteInserirCompensasaoAusenciaComAulasSelecionadas(CompensacaoDeAusenciaDBDto dtoDadoBase)
        {
            await CriarDadosBase(dtoDadoBase);
            await CriaFrequenciaAlunos(dtoDadoBase);
            await CriaRegistroDeFrequencia();

            var alunos = ObtenhaListaDeAlunosComAulasSelecionadas();

            var dto = ObtenhaCompensacaoAusenciaDto(dtoDadoBase, alunos);

            var casoDeUso = ServiceProvider.GetService<ISalvarCompensacaoAusenciaUseCase>();
            await casoDeUso.Executar(0, dto);

            var listaDeCompensacaoAusencia = ObterTodos<CompensacaoAusencia>();
            listaDeCompensacaoAusencia.ShouldNotBeNull();
            var compensacao = listaDeCompensacaoAusencia.FirstOrDefault();
            compensacao.ShouldNotBeNull();

            var listaDeCompensacaoAusenciaAluno = ObterTodos<CompensacaoAusenciaAluno>();
            listaDeCompensacaoAusenciaAluno.ShouldNotBeNull();
            var listaDaCompensacaoAluno = listaDeCompensacaoAusenciaAluno.FindAll(aluno => aluno.CompensacaoAusenciaId == compensacao.Id);
            listaDaCompensacaoAluno.ShouldNotBeNull();

            var listaDeCompensacaoAusenciaAlunoAula = ObterTodos<CompensacaoAusenciaAlunoAula>();
            listaDeCompensacaoAusenciaAlunoAula.ShouldNotBeNull();
            var listaDaCompensacaoAusenciaAula = listaDeCompensacaoAusenciaAlunoAula.FindAll(aula => listaDaCompensacaoAluno.Any(x => x.Id == aula.CompensacaoAusenciaAlunoId));
            listaDaCompensacaoAusenciaAula.ShouldNotBeNull();

            TesteCompensacaoAluno(listaDaCompensacaoAluno, listaDaCompensacaoAusenciaAula, CODIGO_ALUNO_1, QUANTIDADE_AULA);
            TesteCompensacaoAluno(listaDaCompensacaoAluno, listaDaCompensacaoAusenciaAula, CODIGO_ALUNO_2, QUANTIDADE_AULA_2);
            TesteCompensacaoAluno(listaDaCompensacaoAluno, listaDaCompensacaoAusenciaAula, CODIGO_ALUNO_3, QUANTIDADE_AULA);
            TesteCompensacaoAluno(listaDaCompensacaoAluno, listaDaCompensacaoAusenciaAula, CODIGO_ALUNO_4, QUANTIDADE_AULA);

            TesteCompensacaoAlunoAula(listaDeCompensacaoAusenciaAluno, listaDeCompensacaoAusenciaAlunoAula, alunos, CODIGO_ALUNO_1);
            TesteCompensacaoAlunoAula(listaDeCompensacaoAusenciaAluno, listaDeCompensacaoAusenciaAlunoAula, alunos, CODIGO_ALUNO_2);
            TesteCompensacaoAlunoAula(listaDeCompensacaoAusenciaAluno, listaDeCompensacaoAusenciaAlunoAula, alunos, CODIGO_ALUNO_3);
            TesteCompensacaoAlunoAula(listaDeCompensacaoAusenciaAluno, listaDeCompensacaoAusenciaAlunoAula, alunos, CODIGO_ALUNO_4);
        }

        protected async Task ExecutarTesteRemoverAlunoCompensacao(CompensacaoDeAusenciaDBDto dtoDadoBase)
        {
            await CriarDadosBase(dtoDadoBase);
            await CriaFrequenciaAlunos(dtoDadoBase);
            await CriaCompensacaoAusencia(dtoDadoBase);
            await CriaRegistroDeFrequencia();
            await CriaCompensacaoAusenciaAluno();

            var compensacaoAusenciaAlunos = ObterTodos<CompensacaoAusenciaAluno>();
            var registroFrequenciaAlunos = ObterTodos<RegistroFrequenciaAluno>();
            await CriarCompensacaoAusenciaAlunoAula(compensacaoAusenciaAlunos, registroFrequenciaAlunos);

            var comando = ServiceProvider.GetService<IExcluirCompensacaoAusenciaUseCase>();
            var listaIds = new long[] { COMPENSACAO_AUSENCIA_ID_1 };

            await comando.Executar(listaIds);

            var compensacaoAusenciaAlunosExcluidos = ObterTodos<CompensacaoAusenciaAluno>();
            compensacaoAusenciaAlunosExcluidos.ForEach(x => x.Excluido.ShouldBeTrue());
        }

        protected async Task ExecuteAlterarCompensacaoAusenciaSemAulasSelecionadas(CompensacaoDeAusenciaDBDto dtoDadoBase)
        {
            await CriarDadosBase(dtoDadoBase);
            await CriaFrequenciaAlunos(dtoDadoBase);
            await CriaCompensacaoAusencia(dtoDadoBase);
            await CriaRegistroDeFrequencia();
            await CriaCompensacaoAusenciaAluno();

            var compensacaoAusenciaAlunos = ObterTodos<CompensacaoAusenciaAluno>();
            var registroFrequenciaAlunos = ObterTodos<RegistroFrequenciaAluno>();
            await CriarCompensacaoAusenciaAlunoAula(compensacaoAusenciaAlunos, registroFrequenciaAlunos);

            var compensacaoAusencias = ObterTodos<CompensacaoAusencia>();

            var compensacaoAusenciaAlterada = compensacaoAusencias.FirstOrDefault();
            compensacaoAusenciaAlterada.Alunos = compensacaoAusenciaAlunos.Where(t => t.CompensacaoAusenciaId == compensacaoAusenciaAlterada.Id);
            compensacaoAusenciaAlterada.Alunos.FirstOrDefault(t => t.CodigoAluno == CODIGO_ALUNO_2).QuantidadeFaltasCompensadas = 2;

            var compensacaoAusenciaAlunoDto = new List<CompensacaoAusenciaAlunoDto>()
            { 
                new CompensacaoAusenciaAlunoDto()
                {
                    Id = compensacaoAusenciaAlterada.Alunos.FirstOrDefault(t => t.CodigoAluno == CODIGO_ALUNO_2).Id.ToString(),
                    QtdFaltasCompensadas = compensacaoAusenciaAlterada.Alunos.FirstOrDefault(t => t.CodigoAluno == CODIGO_ALUNO_2).QuantidadeFaltasCompensadas
                }
            };

            var dtoAlterado = new CompensacaoAusenciaDto()
            {
                Alunos = compensacaoAusenciaAlunoDto,
                Bimestre = compensacaoAusenciaAlterada.Bimestre,
                Descricao = DESCRICAO_ALTERADA,
                Atividade = compensacaoAusenciaAlterada.Nome,
                DisciplinaId = compensacaoAusenciaAlterada.DisciplinaId,
                Id = compensacaoAusenciaAlterada.Id,
                TurmaId = compensacaoAusenciaAlterada.TurmaId.ToString()
            };

            var casoDeUso = ServiceProvider.GetService<ISalvarCompensacaoAusenciaUseCase>();
            await casoDeUso.Executar(compensacaoAusenciaAlterada.Id, dtoAlterado);

            var listaDeCompensacaoAusencia = ObterTodos<CompensacaoAusencia>();
            listaDeCompensacaoAusencia.ShouldNotBeNull();
            var compensacao = listaDeCompensacaoAusencia.FirstOrDefault();
            compensacao.ShouldNotBeNull();

            var listaDeCompensacaoAusenciaAluno = ObterTodos<CompensacaoAusenciaAluno>();
            listaDeCompensacaoAusenciaAluno.ShouldNotBeNull();
            var listaDaCompensacaoAluno = listaDeCompensacaoAusenciaAluno.FindAll(aluno => aluno.CompensacaoAusenciaId == compensacao.Id && !aluno.Excluido);
            listaDaCompensacaoAluno.ShouldNotBeNull();

            var listaDeCompensacaoAusenciaAlunoAula = ObterTodos<CompensacaoAusenciaAlunoAula>();
            listaDeCompensacaoAusenciaAlunoAula.ShouldNotBeNull();
            var listaDaCompensacaoAusenciaAula = listaDeCompensacaoAusenciaAlunoAula.FindAll(aula => listaDaCompensacaoAluno.Any(x => x.Id == aula.CompensacaoAusenciaAlunoId) && !aula.Excluido);
            listaDaCompensacaoAusenciaAula.ShouldNotBeNull();

            TesteCompensacaoAluno(listaDaCompensacaoAluno, listaDaCompensacaoAusenciaAula, CODIGO_ALUNO_2, 2);

            listaDeCompensacaoAusencia.ToList().Exists(x => x.Descricao == DESCRICAO_ALTERADA).ShouldBeTrue();
        }

        protected async Task ExecuteAlterarCompensacaoAusenciaComAulasSelecionadas(CompensacaoDeAusenciaDBDto dtoDadoBase)
        {
            await CriarDadosBase(dtoDadoBase);
            await CriaFrequenciaAlunos(dtoDadoBase);
            await CriaCompensacaoAusencia(dtoDadoBase);
            await CriaRegistroDeFrequencia();
            await CriaCompensacaoAusenciaAluno();

            var compensacaoAusenciaAlunos = ObterTodos<CompensacaoAusenciaAluno>();
            var registroFrequenciaAlunos = ObterTodos<RegistroFrequenciaAluno>();
            await CriarCompensacaoAusenciaAlunoAula(compensacaoAusenciaAlunos, registroFrequenciaAlunos);

            var compensacaoAusencias = ObterTodos<CompensacaoAusencia>();

            var compensacaoAusenciaAlterada = compensacaoAusencias.FirstOrDefault();
            compensacaoAusenciaAlterada.Alunos = compensacaoAusenciaAlunos.Where(t => t.CompensacaoAusenciaId == compensacaoAusenciaAlterada.Id);
            compensacaoAusenciaAlterada.Alunos.FirstOrDefault(t => t.CodigoAluno == CODIGO_ALUNO_2).QuantidadeFaltasCompensadas = 2;

            var compensacaoAusenciaAlunoDto = new List<CompensacaoAusenciaAlunoDto>()
            {
                new CompensacaoAusenciaAlunoDto()
                {
                    Id = compensacaoAusenciaAlterada.Alunos.FirstOrDefault(t => t.CodigoAluno == CODIGO_ALUNO_2).Id.ToString(),
                    QtdFaltasCompensadas = compensacaoAusenciaAlterada.Alunos.FirstOrDefault(t => t.CodigoAluno == CODIGO_ALUNO_2).QuantidadeFaltasCompensadas,
                    CompensacaoAusenciaAlunoAula = new List<CompensacaoAusenciaAlunoAulaDto> 
                    {
                         new CompensacaoAusenciaAlunoAulaDto()
                        {
                            RegistroFrequenciaAlunoId = registroFrequenciaAlunos.LastOrDefault(t => t.CodigoAluno == CODIGO_ALUNO_2 && t.Valor == (int)TipoFrequencia.F).Id
                        }
                    }
                }
            };

            var dtoAlterado = new CompensacaoAusenciaDto()
            {
                Alunos = compensacaoAusenciaAlunoDto,
                Bimestre = compensacaoAusenciaAlterada.Bimestre,
                Descricao = compensacaoAusenciaAlterada.Descricao,
                Atividade = compensacaoAusenciaAlterada.Nome,
                DisciplinaId = compensacaoAusenciaAlterada.DisciplinaId,
                Id = compensacaoAusenciaAlterada.Id,
                TurmaId = compensacaoAusenciaAlterada.TurmaId.ToString()
            };

            var casoDeUso = ServiceProvider.GetService<ISalvarCompensacaoAusenciaUseCase>();
            await casoDeUso.Executar(compensacaoAusenciaAlterada.Id, dtoAlterado);

            var listaDeCompensacaoAusencia = ObterTodos<CompensacaoAusencia>();
            listaDeCompensacaoAusencia.ShouldNotBeNull();
            var compensacao = listaDeCompensacaoAusencia.FirstOrDefault();
            compensacao.ShouldNotBeNull();

            var listaDeCompensacaoAusenciaAluno = ObterTodos<CompensacaoAusenciaAluno>();
            listaDeCompensacaoAusenciaAluno.ShouldNotBeNull();
            var listaDaCompensacaoAluno = listaDeCompensacaoAusenciaAluno.FindAll(aluno => aluno.CompensacaoAusenciaId == compensacao.Id && !aluno.Excluido);
            listaDaCompensacaoAluno.ShouldNotBeNull();

            var listaDeCompensacaoAusenciaAlunoAula = ObterTodos<CompensacaoAusenciaAlunoAula>();
            listaDeCompensacaoAusenciaAlunoAula.ShouldNotBeNull();
            var listaDaCompensacaoAusenciaAula = listaDeCompensacaoAusenciaAlunoAula.FindAll(aula => listaDaCompensacaoAluno.Any(x => x.Id == aula.CompensacaoAusenciaAlunoId) && !aula.Excluido);
            listaDaCompensacaoAusenciaAula.ShouldNotBeNull();

            TesteCompensacaoAluno(listaDaCompensacaoAluno, listaDaCompensacaoAusenciaAula, CODIGO_ALUNO_2, 2);

            TesteCompensacaoAlunoAula(listaDeCompensacaoAusenciaAluno, listaDeCompensacaoAusenciaAlunoAula, compensacaoAusenciaAlunoDto, CODIGO_ALUNO_2);
        }




        private async Task CriaCompensacaoAusenciaAluno()
        {
            await CriaCompensacaoAusenciaAluno(
                    CODIGO_ALUNO_1,
                    QUANTIDADE_AULA);

            await CriaCompensacaoAusenciaAluno(
                    CODIGO_ALUNO_2,
                    QUANTIDADE_AULA_3);

            await CriaCompensacaoAusenciaAluno(
                    CODIGO_ALUNO_3,
                    QUANTIDADE_AULA_2);

            await CriaCompensacaoAusenciaAluno(
                    CODIGO_ALUNO_4,
                    QUANTIDADE_AULA);
        }

        private static void TesteCompensacaoAlunoAula(List<CompensacaoAusenciaAluno> compensacaoAusenciaAlunos, List<CompensacaoAusenciaAlunoAula> compensacaoAusenciaAlunoAulas, List<CompensacaoAusenciaAlunoDto> alunos, string codigoAluno)
        {
            var registroFrequenciaAlunoId = alunos.FirstOrDefault(t => t.Id == codigoAluno).CompensacaoAusenciaAlunoAula.FirstOrDefault().RegistroFrequenciaAlunoId;
            registroFrequenciaAlunoId.ShouldNotBe(0);

            compensacaoAusenciaAlunoAulas.Any(t =>
            compensacaoAusenciaAlunos.Any(x => x.Id == t.CompensacaoAusenciaAlunoId && x.CodigoAluno == codigoAluno) &&
            t.RegistroFrequenciaAlunoId == registroFrequenciaAlunoId).ShouldBeTrue();
        }

        protected void TesteDisciplinasRegentes()
        {
            var listaCompencaoRegencia = ObterTodos<CompensacaoAusenciaDisciplinaRegencia>();
            listaCompencaoRegencia.ShouldNotBeNull();
            var listaIdDisciplinas = listaCompencaoRegencia.Select(regencia => regencia.DisciplinaId).ToList();
            listaIdDisciplinas.Except(ObtenhaListaDeRegencia()).Count().ShouldBe(0);
        }

        protected async Task RegistroFrequenciaAlunoPorQuantidade(string codigoAluno, int quantidadeAula, TipoFrequencia valorFrequencia)
        {
            for (int i = 1; i <= quantidadeAula; i++)
            {
                await InserirNaBase(new RegistroFrequenciaAluno
                {
                    CodigoAluno = codigoAluno,
                    RegistroFrequenciaId = REGISTRO_FREQUENCIA_ID_1,
                    Valor = (int)valorFrequencia,
                    NumeroAula = i,
                    AulaId = AULA_ID,
                    CriadoEm = DateTimeExtension.HorarioBrasilia(),
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF
                });
            }
        }


        private List<string> ObtenhaListaDeRegencia()
        {
            return new List<string>
            {
                COMPONENTE_CIENCIAS_ID_89,
                COMPONENTE_GEOGRAFIA_ID_8,
                COMPONENTE_HISTORIA_ID_7,
                COMPONENTE_MATEMATICA_ID_2
            };
        }

        protected CompensacaoDeAusenciaDBDto ObtenhaDtoDadoBase(
            string perfil, 
            string componente, 
            Modalidade modalidade = Modalidade.Fundamental,
            ModalidadeTipoCalendario tipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
            long tipoCalendarioId = TIPO_CALENDARIO_1,
            string anoTurma = ANO_5,
            int QuantidadeAula = QUANTIDADE_AULA_4
            )
        {
            return new CompensacaoDeAusenciaDBDto()
            {
                Perfil = perfil,
                Modalidade = modalidade,
                TipoCalendario = tipoCalendario,
                Bimestre = BIMESTRE_3,
                ComponenteCurricular = componente,
                TipoCalendarioId = tipoCalendarioId,
                AnoTurma = anoTurma,
                DataReferencia = DATA_25_07_INICIO_BIMESTRE_3,
                QuantidadeAula = QuantidadeAula
            };
        }

        protected CompensacaoDeAusenciaDBDto ObtenhaDtoDadoBase(
                        string componente,
                        Modalidade modalidade,
                        ModalidadeTipoCalendario modalidadeTipo,
                        string ano)
        {
            return new CompensacaoDeAusenciaDBDto()
            {
                Perfil = ObterPerfilProfessor(),
                Modalidade = modalidade,
                TipoCalendario = modalidadeTipo,
                Bimestre = BIMESTRE_1,
                ComponenteCurricular = componente,
                TipoCalendarioId = TIPO_CALENDARIO_1,
                AnoTurma = ano,
                DataReferencia = DATA_03_01_INICIO_BIMESTRE_1,
                QuantidadeAula = QUANTIDADE_AULA_4
            };
        }

        private void TesteCompensacaoAluno(List<CompensacaoAusenciaAluno> listaDaCompensacaoAluno, List<CompensacaoAusenciaAlunoAula> compensacaoAusenciaAlunoAulas, string codigoAluno, int quantidade)
        {
            var compensacaoAluno = listaDaCompensacaoAluno.Find(aluno => aluno.CodigoAluno == codigoAluno);
            compensacaoAluno.ShouldNotBeNull();
            compensacaoAluno.QuantidadeFaltasCompensadas.ShouldBe(quantidade);

            var compensacaoAusenciaAlunoAula = compensacaoAusenciaAlunoAulas.FindAll(aula => aula.CompensacaoAusenciaAlunoId == compensacaoAluno.Id);
            compensacaoAusenciaAlunoAula.Count.ShouldBe(quantidade);
        }

        private async Task CriaFrequenciaAlunos(CompensacaoDeAusenciaDBDto dtoDadoBase)
        {
            await CriaFrequenciaAlunos(
                dtoDadoBase,
                CODIGO_ALUNO_1,
                QUANTIDADE_AULA_3,
                QUANTIDADE_AULA);

            await CriaFrequenciaAlunos(
                dtoDadoBase,
                CODIGO_ALUNO_2,
                QUANTIDADE_AULA,
                QUANTIDADE_AULA_3);

            await CriaFrequenciaAlunos(
                dtoDadoBase,
                CODIGO_ALUNO_3,
                QUANTIDADE_AULA_2,
                QUANTIDADE_AULA_2);

            await CriaFrequenciaAlunos(
                dtoDadoBase,
                CODIGO_ALUNO_4,
                QUANTIDADE_AULA_3,
                QUANTIDADE_AULA);
        }

        private async Task CriaFrequenciaAlunos(
                        CompensacaoDeAusenciaDBDto dtoDadoBase,
                        string codigoAluno,
                        int totalPresenca,
                        int totalAusencia)
        {
            await CriaFrequenciaAluno(
                dtoDadoBase,
                DATA_03_01_INICIO_BIMESTRE_1,
                DATA_01_05_FIM_BIMESTRE_1,
                codigoAluno,
                totalPresenca,
                totalAusencia,
                PERIODO_ESCOLAR_ID_3);
        }

        protected List<CompensacaoAusenciaAlunoDto> ObtenhaListaDeAlunosSemAulasSelecionadas()
        {
            return new List<CompensacaoAusenciaAlunoDto>()
            {
                new CompensacaoAusenciaAlunoDto()
                {
                    Id = CODIGO_ALUNO_1,
                    QtdFaltasCompensadas = QUANTIDADE_AULA
                },
                new CompensacaoAusenciaAlunoDto()
                {
                    Id = CODIGO_ALUNO_2,
                    QtdFaltasCompensadas = QUANTIDADE_AULA_2
                },
                new CompensacaoAusenciaAlunoDto()
                {
                    Id = CODIGO_ALUNO_3,
                    QtdFaltasCompensadas = QUANTIDADE_AULA
                },
                new CompensacaoAusenciaAlunoDto()
                {
                    Id = CODIGO_ALUNO_4,
                    QtdFaltasCompensadas = QUANTIDADE_AULA
                }
            };
        }

        protected List<CompensacaoAusenciaAlunoDto> ObtenhaListaDeAlunosComAulasSelecionadas()
        {
            var registroFrequenciaAlunos = ObterTodos<RegistroFrequenciaAluno>();

            return new List<CompensacaoAusenciaAlunoDto>()
            {
                new CompensacaoAusenciaAlunoDto()
                {
                    Id = CODIGO_ALUNO_1,
                    QtdFaltasCompensadas = QUANTIDADE_AULA,
                    CompensacaoAusenciaAlunoAula = new List<CompensacaoAusenciaAlunoAulaDto>()
                    {
                        new CompensacaoAusenciaAlunoAulaDto()
                        {
                            RegistroFrequenciaAlunoId = registroFrequenciaAlunos.FirstOrDefault(t => t.CodigoAluno == CODIGO_ALUNO_1 && t.Valor == (int)TipoFrequencia.F).Id
                        }
                    }
                },
                new CompensacaoAusenciaAlunoDto()
                {
                    Id = CODIGO_ALUNO_2,
                    QtdFaltasCompensadas = QUANTIDADE_AULA_2,
                    CompensacaoAusenciaAlunoAula = new List<CompensacaoAusenciaAlunoAulaDto>()
                    {
                        new CompensacaoAusenciaAlunoAulaDto()
                        {
                            RegistroFrequenciaAlunoId = registroFrequenciaAlunos.FirstOrDefault(t => t.CodigoAluno == CODIGO_ALUNO_2 && t.Valor == (int)TipoFrequencia.F).Id
                        }
                    }
                },
                new CompensacaoAusenciaAlunoDto()
                {
                    Id = CODIGO_ALUNO_3,
                    QtdFaltasCompensadas = QUANTIDADE_AULA,
                    CompensacaoAusenciaAlunoAula = new List<CompensacaoAusenciaAlunoAulaDto>()
                    {
                        new CompensacaoAusenciaAlunoAulaDto()
                        {
                            RegistroFrequenciaAlunoId = registroFrequenciaAlunos.FirstOrDefault(t => t.CodigoAluno == CODIGO_ALUNO_3 && t.Valor == (int)TipoFrequencia.F).Id
                        }
                    }
                },
                new CompensacaoAusenciaAlunoDto()
                {
                    Id = CODIGO_ALUNO_4,
                    QtdFaltasCompensadas = QUANTIDADE_AULA,
                    CompensacaoAusenciaAlunoAula = new List<CompensacaoAusenciaAlunoAulaDto>()
                    {
                        new CompensacaoAusenciaAlunoAulaDto()
                        {
                            RegistroFrequenciaAlunoId = registroFrequenciaAlunos.FirstOrDefault(t => t.CodigoAluno == CODIGO_ALUNO_4 && t.Valor == (int)TipoFrequencia.F).Id
                        }
                    }
                }
            };
        }

        private async Task CriaRegistroDeFrequencia()
        {
            await CrieRegistroDeFrenquencia();

            await RegistroFrequenciaAlunoPorQuantidade(CODIGO_ALUNO_1, QUANTIDADE_AULA_3, TipoFrequencia.C);
            await RegistroFrequenciaAlunoPorQuantidade(CODIGO_ALUNO_1, QUANTIDADE_AULA, TipoFrequencia.F);

            await RegistroFrequenciaAlunoPorQuantidade(CODIGO_ALUNO_2, QUANTIDADE_AULA, TipoFrequencia.C);
            await RegistroFrequenciaAlunoPorQuantidade(CODIGO_ALUNO_2, QUANTIDADE_AULA_3, TipoFrequencia.F);

            await RegistroFrequenciaAlunoPorQuantidade(CODIGO_ALUNO_3, QUANTIDADE_AULA_2, TipoFrequencia.C);
            await RegistroFrequenciaAlunoPorQuantidade(CODIGO_ALUNO_3, QUANTIDADE_AULA_2, TipoFrequencia.F);

            await RegistroFrequenciaAlunoPorQuantidade(CODIGO_ALUNO_4, QUANTIDADE_AULA_3, TipoFrequencia.C);
            await RegistroFrequenciaAlunoPorQuantidade(CODIGO_ALUNO_4, QUANTIDADE_AULA, TipoFrequencia.F);
        }
    }
}
