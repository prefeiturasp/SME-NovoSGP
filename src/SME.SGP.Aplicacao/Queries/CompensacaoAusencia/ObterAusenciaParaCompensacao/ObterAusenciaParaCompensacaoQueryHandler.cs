using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Org.BouncyCastle.Crypto.Tls;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterAusenciaParaCompensacaoQueryHandler : IRequestHandler<ObterAusenciaParaCompensacaoQuery, IEnumerable<RegistroFaltasNaoCompensadaDto>>
    {
        private readonly IRepositorioCompensacaoAusencia repositorioCompensacaoAusencia;
        private readonly IMediator mediator;

        public ObterAusenciaParaCompensacaoQueryHandler(IRepositorioCompensacaoAusencia repositorioCompensacao, IMediator mediator)
        {
            repositorioCompensacaoAusencia = repositorioCompensacao ?? throw new ArgumentNullException(nameof(repositorioCompensacao));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<RegistroFaltasNaoCompensadaDto>> Handle(ObterAusenciaParaCompensacaoQuery request, CancellationToken cancellationToken)
        {
            var usuarioLogado = await mediator.Send(ObterUsuarioLogadoQuery.Instance);
            var codigosComponentesConsiderados = new List<string>() { request.DisciplinaId };

            var faltasNaoCompensadas = await repositorioCompensacaoAusencia.ObterAusenciaParaCompensacao(
                request.CompensacaoId,
                request.TurmaCodigo,
                codigosComponentesConsiderados.ToArray(),
                request.AlunosQuantidadeCompensacoes.Select(t => t.CodigoAluno).Distinct().ToArray(),
                request.Bimestre);

            foreach (var alunoQuantidadeCompensar in request.AlunosQuantidadeCompensacoes)
            {
                var faltasNaoCompensadasAluno = faltasNaoCompensadas
                    .Where(t => t.CodigoAluno == alunoQuantidadeCompensar.CodigoAluno);

                var diferenca = alunoQuantidadeCompensar.QuantidadeCompensar - faltasNaoCompensadasAluno.Count(t => t.Sugestao);
                if (diferenca > 0)
                {
                    // -> adiciona como sugestão a quantidade faltante pegando da mais antiga para a mais nova.
                    foreach (var falta in faltasNaoCompensadasAluno.Where(t => !t.Sugestao).OrderBy(t => t.DataAula).ThenBy(t => t.NumeroAula).Take(diferenca))
                    {
                        falta.Sugestao = true;
                    }
                }
                else if (diferenca < 0)
                {
                    // -> remove as sugestões a quantidade a mais pegando da mais nova para a mais antiga.
                    foreach (var falta in faltasNaoCompensadasAluno.Where(t => t.Sugestao).OrderByDescending(t => t.DataAula).ThenByDescending(t => t.NumeroAula).Take(Math.Abs(diferenca)))
                    {
                        falta.Sugestao = false;
                    }
                }
            }

            return faltasNaoCompensadas
                .OrderBy(t => t.CodigoAluno)
                .ThenBy(t => t.DataAula)
                .ThenBy(t => t.NumeroAula);
        }

        private async Task<long> VerificarSeComponenteEhDeTerritorio(string turmaCodigo, long componenteCurricularId)
        {
            long codigoComponenteTerritorioCorrespondente = 0;
            var usuarioLogado = await mediator.Send(ObterUsuarioLogadoQuery.Instance);

            if (usuarioLogado.EhProfessor())
            {
                var componentesProfessor = await mediator.Send(new ObterComponentesCurricularesDoProfessorNaTurmaQuery(turmaCodigo, usuarioLogado.Login, usuarioLogado.PerfilAtual));
                var componenteCorrespondente = componentesProfessor.FirstOrDefault(cp => cp.Codigo.Equals(componenteCurricularId) || cp.CodigoComponenteTerritorioSaber.Equals(componenteCurricularId));
                codigoComponenteTerritorioCorrespondente = (componenteCorrespondente != null && componenteCorrespondente.TerritorioSaber && componenteCorrespondente.Codigo.Equals(componenteCurricularId) ? componenteCorrespondente.CodigoComponenteTerritorioSaber : componenteCorrespondente.Codigo);
            }
            else if (usuarioLogado.EhProfessorCj())
            {
                var turmaId = await mediator.Send(new ObterTurmaIdPorCodigoQuery(turmaCodigo));
                var professores = await mediator.Send(new ObterProfessoresTitularesPorTurmaIdQuery(turmaId));
                var professor = professores.FirstOrDefault(p => p.DisciplinasId.Contains(componenteCurricularId));
                if (professor != null)
                {
                    var componentesProfessor = await mediator.Send(new ObterComponentesCurricularesDoProfessorNaTurmaQuery(turmaCodigo, professor.ProfessorRf, Perfis.PERFIL_PROFESSOR));
                    var componenteProfessorRelacionado = componentesProfessor.FirstOrDefault(cp => cp.CodigoComponenteTerritorioSaber.Equals(componenteCurricularId));
                    if (componenteProfessorRelacionado != null)
                        codigoComponenteTerritorioCorrespondente = componenteProfessorRelacionado.Codigo;
                }
            }

            return codigoComponenteTerritorioCorrespondente;
        }
    }
}