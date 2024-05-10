﻿using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RegistrarHistoricoDeAlteracaoDeImpressaoDoEncaminhamentoNAAPACommandHandler : IRequestHandler<RegistrarHistoricoDeAlteracaoDeImpressaoDoEncaminhamentoNAAPACommand, bool>
    {
        private readonly IRepositorioEncaminhamentoNAAPAHistoricoAlteracoes repositorioEncaminhamentoNAAPAHistoricoAlteracoes;

        public RegistrarHistoricoDeAlteracaoDeImpressaoDoEncaminhamentoNAAPACommandHandler(IRepositorioEncaminhamentoNAAPAHistoricoAlteracoes repositorioEncaminhamentoNAAPAHistoricoAlteracoes)
        {
            this.repositorioEncaminhamentoNAAPAHistoricoAlteracoes = repositorioEncaminhamentoNAAPAHistoricoAlteracoes ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoNAAPAHistoricoAlteracoes));
        }

        public async Task<bool> Handle(RegistrarHistoricoDeAlteracaoDeImpressaoDoEncaminhamentoNAAPACommand request, CancellationToken cancellationToken)
        {

            foreach(var encaminhamentoId in request.EncaminhamentoNaapaIds)
            {
                var historico = new EncaminhamentoNAAPAHistoricoAlteracoes()
                {
                    EncaminhamentoNAAPAId = encaminhamentoId,
                    DataHistorico = DateTimeExtension.HorarioBrasilia(),
                    TipoHistorico = TipoHistoricoAlteracoesEncaminhamentoNAAPA.Impressao,
                    UsuarioId = request.UsuarioLogadoId
                };

                await repositorioEncaminhamentoNAAPAHistoricoAlteracoes.SalvarAsync(historico);
            }

            return true;
        }
    }
}
