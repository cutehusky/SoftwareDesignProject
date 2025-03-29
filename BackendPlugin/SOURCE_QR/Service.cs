using QRCoder;

namespace SOURCE_QR
{
    public class Service
    {
        public string GenerateQrCode(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(value, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new PngByteQRCode(qrCodeData);
            byte[] qrCodeBytes = qrCode.GetGraphic(20);

            var base64 = Convert.ToBase64String(qrCodeBytes);
            return $"data:image/png;base64,{base64}";
        }
    }
}
