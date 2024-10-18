using ProfileService.DAL.DataContext;
using ProfileService.DAL.Models;
using ProfileService.DAL.Repositories.IRepositories;

namespace ProfileService.DAL.Repositories
{
    public class ProfileInfoVisibilityRepository(ProfileDbContext context) : GenericRepository<ProfileInfoVisibility>(context), IProfileInfoVisibilityRepository;
}
