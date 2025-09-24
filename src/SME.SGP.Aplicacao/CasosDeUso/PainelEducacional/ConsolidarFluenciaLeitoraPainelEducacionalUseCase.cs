using MediatR;
using SME.SGP.Aplicacao.Commands.PainelEducacional.ExcluirFluenciaLeitora;
using SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarFluenciaLeitora;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional;
using SME.SGP.Infra.Enumerados;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.PainelEducacional
{
    public class ConsolidarFluenciaLeitoraPainelEducacionalUseCase : AbstractUseCase, IConsolidarFluenciaLeitoraPainelEducacionalUseCase
    {
        private readonly IRepositorioFluenciaLeitora repositorioFluenciaLeitora;

        public ConsolidarFluenciaLeitoraPainelEducacionalUseCase(IMediator mediator, IRepositorioFluenciaLeitora repositorioFluenciaLeitora) : base(mediator)
        {
            this.repositorioFluenciaLeitora = repositorioFluenciaLeitora;
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var registrosFluenciaLeitora = await repositorioFluenciaLeitora.ObterRegistroFluenciaLeitoraGeralAsync();

            if (registrosFluenciaLeitora?.Any() != true)
                return true;

            await mediator.Send(new PainelEducacionalExcluirFluenciaLeitoraCommand());

            var anosEPeriodos = registrosFluenciaLeitora
                .Where(r => r.Periodo > 0) // Garantir que o período seja válido
                .Select(r => new { r.AnoLetivo, r.Periodo })
                .Distinct();

            var todasConsolidacoes = new List<PainelEducacionalRegistroFluenciaLeitoraAgrupamentoFluenciaDto>();
            
            foreach (var item in anosEPeriodos)
            {
                var consolidacoesPorAnoEPeriodo = await ObterConsolidacoesPorAnoEPeriodo(registrosFluenciaLeitora, item.AnoLetivo, item.Periodo);
                todasConsolidacoes.AddRange(consolidacoesPorAnoEPeriodo);
            }

            if (todasConsolidacoes.Any())
            {
                await mediator.Send(new PainelEducacionalSalvarFluenciaLeitoraCommand(todasConsolidacoes));
            }

            return true;
        }       

        private async Task<List<PainelEducacionalRegistroFluenciaLeitoraAgrupamentoFluenciaDto>> ObterConsolidacoesPorAnoEPeriodo(
            IEnumerable<PainelEducacionalRegistroFluenciaLeitoraDto> registrosFluenciaLeitora, 
            int anoLetivo, 
            int periodo)
        {
            var registrosFiltrados = registrosFluenciaLeitora.Where(r => r.AnoLetivo == anoLetivo && r.Periodo == periodo);
            var totalAlunosGeral = registrosFiltrados.Count();

            if (totalAlunosGeral == 0)
                return new List<PainelEducacionalRegistroFluenciaLeitoraAgrupamentoFluenciaDto>();

            var consolidacaoPorCodigoFluencia = registrosFiltrados
                .GroupBy(f => f.CodigoFluencia)
                .Select(g => new PainelEducacionalRegistroFluenciaLeitoraAgrupamentoFluenciaDto
                {
                    Fluencia = ObterNomeFluencia(g.Key),
                    DescricaoFluencia = ObterDescricaoFluencia(g.Key),
                    QuantidadeAluno = g.Count(),
                    Percentual = Math.Round((decimal)g.Count() / totalAlunosGeral * 100, 2),
                    AnoLetivo = anoLetivo,
                    Periodo = periodo, 
                    DreCodigo = null,
                    DreNome = null,
                    UeCodigo = null,
                    UeNome = null
                })
                .ToList();

            return consolidacaoPorCodigoFluencia;
        }

        private static string ObterNomeFluencia(int fluencia)
        {
            return fluencia switch
            {
                (int)FluenciaLeitoraEnum.Fluencia1 => FluenciaLeitoraEnum.Fluencia1.ObterDisplayName(),
                (int)FluenciaLeitoraEnum.Fluencia2 => FluenciaLeitoraEnum.Fluencia2.ObterDisplayName(),
                (int)FluenciaLeitoraEnum.Fluencia3 => FluenciaLeitoraEnum.Fluencia3.ObterDisplayName(),
                (int)FluenciaLeitoraEnum.Fluencia4 => FluenciaLeitoraEnum.Fluencia4.ObterDisplayName(),
                (int)FluenciaLeitoraEnum.Fluencia5 => FluenciaLeitoraEnum.Fluencia5.ObterDisplayName(),
                (int)FluenciaLeitoraEnum.Fluencia6 => FluenciaLeitoraEnum.Fluencia6.ObterDisplayName(),
                _ => "Não identificado"
            };
        }

        private static string ObterDescricaoFluencia(int fluencia)
        {
            return fluencia switch
            {
                (int)FluenciaLeitoraEnum.Fluencia1 => "Não leu",
                (int)FluenciaLeitoraEnum.Fluencia2 => "Soletrou",
                (int)FluenciaLeitoraEnum.Fluencia3 => "Silabou",
                (int)FluenciaLeitoraEnum.Fluencia4 => "Leu até 10 palavras",
                (int)FluenciaLeitoraEnum.Fluencia5 => "",
                (int)FluenciaLeitoraEnum.Fluencia6 => "",
                _ => "Não identificado"
            };
        }
    }
}
