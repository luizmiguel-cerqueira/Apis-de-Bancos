using System.ComponentModel.DataAnnotations;

namespace api_para_banco.Infrastructure.model
{
    public class SqlServerModel
    {
        [Required]
        
        public static string Section = "SqlServer";
        [Required]
        public  string ConnectionString { get; init; }
    }
}
