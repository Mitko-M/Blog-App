﻿using BlogApp.Core.Contracts;
using BlogApp.Core.Models.Contact;
using BlogApp.Core.Models.Identity;
using BlogApp.Core.Models.Report;
using BlogApp.Infrastructure.Data;
using BlogApp.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using static BlogApp.Infrastructure.Common.ValidationConstants;

namespace BlogApp.Core.Services
{
    public class AdminService : IAdminService
    {
        private readonly BlogAppDbContext _context;
        private readonly IUserService _userService;
        public AdminService(
            BlogAppDbContext context, 
            IUserService userService
            )
        {
            _context = context;
            _userService = userService;
        }

        public async Task Bann(string userName)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);

            if (user == null)
            {
                throw new ArgumentException($"User with username {userName} wasn't found");
            }

            user.Banned = true;

            int saves = await _context.SaveChangesAsync();

            if (saves == 0)
            {
                throw new Exception("Database changes to the change Bann property weren't saves");
            }
        }

        public async Task DeleteContactFormEntry(int id)
        {
            var contactForm = await _context.ContactFormEntries.FindAsync(id);

            if (contactForm == null)
            {
                throw new ArgumentException($"Contact form with id {id} doesn't exist.");
            }

            _context.ContactFormEntries.Remove(contactForm);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteReport(int id)
        {
            var report = await _context.PostsReports.FindAsync(id);

            if (report == null)
            {
                throw new ArgumentException($"Report with id {id} doesn't exist.");
            }

            _context.PostsReports.Remove(report);

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ApplicationUserViewModel>> GetAdminsAsync()
        {
            var users = await GetUsersOnRoleNameAsync("Admin");

            return users;
        }

        public async Task<IEnumerable<ContactAdminViewModel>> GetAllContactFormsAsync()
        {
            return await _context.ContactFormEntries
                .Select(c => new ContactAdminViewModel()
                {
                    Id = c.Id,
                    UserId = c.UserId,
                    UserName = c.User.UserName,
                    Name = c.Name,
                    Email = c.Email,
                    Subject = c.Subject,
                    Message = c.Message,
                    CreatedOn = c.CreatedOn.ToString(PostDateFormat)
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<PostReportsAdminViewModel>> GetAllReportsAsync()
        {
            var reports = await _context.PostsReports
                                    .Select(r => new PostReportsAdminViewModel()
                                    {
                                        Id = r.Id,
                                        PostId = r.PostId,
                                        UserId = r.UserId,
                                        ReporterUserName = r.User.UserName,
                                        PostTitle = r.Post.Title,
                                        ReportContent = r.ReportContent
                                    })
                                    .ToListAsync();

            return reports;
        }

        public async Task<IEnumerable<ApplicationUserViewModel>> GetAllUsersAsync()
        {
            var admins = await GetAdminsAsync();
            var users = await GetUsersOnRoleNameAsync("User");
            var allUsers = admins.Concat(users).ToList();

            return allUsers;
        }

        public async Task<ContactAdminViewModel> GetContactFormById(int id)
        {
            var contactForm = await _context.ContactFormEntries
                .Include(cf => cf.User)
                .FirstOrDefaultAsync(cf => cf.Id == id);

            if (contactForm == null)
            {
                return null;
            }

            var model = new ContactAdminViewModel()
            {
                Id = contactForm.Id,
                Name = contactForm.Name,
                UserName = contactForm.User.UserName,
                Email = contactForm.Email,
                Subject = contactForm.Subject,
                Message = contactForm.Message,
                CreatedOn = contactForm.CreatedOn.ToString(PostDateFormat)
            };

            return model;
        }

        public async Task<PostReportsAdminViewModel> GetReportById(int id)
        {
            var report = await _context.PostsReports
                                    .Include(pr => pr.Post)
                                    .Include(pr => pr.User)
                                    .FirstOrDefaultAsync(pr => pr.Id == id);

            if (report == null)
            {
                return null;
            }

            var model = new PostReportsAdminViewModel()
            {
                Id = report.Id,
                PostId = report.PostId,
                UserId = report.UserId,
                ReportContent = report.ReportContent,
                PostTitle = report.Post.Title,
                ReporterUserName = report.User.UserName,
            };

            return model;
        }

        public async Task<IEnumerable<ApplicationUserViewModel>> GetUsersAsync()
        {
            var users = await GetUsersOnRoleNameAsync("User");

            return users;
        }

        public async Task<IEnumerable<ApplicationUserViewModel>> GetUsersOnRoleNameAsync(string roleName)
        {
            //joining the AspNetUser with AspNetRoles and AspNetUserRoles
            //and then we take only the needed values
            var joinedUserRoles = await _context.UserRoles
                            .Join(_context.Users,
                                userRole => userRole.UserId,
                                user => user.Id,
                                (userRole, user) => new { UserRole = userRole, User = user })
                            .Join(_context.Roles,
                                userRoleUser => userRoleUser.UserRole.RoleId,
                                role => role.Id,
                                (userRoleUser, role) => new { UserRoleUser = userRoleUser, Role = role })
                            .Select(ur => new
                            {
                                UserId = ur.UserRoleUser.User.Id,
                                RoleId = ur.Role.Id,
                                RoleName = ur.Role.Name
                            })
                            .ToListAsync();

            //we take all users
            var users = await _context.Users.ToListAsync();

            //we take all roles
            var roles = await _context.Roles.ToArrayAsync();

            //we take all posts
            var posts = await _context.Posts.ToListAsync();

            //here we take the role Id by comparing the given role name
            string roleId = roles.FirstOrDefault(u => u.NormalizedName == roleName.ToUpper()).Id;


            //defining model
            var model = users.Select(u => new ApplicationUserViewModel()
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                UserName = u.UserName,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                PostCount = posts.Where(p => p.UserId == u.Id).Count(),
                Role = joinedUserRoles.Where(jt => jt.RoleId == roleId && jt.UserId == u.Id).Select(jt => jt.RoleName).FirstOrDefault()
            })
                .ToList();

            //taking the users only with the given role
            model = model.Where(u => u.Role == roleName).ToList();

            return model;
        }

        public async Task<ApplicationUserWithAllDataViewModel> ManageUserByUserName(string userName)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);

            if (user == null)
            {
                throw new ArgumentException("User wasn't found");
            }

            var userToReturn = await _userService.GetUserById(user.Id);

            if (userToReturn == null)
            {
                throw new ArgumentException("User wasn't found");
            }

            return userToReturn;
        }

        public async Task UnBann(string userName)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);

            if (user == null)
            {
                throw new ArgumentException($"User with username {userName} wasn't found");
            }

            user.Banned = false;

            int saves = await _context.SaveChangesAsync();

            if (saves == 0)
            {
                throw new Exception("Database changes to the change Bann property weren't saves");
            }
        }

        public async Task WarnApplicationUser(int reportId, int postId, string userId)
        {
            var report = await GetReportById(reportId);

            string reportReason = report.ReportContent;

            await DeleteReport(reportId);

            var post = await _context.Posts.FindAsync(postId);

            if (post == null)
            {
                throw new ArgumentException($"Post with id {postId} doesn't exist");
            }

            var user = await _context.Users.Include(u => u.Warnings).FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                throw new ArgumentException($"Application user with id {userId} doesn't exist");
            }

            post.Hidden = true;

            var warning = new Warning()
            {
                UserId = userId,
                HiddenPostId = postId,
                WarningReason = reportReason,
            };

            user.Warnings.Add(warning);

            if (user.Warnings.Count == 3)
            {
                user.Banned = true;
            }

            await _context.SaveChangesAsync();
        }
    }
}
