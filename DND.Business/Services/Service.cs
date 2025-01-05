using DND.Storage;

namespace DND.Business.Services
{
    public class Service
    {
        protected readonly IRepositoryContext _repositoryContext;

        public Service(IRepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }
    }
}
