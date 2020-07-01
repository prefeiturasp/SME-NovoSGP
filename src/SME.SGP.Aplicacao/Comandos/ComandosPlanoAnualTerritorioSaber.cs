using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ComandosPlanoAnualTerritorioSaber : IComandosPlanoAnualTerritorioSaber
    {
        private readonly IRepositorioPlanoAnualTerritorioSaber repositorioPlanoAnualTerritorioSaber;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IUnitOfWork unitOfWork;

        public ComandosPlanoAnualTerritorioSaber(IRepositorioPlanoAnualTerritorioSaber repositorioPlanoAnualTerritorioSaber,
                                  IUnitOfWork unitOfWork,
                                  IServicoUsuario servicoUsuario)
        {
            this.repositorioPlanoAnualTerritorioSaber = repositorioPlanoAnualTerritorioSaber ?? throw new ArgumentNullException(nameof(repositorioPlanoAnualTerritorioSaber));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
        }

        public async Task<IEnumerable<EntidadeBase>> Salvar(PlanoAnualTerritorioSaberDto planoAnualTerritorioSaberDto)
        {
            Validar(planoAnualTerritorioSaberDto);

            var listaAuditoria = new List<EntidadeBase>();

            unitOfWork.IniciarTransacao();

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
                        throw new NegocioException("Você não pode fazer alterações ou inclusões nesta turma, disciplina e data.");
                }
                planoAnualTerritorioSaber = MapearParaDominio(planoAnualTerritorioSaberDto, planoAnualTerritorioSaber, bimestrePlanoAnual.Bimestre.Value, bimestrePlanoAnual.Desenvolvimento, bimestrePlanoAnual.Reflexao);
                repositorioPlanoAnualTerritorioSaber.Salvar(planoAnualTerritorioSaber);

                listaAuditoria.Add(planoAnualTerritorioSaber);
            }

            unitOfWork.PersistirTransacao();

            return listaAuditoria;
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
        private PlanoAnualTerritorioSaber MapearParaDominio(PlanoAnualTerritorioSaberDto planoAnualTerritorioSaberDto, PlanoAnualTerritorioSaber planoAnualTerritorioSaber, int bimestre, string desenvolvimento, string reflexao)
        {
            if (planoAnualTerritorioSaber == null)
            {
                planoAnualTerritorioSaber = new PlanoAnualTerritorioSaber();
            }
            planoAnualTerritorioSaber.Ano = planoAnualTerritorioSaberDto.AnoLetivo.Value;
            planoAnualTerritorioSaber.Bimestre = bimestre;
            planoAnualTerritorioSaber.Reflexao = reflexao ?? string.Empty;
            planoAnualTerritorioSaber.Desenvolvimento = desenvolvimento ?? string.Empty;
            planoAnualTerritorioSaber.EscolaId = planoAnualTerritorioSaberDto.EscolaId;
            planoAnualTerritorioSaber.TurmaId = planoAnualTerritorioSaberDto.TurmaId.Value;
            planoAnualTerritorioSaber.TerritorioExperienciaId = planoAnualTerritorioSaberDto.TerritorioExperienciaId;
            return planoAnualTerritorioSaber;
        }
    }
}
