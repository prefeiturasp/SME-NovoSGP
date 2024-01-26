﻿using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.Metrica.Worker.Commands;
using SME.SGP.Metrica.Worker.Repositorios.Interfaces;
using SME.SGP.Metrica.Worker.UseCases.Interfaces;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Metrica.Worker.UseCases
{
    public class AulasSemAtribuicaoSubstituicaoMensalUseCase : IAulasSemAtribuicaoSubstituicaoMensalUseCase
    {
        private readonly IRepositorioSGPConsulta repositorioSGP;
        private readonly IMediator mediator;

        public AulasSemAtribuicaoSubstituicaoMensalUseCase(IRepositorioSGPConsulta repositorioSGP, IMediator mediator)
        {
            this.repositorioSGP = repositorioSGP ?? throw new ArgumentNullException(nameof(repositorioSGP));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var parametro = mensagem.EhNulo() || mensagem.Mensagem.EhNulo()
                            ? new FiltroDataMetricasDto(DateTime.Now.Date.AddDays(-1))
                            : mensagem.ObterObjetoMensagem<FiltroDataMetricasDto>();
            var ues = await repositorioSGP.ObterUesCodigo();
            foreach (var ue in ues)
                await mediator.Send(new PublicarFilaCommand(Rotas.RotasRabbitMetrica.AulasSemAtribuicaoSubstituicaoUEMensais, new FiltroCodigoDataMetricasDto(ue, parametro.Data, parametro.IgnorarRecheckCargaMetricas)));
            return true;
        }
    }
}
