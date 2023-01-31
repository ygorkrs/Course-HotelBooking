using Domain.Enums;

namespace Domain.ValueObjects
{
    public class DocumentId
    {
        public string IdNumber { get; set; }
        public DocumentType DocumentType { get; set; }
    }
}
