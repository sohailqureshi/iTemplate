using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace iTemplate.Web.Areas.Api.Controllers
{
  public class DefaultController : ApiController
  {
    // GET: api/Default
    [HttpGet]
    public IHttpActionResult Get()
    {
      return Ok(new { Value1 = "foo", Value2 = "bar" });
    }

    // GET: api/Default/5
    [HttpGet]
    public IHttpActionResult Get(int id)
    {
      return Ok(new { Value1 = "foo" });
    }

    // POST: api/Default
    public void Post([FromBody]string value)
    {
    }

    // PUT: api/Default/5
    public void Put(int id, [FromBody]string value)
    {
    }

    // DELETE: api/Default/5
    public void Delete(int id)
    {
    }
  }
}
