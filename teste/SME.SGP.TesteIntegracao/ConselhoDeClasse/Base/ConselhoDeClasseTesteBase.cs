using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.ConselhoDeClasse
{
    public abstract class ConselhoDeClasseTesteBase : TesteBaseComuns
    {
        protected const long AULA_ID_1 = 1;

        protected const string TIPO_FREQUENCIA_COMPARECEU = "C";
        protected const string TIPO_FREQUENCIA_FALTOU = "F";
        protected const string TIPO_FREQUENCIA_REMOTO = "R";

        protected const string CODIGO_ALUNO_99999 = "99999";

        protected const string ALUNO_CODIGO_1 = "1";
        protected const string ALUNO_CODIGO_2 = "2";
        protected const string ALUNO_CODIGO_3 = "3";
        protected const string ALUNO_CODIGO_4 = "4";

        protected const double NOTA_1 = 1;
        protected const double NOTA_2 = 2;
        protected const double NOTA_3 = 3;
        protected const double NOTA_4 = 4;
        protected const double NOTA_5 = 5;
        protected const double NOTA_6 = 6;
        protected const double NOTA_7 = 7;
        protected const double NOTA_8 = 8;
        protected const double NOTA_9 = 9;
        protected const double NOTA_10 = 10;

        protected const string NOTA = "NOTA";
        protected const string CONCEITO = "CONCEITO";

        protected const int CONSELHO_CLASSE_ID_1 = 1;
        protected const int FECHAMENTO_TURMA_ID_1 = 1;
        protected const int CONSELHO_CLASSE_ALUNO_ID_1 = 1;
        protected const string JUSTIFICATIVA = "Nota pós conselho";
        private const string COMPONENTE_CURRICULAR_GRUPO_AREA_ORDENACAO = "componente_curricular_grupo_area_ordenacao";

        protected ConselhoDeClasseTesteBase(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTurmaItinerarioEnsinoMedioQuery, IEnumerable<TurmaItinerarioEnsinoMedioDto>>), typeof(ObterTurmaItinerarioEnsinoMedioQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery, string[]>), typeof(ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosPorTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterAlunosPorTurmaQueryHandlerComRegistroFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosPorTurmaEAnoLetivoQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterAlunosPorTurmaEAnoLetivoQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterDisciplinasTurmasEolQuery, IEnumerable<DisciplinaResposta>>), typeof(ObterDisciplinasTurmasEolQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTurmaAlunoPorCodigoAlunoQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterTurmaAlunoPorCodigoAlunoQueryHandlerFake), ServiceLifetime.Scoped));
        }

        protected async Task ExecuteTeste(
                    ConselhoClasseNotaDto dto, 
                    int conselhoClasseId,
                    bool anoAnterior, 
                    string codigoAluno, 
                    TipoNota tipoNota,
                    int bimestre)
        {
            var comando = ServiceProvider.GetService<IComandosConselhoClasseNota>();

            var dtoRetorno = await comando.SalvarAsync(dto, codigoAluno, conselhoClasseId, FECHAMENTO_TURMA_ID_1, TURMA_CODIGO_1, bimestre);

            dtoRetorno.ShouldNotBeNull();
            var listaConselhoClasseNota = ObterTodos<ConselhoClasseNota>();
            listaConselhoClasseNota.ShouldNotBeNull();
            var classeNota = listaConselhoClasseNota.FirstOrDefault(nota => nota.ConselhoClasseAlunoId == CONSELHO_CLASSE_ALUNO_ID_1);
            classeNota.ShouldNotBeNull();
            classeNota.Justificativa.ShouldBe(dto.Justificativa);

            if (anoAnterior)
            {
                classeNota.Nota.ShouldBeNull();
                classeNota.ConceitoId.ShouldBeNull();
                var listaAprovacaoNotaConselho = ObterTodos<WFAprovacaoNotaConselho>();
                listaAprovacaoNotaConselho.ShouldNotBeNull();
                var aprovacaoNotaConselho = listaAprovacaoNotaConselho.FirstOrDefault(aprovacao => aprovacao.ConselhoClasseNotaId == classeNota.Id);
                aprovacaoNotaConselho.ShouldNotBeNull();
                if (tipoNota == TipoNota.Nota)
                    aprovacaoNotaConselho.Nota.ShouldBe(dto.Nota);
                else
                    aprovacaoNotaConselho.ConceitoId.ShouldBe(dto.Conceito);
            }
            else
            {
                if (tipoNota == TipoNota.Nota)
                    classeNota.Nota.ShouldBe(dto.Nota);
                else
                    classeNota.ConceitoId.ShouldBe(dto.Conceito);
            }

            var listaConsolidado = ObterTodos<ConselhoClasseConsolidadoTurmaAluno>();
            listaConsolidado.ShouldNotBeNull();
            var consolidado = listaConsolidado.FirstOrDefault(consolidadoAluno => consolidadoAluno.AlunoCodigo == codigoAluno);
        }

        protected async Task CriarDadosBase(FiltroNotasDto filtroNota)
        {
            await CriarDreUePerfilComponenteCurricular();

            CriarClaimUsuario(filtroNota.Perfil);

            await CriarUsuarios();

            await CriarTurmaTipoCalendario(filtroNota);

            if (filtroNota.CriarPeriodoEscolar)
                await CriarPeriodoEscolar(filtroNota.ConsiderarAnoAnterior);

            if (filtroNota.CriarPeriodoAbertura)
                await CriarPeriodoAbertura(filtroNota);

            await CriarAula(filtroNota.ComponenteCurricular, filtroNota.DataAula, RecorrenciaAula.AulaUnica, NUMERO_AULA_1);

            await CriarParametrosNotas();

            await CriarAbrangencia(filtroNota.Perfil);

            await CriarCiclo();

            await CriarNotasTipoEParametros(filtroNota.ConsiderarAnoAnterior);

            await CriarFechamentoTurma();

            await CriarComponenteGrupoAreaOrdenacao();

            await CriarConselhoClasseRecomendacao();

            await CriaConceito();
        }

        private async Task CriarComponenteGrupoAreaOrdenacao() 
        {
            await InserirNaBase(COMPONENTE_CURRICULAR_GRUPO_AREA_ORDENACAO, CODIGO_1, CODIGO_1, CODIGO_1);
        }

        private async Task CriarConselhoClasseRecomendacao()
        {
            await InserirNaBase(new Dominio.ConselhoClasseRecomendacao()
            {
                Recomendacao = "Recomendação aluno",
                Tipo = ConselhoClasseRecomendacaoTipo.Aluno,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
            await InserirNaBase(new Dominio.ConselhoClasseRecomendacao()
            {
                Recomendacao = "Recomendação familia",
                Tipo = ConselhoClasseRecomendacaoTipo.Familia,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task CriarFechamentoTurma()
        {
            await InserirNaBase(new FechamentoTurma()
            {
                PeriodoEscolarId = 1,
                TurmaId = TURMA_ID_1,
                Excluido = false,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        protected async Task CriarTurmaTipoCalendario(FiltroNotasDto filtroNota)
        {
            await CriarTipoCalendario(filtroNota.TipoCalendario, filtroNota.ConsiderarAnoAnterior);
            await CriarTurma(filtroNota.Modalidade, filtroNota.AnoTurma, filtroNota.ConsiderarAnoAnterior);
        }

        private async Task CriarNotasTipoEParametros(bool consideraAnoAnterior = false)
        {
            var dataBase = consideraAnoAnterior ? new DateTime(DateTimeExtension.HorarioBrasilia().AddYears(-1).Year, 01, 01) : new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01);

            await InserirNaBase(new NotaTipoValor()
            {
                Ativo = true,
                InicioVigencia = dataBase,
                TipoNota = TipoNota.Nota,
                Descricao = NOTA,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new NotaTipoValor()
            {
                Ativo = true,
                InicioVigencia = dataBase,
                TipoNota = TipoNota.Conceito,
                Descricao = CONCEITO,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new NotaConceitoCicloParametro()
            {
                CicloId = 1,
                TipoNotaId = 2,
                QtdMinimaAvalicoes = 1,
                PercentualAlerta = 50,
                Ativo = true,
                InicioVigencia = dataBase,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new NotaConceitoCicloParametro()
            {
                CicloId = 2,
                TipoNotaId = 1,
                QtdMinimaAvalicoes = 1,
                PercentualAlerta = 50,
                Ativo = true,
                InicioVigencia = dataBase,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new NotaConceitoCicloParametro()
            {
                CicloId = 3,
                TipoNotaId = 1,
                QtdMinimaAvalicoes = 1,
                PercentualAlerta = 50,
                Ativo = true,
                InicioVigencia = dataBase,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new NotaConceitoCicloParametro()
            {
                CicloId = 4,
                TipoNotaId = 1,
                QtdMinimaAvalicoes = 1,
                PercentualAlerta = 50,
                Ativo = true,
                InicioVigencia = dataBase,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new NotaConceitoCicloParametro()
            {
                CicloId = 5,
                TipoNotaId = 2,
                QtdMinimaAvalicoes = 1,
                PercentualAlerta = 50,
                Ativo = true,
                InicioVigencia = dataBase,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new NotaConceitoCicloParametro()
            {
                CicloId = 6,
                TipoNotaId = 2,
                QtdMinimaAvalicoes = 1,
                PercentualAlerta = 50,
                Ativo = true,
                InicioVigencia = dataBase,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new NotaConceitoCicloParametro()
            {
                CicloId = 7,
                TipoNotaId = 1,
                QtdMinimaAvalicoes = 1,
                PercentualAlerta = 50,
                Ativo = true,
                InicioVigencia = dataBase,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new NotaConceitoCicloParametro()
            {
                CicloId = 8,
                TipoNotaId = 1,
                QtdMinimaAvalicoes = 1,
                PercentualAlerta = 50,
                Ativo = true,
                InicioVigencia = dataBase,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new NotaParametro()
            {
                Minima = 0,
                Media = 5,
                Maxima = 10,
                Incremento = 0.5,
                Ativo = true,
                InicioVigencia = dataBase,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        protected async Task CriarCiclo()
        {
            await InserirNaBase(new Ciclo()
            {
                Descricao = ALFABETIZACAO,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new CicloAno()
            {
                CicloId = 1,
                Ano = ANO_1,
                Modalidade = Modalidade.Fundamental
            });

            await InserirNaBase(new CicloAno()
            {
                CicloId = 1,
                Ano = ANO_2,
                Modalidade = Modalidade.Fundamental
            });

            await InserirNaBase(new CicloAno()
            {
                CicloId = 1,
                Ano = ANO_3,
                Modalidade = Modalidade.Fundamental
            });

            await InserirNaBase(new Ciclo()
            {
                Descricao = INTERDISCIPLINAR,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new CicloAno()
            {
                CicloId = 2,
                Ano = ANO_4,
                Modalidade = Modalidade.Fundamental
            });

            await InserirNaBase(new CicloAno()
            {
                CicloId = 2,
                Ano = ANO_5,
                Modalidade = Modalidade.Fundamental
            });

            await InserirNaBase(new CicloAno()
            {
                CicloId = 2,
                Ano = ANO_6,
                Modalidade = Modalidade.Fundamental
            });

            await InserirNaBase(new Ciclo()
            {
                Descricao = AUTORAL,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new CicloAno()
            {
                CicloId = 3,
                Ano = ANO_7,
                Modalidade = Modalidade.Fundamental
            });

            await InserirNaBase(new CicloAno()
            {
                CicloId = 3,
                Ano = ANO_8,
                Modalidade = Modalidade.Fundamental
            });

            await InserirNaBase(new CicloAno()
            {
                CicloId = 3,
                Ano = ANO_9,
                Modalidade = Modalidade.Fundamental
            });

            await InserirNaBase(new Ciclo()
            {
                Descricao = MEDIO,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new CicloAno()
            {
                CicloId = 4,
                Ano = ANO_1,
                Modalidade = Modalidade.Medio
            });

            await InserirNaBase(new CicloAno()
            {
                CicloId = 4,
                Ano = ANO_2,
                Modalidade = Modalidade.Medio
            });

            await InserirNaBase(new CicloAno()
            {
                CicloId = 4,
                Ano = ANO_3,
                Modalidade = Modalidade.Medio
            });

            await InserirNaBase(new CicloAno()
            {
                CicloId = 4,
                Ano = ANO_4,
                Modalidade = Modalidade.Medio
            });

            await InserirNaBase(new Ciclo()
            {
                Descricao = EJA_ALFABETIZACAO,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new CicloAno()
            {
                CicloId = 5,
                Ano = ANO_1,
                Modalidade = Modalidade.EJA
            });

            await InserirNaBase(new Ciclo()
            {
                Descricao = EJA_BASICA,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new CicloAno()
            {
                CicloId = 6,
                Ano = ANO_2,
                Modalidade = Modalidade.EJA
            });

            await InserirNaBase(new Ciclo()
            {
                Descricao = EJA_COMPLEMENTAR,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new CicloAno()
            {
                CicloId = 7,
                Ano = ANO_3,
                Modalidade = Modalidade.EJA
            });

            await InserirNaBase(new Ciclo()
            {
                Descricao = EJA_FINAL,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new CicloAno()
            {
                CicloId = 8,
                Ano = ANO_4,
                Modalidade = Modalidade.EJA
            });
        }

        protected async Task CriarAbrangencia(string perfil)
        {
            await InserirNaBase(new Abrangencia()
            {
                DreId = DRE_ID_1,
                Historico = false,
                Perfil = new Guid(perfil),
                TurmaId = TURMA_ID_1,
                UeId = UE_ID_1,
                UsuarioId = USUARIO_ID_1
            });
        }

        private async Task CriarParametrosNotas()
        {
            var dataAtualAnoAnterior = DateTimeExtension.HorarioBrasilia().AddYears(-1);

            var dataAtualAnoAtual = DateTimeExtension.HorarioBrasilia();

            await InserirNaBase(new ParametrosSistema()
            {
                Nome = DATA_INICIO_SGP,
                Descricao = DATA_INICIO_SGP,
                Tipo = TipoParametroSistema.DataInicioSGP,
                Valor = dataAtualAnoAnterior.Year.ToString(),
                Ano = dataAtualAnoAnterior.Year,
                Ativo = true,
                CriadoEm = dataAtualAnoAnterior,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoPor = SISTEMA_NOME
            });

            await InserirNaBase(new ParametrosSistema()
            {
                Nome = DATA_INICIO_SGP,
                Descricao = DATA_INICIO_SGP,
                Tipo = TipoParametroSistema.DataInicioSGP,
                Valor = dataAtualAnoAtual.Year.ToString(),
                Ano = dataAtualAnoAtual.Year,
                Ativo = true,
                CriadoEm = dataAtualAnoAtual,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoPor = SISTEMA_NOME
            });

            await InserirNaBase(new ParametrosSistema()
            {
                Nome = PERCENTUAL_ALUNOS_INSUFICIENTES,
                Descricao = PERCENTUAL_ALUNOS_INSUFICIENTES,
                Tipo = TipoParametroSistema.PercentualAlunosInsuficientes,
                Valor = NUMERO_50,
                Ano = dataAtualAnoAnterior.Year,
                Ativo = true,
                CriadoEm = dataAtualAnoAnterior,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoPor = SISTEMA_NOME
            });

            await InserirNaBase(new ParametrosSistema()
            {
                Nome = PERCENTUAL_ALUNOS_INSUFICIENTES,
                Descricao = PERCENTUAL_ALUNOS_INSUFICIENTES,
                Tipo = TipoParametroSistema.PercentualAlunosInsuficientes,
                Valor = NUMERO_50,
                Ano = dataAtualAnoAtual.Year,
                Ativo = true,
                CriadoEm = dataAtualAnoAtual,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoPor = SISTEMA_NOME
            });

            await InserirNaBase(new ParametrosSistema()
            {
                Nome = MEDIA_BIMESTRAL,
                Descricao = MEDIA_BIMESTRAL,
                Tipo = TipoParametroSistema.MediaBimestre,
                Valor = NUMERO_5,
                Ano = dataAtualAnoAnterior.Year,
                Ativo = true,
                CriadoEm = dataAtualAnoAnterior,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoPor = SISTEMA_NOME
            });

            await InserirNaBase(new ParametrosSistema()
            {
                Nome = MEDIA_BIMESTRAL,
                Descricao = MEDIA_BIMESTRAL,
                Tipo = TipoParametroSistema.MediaBimestre,
                Valor = NUMERO_5,
                Ano = dataAtualAnoAtual.Year,
                Ativo = true,
                CriadoEm = dataAtualAnoAtual,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoPor = SISTEMA_NOME
            });

            await InserirNaBase(new ParametrosSistema()
            {
                Nome = MEDIA_BIMESTRAL,
                Descricao = MEDIA_BIMESTRAL,
                Tipo = TipoParametroSistema.AprovacaoAlteracaoNotaConselho,
                Valor = NUMERO_5,
                Ano = dataAtualAnoAnterior.Year,
                Ativo = true,
                CriadoEm = dataAtualAnoAnterior,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoPor = SISTEMA_NOME
            });
        }

        protected async Task CriarPeriodoEscolarEAbertura()
        {
            await CriarPeriodoEscolar(DATA_03_01_INICIO_BIMESTRE_1, DATA_29_04_FIM_BIMESTRE_1, BIMESTRE_1);

            await CriarPeriodoEscolar(DATA_02_05_INICIO_BIMESTRE_2, DATA_08_07_FIM_BIMESTRE_2, BIMESTRE_2);

            await CriarPeriodoEscolar(DATA_25_07_INICIO_BIMESTRE_3, DATA_30_09_FIM_BIMESTRE_3, BIMESTRE_3);

            await CriarPeriodoEscolar(DATA_03_10_INICIO_BIMESTRE_4, DATA_22_12_FIM_BIMESTRE_4, BIMESTRE_4);

            await CriarPeriodoReabertura(TIPO_CALENDARIO_1);
        }

        protected async Task CriarPeriodoEscolar(bool considerarAnoAnterior = false)
        {
            await CriarPeriodoEscolar(DATA_03_01_INICIO_BIMESTRE_1, DATA_29_04_FIM_BIMESTRE_1, BIMESTRE_1, TIPO_CALENDARIO_1, considerarAnoAnterior);

            await CriarPeriodoEscolar(DATA_02_05_INICIO_BIMESTRE_2, DATA_08_07_FIM_BIMESTRE_2, BIMESTRE_2, TIPO_CALENDARIO_1, considerarAnoAnterior);

            await CriarPeriodoEscolar(DATA_25_07_INICIO_BIMESTRE_3, DATA_30_09_FIM_BIMESTRE_3, BIMESTRE_3, TIPO_CALENDARIO_1, considerarAnoAnterior);

            await CriarPeriodoEscolar(DATA_03_10_INICIO_BIMESTRE_4, DATA_22_12_FIM_BIMESTRE_4, BIMESTRE_4, TIPO_CALENDARIO_1, considerarAnoAnterior);
        }

        protected async Task CriarPeriodoAbertura(FiltroNotasDto filtroNotasDto)
        {
            await CriarPeriodoReabertura(filtroNotasDto.TipoCalendarioId, filtroNotasDto.ConsiderarAnoAnterior);
        }

        private ComponenteCurricularDto ObterComponenteCurricular(long componenteCurricularId)
        {
            if (componenteCurricularId == COMPONENTE_CURRICULAR_PORTUGUES_ID_138)
                return new ComponenteCurricularDto()
                {
                    Codigo = COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                    Descricao = COMPONENTE_CURRICULAR_PORTUGUES_NOME
                };
            else if (componenteCurricularId == COMPONENTE_CURRICULAR_DESCONHECIDO_ID_999999)
                return new ComponenteCurricularDto()
                {
                    Codigo = COMPONENTE_CURRICULAR_DESCONHECIDO_ID_999999.ToString(),
                    Descricao = COMPONENTE_CURRICULAR_DESCONHECIDO_NOME
                };
            else if (componenteCurricularId == COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_ID_1213)
                return new ComponenteCurricularDto()
                {
                    Codigo = COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_ID_1213.ToString(),
                    Descricao = COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_NOME
                };
            else if (componenteCurricularId == COMPONENTE_REG_CLASSE_EJA_ETAPA_ALFAB_ID_1113)
                return new ComponenteCurricularDto()
                {
                    Codigo = COMPONENTE_REG_CLASSE_EJA_ETAPA_ALFAB_ID_1113.ToString(),
                    Descricao = COMPONENTE_REG_CLASSE_EJA_ETAPA_ALFAB_NOME
                };

            return null;
        }

        protected async Task CriarAula(string componenteCurricularCodigo, DateTime dataAula, RecorrenciaAula recorrencia, int quantidadeAula, string rf = USUARIO_PROFESSOR_LOGIN_2222222, bool aulaCj = false, TipoAula tipoAula = TipoAula.Normal)
        {
            await InserirNaBase(ObterAula(componenteCurricularCodigo, dataAula, recorrencia, quantidadeAula, rf, aulaCj, tipoAula));
        }

        private SGP.Dominio.Aula ObterAula(string componenteCurricularCodigo, DateTime dataAula, RecorrenciaAula recorrencia, int quantidadeAula, string rf = USUARIO_PROFESSOR_LOGIN_2222222, bool aulaCj = false, TipoAula tipoAula = TipoAula.Normal)
        {
            return new SGP.Dominio.Aula
            {
                UeId = UE_CODIGO_1,
                DisciplinaId = componenteCurricularCodigo,
                TurmaId = TURMA_CODIGO_1,
                TipoCalendarioId = 1,
                ProfessorRf = rf,
                Quantidade = quantidadeAula,
                DataAula = dataAula,
                RecorrenciaAula = recorrencia,
                TipoAula = tipoAula,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                Excluido = false,
                Migrado = false,
                AulaCJ = aulaCj
            };
        }

        protected async Task CriarPeriodoEscolarEAberturaPadrao()
        {
            await CriarPeriodoEscolar(DATA_03_01_INICIO_BIMESTRE_1, DATA_29_04_FIM_BIMESTRE_1, BIMESTRE_1);

            await CriarPeriodoEscolar(DATA_02_05_INICIO_BIMESTRE_2, DATA_08_07_FIM_BIMESTRE_2, BIMESTRE_2);

            await CriarPeriodoEscolar(DATA_25_07_INICIO_BIMESTRE_3, DATA_30_09_FIM_BIMESTRE_3, BIMESTRE_3);

            await CriarPeriodoEscolar(DATA_03_10_INICIO_BIMESTRE_4, DATA_22_12_FIM_BIMESTRE_4, BIMESTRE_4);

            await CriarPeriodoReabertura(TIPO_CALENDARIO_1);
        }

        protected async Task CriarMotivosAusencias(string descricao)
        {
            await InserirNaBase(new MotivoAusencia() { Descricao = descricao });
        }

        protected async Task CriarPeriodoReabertura(long tipoCalendarioId, bool considerarAnoAnterior = false)
        {
            await InserirNaBase(new FechamentoReabertura()
            {
                Descricao = REABERTURA_GERAL,
                Inicio = considerarAnoAnterior ? DATA_01_01.AddYears(-1) : DATA_01_01,
                Fim = DATA_31_12,
                TipoCalendarioId = tipoCalendarioId,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            await InserirNaBase(new FechamentoReaberturaBimestre()
            {
                FechamentoAberturaId = 1,
                Bimestre = BIMESTRE_1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            await InserirNaBase(new FechamentoReaberturaBimestre()
            {
                FechamentoAberturaId = 1,
                Bimestre = BIMESTRE_2,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            await InserirNaBase(new FechamentoReaberturaBimestre()
            {
                FechamentoAberturaId = 1,
                Bimestre = BIMESTRE_3,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            await InserirNaBase(new FechamentoReaberturaBimestre()
            {
                FechamentoAberturaId = 1,
                Bimestre = BIMESTRE_4,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });
        }

        protected async Task CriarTipoAvaliacao(TipoAvaliacaoCodigo tipoAvalicao, string descricaoAvaliacao)
        {
            await InserirNaBase(new TipoAvaliacao
            {
                Nome = descricaoAvaliacao,
                Descricao = descricaoAvaliacao,
                Situacao = true,
                AvaliacoesNecessariasPorBimestre = 1,
                Codigo = tipoAvalicao,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
        }

        protected async Task CriarAtividadeAvaliativa(DateTime dataAvaliacao, long TipoAvaliacaoId, string nomeAvaliacao, bool ehRegencia = false, bool ehCj = false, string professorRf = USUARIO_PROFESSOR_CODIGO_RF_2222222)
        {
            await InserirNaBase(new AtividadeAvaliativa
            {
                DreId = DRE_CODIGO_1,
                UeId = UE_CODIGO_1,
                ProfessorRf = professorRf,
                TurmaId = TURMA_CODIGO_1,
                Categoria = CategoriaAtividadeAvaliativa.Normal,
                TipoAvaliacaoId = TipoAvaliacaoId,
                NomeAvaliacao = nomeAvaliacao,
                DescricaoAvaliacao = nomeAvaliacao,
                DataAvaliacao = dataAvaliacao,
                EhRegencia = ehRegencia,
                EhCj = ehCj,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
        }

        protected async Task CriarAtividadeAvaliativaDisciplina(long atividadeAvaliativaId, string componenteCurricular)
        {
            await InserirNaBase(new AtividadeAvaliativaDisciplina
            {
                AtividadeAvaliativaId = atividadeAvaliativaId,
                DisciplinaId = componenteCurricular,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
        }

        protected async Task CriaConceito()
        {
            await InserirNaBase(new Conceito()
            {
                Descricao = "Excelente",
                Aprovado = true,
                Ativo = true,
                InicioVigencia = DATA_01_01,
                FimVigencia = DATA_31_12,
                Valor = "E",
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new Conceito()
            {
                Descricao = "Bom",
                Aprovado = true,
                Ativo = true,
                InicioVigencia = DATA_01_01,
                FimVigencia = DATA_31_12,
                Valor = "B",
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new Conceito()
            {
                Descricao = "Ruim",
                Aprovado = true,
                Ativo = true,
                InicioVigencia = DATA_01_01,
                FimVigencia = DATA_31_12,
                Valor = "R",
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        protected static ConselhoClasseNotaDto ObterFiltroConselhoClasseNotaDto(double? nota, long? conceito, string justificavtiva, long codigoComponenteCurricular)
        {
            return new ConselhoClasseNotaDto()
            {
                CodigoComponenteCurricular = codigoComponenteCurricular,
                Conceito = conceito,
                Nota = nota,
                Justificativa = justificavtiva
            };
        }

        protected class FiltroNotasDto
        {
            public FiltroNotasDto()
            {
                CriarPeriodoEscolar = true;
                TipoCalendarioId = TIPO_CALENDARIO_1;
                CriarPeriodoAbertura = true;
                ConsiderarAnoAnterior = false;
            }

            public DateTime? DataReferencia { get; set; }
            public string Perfil { get; set; }
            public Modalidade Modalidade { get; set; }
            public ModalidadeTipoCalendario TipoCalendario { get; set; }
            public int Bimestre { get; set; }
            public string ComponenteCurricular { get; set; }
            public long TipoCalendarioId { get; set; }
            public bool CriarPeriodoEscolar { get; set; }
            public bool CriarPeriodoAbertura { get; set; }
            public TipoNota TipoNota { get; set; }
            public string AnoTurma { get; set; }
            public bool ConsiderarAnoAnterior { get; set; }
            public string ProfessorRf { get; set; }
            public DateTime DataAula { get; set; }
        }
    }
}