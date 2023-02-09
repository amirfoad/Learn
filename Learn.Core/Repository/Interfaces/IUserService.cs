using System;
using System.Collections.Generic;
using System.Text;
using Learn.Core.DTOs;
using Learn.DataLayer.Entities.User;
using Learn.DataLayer.Entities.Wallet;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Learn.Core.Services.Interfaces
{
    public interface IUserService
    {
        bool IsExistUserName(string UserName);
        bool IsExistEmail(string Email);
        int AddUser(User user);
        User LoginUser(LoginViewModel login);
        User GetUserByEmail(string email);
        User GetUserByActiveCode(string activeCode);
        User GetUserByUserName(string userName);
        User GetUserByUserId(int userId);
        void UpateUser(User user);
        bool ActiveAccount(string activeCode);
        int GetUserIdByUserName(string userName);
        void DeleteUser(int userId);
        #region UserPanel

        InformationUserViewModel GetUserInformation(string userName);
        InformationUserViewModel GetUserInformation(int userId);
        SideBarUserPanelViewModel GetSideBarUserPanelData(string userName);

        EditProfileViewModel GetDataForEditProfileUser(string userName);

        void EditProfile(string userName, EditProfileViewModel profile);
        bool CompareOldPassword(string oldPassword, string userName);
        void ChangeUserPassword(string userName, string newPassword);

        #endregion

        #region Wallet

        int BalanceUserWallet(string userName);
        List<WalletViewModel> GetWalletUser(string userName);

        int ChargeWallet(string userName, string description, int amount, bool isPay);
        int AddWallet(Wallet wallet);
        Wallet GetWalletByWalletId(int walletId);

        void UpdateWallet(Wallet wallet);
        #endregion

        #region AdminPanel

        UsersForAdminViewModel GetUsers(int? roleid,int pageId = 1, string filterEmail = "", string filterUserName = "");

        List<SelectListItem> GetAllUserRoles();

        int AddUserFromAdmin(CreateUserViewModel user);
        EditUserViewModel GetUserForShowMode(int userId);
        void EditUserFromAdmin(EditUserViewModel editUserViewModel);
        UsersForAdminViewModel GetDeleteUsers(int pageId = 1, string filterEmail = "", string filterUserName = "");
        
        #endregion
    }
}
