using SpearHead.Data.Entities;
using SpearHead.Data.Infrastructure;

namespace SpearHead.Data.Repositories
{
    public sealed class ProductRepository : BaseRepository<Product>
    {
        public ProductRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}
