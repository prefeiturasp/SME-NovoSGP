using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ComandoComunicado : IComandoComunicado
    {
        private readonly IRepositorioComunicado repositorio;
        private readonly IRepositorioComunicadoGrupo repositorioComunicadoGrupo;
        private readonly IServicoAcompanhamentoEscolar servicoAcompanhamentoEscolar;
        private readonly IUnitOfWork unitOfWork;
        private readonly IRepositorioComunicadoAluno repositorioComunicadoAluno;

        public ComandoComunicado(IRepositorioComunicado repositorio,
            IServicoAcompanhamentoEscolar servicoAcompanhamentoEscolar,
            IRepositorioComunicadoGrupo repositorioComunicadoGrupo,
            IUnitOfWork unitOfWork,
            IRepositorioComunicadoAluno repositorioComunicadoAluno)
        {
            this.repositorio = repositorio ?? throw new System.ArgumentNullException(nameof(repositorio));
            this.repositorioComunicadoGrupo = repositorioComunicadoGrupo ?? throw new System.ArgumentNullException(nameof(repositorioComunicadoGrupo));
            this.servicoAcompanhamentoEscolar = servicoAcompanhamentoEscolar ?? throw new System.ArgumentNullException(nameof(servicoAcompanhamentoEscolar));
            this.unitOfWork = unitOfWork ?? throw new System.ArgumentNullException(nameof(unitOfWork));
            this.repositorioComunicadoAluno = repositorioComunicadoAluno ?? throw new ArgumentNullException(nameof(repositorioComunicadoAluno));
        }

        public async Task<string> Alterar(long id, ComunicadoInserirDto comunicadoDto)
        {
            Comunicado comunicado = BuscarComunicado(id);

            ComunicadoInserirAeDto comunicadoServico = new ComunicadoInserirAeDto();

            MapearAlteracao(comunicadoDto, comunicado);

            try
            {
                unitOfWork.IniciarTransacao();

                await repositorio.SalvarAsync(comunicado);

                MapearParaEntidadeServico(comunicadoServico, comunicado);

                await servicoAcompanhamentoEscolar.AlterarComunicado(comunicadoServico, id);

                unitOfWork.PersistirTransacao();
            }
            catch
            {
                unitOfWork.Rollback();
                throw;
            }

            return "Comunicado alterado com sucesso";
        }

        public async Task Excluir(long[] ids)
        {
            var erros = new StringBuilder();
            await servicoAcompanhamentoEscolar.ExcluirComunicado(ids);
            foreach (var id in ids)
            {
                var comunicado = repositorio.ObterPorId(id);
                if (comunicado == null)
                    erros.Append($"<li>{id} - comunicado não encontrado</li>");
                else
                {
                    try
                    {
                        await repositorioComunicadoGrupo.ExcluirPorIdComunicado(id);
                        await repositorioComunicadoAluno.RemoverTodosAlunosComunicado(id);
                        comunicado.MarcarExcluido();
                        await repositorio.SalvarAsync(comunicado);
                    }
                    catch
                    {
                        erros.Append($"<li>{id} - {comunicado.Titulo}</li>");
                    }
                }
            }
            if (!string.IsNullOrEmpty(erros.ToString()))
                throw new NegocioException($"<p>Os seguintes comunicados não puderam ser excluídos:</p><br/>{erros.ToString()} por favor, tente novamente");
        }

        public async Task<string> Inserir(ComunicadoInserirDto comunicadoDto)
        {
            Comunicado comunicado = new Comunicado();
            ComunicadoInserirAeDto comunicadoServico = new ComunicadoInserirAeDto();
            ValidarInsercao(comunicadoDto);
            MapearParaEntidade(comunicadoDto, comunicado);

            try
            {
                unitOfWork.IniciarTransacao();

                var id = await repositorio.SalvarAsync(comunicado);

                await SalvarGrupos(id, comunicadoDto);

                comunicado.AtualizarIdAlunos();

                await SalvarAlunos(comunicado.Alunos);

                MapearParaEntidadeServico(comunicadoServico, comunicado);

                await servicoAcompanhamentoEscolar.CriarComunicado(comunicadoServico);

                unitOfWork.PersistirTransacao();
            }
            catch
            {
                unitOfWork.Rollback();
                throw;
            }

            return "Comunicado criado com sucesso";
        }

        private async Task SalvarAlunos(IEnumerable<ComunicadoAluno> alunos)
        {
            foreach (var aluno in alunos)
                await repositorioComunicadoAluno.SalvarAsync(aluno);
        }

        private void MapearAlteracao(ComunicadoInserirDto comunicadoDto, Comunicado comunicado)
        {
            comunicado.Descricao = comunicadoDto.Descricao;
            comunicado.Titulo = comunicadoDto.Titulo;
            comunicado.DataExpiracao = comunicadoDto.DataExpiracao;
        }

        private void ValidarInsercao(ComunicadoInserirDto comunicadoDto)
        {
            if (comunicadoDto.CodigoDre.Equals("todas") && !comunicadoDto.CodigoUe.Equals("todas"))
                throw new NegocioException("Não é possivel especificar uma escola quando o comunicado é para todas as DREs");

            if (comunicadoDto.CodigoUe.Equals("todas") && !comunicadoDto.Turma.Equals("todas"))
                throw new NegocioException("Não é possivel especificar uma turma quando o comunicado é para todas as UEs");

            if (comunicadoDto.Turma.Equals("todas") && (comunicadoDto.AlunosEspecificados || comunicadoDto.Alunos.Any()))
                throw new NegocioException("Não é possivel especificar alunos quando o comunicado é para todas as Turmas");
        }

        private void MapearParaEntidade(ComunicadoInserirDto comunicadoDto, Comunicado comunicado)
        {
            comunicado.DataEnvio = comunicadoDto.DataEnvio;
            comunicado.DataExpiracao = comunicadoDto.DataExpiracao;
            comunicado.AlunoEspecificado = comunicadoDto.AlunosEspecificados;
            comunicado.Descricao = comunicadoDto.Descricao;
            comunicado.Titulo = comunicadoDto.Titulo;
            comunicado.AnoLetivo = comunicadoDto.AnoLetivo;

            if (!comunicadoDto.CodigoDre.Equals("todas"))
                comunicado.CodigoDre = comunicadoDto.CodigoDre;

            if (!comunicadoDto.CodigoUe.Equals("todas"))
                comunicado.CodigoUe = comunicadoDto.CodigoUe;

            if (comunicadoDto.Turma != "todas")
                comunicado.Turma = comunicadoDto.Turma;

            if (comunicadoDto.Modalidade.HasValue)
                comunicado.Modalidade = comunicadoDto.Modalidade;

            if (comunicadoDto.GruposId.Any())
                comunicado.Grupos = comunicadoDto.GruposId.Select(s => new GrupoComunicacao { Id = s }).ToList();

            if (comunicadoDto.AlunosEspecificados)
                comunicadoDto.Alunos.ToList().ForEach(x => comunicado.AdicionarAluno(x));

            if (comunicadoDto.Semestre > 0)
                comunicado.Semestre = comunicadoDto.Semestre;

            comunicado.SetarTipoComunicado();
        }

        private Comunicado BuscarComunicado(long id)
        {
            return repositorio.ObterPorId(id) ?? throw new NegocioException("Comunicado não encontrado");
        }

        private void MapearParaEntidadeServico(ComunicadoInserirAeDto comunicadoServico, Comunicado comunicado)
        {
            comunicadoServico.Id = comunicado.Id;
            comunicadoServico.AlteradoEm = comunicado.AlteradoEm;
            comunicadoServico.AlteradoPor = comunicado.AlteradoPor;
            comunicadoServico.AlteradoRF = comunicado.AlteradoRF;
            comunicadoServico.DataEnvio = comunicado.DataEnvio;
            comunicadoServico.DataExpiracao = comunicado.DataExpiracao;
            comunicadoServico.Mensagem = comunicado.Descricao;
            comunicadoServico.Titulo = comunicado.Titulo;
            comunicadoServico.Grupo = string.Join(",", comunicado.Grupos.Select(x => x.Id.ToString()).ToArray());
            comunicadoServico.CriadoEm = comunicado.CriadoEm;
            comunicadoServico.CriadoPor = comunicado.CriadoPor;
            comunicadoServico.CriadoRF = comunicado.CriadoRF;
            comunicadoServico.Alunos = comunicado.Alunos.Select(x => x.AlunoCodigo);
            comunicadoServico.AnoLetivo = comunicado.AnoLetivo;
            comunicadoServico.CodigoDre = comunicado.CodigoDre;
            comunicadoServico.CodigoUe = comunicado.CodigoUe;
            comunicadoServico.Turma = comunicado.Turma;
            comunicadoServico.TipoComunicado = comunicado.TipoComunicado;
            comunicadoServico.Semestre = comunicado.Semestre;
            comunicadoServico.Modalidade = comunicado.Modalidade;            
        }

        private async Task SalvarGrupos(long id, ComunicadoInserirDto comunicadoDto)
        {
            foreach (var grupoId in comunicadoDto.GruposId)
            {
                await repositorioComunicadoGrupo.SalvarAsync(new ComunicadoGrupo { ComunicadoId = id, GrupoComunicadoId = grupoId });
            }
        }
    }
}