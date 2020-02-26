using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iTemplate.Web.Areas.Api.Dto
{
  public class dtoToken
  {
    public string Token { get; set; }

    public DateTimeOffset? IssuedUtc { get; set; }

    public DateTimeOffset? ExpiresUtc { get; set; }
  }
}
