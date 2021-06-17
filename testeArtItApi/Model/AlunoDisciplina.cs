using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace testeArtItApi.Model
{
    public class AlunoDisciplina
    {
        public Guid Id { get; set; }
        public Guid IdAluno { get; set; }
        public Guid IdDisciplina { get; set; }
        public decimal Nota { get; set; }
    }
}
