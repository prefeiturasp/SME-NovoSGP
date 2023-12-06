using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes
{
    public class ObterComponentesCurricularesEOLPorTurmaECodigoUeQueryHandlerFake : IRequestHandler<ObterComponentesCurricularesEOLPorTurmaECodigoUeQuery, IEnumerable<ComponenteCurricularDto>>
    {
        public async Task<IEnumerable<ComponenteCurricularDto>> Handle(ObterComponentesCurricularesEOLPorTurmaECodigoUeQuery request, CancellationToken cancellationToken)
        {

            return await Task.FromResult(new List<ComponenteCurricularDto>()
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
        });
        }
    }
}