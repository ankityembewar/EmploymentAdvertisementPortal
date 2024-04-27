using System;
using System.Collections.Generic;

namespace Employement_Advertisement_Portal.Data;

public partial class SmtpSetting
{
    public int Id { get; set; }

    public string Host { get; set; } = null!;

    public int Port { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public bool EnableSsl { get; set; }
}
