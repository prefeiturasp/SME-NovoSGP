using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Consultas
{
    public class ConsultasAtribuicoesTeste
    {
        private readonly ConsultasAtribuicoes _consulta;
        private readonly Mock<IRepositorioAtribuicaoCJ> _repositorioAtribuicaoCJ = new();
        private readonly Mock<IRepositorioDreConsulta> _repositorioDreConsulta = new();
        private readonly Mock<IRepositorioUeConsulta> _repositorioUeConsulta = new();
        private readonly Mock<IServicoUsuario> _servicoUsuario = new();
        private readonly Mock<IConsultasAbrangencia> _consultasAbrangencia = new();
        private readonly Mock<IMediator> _mediator = new();

        public ConsultasAtribuicoesTeste()
        {
            _consulta = new ConsultasAtribuicoes(
                _repositorioAtribuicaoCJ.Object,
                _repositorioDreConsulta.Object,
                _repositorioUeConsulta.Object,
                _servicoUsuario.Object,
                _consultasAbrangencia.Object,
                _mediator.Object
            );
        }

        [Fact(DisplayName = "Deve retornar anos letivos se ano atual estiver presente e consideraHistorico for true")]
        public async Task ObterAnosLetivos_DeveRetornarLista_SeContemAnoAtual()
        {
            // Arrange
            var rf = "123456";
            var perfil = SME.SGP.Dominio.Perfis.PERFIL_CJ;
            var anosQuery = new List<int> { 2023, 2024, DateTime.Now.Year };

            _servicoUsuario.Setup(x => x.ObterLoginAtual()).Returns(rf);
            _servicoUsuario.Setup(x => x.ObterPerfilAtual()).Returns(perfil);

            _mediator.Setup(m => m.Send(It.IsAny<ObterAbrangenciaPorLoginPerfilQuery>(), default))
                .ReturnsAsync(new AbrangenciaRetornoEolDto
                {
                    Dres = new List<AbrangenciaDreRetornoEolDto>
                    {
                new AbrangenciaDreRetornoEolDto
                {
                    Ues = new List<AbrangenciaUeRetornoEolDto>
                    {
                        new AbrangenciaUeRetornoEolDto
                        {
                            Turmas = new List<AbrangenciaTurmaRetornoEolDto>
                            {
                                new AbrangenciaTurmaRetornoEolDto
                                {
                                    AnoLetivo = 2022,
                                    NomeTurma = "1A",
                                    TipoTurma = TipoTurma.Regular
                                }
                            }
                        }
                    }
                }
                    }
                });

            _mediator.Setup(m => m.Send(It.IsAny<ObterAnosAtribuicaoCJQuery>(), default))
                .ReturnsAsync(anosQuery);

            _repositorioAtribuicaoCJ.Setup(r => r.ObterAtribuicaoAtiva(rf, false))
                .Returns(new List<AtribuicaoCJ>());

            // Act
            var resultado = await _consulta.ObterAnosLetivos(true);

            // Assert
            var lista = resultado.ToList();
            Assert.Equal(3, lista.Count);
            Assert.Contains(2023, lista);
            Assert.Contains(2024, lista);
            Assert.Contains(DateTime.Now.Year, lista);
        }

        [Fact(DisplayName = "Não deve retornar anos letivos se ano atual não estiver presente e consideraHistorico for false")]
        public async Task ObterAnosLetivos_DeveRetornarListaVazia_SeNaoContemAnoAtual()
        {
            // Arrange
            var rf = "123456";
            var perfil = SME.SGP.Dominio.Perfis.PERFIL_CJ;
            var anosQuery = new List<int> { 2023, 2024 };

            _servicoUsuario.Setup(x => x.ObterLoginAtual()).Returns(rf);
            _servicoUsuario.Setup(x => x.ObterPerfilAtual()).Returns(perfil);

            _mediator.Setup(m => m.Send(It.IsAny<ObterAbrangenciaPorLoginPerfilQuery>(), default))
                .ReturnsAsync(new AbrangenciaRetornoEolDto
                {
                    Dres = new List<AbrangenciaDreRetornoEolDto>
                    {
                new AbrangenciaDreRetornoEolDto
                {
                    Ues = new List<AbrangenciaUeRetornoEolDto>
                    {
                        new AbrangenciaUeRetornoEolDto
                        {
                            Turmas = new List<AbrangenciaTurmaRetornoEolDto>
                            {
                                new AbrangenciaTurmaRetornoEolDto
                                {
                                    AnoLetivo = 2022,
                                    NomeTurma = "1A",
                                    TipoTurma = TipoTurma.Regular
                                }
                            }
                        }
                    }
                }
                    }
                });

            _mediator.Setup(m => m.Send(It.IsAny<ObterAnosAtribuicaoCJQuery>(), default))
                .ReturnsAsync(anosQuery);

            _repositorioAtribuicaoCJ.Setup(r => r.ObterAtribuicaoAtiva(rf, false))
                .Returns(new List<AtribuicaoCJ>()); // Nenhuma atribuição CJ

            // Act
            var resultado = await _consulta.ObterAnosLetivos(false);

            // Assert
            Assert.Empty(resultado); // Deve retornar vazio porque o ano atual não está presente
        }

        [Fact(DisplayName = "Deve retornar DREs do perfil CJ")]
        public async Task ObterDres_DeveRetornarListaDeDres_QuandoPerfilCJ()
        {
            // Arrange
            var rf = "123456";
            var anoLetivo = 2024;
            var consideraHistorico = false;
            var perfil = SME.SGP.Dominio.Perfis.PERFIL_CJ;

            _servicoUsuario.Setup(s => s.ObterLoginAtual()).Returns(rf);
            _servicoUsuario.Setup(s => s.ObterPerfilAtual()).Returns(perfil);

            var atribuicoesCJ = new List<AtribuicaoCJ>
            {
                new AtribuicaoCJ { DreId = "dre1", Turma = new Turma { AnoLetivo = anoLetivo }, Modalidade = Modalidade.Fundamental }
            };

            _repositorioAtribuicaoCJ
                .Setup(r => r.ObterAtribuicaoAtiva(rf, consideraHistorico))
                .Returns(atribuicoesCJ);

            _mediator.Setup(m => m.Send(It.IsAny<ObterAbrangenciaPorLoginPerfilQuery>(), default))
                .ReturnsAsync(new AbrangenciaRetornoEolDto
                {
                    Dres = new List<AbrangenciaDreRetornoEolDto>
                           {
                                new AbrangenciaDreRetornoEolDto
                                    {
                                        Codigo = "dre1",
                                        Ues = new List<AbrangenciaUeRetornoEolDto>()
                                    }
                           }
                });

            var dres = new List<Dre>
            {
                new Dre { CodigoDre = "dre1", Nome = "DRE Leste", Abreviacao = "LE" }
            };

            _repositorioDreConsulta
                .Setup(r => r.ListarPorCodigos(It.IsAny<string[]>()))
                .Returns(dres);

            // Act
            var resultado = await _consulta.ObterDres(anoLetivo, consideraHistorico);

            // Assert
            var lista = resultado.ToList();
            Assert.Single(lista);
            Assert.Equal("dre1", lista[0].Codigo);
            Assert.Equal("DRE Leste", lista[0].Nome);
        }

        [Fact(DisplayName = "Deve retornar UEs do perfil CJ")]
        public async Task ObterUes_DeveRetornarListaDeUes_QuandoPerfilCJ()
        {
            // Arrange
            var rf = "123456";
            var anoLetivo = 2024;
            var codigoDre = "dre1";
            var consideraHistorico = false;
            var perfil = Perfis.PERFIL_CJ;

            _servicoUsuario.Setup(s => s.ObterLoginAtual()).Returns(rf);
            _servicoUsuario.Setup(s => s.ObterPerfilAtual()).Returns(perfil);

            var atribuicoesCJ = new List<AtribuicaoCJ>
    {
        new AtribuicaoCJ { UeId = "ue1", Turma = new Turma { AnoLetivo = anoLetivo }, Modalidade = Modalidade.Fundamental }
    };

            _repositorioAtribuicaoCJ
                .Setup(r => r.ObterAtribuicaoAtiva(rf, consideraHistorico))
                .Returns(atribuicoesCJ);

            _repositorioAtribuicaoCJ
                .Setup(r => r.ObterPorFiltros(
                    It.IsAny<Modalidade?>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<long>(),
                    rf,
                    It.IsAny<string>(),
                    true,
                    codigoDre,
                    It.IsAny<string[]>(),
                    anoLetivo,
                    consideraHistorico
                ))
                .ReturnsAsync(atribuicoesCJ);

            _mediator.Setup(m => m.Send(It.IsAny<ObterAbrangenciaPorLoginPerfilQuery>(), default))
                .ReturnsAsync(new AbrangenciaRetornoEolDto
                {
                    Dres = new List<AbrangenciaDreRetornoEolDto>
                           {
                                new AbrangenciaDreRetornoEolDto
                                    {
                                        Codigo = codigoDre,
                                        Ues = new List<AbrangenciaUeRetornoEolDto>()
                                        {
                                            new AbrangenciaUeRetornoEolDto
                                            {
                                                Codigo = "ue1"
                                            }
                                        }
                                    }
                           }
                });

            _mediator.Setup(m => m.Send(It.IsAny<ObterNovosTiposUEPorAnoQuery>(), default))
                .ReturnsAsync("");

            var ues = new List<Ue>
            {
                new Ue { CodigoUe = "ue1", Nome = "Escola Modelo", TipoEscola = TipoEscola.EMEF }
            };

            _repositorioUeConsulta
                .Setup(r => r.ListarPorCodigos(It.IsAny<string[]>()))
                .Returns(ues);

            // Act
            var resultado = await _consulta.ObterUes(codigoDre, anoLetivo, consideraHistorico);

            // Assert
            var lista = resultado.ToList();
            Assert.Single(lista);
            Assert.Equal("ue1", lista[0].Codigo);
            Assert.Equal("Escola Modelo", lista[0].NomeSimples);
        }
    }
}