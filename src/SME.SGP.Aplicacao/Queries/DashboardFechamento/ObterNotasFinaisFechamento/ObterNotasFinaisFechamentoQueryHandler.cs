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
    public class ObterNotasFinaisFechamentoQueryHandler : IRequestHandler<ObterNotasFinaisFechamentoQuery, IEnumerable<FechamentoConselhoClasseNotaFinalDto>>
    {

        private readonly IRepositorioConselhoClasseConsulta repositorioConselhoClasseConsulta;

        public ObterNotasFinaisFechamentoQueryHandler(IRepositorioConselhoClasseConsulta repositorio)
        {
            this.repositorioConselhoClasseConsulta = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<IEnumerable<FechamentoConselhoClasseNotaFinalDto>> Handle(ObterNotasFinaisFechamentoQuery request,
            CancellationToken cancellationToken)
        {
            var retorno = await repositorioConselhoClasseConsulta.ObterNotasFechamentoOuConselhoAlunos(request.UeId,
                                                                          request.Ano,
                                                                          request.DreId,
                                                                          request.Modalidade,
                                                                          request.Semestre,
                                                                          request.Bimestre);

            return retorno;
        }
    }
}
