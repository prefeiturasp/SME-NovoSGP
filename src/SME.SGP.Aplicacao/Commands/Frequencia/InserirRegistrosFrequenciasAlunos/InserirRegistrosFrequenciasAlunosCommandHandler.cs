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
        private readonly IRepositorioFrequenciaPreDefinida repositorioFrequenciaPreDefinida;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMediator mediator;

        public InserirRegistrosFrequenciasAlunosCommandHandler(IRepositorioRegistroFrequenciaAluno repositorioRegistroFrequenciaAluno,
            IRepositorioFrequenciaPreDefinida repositorioFrequenciaPreDefinida,
            IUnitOfWork unitOfWork, IMediator mediator)
        {
            this.repositorioRegistroFrequenciaAluno = repositorioRegistroFrequenciaAluno ?? throw new ArgumentNullException(nameof(repositorioRegistroFrequenciaAluno));
            this.repositorioFrequenciaPreDefinida = repositorioFrequenciaPreDefinida ?? throw new ArgumentNullException(nameof(repositorioFrequenciaPreDefinida));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(InserirRegistrosFrequenciasAlunosCommand request, CancellationToken cancellationToken)
        {
            using (var transacao = unitOfWork.IniciarTransacao())
            {
                await mediator.Send(new ExcluirFrequenciasAlunoPorRegistroFrequenciaIdCommand(request.RegistroFrequenciaId));
                await mediator.Send(new ExcluirPreDefinicaoFrequenciaCommand(request.TurmaId, request.ComponenteCurricularId));
                try
                {
                    foreach (var frequencia in request.Frequencias)
                    {
                        foreach (var aulaRegistrada in frequencia.Aulas)
                        {
                            var presenca = (int)aulaRegistrada.TipoFrequencia != 0 ? aulaRegistrada.TipoFrequencia :
                                ((int)frequencia.TipoFrequenciaPreDefinido != 0 ? frequencia.TipoFrequenciaPreDefinido : TipoFrequencia.C);
                            var entidade = new RegistroFrequenciaAluno()
                            {
                                CodigoAluno = frequencia.CodigoAluno,
                                NumeroAula = aulaRegistrada.NumeroAula,
                                Valor = (int)presenca,
                                RegistroFrequenciaId = request.RegistroFrequenciaId,
                            };
                            await repositorioRegistroFrequenciaAluno.SalvarAsync(entidade);

                            var frequenciaPreDefinida = new FrequenciaPreDefinida()
                            {
                                CodigoAluno = frequencia.CodigoAluno,
                                TurmaId = request.TurmaId,
                                ComponenteCurricularId = request.ComponenteCurricularId,
                                TipoFrequencia = frequencia.TipoFrequenciaPreDefinido != 0 ? frequencia.TipoFrequenciaPreDefinido : TipoFrequencia.C
                            };
                            await repositorioFrequenciaPreDefinida.Salvar(frequenciaPreDefinida);
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
