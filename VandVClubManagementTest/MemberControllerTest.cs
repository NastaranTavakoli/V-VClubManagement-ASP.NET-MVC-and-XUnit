using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using VandVCLubManagementSystem.Controllers;
using VandVCLubManagementSystem.Models;
using VandVCLubManagementSystem.Models.ViewModels.Member;
using VandVCLubManagementSystem.Persistence;
using Xunit;

namespace VandVClubManagementTest
{
    public class MemberControllerTest
    {
        private readonly MemberController _controller;
        private readonly Mock<IMemberRepository> _repo;

        //arrange
        public MemberControllerTest()
        {
            var data = new List<Person>
            {new() {Id = 1, FirstName = "Nas"},
                new (){Id = 2,FirstName = "Ben"},
                new (){Id = 3, FirstName = "Fred"}};


            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(m => m.Map<Edit>(It.IsAny<Person>())).Returns((Person src) => new Edit() { DateOfBirth = src.DateOfBirth, FirstName = src.FirstName, LastName = src.LastName, Email = src.Email, PhoneNumber = src.PhoneNumber });

            _repo = new Mock<IMemberRepository>();
            _repo.Setup(r => r.GetMember(1)).Returns(Task.FromResult(data[0]));
            _controller = new MemberController(_repo.Object, mockMapper.Object);
        }


        //Test Number : 1
        [Fact]
        public async Task DeleteConfirmation_WhenCalledForExistingId_GetsTheMemberFromDbMapsItToEditThenPassItToView()
        {

            //act
            var result = await _controller.DeleteConfirmation(1);

            //assert
            _repo.Verify(r => r.GetMember(1));
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<Edit>(viewResult.Model);

        }

        //Test Number : 2
        [Fact]
        public async Task DeleteConfirmation_WhenCalledForNonExistingId_ReturnNotFoundResult()
        {

            //act
            var result = await _controller.DeleteConfirmation(-1);

            //assert
            Assert.IsType<NotFoundResult>(result);
        }


        //Test Number : 3
        [Fact]
        public async Task Delete_WhenCalledForExistingId_DeleteTheMemberFromDbAndRedirectTheUser()
        {

            //act
            var result = await _controller.Delete(1);

            //assert
            _repo.Verify(r => r.Remove(1));
            Assert.IsType<RedirectToActionResult>(result);
        }

        //Test Number : 4
        [Fact]
        public async Task Delete_WhenCalledForNonExistingId_ReturnNotFoundResult()
        {

            //act
            var result = await _controller.Delete(-1);

            //assert
            Assert.IsType<NotFoundResult>(result);
        }





    }

}