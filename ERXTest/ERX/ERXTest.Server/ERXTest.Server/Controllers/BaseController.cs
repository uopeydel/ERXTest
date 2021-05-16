using Microsoft.AspNetCore.Mvc;
using ERXTest.Shared.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace ERXTest.Server.Controllers
{
    public class BaseController : ControllerBase
    {


        protected LogDesc LogDesc
        {
            get
            {
                var newLogDesc = (LogDesc)HttpContext.Items["logDesc"];
                return newLogDesc;
            }
        }
    }
}
