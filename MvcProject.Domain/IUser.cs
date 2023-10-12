using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcProject.Domain
{
    public interface IUser<TKey> where TKey : IEquatable<TKey>
    {
        TKey Id { get; }
        string Email { get; }
        bool IsVerified { get; }
    }
}
