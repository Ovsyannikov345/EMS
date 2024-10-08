using ProfileService.DAL.DataContext;
using ProfileService.DAL.Models;
using ProfileService.DAL.Repositories.IRepositories;

namespace ProfileService.DAL.Repositories
{
    public class ProfileRepository(ProfileDbContext context) : GenericRepository<UserProfile>(context), IProfileRepository
    {
    }
}
