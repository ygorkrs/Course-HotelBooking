using Application.Guest.DTO;
using Application.Guest.Ports;
using Application.Guest.Requests;
using Application.Responses;
using Domain.Guest.Exceptions;
using Domain.Guest.Ports;

namespace Application.Guest
{
    public class GuestManager : IGuestManager
    {
        private readonly IGuestRepository _guestRepository;

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
                    ErrorCode = ErrorCode.COULD_NOT_STORE_GUEST,
                    Message = "There was an error when saving to DB"
                };
            }
        }

        public async Task<GuestResponse> GetGuest(int idGuest)
        {
            var guest = await _guestRepository.Get(idGuest);

            if (guest == null)
            {
                return new GuestResponse
                {
                    Sucess = false,
                    ErrorCode = ErrorCode.NOT_FOUND_GUEST,
                    Message = "Guest not found for the given Id",
                };
            }

            return new GuestResponse
            {
                Data = GuestDTO.MapToDTO(guest),
                Sucess = true,
            };
        }
    }
}
