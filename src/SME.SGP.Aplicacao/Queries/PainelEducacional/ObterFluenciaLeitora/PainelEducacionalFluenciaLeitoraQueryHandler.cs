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

            return registros
                .GroupBy(r => new { r.NomeFluencia })
                .Select(g => new PainelEducacionalFluenciaLeitoraDto
                {
                    NomeFluencia = g.Key.NomeFluencia,
                    DescricaoFluencia = g.First().DescricaoFluencia,
                    DreCodigo = g.First().DreCodigo ?? "",
                    Percentual = Math.Round(g.Sum(p => p.Percentual * p.QuantidadeAlunos) / g.Sum(x => x.QuantidadeAlunos), 2),
                    QuantidadeAlunos = g.Sum(x => x.QuantidadeAlunos),
                    Ano = g.First().Ano,
                    Periodo = g.First().Periodo
                })
                .OrderBy(r => r.NomeFluencia)
                .ThenBy(r => r.DreCodigo);
        }
    }
}
