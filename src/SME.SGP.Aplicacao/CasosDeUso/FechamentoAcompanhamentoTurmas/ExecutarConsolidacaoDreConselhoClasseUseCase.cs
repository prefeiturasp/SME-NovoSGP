using MediatR;
using Minio.DataModel;
using Newtonsoft.Json;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutarConsolidacaoDreConselhoClasseUseCase : AbstractUseCase, IExecutarConsolidacaoDreConselhoClasseUseCase
    {
        private readonly IRepositorioUeConsulta repositorioUeConsulta;
        public ExecutarConsolidacaoDreConselhoClasseUseCase(IMediator mediator, IRepositorioUeConsulta repositorioUeConsulta) : base(mediator)
        {
            this.repositorioUeConsulta = repositorioUeConsulta;
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var filtro = mensagemRabbit.ObterObjetoMensagem<MensagemConsolidarTurmaConselhoClasseAlunoPorDreAnoDto>();
            try
            {
                var ues = await ObterUes(filtro.DreId);
                foreach (var ue in ues)
                {
                    var mensagemPorUe = new MensagemConsolidarTurmaConselhoClasseAlunoPorUeAnoDto(ue, filtro.AnoLetivo);
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpFechamento.ConsolidarUeConselhoClasseSync, JsonConvert.SerializeObject(mensagemPorUe), mensagemRabbit.CodigoCorrelacao, null));
                }
                return true;
            }
            catch (System.Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Não foi possível executar a consolidacao turma conselho classe aluno por Dre/ano.", LogNivel.Critico, LogContexto.ConselhoClasse, ex.Message));
                throw;
            }
        }

        private async Task<IEnumerable<long>> ObterUes(long idDre)
        {
            if (idDre != 0)
                return await repositorioUeConsulta.ObterIdsPorDre(idDre);
            else
                return  await repositorioUeConsulta.ObterTodosIds();

        }
    }
}
