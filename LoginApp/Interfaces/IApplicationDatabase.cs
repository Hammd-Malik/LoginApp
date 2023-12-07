using LoginApp.Models;
using System.ComponentModel.DataAnnotations;

namespace LoginApp.Interfaces
{
    public interface IApplicationDatabase
    {

        public List<UserModel> CheckEmail(String Email);

        public void RegisterUser(UserModel User);

        public List<DevModel> GetDevList();
    }
}
