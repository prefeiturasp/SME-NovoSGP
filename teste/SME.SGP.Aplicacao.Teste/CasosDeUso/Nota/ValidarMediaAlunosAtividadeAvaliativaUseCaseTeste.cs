using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Nota
{
    public class ValidarMediaAlunosAtividadeAvaliativaUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IRepositorioConceitoConsulta> _repositorioConceitoMock;
        private readonly Mock<IRepositorioNotaParametro> _repositorioNotaParametroMock;
        private readonly Mock<IRepositorioAulaConsulta> _repositorioAulaMock;
        private readonly Mock<IRepositorioNotaTipoValorConsulta> _repositorioNotaTipoValorMock;
        private readonly Mock<IRepositorioPeriodoEscolarConsulta> _repositorioPeriodoEscolarMock;
        private readonly Mock<IRepositorioCiclo> _repositorioCicloMock;
        private readonly Mock<IServicoUsuario> _servicoUsuarioMock;

        private readonly ValidarMediaAlunosAtividadeAvaliativaUseCase _useCase;

        public ValidarMediaAlunosAtividadeAvaliativaUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _repositorioConceitoMock = new Mock<IRepositorioConceitoConsulta>();
            _repositorioNotaParametroMock = new Mock<IRepositorioNotaParametro>();
            _repositorioAulaMock = new Mock<IRepositorioAulaConsulta>();
            _repositorioNotaTipoValorMock = new Mock<IRepositorioNotaTipoValorConsulta>();
            _repositorioPeriodoEscolarMock = new Mock<IRepositorioPeriodoEscolarConsulta>();
            _repositorioCicloMock = new Mock<IRepositorioCiclo>();
            _servicoUsuarioMock = new Mock<IServicoUsuario>();

            _useCase = new ValidarMediaAlunosAtividadeAvaliativaUseCase(
                _mediatorMock.Object,
                _repositorioConceitoMock.Object,
                _repositorioNotaParametroMock.Object,
                _repositorioAulaMock.Object,
                _repositorioNotaTipoValorMock.Object,
                _repositorioPeriodoEscolarMock.Object,
                _repositorioCicloMock.Object,
                _servicoUsuarioMock.Object);
        }

        private static MensagemRabbit CriarMensagemRabbit(FiltroValidarMediaAlunosAtividadeAvaliativaDto filtro)
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(filtro);
            return new MensagemRabbit(json);
        }

        private static AtividadeAvaliativa CriarAtividadeAvaliativa(string turmaId = "T1", string nome = "Ativ 1", DateTime? data = null, long id = 1, string dreId = "DRE1", string ueId = "UE1")
        {
            return new AtividadeAvaliativa
            {
                Id = id,
                TurmaId = turmaId,
                NomeAvaliacao = nome,
                DataAvaliacao = data ?? DateTime.Now.Date,
                DreId = dreId,
                UeId = ueId,
            };
        }

        private static Usuario CriarUsuario(string login = "usuario1", Guid? perfil = null, string nome = "Usuario Teste", string codigoRf = "RF123")
        {
            return new Usuario
            {
                Login = login,
                PerfilAtual = perfil ?? Guid.NewGuid(),
                Nome = nome,
                CodigoRf = codigoRf
            };
        }

        private static NotaParametro CriarNotaParametro(double media = 6, double minima = 0, double maxima = 10, double incremento = 0.5)
        {
            return new NotaParametro
            {
                Media = media,
                Minima = minima,
                Maxima = maxima,
                Incremento = incremento
            };
        }

        private static Conceito CriarConceito(bool aprovado = true, int id = 1)
        {
            return new Conceito
            {
                Id = id,
                Aprovado = aprovado,
                Ativo = true
            };
        }

        private static AbrangenciaFiltroRetorno CriarAbrangencia(long turmaId = 1, string nomeTurma = "Turma 1", string nomeUe = "UE 1", string nomeDre = "DRE 1", string ano = "2025", TipoTurma tipoTurma = TipoTurma.Regular)
        {
            return new AbrangenciaFiltroRetorno
            {
                TurmaId = turmaId,
                NomeTurma = nomeTurma,
                NomeUe = nomeUe,
                NomeDre = nomeDre,
                Ano = ano,
                TipoTurma = tipoTurma,
                CodigoUe = "UE1"
            };
        }

        private static PeriodoEscolar CriarPeriodoEscolar(DateTime inicio, DateTime fim, int bimestre = 1)
        {
            return new PeriodoEscolar
            {
                PeriodoInicio = inicio,
                PeriodoFim = fim,
                Bimestre = bimestre
            };
        }

        private static FuncionarioDTO CriarFuncionario(string codigoRf)
        {
            return new FuncionarioDTO
            {
                CodigoRF = codigoRf,
                Nome = "CP Teste"
            };
        }

        private static NotaTipoValor CriarNotaTipoValor()
        {
            return new NotaTipoValor();
        }

        private static NotaConceito CriarNotaConceito(double nota, long conceitoId = 1)
        {
            return new NotaConceito
            {
                Nota = nota,
                ConceitoId = conceitoId
            };
        }

        [Fact]
        public async Task Executar_Deve_Enviar_Notificacao_Quando_Quantidade_Alunos_Suficientes_Inferior()
        {
            var dataAtividade = DateTime.Now.Date;

            var atividade = new AtividadeAvaliativa
            {
                Id = 1,
                NomeAvaliacao = "Prova 1",
                DataAvaliacao = dataAtividade,
                TurmaId = "turma1",
                DreId = "dre1",
                UeId = "ue1"
            };

            var usuario = new Usuario
            {
                CodigoRf = "prof1",
                Nome = "Professor X",
                Login = "prof1login",
                PerfilAtual = Guid.NewGuid()
            };

            var notas = new List<NotaConceito>
            {
                new NotaConceito { Nota = 5, ConceitoId = null, TipoNota = TipoNota.Nota },
                new NotaConceito { Nota = 4, ConceitoId = null, TipoNota = TipoNota.Nota },
                new NotaConceito { Nota = 8, ConceitoId = null, TipoNota = TipoNota.Nota }
            };

            var filtro = new FiltroValidarMediaAlunosAtividadeAvaliativaDto(
                new[] { atividade },
                percentualAlunosInsuficientes: 50,
                chaveNotasAvaliacao: atividade.Id,
                notasPorAvaliacao: notas,
                usuario: usuario,
                disciplinaId: "disciplina1",
                hostAplicacao: "http://host/",
                temAbrangenciaUeOuDreOuSme: true
            );

            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(filtro));

            var conceitos = new List<Conceito> { new Conceito { Id = 1, Aprovado = true, Ativo = true } };
            _repositorioConceitoMock.Setup(x => x.ObterPorData(dataAtividade)).ReturnsAsync(conceitos);

            var abrangencia = new AbrangenciaFiltroRetorno
            {
                NomeTurma = "Turma 1",
                NomeUe = "UE 1",
                NomeDre = "DRE 1",
                AnoLetivo = 2025,
                CodigoUe = "ue1",
                TipoTurma = TipoTurma.Regular,
                Ano = "2025",
                Modalidade = Modalidade.Fundamental
            };
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAbrangenciaTurmaQuery>(), default)).ReturnsAsync(abrangencia);

            var notaParametro = new NotaParametro { Media = 6, Minima = 0, Maxima = 10, Incremento = 0.5 };
            _repositorioNotaParametroMock.Setup(x => x.ObterPorDataAvaliacao(dataAtividade)).ReturnsAsync(notaParametro);

            _repositorioAulaMock.Setup(x => x.ObterAulaIntervaloTurmaDisciplina(
                It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<long>()))
                .ReturnsAsync(new AulaConsultaDto { TipoCalendarioId = 1 });

            _repositorioPeriodoEscolarMock.Setup(x => x.ObterPorTipoCalendario(1)).ReturnsAsync(new[]
            {
                new PeriodoEscolar
                {
                    PeriodoInicio = dataAtividade.AddDays(-1),
                    PeriodoFim = dataAtividade.AddDays(1),
                    Bimestre = 2
                }
            });

            var funcionarios = new[] { new FuncionarioDTO { CodigoRF = "CP1" } };
            _mediatorMock.Setup(x => x.Send(It.IsAny<ObterFuncionariosPorUeECargoQuery>(), default)).ReturnsAsync(funcionarios);

            _servicoUsuarioMock.Setup(s => s.ObterUsuarioPorCodigoRfLoginOuAdiciona("CP1", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync(new Usuario { Id = 123, CodigoRf = "CP1" });

            _repositorioNotaTipoValorMock.Setup(x => x.ObterPorTurmaId(It.IsAny<long>(), It.IsAny<TipoTurma>()))
                .Returns(new NotaTipoValor { TipoNota = TipoNota.Nota });
            _repositorioCicloMock.Setup(x => x.ObterCicloPorAnoModalidade(It.IsAny<string>(), It.IsAny<Modalidade>()))
                .ReturnsAsync(new CicloDto { Id = 1 });
            _repositorioNotaTipoValorMock.Setup(x => x.ObterPorCicloIdDataAvalicacao(It.IsAny<long>(), It.IsAny<DateTime>()))
                .ReturnsAsync(new NotaTipoValor { TipoNota = TipoNota.Nota });

            _mediatorMock.Setup(x => x.Send(It.IsAny<NotificarUsuarioCommand>(), default)).ReturnsAsync(1L);

            var resultado = await _useCase.Executar(mensagem);

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(It.IsAny<NotificarUsuarioCommand>(), default), Times.Once);
        }


        [Fact]
        public async Task Executar_Nao_Deve_Enviar_Notificacao_Quando_Quantidade_Alunos_Suficiente()
        {
            var dataAvaliacao = new DateTime(2025, 4, 10);
            var atividade = CriarAtividadeAvaliativa(data: dataAvaliacao);
            var usuario = CriarUsuario();

            var notas = new List<NotaConceito>
            {
                CriarNotaConceito(8),
                CriarNotaConceito(7),
                CriarNotaConceito(9),
            };

            var filtro = new FiltroValidarMediaAlunosAtividadeAvaliativaDto(
                atividadesAvaliativas: new[] { atividade },
                percentualAlunosInsuficientes: 50,
                chaveNotasAvaliacao: atividade.Id,
                notasPorAvaliacao: notas,
                usuario: usuario,
                disciplinaId: "disciplina1",
                hostAplicacao: "http://host/",
                temAbrangenciaUeOuDreOuSme: true
            );

            var mensagem = CriarMensagemRabbit(filtro);

            var conceitos = new List<Conceito> { CriarConceito() };
            var notaParametro = CriarNotaParametro(media: 6);
            var abrangencia = CriarAbrangencia(tipoTurma: TipoTurma.Regular);
            var periodoEscolar = CriarPeriodoEscolar(dataAvaliacao.AddDays(-10), dataAvaliacao.AddDays(10), bimestre: 2);

            _repositorioConceitoMock
                .Setup(x => x.ObterPorData(It.IsAny<DateTime>()))
                .ReturnsAsync(conceitos);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterAbrangenciaTurmaQuery>(), default))
                .ReturnsAsync(abrangencia);

            _repositorioPeriodoEscolarMock.Setup(x => x.ObterPorTipoCalendario(It.IsAny<long>()))
                .ReturnsAsync(new List<PeriodoEscolar>
                {
                    new()
                    {
                        PeriodoInicio = new DateTime(2025, 4, 1),
                        PeriodoFim = new DateTime(2025, 6, 30),
                        Bimestre = 2
                    }
                });

            _repositorioNotaParametroMock
                .Setup(x => x.ObterPorDataAvaliacao(It.IsAny<DateTime>()))
                .ReturnsAsync(notaParametro);

            _repositorioAulaMock
                .Setup(x => x.ObterAulaIntervaloTurmaDisciplina(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<long>()))
                .ReturnsAsync(new AulaConsultaDto { TipoCalendarioId = 1 });

            _repositorioPeriodoEscolarMock
                .Setup(x => x.ObterPorTipoCalendario(It.IsAny<int>()))
                .ReturnsAsync(new[] { periodoEscolar });

            _repositorioNotaTipoValorMock
                .Setup(x => x.ObterPorTurmaId(It.IsAny<long>(), It.IsAny<TipoTurma>()))
                .Returns(CriarNotaTipoValor());

            _repositorioCicloMock
                .Setup(x => x.ObterCicloPorAnoModalidade(It.IsAny<string>(), It.IsAny<Modalidade>()))
                .ReturnsAsync(new CicloDto { Id = 1 });

            _repositorioNotaTipoValorMock
                .Setup(x => x.ObterPorCicloIdDataAvalicacao(It.IsAny<long>(), It.IsAny<DateTime>()))
                .ReturnsAsync(CriarNotaTipoValor());

            var resultado = await _useCase.Executar(mensagem);

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(It.IsAny<NotificarUsuarioCommand>(), default), Times.Never);
        }

        [Fact]
        public async Task Executar_Deve_Lancar_Excecao_Quando_Nao_Encontrar_Aula()
        {
            var atividade = CriarAtividadeAvaliativa(data: DateTime.Now.Date);
            var usuario = CriarUsuario();
            var notas = new List<NotaConceito>();
            var filtro = new FiltroValidarMediaAlunosAtividadeAvaliativaDto(new[] { atividade }, 50, atividade.Id, notas, usuario, "disciplina1", "http://host/", true);

            var mensagem = CriarMensagemRabbit(filtro);

            var conceitos = new List<Conceito> { CriarConceito() };
            var notaParametro = CriarNotaParametro(media: 6);
            var abrangencia = CriarAbrangencia();

            _repositorioConceitoMock.Setup(x => x.ObterPorData(It.IsAny<DateTime>())).ReturnsAsync(conceitos);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAbrangenciaTurmaQuery>(), default)).ReturnsAsync(abrangencia);

            _repositorioNotaParametroMock.Setup(x => x.ObterPorDataAvaliacao(It.IsAny<DateTime>())).ReturnsAsync(notaParametro);

            _repositorioAulaMock.Setup(x => x.ObterAulaIntervaloTurmaDisciplina(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<long>())).ReturnsAsync((AulaConsultaDto)null);
           
            _repositorioCicloMock
                .Setup(x => x.ObterCicloPorAnoModalidade(It.IsAny<string>(), It.IsAny<Modalidade>()))
                .ReturnsAsync(new CicloDto { Id = 1 });

            _repositorioNotaTipoValorMock
                .Setup(x => x.ObterPorCicloIdDataAvalicacao(It.IsAny<long>(), It.IsAny<DateTime>()))
                .ReturnsAsync(CriarNotaTipoValor());

            var ex = await Assert.ThrowsAsync<NegocioException>(async () => await _useCase.Executar(mensagem));
            Assert.Contains("Não encontrada aula para a atividade avaliativa", ex.Message);
        }

        [Fact]
        public async Task Tipo_Nota_Por_Avaliacao_Retorna_Nota_Tipo_Valor_Para_Turma_Ed_Fisica()
        {
            var atividade = CriarAtividadeAvaliativa(turmaId: "123");
            var usuario = CriarUsuario();
            var abrangencia = CriarAbrangencia(tipoTurma: TipoTurma.EdFisica);

            _repositorioNotaTipoValorMock.Setup(x => x.ObterPorTurmaId(It.IsAny<long>(), It.IsAny<TipoTurma>()))
               .Returns(new NotaTipoValor { TipoNota = TipoNota.Nota });

            var metodo = _useCase.GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.NonPublic)
                .FirstOrDefault(m =>
                    m.Name == "TipoNotaPorAvaliacao" &&
                    m.GetParameters().Length == 4);

            Assert.NotNull(metodo);

            var task = (Task<NotaTipoValor>)metodo.Invoke(_useCase, new object[] { atividade, usuario, abrangencia, false });

            var resultado = await task;

            Assert.NotNull(resultado);
            _repositorioNotaTipoValorMock.Verify(r => r.ObterPorTurmaId(It.IsAny<long>(), TipoTurma.EdFisica), Times.Once);
        }

        [Fact]
        public async Task Tipo_Nota_Por_Avaliacao_Lanca_Excecao_Quando_Nota_Tipo_Null()
        {
            var atividade = CriarAtividadeAvaliativa();
            var usuario = CriarUsuario();
            var abrangencia = CriarAbrangencia(tipoTurma: TipoTurma.Regular);

            _repositorioCicloMock.Setup(x => x.ObterCicloPorAnoModalidade(It.IsAny<string>(), It.IsAny<Modalidade>()))
                .ReturnsAsync(new CicloDto { Id = 1 });

            _repositorioNotaTipoValorMock.Setup(x => x.ObterPorCicloIdDataAvalicacao(It.IsAny<long>(), It.IsAny<DateTime>()))
                .ReturnsAsync((NotaTipoValor)null);

            var metodo = _useCase.GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.NonPublic)
                .FirstOrDefault(m =>
                    m.Name == "TipoNotaPorAvaliacao" &&
                    m.GetParameters().Length == 4);

            Assert.NotNull(metodo);

            var excecao = await Assert.ThrowsAsync<NegocioException>(async () =>
            {
                var task = (Task<NotaTipoValor>)metodo.Invoke(_useCase, new object[] { atividade, usuario, abrangencia, false });
                await task; 
            });

            Assert.Equal(MensagemNegocioNota.TIPO_NOTA_NAO_ENCONTRADO, excecao.Message);
        }

        [Fact]
        public async Task Obter_Nota_Tipo_Lanca_Excecao_Quando_Ciclo_Null()
        {
            var abrangencia = CriarAbrangencia(ano: "2025");

            _repositorioCicloMock.Setup(x => x.ObterCicloPorAnoModalidade(It.IsAny<string>(), It.IsAny<Modalidade>())).ReturnsAsync((CicloDto)null);

            var ex = await Assert.ThrowsAsync<NegocioException>(async () => await _useCase.ObterNotaTipo(abrangencia, DateTime.Now, CriarUsuario()));
            
            Assert.Equal(MensagemNegocioTurma.CICLO_TURMA_NAO_ENCONTRADO, ex.Message);
        }

        [Fact]
        public async Task Obter_Quantidade_Alunos_Suficientes_Conta_Notas_Acima_Da_Media_Caso_Tipo_Nota_Nota()
        {
            var notas = new List<NotaConceito>
            {
                new NotaConceito { Nota = 7, ConceitoId = 1 },
                new NotaConceito { Nota = 5, ConceitoId = 1 },
                new NotaConceito { Nota = 6, ConceitoId = 1 }
            };

            var parametros = CriarNotaParametro(media: 6);
            var conceitos = new List<Conceito>
            {
                CriarConceito()
            };

            var metodo = _useCase.GetType()
               .GetMethods(BindingFlags.Instance | BindingFlags.NonPublic)
               .FirstOrDefault(m =>
                   m.Name == "ObterQuantidadeAlunosSuficientes" &&
                   m.GetParameters().Length == 4);

            Assert.NotNull(metodo);

            var task = metodo.Invoke(_useCase, new object[] { notas, TipoNota.Nota, conceitos, parametros });
            var resultado = task;

            Assert.Equal(2, resultado);
        }

        [Fact]
        public void Obter_Quantidade_Alunos_Suficientes_Conta_Conceitos_Aprovados()
        {
            var notas = new List<NotaConceito>
            {
                new NotaConceito { ConceitoId = 1, Nota = 7 },  // Aprovado (>= 6)
                new NotaConceito { ConceitoId = 2, Nota = 5 },  // Reprovado (< 6)
                new NotaConceito { ConceitoId = 3, Nota = 8 }   // Aprovado (>= 6)
            };

            var parametros = CriarNotaParametro(media: 6);

            var conceitos = new List<Conceito>
            {
                new Conceito { Id = 1, Aprovado = true },
                new Conceito { Id = 2, Aprovado = false },
                new Conceito { Id = 3, Aprovado = true }
            };

            var metodo = _useCase.GetType()
               .GetMethods(BindingFlags.Instance | BindingFlags.NonPublic)
               .FirstOrDefault(m =>
                   m.Name == "ObterQuantidadeAlunosSuficientes" &&
                   m.GetParameters().Length == 4);

            Assert.NotNull(metodo);

            var resultado = metodo.Invoke(_useCase, new object[] { notas, TipoNota.Nota, conceitos, parametros });

            Assert.Equal(2, resultado);
        }

        [Fact]
        public void Quantidade_Alunos_Suficientes_Inferior_Perc_Parametrizado_Deve_Retornar_True_Quando_Inferior()
        {
            int alunosSuficientes = 1;
            int totalAlunos = 10;
            double percentual = 50;

            var metodo = _useCase.GetType()
               .GetMethods(BindingFlags.Instance | BindingFlags.NonPublic)
               .FirstOrDefault(m =>
                   m.Name == "QuantidadeAlunosSuficientesInferiorPercParametrizado" &&
                   m.GetParameters().Length == 3);

            Assert.NotNull(metodo);

            var resultado = (bool)metodo.Invoke(_useCase, new object[] { alunosSuficientes, totalAlunos, percentual });

            Assert.True(resultado);
        }

        [Fact]
        public void Quantidade_Alunos_Suficientes_Inferior_Perc_Parametrizado_Deve_Retornar_False_Quando_Superior()
        {
            int alunosSuficientes = 8;
            int totalAlunos = 10;
            double percentual = 50;

            var metodo = _useCase.GetType()
               .GetMethods(BindingFlags.Instance | BindingFlags.NonPublic)
               .FirstOrDefault(m =>
                   m.Name == "QuantidadeAlunosSuficientesInferiorPercParametrizado" &&
                   m.GetParameters().Length == 3);

            Assert.NotNull(metodo);

            var resultado = (bool)metodo.Invoke(_useCase, new object[] { alunosSuficientes, totalAlunos, percentual });

            Assert.False(resultado);
        }

        [Fact]
        public async Task Obter_Usuarios_CPs_Deve_Retornar_Usuarios()
        {
            var codigoUe = "UE1";

            var funcionarios = new List<FuncionarioDTO>
            {
                CriarFuncionario("CP1"),
                CriarFuncionario("CP2")
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterFuncionariosPorUeECargoQuery>(), default))
                .ReturnsAsync(funcionarios);

            _servicoUsuarioMock.SetupSequence(s => s.ObterUsuarioPorCodigoRfLoginOuAdiciona(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                .ReturnsAsync(CriarUsuario(codigoRf: "CP1"))
                .ReturnsAsync(CriarUsuario(codigoRf: "CP2"));

            var metodo = _useCase.GetType()
               .GetMethods(BindingFlags.Instance | BindingFlags.NonPublic)
               .FirstOrDefault(m =>
                   m.Name == "ObterUsuariosCPs" &&
                   m.GetParameters().Length == 1);

            Assert.NotNull(metodo);

            var task = (Task<IEnumerable<Usuario>>)metodo.Invoke(_useCase, new object[] { codigoUe });
            var resultado = await task;

            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count());
            Assert.Contains(resultado, u => u.CodigoRf == "CP1");
            Assert.Contains(resultado, u => u.CodigoRf == "CP2");
        }

        [Fact]
        public async Task Carrega_Usuarios_Por_RFs_Deve_Retornar_Lista_Vazia_Quando_Lista_Vazia()
        {        
            var metodo = _useCase.GetType()
               .GetMethods(BindingFlags.Instance | BindingFlags.NonPublic)
               .FirstOrDefault(m =>
                   m.Name == "CarregaUsuariosPorRFs" &&
                   m.GetParameters().Length == 1);

            Assert.NotNull(metodo);

            var task = (Task<IEnumerable<Usuario>>)metodo.Invoke(_useCase, new object[] { new List<FuncionarioDTO>() });
            var resultado = await task;

            Assert.NotNull(resultado);
            Assert.Empty(resultado);
        }
    }
}
