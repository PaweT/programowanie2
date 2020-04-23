using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace rpn_api.Controllers
{
    
    [ApiController]
    public class RPNController : ControllerBase
    {
        [HttpGet]
        [Route("api/tokens")]
        [Produces("application/json")]
        public IActionResult  Get(string formula)
        {
            if(string.IsNullOrEmpty(formula))
            {
                var data = new {
                status="error",
                message="Please enter the formula!"
                };
                return BadRequest(data);
            }
            RPN onp = new RPN(formula);
            if (onp.IsCorrect() == "true")
            {
                var data = new {
                status="ok",
                result= new {
                    infix=onp.Tokens().ToArray(),
					postfix=onp.InfixToPostfix(onp.Tokens())
                    }
                };
                return Ok(data);
            }else
            {
                var data = new {
                status="error",
                message=onp.IsCorrect()
                };
                return BadRequest(data);
            }
        }
        [HttpGet]
        [Route("api/calculate")]
        [Produces("application/json")]
        public IActionResult  Get(string formula, double x=Double.NaN)
        {
            if(string.IsNullOrEmpty(formula))
            {
                var data = new {
                status="error",
                message="Please enter the formula!"
                };
                return BadRequest(data);
            }
            if(Double.IsNaN(x))
            {
                var data = new {
                status="error",
                message="Please enter X!"
                };
                return BadRequest(data);
            }
            RPN onp = new RPN(formula);
            if (onp.IsCorrect() == "true")
            {
                var data = new {
                status="ok",
                result=onp.CalX(onp.Tokens(), x)
                };
                return Ok(data);
            }else
            {
                var data = new {
                status="error",
                message=onp.IsCorrect()
                };
                return BadRequest(data);
            }
        }
        [HttpGet]
        [Route("api/calculate/xy")]
        [Produces("application/json")]
        public IActionResult  Get(string formula, double from=Double.NaN, double to=Double.NaN, double n=Double.NaN)
        {
            if(string.IsNullOrEmpty(formula))
            {
                var data = new {
                status="error",
                message="Please enter the formula!"
                };
                return BadRequest(data);
            }
            if(Double.IsNaN(from) || Double.IsNaN(to) || Double.IsNaN(n))
            {
                var data = new {
                status="error",
                message="Please enter all arguments!"
                };
                return BadRequest(data);
            }
            RPN onp = new RPN(formula);
            if (onp.IsCorrect() == "true")
            {
            List<double> res = new List<double>();
            res = onp.MinMax(onp.Tokens(), from, to, n);
            List<dynamic> results = new List<dynamic>();
				for(int i=0; i<res.Count-1; i++){
					results.Add(new{
						x=res[i],
						y=res[i+1]
					});
                    i++;
                }
                var data = new {
                status="ok",
                result=results.ToArray()
                };
                return Ok(data);
            }else
            {
                var data = new {
                status="error",
                message=onp.IsCorrect()
                };
                return BadRequest(data);
            }
        }
        
    }
}