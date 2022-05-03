using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RegistrarFrequenciaTurmaEvasaoCommandHandler : IRequestHandler<RegistrarFrequenciaTurmaEvasaoCommand, long>
    {
        private readonly IRepositorioFrequenciaTurmaEvasao repositorioFrequenciaTurmaEvasao;

        public RegistrarFrequenciaTurmaEvasaoCommandHandler(IRepositorioFrequenciaTurmaEvasao repositorioFrequenciaTurmaEvasao)
        {
            this.repositorioFrequenciaTurmaEvasao = repositorioFrequenciaTurmaEvasao ?? throw new ArgumentNullException(nameof(repositorioFrequenciaTurmaEvasao));
        }

        public async Task<long> Handle(RegistrarFrequenciaTurmaEvasaoCommand request, CancellationToken cancellationToken)
        {
            return await repositorioFrequenciaTurmaEvasao.Inserir(new FrequenciaTurmaEvasao()
            {
                TurmaId = request.TurmaId,
                Mes = request.Mes,
                QuantidadeAlunosAbaixo50Porcento = request.QuantidadeAlunosAbaixo50Porcento,
                QuantidadeAlunos0Porcento = request.QuantidadeAlunox0Porcento
            });
        }
    }
}
