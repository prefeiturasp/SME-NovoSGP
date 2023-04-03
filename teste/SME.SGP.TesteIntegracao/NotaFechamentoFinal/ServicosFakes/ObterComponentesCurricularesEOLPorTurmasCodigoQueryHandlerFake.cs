using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;

namespace SME.SGP.TesteIntegracao.NotaFechamentoFinal.ServicosFakes
{
    public class ObterComponentesCurricularesEOLPorTurmasCodigoQueryHandlerFake : IRequestHandler<ObterComponentesCurricularesEOLPorTurmasCodigoQuery, IEnumerable<ComponenteCurricularDto>>
    {
      public ObterComponentesCurricularesEOLPorTurmasCodigoQueryHandlerFake(){}

      public async Task<IEnumerable<ComponenteCurricularDto>> Handle(ObterComponentesCurricularesEOLPorTurmasCodigoQuery request, CancellationToken cancellationToken)
      {
        return new List<ComponenteCurricularDto>()
        {
          new ComponenteCurricularDto()
          {
            Codigo = "2",
            Descricao = "MATEMATICA",
            LancaNota = true,
            Regencia = false,
            DescricaoEol = "MATEMATICA",
            TerritorioSaber = false
          },
          new ComponenteCurricularDto()
          {
            Codigo = "6",
            Descricao = "ED. FISICA",
            LancaNota = true,
            Regencia = false,
            DescricaoEol = "ED. FISICA",
            TerritorioSaber = false
          },
          new ComponenteCurricularDto()
          {
            Codigo = "7",
            Descricao = "HISTORIA",
            LancaNota = true,
            Regencia = false,
            DescricaoEol = "HISTORIA",
            TerritorioSaber = false
          },
          new ComponenteCurricularDto()
          {
            Codigo = "8",
            Descricao = "GEOGRAFIA",
            LancaNota = true,
            Regencia = false,
            DescricaoEol = "GEOGRAFIA",
            TerritorioSaber = false
          },
          new ComponenteCurricularDto()
          {
            Codigo = "9",
            Descricao = "INGLES",
            LancaNota = true,
            Regencia = false,
            DescricaoEol = "INGLES",
            TerritorioSaber = false
          },
          new ComponenteCurricularDto()
          {
            Codigo = "89",
            Descricao = "CIENCIAS",
            LancaNota = true,
            Regencia = false,
            DescricaoEol = "CIENCIAS",
            TerritorioSaber = false
          },
          new ComponenteCurricularDto()
          {
            Codigo = "138",
            Descricao = "LINGUA PORTUGUESA",
            LancaNota = true,
            Regencia = false,
            DescricaoEol = "LINGUA PORTUGUESA",
            TerritorioSaber = false
          },
          new ComponenteCurricularDto()
          {
            Codigo = "139",
            Descricao = "ARTE",
            LancaNota = true,
            Regencia = false,
            DescricaoEol = "ARTE",
            TerritorioSaber = false
          },
          new ComponenteCurricularDto()
          {
            Codigo = "1060",
            Descricao = "INFORMATICA - OIE",
            LancaNota = false,
            Regencia = false,
            DescricaoEol = "INFORMATICA - OIE",
            TerritorioSaber = false
          },
          new ComponenteCurricularDto()
          {
            Codigo = "1061",
            Descricao = "LEITURA - OSL",
            LancaNota = false,
            Regencia = false,
            DescricaoEol = "LEITURA - OSL",
            TerritorioSaber = false
          },
          new ComponenteCurricularDto()
          {
              Codigo = "1105",
              Descricao = "Regência de Classe Fund I - 5H",
              LancaNota = true,
              Regencia = true,
              DescricaoEol = "REG CLASSE CICLO ALFAB / INTERD 5HRS"              
          }
        };
      }
    }
}