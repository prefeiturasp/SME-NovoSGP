using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterEducacaoIntegral
{
    public class ObterEducacaoIntegralQueryHandler : IRequestHandler<ObterEducacaoIntegralQuery, IEnumerable<PainelEducacionalEducacaoIntegralDto>>
    {
        private readonly IRepositorioEducacaoIntegralConsulta repositorio;

        public ObterEducacaoIntegralQueryHandler(IRepositorioEducacaoIntegralConsulta repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<IEnumerable<PainelEducacionalEducacaoIntegralDto>> Handle(
            ObterEducacaoIntegralQuery request,
            CancellationToken cancellationToken)
        {
            var registros = await repositorio.ObterEducacaoIntegral(request.Filtro);

            return registros
                .GroupBy(r => r.ModalidadeTurma)
                .Select(g => new PainelEducacionalEducacaoIntegralDto
                {
                    Modalidade = ((Modalidade)int.Parse(g.Key)).ObterDisplayName(),
                    Indicadores = g
                        .GroupBy(x => ObterEtapaAnoSerie(int.Parse(g.Key), x.Ano))
                        .Select(a => new IndicadorEducacaoIntegralDto
                        {
                            AnoSerieEtapa = a.Key,
                            QuantidadeAlunosIntegral = a.Sum(z => z.QuantidadeAlunosIntegral),
                            QuantidadeAlunosParcial = a.Sum(z => z.QuantidadeAlunosParcial)
                        })
                        .OrderBy(x => x.AnoSerieEtapa)
                        .ToList()
                })
                .OrderBy(m => m.Modalidade)
                .ToList();
        }

        private static string ObterEtapaAnoSerie(int modalidade, string anoTurma)
        {
            if (modalidade == (int)Modalidade.EducacaoInfantil)
            {
                if (!string.IsNullOrWhiteSpace(anoTurma) && int.TryParse(anoTurma, out var ano))
                {
                    if (ano >= 1 && ano <= 4)
                        return "Creche (0 a 3 anos)";

                    if (ano >= 5 && ano <= 7)
                        return "Pré-Escola (4 e 5 anos)";
                }
            }

            return $"{anoTurma}º";
        }
    }
}
