using MediatR;
using Sentry;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Servicos
{
    public class ServicoEncaminhamentoAEE : IServicoEncaminhamentoAEE
    {
        private readonly IMediator mediator;
        private readonly IUnitOfWork unitOfWork;

        public ServicoEncaminhamentoAEE(IMediator mediator, IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task GerarPendenciaEncaminhamentoAEECP(SituacaoAEE situacao, long encaminhamentoId)
        {
            if (situacao == SituacaoAEE.Encaminhado)
            {
                await mediator.Send(new GerarPendenciaCPEncaminhamentoAEECommand(encaminhamentoId));
            }
        }

        public async Task<bool> RemoverPendenciasCP(long turmaId, long encaminhamentoAEEId)
        {
            var ue = await mediator.Send(new ObterUEPorTurmaIdQuery(turmaId));

            var funcionarios = await ObterFuncionarios(ue.CodigoUe);

            if (funcionarios == null)
                return false;

            var usuarios = await ObterUsuariosId(funcionarios);

            foreach (var usuario in usuarios)
            {
                var pendencia = await mediator.Send(new ObterPendenciaEncaminhamentoAEEPorIdEUsuarioIdQuery(encaminhamentoAEEId, usuario));
                if (pendencia != null)
                    await mediator.Send(new ExcluirPendenciaEncaminhamentoAEECommand(pendencia.PendenciaId));
            }
            return true;
        }

        public async Task<List<long>> ListarUsuariosIdPorCodigoUe(string codigoUe)
        {
            var funcionarios = await ObterFuncionarios(codigoUe);

            if (funcionarios == null)
                return null;

            var usuarios = await ObterUsuariosId(funcionarios);
            return usuarios;
        }

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

        public async Task<bool> RemovePendenciaCEFAI(long turmaId, long encaminhamentoAEEId)
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

        public async Task<long> ObtemUsuarioCEFAIDaDre(string codigoDre)
        {
            var funcionarios = await mediator.Send(new ObterFuncionariosDreOuUePorPerfisQuery(codigoDre, new List<Guid>() { Perfis.PERFIL_CEFAI }));

            if (!funcionarios.Any())
                return 0;            

            var funcionario = funcionarios.FirstOrDefault();
            var usuarioId = await mediator.Send(new ObterUsuarioIdPorRfOuCriaQuery(funcionario));

            return usuarioId;
        }


        public async Task<IEnumerable<UsuarioEolRetornoDto>> ObterPAEETurma(Turma turma)
        {
            var funcionariosUe = await mediator.Send(new PesquisaFuncionariosPorDreUeQuery("", "", turma.Ue.Dre.CodigoDre, turma.Ue.CodigoUe));

            var atividadeFuncaoPAEE = 6;
            return funcionariosUe.Where(c => c.CodigoFuncaoAtividade == atividadeFuncaoPAEE);
        }
    }
}