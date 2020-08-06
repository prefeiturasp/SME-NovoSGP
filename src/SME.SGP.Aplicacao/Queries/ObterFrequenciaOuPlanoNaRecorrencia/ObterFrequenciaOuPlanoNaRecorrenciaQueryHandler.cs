using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciaOuPlanoNaRecorrenciaQueryHandler : IRequestHandler<ObterFrequenciaOuPlanoNaRecorrenciaQuery, bool>
    {
        private readonly IRepositorioAula repositorioAula;
        private readonly IRepositorioFrequencia repositorioFrequencia;
        private readonly IRepositorioPlanoAula repositorioPlanoAula;
        private readonly IRepositorioDiarioBordo repositorioDiarioBordo;

        public ObterFrequenciaOuPlanoNaRecorrenciaQueryHandler(IRepositorioAula repositorioAula,
                                                               IRepositorioFrequencia repositorioFrequencia,
                                                               IRepositorioPlanoAula repositorioPlanoAula,
                                                               IRepositorioDiarioBordo repositorioDiarioBordo)
        {
            this.repositorioAula = repositorioAula ?? throw new ArgumentNullException(nameof(repositorioAula));
            this.repositorioFrequencia = repositorioFrequencia ?? throw new ArgumentNullException(nameof(repositorioFrequencia));
            this.repositorioPlanoAula = repositorioPlanoAula ?? throw new ArgumentNullException(nameof(repositorioPlanoAula));
            this.repositorioDiarioBordo = repositorioDiarioBordo ?? throw new ArgumentNullException(nameof(repositorioDiarioBordo));
        }

        public async Task<bool> Handle(ObterFrequenciaOuPlanoNaRecorrenciaQuery request, CancellationToken cancellationToken)
        {
            var turmaInfantil = await repositorioAula.ObterTurmaInfantilPorAula(request.AulaId);

            var existeRegistro = await ChecarFrequenciaPlanoAula(request.AulaId, turmaInfantil);

            if (!existeRegistro)
            {
                var aulaAtual = repositorioAula.ObterPorId(request.AulaId);
                var aulasRecorrentes = await repositorioAula.ObterAulasRecorrencia(aulaAtual.AulaPaiId ?? aulaAtual.Id, request.AulaId);
                if (aulasRecorrentes != null)
                {
                    foreach (var aula in aulasRecorrentes)
                    {
                        existeRegistro = await ChecarFrequenciaPlanoAula(aula.Id, turmaInfantil);

                        if (existeRegistro)
                            break;
                    }
                }
            }

            return existeRegistro;
        }

        public async Task<bool> ChecarFrequenciaPlanoAula(long aulaId, bool turmaInfantil)
        {
            var existeRegistro = await repositorioFrequencia.FrequenciaAulaRegistrada(aulaId);
            if (!existeRegistro)
            {
                if (turmaInfantil)
                    existeRegistro = await repositorioDiarioBordo.ExisteDiarioParaAula(aulaId);
                else
                    existeRegistro = await repositorioPlanoAula.PlanoAulaRegistradoAsync(aulaId);

            }

            return existeRegistro;
        }
    }
}
