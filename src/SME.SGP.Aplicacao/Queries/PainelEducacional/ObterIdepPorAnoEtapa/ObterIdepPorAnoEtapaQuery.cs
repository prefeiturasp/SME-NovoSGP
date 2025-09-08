using MediatR;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterIdepPorAnoEtapa
{
    public class ObterIdepPorAnoEtapaQuery : IRequest<IEnumerable<PainelEducacionalConsolidacaoIdep>>
    {
        public ObterIdepPorAnoEtapaQuery(int ano, int etapa)
        {
            Ano = ano;
            Etapa = etapa;
        }
        public int Ano { get; set; }
        public int Etapa { get; set; }
    }
}
