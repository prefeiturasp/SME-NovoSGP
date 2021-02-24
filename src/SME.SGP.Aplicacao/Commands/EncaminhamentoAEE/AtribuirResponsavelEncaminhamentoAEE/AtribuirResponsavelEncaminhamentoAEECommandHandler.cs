using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AtribuirResponsavelEncaminhamentoAEECommandHandler : IRequestHandler<AtribuirResponsavelEncaminhamentoAEECommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioEncaminhamentoAEE repositorioEncaminhamentoAEE;

        public AtribuirResponsavelEncaminhamentoAEECommandHandler(IMediator mediator, IRepositorioEncaminhamentoAEE repositorioEncaminhamentoAEE)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioEncaminhamentoAEE = repositorioEncaminhamentoAEE ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoAEE));
        }

        public async Task<bool> Handle(AtribuirResponsavelEncaminhamentoAEECommand request, CancellationToken cancellationToken)
        {
            var encaminhamentoAEE = await mediator.Send(new ObterEncaminhamentoAEEComTurmaPorIdQuery(request.EncaminhamentoId));

            if (encaminhamentoAEE == null)
                throw new NegocioException("O encaminhamento informado não foi encontrado");

            if (encaminhamentoAEE.Situacao == Dominio.Enumerados.SituacaoAEE.Finalizado
             || encaminhamentoAEE.Situacao == Dominio.Enumerados.SituacaoAEE.Encerrado)
                throw new NegocioException("A situação do encaminhamento não permite a remoção do responsável");

            encaminhamentoAEE.Situacao = Dominio.Enumerados.SituacaoAEE.Analise;
            encaminhamentoAEE.ResponsavelId = await mediator.Send(new ObterUsuarioIdPorRfOuCriaQuery(request.RfResponsavel));

            var idEntidadeEncaminhamento = await repositorioEncaminhamentoAEE.SalvarAsync(encaminhamentoAEE);

            await RemovePendencias(encaminhamentoAEE.TurmaId, encaminhamentoAEE.Id);

            await mediator.Send(new GerarPendenciaPAEEEncaminhamentoAEECommand(encaminhamentoAEE));

            return idEntidadeEncaminhamento != 0;
        }

        private async Task RemovePendencias(long turmaId, long encaminhamentoAEEId)
        {
            var ehCEFAI = await RemovePendenciaCEFAI(turmaId, encaminhamentoAEEId);
            if (!ehCEFAI)
            {
                await RemoverPendenciasCP(turmaId, encaminhamentoAEEId);
            }
        }

        private async Task<bool> RemoverPendenciasCP(long turmaId, long encaminhamentoAEEId)
        {
            var ue = await mediator.Send(new ObterUEPorTurmaIdQuery(turmaId));

            var funcionarios = await ObterFuncionarios(ue.CodigoUe);

            if (funcionarios == null)
                return false;

            var usuarios = await ObterUsuariosId(funcionarios);

            foreach (var usuario in usuarios)
            {
                var pendencia = await mediator.Send(new ObterPendenciaEncaminhamentoAEEPorIdEUsuarioIdQuery(encaminhamentoAEEId, usuario));
                await mediator.Send(new ExcluirPendenciaEncaminhamentoAEECommand(pendencia.PendenciaId));
            }
            return true;
        }

        private async Task<List<string>> ObterFuncionarios(string codigoUe)
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

        private async Task<bool> RemovePendenciaCEFAI(long turmaId, long encaminhamentoAEEId)
        {
            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());
            var ehCEFAI = await EhCoordenadorCEFAI(usuarioLogado, turmaId);
            if (ehCEFAI)
            {
                var pendencia = await mediator.Send(new ObterPendenciaEncaminhamentoAEEPorIdEUsuarioIdQuery(encaminhamentoAEEId, usuarioLogado.Id));
                await mediator.Send(new ExcluirPendenciaEncaminhamentoAEECommand(pendencia.PendenciaId));
                return true;
            }
            return false;
        }

        private async Task<bool> EhCoordenadorCEFAI(Usuario usuarioLogado, long turmaId)
        {
            if (!usuarioLogado.EhCoordenadorCEFAI())
                return false;

            var codigoDRE = await mediator.Send(new ObterCodigoDREPorTurmaIdQuery(turmaId));
            if (string.IsNullOrEmpty(codigoDRE))
                return false;

            return await UsuarioTemFuncaoCEFAINaDRE(usuarioLogado, codigoDRE);
        }

        private async Task<bool> UsuarioTemFuncaoCEFAINaDRE(Usuario usuarioLogado, string codigoDre)
        {
            var funcionarios = await mediator.Send(new ObterFuncionariosDreOuUePorPerfisQuery(codigoDre, new List<Guid>() { Perfis.PERFIL_CEFAI }));
            return funcionarios.Any(c => c == usuarioLogado.CodigoRf);
        }
    }
}
