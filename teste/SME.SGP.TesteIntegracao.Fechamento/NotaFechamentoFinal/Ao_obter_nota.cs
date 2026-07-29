using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.TesteIntegracao.Fechamento.NotaFechamentoFinal.ServicosFakes;
using SME.SGP.TesteIntegracao.NotaFechamento.ServicosFakes;
using SME.SGP.TesteIntegracao.NotaFechamentoFinal.Base;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.ServicosFakes.Query;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.NotaFechamentoFinal
{
    public class Ao_obter_nota : NotaFechamentoTesteBase
    {
        private const int REGISTRO_FREQUENCIA_ID = 1;
        private const double PERCENTUAL_ALUNO_1 = 87.5;
        private const double PERCENTUAL_ALUNO_2 = 75;
        private const int TOTAL_FALTA = 3;
        private const int TOTAL_AUSENCIA_1 = 2;
        private const int TOTAL_AUSENCIA_2 = 1;
        private const int VALOR_FREQUENCIA_3 = 3;
        private const int VALOR_FREQUENCIA_2 = 2;
        private readonly Dictionary<string, Dictionary<int, double>> dicionarioNotaNumericaBimestres;
        private readonly Dictionary<string, Dictionary<int, int>> dicionarioNotaConceitoAluno;
        private readonly Dictionary<string, Dictionary<int, int>> dicionarioNotaRegenteAluno;
        public Ao_obter_nota(CollectionFixture collectionFixture) : base(collectionFixture)
        {
            dicionarioNotaNumericaBimestres = ObtenhaDicionarioNotaNumericaAluno();
            dicionarioNotaConceitoAluno = ObtenhaDicionarioNotaConceitoAluno();
            dicionarioNotaRegenteAluno = ObtenhaDicionarioNotaRegenteAluno();
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<PodePersistirTurmaDisciplinaQuery, bool>), typeof(PodePersistirTurmaDisciplinaQueryHandlerFakeRetornaTrue), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosPorTurmaEAnoLetivoQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterAlunosPorTurmaEAnoLetivoQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterDadosTurmaEolPorCodigoQuery, DadosTurmaEolDto>), typeof(ObterDadosTurmaEolPorCodigoQueryHandlerFakeRegular), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterValorParametroSistemaTipoEAnoQuery, string>), typeof(ObterValorParametroSistemaTipoEAnoQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesRegenciaPorAnoEolQuery, IEnumerable<ComponenteCurricularEol>>), typeof(ObterComponentesRegenciaPorAnoQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterPeriodoFechamentoPorCalendarioIdEBimestreQuery, PeriodoFechamentoBimestre>), typeof(ObterPeriodoFechamentoPorCalendarioIdEBimestreQueryHandlerFake), ServiceLifetime.Scoped));
        }

        [Fact]
        public async Task Deve_obter_nota_numerica_falta_ausencia_notaFinal_e_frenquencia()
        {
            var filtroNotaFechamento = ObterFiltroNotas(
                    ObterPerfilProfessor(),
                    ANO_7,
                    COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                    TipoNota.Nota,
                    Modalidade.Fundamental,
                    ModalidadeTipoCalendario.FundamentalMedio,
                    false);
            await CriarDadosBase(filtroNotaFechamento);
            await CriaFechamentoNotaNumerica();
            await CriaAula(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString());
            await CriaRegistroDeFrenquencia();
            await CriaRegistroDeFrequenciaAluno(CODIGO_ALUNO_1, VALOR_FREQUENCIA_3);
            await CriaRegistroDeFrequenciaAluno(CODIGO_ALUNO_2, VALOR_FREQUENCIA_2);

            var consulta = ServiceProvider.GetService<IConsultasFechamentoFinal>();
            var dto = new FechamentoFinalConsultaFiltroDto()
            {
                DisciplinaCodigo = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                TurmaCodigo = TURMA_CODIGO_1
            };
            var retorno = await consulta.ObterFechamentos(dto);

            ValideNotaNumericaAlunos(retorno, CODIGO_ALUNO_1, PERCENTUAL_ALUNO_1, TOTAL_FALTA, TOTAL_AUSENCIA_1, NOTA_8.ToString());
            ValideNotaNumericaAlunos(retorno, CODIGO_ALUNO_2, PERCENTUAL_ALUNO_2, TOTAL_FALTA, TOTAL_AUSENCIA_2, NOTA_9.ToString());
        }

        [Fact]
        public async Task Deve_obter_nota_conceito_falta_ausencia_notaFinal_e_frenquencia()
        {
            var filtroNotaFechamento = ObterFiltroNotas(
                            ObterPerfilProfessor(),
                            ANO_3,
                            COMPONENTE_CURRICULAR_ARTES_ID_139.ToString(),
                            TipoNota.Conceito,
                            Modalidade.Fundamental,
                            ModalidadeTipoCalendario.FundamentalMedio,
                            false);
            await CriarDadosBase(filtroNotaFechamento);
            await CriaFechamentoNotaConceito();
            await CriaAula(COMPONENTE_CURRICULAR_ARTES_ID_139.ToString());
            await CriaRegistroDeFrenquencia();
            await CriaRegistroDeFrequenciaAluno(CODIGO_ALUNO_1, VALOR_FREQUENCIA_3);
            await CriaRegistroDeFrequenciaAluno(CODIGO_ALUNO_2, VALOR_FREQUENCIA_2);

            var consulta = ServiceProvider.GetService<IConsultasFechamentoFinal>();
            var dto = new FechamentoFinalConsultaFiltroDto()
            {
                DisciplinaCodigo = COMPONENTE_CURRICULAR_ARTES_ID_139,
                TurmaCodigo = TURMA_CODIGO_1
            };
            var retorno = await consulta.ObterFechamentos(dto);

            ValideNotaConceitoAlunos(retorno, COMPONENTE_CURRICULAR_ARTES_ID_139, CODIGO_ALUNO_1, PERCENTUAL_ALUNO_1, TOTAL_FALTA, TOTAL_AUSENCIA_1, (int)ConceitoValores.P);
            ValideNotaConceitoAlunos(retorno, COMPONENTE_CURRICULAR_ARTES_ID_139, CODIGO_ALUNO_2, PERCENTUAL_ALUNO_2, TOTAL_FALTA, TOTAL_AUSENCIA_2, (int)ConceitoValores.S);
        }

        [Fact]
        public async Task Deve_obter_nota_regencia_classe_falta_ausencia_notaFinal_e_frenquencia()
        {
            var filtroNotaFechamento = ObterFiltroNotas(
                    ObterPerfilProfessor(),
                    ANO_3,
                    COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105.ToString(),
                    TipoNota.Conceito,
                    Modalidade.Fundamental,
                    ModalidadeTipoCalendario.FundamentalMedio,
                    false);
            await CriarDadosBase(filtroNotaFechamento);
            await CriaFechamentoNotaConceitoRegencia();
            await CriaAula(COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105.ToString());
            await CriaRegistroDeFrenquencia();
            await CriaRegistroDeFrequenciaAluno(CODIGO_ALUNO_1, VALOR_FREQUENCIA_3);

            var consulta = ServiceProvider.GetService<IConsultasFechamentoFinal>();
            var dto = new FechamentoFinalConsultaFiltroDto()
            {
                DisciplinaCodigo = COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105,
                TurmaCodigo = TURMA_CODIGO_1,
                EhRegencia = true
            };
            var retorno = await consulta.ObterFechamentos(dto);

            ValideNotaRegenteAlunos(retorno, COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105, CODIGO_ALUNO_1, PERCENTUAL_ALUNO_1, TOTAL_FALTA, TOTAL_AUSENCIA_1, (int)ConceitoValores.P);
        }

        [Fact]
        public async Task Deve_retornar_nenhuma_nota_ao_ter_somente_fechamento_turma_disciplina_sem_fechamento_nota()
        {
            var filtroNotaFechamento = ObterFiltroNotas(
                            ObterPerfilProfessor(),
                            ANO_3,
                            COMPONENTE_CURRICULAR_ARTES_ID_139.ToString(),
                            TipoNota.Conceito,
                            Modalidade.Fundamental,
                            ModalidadeTipoCalendario.FundamentalMedio,
                            false);
            await CriarDadosBase(filtroNotaFechamento);
            await CriaFechamentoBimestre(COMPONENTE_CURRICULAR_ARTES_ID_139);
            await CriaFechamentoFinal(COMPONENTE_CURRICULAR_ARTES_ID_139);
            await CriaAula(COMPONENTE_CURRICULAR_ARTES_ID_139.ToString());
            await CriaRegistroDeFrenquencia();
            await CriaRegistroDeFrequenciaAluno(CODIGO_ALUNO_1, VALOR_FREQUENCIA_3);

            var consulta = ServiceProvider.GetService<IConsultasFechamentoFinal>();
            var dto = new FechamentoFinalConsultaFiltroDto()
            {
                DisciplinaCodigo = COMPONENTE_CURRICULAR_ARTES_ID_139,
                TurmaCodigo = TURMA_CODIGO_1,
                EhRegencia = true
            };

            var retorno = await consulta.ObterFechamentos(dto);

            retorno.Alunos.ShouldNotBeNull();
            retorno.Alunos.Any(a => a.Codigo == CODIGO_ALUNO_1).ShouldBeTrue();
            retorno.Alunos.FirstOrDefault(a => a.Codigo == CODIGO_ALUNO_1).NotasConceitoFinal.Any(n => !string.IsNullOrEmpty(n.NotaConceito)).ShouldBeFalse();
        }

        [Fact]
        public async Task Deve_obter_nota_final_nao_excluida()
        {
            var turmas = new string[] { TURMA_CODIGO_1 };

            var filtroNotaFechamento = ObterFiltroNotas(
                           ObterPerfilProfessor(),
                           ANO_1,
                           COMPONENTE_CURRICULAR_ARTES_ID_139.ToString(),
                           TipoNota.Conceito,
                           Modalidade.EJA,
                           ModalidadeTipoCalendario.EJA,
                           false);
            await CriarDadosBase(filtroNotaFechamento);
            await CriaFechamentoTurma_Disciplina(null, COMPONENTE_CURRICULAR_ARTES_ID_139, FECHAMENTO_TURMA_ID_1);
            await CriaFechamentoTurma_Disciplina(null, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, FECHAMENTO_TURMA_ID_1);
            await CriaFechamentoAluno(FECHAMENTO_TURMA_DISCIPLINA_ID_1, CODIGO_ALUNO_1);
            await CriaFechamentoAluno(FECHAMENTO_TURMA_DISCIPLINA_ID_2, CODIGO_ALUNO_1);
            await CriaFechamentoNota(FECHAMENTO_ALUNO_ID_1, COMPONENTE_CURRICULAR_ARTES_ID_139,(int)ConceitoValores.P);
            await CriaFechamentoNota(FECHAMENTO_ALUNO_ID_2, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, (int)ConceitoValores.NS,true);

            var consulta = ServiceProvider.GetService<IRepositorioFechamentoNotaConsulta>();
            var retorno = await consulta.ObterNotasFinaisAlunoAsync(turmas, CODIGO_ALUNO_1);

            var valor = retorno.Any(x => x.ComponenteCurricularCodigo == COMPONENTE_CURRICULAR_PORTUGUES_ID_138);
            Assert.False(valor);
        }

        [Fact]
        public async Task Deve_obter_nota_bimestre_nao_excluida()
        {
            var turmas = new string[] { TURMA_CODIGO_1 };

            var filtroNotaFechamento = ObterFiltroNotas(
                           ObterPerfilProfessor(),
                           ANO_1,
                           COMPONENTE_CURRICULAR_ARTES_ID_139.ToString(),
                           TipoNota.Conceito,
                           Modalidade.EJA,
                           ModalidadeTipoCalendario.EJA,
                           false);
            await CriarDadosBase(filtroNotaFechamento);
            await CriaFechamentoTurma_Disciplina(PERIODO_ESCOLAR_CODIGO_1, COMPONENTE_CURRICULAR_ARTES_ID_139, FECHAMENTO_TURMA_ID_1);
            await CriaFechamentoTurma_Disciplina(PERIODO_ESCOLAR_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, FECHAMENTO_TURMA_ID_1);
            await CriaFechamentoAluno(FECHAMENTO_TURMA_DISCIPLINA_ID_1, CODIGO_ALUNO_1);
            await CriaFechamentoAluno(FECHAMENTO_TURMA_DISCIPLINA_ID_2, CODIGO_ALUNO_1);
            await CriaFechamentoNota(FECHAMENTO_ALUNO_ID_1, COMPONENTE_CURRICULAR_ARTES_ID_139, (int)ConceitoValores.P);
            await CriaFechamentoNota(FECHAMENTO_ALUNO_ID_2, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, (int)ConceitoValores.NS, true);


            var consulta = ServiceProvider.GetService<IRepositorioFechamentoTurmaDisciplinaConsulta>();
            var retorno = await consulta.ObterFechamentosTurmasCodigosEBimestreEAlunoCodigoAsync(turmas, BIMESTRE_1, CODIGO_ALUNO_1);

            var valor = retorno.Any(x => x.ComponenteCurricularId == COMPONENTE_CURRICULAR_PORTUGUES_ID_138);
            Assert.False(valor);
        }

        [Fact]
        public async Task Deve_obter_nota_fechamento_final_por_turma_nao_excluida()
        {
            var turmas = new string[] { TURMA_CODIGO_1 };

            var filtroNotaFechamento = ObterFiltroNotas(
                           ObterPerfilProfessor(),
                           ANO_1,
                           COMPONENTE_CURRICULAR_ARTES_ID_139.ToString(),
                           TipoNota.Conceito,
                           Modalidade.EJA,
                           ModalidadeTipoCalendario.EJA,
                           false);
            await CriarDadosBase(filtroNotaFechamento);
            await CriaFechamentoTurma_Disciplina(null, COMPONENTE_CURRICULAR_ARTES_ID_139, FECHAMENTO_TURMA_ID_1);
            await CriaFechamentoTurma_Disciplina(null, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, FECHAMENTO_TURMA_ID_1);
            await CriaFechamentoAluno(FECHAMENTO_TURMA_DISCIPLINA_ID_1, CODIGO_ALUNO_1);
            await CriaFechamentoAluno(FECHAMENTO_TURMA_DISCIPLINA_ID_2, CODIGO_ALUNO_1);
            await CriaFechamentoNota(FECHAMENTO_ALUNO_ID_1, COMPONENTE_CURRICULAR_ARTES_ID_139, (int)ConceitoValores.P);
            await CriaFechamentoNota(FECHAMENTO_ALUNO_ID_2, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, (int)ConceitoValores.NS, true);

            var consulta = ServiceProvider.GetService<IRepositorioFechamentoNota>();
            var retorno = await consulta.ObterPorFechamentosTurma(new long[] { FECHAMENTO_TURMA_ID_1 } );

            var valor = retorno.Any(x => x.ComponenteCurricularId == COMPONENTE_CURRICULAR_PORTUGUES_ID_138);
            Assert.False(valor);
        }

        [Fact]
        public async Task Deve_obter_nota_final_fechamento_por_turma_nao_excluida()
        {
            var turmas = new string[] { TURMA_CODIGO_1 };

            var filtroNotaFechamento = ObterFiltroNotas(
                           ObterPerfilProfessor(),
                           ANO_1,
                           COMPONENTE_CURRICULAR_ARTES_ID_139.ToString(),
                           TipoNota.Conceito,
                           Modalidade.EJA,
                           ModalidadeTipoCalendario.EJA,
                           false);
            await CriarDadosBase(filtroNotaFechamento);
            await CriaFechamentoTurma_Disciplina(PERIODO_ESCOLAR_CODIGO_1, COMPONENTE_CURRICULAR_ARTES_ID_139, FECHAMENTO_TURMA_ID_1);
            await CriaFechamentoTurma_Disciplina(PERIODO_ESCOLAR_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, FECHAMENTO_TURMA_ID_1);
            await CriaFechamentoAluno(FECHAMENTO_TURMA_DISCIPLINA_ID_1, CODIGO_ALUNO_1);
            await CriaFechamentoAluno(FECHAMENTO_TURMA_DISCIPLINA_ID_2, CODIGO_ALUNO_1);
            await CriaFechamentoNota(FECHAMENTO_ALUNO_ID_1, COMPONENTE_CURRICULAR_ARTES_ID_139, (int)ConceitoValores.P);
            await CriaFechamentoNota(FECHAMENTO_ALUNO_ID_2, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, (int)ConceitoValores.NS, true);

            var consulta = ServiceProvider.GetService<IRepositorioFechamentoNota>();
            var retorno = await consulta.ObterPorFechamentosTurma(new long[] { FECHAMENTO_TURMA_ID_1 });

            var valor = retorno.Any(x => x.ComponenteCurricularId == COMPONENTE_CURRICULAR_PORTUGUES_ID_138);
            Assert.False(valor);
        }



        private async Task CriaAula(string componente)
        {
            await CriarAula(
                    DATA_INICIO_BIMESTRE_4,
                    RecorrenciaAula.AulaUnica,
                    TipoAula.Normal,
                    USUARIO_PROFESSOR_CODIGO_RF_2222222,
                    TURMA_CODIGO_1,
                    UE_CODIGO_1,
                    componente,
                    TIPO_CALENDARIO_1);
        }

        private void ValideNotaNumericaAlunos(
                                FechamentoFinalConsultaRetornoDto dto,
                                string codigoAluno,
                                double percentual,
                                int totalFalta,
                                int totalAusenciasCompensadas,
                                string notaFinal)
        {
            var listaDeNotasBimestres = ValideNotaAlunosRetornandoBimentres(dto, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, codigoAluno, percentual, totalFalta, totalAusenciasCompensadas, notaFinal);

            foreach (var notaBimestre in listaDeNotasBimestres)
            {
                if (dicionarioNotaNumericaBimestres.ContainsKey(codigoAluno))
                    notaBimestre.NotaConceito.ShouldBe(dicionarioNotaNumericaBimestres[codigoAluno][notaBimestre.Bimestre].ToString());
            }
        }

        private void ValideNotaConceitoAlunos(
                        FechamentoFinalConsultaRetornoDto dto,
                        long disciplinaCodidgo,
                        string codigoAluno,
                        double percentual,
                        int totalFalta,
                        int totalAusenciasCompensadas,
                        int notaFinal)
        {
            var listaDeNotasBimestres = ValideNotaAlunosRetornandoBimentres(dto, disciplinaCodidgo, codigoAluno, percentual, totalFalta, totalAusenciasCompensadas, notaFinal.ToString());

            foreach (var notaBimestre in listaDeNotasBimestres)
            {
                if (dicionarioNotaConceitoAluno.ContainsKey(codigoAluno))
                    notaBimestre.NotaConceito.ShouldBe(dicionarioNotaConceitoAluno[codigoAluno][notaBimestre.Bimestre].ToString());
            }
        }

        private void ValideNotaRegenteAlunos(
                FechamentoFinalConsultaRetornoDto dto,
                long disciplinaCodidgo,
                string codigoAluno,
                double percentual,
                int totalFalta,
                int totalAusenciasCompensadas,
                int notaFinal)
        {
            var listaDeNotasBimestres = ValideNotaAlunosRetornandoBimentres(dto, disciplinaCodidgo, codigoAluno, percentual, totalFalta, totalAusenciasCompensadas, notaFinal.ToString());

            foreach (var notaBimestre in listaDeNotasBimestres)
            {
                if (dicionarioNotaRegenteAluno.ContainsKey(notaBimestre.DisciplinaCodigo.ToString()))
                    notaBimestre.NotaConceito.ShouldBe(dicionarioNotaRegenteAluno[notaBimestre.DisciplinaCodigo.ToString()][notaBimestre.Bimestre].ToString());
            }
        }

        private IList<FechamentoFinalConsultaRetornoAlunoNotaConceitoDto> ValideNotaAlunosRetornandoBimentres(
                                                    FechamentoFinalConsultaRetornoDto dto,
                                                    long disciplinaCodidgo,
                                                    string codigoAluno,
                                                    double percentual,
                                                    int totalFalta,
                                                    int totalAusenciasCompensadas,
                                                    string notaFinal)
        {
            dto.ShouldNotBeNull();
            dto.Alunos.ShouldNotBeNull();
            var aluno = dto.Alunos.FirstOrDefault(aluno => aluno.Codigo == codigoAluno);
            aluno.ShouldNotBeNull();
            aluno.FrequenciaValor.ShouldBe(percentual);
            aluno.TotalFaltas.ShouldBe(totalFalta);
            aluno.TotalAusenciasCompensadas.ShouldBe(totalAusenciasCompensadas);
            aluno.NotasConceitoFinal.ShouldNotBeNull();
            var notaConceitoFinal = aluno.NotasConceitoFinal.FirstOrDefault(final => final.DisciplinaCodigo == disciplinaCodidgo);
            notaConceitoFinal.ShouldNotBeNull();
            notaConceitoFinal.NotaConceito.ShouldBe(notaFinal);
            aluno.NotasConceitoBimestre.ShouldNotBeNull();

            return aluno.NotasConceitoBimestre;
        }

        private Dictionary<string, Dictionary<int, double>> ObtenhaDicionarioNotaNumericaAluno()
        {
            var dicionario = new Dictionary<string, Dictionary<int, double>>();

            dicionario.Add(CODIGO_ALUNO_1, new Dictionary<int, double>());
            dicionario.Add(CODIGO_ALUNO_2, new Dictionary<int, double>());

            dicionario[CODIGO_ALUNO_1].Add(BIMESTRE_1, NOTA_7);
            dicionario[CODIGO_ALUNO_1].Add(BIMESTRE_2, NOTA_8);
            dicionario[CODIGO_ALUNO_1].Add(BIMESTRE_3, NOTA_8);
            dicionario[CODIGO_ALUNO_1].Add(BIMESTRE_4, NOTA_9);
            dicionario[CODIGO_ALUNO_1].Add(BIMESTRE_FINAL, NOTA_8);

            dicionario[CODIGO_ALUNO_2].Add(BIMESTRE_1, NOTA_7);
            dicionario[CODIGO_ALUNO_2].Add(BIMESTRE_2, NOTA_9);
            dicionario[CODIGO_ALUNO_2].Add(BIMESTRE_3, NOTA_8);
            dicionario[CODIGO_ALUNO_2].Add(BIMESTRE_4, NOTA_10);
            dicionario[CODIGO_ALUNO_2].Add(BIMESTRE_FINAL, NOTA_9);

            return dicionario;
        }

        private Dictionary<string, Dictionary<int, int>> ObtenhaDicionarioNotaConceitoAluno()
        {
            var dicionario = new Dictionary<string, Dictionary<int, int>>();

            dicionario.Add(CODIGO_ALUNO_1, new Dictionary<int, int>());
            dicionario.Add(CODIGO_ALUNO_2, new Dictionary<int, int>());

            dicionario[CODIGO_ALUNO_1].Add(BIMESTRE_1, (int)ConceitoValores.P);
            dicionario[CODIGO_ALUNO_1].Add(BIMESTRE_2, (int)ConceitoValores.S);
            dicionario[CODIGO_ALUNO_1].Add(BIMESTRE_3, (int)ConceitoValores.NS);
            dicionario[CODIGO_ALUNO_1].Add(BIMESTRE_4, (int)ConceitoValores.P);

            dicionario[CODIGO_ALUNO_2].Add(BIMESTRE_1, (int)ConceitoValores.S);
            dicionario[CODIGO_ALUNO_2].Add(BIMESTRE_2, (int)ConceitoValores.NS);
            dicionario[CODIGO_ALUNO_2].Add(BIMESTRE_3, (int)ConceitoValores.P);
            dicionario[CODIGO_ALUNO_2].Add(BIMESTRE_4, (int)ConceitoValores.S);

            return dicionario;
        }

        private Dictionary<string, Dictionary<int, int>> ObtenhaDicionarioNotaRegenteAluno()
        {
            var dicionarioNotaRegenteAluno = new Dictionary<string, Dictionary<int, int>>();
            dicionarioNotaRegenteAluno.Add(COMPONENTE_CIENCIAS_ID_89, new Dictionary<int, int>());
            dicionarioNotaRegenteAluno.Add(COMPONENTE_GEOGRAFIA_ID_8, new Dictionary<int, int>());
            dicionarioNotaRegenteAluno.Add(COMPONENTE_MATEMATICA_ID_2, new Dictionary<int, int>());
            dicionarioNotaRegenteAluno.Add(COMPONENTE_HISTORIA_ID_7, new Dictionary<int, int>());

            dicionarioNotaRegenteAluno[COMPONENTE_CIENCIAS_ID_89].Add(BIMESTRE_1, (int)ConceitoValores.P);
            dicionarioNotaRegenteAluno[COMPONENTE_GEOGRAFIA_ID_8].Add(BIMESTRE_1, (int)ConceitoValores.S);
            dicionarioNotaRegenteAluno[COMPONENTE_MATEMATICA_ID_2].Add(BIMESTRE_1, (int)ConceitoValores.NS);
            dicionarioNotaRegenteAluno[COMPONENTE_HISTORIA_ID_7].Add(BIMESTRE_1, (int)ConceitoValores.S);

            dicionarioNotaRegenteAluno[COMPONENTE_CIENCIAS_ID_89].Add(BIMESTRE_2, (int)ConceitoValores.S);
            dicionarioNotaRegenteAluno[COMPONENTE_GEOGRAFIA_ID_8].Add(BIMESTRE_2, (int)ConceitoValores.NS);
            dicionarioNotaRegenteAluno[COMPONENTE_MATEMATICA_ID_2].Add(BIMESTRE_2, (int)ConceitoValores.P);
            dicionarioNotaRegenteAluno[COMPONENTE_HISTORIA_ID_7].Add(BIMESTRE_2, (int)ConceitoValores.P);

            dicionarioNotaRegenteAluno[COMPONENTE_CIENCIAS_ID_89].Add(BIMESTRE_3, (int)ConceitoValores.NS);
            dicionarioNotaRegenteAluno[COMPONENTE_GEOGRAFIA_ID_8].Add(BIMESTRE_3, (int)ConceitoValores.P);
            dicionarioNotaRegenteAluno[COMPONENTE_MATEMATICA_ID_2].Add(BIMESTRE_3, (int)ConceitoValores.S);
            dicionarioNotaRegenteAluno[COMPONENTE_HISTORIA_ID_7].Add(BIMESTRE_3, (int)ConceitoValores.NS);

            dicionarioNotaRegenteAluno[COMPONENTE_CIENCIAS_ID_89].Add(BIMESTRE_4, (int)ConceitoValores.NS);
            dicionarioNotaRegenteAluno[COMPONENTE_GEOGRAFIA_ID_8].Add(BIMESTRE_4, (int)ConceitoValores.S);
            dicionarioNotaRegenteAluno[COMPONENTE_MATEMATICA_ID_2].Add(BIMESTRE_4, (int)ConceitoValores.S);
            dicionarioNotaRegenteAluno[COMPONENTE_HISTORIA_ID_7].Add(BIMESTRE_4, (int)ConceitoValores.NS);

            return dicionarioNotaRegenteAluno;
        }


        private async Task CriaFechamentoNotaNumerica()
        {
            await CriaFechamentoBimestre(COMPONENTE_CURRICULAR_PORTUGUES_ID_138);
            await CriaFechamentoNota(FECHAMENTO_ALUNO_ID_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, dicionarioNotaNumericaBimestres[CODIGO_ALUNO_1][BIMESTRE_1]);
            await CriaFechamentoNota(FECHAMENTO_ALUNO_ID_2, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, dicionarioNotaNumericaBimestres[CODIGO_ALUNO_1][BIMESTRE_2]);
            await CriaFechamentoNota(FECHAMENTO_ALUNO_ID_3, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, dicionarioNotaNumericaBimestres[CODIGO_ALUNO_1][BIMESTRE_3]);
            await CriaFechamentoNota(FECHAMENTO_ALUNO_ID_4, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, dicionarioNotaNumericaBimestres[CODIGO_ALUNO_1][BIMESTRE_4]);
            await CriaFechamentoNota(FECHAMENTO_ALUNO_ID_5, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, dicionarioNotaNumericaBimestres[CODIGO_ALUNO_2][BIMESTRE_1]);
            await CriaFechamentoNota(FECHAMENTO_ALUNO_ID_6, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, dicionarioNotaNumericaBimestres[CODIGO_ALUNO_2][BIMESTRE_2]);
            await CriaFechamentoNota(FECHAMENTO_ALUNO_ID_7, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, dicionarioNotaNumericaBimestres[CODIGO_ALUNO_2][BIMESTRE_3]);
            await CriaFechamentoNota(FECHAMENTO_ALUNO_ID_8, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, dicionarioNotaNumericaBimestres[CODIGO_ALUNO_2][BIMESTRE_4]);
            await CriaFechamentoFinal(COMPONENTE_CURRICULAR_PORTUGUES_ID_138);
            await CriaFechamentoNota(FECHAMENTO_ALUNO_ID_9, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, NOTA_8);
            await CriaFechamentoNota(FECHAMENTO_ALUNO_ID_10, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, NOTA_9);

            await CriaFrequencia(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), CODIGO_ALUNO_1, PERIODO_ESCOLAR_CODIGO_1, 2, 1, 4, BIMESTRE_1, DATA_01_02_INICIO_BIMESTRE_1, DATA_25_04_FIM_BIMESTRE_1);
            await CriaFrequencia(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), CODIGO_ALUNO_1, PERIODO_ESCOLAR_CODIGO_2, 1, 1, 4, BIMESTRE_2, DATA_02_05_INICIO_BIMESTRE_2, DATA_24_07_FIM_BIMESTRE_2);
            await CriaFrequencia(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), CODIGO_ALUNO_2, PERIODO_ESCOLAR_CODIGO_1, 0, 0, 4, BIMESTRE_1, DATA_01_02_INICIO_BIMESTRE_1, DATA_25_04_FIM_BIMESTRE_1);
            await CriaFrequencia(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), CODIGO_ALUNO_2, PERIODO_ESCOLAR_CODIGO_2, 3, 1, 4, BIMESTRE_2, DATA_02_05_INICIO_BIMESTRE_2, DATA_24_07_FIM_BIMESTRE_2);
        }

        private async Task CriaFechamentoFinal(long idDiciplina)
        {
            await CriaFechamentoTurma_Disciplina(null, idDiciplina, FECHAMENTO_TURMA_DISCIPLINA_ID_5);

            await CriaFechamentoAluno(FECHAMENTO_TURMA_DISCIPLINA_ID_5, CODIGO_ALUNO_1);
            await CriaFechamentoAluno(FECHAMENTO_TURMA_DISCIPLINA_ID_5, CODIGO_ALUNO_2);
        }

        private async Task CriaFechamentoBimestre(long idDiciplina)
        {
            await CriaFechamentoTurmaTodosBimestres(idDiciplina);
            await CriaFechamentoAlunoTodosBimestre(CODIGO_ALUNO_1);
            await CriaFechamentoAlunoTodosBimestre(CODIGO_ALUNO_2);
        }

        private async Task CriaFechamentoTurmaTodosBimestres(long idDiciplina)
        {
            await CriaFechamentoTurma_Disciplina(PERIODO_ESCOLAR_CODIGO_1, idDiciplina, FECHAMENTO_TURMA_ID_1);
            await CriaFechamentoTurma_Disciplina(PERIODO_ESCOLAR_CODIGO_2, idDiciplina, FECHAMENTO_TURMA_ID_2);
            await CriaFechamentoTurma_Disciplina(PERIODO_ESCOLAR_CODIGO_3, idDiciplina, FECHAMENTO_TURMA_ID_3);
            await CriaFechamentoTurma_Disciplina(PERIODO_ESCOLAR_CODIGO_4, idDiciplina, FECHAMENTO_TURMA_ID_4);
        }

        private async Task CriaFechamentoAlunoTodosBimestre(string codigoAluno)
        {
            await CriaFechamentoAluno(FECHAMENTO_TURMA_DISCIPLINA_ID_1, codigoAluno);
            await CriaFechamentoAluno(FECHAMENTO_TURMA_DISCIPLINA_ID_2, codigoAluno);
            await CriaFechamentoAluno(FECHAMENTO_TURMA_DISCIPLINA_ID_3, codigoAluno);
            await CriaFechamentoAluno(FECHAMENTO_TURMA_DISCIPLINA_ID_4, codigoAluno);
        }

        private async Task CriaFechamentoTurma_Disciplina(long? idPeriodo, long idDiciplina, long idFechamentoTurma)
        {
            await InserirNaBase(new FechamentoTurma()
            {
                TurmaId = TURMA_ID_1,
                PeriodoEscolarId = idPeriodo,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new FechamentoTurmaDisciplina()
            {
                DisciplinaId = idDiciplina,
                FechamentoTurmaId = idFechamentoTurma,
                Situacao = SituacaoFechamento.ProcessadoComSucesso,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task CriaFechamentoAluno(
                            long idFechamenteoTurmaDiciplina,
                            string codigoAluno)
        {
            await InserirNaBase(new FechamentoAluno()
            {
                AlunoCodigo = codigoAluno,
                FechamentoTurmaDisciplinaId = idFechamenteoTurmaDiciplina,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task CriaFechamentoNotaConceito()
        {
            await CriaFechamentoBimestre(COMPONENTE_CURRICULAR_ARTES_ID_139);
            await CriaFechamentoConceito(FECHAMENTO_ALUNO_ID_1, COMPONENTE_CURRICULAR_ARTES_ID_139, dicionarioNotaConceitoAluno[CODIGO_ALUNO_1][BIMESTRE_1]);
            await CriaFechamentoConceito(FECHAMENTO_ALUNO_ID_2, COMPONENTE_CURRICULAR_ARTES_ID_139, dicionarioNotaConceitoAluno[CODIGO_ALUNO_1][BIMESTRE_2]);
            await CriaFechamentoConceito(FECHAMENTO_ALUNO_ID_3, COMPONENTE_CURRICULAR_ARTES_ID_139, dicionarioNotaConceitoAluno[CODIGO_ALUNO_1][BIMESTRE_3]);
            await CriaFechamentoConceito(FECHAMENTO_ALUNO_ID_4, COMPONENTE_CURRICULAR_ARTES_ID_139, dicionarioNotaConceitoAluno[CODIGO_ALUNO_1][BIMESTRE_4]);
            await CriaFechamentoConceito(FECHAMENTO_ALUNO_ID_5, COMPONENTE_CURRICULAR_ARTES_ID_139, dicionarioNotaConceitoAluno[CODIGO_ALUNO_2][BIMESTRE_1]);
            await CriaFechamentoConceito(FECHAMENTO_ALUNO_ID_6, COMPONENTE_CURRICULAR_ARTES_ID_139, dicionarioNotaConceitoAluno[CODIGO_ALUNO_2][BIMESTRE_2]);
            await CriaFechamentoConceito(FECHAMENTO_ALUNO_ID_7, COMPONENTE_CURRICULAR_ARTES_ID_139, dicionarioNotaConceitoAluno[CODIGO_ALUNO_2][BIMESTRE_3]);
            await CriaFechamentoConceito(FECHAMENTO_ALUNO_ID_8, COMPONENTE_CURRICULAR_ARTES_ID_139, dicionarioNotaConceitoAluno[CODIGO_ALUNO_2][BIMESTRE_4]);
            await CriaFechamentoFinal(COMPONENTE_CURRICULAR_ARTES_ID_139);
            await CriaFechamentoConceito(FECHAMENTO_ALUNO_ID_9, COMPONENTE_CURRICULAR_ARTES_ID_139, (int)ConceitoValores.P);
            await CriaFechamentoConceito(FECHAMENTO_ALUNO_ID_10, COMPONENTE_CURRICULAR_ARTES_ID_139, (int)ConceitoValores.S);

            await CriaFrequencia(COMPONENTE_CURRICULAR_ARTES_ID_139.ToString(), CODIGO_ALUNO_1, PERIODO_ESCOLAR_CODIGO_1, 2, 1, 4, BIMESTRE_1, DATA_01_02_INICIO_BIMESTRE_1, DATA_25_04_FIM_BIMESTRE_1);
            await CriaFrequencia(COMPONENTE_CURRICULAR_ARTES_ID_139.ToString(), CODIGO_ALUNO_1, PERIODO_ESCOLAR_CODIGO_2, 1, 1, 4, BIMESTRE_2, DATA_02_05_INICIO_BIMESTRE_2, DATA_24_07_FIM_BIMESTRE_2);
            await CriaFrequencia(COMPONENTE_CURRICULAR_ARTES_ID_139.ToString(), CODIGO_ALUNO_2, PERIODO_ESCOLAR_CODIGO_1, 0, 0, 4, BIMESTRE_1, DATA_01_02_INICIO_BIMESTRE_1, DATA_25_04_FIM_BIMESTRE_1);
            await CriaFrequencia(COMPONENTE_CURRICULAR_ARTES_ID_139.ToString(), CODIGO_ALUNO_2, PERIODO_ESCOLAR_CODIGO_2, 3, 1, 4, BIMESTRE_2, DATA_02_05_INICIO_BIMESTRE_2, DATA_24_07_FIM_BIMESTRE_2);
        }

        private async Task CriaFechamentoNotaConceitoRegencia()
        {
            await CriaFechamentoTurmaTodosBimestres(COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105);
            await CriaFechamentoAlunoTodosBimestre(CODIGO_ALUNO_1);

            await CriaFechamentoConceito(FECHAMENTO_ALUNO_ID_1, long.Parse(COMPONENTE_CIENCIAS_ID_89), dicionarioNotaRegenteAluno[COMPONENTE_CIENCIAS_ID_89][BIMESTRE_1]);
            await CriaFechamentoConceito(FECHAMENTO_ALUNO_ID_1, long.Parse(COMPONENTE_GEOGRAFIA_ID_8), dicionarioNotaRegenteAluno[COMPONENTE_GEOGRAFIA_ID_8][BIMESTRE_1]);
            await CriaFechamentoConceito(FECHAMENTO_ALUNO_ID_1, long.Parse(COMPONENTE_MATEMATICA_ID_2), dicionarioNotaRegenteAluno[COMPONENTE_MATEMATICA_ID_2][BIMESTRE_1]);
            await CriaFechamentoConceito(FECHAMENTO_ALUNO_ID_1, long.Parse(COMPONENTE_HISTORIA_ID_7), dicionarioNotaRegenteAluno[COMPONENTE_HISTORIA_ID_7][BIMESTRE_1]);

            await CriaFechamentoConceito(FECHAMENTO_ALUNO_ID_2, long.Parse(COMPONENTE_CIENCIAS_ID_89), dicionarioNotaRegenteAluno[COMPONENTE_CIENCIAS_ID_89][BIMESTRE_2]);
            await CriaFechamentoConceito(FECHAMENTO_ALUNO_ID_2, long.Parse(COMPONENTE_GEOGRAFIA_ID_8), dicionarioNotaRegenteAluno[COMPONENTE_GEOGRAFIA_ID_8][BIMESTRE_2]);
            await CriaFechamentoConceito(FECHAMENTO_ALUNO_ID_2, long.Parse(COMPONENTE_MATEMATICA_ID_2), dicionarioNotaRegenteAluno[COMPONENTE_MATEMATICA_ID_2][BIMESTRE_2]);
            await CriaFechamentoConceito(FECHAMENTO_ALUNO_ID_2, long.Parse(COMPONENTE_HISTORIA_ID_7), dicionarioNotaRegenteAluno[COMPONENTE_HISTORIA_ID_7][BIMESTRE_2]);

            await CriaFechamentoConceito(FECHAMENTO_ALUNO_ID_3, long.Parse(COMPONENTE_CIENCIAS_ID_89), dicionarioNotaRegenteAluno[COMPONENTE_CIENCIAS_ID_89][BIMESTRE_3]);
            await CriaFechamentoConceito(FECHAMENTO_ALUNO_ID_3, long.Parse(COMPONENTE_GEOGRAFIA_ID_8), dicionarioNotaRegenteAluno[COMPONENTE_GEOGRAFIA_ID_8][BIMESTRE_3]);
            await CriaFechamentoConceito(FECHAMENTO_ALUNO_ID_3, long.Parse(COMPONENTE_MATEMATICA_ID_2), dicionarioNotaRegenteAluno[COMPONENTE_MATEMATICA_ID_2][BIMESTRE_3]);
            await CriaFechamentoConceito(FECHAMENTO_ALUNO_ID_3, long.Parse(COMPONENTE_HISTORIA_ID_7), dicionarioNotaRegenteAluno[COMPONENTE_HISTORIA_ID_7][BIMESTRE_3]);

            await CriaFechamentoConceito(FECHAMENTO_ALUNO_ID_4, long.Parse(COMPONENTE_CIENCIAS_ID_89), dicionarioNotaRegenteAluno[COMPONENTE_CIENCIAS_ID_89][BIMESTRE_4]);
            await CriaFechamentoConceito(FECHAMENTO_ALUNO_ID_4, long.Parse(COMPONENTE_GEOGRAFIA_ID_8), dicionarioNotaRegenteAluno[COMPONENTE_GEOGRAFIA_ID_8][BIMESTRE_4]);
            await CriaFechamentoConceito(FECHAMENTO_ALUNO_ID_4, long.Parse(COMPONENTE_MATEMATICA_ID_2), dicionarioNotaRegenteAluno[COMPONENTE_MATEMATICA_ID_2][BIMESTRE_4]);
            await CriaFechamentoConceito(FECHAMENTO_ALUNO_ID_4, long.Parse(COMPONENTE_HISTORIA_ID_7), dicionarioNotaRegenteAluno[COMPONENTE_HISTORIA_ID_7][BIMESTRE_4]);

            await CriaFechamentoFinal(COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105);
            await CriaFechamentoConceito(FECHAMENTO_ALUNO_ID_5, COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105, (int)ConceitoValores.P);

            await CriaFrequencia(COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105.ToString(), CODIGO_ALUNO_1, PERIODO_ESCOLAR_CODIGO_1, 2, 1, 4, BIMESTRE_1, DATA_01_02_INICIO_BIMESTRE_1, DATA_25_04_FIM_BIMESTRE_1);
            await CriaFrequencia(COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105.ToString(), CODIGO_ALUNO_1, PERIODO_ESCOLAR_CODIGO_2, 1, 1, 4, BIMESTRE_2, DATA_02_05_INICIO_BIMESTRE_2, DATA_24_07_FIM_BIMESTRE_2);
        }

        private async Task CriaFechamentoNota(
                            long idFechamentoAluno,
                            long idDiciplina,
                            double nota, 
                            bool excluido = false)
        {
            await InserirNaBase(new FechamentoNota()
            {
                FechamentoAlunoId = idFechamentoAluno,
                DisciplinaId = idDiciplina,
                Nota = nota,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                Excluido = excluido,
            });
        }

        private async Task CriaFechamentoConceito(
                                      long idFechamentoAluno,
                                      long idDiciplina,
                                      long idConceito)
        {
            await InserirNaBase(new FechamentoNota()
            {
                FechamentoAlunoId = idFechamentoAluno,
                ConceitoId = idConceito,
                DisciplinaId = idDiciplina,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task CriaFrequencia(
                            string idDisciplina,
                            string codigoAluno,
                            long idPeriodoEscolar,
                            int totalFalta,
                            int totalCompensacao,
                            int totalDeAula,
                            int bimestre,
                            DateTime dataInicio,
                            DateTime dataFim)
        {
            await InserirNaBase(new Dominio.FrequenciaAluno()
            {

                DisciplinaId = idDisciplina,
                CodigoAluno = codigoAluno,
                PeriodoEscolarId = idPeriodoEscolar,
                TurmaId = TURMA_CODIGO_1,
                Tipo = TipoFrequenciaAluno.PorDisciplina,
                TotalAusencias = totalFalta,
                TotalCompensacoes = totalCompensacao,
                TotalAulas = totalDeAula,
                Bimestre = bimestre,
                PeriodoInicio = dataInicio,
                PeriodoFim = dataFim,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task CriaRegistroDeFrenquencia()
        {
            await InserirNaBase(new RegistroFrequencia
            {
                AulaId = AULA_ID,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task CriaRegistroDeFrequenciaAluno(string codigoAluno, int valor)
        {
            await InserirNaBase(new RegistroFrequenciaAluno
            {
                CodigoAluno = codigoAluno,
                RegistroFrequenciaId = REGISTRO_FREQUENCIA_ID,
                Valor = valor,
                NumeroAula = QUANTIDADE_AULA_4,
                AulaId = AULA_ID,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }
    }
}
