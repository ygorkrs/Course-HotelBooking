using Application.Guest.DTO;
using Application.Guest.Ports;
using Application.Guest.Requests;
using Application.Responses;
using Domain.Exceptions;
using Domain.Ports;

namespace Application
{
    public class GuestManager : IGuestManager
    {
        private IGuestRepository _guestRepository;

        public GuestManager(IGuestRepository guestRepository) 
        {
            _guestRepository = guestRepository;
        }

        public async Task<GuestResponse> CreateGuest(CreateGuestRequest request)
        {
            try
            {
                var guest = GuestDTO.MapToEntity(request.Data);

                await guest.Save(_guestRepository);

                request.Data.Id = guest.Id;

                return new GuestResponse
                {
                    Data = request.Data,
                    Sucess = true,
                };
            }
            catch (InvalidDocumentIdException e)
            {
                return new GuestResponse
                {
                    Sucess = false,
                    ErrorCode = ErrorCode.INVALID_DOCUMENT_ID,
                    Message = "The DocumentID passed is not valid"
                };
            }
            catch (MissingRequiredInformationException e)
            {
                return new GuestResponse
                {
                    Sucess = false,
                    ErrorCode = ErrorCode.MISSING_REQUIRED_INFORMATION,
                    Message = "Missing required information passed"
                };
            }
            catch (InvalidEmailException e)
            {
                return new GuestResponse
                {
                    Sucess = false,
                    ErrorCode = ErrorCode.INVALID_EMAIL,
                    Message = "The given Email is not valid"
                };
            }
            catch (Exception)
            {
                return new GuestResponse
                {
                    Sucess = false,
                    ErrorCode = ErrorCode.COULD_NOT_STORE_DATA,
                    Message = "There was an error when saving to DB"
                };
            }
        }
    }
}
