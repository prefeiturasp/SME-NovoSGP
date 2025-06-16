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
        private readonly IRepositorioAnotacaoFrequenciaAlunoConsulta repositorioAnotacaoFrequenciaAluno;
        private readonly IRepositorioDiarioBordo repositorioDiarioBordo;
        private readonly IRepositorioPlanoAula repositorioPlanoAula;
        
        public ExcluirAulaUnicaCommandHandler(IMediator mediator,
                                              IRepositorioAula repositorioAula,IRepositorioAnotacaoFrequenciaAlunoConsulta repositorioAnotacaoFrequenciaAluno,IRepositorioDiarioBordo repositorioDiarioBordo
                                              ,IRepositorioPlanoAula repositorioPlanoAula)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioAula = repositorioAula ?? throw new ArgumentNullException(nameof(repositorioAula));
            this.repositorioAnotacaoFrequenciaAluno = repositorioAnotacaoFrequenciaAluno ?? throw new ArgumentNullException(nameof(repositorioAnotacaoFrequenciaAluno));
            this.repositorioDiarioBordo = repositorioDiarioBordo ?? throw new ArgumentNullException(nameof(repositorioDiarioBordo));
            this.repositorioPlanoAula = repositorioPlanoAula ?? throw new ArgumentNullException(nameof(repositorioPlanoAula));
        }

        public async Task<RetornoBaseDto> Handle(ExcluirAulaUnicaCommand request, CancellationToken cancellationToken)
        {
            var aula = request.Aula;

            if (await mediator.Send(new AulaPossuiAvaliacaoQuery(aula, request.Usuario.CodigoRf), cancellationToken))
                throw new NegocioException("Aula com avaliação vinculada. Para excluir esta aula primeiro deverá ser excluída a avaliação.");            

            if (!request.Usuario.EhGestorEscolar())
                await ValidarComponentesDoProfessor(aula.TurmaId, long.Parse(aula.DisciplinaId), aula.DataAula, request.Usuario);

            if (aula.WorkflowAprovacaoId.HasValue)
                await PulicaFilaSgp(RotasRabbitSgp.WorkflowAprovacaoExcluir, aula.WorkflowAprovacaoId.Value, request.Usuario);

            var filas = new []
            {
                RotasRabbitSgpAula.NotificacoesDaAulaExcluir,
                RotasRabbitSgpFrequencia.FrequenciaDaAulaExcluir,
                RotasRabbitSgpAula.PlanoAulaDaAulaExcluir,
                RotasRabbitSgpFrequencia.AnotacoesFrequenciaDaAulaExcluir,
                RotasRabbitSgp.DiarioBordoDaAulaExcluir,
                RotasRabbitSgpAula.RotaExecutaExclusaoPendenciasAula,
                RotasRabbitSgpAula.RotaExecutaExclusaoPendenciaDiarioBordoAula
            };

            await PulicaFilaSgp(filas, aula.Id, request.Usuario);

            aula.Excluido = true;
            await repositorioAula.SalvarAsync(aula);

            await mediator.Send(new ExcluirCompensacaoAusenciaAlunoEAulaPorAulaIdCommand(aula.Id),cancellationToken);
            
            await mediator.Send(new RecalcularFrequenciaPorTurmaCommand(aula.TurmaId, aula.DisciplinaId, aula.Id), cancellationToken);

            await ExcluirArquivoAnotacaoFrequencia(request.Aula.Id);
            await ExcluirArquivosPlanoAula(request.Aula.Id);
            await RemoverArquivosDiarioBordo(request.Aula.Id);

            var retorno = new RetornoBaseDto();
            retorno.Mensagens.Add("Aula excluída com sucesso.");

            return retorno;
        }

        private async Task ExcluirArquivosPlanoAula(long aulaId)
        {
            var plano = await repositorioPlanoAula.ObterPlanoAulaPorAulaRegistroExcluido(aulaId);

            if (plano.NaoEhNulo())
            {
                await ExcluirArquivo(plano.Descricao, TipoArquivo.PlanoAula);
                await ExcluirArquivo(plano.RecuperacaoAula, TipoArquivo.PlanoAulaRecuperacao);
                await ExcluirArquivo(plano.LicaoCasa, TipoArquivo.PlanoAulaLicaoCasa); 
            }
        }

        private async Task RemoverArquivosDiarioBordo(long aulaId)
        {
            var diariosDeBordos = await repositorioDiarioBordo.ObterPorAulaId(aulaId);

            foreach (var diarioDeBordo in diariosDeBordos)
            {
                if((diarioDeBordo?.Planejamento).NaoEhNulo())
                    await ExcluirArquivo(diarioDeBordo.Planejamento,TipoArquivo.DiarioBordo);
            }
        }
        
        private async Task ExcluirArquivoAnotacaoFrequencia(long aulaId)
        {
            var anotacaoFrequencia = await repositorioAnotacaoFrequenciaAluno.ObterPorAulaIdRegistroExcluido(aulaId);
            foreach (var item in anotacaoFrequencia)
            {
                await ExcluirArquivo(item.Anotacao,TipoArquivo.FrequenciaAnotacaoEstudante);
            }
        }
        private async Task ExcluirArquivo(string mensagem,TipoArquivo tipo)
        {
            if (!string.IsNullOrEmpty(mensagem))
            {
                await mediator.Send(new RemoverArquivosExcluidosCommand(mensagem, string.Empty, tipo.Name()));
            }
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
