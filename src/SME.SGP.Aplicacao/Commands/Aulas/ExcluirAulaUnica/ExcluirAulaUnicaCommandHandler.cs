using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirAulaUnicaCommandHandler : IRequestHandler<ExcluirAulaUnicaCommand, RetornoBaseDto>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioAula repositorioAula;

        public ExcluirAulaUnicaCommandHandler(IMediator mediator,
                                              IRepositorioAula repositorioAula)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioAula = repositorioAula ?? throw new ArgumentNullException(nameof(repositorioAula));
        }

        public async Task<RetornoBaseDto> Handle(ExcluirAulaUnicaCommand request, CancellationToken cancellationToken)
        {
            var aula = await repositorioAula.ObterPorIdAsync(request.AulaId);

            if (await mediator.Send(new AulaPossuiAvaliacaoQuery(aula, request.Usuario.CodigoRf)))
                throw new NegocioException("Aula com avaliação vinculada. Para excluir esta aula primeiro deverá ser excluída a avaliação.");

            if(!request.Usuario.EhGestorEscolar())
                await ValidarComponentesDoProfessor(aula.TurmaId, long.Parse(aula.DisciplinaId), aula.DataAula, request.Usuario);

            if (aula.WorkflowAprovacaoId.HasValue)
                await PulicaFilaSgp(RotasRabbitSgp.WorkflowAprovacaoExcluir, aula.WorkflowAprovacaoId.Value, request.Usuario);

            var filas = new string[]
            {
                RotasRabbitSgp.NotificacoesDaAulaExcluir,
                RotasRabbitSgp.FrequenciaDaAulaExcluir,
                RotasRabbitSgp.PlanoAulaDaAulaExcluir,
                RotasRabbitSgp.AnotacoesFrequenciaDaAulaExcluir,
                RotasRabbitSgp.DiarioBordoDaAulaExcluir,
                RotasRabbitSgp.RotaExecutaExclusaoPendenciasAula
            };

            await PulicaFilaSgp(filas, aula.Id, request.Usuario);

            aula.Excluido = true;
            await repositorioAula.SalvarAsync(aula);

            await mediator.Send(new RecalcularFrequenciaPorTurmaCommand(aula.TurmaId, aula.DisciplinaId, aula.Id));

            var retorno = new RetornoBaseDto();
            retorno.Mensagens.Add("Aula excluída com sucesso.");
            return retorno;
        }

        private async Task PulicaFilaSgp(string fila, long id, Usuario usuario)
        {
            await mediator.Send(new PublicarFilaSgpCommand(fila, new FiltroIdDto(id), Guid.NewGuid(), usuario));
        }

        private async Task PulicaFilaSgp(string[] filas, long id, Usuario usuario)
        {
            var commands = new List<PublicarFilaSgpCommand>();

            filas.ToList()
                .ForEach(f => commands.Add(new PublicarFilaSgpCommand(f, new FiltroIdDto(id), Guid.NewGuid(), usuario)));

            await mediator.Send(new PublicarFilaEmLoteSgpCommand(commands));
        }

        private async Task ValidarComponentesDoProfessor(string codigoTurma, long componenteCurricularCodigo, DateTime dataAula, Usuario usuario)
        {
            var resultadoValidacao = await mediator
                .Send(new ValidarComponentesDoProfessorCommand(usuario, codigoTurma, componenteCurricularCodigo, dataAula));

            if (!resultadoValidacao.resultado)
                throw new NegocioException(resultadoValidacao.mensagem);
        }
    }
}
