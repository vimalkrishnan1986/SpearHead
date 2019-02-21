using SpearHead.Data.Entities;
using SpearHead.Data.Infrastructure;

namespace SpearHead.Data.Repositories
{
    public sealed class PacketRepository : BaseRepository<Packet>
    {
        public PacketRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}
