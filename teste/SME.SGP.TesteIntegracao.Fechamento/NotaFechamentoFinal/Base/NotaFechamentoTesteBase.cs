using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.NotaFechamentoFinal.Base
{
    public abstract class NotaFechamentoTesteBase : TesteBaseComuns
    {
        protected const string CODIGO_ALUNO_99999 = "99999";

        protected readonly double NOTA_1 = 1;
        protected readonly double NOTA_2 = 2;
        protected readonly double NOTA_3 = 3;
        protected readonly double NOTA_4 = 4;
        protected readonly double NOTA_5 = 5;
        protected readonly double NOTA_6 = 6;
        protected readonly double NOTA_7 = 7;
        protected readonly double NOTA_8 = 8;
        protected readonly double NOTA_9 = 9;
        protected readonly double NOTA_10 = 10;

        protected const string PLENAMENTE_SATISFATORIO = "P";
        protected const string SATISFATORIO = "S";
        protected const string NAO_SATISFATORIO = "NS";

        protected const long PERIODO_ESCOLAR_CODIGO_1 = 1;
        protected const long PERIODO_ESCOLAR_CODIGO_2 = 2;
        protected const long PERIODO_ESCOLAR_CODIGO_3 = 3;
        protected const long PERIODO_ESCOLAR_CODIGO_4 = 4;

        protected readonly long FECHAMENTO_TURMA_ID_1 = 1;
        protected const long FECHAMENTO_TURMA_ID_2 = 2;
        protected const long FECHAMENTO_TURMA_ID_3 = 3;
        protected const long FECHAMENTO_TURMA_ID_4 = 4;
        protected readonly long FECHAMENTO_TURMA_DISCIPLINA_ID_1 = 1;
        protected const long FECHAMENTO_TURMA_DISCIPLINA_ID_2 = 2;
        protected const long FECHAMENTO_TURMA_DISCIPLINA_ID_3 = 3;
        protected const long FECHAMENTO_TURMA_DISCIPLINA_ID_4 = 4;
        protected const long FECHAMENTO_TURMA_DISCIPLINA_ID_5 = 5;
        protected readonly long FECHAMENTO_ALUNO_ID_1 = 1;
        protected readonly long FECHAMENTO_ALUNO_ID_2 = 2;
        protected readonly long FECHAMENTO_ALUNO_ID_3 = 3;
        protected readonly long FECHAMENTO_ALUNO_ID_4 = 4;
        protected readonly long FECHAMENTO_ALUNO_ID_5 = 5;
        protected const long FECHAMENTO_ALUNO_ID_6 = 6;
        protected const long FECHAMENTO_ALUNO_ID_7 = 7;
        protected const long FECHAMENTO_ALUNO_ID_8 = 8;
        protected const long FECHAMENTO_ALUNO_ID_9 = 9;
        protected const long FECHAMENTO_ALUNO_ID_10 = 10;
        protected readonly string NOTA = "NOTA";
        protected readonly string CONCEITO = "CONCEITO";

        protected NotaFechamentoTesteBase(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ConsolidacaoNotaAlunoCommand, bool>), typeof(ConsolidacaoNotaAlunoCommandHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterProfessoresTitularesDisciplinasEolQuery, IEnumerable<ProfessorTitularDisciplinaEol>>), typeof(ObterProfessoresTitularesDisciplinasEolQueryHandlerFakePortugues), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesCurricularesEOLPorTurmasCodigoQuery, IEnumerable<ComponenteCurricularEol>>), typeof(ServicosFakes.ObterComponentesCurricularesEOLPorTurmasCodigoQueryHandlerFake), ServiceLifetime.Scoped));

            base.RegistrarFakes(services);
        }

        private IComandosFechamentoFinal RetornarServicosBasicos()
        {
            return ServiceProvider.GetService<IComandosFechamentoFinal>();
        }

        private IConsultasFechamentoTurmaDisciplina RetornarServicoConsultasFechamentoTurmaDisciplina()
        {
            return ServiceProvider.GetService<IConsultasFechamentoTurmaDisciplina>();
        }

        private IComandosFechamentoTurmaDisciplina RetornarServicoComandosFechamentoTurmaDisciplina()
        {
            return ServiceProvider.GetService<IComandosFechamentoTurmaDisciplina>();
        }

        private IMediator RetornarServicoMediator()
        {
            return ServiceProvider.GetService<IMediator>();
        }

        protected async Task<AuditoriaPersistenciaDto> ExecutarComandosFechamentoFinal(FechamentoFinalSalvarDto fechamentoFinalSalvarDto)
        {
            var comandosFechamentoFinal = RetornarServicosBasicos();

            var retorno = await comandosFechamentoFinal.SalvarAsync(fechamentoFinalSalvarDto);

            retorno.ShouldNotBeNull();

            retorno.Mensagens.Any().ShouldBeFalse();

            (retorno.MensagemConsistencia.Length > 0).ShouldBeTrue();

            return retorno;
        }

        protected async Task ExecutarComandosFechamentoTurmaDisciplina(long fechamentoId)
        {
            var comandosFechamentoTurmaDisciplina = RetornarServicoComandosFechamentoTurmaDisciplina();

            await comandosFechamentoTurmaDisciplina.Reprocessar(fechamentoId);
        }

        protected async Task<FechamentoTurmaDisciplinaBimestreDto> ExecutarConsultasFechamentoTurmaDisciplinaComValidacaoAluno(FiltroNotaFechamentoAlunosDto fechamentoFinalSalvarAlunoDto)
        {
            var consultasFechamentoTurmaDisciplina = RetornarServicoConsultasFechamentoTurmaDisciplina();

            var retorno = await consultasFechamentoTurmaDisciplina.ObterNotasFechamentoTurmaDisciplina(
                fechamentoFinalSalvarAlunoDto.TurmaCodigo, fechamentoFinalSalvarAlunoDto.DisciplinaCodigo,
                fechamentoFinalSalvarAlunoDto.Bimestre, fechamentoFinalSalvarAlunoDto.Semestre);

            retorno.ShouldNotBeNull();

            return retorno;
        }

        protected async Task<NegocioException> ExecutarComandosFechamentoFinalComExcecao(FechamentoFinalSalvarDto fechamentoFinalSalvarDto)
        {
            var comandosFechamentoFinal = RetornarServicosBasicos();

            return await Assert.ThrowsAsync<NegocioException>(async () => await comandosFechamentoFinal.SalvarAsync(fechamentoFinalSalvarDto));
        }

        protected async Task ExecutarComandosFechamentoFinalComValidacaoNota(FechamentoFinalSalvarDto fechamentoFinalSalvarDto)
        {
            var comandosFechamentoFinal = RetornarServicosBasicos();

            var retorno = await comandosFechamentoFinal.SalvarAsync(fechamentoFinalSalvarDto);

            retorno.ShouldNotBeNull();

            retorno.Mensagens.Any().ShouldBeFalse();

            (retorno.MensagemConsistencia.Length > 0).ShouldBeTrue();

            var turmaFechamento = ObterTodos<FechamentoTurma>();
            turmaFechamento.ShouldNotBeNull();
            turmaFechamento.FirstOrDefault().TurmaId.ShouldBe(long.Parse(fechamentoFinalSalvarDto.TurmaCodigo));

            var turmaFechamentoDiciplina = ObterTodos<FechamentoTurmaDisciplina>();
            turmaFechamentoDiciplina.ShouldNotBeNull();
            turmaFechamentoDiciplina.FirstOrDefault().DisciplinaId.ShouldBe(long.Parse(fechamentoFinalSalvarDto.DisciplinaId));

            var fechamentosAlunos = ObterTodos<FechamentoAluno>();
            fechamentosAlunos.ShouldNotBeNull();
            (ObterFechamentoAlunosInseridos(fechamentosAlunos).Except(ObterAlunosDto(fechamentoFinalSalvarDto))).Count().ShouldBe(0);
            (ObterAlunosDto(fechamentoFinalSalvarDto).Except(ObterFechamentoAlunosInseridos(fechamentosAlunos))).Count().ShouldBe(0);

            var fechamentosNotas = ObterTodos<FechamentoNota>();
            fechamentosNotas.ShouldNotBeNull();
            (fechamentosNotas.Select(s => s.Nota).Except(fechamentoFinalSalvarDto.Itens.Select(f => f.Nota))).Count().ShouldBe(0);
            (fechamentoFinalSalvarDto.Itens.Select(f => f.Nota).Except(fechamentosNotas.Select(s => s.Nota))).Count().ShouldBe(0);

            foreach (var fechamentoNota in fechamentosNotas)
            {
                var alunoRf = fechamentosAlunos.FirstOrDefault(f => f.Id == fechamentoNota.FechamentoAlunoId).AlunoCodigo;
                var proposta = ObterFechamentoNotaDto(fechamentoFinalSalvarDto, alunoRf);

                if (fechamentoNota.Nota.HasValue)
                {
                    var atual = fechamentoNota.Nota;
                    (proposta.Nota == atual).ShouldBeTrue();
                }
                else
                {
                    var conceitoId = fechamentoNota.ConceitoId;
                    (proposta.ConceitoId == conceitoId).ShouldBeTrue();
                }
            }

            var listaConsolidacaoTurmaAluno = ObterTodos<ConselhoClasseConsolidadoTurmaAluno>();
            listaConsolidacaoTurmaAluno.ShouldNotBeNull();
            listaConsolidacaoTurmaAluno.Select(s => s.AlunoCodigo).Distinct().Except(ObterAlunosDto(fechamentoFinalSalvarDto)).Count().ShouldBe(0);
            ObterAlunosDto(fechamentoFinalSalvarDto).Except(listaConsolidacaoTurmaAluno.Select(s => s.AlunoCodigo).Distinct()).Count().ShouldBe(0);

            var listaConsolidacaoTurmaAlunoNota = ObterTodos<ConselhoClasseConsolidadoTurmaAlunoNota>();
            listaConsolidacaoTurmaAlunoNota.ShouldNotBeNull();

            foreach (var consolidacaoTurmaAlunoNota in listaConsolidacaoTurmaAlunoNota.Where(w => w.ComponenteCurricularId == long.Parse(fechamentoFinalSalvarDto.DisciplinaId)))
            {
                var alunoRf = listaConsolidacaoTurmaAluno.FirstOrDefault(f => f.Id == consolidacaoTurmaAlunoNota.ConselhoClasseConsolidadoTurmaAlunoId).AlunoCodigo;
                var proposta = ObterFechamentoNotaDto(fechamentoFinalSalvarDto, alunoRf);

                if (consolidacaoTurmaAlunoNota.Nota.HasValue)
                {
                    var atual = consolidacaoTurmaAlunoNota.Nota;
                    (proposta.Nota == atual).ShouldBeTrue();
                }
                else
                {
                    var conceitoId = consolidacaoTurmaAlunoNota.ConceitoId;
                    (proposta.ConceitoId == conceitoId).ShouldBeTrue();
                }
            }

        }

        private IEnumerable<string> ObterAlunosDto(FechamentoFinalSalvarDto fechamentoFinalSalvarDto)
        {
            return fechamentoFinalSalvarDto.Itens.Select(d => d.AlunoRf).Distinct();
        }

        private IEnumerable<string> ObterFechamentoAlunosInseridos(List<FechamentoAluno> fechamentosAlunos)
        {
            return fechamentosAlunos.Select(s => s.AlunoCodigo).Distinct();
        }

        private FechamentoFinalSalvarItemDto ObterFechamentoNotaDto(FechamentoFinalSalvarDto fechamentoFinalSalvarDto, string alunoCodigo)
        {
            var retorno = fechamentoFinalSalvarDto.Itens.FirstOrDefault(f => f.AlunoRf.Equals(alunoCodigo));
            return retorno;
        }

        protected async Task CriarDadosBase(FiltroNotaFechamentoDto filtroNotaFechamentoDto)
        {
            await CriarDreUePerfilComponenteCurricular();

            CriarClaimUsuario(filtroNotaFechamentoDto.Perfil);

            await CriarUsuarios();

            await CriarTurmaTipoCalendario(filtroNotaFechamentoDto);

            if (filtroNotaFechamentoDto.CriarPeriodoEscolar)
                await CriarPeriodoEscolar(filtroNotaFechamentoDto.ConsiderarAnoAnterior);

            if (filtroNotaFechamentoDto.CriarPeriodoAbertura)
                await CriarPeriodoAbertura(filtroNotaFechamentoDto);

            await CriarParametrosNotas();

            await CriarAbrangencia(filtroNotaFechamentoDto.Perfil);

            await CriarCiclo();

            await CriarNotasTipoEParametros(filtroNotaFechamentoDto.ConsiderarAnoAnterior);

            await CrieConceitoValores();

            await CriarParametrosSistema();
        }

        protected async Task CriarTurmaTipoCalendario(FiltroNotaFechamentoDto filtroNota)
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
                Tipo = TipoParametroSistema.AprovacaoAlteracaoNotaFechamento,
                Ano = dataAtualAnoAnterior.Year,
                Valor = string.Empty,
                Ativo = true,
                CriadoEm = dataAtualAnoAtual,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoPor = SISTEMA_NOME
            });

        }

        protected async Task CriarPeriodoEscolarEAbertura()
        {
            await CriarPeriodoEscolar(DATA_03_01_INICIO_BIMESTRE_1, DATA_01_05_FIM_BIMESTRE_1, BIMESTRE_1);

            await CriarPeriodoEscolar(DATA_02_05_INICIO_BIMESTRE_2, DATA_24_07_FIM_BIMESTRE_2, BIMESTRE_2);

            await CriarPeriodoEscolar(DATA_25_07_INICIO_BIMESTRE_3, DATA_02_10_FIM_BIMESTRE_3, BIMESTRE_3);

            await CriarPeriodoEscolar(DATA_03_10_INICIO_BIMESTRE_4, DATA_22_12_FIM_BIMESTRE_4, BIMESTRE_4);

            await CriarPeriodoReabertura(TIPO_CALENDARIO_1);
        }

        protected async Task CriarPeriodoEscolar(bool considerarAnoAnterior = false)
        {
            await CriarPeriodoEscolar(DATA_03_01_INICIO_BIMESTRE_1, DATA_01_05_FIM_BIMESTRE_1, BIMESTRE_1, TIPO_CALENDARIO_1, considerarAnoAnterior);

            await CriarPeriodoEscolar(DATA_02_05_INICIO_BIMESTRE_2, DATA_24_07_FIM_BIMESTRE_2, BIMESTRE_2, TIPO_CALENDARIO_1, considerarAnoAnterior);

            await CriarPeriodoEscolar(DATA_25_07_INICIO_BIMESTRE_3, DATA_02_10_FIM_BIMESTRE_3, BIMESTRE_3, TIPO_CALENDARIO_1, considerarAnoAnterior);

            await CriarPeriodoEscolar(DATA_03_10_INICIO_BIMESTRE_4, DATA_22_12_FIM_BIMESTRE_4, BIMESTRE_4, TIPO_CALENDARIO_1, considerarAnoAnterior);
        }

        protected async Task CriarPeriodoAbertura(FiltroNotaFechamentoDto filtroNotasDto)
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

        protected async Task CriarPeriodoEscolarEAberturaPadrao()
        {
            await CriarPeriodoEscolar(DATA_03_01_INICIO_BIMESTRE_1, DATA_01_05_FIM_BIMESTRE_1, BIMESTRE_1);

            await CriarPeriodoEscolar(DATA_02_05_INICIO_BIMESTRE_2, DATA_24_07_FIM_BIMESTRE_2, BIMESTRE_2);

            await CriarPeriodoEscolar(DATA_25_07_INICIO_BIMESTRE_3, DATA_02_10_FIM_BIMESTRE_3, BIMESTRE_3);

            await CriarPeriodoEscolar(DATA_03_10_INICIO_BIMESTRE_4, DATA_22_12_FIM_BIMESTRE_4, BIMESTRE_4);

            await CriarPeriodoReabertura(TIPO_CALENDARIO_1);
        }

        protected async Task CriarPeriodoReabertura(long tipoCalendarioId, bool considerarAnoAnterior = false)
        {
            await InserirNaBase(new FechamentoReabertura()
            {
                Descricao = REABERTURA_GERAL,
                Inicio = considerarAnoAnterior ? DateTimeExtension.HorarioBrasilia().AddYears(-1).Date : DateTimeExtension.HorarioBrasilia().Date,
                Fim = DateTimeExtension.HorarioBrasilia().AddYears(1).Date,
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

        protected async Task ExecuteTesteConceitoInsercao(string perfil, long componenteCurricular, Modalidade modalidade, ModalidadeTipoCalendario modalidadeTipoCalendario, bool ehRegencia, TipoNota tipoNota)
        {
            await CriarDadosBase(ObterFiltroNotas(perfil, ANO_3, componenteCurricular.ToString(), tipoNota, modalidade, modalidadeTipoCalendario, false));

            await ExecutarComandosFechamentoFinalComValidacaoNota(ObterFechamentoFinalConceitoDto(componenteCurricular, ehRegencia));
        }

        protected async Task ExecuteTesteConceitoAlteracao(string perfil, long componenteCurricular, Modalidade modalidade, ModalidadeTipoCalendario modalidadeTipoCalendario, bool ehRegencia, TipoNota tipoNota)
        {
            await CriarDadosBase(ObterFiltroNotas(perfil, ANO_3, componenteCurricular.ToString(), tipoNota, modalidade, modalidadeTipoCalendario, false));

            await ExecutarComandosFechamentoFinal(ObterFechamentoFinalConceitoDto(componenteCurricular, ehRegencia));

            var dto = new FechamentoFinalSalvarDto()
            {
                DisciplinaId = componenteCurricular.ToString(),
                TurmaCodigo = TURMA_CODIGO_1,
                EhRegencia = ehRegencia,
                Itens = new List<FechamentoFinalSalvarItemDto>()
                {
                    new FechamentoFinalSalvarItemDto()
                    {
                        AlunoRf = ALUNO_CODIGO_1,
                        ComponenteCurricularCodigo = componenteCurricular,
                        ConceitoId = (int)ConceitoValores.P
                    },
                    new FechamentoFinalSalvarItemDto()
                    {
                        AlunoRf = ALUNO_CODIGO_2,
                        ComponenteCurricularCodigo = componenteCurricular,
                        ConceitoId = (int)ConceitoValores.NS
                    },
                    new FechamentoFinalSalvarItemDto()
                    {
                        AlunoRf = ALUNO_CODIGO_3,
                        ComponenteCurricularCodigo = componenteCurricular,
                        ConceitoId = (int)ConceitoValores.S
                    },
                }
            };

            await ExecutarComandosFechamentoFinalComValidacaoNota(dto);
        }

        protected FiltroNotaFechamentoDto ObterFiltroNotas(
                                string perfil,
                                string anoTurma,
                                string componenteCurricular,
                                TipoNota tipoNota,
                                Modalidade modalidade,
                                ModalidadeTipoCalendario modalidadeTipoCalendario,
                                bool anoAnterior)
        {
            return new FiltroNotaFechamentoDto()
            {
                Perfil = perfil,
                Modalidade = modalidade,
                TipoCalendario = modalidadeTipoCalendario,
                Bimestre = BIMESTRE_1,
                ComponenteCurricular = componenteCurricular,
                TipoCalendarioId = TIPO_CALENDARIO_1,
                CriarPeriodoEscolar = true,
                CriarPeriodoAbertura = true,
                TipoNota = tipoNota,
                AnoTurma = anoTurma,
                ConsiderarAnoAnterior = anoAnterior
            };
        }

        protected FechamentoFinalSalvarDto ObterFechamentoFinalConceitoDto(long componenteCurricular, bool ehRegencia)
        {
            return new FechamentoFinalSalvarDto()
            {
                DisciplinaId = componenteCurricular.ToString(),
                TurmaCodigo = TURMA_CODIGO_1,
                EhRegencia = ehRegencia,
                Itens = new List<FechamentoFinalSalvarItemDto>()
                {
                    new ()
                    {
                        AlunoRf = ALUNO_CODIGO_1,
                        ComponenteCurricularCodigo = componenteCurricular,
                        ConceitoId = (int)ConceitoValores.NS
                    },
                    new ()
                    {
                        AlunoRf = ALUNO_CODIGO_2,
                        ComponenteCurricularCodigo = componenteCurricular,
                        ConceitoId = (int)ConceitoValores.S
                    },
                    new ()
                    {
                        AlunoRf = ALUNO_CODIGO_3,
                        ComponenteCurricularCodigo = componenteCurricular,
                        ConceitoId = (int)ConceitoValores.P
                    },
                }
            };
        }

        private async Task CrieConceitoValores()
        {

            await InserirNaBase(new Conceito()
            {
                Valor = PLENAMENTE_SATISFATORIO,
                InicioVigencia = DATA_01_01,
                Ativo = true,
                Descricao = ConceitoValores.P.GetAttribute<DisplayAttribute>().Name,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });
            await InserirNaBase(new Conceito()
            {
                Valor = SATISFATORIO,
                InicioVigencia = DATA_01_01,
                Ativo = true,
                Descricao = ConceitoValores.S.GetAttribute<DisplayAttribute>().Name,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });
            await InserirNaBase(new Conceito()
            {
                Valor = NAO_SATISFATORIO,
                InicioVigencia = DATA_01_01,
                Ativo = true,
                Descricao = ConceitoValores.NS.GetAttribute<DisplayAttribute>().Name,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });
        }

        private async Task CriarParametrosSistema()
        {
            await InserirNaBase(new ParametrosSistema
            {
                Nome = "AprovacaoAlteracaoNotaConselho",
                Tipo = TipoParametroSistema.AprovacaoAlteracaoNotaConselho,
                Descricao = "Aprovação alteracao nota conselho",
                Valor = string.Empty,
                Ano = DateTimeExtension.HorarioBrasilia().AddYears(-1).Year,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                Ativo = true
            });
        }

        protected class FiltroNotaFechamentoDto
        {
            public FiltroNotaFechamentoDto()
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
            public bool EhRegencia { get; set; }
        }

        protected class FiltroNotaFechamentoAlunosDto
        {
            public FiltroNotaFechamentoAlunosDto()
            { }

            public string TurmaCodigo { get; set; }
            public long DisciplinaCodigo { get; set; }
            public int Bimestre { get; set; }
            public int Semestre { get; set; }
        }

        protected async Task ExecutarTesteComandosFechamentoTurmaDisciplina(FiltroNotaFechamentoDto filtroNotaFechamentoDto)
        {
            await CriarDadosBase(filtroNotaFechamentoDto);

            await CriarAula(DATA_28_04, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, filtroNotaFechamentoDto.ComponenteCurricular, TIPO_CALENDARIO_1);

            await InserirFechamentoAluno(filtroNotaFechamentoDto);

            await ExecutarComandosFechamentoTurmaDisciplina(NUMERO_1);

            await ValidarSituacaoFechamentoPorTipo(SituacaoFechamento.EmProcessamento);
        }

        protected async Task ValidarSituacaoFechamentoPorTipo(SituacaoFechamento situacaoTipo)
        {
            var fechamentoTurmaDisciplina = ObterTodos<FechamentoTurmaDisciplina>();
            (fechamentoTurmaDisciplina.FirstOrDefault().Situacao == situacaoTipo).ShouldBeTrue();
            (fechamentoTurmaDisciplina.FirstOrDefault().Situacao != situacaoTipo).ShouldBeFalse();
        }

        protected async Task ExecutarTesteGerarPendenciasFechamentoCommand(FiltroNotaFechamentoDto filtroNotaFechamentoDto)
        {
            await CriarDadosBase(filtroNotaFechamentoDto);

            var dataAula = (DATA_28_04 > DateTimeExtension.HorarioBrasilia()) ? DateTimeExtension.HorarioBrasilia().Date : DATA_28_04;
            await CriarAula(dataAula, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, filtroNotaFechamentoDto.ComponenteCurricular, TIPO_CALENDARIO_1);

            await InserirFechamentoAluno(filtroNotaFechamentoDto);

            var servicoMediator = RetornarServicoMediator();

            var usuarioLogado = ObterTodos<Usuario>().FirstOrDefault();

            var fechamentoDto = new FechamentoTurmaDisciplinaPendenciaDto()
            {
                DisciplinaId = long.Parse(filtroNotaFechamentoDto.ComponenteCurricular),
                CodigoTurma = TURMA_CODIGO_1,
                NomeTurma = TURMA_NOME_1,
                PeriodoInicio = DATA_03_01_INICIO_BIMESTRE_1,
                PeriodoFim = DATA_01_05_FIM_BIMESTRE_1,
                Bimestre = BIMESTRE_1,
                UsuarioId = usuarioLogado.Id,
                Id = NUMERO_1,
                Justificativa = string.Empty,
                CriadoRF = SISTEMA_CODIGO_RF,
                TurmaId = TURMA_ID_1
            };

            var commando = new IncluirFilaGeracaoPendenciasFechamentoCommand(fechamentoDto, true);

            await servicoMediator.Send(commando);

            await ValidarResultadosTesteComPendencias();
        }

        private async Task ValidarResultadosTesteComPendencias()
        {
            await ValidarSituacaoFechamentoPorTipo(SituacaoFechamento.ProcessadoComPendencias);

            var pendencias = ObterTodos<Pendencia>();
            pendencias.Any(a =>
                    a.Tipo == TipoPendencia.AulasSemFrequenciaNaDataDoFechamento && a.Situacao == SituacaoPendencia.Pendente)
                .ShouldBeTrue();
            pendencias.Any(a =>
                    a.Tipo == TipoPendencia.AulasSemFrequenciaNaDataDoFechamento && a.Situacao != SituacaoPendencia.Pendente)
                .ShouldBeFalse();

            pendencias.Any(a =>
                    a.Tipo == TipoPendencia.AulasSemPlanoAulaNaDataDoFechamento && a.Situacao == SituacaoPendencia.Pendente)
                .ShouldBeTrue();
            pendencias.Any(a =>
                    a.Tipo == TipoPendencia.AulasSemPlanoAulaNaDataDoFechamento && a.Situacao != SituacaoPendencia.Pendente)
                .ShouldBeFalse();

            await InserirNaBase(new RegistroFrequencia
            {
                AulaId = 1,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            await InserirNaBase(new RegistroFrequenciaAluno
            {
                CodigoAluno = CODIGO_ALUNO_1,
                RegistroFrequenciaId = 1,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now,
                Valor = 1,
                NumeroAula = 1,
                AulaId = 1
            });

            await InserirNaBase(new Dominio.PlanoAula()
            {
                AulaId = 1,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            await ExecutarComandosFechamentoTurmaDisciplina(NUMERO_1);

            await ValidarSituacaoFechamentoPorTipo(SituacaoFechamento.ProcessadoComSucesso);

            pendencias = ObterTodos<Pendencia>();
            pendencias.Any(a =>
                    a.Tipo == TipoPendencia.AulasSemFrequenciaNaDataDoFechamento && a.Situacao == SituacaoPendencia.Resolvida)
                .ShouldBeTrue();
            pendencias.Any(a =>
                    a.Tipo == TipoPendencia.AulasSemFrequenciaNaDataDoFechamento && a.Situacao != SituacaoPendencia.Resolvida)
                .ShouldBeFalse();

            pendencias.Any(a =>
                    a.Tipo == TipoPendencia.AulasSemPlanoAulaNaDataDoFechamento && a.Situacao == SituacaoPendencia.Resolvida)
                .ShouldBeTrue();
            pendencias.Any(a =>
                    a.Tipo == TipoPendencia.AulasSemPlanoAulaNaDataDoFechamento && a.Situacao != SituacaoPendencia.Resolvida)
                .ShouldBeFalse();
        }

        private async Task InserirFechamentoAluno(FiltroNotaFechamentoDto filtroNotaFechamentoDto)
        {
            await InserirNaBase(new FechamentoTurma()
            {
                TurmaId = TURMA_ID_1,
                PeriodoEscolarId = PERIODO_ESCOLAR_CODIGO_1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new FechamentoTurmaDisciplina()
            {
                DisciplinaId = long.Parse(filtroNotaFechamentoDto.ComponenteCurricular),
                FechamentoTurmaId = FECHAMENTO_TURMA_ID_1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new FechamentoAluno()
            {
                FechamentoTurmaDisciplinaId = FECHAMENTO_TURMA_DISCIPLINA_ID_1,
                AlunoCodigo = CODIGO_ALUNO_1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new FechamentoNota()
            {
                DisciplinaId = long.Parse(filtroNotaFechamentoDto.ComponenteCurricular),
                FechamentoAlunoId = FECHAMENTO_ALUNO_ID_1,
                Nota = NOTA_8,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new FechamentoAluno()
            {
                FechamentoTurmaDisciplinaId = FECHAMENTO_TURMA_DISCIPLINA_ID_1,
                AlunoCodigo = CODIGO_ALUNO_2,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new FechamentoNota()
            {
                DisciplinaId = long.Parse(filtroNotaFechamentoDto.ComponenteCurricular),
                FechamentoAlunoId = FECHAMENTO_ALUNO_ID_2,
                Nota = NOTA_7,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new FechamentoAluno()
            {
                FechamentoTurmaDisciplinaId = FECHAMENTO_TURMA_DISCIPLINA_ID_1,
                AlunoCodigo = CODIGO_ALUNO_3,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new FechamentoNota()
            {
                DisciplinaId = long.Parse(filtroNotaFechamentoDto.ComponenteCurricular),
                FechamentoAlunoId = FECHAMENTO_ALUNO_ID_3,
                Nota = NOTA_6,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new FechamentoAluno()
            {
                FechamentoTurmaDisciplinaId = FECHAMENTO_TURMA_DISCIPLINA_ID_1,
                AlunoCodigo = CODIGO_ALUNO_4,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new FechamentoNota()
            {
                DisciplinaId = long.Parse(filtroNotaFechamentoDto.ComponenteCurricular),
                FechamentoAlunoId = FECHAMENTO_ALUNO_ID_4,
                Nota = NOTA_5,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new FechamentoAluno()
            {
                FechamentoTurmaDisciplinaId = FECHAMENTO_TURMA_DISCIPLINA_ID_1,
                AlunoCodigo = CODIGO_ALUNO_5,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new FechamentoNota()
            {
                DisciplinaId = long.Parse(filtroNotaFechamentoDto.ComponenteCurricular),
                FechamentoAlunoId = FECHAMENTO_ALUNO_ID_5,
                Nota = NOTA_7,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        protected FiltroNotaFechamentoDto ObterFiltroNotasFechamento(string perfil, TipoNota tipoNota, string anoTurma, Modalidade modalidade, ModalidadeTipoCalendario modalidadeTipoCalendario, string componenteCurricular, bool considerarAnoAnterior = false, bool ehRegencia = false)
        {
            return new FiltroNotaFechamentoDto()
            {
                Perfil = perfil,
                Modalidade = modalidade,
                TipoCalendario = modalidadeTipoCalendario,
                Bimestre = BIMESTRE_1,
                ComponenteCurricular = componenteCurricular,
                TipoCalendarioId = TIPO_CALENDARIO_1,
                CriarPeriodoEscolar = true,
                CriarPeriodoAbertura = true,
                TipoNota = tipoNota,
                AnoTurma = anoTurma,
                ConsiderarAnoAnterior = considerarAnoAnterior,
                ProfessorRf = USUARIO_PROFESSOR_LOGIN_2222222,
                EhRegencia = ehRegencia
            };
        }
    }
}