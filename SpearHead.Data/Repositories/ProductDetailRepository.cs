using SpearHead.Data.Entities;
using SpearHead.Data.Infrastructure;

namespace SpearHead.Data.Repositories
{
    public sealed class ProductDetailRepository : BaseRepository<ProductDetail>
    {
        public ProductDetailRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}
