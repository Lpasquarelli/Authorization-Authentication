
using api_login.Models;
using api_login.Repositories;
using api_login.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api_login.Controllers 
{
    [Route("v1/account")]
    public class HomeController : ControllerBase
    {
        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<ActionResult<dynamic>> Autenticate([FromBody] User model)
        {
            var user = UserRepository.Get(model.Username, model.Password);

            if(user == null)
            {
                return NotFound("Usuario ou Senha nao encontrados!");
            }

            var token = TokenService.GenerateToken(user);
            user.Password = "";

            return new
            {
                user = user,
                token = token
            };
        }

        [HttpGet]
        [Route("anonymous")]
        [AllowAnonymous]
        public string Anonymous()
        {
            return "Anonimo";
        }

        [HttpGet]
        [Route("authenticated")]
        [Authorize]
        public string Authenticated()
        {
            return String.Format("Authenticado = {0}", User.Identity.Name);
        }

        [HttpGet]
        [Route("employee")]
        [Authorize(Roles = "employee, manager")]
        public string Employee()
        {
            return "Funcionário";
        }

        [HttpGet]
        [Route("manager")]
        [Authorize(Roles = "manager")]
        public string Manager()
        {
            return "Gerente";
        }



    }
}
