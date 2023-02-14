using Domain.Guest.Enums;

namespace Domain.Guest.ValueObjects
{
    public class DocumentId
    {
        public string IdNumber { get; set; }
        public DocumentType DocumentType { get; set; }
    }
}
