using BlogApp.Core.Contracts;
using BlogApp.Core.Models.Identity;
using BlogApp.Core.Models.Report;
using BlogApp.Infrastructure.Data;
using BlogApp.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Core.Services
{
    public class AdminService : IAdminService
    {
        private readonly BlogAppDbContext _context;
        public AdminService(BlogAppDbContext context)
        {
            _context = context;
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
            var joinedUserRoles = _context.UserRoles
                            .Join(_context.Users,
                                userRole => userRole.UserId,
                                user => user.Id,
                                (userRole, user) => new { UserRole = userRole, User = user })
                            .Join(_context.Roles,
                                userRoleUser => userRoleUser.UserRole.RoleId,
                                role => role.Id,
                                (userRoleUser, role) => new { UserRoleUser = userRoleUser, Role = role })
                            .Select(x => new
                            {
                                UserId = x.UserRoleUser.User.Id,
                                RoleId = x.Role.Id,
                                RoleName = x.Role.Name
                            })
                            .ToList();

            //we take all users
            var users = await _context.ApplicationUsers.ToListAsync();

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
