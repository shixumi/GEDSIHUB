namespace GedsiHub.Models
{
    public class CertificateFile
    {
        public int Id { get; set; }
        public int CertificateId { get; set; } // Foreign key to Certificate
        public byte[] PdfBytes { get; set; } // Store the actual PDF file
    }

}
