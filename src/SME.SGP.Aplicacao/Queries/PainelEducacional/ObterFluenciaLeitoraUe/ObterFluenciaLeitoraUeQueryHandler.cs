using MediatR;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterFluenciaLeitoraUe
{
    public class ObterFluenciaLeitoraUeQueryHandler : IRequestHandler<ObterFluenciaLeitoraUeQuery, IEnumerable<PainelEducacionalFluenciaLeitoraUeDto>>
    {
        private readonly IRepositorioConsultaFluenciaLeitoraUe repositorio;

        public ObterFluenciaLeitoraUeQueryHandler(IRepositorioConsultaFluenciaLeitoraUe repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<IEnumerable<PainelEducacionalFluenciaLeitoraUeDto>> Handle(ObterFluenciaLeitoraUeQuery request, CancellationToken cancellationToken)
        {
            var registros = await repositorio.ObterFluenciaLeitoraUe(request.Filtro);
            return registros
                .GroupBy(r => new { r.Turma, r.AlunosPrevistos, r.AlunosAvaliados, r.PreLeitorTotal })
                .Select(g => new PainelEducacionalFluenciaLeitoraUeDto
                {
                    Turma = g.Key.Turma,
                    AlunosPrevistos = g.Key.AlunosPrevistos,
                    AlunosAvaliados = g.Key.AlunosAvaliados,
                    TotalPreLeitor = g.Key.PreLeitorTotal,
                    Niveis = g
                        .GroupBy(f => new { f.Fluencia, f.QuantidadeAlunoFluencia, f.PercentualFluencia })
                        .Select(a => new IndicadorPreLeitorDto
                        {
                            Fluencia = (int)a.Key.Fluencia,
                            Quantidade = a.Sum(z => z.QuantidadeAlunoFluencia),
                            Percentual = a.Sum(z => z.PercentualFluencia),
                        })
                        .OrderBy(x => x.Fluencia)
                        .ToList()
                })
                .OrderBy(x => x.Turma)
                .ToList();
        }
    }
}
