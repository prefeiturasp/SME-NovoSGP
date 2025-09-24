using SME.SGP.Dominio;
=======
﻿using SME.SGP.Dominio;
>>>>>>> e426bed35b (Merge branch 'release' of https://github.com/prefeiturasp/SME-NovoSGP into feat/132829-consolidacao-fluencia-leitora)
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioProficienciaIdep : RepositorioBase<ProficienciaIdep>, IRepositorioProficienciaIdep
    {
        public RepositorioProficienciaIdep(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task<bool> ExcluirPorAnoEscolaSerie(int anoLetivo, string codigoEOLEscola, long serieAno)
        {
            var query = "delete from proficiencia_idep where ano_letivo = @anoLetivo and codigo_eol_escola = @CodigoEOLEscola and serie_ano = @SerieAno";

            await database.Conexao.ExecuteAsync(query, new { anoLetivo, codigoEOLEscola, serieAno });

            return true;
        }
    }
}
