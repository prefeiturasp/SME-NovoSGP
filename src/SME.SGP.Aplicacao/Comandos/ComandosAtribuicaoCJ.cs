using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ComandosAtribuicaoCJ : IComandosAtribuicaoCJ
    {
        private readonly IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ;

        public ComandosAtribuicaoCJ(IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ)
        {
            this.repositorioAtribuicaoCJ = repositorioAtribuicaoCJ ?? throw new ArgumentNullException(nameof(repositorioAtribuicaoCJ));
        }

        public async Task Salvar(AtribuicaoCJPersistenciaDto atribuicaoCJPersistenciaDto)
        {
            var atribuicaoCJs = await repositorioAtribuicaoCJ.ObterPorFiltros(atribuicaoCJPersistenciaDto.Modalidade, atribuicaoCJPersistenciaDto.TurmaId,
                atribuicaoCJPersistenciaDto.UeId, string.Empty, string.Empty, string.Empty);

            foreach (var atribuicaoDto in atribuicaoCJPersistenciaDto.Disciplinas)
            {
                var atribuicao = atribuicaoCJs.FirstOrDefault(a => a.DisciplinaId == atribuicaoDto.DisciplinaId);
                if (atribuicao == null)
                {
                    var novaAtribuicao = TransformaDtoEmEntidade(atribuicaoCJPersistenciaDto, atribuicaoDto);
                    await repositorioAtribuicaoCJ.SalvarAsync(novaAtribuicao);
                }
                else
                {
                    if (atribuicao.Substituir != atribuicaoDto.Substituir)
                    {
                        atribuicao.Substituir = atribuicaoDto.Substituir;
                        await repositorioAtribuicaoCJ.SalvarAsync(atribuicao);
                    }
                }
            }
        }

        private AtribuicaoCJ TransformaDtoEmEntidade(AtribuicaoCJPersistenciaDto dto, AtribuicaoCJPersistenciaItemDto itemDto)
        {
            return new AtribuicaoCJ()
            {
                DreId = dto.DreId,
                Modalidade = dto.Modalidade,
                ProfessorRf = dto.UsuarioRf,
                Substituir = itemDto.Substituir,
                TurmaId = dto.TurmaId,
                UeId = dto.UeId,
                DisciplinaId = itemDto.DisciplinaId
            };
        }
    }
}