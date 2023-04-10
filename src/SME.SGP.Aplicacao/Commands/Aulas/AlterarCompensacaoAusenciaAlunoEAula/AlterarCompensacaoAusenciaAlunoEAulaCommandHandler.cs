using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class AlterarCompensacaoAusenciaAlunoEAulaCommandHandler : IRequestHandler<AlterarCompensacaoAusenciaAlunoEAulaCommand, bool>
    {
        private readonly IRepositorioCompensacaoAusenciaAlunoConsulta repositorioCompensacaoAusenciaAlunoConsulta;
        private readonly IUnitOfWork unitOfWork;
        private readonly IRepositorioCompensacaoAusenciaAluno repositorioCompensacaoAusenciaAluno;
        private readonly IRepositorioCompensacaoAusenciaAlunoAula repositorioCompensacaoAusenciaAlunoAula;

        public AlterarCompensacaoAusenciaAlunoEAulaCommandHandler(IRepositorioCompensacaoAusenciaAlunoConsulta repositorioCompensacaoAusenciaAlunoConsulta,
               IRepositorioCompensacaoAusenciaAluno repositorioCompensacaoAusenciaAluno, IUnitOfWork unitOfWork,
               IRepositorioCompensacaoAusenciaAlunoAula repositorioCompensacaoAusenciaAlunoAula)
        {
            this.repositorioCompensacaoAusenciaAlunoConsulta = repositorioCompensacaoAusenciaAlunoConsulta ?? throw new ArgumentNullException(nameof(repositorioCompensacaoAusenciaAlunoConsulta));
            this.repositorioCompensacaoAusenciaAluno = repositorioCompensacaoAusenciaAluno ?? throw new ArgumentNullException(nameof(repositorioCompensacaoAusenciaAluno));
            this.repositorioCompensacaoAusenciaAlunoAula = repositorioCompensacaoAusenciaAlunoAula ?? throw new ArgumentNullException(nameof(repositorioCompensacaoAusenciaAlunoAula));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<bool> Handle(AlterarCompensacaoAusenciaAlunoEAulaCommand request, CancellationToken cancellationToken)
        {
            var compensacoes = await repositorioCompensacaoAusenciaAlunoConsulta.ObterCompensacoesAusenciasAlunosPorRegistroFrequenciaAlunoIdQuery(request.RegistroFrequenciaAlunoIds);

            if (compensacoes.Any())
            {
                try
                {
                    unitOfWork.IniciarTransacao();
                    
                    await repositorioCompensacaoAusenciaAluno.AlterarQuantidadeFaltasCompensadasPorIds(compensacoes.Select(s => s.Id).ToArray(), request.QtdeFaltasCompensadas);
                    
                    await repositorioCompensacaoAusenciaAlunoAula.RemoverLogico(request.RegistroFrequenciaAlunoIds.ToArray());
                    
                    unitOfWork.PersistirTransacao();
                }
                catch (Exception e)
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }
            return true;
        }
    }
}
