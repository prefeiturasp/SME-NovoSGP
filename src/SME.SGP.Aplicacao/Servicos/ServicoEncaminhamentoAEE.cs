using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
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