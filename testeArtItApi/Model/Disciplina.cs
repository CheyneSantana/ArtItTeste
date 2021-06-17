using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace testeArtItApi.Model
{
    public class Disciplina
    {
        public Disciplina(string nome)
        {
            Nome = nome;
        }

        public Guid Id { get; set; }
        public string Nome { get; set; }
    }
}
