using Application.Activities;
using Application.Errors;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using Persistence;
using System.Threading;
using Xunit;

namespace Application.Tests.Activities
{
    public class DetailsTest
    {
        private readonly DataContext _context;
        private readonly Mock<IMapper> _mapper;
        public DetailsTest()
        {
            var builder = new DbContextOptionsBuilder<DataContext>();
            builder.UseInMemoryDatabase("DefaultConnection");
            _context = new DataContext(builder.Options);

            _mapper = new Mock<IMapper>();
        }

        [Fact]
        public async void GetActivityDetails_ActvitityNotFound_ThrowNotFoundException()
        {
            //Arrange
            var handler = new Details.Handler(_context, _mapper.Object);
            var query = new Details.Query();

            //Act and Assert
            var exception = await Assert.ThrowsAsync<RestException>(() => handler.Handle(query, CancellationToken.None));
            Assert.Equal(System.Net.HttpStatusCode.NotFound, exception.Code);
        }
    }
}
