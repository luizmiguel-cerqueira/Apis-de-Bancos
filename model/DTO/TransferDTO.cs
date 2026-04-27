namespace api_para_banco.model.DTO
{
    public class TransferDTO
    {
        public string FromAccount { get; set; }
        public string ToAccount { get; set; }
        public decimal quantity { get; set; }
    }
}
