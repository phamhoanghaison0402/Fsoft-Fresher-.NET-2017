using IPM.Business;
using System.Collections.Generic;
using IPM.Model.Models;
using System;

namespace IPM.Service
{
    public interface IUserService
    {
       // bool Login(string a, string p);
        bool CheckLogin(string account, string password);
        string GetFullName(string account);
        bool Insert(User user, List<int> role);
        bool Edit(User user, List<int> role);
        List<InfoUser> GetUser();
        List<InfoUser> GetListUserByAccount(string searchString);
        bool CheckExistAccount(string account);
        bool CheckAccountFpt(string account);
        InfoUser GetUserByAccount(string account);
        int GetIDbyAccount(string account);


    }


    public class UserService : IUserService
    {
        private IUserBusiness _userBusiness;

        public UserService(IUserBusiness userBusiness)
        {
            _userBusiness = userBusiness;
        }


        //check account existed fpt
        public bool CheckAccountFpt(string account)
        {
           return _userBusiness.CheckAccountFpt(account);
        }


        //check account exist database 
        public bool CheckExistAccount(string account)
        {
           return _userBusiness.CheckExistAccount(account);
        }

        // check incorrect of account and pasword 
        public bool CheckLogin(string account, string password)
        {
            return _userBusiness.CheckLogin(account, password);

        }

        // Edit user
        public bool Edit(User user, List<int> role)
        {
            return _userBusiness.Edit(user, role);
        }


        // Get full name of account
        public string GetFullName(string account)
        {
            return _userBusiness.GetFullName(account);
        }

        public List<InfoUser> GetListUserByAccount(string searchString)
        {
            return _userBusiness.GetListUserByAccount(searchString);
        }


        //get all user
        public List<InfoUser> GetUser()
        {
            return _userBusiness.GetUser();
        }


        // get user by account
        public InfoUser GetUserByAccount(string account)
        {
            return _userBusiness.GetUserByAccount(account);
        }


        //Insert User 
        public bool Insert(User user, List<int> role)
        {
            return _userBusiness.Insert(user, role);
        }

        //Get id by account
        public int GetIDbyAccount(string account)
        {
            return _userBusiness.GetIDbyAccount(account);
        }
    }
}
