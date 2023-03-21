using MediatR;
using MimeKit.Encodings;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesPorIdsEUsuariosQueryHandler : IRequestHandler<ObterComponentesCurricularesPorIdsEUsuariosQuery, IEnumerable<DisciplinaDto>>
    {
        private readonly IRepositorioComponenteCurricularConsulta repositorioComponenteCurricular;
        private readonly IServicoEol servicoEol;
        private readonly IMediator mediator;

        public ObterComponentesCurricularesPorIdsEUsuariosQueryHandler(IRepositorioComponenteCurricularConsulta repositorioComponenteCurricular, IServicoEol servicoEol, IMediator mediator)
        {
            this.repositorioComponenteCurricular = repositorioComponenteCurricular ?? throw new System.ArgumentNullException(nameof(repositorioComponenteCurricular));
            this.servicoEol = servicoEol ?? throw new System.ArgumentNullException(nameof(servicoEol));
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<DisciplinaDto>> Handle(ObterComponentesCurricularesPorIdsEUsuariosQuery request, CancellationToken cancellationToken)
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
                        .FirstOrDefault(du => du.Codigo.Equals(disciplina.CodigoComponenteCurricular) || du.CodigoComponenteTerritorioSaber.Equals(disciplina.CodigoComponenteCurricular));

                    disciplina.RegistraFrequencia = await mediator
                        .Send(new ObterComponenteRegistraFrequenciaQuery(disciplinaCorrespondente?.CodigoComponenteTerritorioSaber > 0 ? disciplinaCorrespondente.CodigoComponenteTerritorioSaber : disciplina.CodigoComponenteCurricular));

                    disciplinasRetorno.Add(disciplina);
                }
            }
            else
            {
                foreach (var id in request.Ids)
                {
                    var disciplinaCorrespondente = disciplinasUsuario
                        .FirstOrDefault(d => d.Codigo.Equals(id) || d.CodigoComponenteTerritorioSaber.Equals(id));                           

                    if (disciplinaCorrespondente != null)
                    {
                        var registraFrequencia = await mediator
                            .Send(new ObterComponenteRegistraFrequenciaQuery(disciplinaCorrespondente != null && disciplinaCorrespondente.CodigoComponenteTerritorioSaber > 0 ? disciplinaCorrespondente.CodigoComponenteTerritorioSaber : disciplinaCorrespondente.Codigo));

                        disciplinasRetorno.Add(new DisciplinaDto()
                        {
                            Id = disciplinaCorrespondente.TerritorioSaber ? disciplinaCorrespondente.CodigoComponenteTerritorioSaber : disciplinaCorrespondente.Codigo,
                            CodigoComponenteCurricular = disciplinaCorrespondente.Codigo,
                            CdComponenteCurricularPai = disciplinaCorrespondente.CodigoComponenteCurricularPai,
                            Compartilhada = disciplinaCorrespondente.Compartilhada,
                            Nome = disciplinaCorrespondente.Descricao,
                            NomeComponenteInfantil = disciplinaCorrespondente.Descricao,
                            PossuiObjetivos = disciplinaCorrespondente.PossuiObjetivos,
                            Regencia = disciplinaCorrespondente.Regencia,
                            RegistraFrequencia = registraFrequencia,
                            TerritorioSaber = disciplinaCorrespondente.TerritorioSaber,
                            LancaNota = disciplinaCorrespondente.LancaNota,
                            TurmaCodigo = disciplinaCorrespondente.TurmaCodigo,
                            GrupoMatrizId = disciplinaCorrespondente.GrupoMatriz?.Id ?? 0,
                            GrupoMatrizNome = disciplinaCorrespondente.GrupoMatriz?.Nome ?? ""
                        });
                    }
                    else
                    {
                        var disciplina = disciplinasAgrupadas.FirstOrDefault(da => da.CodigoComponenteCurricular.Equals(id)) ?? (await repositorioComponenteCurricular
                            .ObterDisciplinasPorIds(new long[] { id })).FirstOrDefault();

                        if (disciplina != null)
                        {
                            disciplina.RegistraFrequencia = await mediator
                                .Send(new ObterComponenteRegistraFrequenciaQuery(disciplina.CodigoComponenteCurricular));

                            disciplinasRetorno.Add(disciplina);
                        }
                    }
                }
            }

            return disciplinasRetorno;
        }

        private async Task<Usuario> RetornarUsuario()
        {
            try
            {
               return await mediator.Send(new ObterUsuarioLogadoQuery());
            } catch(Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Erro ao obter usuario obter componentes curriculares por ids Motivo: {ex.Message}", LogNivel.Critico, LogContexto.Usuario, ex.Message));
                throw;
            }
        }
    }
}
