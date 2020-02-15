using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace TimestampMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExerciseController : ControllerBase
    {
        private static UserStore _userStore = new UserStore();

        [HttpPost("new-user")]
        public IActionResult NewUser(UserStore.User user)
        {
            var newUser = _userStore.AddUser(user.Username);
            return Ok(newUser);
        }

        [HttpGet("users")]
        public IActionResult Users()
        {
            return Ok(_userStore.GetUsers());
        }

        [HttpPost("add")]
        public IActionResult AddExercise(UserStore.Exercise exercise)
        {
            _userStore.AddExercise(exercise);
            return Ok(exercise);
        }

        [HttpGet("log")]
        public IActionResult Search(string userId, DateTime? from = null, DateTime? to = null, int? limit = null)
        {
            return Ok(_userStore.Search(userId, from, to, limit));
        }
    }

    public class UserStore
    {

        private List<User> _users = new List<User>();

        public User AddUser(string userName)
        {
            var user = new User(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString(), userName);
            this._users.Add(user);
            return user;
        }

        public List<User> GetUsers()
        {
            return _users;
        }

        public void AddExercise(Exercise exercise)
        {
            var user = _users.Find(e => e.UserId == exercise.UserId);
            if (user == null) throw new Exception("User not found");

            if(!exercise.Date.HasValue)
            {
                exercise.Date = DateTime.Now;
            }

            exercise.Username = user.Username;

            user.Exercises.Add(exercise);
        }

        public User Search(string userId, DateTime? from = null, DateTime? to = null, int? limit= null)
        {
            var user = _users.Find(e => e.UserId == userId);

            var searchUser = new User { UserId = user.UserId, Username = user.Username };
            var query = user.Exercises.AsQueryable();

            if(from.HasValue)
            {
                query = query.Where(e => from.HasValue && e.Date >= from);
            }
            if (to.HasValue)
            {
                query = query.Where(e => to.HasValue && e.Date <= to);
            }

            searchUser.Exercises = query.Take(limit.HasValue ? limit.Value : user.Count).ToList();

            return searchUser;
        }

        public class User
        {
            public User()
            {

            }

            public User(string userId, string userName)
            {
                this.Username = userName;
                this.UserId = userId;
            }

            public string Username { get; set; }

            [JsonPropertyName("_id")]
            public string UserId { get; set; }

            [JsonPropertyName("log")]
            public List<Exercise> Exercises { get; set; } = new List<Exercise>();

            public int Count 
            {
                get
                {
                    return this.Exercises.Count;
                }
            }
        }

        public class Exercise
        {
            [JsonPropertyName("_id")]
            public string UserId { get; set; }

            public string Username { get; set; }

            public string Description { get; set; }

            public int Duration { get; set; }

            public DateTime? Date { get; set; }
        }

    }

}