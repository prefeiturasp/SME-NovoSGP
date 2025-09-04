using MediatR;
using SME.SGP.Aplicacao.Commands.PainelEducacional.ObterIdep;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterIdepCommandHandler : IRequestHandler<ObterIdepCommand, IEnumerable<PainelEducacionalConsolidacaoIdep>>
    {
        private readonly IRepositorioIdepPainelEducacionalConsulta repositorioIdepConsulta;

        public ObterIdepCommandHandler(IRepositorioIdepPainelEducacionalConsulta repositorioIdepConsulta)
        {
            this.repositorioIdepConsulta = repositorioIdepConsulta ?? throw new ArgumentNullException(nameof(repositorioIdepConsulta));
        }

        public async Task<IEnumerable<PainelEducacionalConsolidacaoIdep>> Handle(ObterIdepCommand request, CancellationToken cancellationToken)
        {
            return await repositorioIdepConsulta.ObterTodosIdep();
        }
    }
}