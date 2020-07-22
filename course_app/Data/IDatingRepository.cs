using course_app.Helpers;
using course_app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace course_app.Data
{
    public interface IDatingRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveAll();
        Task<PagedList<User>> GetUsers(int page, int pageSize);
        Task<PagedList<User>> GetUsers(UserParams userParams);
        Task<User> GetUserWithPhotos(int id);
        Task<User> GetUser(int id);
        Task<Photo> GetPhoto(int id);
        Task<User> Test(int id);
    }
}
