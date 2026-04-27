namespace api_para_banco.model.DTO
{
    public class HistoricoTransferenciaDTO
    {
        public Guid IdTransferencia { get; private set; } = Guid.NewGuid();
        public decimal ValorTransferencia { get; set; }
        public DateTime DataTransferencia { get; set; }
        public string Idpagador { get; set; }
        public string IdBeneficiado { get; set; }
    }
}
