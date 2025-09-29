using MediatR;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterFluenciaLeitora
{
    public class PainelEducacionalFluenciaLeitoraQueryHandler : IRequestHandler<PainelEducacionalFluenciaLeitoraQuery, IEnumerable<PainelEducacionalFluenciaLeitoraDto>>
    {
        private readonly IRepositorioPainelEducacionalFluenciaLeitoraConsulta repositorioPainelEducacionalFluenciaLeitoraConsulta;
        
        public PainelEducacionalFluenciaLeitoraQueryHandler(IRepositorioPainelEducacionalFluenciaLeitoraConsulta repositorioPainelEducacionalFluenciaLeitoraConsulta)
        {
            this.repositorioPainelEducacionalFluenciaLeitoraConsulta = repositorioPainelEducacionalFluenciaLeitoraConsulta;
        }

        public async Task<IEnumerable<PainelEducacionalFluenciaLeitoraDto>> Handle(PainelEducacionalFluenciaLeitoraQuery request, CancellationToken cancellationToken)
        {
            var registros = await repositorioPainelEducacionalFluenciaLeitoraConsulta.ObterFluenciaLeitora(request.Periodo, request.AnoLetivo, request.CodigoDre);

            var ehConsolidacaoSme = string.IsNullOrWhiteSpace(request.CodigoDre) || request.CodigoDre == "0";
            
            if (ehConsolidacaoSme)
            {
                return registros
                    .GroupBy(r => new { r.NomeFluencia })
                    .Select(g => new PainelEducacionalFluenciaLeitoraDto
                    {
                        NomeFluencia = g.Key.NomeFluencia,
                        DescricaoFluencia = g.First().DescricaoFluencia,
                        DreCodigo = g.First().DreCodigo ?? "",
                        Percentual = CalcularPercentual(g),
                        QuantidadeAlunos = g.Sum(x => x.QuantidadeAlunos),
                        Ano = g.First().Ano,
                        Periodo = g.First().Periodo
                    })
                    .OrderBy(r => r.NomeFluencia)
                    .ThenBy(r => r.DreCodigo);
            }
            else
            {
                return registros
                    .GroupBy(r => new { r.NomeFluencia, r.DreCodigo })
                    .Select(g => new PainelEducacionalFluenciaLeitoraDto
                    {
                        NomeFluencia = g.Key.NomeFluencia,
                        DescricaoFluencia = g.First().DescricaoFluencia,
                        DreCodigo = g.Key.DreCodigo ?? "",
                        Percentual = CalcularPercentual(g),
                        QuantidadeAlunos = g.Sum(x => x.QuantidadeAlunos),
                        Ano = g.First().Ano,
                        Periodo = g.First().Periodo
                    })
                    .OrderBy(r => r.NomeFluencia)
                    .ThenBy(r => r.DreCodigo);
            }
        }

        private static decimal CalcularPercentual(IEnumerable<PainelEducacionalFluenciaLeitoraDto> grupo)
        {
            var totalAlunos = grupo.Sum(x => x.QuantidadeAlunos);
            if (totalAlunos == 0) return 0;
            
            var somaPonderada = grupo.Sum(x => x.Percentual * x.QuantidadeAlunos);
            return Math.Round(somaPonderada / totalAlunos, 2);
        }
    }
}
