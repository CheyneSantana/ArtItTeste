using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace testeArtItApi.Interface
{
    public interface IEscolaService
    {
        Task<string> cadastrar();
        Task<string> gerarRelatorio();
    }
}
