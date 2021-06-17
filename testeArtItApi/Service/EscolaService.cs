using Bogus;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using testeArtItApi.Context;
using testeArtItApi.Interface;
using testeArtItApi.Model;

namespace testeArtItApi.Service
{
    public class EscolaService : IEscolaService
    {
        private EscolaContext _escolaContext;

        public EscolaService(EscolaContext escolaContext)
        {
            _escolaContext = escolaContext;
        }

        // loop 40
        private void cadastrarAlunos()
        {
            var testUsers = new Faker<RandomUser>().RuleFor(r => r.FullName, (f, r) => f.Name.FullName());
            List<Aluno> alunos = this._escolaContext.Alunos.ToList();
            List<Disciplina> disciplinas = this._escolaContext.Disciplinas.ToList();

            for (int i = 0; i < 1000; i++)
            {
                var user = testUsers.Generate();
                Aluno aluno = alunos.Where(w => w.Nome == user.FullName).FirstOrDefault();
                if (aluno == null)
                {
                    aluno = new Aluno()
                    {
                        Nome = user.FullName
                    };

                    this._escolaContext.Add(aluno);

                    disciplinas.ForEach(disciplina =>
                    {
                        var alunoDisciplina = new AlunoDisciplina()
                        {
                            IdAluno = aluno.Id,
                            IdDisciplina = disciplina.Id,
                            Nota = decimal.Parse(string.Format("{0:0.##}", new Random().NextDouble() * 10))
                        };

                        this._escolaContext.AlunoDisciplinas.Add(alunoDisciplina);
                    });
                }
            }
            this._escolaContext.SaveChanges();

        }

        private void cadastrarDisciplinas()
        {
            Disciplina matematica = new Disciplina("Matemática");
            Disciplina portugues = new Disciplina("Português");
            Disciplina historia = new Disciplina("História");
            Disciplina geografia = new Disciplina("Geografia");
            Disciplina ingles = new Disciplina("Inglês");
            Disciplina biologia = new Disciplina("Biologia");
            Disciplina filosofia = new Disciplina("Filosofia");
            Disciplina fisica = new Disciplina("Física");
            Disciplina quimica = new Disciplina("Química");

            if (!this._escolaContext.Disciplinas.Contains(matematica))
                this._escolaContext.Disciplinas.Add(matematica);

            if (!this._escolaContext.Disciplinas.Contains(portugues))
                this._escolaContext.Disciplinas.Add(portugues);

            if (!this._escolaContext.Disciplinas.Contains(historia))
                this._escolaContext.Disciplinas.Add(historia);

            if (!this._escolaContext.Disciplinas.Contains(geografia))
                this._escolaContext.Disciplinas.Add(geografia);

            if (!this._escolaContext.Disciplinas.Contains(ingles))
                this._escolaContext.Disciplinas.Add(ingles);

            if (!this._escolaContext.Disciplinas.Contains(biologia))
                this._escolaContext.Disciplinas.Add(biologia);

            if (!this._escolaContext.Disciplinas.Contains(filosofia))
                this._escolaContext.Disciplinas.Add(filosofia);

            if (!this._escolaContext.Disciplinas.Contains(fisica))
                this._escolaContext.Disciplinas.Add(fisica);

            if (!this._escolaContext.Disciplinas.Contains(quimica))
                this._escolaContext.Disciplinas.Add(quimica);

            this._escolaContext.SaveChanges();
        }

        public async Task<string> cadastrar()
        {
            this.cadastrarDisciplinas();
            this.cadastrarAlunos();

            return "OK";
        }

        public async Task<string> gerarRelatorio()
        {
            using (var workbook = new XLWorkbook())
            {
                List<Aluno> alunos = this._escolaContext.Alunos.ToList();
                List<Disciplina> disciplinas = this._escolaContext.Disciplinas.ToList();
                List<AlunoDisciplina> alunosdisciplinas = this._escolaContext.AlunoDisciplinas.ToList();

                var worksheet = workbook.Worksheets.Add("Alunos");
                int line = 1;
                decimal totalNotas = 0;

                alunos.ForEach(aluno =>
                {
                    disciplinas.ForEach(disciplina =>
                    {
                        AlunoDisciplina alunoDisciplina = alunosdisciplinas.Where(w => w.IdAluno == aluno.Id && w.IdDisciplina == disciplina.Id).FirstOrDefault();
                        if (alunoDisciplina != null)
                        {
                            worksheet.Cell(line, 1).Value = aluno.Nome;
                            worksheet.Cell(line, 2).Value = alunoDisciplina.Nota;
                            totalNotas += alunoDisciplina.Nota;
                        };
                        line++;
                    });
                    worksheet.Cell(line, 2).Value = "Media";
                    worksheet.Cell(line, 3).Value = totalNotas / 9;
                    line++;
                });

                workbook.SaveAs(Path.Combine(Path.GetTempPath(), "listaAlunos.xlsx"));
            }
            return Path.Combine(Path.GetTempPath(), "listaAlunos.xlsx");
        }

        public async Task<Usuario> autenticar(Usuario usuario)
        {
            Usuario user = this._escolaContext.Usuarios.Where(w => w.Login == usuario.Login && w.Senha == usuario.Senha).FirstOrDefault();
            if (user == null)
                throw new Exception("Usuario não encontrado");

            return user;

        }
    }
}
