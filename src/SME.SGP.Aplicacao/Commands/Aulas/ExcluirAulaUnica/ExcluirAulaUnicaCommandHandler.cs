using MediatR;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirAulaUnicaCommandHandler : IRequestHandler<ExcluirAulaUnicaCommand, RetornoBaseDto>
    {
        public ExcluirAulaUnicaCommandHandler()
        {
        }

        public async Task<RetornoBaseDto> Handle(ExcluirAulaUnicaCommand request, CancellationToken cancellationToken)
        {
            //if (await repositorioAtividadeAvaliativa.VerificarSeExisteAvaliacao(aula.DataAula.Date, aula.UeId, aula.TurmaId, usuario.CodigoRf, aula.DisciplinaId))
            //    throw new NegocioException("Aula com avaliação vinculada. Para excluir esta aula primeiro deverá ser excluída a avaliação.");

            //await VerificaSeProfessorPodePersistirTurmaDisciplina(usuario.CodigoRf, aula.TurmaId, aula.DisciplinaId, aula.DataAula, usuario);

            //unitOfWork.IniciarTransacao();
            //try
            //{
            //    if (aula.WorkflowAprovacaoId.HasValue)
            //        await servicoWorkflowAprovacao.ExcluirWorkflowNotificacoes(aula.WorkflowAprovacaoId.Value);

            //    await comandosNotificacaoAula.Excluir(aula.Id);
            //    await servicoFrequencia.ExcluirFrequenciaAula(aula.Id);
            //    await comandosPlanoAula.ExcluirPlanoDaAula(aula.Id);

            //    aula.Excluido = true;
            //    await repositorioAula.SalvarAsync(aula);

            //    unitOfWork.PersistirTransacao();
            //}
            //catch (Exception)
            //{
            //    unitOfWork.Rollback();
            //    throw;
            //}
            return null;
        }
    }
}
