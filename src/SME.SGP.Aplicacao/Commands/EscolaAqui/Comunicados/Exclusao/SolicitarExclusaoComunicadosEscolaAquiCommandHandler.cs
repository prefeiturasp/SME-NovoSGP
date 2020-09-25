using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Dto;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SolicitarExclusaoComunicadosEscolaAquiCommandHandler : IRequestHandler<SolicitarExclusaoComunicadosEscolaAquiCommand, string>
    {
        private readonly IRepositorioComunicado _repositorioComunicado;
        private readonly IRepositorioComunicadoGrupo _repositorioComunicadoGrupo;
        private readonly IRepositorioComunicadoTurma _repositorioComunicadoTurma;
        private readonly IRepositorioComunicadoAluno _repositorioComunicadoAluno;
        private readonly IServicoAcompanhamentoEscolar _servicoAcompanhamentoEscolar;
        private readonly IConsultasAbrangencia _consultasAbrangencia;

        public SolicitarExclusaoComunicadosEscolaAquiCommandHandler(
              IRepositorioComunicado repositorioComunicado
            , IRepositorioComunicadoGrupo repositorioComunicadoGrupo
            , IRepositorioComunicadoTurma repositorioComunicadoTurma
            , IRepositorioComunicadoAluno repositorioComunicadoAluno
            , IServicoAcompanhamentoEscolar servicoAcompanhamentoEscolar
            )
        {
            this._repositorioComunicado = repositorioComunicado ?? throw new ArgumentNullException(nameof(repositorioComunicado));
            this._repositorioComunicadoGrupo = repositorioComunicadoGrupo ?? throw new ArgumentNullException(nameof(repositorioComunicadoGrupo));
            this._repositorioComunicadoTurma = repositorioComunicadoTurma ?? throw new ArgumentNullException(nameof(repositorioComunicadoTurma));
            this._repositorioComunicadoAluno = repositorioComunicadoAluno ?? throw new ArgumentNullException(nameof(repositorioComunicadoAluno));
            this._servicoAcompanhamentoEscolar = servicoAcompanhamentoEscolar ?? throw new ArgumentNullException(nameof(servicoAcompanhamentoEscolar));
        }

        public async Task<string> Handle(SolicitarExclusaoComunicadosEscolaAquiCommand request, CancellationToken cancellationToken)
        {
            var erros = new StringBuilder();
            
            await _servicoAcompanhamentoEscolar.ExcluirComunicado(request.Ids);

            foreach (var id in request.Ids)
            {
                var comunicado = _repositorioComunicado.ObterPorId(id);
                if (comunicado == null)
                    erros.Append($"<li>{id} - comunicado não encontrado</li>");
                else
                {
                    try
                    {
                        await _repositorioComunicadoGrupo.ExcluirPorIdComunicado(id);
                        await _repositorioComunicadoAluno.RemoverTodosAlunosComunicado(id);
                        await _repositorioComunicadoTurma.RemoverTodasTurmasComunicado(id);

                        comunicado.MarcarExcluido();

                        await _repositorioComunicado.SalvarAsync(comunicado);
                    }
                    catch
                    {
                        erros.Append($"<li>{id} - {comunicado.Titulo}</li>");
                    }
                }
            }
            if (!string.IsNullOrEmpty(erros.ToString()))
                throw new NegocioException($"<p>Os seguintes comunicados não puderam ser excluídos:</p><br/>{erros.ToString()} por favor, tente novamente");

            return "Comunicaos excluídos com sucesso";
        }
    }
}
