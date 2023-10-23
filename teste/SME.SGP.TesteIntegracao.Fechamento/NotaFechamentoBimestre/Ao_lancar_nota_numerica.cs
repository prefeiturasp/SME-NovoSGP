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
using SME.SGP.TesteIntegracao.NotaFechamentoBimestre.ServicosFakes;
using Xunit;
using SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes;

namespace SME.SGP.TesteIntegracao.NotaFechamentoBimestre
{
    public class Ao_lancar_nota_numerica : NotaFechamentoBimestreTesteBase
    {
        private const decimal CINQUENTA_PORCENTO = 50;

        private readonly DateTime DATA_18_04 = new(DateTimeExtension.HorarioBrasilia().Year, 04, 18);

        public Ao_lancar_nota_numerica(CollectionFixture collectionFixture) : base(collectionFixture)
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
                typeof(ObterTodosAlunosNaTurmaQueryHandlerFake), ServiceLifetime.Scoped));

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery, string[]>),
                typeof(ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQueryHandlerFake), ServiceLifetime.Scoped));

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTurmaItinerarioEnsinoMedioQuery, IEnumerable<TurmaItinerarioEnsinoMedioDto>>),
                    typeof(SME.SGP.TesteIntegracao.ServicosFakes.ObterTurmaItinerarioEnsinoMedioQueryHandlerFake), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName = "Fechamento Bimestre - Deve lançar nota numérica pelo Professor Titular em ano atual")]
        public async Task Deve_lancar_nota_para_fundamental()
        {
            var filtroFechamentoNota = await ObterFiltroFechamentoNota(ObterPerfilProfessor(),
                ModalidadeTipoCalendario.FundamentalMedio,
                false,
                Modalidade.Fundamental,
                ANO_7,
                TipoFrequenciaAluno.PorDisciplina,
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString());

            await CriarDadosBase(filtroFechamentoNota);

            var fechamentoNota = await LancarNotasAlunos(COMPONENTE_CURRICULAR_PORTUGUES_ID_138);

            await ExecutarTeste(fechamentoNota);

            var fechamentosNotas = ObterTodos<FechamentoNota>();
            fechamentosNotas.ShouldNotBeNull();
            fechamentosNotas.Count.ShouldBe(4);
            
            var historicoNotas = ObterTodos<HistoricoNota>();
            historicoNotas.Count.ShouldBe(4);
            
            var historicoNotasNotaFechamentos = ObterTodos<HistoricoNotaFechamento>();
            historicoNotasNotaFechamentos.Count.ShouldBe(4);

            historicoNotas.Count(w=> !w.NotaAnterior.HasValue).ShouldBe(4);
        }

        [Fact(DisplayName = "Fechamento Bimestre - Deve lançar nota numérica pelo Professor Titular em ano atual com avaliação")]
        public async Task Deve_lancar_nota_para_fundamental_com_avaliacao()
        {
            var filtroFechamentoNota = await ObterFiltroFechamentoNota(ObterPerfilProfessor(),
                ModalidadeTipoCalendario.FundamentalMedio,
                false,
                Modalidade.Fundamental,
                ANO_7,
                TipoFrequenciaAluno.PorDisciplina,
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString());

            await CriarDadosBase(filtroFechamentoNota);

            await CriarTipoAvaliacao(TipoAvaliacaoCodigo.AvaliacaoBimestral, AVALIACAO_NOME_1);
            await CriarAtividadeAvaliativa(DATA_18_04, TIPO_AVALIACAO_CODIGO_1, AVALIACAO_NOME_1, false, false, filtroFechamentoNota.ProfessorRf);
            await CriarAtividadeAvaliativaDisciplina(ATIVIDADE_AVALIATIVA_1, filtroFechamentoNota.ComponenteCurricular);

            var fechamentoNota = await LancarNotasAlunos(COMPONENTE_CURRICULAR_PORTUGUES_ID_138);

            await ExecutarTeste(fechamentoNota);

            var fechamentosNotas = ObterTodos<FechamentoNota>();
            fechamentosNotas.ShouldNotBeNull();
            fechamentosNotas.Count.ShouldBe(4);
            
            var historicoNotas = ObterTodos<HistoricoNota>();
            historicoNotas.Count.ShouldBe(4);
            
            var historicoNotasNotaFechamentos = ObterTodos<HistoricoNotaFechamento>();
            historicoNotasNotaFechamentos.Count.ShouldBe(4);

            historicoNotas.Count(w=> !w.NotaAnterior.HasValue).ShouldBe(4);
        }

        [Fact(DisplayName = "Fechamento Bimestre - Deve lançar nota numérica pelo Professor Titular em ano atual regência com avaliação")]
        public async Task Deve_lancar_nota_para_fundamental_regencia_com_avaliacao()
        {
            var filtroFechamentoNota = await ObterFiltroFechamentoNota(ObterPerfilProfessor(),
                ModalidadeTipoCalendario.FundamentalMedio,
                false,
                Modalidade.Fundamental,
                ANO_7,
                TipoFrequenciaAluno.PorDisciplina,
                COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105.ToString());

            await CriarDadosBase(filtroFechamentoNota);

            await CriarTipoAvaliacao(TipoAvaliacaoCodigo.AvaliacaoBimestral, AVALIACAO_NOME_1);
            await CriarAtividadeAvaliativa(DATA_18_04, TIPO_AVALIACAO_CODIGO_1, AVALIACAO_NOME_1, true, false, filtroFechamentoNota.ProfessorRf);
            await CriarAtividadeAvaliativaDisciplina(ATIVIDADE_AVALIATIVA_1, filtroFechamentoNota.ComponenteCurricular);

            var fechamentoNota = await LancarNotasAlunos(COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105);

            await ExecutarTeste(fechamentoNota);

            var fechamentosNotas = ObterTodos<FechamentoNota>();
            fechamentosNotas.ShouldNotBeNull();
            fechamentosNotas.Count.ShouldBe(4);
            
            var historicoNotas = ObterTodos<HistoricoNota>();
            historicoNotas.Count.ShouldBe(4);
            
            var historicoNotasNotaFechamentos = ObterTodos<HistoricoNotaFechamento>();
            historicoNotasNotaFechamentos.Count.ShouldBe(4);

            historicoNotas.Count(w=> !w.NotaAnterior.HasValue).ShouldBe(4);
        }

        [Fact(DisplayName = "Fechamento Bimestre - Deve lançar nota numérica pelo Professor Titular em ano atual EM com avaliação")]
        public async Task Deve_lancar_nota_para_medio_com_avaliacao()
        {
            var filtroFechamentoNota = await ObterFiltroFechamentoNota(ObterPerfilProfessor(),
                ModalidadeTipoCalendario.FundamentalMedio,
                false,
                Modalidade.Medio,
                ANO_3,
                TipoFrequenciaAluno.PorDisciplina,
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString());

            await CriarDadosBase(filtroFechamentoNota);

            await CriarTipoAvaliacao(TipoAvaliacaoCodigo.AvaliacaoBimestral, AVALIACAO_NOME_1);
            await CriarAtividadeAvaliativa(DATA_18_04, TIPO_AVALIACAO_CODIGO_1, AVALIACAO_NOME_1, false, false, filtroFechamentoNota.ProfessorRf);
            await CriarAtividadeAvaliativaDisciplina(ATIVIDADE_AVALIATIVA_1, filtroFechamentoNota.ComponenteCurricular);

            var fechamentoNota = await LancarNotasAlunos(COMPONENTE_CURRICULAR_PORTUGUES_ID_138);

            await ExecutarTeste(fechamentoNota);

            var fechamentosNotas = ObterTodos<FechamentoNota>();
            fechamentosNotas.ShouldNotBeNull();
            fechamentosNotas.Count.ShouldBe(4);
            
            var historicoNotas = ObterTodos<HistoricoNota>();
            historicoNotas.Count.ShouldBe(4);
            
            var historicoNotasNotaFechamentos = ObterTodos<HistoricoNotaFechamento>();
            historicoNotasNotaFechamentos.Count.ShouldBe(4);

            historicoNotas.Count(w=> !w.NotaAnterior.HasValue).ShouldBe(4);
        }

        [Fact(DisplayName = "Fechamento Bimestre - Deve lançar nota numérica pelo Professor Titular em ano atual EJA com avaliação")]
        public async Task Deve_lancar_nota_para_eja_com_avaliacao()
        {
            var filtroFechamentoNota = await ObterFiltroFechamentoNota(ObterPerfilProfessor(),
                ModalidadeTipoCalendario.EJA,
                false,
                Modalidade.EJA,
                ANO_3,
                TipoFrequenciaAluno.PorDisciplina,
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString());

            await CriarDadosBase(filtroFechamentoNota);

            await CriarTipoAvaliacao(TipoAvaliacaoCodigo.AvaliacaoBimestral, AVALIACAO_NOME_1);
            await CriarAtividadeAvaliativa(DATA_18_04, TIPO_AVALIACAO_CODIGO_1, AVALIACAO_NOME_1, false, false, filtroFechamentoNota.ProfessorRf);
            await CriarAtividadeAvaliativaDisciplina(ATIVIDADE_AVALIATIVA_1, filtroFechamentoNota.ComponenteCurricular);

            var fechamentoNota = await LancarNotasAlunos(COMPONENTE_CURRICULAR_PORTUGUES_ID_138);

            await ExecutarTeste(fechamentoNota);

            var fechamentosNotas = ObterTodos<FechamentoNota>();
            fechamentosNotas.ShouldNotBeNull();
            fechamentosNotas.Count.ShouldBe(4);
            
            var historicoNotas = ObterTodos<HistoricoNota>();
            historicoNotas.Count.ShouldBe(4);
            
            var historicoNotasNotaFechamentos = ObterTodos<HistoricoNotaFechamento>();
            historicoNotasNotaFechamentos.Count.ShouldBe(4);

            historicoNotas.Count(w=> !w.NotaAnterior.HasValue).ShouldBe(4);
        }

        [Fact(DisplayName = "Fechamento Bimestre - Deve lançar nota numérica pelo Professor Titular em ano atual com mais de 50% alunos abaixo da media")]
        public async Task Deve_lancar_notas_com_mais_de_50_porcento_dos_alunos_abaixo_da_media()
        {
            var filtroFechamentoNota = await ObterFiltroFechamentoNota(ObterPerfilProfessor(),
                ModalidadeTipoCalendario.FundamentalMedio,
                false,
                Modalidade.Fundamental,
                ANO_7,
                TipoFrequenciaAluno.PorDisciplina,
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString());

            await CriarDadosBase(filtroFechamentoNota);

            var fechamentoNota = await LancarNotasAlunos50PorcentoAbaixoDaMedia(COMPONENTE_CURRICULAR_PORTUGUES_ID_138);

            await ExecutarTeste(fechamentoNota);

            var mediaBimestre = ObterMediaBimestre();

            var fechamentosNotas = ObterTodos<FechamentoNota>();

            var qtdeLancamentos = fechamentosNotas.Count;
            var qtdeAlunosAbaixoMedia = fechamentosNotas.Count(c => c.Nota < double.Parse(mediaBimestre.ToString()));

            var percentualAlunosAbaixoMedia = Convert.ToDecimal(qtdeAlunosAbaixoMedia * 100 / qtdeLancamentos);

            percentualAlunosAbaixoMedia.ShouldBeGreaterThanOrEqualTo(CINQUENTA_PORCENTO);
            
            var historicoNotas = ObterTodos<HistoricoNota>();
            historicoNotas.Count.ShouldBe(4);
            
            var historicoNotasNotaFechamentos = ObterTodos<HistoricoNotaFechamento>();
            historicoNotasNotaFechamentos.Count.ShouldBe(4);

            historicoNotas.Count(w=> !w.NotaAnterior.HasValue).ShouldBe(4);
        }

        [Fact(DisplayName = "Fechamento Bimestre - Deve lançar nota numérica pelo Professor Titular em ano atual com mais de 50% alunos abaixo da media regência")]
        public async Task Deve_lancar_notas_com_mais_de_50_porcento_dos_alunos_abaixo_da_media_regencia()
        {
            var filtroFechamentoNota = await ObterFiltroFechamentoNota(ObterPerfilProfessor(),
                ModalidadeTipoCalendario.FundamentalMedio,
                false,
                Modalidade.Fundamental,
                ANO_7,
                TipoFrequenciaAluno.PorDisciplina,
                COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105.ToString());

            await CriarDadosBase(filtroFechamentoNota);

            var fechamentoNota = await LancarNotasAlunos50PorcentoAbaixoDaMedia(COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105);

            await ExecutarTeste(fechamentoNota);

            var mediaBimestre = ObterMediaBimestre();

            var fechamentosNotas = ObterTodos<FechamentoNota>();

            var qtdeLancamentos = fechamentosNotas.Count;
            var qtdeAlunosAbaixoMedia = fechamentosNotas.Count(c => c.Nota < double.Parse(mediaBimestre.ToString()));

            var percentualAlunosAbaixoMedia = Convert.ToDecimal(qtdeAlunosAbaixoMedia * 100 / qtdeLancamentos);

            percentualAlunosAbaixoMedia.ShouldBeGreaterThanOrEqualTo(CINQUENTA_PORCENTO);
            
            var historicoNotas = ObterTodos<HistoricoNota>();
            historicoNotas.Count.ShouldBe(4);
            
            var historicoNotasNotaFechamentos = ObterTodos<HistoricoNotaFechamento>();
            historicoNotasNotaFechamentos.Count.ShouldBe(4);

            historicoNotas.Count(w=> !w.NotaAnterior.HasValue).ShouldBe(4);
        }

        private async Task<IList<FechamentoTurmaDisciplinaDto>> LancarNotasAlunos50PorcentoAbaixoDaMedia(long disciplinaId)
        {
            var alunosCodigos = new string[] { CODIGO_ALUNO_1, CODIGO_ALUNO_2, CODIGO_ALUNO_3, CODIGO_ALUNO_4 };
            var fechamentosNotas = new List<FechamentoNotaDto>();

            var mediaBimestre = ObterMediaBimestre();

            foreach (var alunoCodigo in alunosCodigos)
            {
                Random randomNota = new();

                var nota = 0;

                var notaAbaixoMedia = randomNota.Next(0, mediaBimestre - 1);
                var notaIgualOuAcimaMedia = randomNota.Next(mediaBimestre, 10);

                var qtdeAlunos = alunosCodigos.Length;
                var qtdeAlunosAbaixoMedia = fechamentosNotas.Count(c => c.Nota < double.Parse(mediaBimestre.ToString()));

                var percentualAlunosAbaixoMedia = Convert.ToDecimal(qtdeAlunosAbaixoMedia * 100 / qtdeAlunos);

                nota = percentualAlunosAbaixoMedia < CINQUENTA_PORCENTO ? notaAbaixoMedia : notaIgualOuAcimaMedia;

                var fechamentoNota = new FechamentoNotaDto()
                {
                    CodigoAluno = alunoCodigo,
                    DisciplinaId = disciplinaId,
                    Nota = nota,
                    ConceitoId = null,
                    SinteseId = null,
                    Anotacao = $"Anotação fechamento teste de integração do aluno {alunoCodigo}.",
                    CriadoEm = DateTimeExtension.HorarioBrasilia(),
                    CriadoRf = SISTEMA_CODIGO_RF,
                    CriadoPor = SISTEMA_NOME
                };

                fechamentosNotas.Add(fechamentoNota);
            }

            var fechamentoTurma = new List<FechamentoTurmaDisciplinaDto>()
            {
                new FechamentoTurmaDisciplinaDto()
                {
                    Bimestre = BIMESTRE_3,
                    DisciplinaId = disciplinaId,
                    Justificativa = "Teste",
                    TurmaId = TURMA_CODIGO_1 ,
                    NotaConceitoAlunos = fechamentosNotas
                }
            };

            return await Task.FromResult(fechamentoTurma);
        }

        private int ObterMediaBimestre()
        {
            var parametros = ObterTodos<ParametrosSistema>();
            return short.Parse(parametros.FirstOrDefault(c => c.Tipo == TipoParametroSistema.MediaBimestre && c.Ano == DateTimeExtension.HorarioBrasilia().Year).Valor);
        }

        private static async Task<List<FechamentoTurmaDisciplinaDto>> LancarNotasAlunos(long disciplinaId)
        {
            var alunosCodigos = new string[] { CODIGO_ALUNO_1, CODIGO_ALUNO_2, CODIGO_ALUNO_3, CODIGO_ALUNO_4 };
            var fechamentosNotas = new List<FechamentoNotaDto>();

            foreach (var alunoCodigo in alunosCodigos)
            {
                Random randomNota = new();

                var nota = randomNota.Next(0, 10);

                var fechamentoNota = new FechamentoNotaDto()
                {
                    CodigoAluno = alunoCodigo,
                    DisciplinaId = disciplinaId,
                    Nota = nota,
                    ConceitoId = null,
                    SinteseId = null,
                    Anotacao = $"Anotação fechamento teste de integração do aluno {alunoCodigo}.",
                    CriadoEm = DateTimeExtension.HorarioBrasilia(),
                    CriadoRf = SISTEMA_CODIGO_RF,
                    CriadoPor = SISTEMA_NOME,
                };

                fechamentosNotas.Add(fechamentoNota);
            }

            var fechamentoTurma = new List<FechamentoTurmaDisciplinaDto>()
            {
                new FechamentoTurmaDisciplinaDto()
                {
                    Bimestre = BIMESTRE_3,
                    DisciplinaId = disciplinaId,
                    Justificativa = "teste",
                    TurmaId = TURMA_CODIGO_1 ,
                    NotaConceitoAlunos = fechamentosNotas
                }
            };

            return await Task.FromResult(fechamentoTurma);
        }

        private static async Task<FiltroFechamentoNotaDto> ObterFiltroFechamentoNota(string perfil, ModalidadeTipoCalendario tipoCalendario,
            bool considerarAnoAnterior, Modalidade modalidade, string anoTurma, TipoFrequenciaAluno tipoFrequenciaAluno,
            string componenteCurricular)
        {
            return await Task.FromResult(new FiltroFechamentoNotaDto
            {
                Perfil = perfil,
                TipoCalendario = tipoCalendario,
                ConsiderarAnoAnterior = considerarAnoAnterior,
                Modalidade = modalidade,
                AnoTurma = anoTurma,
                TipoFrequenciaAluno = tipoFrequenciaAluno,
                ProfessorRf = USUARIO_PROFESSOR_LOGIN_2222222,
                ComponenteCurricular = componenteCurricular
            });
        }
    }
}
