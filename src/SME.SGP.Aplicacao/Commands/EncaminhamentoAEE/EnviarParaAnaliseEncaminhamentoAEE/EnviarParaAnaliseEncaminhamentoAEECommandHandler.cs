using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class EnviarParaAnaliseEncaminhamentoAEECommandHandler : IRequestHandler<EnviarParaAnaliseEncaminhamentoAEECommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioEncaminhamentoAEE repositorioEncaminhamentoAEE;

        public EnviarParaAnaliseEncaminhamentoAEECommandHandler(IMediator mediator, IRepositorioEncaminhamentoAEE repositorioEncaminhamentoAEE)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioEncaminhamentoAEE = repositorioEncaminhamentoAEE ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoAEE));
        }

        public async Task<bool> Handle(EnviarParaAnaliseEncaminhamentoAEECommand request, CancellationToken cancellationToken)
        {
            var encaminhamentoAEE = await mediator.Send(new ObterEncaminhamentoAEEComTurmaPorIdQuery(request.EncaminhamentoId));

            if (encaminhamentoAEE == null)
                throw new NegocioException("O encaminhamento informado não foi encontrado");

            var turma = await mediator.Send(new ObterTurmaComUeEDrePorIdQuery(encaminhamentoAEE.TurmaId));

            if (turma == null)
                throw new NegocioException("turma não encontrada");


            encaminhamentoAEE.Situacao = Dominio.Enumerados.SituacaoAEE.AtribuicaoResponsavel;

            IEnumerable<Guid> perfis = new List<Guid>() { Perfis.PERFIL_PAEE };

            var funciorarioPAEE = await ObterPAEETurma(turma);

            if (funciorarioPAEE != null && funciorarioPAEE.Count() == 1)
            {
                encaminhamentoAEE.Situacao = Dominio.Enumerados.SituacaoAEE.Analise;
                encaminhamentoAEE.ResponsavelId = await mediator.Send(new ObterUsuarioIdPorRfOuCriaQuery(funciorarioPAEE.FirstOrDefault().CodigoRf));
                await mediator.Send(new GerarPendenciaPAEEEncaminhamentoAEECommand(encaminhamentoAEE));
            }

            if (!funciorarioPAEE.Any())
                await mediator.Send(new GerarPendenciaCEFAIEncaminhamentoAEECommand(encaminhamentoAEE));

            var idEntidadeEncaminhamento = await repositorioEncaminhamentoAEE.SalvarAsync(encaminhamentoAEE);

            await RemoverPendenciasCP(encaminhamentoAEE.TurmaId, encaminhamentoAEE.Id);

            return idEntidadeEncaminhamento != 0;
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

        private async Task<IEnumerable<UsuarioEolRetornoDto>> ObterPAEETurma(Turma turma)
        {
            var funcionariosUe = await mediator.Send(new PesquisaFuncionariosPorDreUeQuery("", "", turma.Ue.Dre.CodigoDre, turma.Ue.CodigoUe));

            var atividadeFuncaoPAEE = 6;
            return funcionariosUe.Where(c => c.CodigoFuncaoAtividade == atividadeFuncaoPAEE);
        }
    }
}
