using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using testeArtItApi.Context;
using testeArtItApi.Interface;
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

        [HttpPost("[action]")]
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

        [HttpGet("[action]")]
        public async Task<IActionResult> gerarRelatorio()
        {
            try
            {
                string file = await this._escolaService.gerarRelatorio();

                return Ok(file);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
