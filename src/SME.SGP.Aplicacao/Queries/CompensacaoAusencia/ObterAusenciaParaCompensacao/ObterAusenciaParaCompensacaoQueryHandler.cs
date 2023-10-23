﻿using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
            var professorConsiderado = (string)null;
            var codigosComponentesConsiderados = new List<string>() { request.DisciplinaId };
            var codigosTerritorioEquivalentes = await mediator
                .Send(new ObterCodigosComponentesCurricularesTerritorioSaberEquivalentesPorTurmaQuery(long.Parse(request.DisciplinaId), request.TurmaCodigo, usuarioLogado.EhProfessor() ? usuarioLogado.Login : null));

            if (codigosTerritorioEquivalentes != null && codigosTerritorioEquivalentes.Any())
            {
                codigosComponentesConsiderados.AddRange(codigosTerritorioEquivalentes.Select(c => c.codigoComponente).Except(codigosComponentesConsiderados));
                professorConsiderado = codigosTerritorioEquivalentes.First().professor;
            }

            var faltasNaoCompensadas = await repositorioCompensacaoAusencia.ObterAusenciaParaCompensacao(
                request.CompensacaoId,
                request.TurmaCodigo,
                codigosComponentesConsiderados.ToArray(),
                request.AlunosQuantidadeCompensacoes.Select(t => t.CodigoAluno).Distinct().ToArray(),
                request.Bimestre,
                professorConsiderado);

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
    }
}