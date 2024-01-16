namespace AuthModule.DTOs.Abstract
{
    public interface IUserDto
    {
        string Handle { get; set; }
        string Password { get; set; }
    }
}