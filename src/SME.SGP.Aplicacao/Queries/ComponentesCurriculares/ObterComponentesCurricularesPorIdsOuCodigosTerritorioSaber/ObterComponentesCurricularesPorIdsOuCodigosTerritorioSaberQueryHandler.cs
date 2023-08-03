using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesPorIdsOuCodigosTerritorioSaberQueryHandler : IRequestHandler<ObterComponentesCurricularesPorIdsOuCodigosTerritorioSaberQuery, IEnumerable<DisciplinaDto>>
    {
        private readonly IRepositorioComponenteCurricularConsulta repositorioComponenteCurricular;
        private readonly IMediator mediator;

        public ObterComponentesCurricularesPorIdsOuCodigosTerritorioSaberQueryHandler(IRepositorioComponenteCurricularConsulta repositorioComponenteCurricular, IMediator mediator)
        {
            this.repositorioComponenteCurricular = repositorioComponenteCurricular ?? throw new ArgumentNullException(nameof(repositorioComponenteCurricular));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<DisciplinaDto>> Handle(ObterComponentesCurricularesPorIdsOuCodigosTerritorioSaberQuery request, CancellationToken cancellationToken)
        {
            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());
            var codigosComponentes = request.CodigoComponentes
                    .Select(c => c.codigo)
                    .ToArray();

            if (request.PossuiTerritorio.HasValue && request.PossuiTerritorio.Value && !usuarioLogado.EhProfessorCj())
            {
                var listaDisciplinas = new List<DisciplinaDto>();                

                var disciplinasAgrupadas = await mediator.Send(new ObterComponentesCurricularesEOLComSemAgrupamentoTurmaQuery(codigosComponentes, request.CodigoTurma)); 
                
                foreach (var disciplina in disciplinasAgrupadas)
                {
                    var codigoTerritorioSaberCorrespondente = request.CodigoComponentes
                        .FirstOrDefault(c => (!disciplina.TerritorioSaber && c.codigo.Equals(disciplina.CodigoComponenteCurricular)) || (disciplina.TerritorioSaber && (c.codigoTerritorioSaber.Equals(disciplina.CodigoTerritorioSaber) || c.codigo.Equals(disciplina.CodigoTerritorioSaber))));

                    if (codigoTerritorioSaberCorrespondente == default)
                        continue;

                    disciplina.RegistraFrequencia = await mediator
                        .Send(new ObterComponenteRegistraFrequenciaQuery(codigoTerritorioSaberCorrespondente.codigo, codigoTerritorioSaberCorrespondente != default && codigoTerritorioSaberCorrespondente.codigoTerritorioSaber.HasValue && codigoTerritorioSaberCorrespondente.codigoTerritorioSaber.Value > 0 ? codigoTerritorioSaberCorrespondente.codigoTerritorioSaber : null));

                    disciplina.Id = codigoTerritorioSaberCorrespondente.codigoTerritorioSaber ?? 0;
                    listaDisciplinas.Add(disciplina);
                }

                request.CodigoComponentes
                    .Where(cc => !disciplinasAgrupadas.Select(d => d.CodigoComponenteCurricular).Contains(cc.codigo) && cc.codigoTerritorioSaber.Equals(0))
                    .ToList().ForEach(cc =>
                    {
                        var componente = repositorioComponenteCurricular.ObterDisciplinasPorIds(new long[] { cc.codigo }).Result;
                        listaDisciplinas.Add(componente.First());
                    });

                return listaDisciplinas;
            }
            else
                return await repositorioComponenteCurricular.ObterDisciplinasPorIds(codigosComponentes);
        }
    }
}
