using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SignPressServer.SignData
{
    class User
    {
        private String m_username;      //  用户名
        public String Username
        {
            get { return this.m_username; }
            set { this.m_username =  value; }
        }   
        
        
        private String m_password;      //  密码
        public String Password
        {
            get { return this.m_password; }
            set { this.m_password = value; }
        }
        public override String ToString()
        {
            return "USERNAME : " + this.m_username + "PASSWORD : " + this.m_password;
        }
    }
}
