public interface IOtpRepository
{
    Task SaveOtpAsync(Otp otp);
    Task<Otp?> GetValidOtpAsync(string email, string code);
    Task MarkAsUsedAsync(Otp otp);
}