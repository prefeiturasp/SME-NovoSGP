using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dominio.Interfaces.Repositorios;
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
        private readonly IRepositorioComunicadoTurma _repositorioComunicadoTurma;
        private readonly IRepositorioComunicadoAluno _repositorioComunicadoAluno;
        private readonly IServicoAcompanhamentoEscolar _servicoAcompanhamentoEscolar;        

        public SolicitarExclusaoComunicadosEscolaAquiCommandHandler(
              IRepositorioComunicado repositorioComunicado
            , IRepositorioComunicadoTurma repositorioComunicadoTurma
            , IRepositorioComunicadoAluno repositorioComunicadoAluno
            , IServicoAcompanhamentoEscolar servicoAcompanhamentoEscolar            
            )
        {
            this._repositorioComunicado = repositorioComunicado ?? throw new ArgumentNullException(nameof(repositorioComunicado));
            this._repositorioComunicadoTurma = repositorioComunicadoTurma ?? throw new ArgumentNullException(nameof(repositorioComunicadoTurma));
            this._repositorioComunicadoAluno = repositorioComunicadoAluno ?? throw new ArgumentNullException(nameof(repositorioComunicadoAluno));
            this._servicoAcompanhamentoEscolar = servicoAcompanhamentoEscolar ?? throw new ArgumentNullException(nameof(servicoAcompanhamentoEscolar));            
        }

        public async Task<string> Handle(SolicitarExclusaoComunicadosEscolaAquiCommand request, CancellationToken cancellationToken)
        {
            var erros = new StringBuilder();

            var comunicados = request.Ids.Select(id =>
            {
                var comunicado = _repositorioComunicado.ObterPorId(id);
                if (comunicado == null)
                {
                    erros.Append($"<li>{id} - comunicado não encontrado</li>");
                }
                return comunicado;
            });

            if (string.IsNullOrEmpty(erros.ToString()))
            {
                await _servicoAcompanhamentoEscolar.ExcluirComunicado(request.Ids);
                foreach (var comunicado in comunicados)
                {
                    try
                    {
                        await _repositorioComunicadoAluno.RemoverTodosAlunosComunicado(comunicado.Id);
                        await _repositorioComunicadoTurma.RemoverTodasTurmasComunicado(comunicado.Id);

                        comunicado.MarcarExcluido();

                        await _repositorioComunicado.SalvarAsync(comunicado);
                    }
                    catch
                    {
                        erros.Append($"<li>{comunicado.Id} - {comunicado.Titulo}</li>");
                    }
                }
            }

            if (!string.IsNullOrEmpty(erros.ToString()))
                throw new NegocioException($"<p>Os seguintes comunicados não puderam ser excluídos:</p><br/>{erros.ToString()} por favor, tente novamente");

            return "Comunicaos excluídos com sucesso";
        }
    }
}
