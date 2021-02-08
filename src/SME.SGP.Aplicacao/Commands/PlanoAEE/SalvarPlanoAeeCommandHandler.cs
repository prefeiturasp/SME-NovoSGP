using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands
{
    public class SalvarPlanoAeeCommandHandler : IRequestHandler<SalvarPlanoAeeCommand, long>
    {

        private readonly IRepositorioPlanoAEE repositorioPlanoAEE;

        public SalvarPlanoAeeCommandHandler(IRepositorioPlanoAEE repositorioPlanoAEE)
        {
            this.repositorioPlanoAEE = repositorioPlanoAEE ?? throw new ArgumentNullException(nameof(repositorioPlanoAEE));
        }

        public async Task<long> Handle(SalvarPlanoAeeCommand request, CancellationToken cancellationToken)
        {
            var plano = MapearParaEntidade(request);
            return await repositorioPlanoAEE.SalvarAsync(plano);
        }

        private PlanoAEE MapearParaEntidade(SalvarPlanoAeeCommand request)
            => new PlanoAEE()
            {
                TurmaId = request.TurmaId,
                Situacao = request.Situacao,
                AlunoCodigo = request.AlunoCodigo,
                AlunoNumero = request.AlunoNumero,
                AlunoNome = request.AlunoNome
            };
    }
}
