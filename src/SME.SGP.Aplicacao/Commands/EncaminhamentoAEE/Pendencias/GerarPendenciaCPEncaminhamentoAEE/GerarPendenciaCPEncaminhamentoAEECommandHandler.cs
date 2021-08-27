using MediatR;
using Microsoft.Extensions.Configuration;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class GerarPendenciaCPEncaminhamentoAEECommandHandler : IRequestHandler<GerarPendenciaCPEncaminhamentoAEECommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IUnitOfWork unitOfWork;
        private readonly IConfiguration configuration;
        private readonly IRepositorioPendencia repositorioPendencia;
        private readonly IRepositorioPendenciaUsuario repositorioPendenciaUsuario;
        private readonly IRepositorioPendenciaEncaminhamentoAEE repositorioPendenciaEncaminhamentoAEE;

        public GerarPendenciaCPEncaminhamentoAEECommandHandler(IMediator mediator, IUnitOfWork unitOfWork, IConfiguration configuration,
            IRepositorioPendencia repositorioPendencia, IRepositorioPendenciaUsuario repositorioPendenciaUsuario,
            IRepositorioPendenciaEncaminhamentoAEE repositorioPendenciaEncaminhamentoAEE)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.repositorioPendencia = repositorioPendencia ?? throw new ArgumentNullException(nameof(repositorioPendencia));
            this.repositorioPendenciaUsuario = repositorioPendenciaUsuario ?? throw new ArgumentNullException(nameof(repositorioPendenciaUsuario));
            this.repositorioPendenciaEncaminhamentoAEE = repositorioPendenciaEncaminhamentoAEE ?? throw new ArgumentNullException(nameof(repositorioPendenciaEncaminhamentoAEE));
        }

        public async Task<bool> Handle(GerarPendenciaCPEncaminhamentoAEECommand request, CancellationToken cancellationToken)
        {
            var encaminhamentoAEE = await mediator.Send(new ObterEncaminhamentoAEEPorIdQuery(request.EncaminhamentoAEEId));

            if (encaminhamentoAEE == null)
                throw new NegocioException("Não foi possível localizar o EncaminhamentoAEE");

            

            if (encaminhamentoAEE.Situacao == SituacaoAEE.Encaminhado)
            {
                var turma = await mediator.Send(new ObterTurmaComUeEDrePorIdQuery(encaminhamentoAEE.TurmaId));

                var funcionarios = await ObterFuncionarios(turma.Ue.CodigoUe);

                if (funcionarios == null)
                    return false;

                var usuarios = await ObterUsuariosId(funcionarios);

                var ueDre = $"{turma.Ue.TipoEscola.ShortName()} {turma.Ue.Nome} ({turma.Ue.Dre.Abreviacao})";
                var hostAplicacao = configuration["UrlFrontEnd"];
                var estudanteOuCrianca = turma.ModalidadeCodigo == Modalidade.EducacaoInfantil ? "da criança" : "do estudante";

                var titulo = $"Encaminhamento AEE para análise - {encaminhamentoAEE.AlunoNome} ({encaminhamentoAEE.AlunoCodigo}) - {ueDre}";
                var descricao = $"O encaminhamento {estudanteOuCrianca} {encaminhamentoAEE.AlunoNome} ({encaminhamentoAEE.AlunoCodigo}) da turma {turma.NomeComModalidade()} da {ueDre} está disponível para análise da coordenação. <br/><a href='{hostAplicacao}aee/encaminhamento/editar/{encaminhamentoAEE.Id}'>Clique aqui para acessar o encaminhamento.</a> " +
                    $"<br/><br/>Esta pendência será resolvida automaticamente quando o parecer da coordenação for registrado no sistema.";

                using (var transacao = unitOfWork.IniciarTransacao())
                {
                    try
                    {
                        foreach (var usuario in usuarios)
                        {
                            var pendenciaExiste = await mediator.Send(new ObterPendenciaEncaminhamentoAEEPorIdEUsuarioIdQuery(encaminhamentoAEE.Id, usuario));

                            if (pendenciaExiste == null)
                            {
                                var pendencia = new Pendencia(TipoPendencia.AEE, titulo, descricao);
                                pendencia.Id = await repositorioPendencia.SalvarAsync(pendencia);

                                var pendenciaUsuario = new PendenciaUsuario { PendenciaId = pendencia.Id, UsuarioId = usuario };
                                await repositorioPendenciaUsuario.SalvarAsync(pendenciaUsuario);

                                var pendenciaEncaminhamento = new PendenciaEncaminhamentoAEE { PendenciaId = pendencia.Id, EncaminhamentoAEEId = encaminhamentoAEE.Id };
                                await repositorioPendenciaEncaminhamentoAEE.SalvarAsync(pendenciaEncaminhamento);
                            }
                        }

                        unitOfWork.PersistirTransacao();
                        return true;
                    }
                    catch (Exception e)
                    {
                        unitOfWork.Rollback();
                        throw;
                    }
                }
            }
            return false;
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
    }
}
