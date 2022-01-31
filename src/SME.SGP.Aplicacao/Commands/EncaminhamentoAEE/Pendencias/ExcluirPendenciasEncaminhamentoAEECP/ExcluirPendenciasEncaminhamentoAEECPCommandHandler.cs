using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirPendenciasEncaminhamentoAEECPCommandHandler : IRequestHandler<ExcluirPendenciasEncaminhamentoAEECPCommand, bool>
    {
        private readonly IMediator mediator;

        public ExcluirPendenciasEncaminhamentoAEECPCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(ExcluirPendenciasEncaminhamentoAEECPCommand request, CancellationToken cancellationToken)
        {
            return await RemoverPendenciasCP(request.TurmaId, request.EncaminhamentoId);
        }

        public async Task<bool> RemoverPendenciasCP(long turmaId, long encaminhamentoAEEId)
        {
            var ue = await mediator.Send(new ObterUEPorTurmaIdQuery(turmaId));
            if (ue == null)
                return false;

            var pendencia = await mediator.Send(new ObterPendenciaEncaminhamentoAEEPorIdQuery(encaminhamentoAEEId));
            if (pendencia != null)
                await mediator.Send(new ExcluirPendenciaEncaminhamentoAEECommand(pendencia.PendenciaId));

            return true;
        }

        //TODO: Mover para uma QUERYHandler
        public async Task<List<string>> ObterFuncionarios(string codigoUe)
        {
            var funcionariosCP = await mediator.Send(new ObterFuncionariosPorUeECargoQuery(codigoUe, (int)Cargo.CP));
            if (funcionariosCP.Any())
                return funcionariosCP.Select(f => f.CodigoRF).ToList();

            var funcionariosAD = await mediator.Send(new ObterFuncionariosPorUeECargoQuery(codigoUe, (int)Cargo.AD));
            if (funcionariosAD.Any())
                return funcionariosAD.Select(f => f.CodigoRF).ToList();

            var funcionariosDiretor = await mediator.Send(new ObterFuncionariosPorUeECargoQuery(codigoUe, (int)Cargo.Diretor));
            if (funcionariosDiretor.Any())
                return funcionariosDiretor.Select(f => f.CodigoRF).ToList();

            return null;
        }

        //TODO: Mover para uma QUERYHandler
        private async Task<List<long>> ObterUsuariosId(List<string> funcionarios)
        {
            List<long> usuarios = new List<long>();
            foreach (var functionario in funcionarios)
            {
                var usuario = await mediator.Send(new ObterUsuarioIdPorRfOuCriaQuery(functionario));
                usuarios.Add(usuario);
            }
            return usuarios;
        }
    }
}
