using MvcProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MvcProject.Application.Interfaces
{
    internal interface IBorrowForWishListService
    {
        Task BorrowForFirstUserOnWishlistAsync(List<(Book book, int maxNrOfUsers)> books, CancellationToken ct);
    }
}
