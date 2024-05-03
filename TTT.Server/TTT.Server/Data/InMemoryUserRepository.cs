using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTT.Server.Data
{
    public class InMemoryUserRepository : IUserRepository {
        private readonly List<User> _entities;
        public InMemoryUserRepository()
        {
            _entities = new List<User>()
            {
                new User
                {
                    id  = "Hero1",
                    password = "1234",
                    IsOnline = true,
                    score = 10,
                },
                new User
                {
                    id  = "Hero2",
                    password = "234",
                    IsOnline = true,
                    score = 50,
                },
                new User
                {
                    id  = "Hero3",
                    password = "2345",
                    IsOnline = true,
                    score = 100,
                }
            };
        }
        public void Add(User entity) {
            _entities.Add(entity);
        }

        public void Delete(string id)
        {
            var entittyToDelet =  _entities.FirstOrDefault(x => x.id == id);
            _entities.Remove(entittyToDelet);
        }

        public User Get(string id)
        {
            return _entities.FirstOrDefault(x => x.id == id);
        }

        public IQueryable<User> GetQuerry()
        {
            return _entities.AsQueryable();
        }

        public ushort GetTotalCount()
        {
            return (ushort) _entities.Count(x => x.IsOnline);
        }

        public void SetOffline(string id)
        {
            _entities.FirstOrDefault(x => x.IsOnline = false);
        }

        public void SetOnline(string id)
        {
            _entities.FirstOrDefault(x => x.IsOnline = true);
        }

        public void Update(User entity)
        {
            var dbIndex = _entities.FindIndex(e => e.id == entity.id);
            _entities[dbIndex] = entity;
        }
    }
}
