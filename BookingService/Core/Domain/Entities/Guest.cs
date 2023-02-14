using Domain.Exceptions;
using Domain.Guest.Exceptions;
using Domain.UtilsTools;
using Domain.Guest.Ports;
using Domain.Guest.ValueObjects;

namespace Domain.Entities
{
    public class Guest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public DocumentId DocumentId { get; set; }

        private void IsValid() 
        {
            if (DocumentId == null ||
                string.IsNullOrEmpty(DocumentId.IdNumber) ||
                DocumentId.IdNumber.Length <= 3 ||
                DocumentId.DocumentType == 0) 
            {
                throw new InvalidDocumentIdException();
            }

            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Surname) || string.IsNullOrEmpty(Email))
            {
                throw new MissingRequiredInformationException();
            }

            if (!Utils.ValidEmail(Email))
            {
                throw new InvalidEmailException();
            }
        }

        public async Task Save(IGuestRepository guestRepository)
        {
            this.IsValid();

            if (this.Id == 0)
            {
                this.Id = await guestRepository.Create(this);
            }
            else
            {
                //await guestRepository.Update(this);
            }
        }
    }
}
