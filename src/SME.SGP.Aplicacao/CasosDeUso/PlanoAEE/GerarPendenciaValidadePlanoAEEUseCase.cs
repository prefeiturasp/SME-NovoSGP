﻿using MediatR;
using Microsoft.Extensions.Configuration;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class GerarPendenciaValidadePlanoAEEUseCase : AbstractUseCase, IGerarPendenciaValidadePlanoAEEUseCase
    {
        private readonly IConfiguration configuration;

        public GerarPendenciaValidadePlanoAEEUseCase(IMediator mediator, IConfiguration configuration) : base(mediator)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            if (!await ParametroGeracaoPendenciasAtivo())
                return false;

            var dataFim = DateTime.Today.AddDays(-1);
            var planosEncerrados = await mediator.Send(new ObterPlanosAEEPorDataFimQuery(dataFim, false));

            foreach (var planoEncerrado in planosEncerrados)
            {
                await ExpirarPlano(planoEncerrado);
                await GerarPendenciaValidadePlano(planoEncerrado, dataFim);
            }

            return true;
        }

        private async Task ExpirarPlano(PlanoAEE plano)
        {
            plano.Situacao = SituacaoPlanoAEE.Expirado;
            await mediator.Send(new PersistirPlanoAEECommand(plano));
        }

        private async Task<bool> ParametroGeracaoPendenciasAtivo()
        {
            var parametro = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.GerarPendenciasPlanoAEE, DateTime.Today.Year));

            return parametro != null && parametro.Ativo;
        }

        private async Task GerarPendenciaValidadePlano(PlanoAEE planoEncerrado, DateTime dataFim)
        {
            var turma = await ObterTurma(planoEncerrado.TurmaId);

            var ueDre = $"{turma.Ue.TipoEscola.ShortName()} {turma.Ue.Nome} ({turma.Ue.Dre.Abreviacao})";
            var hostAplicacao = configuration["UrlFrontEnd"];
            var estudanteOuCrianca = turma.ModalidadeCodigo == Modalidade.EducacaoInfantil ? "da criança" : "do estudante";

            var titulo = $"Plano AEE Expirado - {planoEncerrado.AlunoNome} ({planoEncerrado.AlunoCodigo}) - {ueDre}";
            var descricao = $"O Plano AEE {estudanteOuCrianca} {planoEncerrado.AlunoNome} ({planoEncerrado.AlunoCodigo}) da turma {turma.NomeComModalidade()} da {ueDre} expirou em {dataFim:dd/MM/yyyy}. <br/><a href='{hostAplicacao}aee/plano/editar/{planoEncerrado.Id}'>Clique aqui para acessar o plano.</a> " +
                $"<br/><br/>Para resolver esta pendência você precisa alterar o vencimento do plano, criando uma nova versão ou encerrá-lo.";

            await mediator.Send(new GerarPendenciaPlanoAEECommand(planoEncerrado.Id, planoEncerrado.ResponsavelId, titulo, descricao, turma.UeId, turma.Id));
        }

        private async Task<Turma> ObterTurma(long turmaId)
        {
            var turma = await mediator.Send(new ObterTurmaComUeEDrePorIdQuery(turmaId));

            if (turma == null)
                throw new NegocioException($"Turma [{turmaId}] não localizada para geração da pendência de validade do plano");

            return turma;
        }
    }
}
