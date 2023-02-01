using Application.Guest.Requests;
using Application.Responses;

namespace Application.Guest.Ports
{
    public interface IGuestManager
    {
        Task<GuestResponse> CreateGuest(CreateGuestRequest request);
    }
}
