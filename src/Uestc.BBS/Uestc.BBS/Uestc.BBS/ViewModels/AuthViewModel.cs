namespace Uestc.BBS.ViewModels
{
    public class AuthViewModel : ViewModelBase
    {
        public AuthViewModel() 
        {
#if ANDROID
Debug.WriteLine("aaa");
#endif
        }
    }
}
