using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using testeArtItApi.Context;
using testeArtItApi.Interface;
using testeArtItApi.Model;
using testeArtItApi.Service;

namespace testeArtItApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EscolaController : ControllerBase
    {
        private readonly EscolaContext _escolaContext;
        private IEscolaService _escolaService;

        public EscolaController(EscolaContext escolaContext, IEscolaService escolaService)
        {
            this._escolaContext = escolaContext;
            this._escolaService = new EscolaService(this._escolaContext);
        }

        /// <summary>
        /// Cadastra os Alunos e atribui as materias e as notas
        /// </summary>
        /// <returns></returns>
        /// <response code="401">Não autorizado</response>
        [Authorize]
        [HttpPost("[action]")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> cadastrar()
        {
            try
            {
                await this._escolaService.cadastrar();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Gerar o relatorio de alunos
        /// </summary>
        /// <returns>Caminho do relatorio</returns>
        /// <response code="401">Não autorizado</response>
        [Authorize]
        [HttpGet("[action]")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> gerarRelatorio()
        {
            try
            {
                string file = await this._escolaService.gerarRelatorio();

                var memory = new MemoryStream();
                using(var fileStream = new FileStream(file, FileMode.Open))
                {
                    await fileStream.CopyToAsync(memory);
                }

                memory.Position = 0;

                return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "listaAlunos");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private void createTokenLogin(Usuario usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes("Teste ArtIt Cheyne");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            usuario.Token = tokenHandler.WriteToken(token);

        }

        /// <summary>
        /// Autenticação
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     POST /autenticar
        ///     {
        ///         "login": "candidato-evolucional",
        ///         "senha": "123456"
        ///     }
        /// </remarks>
        /// <param name="user"></param>
        /// <returns>user</returns>
        /// <response code="401">Usuario não encontrado</response>
        [AllowAnonymous]
        [HttpPost("[action]")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> autenticar([FromBody] Usuario user)
        {
            try
            {
                user = await this._escolaService.autenticar(user);

                this.createTokenLogin(user);

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
