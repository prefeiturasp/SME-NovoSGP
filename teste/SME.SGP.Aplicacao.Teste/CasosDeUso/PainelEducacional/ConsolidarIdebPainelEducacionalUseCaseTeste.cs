using MediatR;
using Moq;
using SME.SGP.Aplicacao.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoIdeb;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterIdeb;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PainelEducacional
{
    public class ConsolidarIdebPainelEducacionalUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ConsolidarIdebPainelEducacionalUseCase useCase;

        public ConsolidarIdebPainelEducacionalUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ConsolidarIdebPainelEducacionalUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Deve_Chamar_Query_E_Salvar_Comando()
        {
            var dto = new List<PainelEducacionalIdebDto>
            {
                new PainelEducacionalIdebDto
                {
                    AnoLetivo = 2025,
                    SerieAno = 3,
                    Nota = 6.5m,
                    CriadoEm = new DateTime(2025,1,1),
                    CodigoDre = "1",
                    CodigoUe = "10"
                }
            };

            mediatorMock
                .Setup(m => m.Send(It.IsAny<PainelEducacionalIdebQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dto);

            mediatorMock
                .Setup(m => m.Send(It.IsAny<PainelEducacionalSalvarConsolidacaoIdebCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var resultado = await useCase.Executar(new MensagemRabbit("acao", "msg", Guid.NewGuid(), "123"));

            Assert.True(resultado);
            mediatorMock.Verify(m => m.Send(It.IsAny<PainelEducacionalIdebQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<PainelEducacionalSalvarConsolidacaoIdebCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public void Converter_Para_Ideb_Raw_Deve_Mapear_Corretamente()
        {
            var dtos = new List<PainelEducacionalIdebDto>
            {
                new PainelEducacionalIdebDto { AnoLetivo = 2025, SerieAno = 2, Nota = 5, CriadoEm = DateTime.Now, CodigoDre = "1", CodigoUe = "2" },
                new PainelEducacionalIdebDto { AnoLetivo = 2025, SerieAno = 9, Nota = 7, CriadoEm = DateTime.Now, CodigoDre = "3", CodigoUe = "4" },
            };

            var resultado = useCase
                .GetType()
                .GetMethod("ConverterParaIdebRaw", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .Invoke(useCase, new object[] { dtos }) as IEnumerable<PainelEducacionalIdebAgrupamento>;

            Assert.Equal(2, resultado.Count());
            Assert.Contains(resultado, r => r.Serie == "AnosIniciais");
            Assert.Contains(resultado, r => r.Serie == "AnosFinais");
        }

        [Fact]
        public void Get_Faixa_Deve_Retornar_Corretamente()
        {
            var metodo = typeof(ConsolidarIdebPainelEducacionalUseCase).GetMethod("GetFaixa", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

            Assert.Equal("0-1", metodo.Invoke(null, new object[] { 0.5m }));
            Assert.Equal("1-2", metodo.Invoke(null, new object[] { 1m }));
            Assert.Equal("9-10", metodo.Invoke(null, new object[] { 10m }));
            Assert.Null(metodo.Invoke(null, new object[] { -1m }));
        }

        [Fact]
        public void Parse_Etapa_Deve_Retornar_Enum_Correto()
        {
            var metodo = typeof(ConsolidarIdebPainelEducacionalUseCase).GetMethod("ParseEtapa", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

            var iniciais = metodo.Invoke(null, new object[] { "AnosIniciais" });
            var finais = metodo.Invoke(null, new object[] { "AnosFinais" });

            Assert.Equal(PainelEducacionalIdebSerie.AnosIniciais, iniciais);
            Assert.Equal(PainelEducacionalIdebSerie.AnosFinais, finais);
        }

        [Fact]
        public void Parse_Etapa_Deve_Lancar_Excecao_Para_Valor_Invalido()
        {
            var metodo = typeof(ConsolidarIdebPainelEducacionalUseCase)
                .GetMethod("ParseEtapa", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

            var ex = Assert.Throws<TargetInvocationException>(() =>
                metodo.Invoke(null, new object[] { "Outro" }));

            Assert.IsType<ArgumentException>(ex.InnerException);
            Assert.Contains("Etapa inválida", ex.InnerException.Message);
        }

        [Fact]
        public void Mensagem_Rabbit_Deve_Serializar_Objeto()
        {
            var obj = new { Nome = "Teste", Valor = 123 };
            var mensagem = new MensagemRabbit(Newtonsoft.Json.JsonConvert.SerializeObject(obj));

            var resultado = mensagem.ObterObjetoMensagem<Dictionary<string, object>>();

            Assert.Equal("Teste", resultado["Nome"].ToString());
            Assert.Equal("123", resultado["Valor"].ToString());
        }

        [Fact]
        public void Deve_Construir_MensagemRabbit_Com_Todos_Construtores()
        {
            var guid = Guid.NewGuid();

            var m1 = new MensagemRabbit("acao", "mensagem", guid, "rf");
            Assert.Equal("acao", m1.Action);

            var m2 = new MensagemRabbit("msg", guid, "nome", "rf", Guid.NewGuid());
            Assert.Equal("msg", m2.Mensagem);

            var m3 = new MensagemRabbit("somenteMensagem");
            Assert.Equal("somenteMensagem", m3.Mensagem);

            var m4 = new MensagemRabbit();
            Assert.Null(m4.Mensagem);
        }

        [Fact]
        public void Painel_Educacional_Salvar_Consolidacao_Ideb_Command_Deve_Atribuir_Ideb()
        {
            var ideb = new List<PainelEducacionalConsolidacaoIdeb> { new PainelEducacionalConsolidacaoIdeb() };
            var cmd = new PainelEducacionalSalvarConsolidacaoIdebCommand(ideb);
            Assert.Single(cmd.Ideb);
        }

        [Fact]
        public void Painel_Educacional_Idep_Agrupamento_Deve_Atribuir_Propriedades()
        {
            var obj = new PainelEducacionalIdepAgrupamento
            {
                AnoLetivo = 2025,
                Etapa = "AnosIniciais",
                Nota = 7.5m,
                CriadoEm = DateTime.Now,
                CodigoDre = 1,
                CodigoUe = 2
            };
            Assert.Equal("AnosIniciais", obj.Etapa);
        }

        [Fact]
        public void Painel_Educacional_Ideb_Dto_Deve_Atribuir_Propriedades()
        {
            var dto = new PainelEducacionalIdebDto
            {
                AnoLetivo = 2025,
                SerieAno = 9,
                Nota = 8.5m,
                Faixa = "8-9",
                CriadoEm = DateTime.Now,
                CodigoDre = "1",
                CodigoUe = "2",
                Quantidade = 10
            };
            Assert.Equal("8-9", dto.Faixa);
        }

        [Fact]
        public void Painel_Educacional_Consolidacao_Ideb_Deve_Atribuir_Propriedades()
        {
            var consolidado = new PainelEducacionalConsolidacaoIdeb
            {
                AnoLetivo = 2025,
                Etapa = PainelEducacionalIdebSerie.AnosIniciais,
                Faixa = "5-6",
                Quantidade = 2,
                MediaGeral = 5.5m,
                CodigoDre = "1",
                CodigoUe = "2"
            };
            Assert.Equal("5-6", consolidado.Faixa);
        }
    }
}