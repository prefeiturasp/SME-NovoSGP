﻿using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.ComponentesCurriculares.ObterComponentesCurricularesPorAnosEModalidade
{
    public class ObterComponentesCurricularesPorAnosEModalidadeQueryHandler : IRequestHandler<ObterComponentesCurricularesPorAnosEModalidadeQuery, IEnumerable<ComponenteCurricularEol>>
    {
        private readonly IServicoEol servicoEol;
        public ObterComponentesCurricularesPorAnosEModalidadeQueryHandler(IServicoEol servicoEol)
        {
            this.servicoEol = servicoEol ?? throw new System.ArgumentNullException(nameof(servicoEol));
        }

        public async Task<IEnumerable<ComponenteCurricularEol>> Handle(ObterComponentesCurricularesPorAnosEModalidadeQuery request, CancellationToken cancellationToken)
        {
            var componentes = (await servicoEol.ObterComponentesCurricularesPorAnosEModalidade(request.CodigoUe, request.Modalidade, request.AnoLetivo, request.AnosEscolares))?.ToList();

            if (request.TurmaPrograma)
            {                
                var componentesTurmaPrograma = (await servicoEol.ObterComponentesCurricularesPorAnosEModalidade(request.CodigoUe, request.Modalidade, request.AnoLetivo, null))?.ToList();
                if (componentesTurmaPrograma != null)
                {
                    var componentesTurmaProgramaFiltrada = componentesTurmaPrograma.Where(x => !componentes.Any(y => y.Codigo == x.Codigo));

                    componentes.AddRange(componentesTurmaProgramaFiltrada);
                }
            }

            if (componentes == null || !componentes.Any())
            {
                throw new NegocioException("Nenhum componente localizado para a modalidade e anos informados.");
            }
            componentes = componentes.OrderBy(c => c.Descricao).ToList();

            if (request.Modalidade != Modalidade.EducacaoInfantil)
            {
                componentes.Insert(0, new ComponenteCurricularEol
                {
                    Codigo = -99,
                    Descricao = "Todos"
                });
            }
            return componentes;
        }
    }
}
