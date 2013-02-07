using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections;

namespace Inbid.ViewModels
{


    //public class FreeUsersViewModel
    //{
    //    //Guid UserId { get; set; }
    //    //string UserName { get; set; }

    //    //public FreeUsersViewModel(Guid userId, string userName)
    //    //{
    //    //    UserId = userId;
    //    //    UserName = userName;
    //    //}
    //    public IEnumerable<SelectListItem> FreeUsers { get; set; }
        
    //}

    public class User
    {
        private Guid _userId;
        private string _userName;

        public Guid UserId 
        {
            get { return _userId; }
            set { _userId=value;} 
        }
        public string UserName 
        {
            get { return _userName;}
            set { _userName=value;} 
        }

        public User(Guid userId, string userName)
        {
            _userId = userId;
            _userName = userName;
        }
        //public IEnumerable<SelectListItem> FreeUsers { get; set; }

    }

    public class Users : IEnumerable
    {
        private ArrayList _users;

        public Users(List<User> users)
        {
            _users = new ArrayList();

            foreach (User u in users)
            {
                _users.Add(u);
            }
        }

        public IEnumerator GetEnumerator()
        {
            return (IEnumerator)new UsersEnumerator(this);
        }

        private class UsersEnumerator : IEnumerator
        {
            private Users _users;
            private int _index;

            public UsersEnumerator(Users usersList)
            {
                _users = usersList;
                _index = -1;
            }

            #region IEnumerator Members

            public void Reset()
            {
                _index = -1;
            }

            public object Current
            {
                get
                {
                    return _users._users[_index];
                }
            }

            public bool MoveNext()
            {
                _index++;
                if (_index >= _users._users.Count)
                    return false;
                else
                    return true;
            }

            #endregion
        }
    }

   
}