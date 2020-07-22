using course_app.Helpers;
using course_app.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace course_app.Data
{
    public class DatingRepository : IDatingRepository
    {
        private readonly DataContext _context;

        public DatingRepository(DataContext context)
        {
            _context = context;
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<User> GetUserWithPhotos(int id)
        {
            var users = await _context.Users.Include(u => u.Photos).FirstOrDefaultAsync(u => u.Id == id);

            return users;
        }

        public async Task<PagedList<User>> GetUsers(int page, int pageSize)
        {
            var users = _context.Users.Include(u => u.Photos);

            return await PagedList<User>.CreateAsync(users, page, pageSize);
        }

        public async Task<PagedList<User>> GetUsers(UserParams userParams)
        {
            var users = _context.Users.Include(u => u.Photos).OrderByDescending(u => u.LastActive).AsQueryable();

            users = users.Where(u => u.Id == userParams.UserId);

            users = users.Where(u => u.Gender == userParams.Gender);

            if (userParams.MinAge != 18 || userParams.MaxAge != 99)
            {
                var minDob = DateTime.UtcNow.AddYears(-userParams.MaxAge - 1);
                var maxDob = DateTime.UtcNow.AddYears(-userParams.MinAge);

                users = users.Where(u => u.DateOfBirth >= minDob && u.DateOfBirth <= maxDob);
            }

            if (!string.IsNullOrEmpty(userParams.OrderBy))
            {
                switch (userParams.OrderBy)
                {
                    case "created":
                        users = users.OrderByDescending(u => u.Created);
                        break;
                    default:
                        users = users.OrderByDescending(u => u.LastActive);
                        break;
                }
            }

            return await PagedList<User>.CreateAsync(users, userParams.PageNumber, userParams.PageSize);
        }

        public async Task<User> GetUser(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            return user;
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Photo> GetPhoto(int id)
        {
            var photo = await _context.Photos.FirstOrDefaultAsync(p => p.Id == id);

            return photo;
        }

        public async Task<User> Test(int id)
        {
            var user = await _context
                .Users
                .Select(u => new { user = u, mPhoto = u.Photos.Where(p => p.IsMain).Take(1) })
                .FirstOrDefaultAsync(u => u.user.Id == id);

            var mUser = user.user;
            mUser.Photos = user.mPhoto.ToList();

            return mUser;
        }
    }
}
