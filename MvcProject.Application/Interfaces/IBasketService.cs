using MvcProject.Application.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MvcProject.Application.Interfaces
{
    public interface IBasketService
    {
        Task<BaseResult> AddToBasketAsync(int bookId, int usersId, CancellationToken ct);
    }
}
