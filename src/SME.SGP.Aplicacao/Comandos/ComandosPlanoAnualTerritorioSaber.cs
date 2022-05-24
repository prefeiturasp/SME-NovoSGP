using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ComandosPlanoAnualTerritorioSaber : IComandosPlanoAnualTerritorioSaber
    {
        private readonly IRepositorioPlanoAnualTerritorioSaber repositorioPlanoAnualTerritorioSaber;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMediator mediator;

        public ComandosPlanoAnualTerritorioSaber(IRepositorioPlanoAnualTerritorioSaber repositorioPlanoAnualTerritorioSaber,
                                  IUnitOfWork unitOfWork,
                                  IServicoUsuario servicoUsuario, IMediator mediator)
        {
            this.repositorioPlanoAnualTerritorioSaber = repositorioPlanoAnualTerritorioSaber ?? throw new ArgumentNullException(nameof(repositorioPlanoAnualTerritorioSaber));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<EntidadeBase>> Salvar(PlanoAnualTerritorioSaberDto planoAnualTerritorioSaberDto)
        {
            Validar(planoAnualTerritorioSaberDto);

            var listaAuditoria = new List<EntidadeBase>();

            unitOfWork.IniciarTransacao();
            var listaDescricao = new List<PlanoAnualTerritorioSaberResumidoDto>();
            var usuarioAtual = servicoUsuario.ObterUsuarioLogado().Result;
            if (string.IsNullOrWhiteSpace(usuarioAtual.CodigoRf))
            {
                throw new NegocioException("Não foi possível obter o RF do usuário.");
            }
            foreach (var bimestrePlanoAnual in planoAnualTerritorioSaberDto.Bimestres)
            {
                PlanoAnualTerritorioSaber planoAnualTerritorioSaber = await ObterPlanoAnualTerritorioSaberSimplificado(planoAnualTerritorioSaberDto, bimestrePlanoAnual.Bimestre.Value);
                if (planoAnualTerritorioSaber != null)
                {
                    if (usuarioAtual.PerfilAtual == Perfis.PERFIL_PROFESSOR && !servicoUsuario.PodePersistirTurmaDisciplina(usuarioAtual.CodigoRf, planoAnualTerritorioSaberDto.TurmaId.ToString(), planoAnualTerritorioSaberDto.TerritorioExperienciaId.ToString(), DateTime.Now).Result)
                        throw new NegocioException("Você não pode fazer alterações ou inclusões nesta turma, componente curricular e data.");
                }
                
                planoAnualTerritorioSaber = await MapearParaDominio(planoAnualTerritorioSaberDto, planoAnualTerritorioSaber, bimestrePlanoAnual.Bimestre.Value, bimestrePlanoAnual.Desenvolvimento, bimestrePlanoAnual.Reflexao);
                repositorioPlanoAnualTerritorioSaber.Salvar(planoAnualTerritorioSaber);

                listaAuditoria.Add(planoAnualTerritorioSaber);
            }

            unitOfWork.PersistirTransacao();

            return listaAuditoria;
        }
        private async Task<string> MoverRemoverExcluidos(string novo, string atual)
        {
            var caminho = string.Empty;

            if (!string.IsNullOrEmpty(novo))
                caminho = await mediator.Send(new MoverArquivosTemporariosCommand(TipoArquivo.TerritorioSaber, atual, novo));

            if (!string.IsNullOrEmpty(atual))
                await mediator.Send(new RemoverArquivosExcluidosCommand(atual, novo, TipoArquivo.TerritorioSaber.Name()));

            return caminho;
        }
        private void Validar(PlanoAnualTerritorioSaberDto planoAnualTerritorioSaberDto)
        {
            var bimestresDescricaoVazia = planoAnualTerritorioSaberDto.Bimestres.Where(b =>
                   string.IsNullOrEmpty(b.Desenvolvimento) && string.IsNullOrEmpty(b.Reflexao));

            if (bimestresDescricaoVazia.Any())
                throw new NegocioException($@"É necessário preencher o desenvolvimento e/ou reflexão do 
                                            {string.Join(", ", bimestresDescricaoVazia.Select(b => $"{b.Bimestre}º"))} bimestre");
        }

        private async Task<PlanoAnualTerritorioSaber> ObterPlanoAnualTerritorioSaberSimplificado(PlanoAnualTerritorioSaberDto planoAnualTerritorioSaberDto, int bimestre)
        {
            return await repositorioPlanoAnualTerritorioSaber.ObterPlanoAnualTerritorioSaberSimplificadoPorAnoEscolaBimestreETurma(planoAnualTerritorioSaberDto.AnoLetivo.Value,
                                                                                                      planoAnualTerritorioSaberDto.EscolaId,
                                                                                                      planoAnualTerritorioSaberDto.TurmaId.Value,
                                                                                                      bimestre,
                                                                                                      planoAnualTerritorioSaberDto.TerritorioExperienciaId);
        }
        private async Task<PlanoAnualTerritorioSaber> MapearParaDominio(PlanoAnualTerritorioSaberDto planoAnualTerritorioSaberDto, PlanoAnualTerritorioSaber planoAnualTerritorioSaber, int bimestre, string desenvolvimento, string reflexao)
        {
            if (planoAnualTerritorioSaber == null)
                planoAnualTerritorioSaber = new PlanoAnualTerritorioSaber();

            planoAnualTerritorioSaber.Ano = planoAnualTerritorioSaberDto.AnoLetivo.Value;
            planoAnualTerritorioSaber.Bimestre = bimestre;
            planoAnualTerritorioSaber.EscolaId = planoAnualTerritorioSaberDto.EscolaId;
            planoAnualTerritorioSaber.TurmaId = planoAnualTerritorioSaberDto.TurmaId.Value;
            planoAnualTerritorioSaber.TerritorioExperienciaId = planoAnualTerritorioSaberDto.TerritorioExperienciaId;

            var desenvolvimentoAtual = planoAnualTerritorioSaber != null ? planoAnualTerritorioSaber.Desenvolvimento : string.Empty;
            var reflexaoAtual = planoAnualTerritorioSaber != null ? planoAnualTerritorioSaber.Reflexao : string.Empty;
            planoAnualTerritorioSaber.Reflexao = await MoverRemoverExcluidos(reflexao, reflexaoAtual);
            planoAnualTerritorioSaber.Desenvolvimento = await MoverRemoverExcluidos(desenvolvimento, desenvolvimentoAtual);

            return planoAnualTerritorioSaber;
        }
    }
}
