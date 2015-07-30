using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignPressClient.Model
{
    //要先将User类先设为可以序列化(即在类的前面加[Serializable])
    [Serializable] 
    public class User
    {

        //public User(string username, string password)
        //{
        //    this.userName = username;
        //    this.passWord = password;
        //}

        private string userName;
        public string Username
        {
            get { return userName; }
            set { userName = value; }
        }

        private string passWord;
        public string Password
        {
            get { return passWord; }
            set { passWord = value; }
        }
    }
}
