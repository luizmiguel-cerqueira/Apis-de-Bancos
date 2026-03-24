using System;
using System.Collections.Generic;

namespace api_para_banco.Infrastructure.model;

public partial class User
{
    public int UserId { get; set; }

    public string Pass { get; set; } = null!;
}
