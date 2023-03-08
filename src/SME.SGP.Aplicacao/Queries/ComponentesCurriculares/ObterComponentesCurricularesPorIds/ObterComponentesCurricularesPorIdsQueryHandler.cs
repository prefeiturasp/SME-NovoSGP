using MediatR;
using MimeKit.Encodings;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesPorIdsQueryHandler : IRequestHandler<ObterComponentesCurricularesPorIdsQuery, IEnumerable<DisciplinaDto>>
    {
        private readonly IRepositorioComponenteCurricularConsulta repositorioComponenteCurricular;
        private readonly IServicoEol servicoEol;
        private readonly IMediator mediator;

        public ObterComponentesCurricularesPorIdsQueryHandler(IRepositorioComponenteCurricularConsulta repositorioComponenteCurricular, IServicoEol servicoEol, IMediator mediator)
        {
            this.repositorioComponenteCurricular = repositorioComponenteCurricular ?? throw new System.ArgumentNullException(nameof(repositorioComponenteCurricular));
            this.servicoEol = servicoEol ?? throw new System.ArgumentNullException(nameof(servicoEol));
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<DisciplinaDto>> Handle(ObterComponentesCurricularesPorIdsQuery request, CancellationToken cancellationToken)
        {
            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());
            var disciplinasRetorno = new List<DisciplinaDto>();

            var disciplinasUsuario = await mediator
                .Send(new ObterComponentesCurricularesDoProfessorNaTurmaQuery(request.CodigoTurma, usuarioLogado.Login, usuarioLogado.PerfilAtual));

            var disciplinasAgrupadas = await servicoEol
                    .ObterDisciplinasPorIdsAgrupadas(request.Ids, request.CodigoTurma);            

            if (request.PossuiTerritorio.HasValue && request.PossuiTerritorio.Value && !usuarioLogado.EhProfessorCj())
            {   
                foreach (var disciplina in disciplinasAgrupadas)
                {
                    var disciplinaCorrespondente = disciplinasUsuario
                        .SingleOrDefault(du => du.Codigo.Equals(disciplina.CodigoComponenteCurricular) || du.CodigoComponenteTerritorioSaber.Equals(disciplina.CodigoComponenteCurricular));

                    disciplina.RegistraFrequencia = await mediator
                        .Send(new ObterComponenteRegistraFrequenciaQuery(disciplinaCorrespondente?.CodigoComponenteTerritorioSaber > 0 ? disciplinaCorrespondente.CodigoComponenteTerritorioSaber : disciplina.CodigoComponenteCurricular));

                    disciplinasRetorno.Add(disciplina);
                }
            }
            else
            {
                foreach (var id in request.Ids)
                {
                    var disciplinaCorreposdente = disciplinasUsuario
                        .SingleOrDefault(d => d.Codigo.Equals(id) || d.CodigoComponenteTerritorioSaber.Equals(id));                    

                    if (disciplinaCorreposdente != null)
                    {
                        var registraFrequencia = await mediator
                            .Send(new ObterComponenteRegistraFrequenciaQuery(disciplinaCorreposdente != null && disciplinaCorreposdente.CodigoComponenteTerritorioSaber > 0 ? disciplinaCorreposdente.CodigoComponenteTerritorioSaber : disciplinaCorreposdente.Codigo));

                        disciplinasRetorno.Add(new DisciplinaDto()
                        {
                            Id = disciplinaCorreposdente.CodigoComponenteTerritorioSaber,
                            CodigoComponenteCurricular = disciplinaCorreposdente.Codigo,
                            CdComponenteCurricularPai = disciplinaCorreposdente.CodigoComponenteCurricularPai,
                            Compartilhada = disciplinaCorreposdente.Compartilhada,
                            Nome = disciplinaCorreposdente.Descricao,
                            NomeComponenteInfantil = disciplinaCorreposdente.Descricao,
                            PossuiObjetivos = disciplinaCorreposdente.PossuiObjetivos,
                            Regencia = disciplinaCorreposdente.Regencia,
                            RegistraFrequencia = registraFrequencia,
                            TerritorioSaber = disciplinaCorreposdente.TerritorioSaber,
                            LancaNota = disciplinaCorreposdente.LancaNota,
                            TurmaCodigo = disciplinaCorreposdente.TurmaCodigo
                        });
                    }
                    else
                    {
                        var disciplina = disciplinasAgrupadas.SingleOrDefault(da => da.CodigoComponenteCurricular.Equals(id)) ?? (await repositorioComponenteCurricular
                            .ObterDisciplinasPorIds(new long[] { id })).FirstOrDefault();

                        if (disciplina != null)
                            disciplinasRetorno.Add(disciplina);
                    }
                }
            }

            return disciplinasRetorno;
        }
    }
}
