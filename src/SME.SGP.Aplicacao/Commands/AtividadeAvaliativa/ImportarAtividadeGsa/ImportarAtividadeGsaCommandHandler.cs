using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ImportarAtividadeGsaCommandHandler : AsyncRequestHandler<ImportarAtividadeGsaCommand>
    {
        private readonly IMediator mediator;
        public ImportarAtividadeGsaCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        protected override async Task Handle(ImportarAtividadeGsaCommand request, CancellationToken cancellationToken)
        {
            await ValidarImportacaoAtividade(request.AtividadeGsa.DataCriacao);

            var aula = await mediator.Send(new ObterAulaPorCodigoTurmaComponenteEDataQuery(request.AtividadeGsa.TurmaId, request.AtividadeGsa.ComponenteCurricularId.ToString(), request.AtividadeGsa.DataCriacao));

            if (ReagendarImportacao(aula))
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpAgendamento.RotaAtividadesSync,
                                                               new MensagemAgendamentoSyncDto(RotasRabbitSgp.RotaAtividadesSync, request.AtividadeGsa)));
            else
            {
                if (aula.EhModalidadeInfantil)
                    await SalvarAtividadeInfantil(request, aula);
                else
                {
                    await ValidarLancamentoNotaComponente(request.AtividadeGsa.ComponenteCurricularId);
                    await SalvarAtividadeAvaliativa(request, aula);
                }
            }
        }

        private async Task SalvarAtividadeInfantil(ImportarAtividadeGsaCommand request, DataAulaDto aula)
        {
            await mediator.Send(new SalvarAtividadeInfantilCommand(aula.AulaId,
                                                                  request.AtividadeGsa.UsuarioRf,
                                                                  request.AtividadeGsa.Titulo,
                                                                  request.AtividadeGsa.Descricao,
                                                                  request.AtividadeGsa.DataCriacao,
                                                                  request.AtividadeGsa.DataAlteracao,
                                                                  request.AtividadeGsa.AtividadeClassroomId,
                                                                  request.AtividadeGsa.Email
                                                                  ));
        }

        private async Task SalvarAtividadeAvaliativa(ImportarAtividadeGsaCommand request, DataAulaDto aula)
        {
            await mediator.Send(new SalvarAtividadeAvaliativaGsaCommand(aula.DataAula,
                                                                  request.AtividadeGsa.UsuarioRf,
                                                                  request.AtividadeGsa.TurmaId,
                                                                  request.AtividadeGsa.ComponenteCurricularId,
                                                                  request.AtividadeGsa.Titulo,
                                                                  request.AtividadeGsa.Descricao,
                                                                  request.AtividadeGsa.DataCriacao,
                                                                  request.AtividadeGsa.DataAlteracao,
                                                                  request.AtividadeGsa.AtividadeClassroomId
                                                                  ));
        }

        private async Task ValidarLancamentoNotaComponente(long componenteCurricularId)
        {
            if (!await mediator.Send(new ObterComponenteLancaNotaQuery(componenteCurricularId)))
                throw new NegocioException($"Componentes que não lançam nota não terão atividades avaliativas importada do classroom. Componente Curricular: {componenteCurricularId}");
        }

        private async Task ValidarImportacaoAtividade(DateTime dataCriacao)
        {
            var anoAtual = DateTime.Now.Year;

            if (dataCriacao.Year < anoAtual)
                throw new NegocioException($"Atividade Avaliativa do Classroom de ano anterior não será importada. Data Atividade: {dataCriacao:dd/MM/yyyy}");

            var parametroInicioImportacao = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.AtualizacaoDeAtividadesAvaliativas, anoAtual));
            if (!DateTime.TryParse(parametroInicioImportacao.Valor, out var dataInicioImportacao))
                throw new Exception("Erro ao obter parâmetro de data de início de integração das atividades classroom");

            if (dataCriacao < dataInicioImportacao)
                throw new NegocioException($"Atividade Avaliativa Classroom com data anterior ao parâmetro de início da integração de atividades não será importada para o SGP. Data Atividade: {dataCriacao:dd/MM/yyyy}");
        }

        private bool ReagendarImportacao(DataAulaDto dataAula)
            => dataAula == null 
            || dataAula.AulaId == 0;
    }
}
