using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDiarioDeBordoPorIdQueryHandler : IRequestHandler<ObterDiarioDeBordoPorIdQuery, DiarioBordoDetalhesDto>
    {
        private readonly IRepositorioDiarioBordo repositorioDiarioBordo;
        private readonly IConsultasDisciplina consultasDisciplina;
        private readonly IRepositorioDiarioBordoObservacao repositorioDiarioBordoObservacao;
        private readonly IMediator mediator;

        public ObterDiarioDeBordoPorIdQueryHandler(IRepositorioDiarioBordo repositorioDiarioBordo, IConsultasDisciplina consultasDisciplina, IMediator mediator, IRepositorioDiarioBordoObservacao repositorioDiarioBordoObservacao)
        {
            this.consultasDisciplina = consultasDisciplina ?? throw new ArgumentNullException(nameof(consultasDisciplina));
            this.repositorioDiarioBordo = repositorioDiarioBordo ?? throw new ArgumentNullException(nameof(repositorioDiarioBordo));
            this.repositorioDiarioBordoObservacao = repositorioDiarioBordoObservacao ?? throw new ArgumentNullException(nameof(repositorioDiarioBordoObservacao));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<DiarioBordoDetalhesDto> Handle(ObterDiarioDeBordoPorIdQuery request, CancellationToken cancellationToken)
        {
            var diariosBordo = await repositorioDiarioBordo.ObterDiariosDaMesmaAulaPorId(request.Id);

            long componenteCurricularIdPrincipal = 0;

            Aula aula = await mediator.Send(new ObterAulaPorIdQuery(diariosBordo.FirstOrDefault(diario => diario.Id == request.Id).AulaId));
            
            if (aula != null || !aula.Excluido)
                componenteCurricularIdPrincipal = await RetornaComponenteCurricularIdPrincipalDoProfessor(aula.TurmaId, long.Parse(aula.DisciplinaId));

            if (componenteCurricularIdPrincipal == 0)
                throw new NegocioException($"Componente Curricular não encontrado");

            if (diariosBordo.All(b => b.ComponenteCurricularId != componenteCurricularIdPrincipal))
                componenteCurricularIdPrincipal = diariosBordo.FirstOrDefault().ComponenteCurricularId;

            var usuario = await mediator.Send(ObterUsuarioLogadoIdQuery.Instance);
            var diarioBordo = diariosBordo.FirstOrDefault(diario => diario.ComponenteCurricularId == componenteCurricularIdPrincipal);
            var componentes = await mediator.Send(new ObterComponentesCurricularesPorIdsQuery(diariosBordo.Select(diario => diario.ComponenteCurricularId).ToArray()));
            var diarioIrmao = diariosBordo.FirstOrDefault(diario => diario.ComponenteCurricularId != componenteCurricularIdPrincipal);
            var observacoes = diarioBordo != null ? await mediator.Send(new ListarObservacaoDiarioBordoQuery(diarioBordo.Id, usuario)) : null;
            var observacoesComUsuariosNotificados = observacoes != null ? await ObterUsuariosNotificados(observacoes) : null;
            
            return MapearParaDto(diarioBordo, observacoesComUsuariosNotificados, diarioIrmao, componentes, componenteCurricularIdPrincipal);
        }

        private async Task<long> RetornaComponenteCurricularIdPrincipalDoProfessor(string turmaCodigo, long componenteCurricularId)
        {
            var disciplinas = await consultasDisciplina.ObterComponentesCurricularesPorProfessorETurma(turmaCodigo, false, false, false);

            if (disciplinas != null && disciplinas.Any())
            {
                if (disciplinas.Count > 1)
                {
                    var disciplina = disciplinas.Where(b => b.CodigoComponenteCurricular == componenteCurricularId);

                    if (disciplina == null)
                        return 0;

                    if (disciplina.Any())
                        return disciplina.FirstOrDefault().CodigoComponenteCurricular;
                    else
                        return (long)disciplinas.FirstOrDefault().CdComponenteCurricularPai;
                }

                return disciplinas.FirstOrDefault().CodigoComponenteCurricular;
            }
            return 0;
        }

        private async Task<IEnumerable<ListarObservacaoDiarioBordoDto>> ObterUsuariosNotificados(IEnumerable<ListarObservacaoDiarioBordoDto> observacoes)
        {
            var listaObservacoes = new List<ListarObservacaoDiarioBordoDto>();
            foreach (var item in observacoes)
            {
                var usuariosNotificados = await repositorioDiarioBordoObservacao.ObterNomeUsuariosNotificadosObservacao(item.Id);
                item.QtdUsuariosNotificados = usuariosNotificados.Count();
                item.NomeUsuariosNotificados = string.Join(", ", usuariosNotificados);
                listaObservacoes.Add(item);
            }

            return listaObservacoes;
        }
        private DiarioBordoDetalhesDto MapearParaDto(
                                                    Dominio.DiarioBordo diarioBordo, 
                                                    IEnumerable<ListarObservacaoDiarioBordoDto> observacoes,
                                                    DiarioBordo diarioBordoIrmao, 
                                                    IEnumerable<DisciplinaDto> disciplinas, 
                                                    long componenteCurricularIdPrincipal)
        {
            return new DiarioBordoDetalhesDto()
            {
                Auditoria = (AuditoriaDto)diarioBordo,
                AulaId = diarioBordo.AulaId,
                DevolutivaId = diarioBordo.DevolutivaId,
                Excluido = diarioBordo.Excluido,
                Id = diarioBordo.Id,
                Migrado = diarioBordo.Migrado,
                Planejamento = diarioBordo.Planejamento,
                InseridoCJ = diarioBordo.InseridoCJ,
                NomeComponente = disciplinas.FirstOrDefault(disciplina => disciplina.CodigoComponenteCurricular == componenteCurricularIdPrincipal)?.NomeComponenteInfantil,
                NomeComponenteIrmao = diarioBordoIrmao.NaoEhNulo() ? disciplinas.FirstOrDefault(disciplina => disciplina.CodigoComponenteCurricular != componenteCurricularIdPrincipal)?.NomeComponenteInfantil : string.Empty,
                PlanejamentoIrmao = diarioBordoIrmao?.Planejamento,
                Observacoes = observacoes.NaoEhNulo() ? observacoes.Select(obs =>
                {
                    return new ObservacaoNotificacoesDiarioBordoDto()
                    {
                        Auditoria = obs.Auditoria,
                        Id = obs.Id,
                        Observacao = obs.Observacao,
                        QtdUsuariosNotificacao = obs.QtdUsuariosNotificados,
                        NomeUsuariosNotificados = obs.NomeUsuariosNotificados,
                        Proprietario = obs.Proprietario
                    };
                }) : null
            };
        }
    }
}

