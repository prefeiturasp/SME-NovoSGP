using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Aula.Nota.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.Nota
{
    public abstract class NotaTesteBase : TesteBaseComuns
    {
        protected new const long AULA_ID_1 = 1;

        protected const string TIPO_FREQUENCIA_COMPARECEU = "C";
        protected const string TIPO_FREQUENCIA_FALTOU = "F";
        protected const string TIPO_FREQUENCIA_REMOTO = "R";

        protected const string CODIGO_ALUNO_99999 = "99999";

        protected new readonly string ALUNO_CODIGO_1 = "1";
        protected new readonly string ALUNO_CODIGO_2 = "2";
        protected new readonly string ALUNO_CODIGO_3 = "3";
        protected new readonly string ALUNO_CODIGO_4 = "4";
        protected new readonly string ALUNO_CODIGO_5 = "5";
        protected new readonly string ALUNO_CODIGO_6 = "6";
        protected new readonly string ALUNO_CODIGO_7 = "7";
        protected new readonly string ALUNO_CODIGO_8 = "8";
        protected new readonly string ALUNO_CODIGO_9 = "9";
        protected new readonly string ALUNO_CODIGO_10 = "10";

        protected new readonly double NOTA_1 = 1;
        protected new readonly double NOTA_2 = 2;
        protected new readonly double NOTA_3 = 3;
        protected new readonly double NOTA_4 = 4;
        protected new readonly double NOTA_5 = 5;
        protected new readonly double NOTA_6 = 6;
        protected new readonly double NOTA_7 = 7;
        protected new readonly double NOTA_8 = 8;
        protected new readonly double NOTA_9 = 9;
        protected new readonly double NOTA_10 = 10;

        protected readonly string AVALIACAO_NOME_1 = "Avaliação 1";
        protected readonly string AVALIACAO_NOME_2 = "Avaliação 2";

        protected readonly long TIPO_AVALIACAO_CODIGO_1 = 1;
        protected readonly long TIPO_AVALIACAO_CODIGO_2 = 2;

        protected new readonly long PERIODO_ESCOLAR_CODIGO_1 = 1;

        protected new readonly string NOTA = "NOTA";
        protected new readonly string CONCEITO = "CONCEITO";

        protected NotaTesteBase(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterFuncionarioCoreSSOPorPerfilDreQuery, IEnumerable<UsuarioEolRetornoDto>>), typeof(ObterFuncionarioCoreSSOPorPerfilDreQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTodosAlunosNaTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterTodosAlunosNaTurmaQueryHandlerFake), ServiceLifetime.Scoped));
        }

        private (IComandosNotasConceitos, IObterNotasParaAvaliacoesUseCase) RetornarServicosBasicos()
        {
            var comandosNotasConceitos = ServiceProvider.GetService<IComandosNotasConceitos>();

            var obterNotasParaAvaliacoesUseCase = ServiceProvider.GetService<IObterNotasParaAvaliacoesUseCase>();

            return (comandosNotasConceitos, obterNotasParaAvaliacoesUseCase);
        }

        protected async Task ExecutarNotasConceito(NotaConceitoListaDto notaconceito, ListaNotasConceitosDto listaNotaConceito, bool ehInclusao = true, bool gerarExcecao = false)
        {
            var (comandosNotasConceitos, obterNotasParaAvaliacoesUseCase) = RetornarServicosBasicos();

            if (gerarExcecao)
            {
                async Task doExecutarSalvar() { await comandosNotasConceitos.Salvar(notaconceito); }

                await Should.ThrowAsync<NegocioException>(() => doExecutarSalvar());
            }
            else
            {
                await comandosNotasConceitos.Salvar(notaconceito);

                var notasConceitoRetorno = await obterNotasParaAvaliacoesUseCase.Executar(listaNotaConceito);

                notasConceitoRetorno.AuditoriaInserido.ShouldNotBeEmpty();

                notasConceitoRetorno.Bimestres.Any().ShouldBeTrue();

                var ehNotaConceito = notaconceito.NotasConceitos.Any(f => f.Conceito.HasValue);

                if (ehInclusao)
                    ValidarInclusaoNotas(notaconceito, notasConceitoRetorno, ehNotaConceito);
                else
                    ValidarAlteracaoNotas(notaconceito, notasConceitoRetorno, ehNotaConceito);

                var notasPersistidas = ObterTodos<NotaConceito>();

                if (ehNotaConceito)
                    notasPersistidas.Any(a => a.TipoNota == TipoNota.Nota).ShouldBeFalse();
                else
                    notasPersistidas.Any(a => a.TipoNota == TipoNota.Conceito).ShouldBeFalse();
            }
        }

        private void ValidarAlteracaoNotas(NotaConceitoListaDto notaconceito, NotasConceitosRetornoDto notasConceitoRetorno, bool ehNotaConceito)
        {
            var alunosAlterados = notaconceito.NotasConceitos.Select(s => s.AlunoId).Distinct();

            foreach (var bimestre in notasConceitoRetorno.Bimestres)
            {
                bimestre.Alunos.Any().ShouldBeTrue();

                var notaConceitoAlteracao = bimestre.Alunos.Where(w => alunosAlterados.Contains(w.Id));

                (notaConceitoAlteracao.Select(s => s.Id).Distinct().Count() == notaconceito.NotasConceitos.Select(s => s.AlunoId).Distinct().Count()).ShouldBeTrue();

                foreach (var aluno in notaConceitoAlteracao)
                {
                    aluno.NotasAvaliacoes.Any().ShouldBeTrue();

                    (aluno.NotasAvaliacoes.Select(s => s.AtividadeAvaliativaId).Distinct().Count() == notaconceito.NotasConceitos.Select(s => s.AtividadeAvaliativaId).Distinct().Count()).ShouldBeTrue();

                    foreach (var notaAvaliacao in aluno.NotasAvaliacoes)
                    {
                        var notaAvaliacaoPrevista = notaconceito.NotasConceitos.FirstOrDefault(w => w.AlunoId.Equals(aluno.Id) && w.AtividadeAvaliativaId == notaAvaliacao.AtividadeAvaliativaId);

                        if (ehNotaConceito)
                            notaAvaliacao.NotaConceito.Equals(notaAvaliacaoPrevista.Conceito.ToString()).ShouldBeTrue();
                        else
                            notaAvaliacao.NotaConceito.Equals(notaAvaliacaoPrevista.Nota.ToString()).ShouldBeTrue();

                        var notasPersistidas = ObterTodos<NotaConceito>();
                    }
                }
            }
        }

        private void ValidarInclusaoNotas(NotaConceitoListaDto notaconceito, NotasConceitosRetornoDto notasConceitoRetorno, bool ehNotaConceito)
        {
            foreach (var bimestre in notasConceitoRetorno.Bimestres)
            {
                bimestre.Alunos.Any().ShouldBeTrue();

                bimestre.Avaliacoes.Any().ShouldBeTrue();

                (bimestre.Avaliacoes.Count() == notaconceito.NotasConceitos.Select(s => s.AtividadeAvaliativaId).Distinct().Count()).ShouldBeTrue();

                foreach (var aluno in bimestre.Alunos)
                {
                    aluno.NotasAvaliacoes.Any().ShouldBeTrue();

                    (aluno.NotasAvaliacoes.Count() == notaconceito.NotasConceitos.Select(s => s.AtividadeAvaliativaId).Distinct().Count()).ShouldBeTrue();

                    aluno.NotasAvaliacoes.Any().ShouldBeTrue();

                    foreach (var notaAvaliacao in aluno.NotasAvaliacoes)
                    {
                        var notaAvaliacaoPrevista = notaconceito.NotasConceitos.FirstOrDefault(w => w.AlunoId.Equals(aluno.Id) && w.AtividadeAvaliativaId == notaAvaliacao.AtividadeAvaliativaId);

                        if (ehNotaConceito)
                            notaAvaliacao.NotaConceito.Equals(notaAvaliacaoPrevista.Conceito.ToString()).ShouldBeTrue();
                        else
                            notaAvaliacao.NotaConceito.Equals(notaAvaliacaoPrevista.Nota.ToString()).ShouldBeTrue();
                    }
                }
            }
        }

        protected async Task<NotasConceitosRetornoDto> ExecutarNotasConceito(ListaNotasConceitosDto consultaListaNotasConceitosDto, NotaConceitoListaDto notaConceitoLista)
        {
            var (comandosNotasConceitos, obterNotasParaAvaliacoesUseCase) = RetornarServicosBasicos();

            await comandosNotasConceitos.Salvar(notaConceitoLista);

            return await obterNotasParaAvaliacoesUseCase.Executar(consultaListaNotasConceitosDto);
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

            await CriarParametrosNotas();

            await CriarAbrangencia(filtroNota.Perfil);
            
            await CriarCiclo();

            await CriarNotasTipoEParametros(filtroNota.ConsiderarAnoAnterior);
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
        }

        protected async Task CriarPeriodoEscolarEAbertura()
        {
            await CriarPeriodoEscolar(DATA_01_01_INICIO_BIMESTRE_1, DATA_01_05_FIM_BIMESTRE_1, BIMESTRE_1);

            await CriarPeriodoEscolar(DATA_02_05_INICIO_BIMESTRE_2, DATA_24_07_FIM_BIMESTRE_2, BIMESTRE_2);

            await CriarPeriodoEscolar(DATA_25_07_INICIO_BIMESTRE_3, DATA_02_10_FIM_BIMESTRE_3, BIMESTRE_3);

            await CriarPeriodoEscolar(DATA_03_10_INICIO_BIMESTRE_4, DATA_22_12_FIM_BIMESTRE_4, BIMESTRE_4);

            await CriarPeriodoReabertura(TIPO_CALENDARIO_1);
        }

        protected async Task CriarPeriodoEscolar(bool considerarAnoAnterior = false)
        {
            await CriarPeriodoEscolar(DATA_01_01_INICIO_BIMESTRE_1, DATA_01_05_FIM_BIMESTRE_1, BIMESTRE_1, TIPO_CALENDARIO_1, considerarAnoAnterior);

            await CriarPeriodoEscolar(DATA_02_05_INICIO_BIMESTRE_2, DATA_24_07_FIM_BIMESTRE_2, BIMESTRE_2, TIPO_CALENDARIO_1, considerarAnoAnterior);

            await CriarPeriodoEscolar(DATA_25_07_INICIO_BIMESTRE_3, DATA_02_10_FIM_BIMESTRE_3, BIMESTRE_3, TIPO_CALENDARIO_1, considerarAnoAnterior);

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
            await CriarPeriodoEscolar(DATA_01_01_INICIO_BIMESTRE_1, DATA_01_05_FIM_BIMESTRE_1, BIMESTRE_1);

            await CriarPeriodoEscolar(DATA_02_05_INICIO_BIMESTRE_2, DATA_24_07_FIM_BIMESTRE_2, BIMESTRE_2);

            await CriarPeriodoEscolar(DATA_25_07_INICIO_BIMESTRE_3, DATA_02_10_FIM_BIMESTRE_3, BIMESTRE_3);

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
        }
    }
}