namespace Viq.AccessPoint.TestHarness.Services.Dtos
{
    public class UserDetailDto : UserDto
    { 
        public string CurrentPassWord { get; set; } 
        public object NewPassword { get; set; } 
        public object ConfirmPassword { get; set; } 
    }
}
