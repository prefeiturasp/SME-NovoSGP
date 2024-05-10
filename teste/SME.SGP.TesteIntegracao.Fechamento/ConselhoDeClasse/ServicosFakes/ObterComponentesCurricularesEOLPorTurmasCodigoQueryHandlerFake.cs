using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using k8s.Models;
using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes
{
    public class ObterComponentesCurricularesEOLPorTurmasCodigoQueryHandlerFake : IRequestHandler<ObterComponentesCurricularesEOLPorTurmasCodigoQuery, IEnumerable<ComponenteCurricularEol>>
    {
      public ObterComponentesCurricularesEOLPorTurmasCodigoQueryHandlerFake(){}

      public async Task<IEnumerable<ComponenteCurricularEol>> Handle(ObterComponentesCurricularesEOLPorTurmasCodigoQuery request, CancellationToken cancellationToken)
      {
        return await Task.FromResult(new List<ComponenteCurricularEol>()
        {
          new ComponenteCurricularEol()
          {
            Codigo = 2,
            Descricao = "MATEMATICA",
            LancaNota = true,
            Regencia = false,
            TerritorioSaber = false
          },
          new ComponenteCurricularEol()
          {
            Codigo = 6,
            Descricao = "ED. FISICA",
            LancaNota = true,
            Regencia = false,
            TerritorioSaber = false
          },
          new ComponenteCurricularEol()
          {
            Codigo = 7,
            Descricao = "HISTORIA",
            LancaNota = true,
            Regencia = false,
            TerritorioSaber = false
          },
          new ComponenteCurricularEol()
          {
            Codigo = 8,
            Descricao = "GEOGRAFIA",
            LancaNota = true,
            Regencia = false,
            TerritorioSaber = false
          },
          new ComponenteCurricularEol()
          {
            Codigo = 9,
            Descricao = "INGLES",
            LancaNota = true,
            Regencia = false,
            TerritorioSaber = false
          },
          new ComponenteCurricularEol()
          {
            Codigo = 89,
            Descricao = "CIENCIAS",
            LancaNota = true,
            Regencia = false,
            TerritorioSaber = false
          },
          new ComponenteCurricularEol()
          {
            Codigo = 138,
            Descricao = "LINGUA PORTUGUESA",
            LancaNota = true,
            Regencia = false,
            TerritorioSaber = false
          },
          new ComponenteCurricularEol()
          {
            Codigo = 139,
            Descricao = "ARTE",
            LancaNota = true,
            Regencia = false,
            TerritorioSaber = false
          },
          new ComponenteCurricularEol()
          {
            Codigo = 1060,
            Descricao = "INFORMATICA - OIE",
            LancaNota = false,
            Regencia = false,
            TerritorioSaber = false
          },
          new ComponenteCurricularEol()
          {
            Codigo = 1061,
            Descricao = "LEITURA - OSL",
            LancaNota = false,
            Regencia = false,
            TerritorioSaber = false
          },
          new ComponenteCurricularEol()
          {
              Codigo = 1105,
              Descricao = "Regência de Classe Fund I - 5H",
              LancaNota = true,
              Regencia = true
          }
        });
      }
    }
}