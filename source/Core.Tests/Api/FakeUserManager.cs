﻿using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thinktecture.IdentityManager.Core;

namespace Core.Tests.Api
{
    public class FakeUserManager : Mock<IUserManager>
    {
        public FakeUserManager()
        {
            this.SetReturnsDefault(new UserManagerResult());
            this.SetReturnsDefault(new UserManagerResult<QueryResult>(new QueryResult()));
            this.SetReturnsDefault(new UserManagerResult<CreateResult>(new CreateResult()));
            this.SetReturnsDefault(new UserManagerResult<UserResult>(new UserResult()));
        }

        public void SetupQueryUsersAsync(QueryResult result)
        {
            Setup(x => x.QueryUsersAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(Task.FromResult(new UserManagerResult<QueryResult>(result)));
        }
        public void SetupQueryUsersAsync(params string[] errors)
        {
            Setup(x => x.QueryUsersAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(Task.FromResult(new UserManagerResult<QueryResult>(errors)));
        }
        public void SetupQueryUsersAsync(Exception ex)
        {
            Setup(x => x.QueryUsersAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .Throws(ex);
        }
        public void VerifyQueryUsersAsync()
        {
            Verify(x=>x.QueryUsersAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()));
        }
        public void VerifyQueryUsersAsync(string filter, int start, int count)
        {
            Verify(x=>x.QueryUsersAsync(filter, start, count));
        }

        public void VerifyCreateUserAsync(string username, string password, Times? times = null)
        {
            var t = times != null ? times.Value : Times.AtLeastOnce();
            Verify(x => x.CreateUserAsync(username, password), t);
        }
    }
}
