using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using VandVCLubManagementSystem.Models;
using VandVCLubManagementSystem.Models.ViewModels.Member;
using VandVCLubManagementSystem.Persistence;
using Index = VandVCLubManagementSystem.Models.ViewModels.Member.Index;

namespace VandVCLubManagementSystem.Controllers
{
    public class MemberController : Controller
    {

        private readonly IMemberRepository _repository;
        private readonly IMapper _mapper;

        public MemberController(IMemberRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {

            var query = _repository.GetMembers();
            var members = await query.Take(10).ToListAsync();
            var options = new Collection()
            {
                new {Id = 1, Description = "Latest Members"},
                new {Id = 2, Description = "Oldest Members"},
                new {Id = 3, Description = "Name Ascending"},
                new {Id = 4, Description = "Name Descending"},
            };
            var m = new Index()
            {
                OrderOptions = new SelectList(options, "Id", "Description"),
                TotalPages = query.Count() / 10 + 1,
                People = members,
                PageNumber = 1
            };
            return View(m);
        }


        [HttpPost]
        public async Task<IActionResult> Index(Index m)
        {

            var query = _repository.GetMembers(m);

            List<Person> members;
            switch (m.OrderOptionId)
            {
                case 2:
                    members = await query.OrderBy(p => p.Id).Skip((m.PageNumber.Value - 1) * 10).Take(10).ToListAsync();
                    break;
                case 3:
                    members = await query.OrderBy(p => p.FirstName).Skip((m.PageNumber.Value - 1) * 10).Take(10).ToListAsync();
                    break;
                case 4:
                    members = await query.OrderByDescending(p => p.FirstName).Skip((m.PageNumber.Value - 1) * 10).Take(10).ToListAsync();
                    break;
                default:
                    members = await query.OrderByDescending(p => p.Id).Skip((m.PageNumber.Value - 1) * 10).Take(10).ToListAsync();
                    break;
            }

            var options = new Collection()
            {
                new {Id = 1, Description = "Latest Members"},
                new {Id = 2, Description = "Oldest Members"},
                new {Id = 3, Description = "Name Ascending"},
                new {Id = 4, Description = "Name Descending"},
            };

            m.OrderOptions = new SelectList(options, "Id", "Description");
            m.TotalPages = query.Count() / 10 + 1;
            m.People = members;

            return View(m);
        }


        public IActionResult Create()
        {
            var m = new Create();
            return View(m);
        }


        [HttpPost]
        public async Task<IActionResult> Create(Create m)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var member = _mapper.Map<Person>(m);
                    await _repository.Add(member);
                    return RedirectToAction(nameof(Confirmation), new { id = member.Id });
                }
                catch (Exception)
                {
                    ModelState.AddModelError("Exception", "Something went wrong!!");
                }
            }
            return View(m);
        }

        public async Task<IActionResult> Confirmation(int id)
        {

            var member = await _repository.GetMember(id);
            if (member == null) return NotFound();
            var m = _mapper.Map<Create>(member);
            return View(m);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var member = await _repository.GetMember(id);
            if (member == null)
            {
                return NotFound();
            }
            var m = _mapper.Map<Edit>(member);
            return View(m);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(Edit m)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    await _repository.EditMember(m);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    ModelState.AddModelError("Exception", "Something went wrong!!");
                }
            }
            return View(m);
        }


        public async Task<IActionResult> DeleteConfirmation(int id)
        {
            var member = await _repository.GetMember(id);
            if (member == null)
            {
                return NotFound();
            }
            var m = _mapper.Map<Edit>(member);
            return View(m);
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var member = await _repository.GetMember(id);
                if (member == null)
                {
                    return NotFound();
                }
                await _repository.Remove(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                ModelState.AddModelError("Exception", "Something went wrong!!");
            }
            return RedirectToAction(nameof(DeleteConfirmation), new { id });
        }


    }
}
