using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VandVCLubManagementSystem.Models;
using VandVCLubManagementSystem.Models.ViewModels.Member;
using Index = VandVCLubManagementSystem.Models.ViewModels.Member.Index;

namespace VandVCLubManagementSystem.Persistence
{
    public interface IMemberRepository
    {
        Task<Person> GetMember(int id);
        IQueryable<Person> GetMembers(Index m = null);
        Task Add(Person p);
        Task Remove(int id);
        Task EditMember(Edit p);
    }

    public class MemberRepository : IMemberRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public MemberRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Person> GetMember(int id)
        {
            return await _context.People.FindAsync(id);
        }

        public IQueryable<Person> GetMembers(Index m = null)
        {
            if (m == null)
            {
                m = new Index()
                {
                    SearchString = "",
                    PageNumber = 1,
                    OrderOptionId = 1,
                };
            }
            else
            {
                m.SearchString ??= "";
                m.PageNumber ??= 1;
                m.OrderOptionId ??= 1;
            }
            var query = _context.People.Where(p => p.FirstName.Contains(m.SearchString) || p.LastName.Contains(m.SearchString) || p.Email.Contains(m.SearchString));
            return query;
        }



        public async Task EditMember(Edit m)
        {
            if (!_context.People.Any(p => p.Id == m.Id))
            {
                throw new Exception("Not found");
            }
            var member = _mapper.Map<Person>(m);
            _context.Update(member);
            await _context.SaveChangesAsync();
        }

        public async Task Add(Person p)
        {
            await _context.People.AddAsync(p);
            await _context.SaveChangesAsync();
        }

        public async Task Remove(int id)
        {
            var p = await _context.People.FindAsync(id);
            _context.Remove(p);
            await _context.SaveChangesAsync();
        }


    }

}
