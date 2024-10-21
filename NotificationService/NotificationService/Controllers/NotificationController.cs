using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotificationService.ViewModels;
using NotificationService.BLL.Services.IServices;
using System.Security.Claims;
using AutoMapper;
using NotificationService.BLL.Models;

namespace NotificationService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NotificationController(INotificationService notificationService, IMapper mapper) : ControllerBase
    {
        [HttpGet]
        public async Task<IEnumerable<NotificationViewModel>> GetNotificationList(CancellationToken cancellationToken)
        {
            var notificationList = await notificationService.GetNotificationListAsync(GetAuth0IdFromContext(), cancellationToken);

            return mapper.Map<IEnumerable<NotificationModel>, IEnumerable<NotificationViewModel>>(notificationList);
        }

        [HttpPost("{id}/read")]
        public async Task<NotificationViewModel> ReadNotification(Guid id, CancellationToken cancellationToken)
        {
            var notification = await notificationService.MarkNotificationAsReadAsync(id, GetAuth0IdFromContext(), cancellationToken);

            return mapper.Map<NotificationViewModel>(notification);
        }

        private string GetAuth0IdFromContext()
        {
            return HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        }
    }
}
