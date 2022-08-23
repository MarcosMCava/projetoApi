using AlunosApi.Context;
using AlunosApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlunosApi.Services
{
    public class AlunoService : IAlunoService
    {
        private readonly AppDbContext _context;

        public AlunoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Aluno>> GetAlunos()
        {
            return await _context.Alunos.AsNoTracking().ToListAsync();
        }

        public async Task<Aluno> GetAlunoById(int Id)
        {
            return await _context.Alunos.AsNoTracking().FirstOrDefaultAsync(x => x.AlunoId == Id);
        }
        public async Task<IEnumerable<Aluno>> GetAlunoByNome(string nome)
        {
            IEnumerable<Aluno> alunos;

            if (!string.IsNullOrWhiteSpace(nome))
                alunos = await _context.Alunos.AsNoTracking().Where(x => x.Nome.Contains(nome)).ToListAsync();
            else
                alunos = null;

            return alunos;
        }
        public async Task CreateAluno(Aluno aluno)
        {
            _context.Alunos.Add(aluno);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAluno(Aluno aluno)
        {
            _context.Entry(aluno).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAluno(Aluno aluno)
        {
            _context.Alunos.Remove(aluno);
            await _context.SaveChangesAsync();
        }
    }
}
