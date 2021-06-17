using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using testeArtItApi.Model;

namespace testeArtItApi.Interface
{
    public interface IEscolaService
    {
        Task<string> cadastrar();
        Task<string> gerarRelatorio();
        Task<Usuario> autenticar(Usuario usuario);
    }
}
