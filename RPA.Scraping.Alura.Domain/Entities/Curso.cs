namespace RPA.Scraping.Alura.Domain.Entities
{
    public  class Curso : BaseEntity
    {
        public string TermoPesquisado { get; set; } = string.Empty;
        public string Titulo { get; set; } = string.Empty;
        public string Professor { get; set; } = string.Empty;
        public string CargaHoraria { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string UrlCurso { get; set; } = string.Empty;
        public string LastUpdate { get; set; } = string.Empty;
    }
}
