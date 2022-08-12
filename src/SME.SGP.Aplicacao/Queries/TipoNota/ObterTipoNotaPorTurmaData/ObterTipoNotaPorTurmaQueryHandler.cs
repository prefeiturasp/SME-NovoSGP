﻿using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTipoNotaPorTurmaQueryHandler : IRequestHandler<ObterTipoNotaPorTurmaQuery, TipoNota>
    {
        private readonly IServicoEol servicoEol;
        private readonly IRepositorioNotaTipoValorConsulta repositorioNotaTipoValor;
        private readonly IMediator mediator;

        public ObterTipoNotaPorTurmaQueryHandler(IServicoEol  servicoEol, IRepositorioNotaTipoValorConsulta repositorioNotaTipoValor, IMediator mediator)
        {
            this.servicoEol = servicoEol ?? throw new ArgumentNullException(nameof(servicoEol));
            this.repositorioNotaTipoValor = repositorioNotaTipoValor ?? throw new ArgumentNullException(nameof(repositorioNotaTipoValor));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<TipoNota> Handle(ObterTipoNotaPorTurmaQuery request, CancellationToken cancellationToken)
        {
            //TODO: TIPO DE TURMA NÃO EXISTE NO SGP, É NECESSÁRIO SEMPRE CONSULTAR O EOL.....
            var turmaEOL = await mediator.Send(new ObterDadosTurmaEolPorCodigoQuery(request.Turma.CodigoTurma));

            // Para turma tipo 2 o padrão é nota.
            if (turmaEOL.TipoTurma == TipoTurma.EdFisica)
                return TipoNota.Nota;

            var anoCicloModalidade = string.Empty;
            if (request.Turma != null)
                anoCicloModalidade = request.Turma.Ano == AnoCiclo.Alfabetizacao.Name() ? AnoCiclo.Alfabetizacao.Description() : request.Turma.Ano;            

            var ciclo = await mediator.Send(new ObterCicloPorAnoModalidadeQuery(anoCicloModalidade, request.Turma.ModalidadeCodigo));

            if (ciclo == null)
                throw new NegocioException("Não foi encontrado o ciclo da turma informada");

            var retorno = await mediator.Send(new ObterNotaTipoPorCicloIdDataAvalicacaoQuery(ciclo.Id, request.DataReferencia));
            return retorno?.TipoNota ?? TipoNota.Nota;            
        }
    }
}
