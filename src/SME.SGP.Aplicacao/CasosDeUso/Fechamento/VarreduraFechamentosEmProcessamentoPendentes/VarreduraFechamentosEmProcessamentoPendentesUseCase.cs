using MediatR;
using Sentry;
using SME.SGP.Dominio;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class VarreduraFechamentosEmProcessamentoPendentesUseCase : IVarreduraFechamentosEmProcessamentoPendentesUseCase
    {
        private readonly IMediator mediator;
        private readonly IUnitOfWork unitOfWork;
        private readonly IComandosFechamentoTurmaDisciplina comandosFechamentoTurmaDisciplina;

        public VarreduraFechamentosEmProcessamentoPendentesUseCase(IMediator mediator,
                                                                   IUnitOfWork unitOfWork,
                                                                   IComandosFechamentoTurmaDisciplina comandosFechamentoTurmaDisciplina)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.comandosFechamentoTurmaDisciplina = comandosFechamentoTurmaDisciplina ?? throw new ArgumentNullException(nameof(comandosFechamentoTurmaDisciplina));
        }

        public async Task Executar()
        {
            try
            {
                var listaFechamentoTurmaDisciplinaEmDuplicidade = await mediator
                    .Send(new ObterFechamentosTurmaDisciplinasDuplicadosQuery(null));

                var listaFechamentoTurmaDisciplinaExpiradosReprocessar = await mediator
                    .Send(new ObterFechamentosTurmaDisciplinaEmProcessamentoTempoExpiradoQuery(null, null));

                unitOfWork
                    .IniciarTransacao();

                await mediator
                    .Send(new ExcluirLogicamenteFechamentosTurmaDisciplinaCommand(listaFechamentoTurmaDisciplinaEmDuplicidade.ToArray()));

                foreach (var fechamentoAtual in listaFechamentoTurmaDisciplinaExpiradosReprocessar)
                {
                    var usuario = await mediator
                        .Send(new ObterUsuarioPorRfOuCriaQuery(fechamentoAtual.codigoRf));

                    await comandosFechamentoTurmaDisciplina
                        .Reprocessar(fechamentoAtual.fechamentoTurmaDisciplinaId, usuario);

                    unitOfWork
                        .PersistirTransacao();
                }
            }
            catch (Exception ex)
            {
                unitOfWork.Rollback();
                SentrySdk.CaptureException(ex);
            }
        }
    }
}
