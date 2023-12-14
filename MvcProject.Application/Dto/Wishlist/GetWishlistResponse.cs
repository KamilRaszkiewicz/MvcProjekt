using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcProject.Application.Dto.Wishlist
{
    public class GetWishlistResponse : BaseResponse
    {
        public List<WishlistItemDto> Wishlist { get; set; }
    }
}
