using Nest;

namespace SME.SGP.Infra.ElasticSearch.Dtos
{
    public class DocumentoElastic
    {
        [Text(Name = "Id")]
        public string Id { get; set; }

    }
}
