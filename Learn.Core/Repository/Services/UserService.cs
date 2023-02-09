using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Learn.Core.Convertors;
using Learn.Core.DTOs;
using Learn.Core.Genarator;
using Learn.Core.Security;
using Learn.Core.Services.Interfaces;
using Learn.DataLayer.Context;
using Learn.DataLayer.Entities.User;
using Learn.DataLayer.Entities.Wallet;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Learn.Core.Services
{
    public class UserService : IUserService
    {
        private LearnContext _context;
        public UserService(LearnContext context)
        {
            _context = context;
        }

        public bool ActiveAccount(string activeCode)
        {
            var user = _context.Users.SingleOrDefault(u => u.ActiveCode == activeCode);
            if (user == null || user.IsActive)
                return false;

            user.IsActive = true;
            user.ActiveCode = NameGenarator.GenarateUniqCode();
            _context.SaveChanges();
            return true;
        }

        public int AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return user.UserId;
        }

        public int AddUserFromAdmin(CreateUserViewModel user)
        {
            User user1 = new User()
            {
                Password = PasswordHelper.EncodePasswordMd5(user.Password),
                UserName = user.UserName,
                Email = user.Email,
                ActiveCode = NameGenarator.GenarateUniqCode(),
                IsActive = true,
                RegisterDate = DateTime.Now,
                Description = user.Description
            };

            #region Save Avatar
            if (user.UserAvatar != null)
            {
                string ImagePath = "";


                user1.UserAvatar = NameGenarator.GenarateUniqCode() + Path.GetExtension(user.UserAvatar.FileName);
                ImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar", user1.UserAvatar);
                using (var stream = new FileStream(ImagePath, FileMode.Create))
                {
                    user.UserAvatar.CopyTo(stream);
                }
            }
            #endregion

            return AddUser(user1);


        }

        public int AddWallet(Wallet wallet)
        {
            _context.Wallets.Add(wallet);
            _context.SaveChanges();
            return wallet.WalletId;
        }

        public int BalanceUserWallet(string userName)
        {
            var userId = GetUserIdByUserName(userName);
            var enter = _context.Wallets
                .Where(w => w.UserId == userId && w.TypeId == 1 && w.IsPay)
                .Select(w => w.Amount).ToList();

            var exit = _context.Wallets
                .Where(w => w.UserId == userId && w.TypeId == 2)
                .Select(w => w.Amount).ToList();

            return (enter.Sum() - exit.Sum());
        }

        public void ChangeUserPassword(string userName, string newPassword)
        {
            var user = GetUserByUserName(userName);
            user.Password = PasswordHelper.EncodePasswordMd5(newPassword);
            UpateUser(user);
        }

        public int ChargeWallet(string userName, string description, int amount, bool isPay)
        {
            Wallet wallet = new Wallet()
            {
                Amount = amount,
                Description = description,
                CreateDate = DateTime.Now,
                TypeId = 1,
                UserId = GetUserIdByUserName(userName)
            };

            return AddWallet(wallet);
        }

        public bool CompareOldPassword(string oldPassword, string userName)
        {
            string hashOldPassword = PasswordHelper.EncodePasswordMd5(oldPassword);
            return _context.Users.Any(u => u.UserName == userName && u.Password == hashOldPassword);
        }

        public void DeleteUser(int userId)
        {
            User user = GetUserByUserId(userId);
            user.IsDelete = true;
            UpateUser(user);

        }

        public void EditProfile(string userName, EditProfileViewModel profile)
        {
            if (profile.UserAvatar != null)
            {
                string ImagePath = "";

                if (profile.AvatarName != "Defult.jpg")
                {
                    ImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar", profile.AvatarName);
                    if (File.Exists(ImagePath))
                    {
                        File.Delete(ImagePath);
                    }
                }
                profile.AvatarName = NameGenarator.GenarateUniqCode() + Path.GetExtension(profile.UserAvatar.FileName);
                ImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar", profile.AvatarName);
                using (var stream = new FileStream(ImagePath, FileMode.Create))
                {
                    profile.UserAvatar.CopyTo(stream);
                }
            }
            var user = GetUserByUserName(userName);
            user.UserName = profile.UserName;
            user.Email = profile.Email;

            user.UserAvatar = profile.AvatarName;
            UpateUser(user);

        }

        public void EditUserFromAdmin(EditUserViewModel editUserViewModel)
        {
            User user = GetUserByUserId(editUserViewModel.UserId);
            user.Email = editUserViewModel.Email;
            if (!string.IsNullOrEmpty(editUserViewModel.Description))
            {
                user.Description = editUserViewModel.Description;
            }
            if (!string.IsNullOrEmpty(editUserViewModel.Password))
            {
                user.Password = PasswordHelper.EncodePasswordMd5(editUserViewModel.Password);
            }
            if (editUserViewModel.UserAvatar != null)
            {
                //Delete Old Image
                if (editUserViewModel.AvatarName != "Defult.jpg")
                {
                    string DeletePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar", editUserViewModel.AvatarName);
                    if (File.Exists(DeletePath))
                    {
                        File.Delete(DeletePath);
                    }

                }



                //Save New Image
                user.UserAvatar = NameGenarator.GenarateUniqCode() + Path.GetExtension(editUserViewModel.UserAvatar.FileName);
                string ImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar", user.UserAvatar);
                using (var stream = new FileStream(ImagePath, FileMode.Create))
                {
                    editUserViewModel.UserAvatar.CopyTo(stream);
                }
            }
            _context.Users.Update(user);
            _context.SaveChanges();

        }

        public List<SelectListItem> GetAllUserRoles()
        {
            return _context.Roles
             .Select(u => new SelectListItem()
             {
                 Text = u.RoleTitle,
                 Value = u.RoleId.ToString()
             }).ToList();
        }

        public EditProfileViewModel GetDataForEditProfileUser(string userName)
        {
            return _context.Users.Where(u => u.UserName == userName).Select(u => new EditProfileViewModel()
            {
                UserName = u.UserName,
                Email = u.Email,
                AvatarName = u.UserAvatar
            }).Single();
        }

        public UsersForAdminViewModel GetDeleteUsers(int pageId = 1, string filterEmail = "", string filterUserName = "")
        {
            IQueryable<User> result = _context.Users.IgnoreQueryFilters().Where(u => u.IsDelete);

            if (!string.IsNullOrEmpty(filterEmail))
            {
                result = result.Where(u => u.Email.Contains(filterEmail));
            }
            if (!string.IsNullOrEmpty(filterUserName))
            {
                result = result.Where(u => u.UserName.Contains(filterUserName));
            }
            //Show Item In Page
            int Take = 20;
            int Skip = (pageId - 1) * Take;

            UsersForAdminViewModel list = new UsersForAdminViewModel()
            {
                CurrentPage = pageId,
                PageCount = result.Count() / Take,
                users = result.OrderBy(u => u.RegisterDate).Skip(Skip).Take(Take).ToList()
            };

            return list;
        }

        public SideBarUserPanelViewModel GetSideBarUserPanelData(string userName)
        {
            return _context.Users.Where(u => u.UserName == userName).Select(u => new SideBarUserPanelViewModel()
            {
                UserName = u.UserName,
                ImageName = u.UserAvatar,
                RegisterDate = u.RegisterDate
            }).Single();
        }


        public User GetUserByActiveCode(string activeCode)
        {
            return _context.Users.SingleOrDefault(u => u.ActiveCode == activeCode);

        }

        public User GetUserByEmail(string email)
        {
            return _context.Users.SingleOrDefault(u => u.Email == email);
        }

        public User GetUserByUserId(int userId)
        {
            return _context.Users.Find(userId);
        }

        public User GetUserByUserName(string userName)
        {
            return _context.Users.SingleOrDefault(u => u.UserName == userName);
        }

        public EditUserViewModel GetUserForShowMode(int userId)
        {
            return _context.Users.Where(u => u.UserId == userId)
                 .Select(u => new EditUserViewModel()
                 {
                     UserId = userId,
                     AvatarName = u.UserAvatar,
                     Email = u.Email,
                     UserName = u.UserName,
                     UserRoles = u.UserRoles.Select(r => r.RoleId).ToList(),
                     Description = u.Description
                 }).Single();
        }

        public int GetUserIdByUserName(string userName)
        {
            return _context.Users.Single(u => u.UserName == userName).UserId;
        }

        public InformationUserViewModel GetUserInformation(string userName)
        {
            var user = GetUserByUserName(userName);
            InformationUserViewModel information = new InformationUserViewModel()
            {
                UserName = user.UserName,
                Email = user.Email,
                RegisterDate = user.RegisterDate,
                Wallet = BalanceUserWallet(userName)
            };
            return information;
        }

        public InformationUserViewModel GetUserInformation(int userId)
        {
            var user = GetUserByUserId(userId);
            InformationUserViewModel information = new InformationUserViewModel()
            {
                UserName = user.UserName,
                Email = user.Email,
                RegisterDate = user.RegisterDate,
                Wallet = BalanceUserWallet(user.UserName)
            };
            return information;
        }

        public UsersForAdminViewModel GetUsers(int? roleid, int pageId = 1, string filterEmail = "", string filterUserName = "")
        {
            IQueryable<User> result;
            if (roleid != null)
            {
                result = _context.UserRoles.Where(u => u.RoleId == roleid).Select(u => u.User);

            }
            else
            {
                result = _context.Users;

            }

            if (!string.IsNullOrEmpty(filterEmail))
            {
                result = result.Where(u => u.Email.Contains(filterEmail));
            }
            if (!string.IsNullOrEmpty(filterUserName))
            {
                result = result.Where(u => u.UserName.Contains(filterUserName));
            }
            //Show Item In Page
            int Take = 20;
            int Skip = (pageId - 1) * Take;

            UsersForAdminViewModel list = new UsersForAdminViewModel()
            {
                CurrentPage = pageId,
                PageCount = result.Count() / Take,
                users = result.OrderBy(u => u.RegisterDate).Skip(Skip).Take(Take).ToList()
            };

            return list;
        }

        public Wallet GetWalletByWalletId(int walletId)
        {
            return _context.Wallets.Find(walletId);
        }

        public List<WalletViewModel> GetWalletUser(string userName)
        {
            int userId = GetUserIdByUserName(userName);
            return _context.Wallets.Where(w => w.UserId == userId && w.IsPay)
                .Select(w => new WalletViewModel()
                {
                    Amount = w.Amount,
                    DateTime = w.CreateDate,
                    Description = w.Description,
                    Type = w.TypeId

                }).ToList();
        }

        public bool IsExistEmail(string Email)
        {
            return _context.Users.Any(u => u.Email == Email);

        }

        public bool IsExistUserName(string UserName)
        {
            return _context.Users.Any(u => u.UserName == UserName);
        }

        public User LoginUser(LoginViewModel login)
        {
            string HashPassword = PasswordHelper.EncodePasswordMd5(login.Password);
            string Email = FixedText.FixEmail(login.Email);
            return _context.Users.SingleOrDefault(u => u.Email == Email && u.Password == HashPassword);
        }

        public void UpateUser(User user)
        {
            _context.Update(user);
            _context.SaveChanges();
        }

        public void UpdateWallet(Wallet wallet)
        {
            _context.Wallets.Update(wallet);
            _context.SaveChanges();
        }
    }
}
