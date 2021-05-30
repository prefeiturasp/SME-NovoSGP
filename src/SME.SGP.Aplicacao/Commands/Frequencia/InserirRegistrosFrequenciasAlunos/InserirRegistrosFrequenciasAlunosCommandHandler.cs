using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class InserirRegistrosFrequenciasAlunosCommandHandler : IRequestHandler<InserirRegistrosFrequenciasAlunosCommand, bool>
    {
        private readonly IRepositorioRegistroFrequenciaAluno repositorioRegistroFrequenciaAluno;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMediator mediator;

        public InserirRegistrosFrequenciasAlunosCommandHandler(IRepositorioRegistroFrequenciaAluno repositorioRegistroFrequenciaAluno, 
            IUnitOfWork unitOfWork, IMediator mediator)
        {
            this.repositorioRegistroFrequenciaAluno = repositorioRegistroFrequenciaAluno ?? throw new ArgumentNullException(nameof(repositorioRegistroFrequenciaAluno));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(InserirRegistrosFrequenciasAlunosCommand request, CancellationToken cancellationToken)
        {
            using (var transacao = unitOfWork.IniciarTransacao())
            {
                await mediator.Send(new ExcluirFrequenciasAlunoPorRegistroFrequenciaIdCommand(request.RegistroFrequenciaId));
                try
                {
                    foreach (var frequencia in request.Frequencias)
                    {
                        foreach (var aulaRegistrada in frequencia.Aulas)
                        {
                            var presenca = (int)aulaRegistrada.TipoFrequencia != 0 ? (int)aulaRegistrada.TipoFrequencia : 
                                ((int)frequencia.TipoFrequenciaPreDefinido != 0 ? (int)frequencia.TipoFrequenciaPreDefinido : (int)TipoFrequencia.C);
                            var entidade = new RegistroFrequenciaAluno()
                            {
                                CodigoAluno = frequencia.CodigoAluno,
                                NumeroAula = aulaRegistrada.NumeroAula,
                                Valor = presenca,
                                RegistroFrequenciaId = request.RegistroFrequenciaId,
                            };
                            await repositorioRegistroFrequenciaAluno.SalvarAsync(entidade);
                        }
                    }
                    unitOfWork.PersistirTransacao();
                }
                catch (Exception ex)
                {
                    unitOfWork.Rollback();
                    return false;
                }
            }

            return true;
        }
    }
}
