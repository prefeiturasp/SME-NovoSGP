﻿using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AlterarQuestaoMapeamentoEstudanteCommandHandler : IRequestHandler<AlterarQuestaoMapeamentoEstudanteCommand, bool>
    {
        private readonly IRepositorioQuestaoMapeamentoEstudante repositorioQuestao;

        public AlterarQuestaoMapeamentoEstudanteCommandHandler(IRepositorioQuestaoMapeamentoEstudante repositorioQuestao)
        {
            this.repositorioQuestao = repositorioQuestao ?? throw new ArgumentNullException(nameof(repositorioQuestao));
        }

        public async Task<bool> Handle(AlterarQuestaoMapeamentoEstudanteCommand request, CancellationToken cancellationToken)
        {
            await repositorioQuestao.SalvarAsync(request.Questao);
            return true;
        }
    }
}
